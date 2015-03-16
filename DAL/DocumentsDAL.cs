using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Entities;
using System.Text;

namespace DAL
{
    /// <summary>
    /// Class handles data access for DocumentsBLL
    /// </summary>
    public class DocumentsDAL
    {
        #region properties and variables

        /// <summary>
        /// class variable to store project ID
        /// </summary>
        public int ProjectID { get; set; }
        applicationDAL palinoiaDAL;
        BusinessRulesDAL brDAL;

        #endregion properties and variables

        #region constructors

        /// <summary>
        /// constructor for documentsDAL
        /// </summary>
        /// <param name="projectID">int</param>
        public DocumentsDAL(int projectID)
        {
            this.ProjectID = projectID;
            palinoiaDAL = new applicationDAL(projectID);
            brDAL = new BusinessRulesDAL(projectID);
        }

        #endregion constructors

        #region documents

        /// <summary>
        /// fetch all documents from the database
        /// </summary>
        /// <returns>List&lt;Document&gt;</returns>
        public List<Document> getAllDocuments()
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var entityDocuments = context.Documents
                                            .OrderBy((doc) => doc.Name);
                    List<Document> docList = entityDocuments.ToList<Document>();
                    return docList;
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.ProjectID, 0);
                }

                return new List<Document>();
            }
        }

        /// <summary>
        /// fetch all documents from the database
        /// </summary>
        /// <returns>Listl&lt;Document&gt;</returns>
        public List<viewDocument> getAllViewDocuments()
        {
            var viewDocumentList = new List<viewDocument>();
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var entityDocuments = context.Documents
                                            .OrderBy((doc) => doc.Name);
                    List<Document> docList = entityDocuments.ToList<Document>();
                    foreach (var entityDocument in docList)
                    {
                        viewDocument vdoc;
                        vdoc = new viewDocument((int)entityDocument.ID,
                                                (int)entityDocument.fk_DocumentType,
                                                entityDocument.Name,
                                                entityDocument.Description,
                                                entityDocument.Active,
                                                (int)entityDocument.UpdatedBy);
                        vdoc.setChapters(this.getChaptersForDocumentID(vdoc.ID));
                        viewDocumentList.Add(vdoc);
                    }
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.ProjectID, 0);
                }
                return viewDocumentList;
            }
        }

        /// <summary>
        /// fetch all active documents from the database
        /// </summary>
        /// <returns>List&lt;Document&gt;</returns>
        public List<Document> getAllActiveDocuments()
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var entityDocuments = context.Documents
                                            .Where((d) => d.Active == true)
                                            .OrderBy((doc) => doc.Name);
                    List<Document> docList = entityDocuments.ToList<Document>();
                    return docList;
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.ProjectID, 0);
                }

                return new List<Document>();
            }
        }

        /// <summary>
        /// fetch all documents by type id
        /// </summary>
        /// <param name="docTypeID">int</param>
        /// <returns>List&lt;Document&gt;</returns>
        public List<Document> getDocumentsByType(int docTypeID)
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var entityDocuments = context.Documents
                                            .Where ((dt) => dt.fk_DocumentType == docTypeID)
                                            .OrderBy((doc) => doc.Name);
                    List<Document> docList = entityDocuments.ToList<Document>();
                    return docList;
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.ProjectID, 0);
                }

                return new List<Document>();
            }
        }

        /// <summary>
        /// delete document by ID
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <returns>string</returns>
        public string deleteDocumentByID(int deleteID)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var deleteDocument = context.Documents
                                            .First((doc) => doc.ID == deleteID);
                    context.Documents.DeleteObject(deleteDocument);
                    context.SaveChanges();
                    //delete all document/chapter relationships
                    var documentRelationships = from dc in context.Document_Chapters
                                                where dc.fk_DocumentID == deleteID
                                                select dc;
                    foreach (var docRelationship in documentRelationships)
                    {
                        //delete related chapter
                        var deleteChapter = context.Chapters
                                            .First((c) => c.ID == docRelationship.fk_ChapterID);
                        context.Chapters.DeleteObject(deleteChapter);
                        //delete document chapter relationship
                        context.Document_Chapters.DeleteObject(docRelationship);
                    }
                    context.SaveChanges();
                    result = "OK";

                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, 0);

                }
            }
            return result;
        }

        /// <summary>
        /// fetch document by ID
        /// </summary>
        /// <param name="docID">int</param>
        /// <returns>viewDocument</returns>
        public viewDocument getDocumentByID(int docID)
        {
            viewDocument editDocument = new viewDocument(0,0,null,null,true,0);
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var entityEditDocument = context.Documents
                                            .FirstOrDefault((doc) => doc.ID == docID);
                    editDocument = new viewDocument((int)entityEditDocument.ID,
                                                    (int)entityEditDocument.fk_DocumentType,
                                                    entityEditDocument.Name,
                                                    entityEditDocument.Description,
                                                    entityEditDocument.Active,
                                                    (int)entityEditDocument.UpdatedBy);
                    editDocument.setChapters(this.getChaptersForDocumentID(editDocument.ID));
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.ProjectID, 0);
                }
            }
            return editDocument;
        }
                
        /// <summary>
        /// add new document to dbo.Documents.  Returns "OK" or error 
        /// </summary>
        /// <param name="doc">viewDocument</param>
        /// <returns>string</returns>
        public string addNewDocument(viewDocument doc)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    Document document = Document.CreateDocument(0, 
                                                                (long)doc.DocumentTypeID, 
                                                                doc.Name, 
                                                                (bool)doc.Active,
                                                                (int)doc.UpdatedBy);
                    document.Description = doc.Description;
                    context.Documents.AddObject(document);
                    context.SaveChanges();
                    result = document.ID.ToString();
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, doc.UpdatedBy);
                }
                return result;
            }
        }
                
        /// <summary>
        /// update Document record in the database
        /// </summary>
        /// <param name="doc">viewDocument</param>
        /// <returns>string</returns>
        public string updateDocument(viewDocument doc)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    Document document = context.Documents
                                        .First((d) => d.ID == doc.ID);
                    document.fk_DocumentType = doc.DocumentTypeID;
                    document.Name = doc.Name;
                    document.Description = doc.Description;
                    document.Active = doc.Active;
                    context.SaveChanges();
                    //update chapters
                    int sequence = 1;
                    foreach (var chapter in doc.getChapters())
                    {
                        sequence++;
                    }
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, doc.UpdatedBy);
                }
                return result;
            }
        }

        #endregion documents

        #region chapters

        /// <summary>
        /// save document-chapter relationships
        /// </summary>
        /// <param name="documentID">int</param>
        /// <param name="chapterID">int</param>
        /// <param name="seqNum">int</param>
        /// <returns>string</returns>
        public void saveDocumentChapterRelationship(int documentID, int chapterID, int seqNum)
        {
            
            using (var context = palinoiaDAL.getContextForProject())
            {
                //check if relationship already exists
                var record = context.Document_Chapters
                             .FirstOrDefault((dc) => dc.fk_DocumentID == documentID &&
                                                     dc.fk_ChapterID == chapterID);
                if (record == null)
                {
                    //if not, add new relationship
                    var newRelationship = Document_Chapters.CreateDocument_Chapters(0,
                                                                                    (long)seqNum,
                                                                                    (long)documentID,
                                                                                    (long)chapterID);
                    context.Document_Chapters.AddObject(newRelationship);
                    context.SaveChanges();
                }
                else
                {
                    //update sequence number
                    record.SeqNum = seqNum;
                    context.SaveChanges();
                }
            }
        }
        
        /// <summary>
        /// fetch test case ID for document ID
        /// </summary>
        /// <param name="documentID">int</param>
        /// <returns>int</returns>
        public int getTestCaseIDForDocumentID(int documentID)
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                //delete all document/chapter relationships for delete id
                var associatedTestCaseID = from tcd in context.TestCase_Documents
                                           where tcd.fk_DocumentID == documentID
                                           select (int)tcd.fk_TestCaseID;
                int tcID = Convert.ToInt32(associatedTestCaseID.FirstOrDefault());
                return tcID;
            }
        }

        /// <summary>
        /// fetches the next chapter sequence number by document id
        /// </summary>
        /// <param name="documentID">int</param>
        /// <returns>int</returns>
        public int getNextChapterSequenceNumber(int documentID)
        {
            int nextSeqNum = 1;

            //get all chapters associated with document
            using (var context = palinoiaDAL.getContextForProject())
            {
                //context.Persons.Max(p => p.Age);
                var SeqNum = from dc in context.Document_Chapters
                             where dc.fk_DocumentID == documentID
                             orderby dc.SeqNum descending
                             select (int)dc.SeqNum;
                if (SeqNum.Count() > 0)
                {
                    nextSeqNum = (int)SeqNum.First() + 1;
                }
            }
            return nextSeqNum;
        }

        /// <summary>
        /// fetch chapters for document ID
        /// </summary>
        /// <param name="documentID">int</param>
        /// <returns>List&lt;viewChapter&gt;</returns>
        public List<viewChapter> getChaptersForDocumentID(int documentID)
        {
            List<viewChapter> chapterList = new List<viewChapter>();
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var document_chapters = from docc in context.Document_Chapters
                                            where docc.fk_DocumentID == documentID
                                            orderby docc.SeqNum
                                            select docc;
                    foreach (var docChapter in document_chapters)
                    {
                        viewChapter chapter = getChapterByID((int)docChapter.fk_ChapterID);
                        chapter.SeqNum = (int)docChapter.SeqNum;
                        chapterList.Add(chapter);
                    }
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.ProjectID, 0);
                }
            }


            return chapterList;
        }

        /// <summary>
        /// fetch chapter by ID
        /// </summary>
        /// <param name="chapterID">int</param>
        /// <returns>viewChapter</returns>
        public viewChapter getChapterByID(int chapterID)
        {
            viewChapter editChapter = new viewChapter();
            if (chapterID > 0)
            {
                using (var context = palinoiaDAL.getContextForProject())
                {
                    try
                    {
                        var entityEditChapter = context.Chapters
                                                .FirstOrDefault((chp) => chp.ID == chapterID);
                        editChapter.ID = (int)entityEditChapter.ID;
                        editChapter.Title = entityEditChapter.Title;
                        editChapter.Text = entityEditChapter.Text;
                        editChapter.TypeID = (int)entityEditChapter.fk_ChapterTypeID;
                    }
                    catch (Exception ex)
                    {
                        palinoiaDAL.logError(ex, this.ProjectID, 0);
                    }
                }
            }
            return editChapter;
        }

        /// <summary>
        /// add new chapter
        /// </summary>
        /// <param name="documentID">int</param>
        /// <param name="newChapter">viewChapter</param>
        /// <returns>string</returns>
        public string addNewChapter(int documentID, viewChapter newChapter)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    Chapter chapter = Chapter.CreateChapter(0, 
                                                            (long)newChapter.TypeID, 
                                                            newChapter.Title, 
                                                            (long)newChapter.UpdatedBy, 
                                                            newChapter.Active);
                    chapter.Text = newChapter.Text;
                    context.Chapters.AddObject(chapter);
                    context.SaveChanges();
                    result = chapter.ID.ToString();
                    //associate new chapter with document
                    int seqNum = getNextChapterSequenceNumber(documentID);
                    saveDocumentChapterRelationship(documentID, (int)chapter.ID, seqNum);
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, newChapter.UpdatedBy);
                }
                return result;
            }
        }

        /// <summary>
        /// update chapter
        /// </summary>
        /// <param name="editChapter">viewChapter</param>
        /// <returns>string</returns>
        public string updateChapter(viewChapter editChapter)
        {
            string result = "";
            try
            {
                using (var context = palinoiaDAL.getContextForProject())
                {
                    var chapter = context.Chapters
                                  .First((c) => c.ID == (long)editChapter.ID);
                    chapter.Title = editChapter.Title;
                    chapter.Text = editChapter.Text;
                    chapter.UpdatedBy = editChapter.UpdatedBy;
                    context.SaveChanges();
                    result = "OK";
                }
            }
            catch (Exception ex)
            {
                result = palinoiaDAL.logError(ex, this.ProjectID, editChapter.UpdatedBy);
            }

            return result;
        }

        /// <summary>
        /// delete chapter by ID
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteChapterByID(int deleteID, int userID)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var deleteChapter = context.Chapters
                                            .First((c) => c.ID == deleteID);
                    context.Chapters.DeleteObject(deleteChapter);
                    //delete all document/chapter relationships for delete id
                    var documentRelationships = from dc in context.Document_Chapters
                                                where dc.fk_ChapterID == deleteID
                                                select dc;
                    foreach (var docRelationship in documentRelationships)
                    {
                        //delete document chapter relationship
                        context.Document_Chapters.DeleteObject(docRelationship);
                    }
                    context.SaveChanges();
                    result = "OK";

                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, userID);
                }
            }
            return result;
        }

        /// <summary>
        /// fetch document ID for chapter
        /// </summary>
        /// <param name="chapterID">int</param>
        /// <returns>int</returns>
        public int getDocumentIDForChapter(int chapterID)
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                //fetch the document id the chapter is associated with
                var associatedDocumentID = from d in context.Document_Chapters
                                           where d.fk_ChapterID == chapterID
                                           select (int)d.fk_DocumentID;
                int docID = Convert.ToInt32(associatedDocumentID.FirstOrDefault());
                return docID;
            }
        }
        
        #endregion chapters

        #region images

        //public bool isImageInDatabase(string fileName)
        //{
        //    using (var context = palinoiaDAL.getContextForProject())
        //    {
        //        bool imageInDatabase = false;
        //        try
        //        {
        //            //is image record already in database?
        //            var entityDocumentImage = context.DocumentImages
        //                                    .FirstOrDefault((di) => di.FileName.Equals(iData.FileName));
        //            if (entityDocumentImage != null)
        //            {
        //                imageInDatabase = true;
        //                //return id of existing record
        //                result = entityDocumentImage.ID.ToString();
        //            }
        //}

        /// <summary>
        /// add an image
        /// </summary>
        /// <param name="iData">ImageData</param>
        /// <returns>string</returns>
        public string addImage(ImageData iData)
        {
            var result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    //is image record already in database?
                    bool imageInDatabase = false;
                    var entityDocumentImage = context.DocumentImages
                                            .FirstOrDefault((di) => di.FileName.Equals(iData.FileName));
                    if (entityDocumentImage != null)
                    {
                        imageInDatabase = true;
                        //return id of existing record
                        result = entityDocumentImage.ID.ToString();
                    }
                    //is image already stored in documentimages?
                    bool imageInFolder = false;
                    var imagePath = new StringBuilder();
                    if (File.Exists(iData.FilePath + "/" + iData.FileName))
                    {
                        imageInFolder = true;
                    }
                    if (!imageInDatabase)
                    {
                        //create new db record and save image to documentimages
                        DocumentImage di = DocumentImage.CreateDocumentImage(iData.ID, iData.FileName, iData.UpdatedBy);
                        di.Description = iData.Description;
                        context.DocumentImages.AddObject(di);
                        context.SaveChanges();
                        result = di.ID.ToString();
                    }
                    if (!imageInFolder)
                    {
                        //save image file to DocumentImages folder
                        string file = iData.FilePath + iData.FileName;
                        File.WriteAllBytes(file, iData.ByteArray);
                    }
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, iData.UpdatedBy);
                }
            }
            return result;
        }

        /// <summary>
        /// fetch image file name by ID.  retures default image not
        /// found graphic(ID = 1) if no image located.
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>string</returns>
        public string getImageFileNameByID(int id)
        {
            int defaultImageNotFoundID = 1;
            string filename = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var entityDocumentImage = context.DocumentImages
                                            .FirstOrDefault((di) => di.ID == id);
                    filename = entityDocumentImage.FileName;
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.ProjectID, 0);
                }
            }
            if (filename == "")
            {
                //no image was found
                //return Image Not Found graphic
                filename = getImageFileNameByID(defaultImageNotFoundID);
            }
            return filename;    
        }

        /// <summary>
        /// if image filename is found in the database,
        /// returnn the id of that image.
        /// if no image is found, return the next pk sequence
        /// number that will be assigned when image is saved.
        /// </summary>
        /// <param name="dbDataSource">string</param>
        /// <param name="fileName">string</param>
        /// <returns>int</returns>
        public int getNextImageID(string dbDataSource, string fileName)
        {
            int nextImageID = 0;
            //check if image is in database
            using (var context = palinoiaDAL.getContextForProject())
            {
                bool imageInDatabase = false;
                try
                {
                    //is image record already in database?
                    var entityDocumentImage = context.DocumentImages
                                            .FirstOrDefault((di) => di.FileName.Equals(fileName));
                    if (entityDocumentImage != null)
                    {
                        imageInDatabase = true;
                        //return id of existing record
                        nextImageID = (int)entityDocumentImage.ID;
                    }
                }
                catch (Exception ex)
                {

                }
            }
            if (nextImageID == 0)
            {
                //get next id to be assigned when image is saved
                SQLiteConnection con;
                SQLiteCommand cmd;
                SQLiteDataReader reader;
                string projectName = palinoiaDAL.getProjectByID(this.ProjectID).NAME;
                con = new SQLiteConnection("Data Source=" + dbDataSource + "\\" + projectName + ".s3db");
                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandText = "SELECT seq FROM SQLITE_SEQUENCE WHERE name='DocumentImages'";
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    nextImageID = Convert.ToInt32(reader[0]) + 1;
                }
            }
            return nextImageID;
        }

        #endregion images

    }
}
