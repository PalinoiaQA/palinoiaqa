using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    /// <summary>
    /// class to hold code for viewDocument object
    /// </summary>
    public class viewDocument
    {
        /// <summary>
        /// class variable to store document ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable to store document type ID
        /// </summary>
        public int DocumentTypeID { get; set; }
        /// <summary>
        /// class variable to store document name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// class variable to store document description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// class variable to store document active status
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// class variable to store list of documents
        /// </summary>
        private List<viewChapter> ChapterList = new List<viewChapter>();
        /// <summary>
        /// class variable to store key to person updating document
        /// </summary>
        public int UpdatedBy { get; set; }

        #region constructors
        /// <summary>
        /// constructor with five parameters
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="documentTypeID">int</param>
        /// <param name="name">string</param>
        /// <param name="description">string</param>
        /// <param name="active">bool</param>
        /// <param name="updatedBy">int</param>
        public viewDocument(int id, int documentTypeID, string name, string description, bool active, int updatedBy)
        {
            this.ID = id;
            this.DocumentTypeID = documentTypeID;
            this.Name = name;
            this.Description = description;
            this.Active = active;
            this.UpdatedBy = updatedBy;
        }

        /// <summary>
        /// constructor for viewDocument object
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="documentTypeID">int</param>
        /// <param name="name">string</param>
        /// <param name="description">string</param>
        /// <param name="active">bool</param>
        /// <param name="chapterList">List</param>
        /// <param name="updatedBy">int</param>
        public viewDocument(int id, int documentTypeID, string name, string description, bool active, List<viewChapter> chapterList, int updatedBy)
        {
            this.ID = id;
            this.DocumentTypeID = documentTypeID;
            this.Name = name;
            this.Description = description;
            this.Active = active;
            this.ChapterList = chapterList;
            this.UpdatedBy = updatedBy;
        }
        #endregion constructors

        /// <summary>
        /// fetch all document text
        /// </summary>
        /// <returns>string</returns>
        public string getDocumentHTML()
        {
            StringBuilder html = new StringBuilder();
            //loop though all chapter and get text
            foreach (var chapter in this.ChapterList)
            {
                html.Append(chapter.Text);
            }

            return html.ToString();
        }

        #region private methods

        /// <summary>
        /// add a chapter
        /// </summary>
        /// <param name="chapter">viewChapter</param>
        public void addChapter(viewChapter chapter)
        {
            this.ChapterList.Add(chapter);
        }

        /// <summary>
        /// remove a chapter
        /// </summary>
        /// <param name="chapterID">int</param>
        public void removeChapter(int chapterID)
        {
            var deleteChapter = this.ChapterList.Find((c) => c.ID == chapterID);
            this.ChapterList.Remove(deleteChapter);
        }

        /// <summary>
        /// remove a chapter
        /// </summary>
        /// <param name="chapter">viewChapter</param>
        public void removeChapter(viewChapter chapter)
        {
            this.ChapterList.Remove(chapter);
        }

        /// <summary>
        /// fetch a chapter
        /// </summary>
        /// <returns>List&lt;viewChapter&gt;</returns>
        public List<viewChapter> getChapters()
        {
            return this.ChapterList;
        }

        /// <summary>
        /// set chapters
        /// </summary>
        /// <param name="chapList">List&lt;viewChapter&gt;</param>
        public void setChapters(List<viewChapter> chapList)
        {
            this.ChapterList = chapList;
        }

        #endregion private methods
    }

    /// <summary>
    /// Data object to transport docuemnt detail information to client javascript
    /// </summary>
    [Serializable]
    public class EditDocumentDetails
    {
        /// <summary>
        /// class variable to store value of document ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// class variable to store value of document title
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// class variable to store value of document type
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// class variable to store value of document type ID
        /// </summary>
        public string typeID { get; set; }
        /// <summary>
        /// class variable to store value of document description
        /// </summary>
        public string desc { get; set; }

        /// <summary>
        /// empty constructor for EditDocumentDetails
        /// </summary>
        public EditDocumentDetails()
        {

        }
    }

    /// <summary>
    /// class to hold code for DocumentSearchResult
    /// </summary>
    [Serializable]
    public class DocumentSearchResult
    {
        /// <summary>
        /// class variable to store list of document IDs
        /// </summary>
        public List<string> docID { get; set; }
        /// <summary>
        /// class variable to store list of chapter IDs
        /// </summary>
        public List<string> chapID { get; set; }

        /// <summary>
        /// empty constructor for DocumentSearchResult
        /// </summary>
        public DocumentSearchResult()
        {

        }

        /// <summary>
        /// constructor for DocumentSearchResult with two parameters
        /// </summary>
        /// <param name="did">List&lt;string&gt;</param>
        /// <param name="cid">List&lt;string&gt;</param>
        public DocumentSearchResult(List<string> did, List<string> cid)
        {
            this.docID = did;
            this.chapID = cid;
        }
    }

    //public class DocumentChapter
    //{
    //    public int docID { get; set; }
    //    public int chapID { get; set; }

    //    public DocumentChapter(int did, int cid)
    //    {
    //        this.docID = did;
    //        this.chapID = cid;
    //    }
    //}

}
