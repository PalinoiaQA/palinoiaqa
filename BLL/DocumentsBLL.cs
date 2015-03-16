using System.Collections.Generic;
using System.Text;
using DAL;
using Entities;
using Enums;
using System.Text.RegularExpressions;

namespace BLL
{
    /// <summary>
    /// Class that contains all business logic related to Documents and
    /// provides all accesss to the DAL (Data Access Layer)
    /// </summary>
    public class DocumentsBLL
    {
        #region properties and variables

        DocumentsDAL dal;
        BusinessRulesBLL brBLL;
        CustomerServiceMessagesBLL csmBLL;
        TestCasesBLL tcBLL;
        AdminBLL adminBLL;
        applicationBLL appBLL;

        /// <summary>
        /// class variable to store project ID
        /// </summary>
        public int ProjectID { get; set; }

        #endregion properties and variables

        #region constructors

        /// <summary>
        /// constructor for document BLL
        /// </summary>
        /// <param name="projectID">int</param>
        public DocumentsBLL(int projectID)
        {
            if (projectID == 0)
            {
                projectID = 1;
            }
            this.ProjectID = projectID;
            dal = new DocumentsDAL(projectID);
            brBLL = new BusinessRulesBLL(projectID);
            csmBLL = new CustomerServiceMessagesBLL(projectID);
            adminBLL = new AdminBLL(projectID);
            appBLL = new applicationBLL();
            tcBLL = new TestCasesBLL(projectID);
        }

        #endregion constructors

        #region documents

        /// <summary>
        /// fetch all documents from the database
        /// </summary>
        /// <returns></returns>
        public List<viewDocument> getAllDocuments()
        {
            List<viewDocument> docList = new List<viewDocument>();
            var entityDocList = dal.getAllDocuments();
            foreach (var entityDoc in entityDocList)
            {
                docList.Add(new viewDocument((int)entityDoc.ID,
                                             (int)entityDoc.fk_DocumentType,
                                             entityDoc.Name,
                                             entityDoc.Description,
                                             entityDoc.Active,
                                             (int)entityDoc.UpdatedBy));
            }
            entityDocList = null;
            return docList;
        }

        /// <summary>
        /// fetch all active documents from the database
        /// </summary>
        /// <returns>List&lt;viewDocument&gt;</returns>
        public List<viewDocument> getAllActiveDocuments()
        {
            List<viewDocument> docList = new List<viewDocument>();
            var entityDocList = dal.getAllActiveDocuments();
            foreach (var entityDoc in entityDocList)
            {
                docList.Add(new viewDocument((int)entityDoc.ID,
                                             (int)entityDoc.fk_DocumentType,
                                             entityDoc.Name,
                                             entityDoc.Description,
                                             entityDoc.Active,
                                             (int)entityDoc.UpdatedBy));
            }
            entityDocList = null;
            return docList;
        }

        /// <summary>
        /// pass through method to fetch documents by ID
        /// </summary>
        /// <param name="docID">int</param>
        /// <returns>viewDocument</returns>
        public viewDocument getDocumentByID(int docID)
        {
            return dal.getDocumentByID(docID);
        }

        /// <summary>
        /// fetch all documents from the database of a specific type
        /// </summary>
        /// <returns></returns>
        public List<viewDocument> getDocumentsByType(int docTypeID)
        {
            List<viewDocument> docList = new List<viewDocument>();
            var entityDocList = dal.getDocumentsByType(docTypeID);
            foreach (var entityDoc in entityDocList)
            {
                docList.Add(new viewDocument((int)entityDoc.ID,
                                             (int)entityDoc.fk_DocumentType,
                                             entityDoc.Name,
                                             entityDoc.Description,
                                             entityDoc.Active,
                                             (int)entityDoc.UpdatedBy));
            }
            entityDocList = null;
            return docList;
        }

        /// <summary>
        /// pass through method to delete document by ID
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <returns>string</returns>
        public string deleteDocumentByID(int deleteID)
        {
            return dal.deleteDocumentByID(deleteID);
        }

        /// <summary>
        /// pass through method to add new document
        /// </summary>
        /// <param name="doc">viewDocument</param>
        /// <returns>string</returns>
        public string addNewDocument(viewDocument doc)
        {
            string result = dal.addNewDocument(doc);
            return result;
        }

        /// <summary>
        /// pass through method to update document
        /// </summary>
        /// <param name="doc">viewDocument</param>
        /// <returns>string</returns>
        public string updateDocument(viewDocument doc)
        {
            string result = dal.updateDocument(doc);
            return result;
        }

        /// <summary>
        /// pass through method to save document-chapter relationship
        /// </summary>
        /// <param name="documentID">int</param>
        /// <param name="chapterID">int</param>
        /// <param name="seqNum">int</param>
        /// <returns>string</returns>
        public void saveDocumentChapterRelationship(int documentID, int chapterID, int seqNum)
        {
            dal.saveDocumentChapterRelationship(documentID, chapterID, seqNum);
        }

        /// <summary>
        /// returns test case id for associated document id from
        /// TestCase_Documents table
        /// </summary>
        /// <param name="documentID">int</param>
        /// <returns>int</returns>
        public int getTestCaseIDForDocumentID(int documentID)
        {
            return dal.getTestCaseIDForDocumentID(documentID);
        }

        /// <summary>
        /// return data object created to be utilized by the client javascript
        /// for editing document details in the document manager
        /// </summary>
        /// <param name="docID">int</param>
        /// <returns>EditDocumentDetails</returns>
        public EditDocumentDetails getEditDetailsForDocument(int docID)
        {
            //var docBLL = new DocumentsBLL(this.ProjectID);
            viewDocument doc = dal.getDocumentByID(docID);
            EditDocumentDetails details = new EditDocumentDetails();
            details.id = docID.ToString();
            details.title = doc.Name;
            details.type = adminBLL.getDocumentTypeByID(doc.DocumentTypeID).Text;
            details.typeID = doc.DocumentTypeID.ToString();
            details.desc = doc.Description;
            return details;
        }

        /// <summary>
        /// document has chapters
        /// </summary>
        /// <param name="documentID">int</param>
        /// <returns>bool</returns>
        public bool documentHasChapters(int documentID)
        {
            return dal.getChaptersForDocumentID(documentID).Count > 0;
        }

        /// <summary>
        /// update sequence numbers for all chapters in a document in order
        /// contained in chapterIDList
        /// </summary>
        /// <param name="documentID">int</param>
        /// <param name="chapterIDList">List&lt;int&gt;</param>
        public void reorderDocumentChapters(int documentID, List<int> chapterIDList)
        {
            int seqNum = 0;
            foreach (var id in chapterIDList)
            {
                this.updateChapterSequence(documentID, id, seqNum);
                seqNum++;
            }
        }

        #endregion documents

        #region chapters

        /// <summary>
        /// pass through method to fetch chapters for document ID
        /// </summary>
        /// <param name="documentID">int</param>
        /// <returns>List&lt;viewChapter&gt;</returns>
        public List<viewChapter> getChaptersForDocumentID(int documentID)
        {
            return dal.getChaptersForDocumentID(documentID);
        }

        /// <summary>
        /// pass through method to fetch chapters by ID
        /// </summary>
        /// <param name="chapterID">int</param>
        /// <returns>viewChapter</returns>
        public viewChapter getChapterByID(int chapterID)
        {
            return dal.getChapterByID(chapterID);
        }

        /// <summary>
        /// pass through method to add new chapter
        /// </summary>
        /// <param name="documentID">int</param>
        /// <param name="newChapter">viewChapter</param>
        /// <returns>string</returns>
        public string addNewChapter(int documentID, viewChapter newChapter)
        {
            return dal.addNewChapter(documentID, newChapter);
        }

        /// <summary>
        /// pass through method to update chapter
        /// </summary>
        /// <param name="editChapter">viewChapter</param>
        /// <returns>string</returns>
        public string updateChapter(viewChapter editChapter)
        {
            string result = "";
            result = dal.updateChapter(editChapter);
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
            return dal.deleteChapterByID(deleteID, userID);
        }

        /// <summary>
        /// call to dal saveDocumentChapterRelationship.  Should only be call via
        /// the BLL for reordering chapters based on DocumentEdit UI
        /// </summary>
        /// <param name="documentID">int</param>
        /// <param name="chapterID">int</param>
        /// <param name="sequenceNumber">int</param>
        public void updateChapterSequence(int documentID, int chapterID, int sequenceNumber)
        {
            dal.saveDocumentChapterRelationship(documentID, chapterID, sequenceNumber);
        }

        /// <summary>
        /// fetch document ID for chapter
        /// </summary>
        /// <param name="chapterID">int</param>
        /// <returns>int</returns>
        public int getDocumentIDForChapter(int chapterID)
        {
            return dal.getDocumentIDForChapter(chapterID);
        }

        #endregion chapters

        #region images

        /// <summary>
        /// add an image 
        /// </summary>
        /// <param name="iData">ImageData</param>
        /// <returns>string</returns>
        public string addImage(ImageData iData)
        {
            return dal.addImage(iData);
        }

        /// <summary>
        /// fetch image file by ID
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>string</returns>
        public string getImageFileNameByID(int id)
        {
            return dal.getImageFileNameByID(id);
        }

        /// <summary>
        /// fetch next image ID
        /// </summary>
        /// <param name="dbDataSource">string</param>
        /// <param name="fileName">string</param>
        /// <returns>int</returns>
        public int getNextImageID(string dbDataSource, string fileName)
        {
            return dal.getNextImageID(dbDataSource, fileName);
        }

        #endregion images

        #region utilities

        /// <summary>
        /// scans input text for business rules and csm names and
        /// creates HTML summary table
        /// </summary>
        /// <param name="text"></param>
        /// <returns>string</returns>
        public string createSummaryFromText(string text, bool showStatus)
        {
            string summaryHTML = "";
            viewChapter chapter = new viewChapter();
            chapter.Text = text;
            SortedDictionary<string, string> chapterRules = getChapterRules(chapter);
            StringBuilder sb = new StringBuilder();
            sb.Append(chapter.Text);
            sb.AppendLine();
            if (chapterRules.Count > 0)
            {
                summaryHTML = getChapterSummaryHTML(chapterRules, showStatus);
            }
            return summaryHTML;
        }

        /// <summary>
        /// generates and returns the entire html source for the chapter summary
        /// </summary>
        /// <param name="chapterRules">SortedDictionary&lt;string, string&gt;</param>
        /// <returns></returns>
        private string getChapterSummaryHTML(SortedDictionary<string, string> chapterRules, bool showStatus)
        {
            StringBuilder html = new StringBuilder();
            html.Append("<div id=\"BRCSMSummary\">");
            html.Append("<p align=\"center\"><b>Business Rule and CSM Summary</b></p>");
            html.Append("<br />");
            html.Append("<table border=\"1\" width=\"95%\">");
            foreach (var item in chapterRules)
            {
                if (item.Key.Contains("BR.")) //Business Rule?
                {
                    int brID = 0;
                    bool parseResult = int.TryParse(item.Value, out brID);
                    if (parseResult)
                    {
                        viewBusinessRule rule = brBLL.getBusinessRuleByID(brID);
                        html.Append(buildRow(rule, showStatus));
                    }
                }
                else // CSM
                {
                    int csmID = 0;
                    bool parseResult = int.TryParse(item.Value, out csmID);
                    if (parseResult)
                    {
                        viewCustomerServiceMessage csm = csmBLL.getCSMByID(csmID);
                        html.Append(buildRow(csm));
                    }
                }

            }
            html.Append("</table>");
            html.Append("</div>");
            return html.ToString();
        }

        /// <summary>
        /// generate individual row in the chapter summary table for a 
        /// specific business rule
        /// </summary>
        /// <param name="br">viewBusinessRule</param>
        /// <returns>string</returns>
        private string buildRow(viewBusinessRule br, bool showStatus)
        {
            var vStatus = adminBLL.getStatusByID(br.StatusID);
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr>"); 
            sb.Append("<td bgcolor=\"");
            if (showStatus)
            {
                sb.Append(vStatus.Color);
            }
            else
            {
                sb.Append("#FFFFFF"); // no highlight. color = white
            }
            sb.Append("\" valign=\"top\" width=\"20%\">");
            sb.Append("<p>");
            sb.Append(br.Name.Trim());
            sb.Append("</p>");
            sb.Append("</td>");
            sb.Append("<td valign=\"top\" bgcolor=\"");
            if (showStatus)
            {
                sb.Append(vStatus.Color);
            }
            else
            {
                sb.Append("#FFFFFF"); // no highlight. color = white
            }
            sb.Append("\">");
            if (showStatus)
            {
                if (vStatus.DisplayInChapterSummary)
                {
                    sb.Append("<b>");
                    sb.Append("** ");
                    sb.Append(vStatus.Text);
                    sb.Append(" ** ");
                    sb.Append("</b>");
                }
            }
            sb.Append(br.Text.Trim());
            sb.Append("</td>");
            sb.Append("</tr>");
            return sb.ToString();
        }

        /// <summary>
        /// generate individual row in the chapter summary table for a 
        /// specific customer service message
        /// </summary>
        /// <param name="csm">viewCustomerServiceMessage</param>
        /// <returns>string</returns>
        private string buildRow(viewCustomerServiceMessage csm)
        {
            var vStatus = adminBLL.getStatusByID(csm.StatusID);
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr>");
            sb.Append("<td valign=\"top\" width=\"20%\" bgcolor=\"");
            sb.Append(vStatus.Color);
            sb.Append("\">");
            sb.Append("<p>");
            sb.Append(csm.Name.Trim());
            sb.Append("</p>");
            sb.Append("</td>");
            sb.Append("<td valign=\"top\" bgcolor=\"");
            sb.Append(vStatus.Color);
            sb.Append("\">");
            if (vStatus.DisplayInChapterSummary)
            {
                sb.Append("<b>");
                sb.Append("** ");
                sb.Append(vStatus.Text);
                sb.Append(" ** ");
                sb.Append("</b>");
            }
            sb.Append(csm.Text.Trim());
            sb.Append("</td>");
            sb.Append("</tr>");
            return sb.ToString();
        }

        /// <summary>
        /// return sorted dictionary object containing all the business rules(name and text) and
        /// customer service messages (name and text) references in a specific chapter
        /// </summary>
        /// <param name="chapter">viewChapter</param>
        /// <returns>SortedDictionary&lt;string, string&gt;</returns>
        private SortedDictionary<string, string> getChapterRules(viewChapter chapter)
        {
            var brList = brBLL.getAllBusinessRules();
            var csmList = csmBLL.getAllCSMs();
            SortedDictionary<string, string> chapterRules = new SortedDictionary<string, string>();
            //spin through all business rules and add to chapterRules if name
            //contained in text
            foreach (var br in brList)
            {
                if (chapter.Text.Contains(br.Name + " ") ||
                    chapter.Text.Contains(br.Name + ")") ||
                    chapter.Text.Contains(br.Name + ".") ||
                    chapter.Text.Contains(br.Name + ",") ||
                    chapter.Text.Contains(br.Name + "\n"))
                {
                    chapterRules.Add(br.Name, br.ID.ToString());
                }
            }
            //spin through all customer service messages and add to chapterRules if name
            //contained in text
            foreach (var csm in csmList)
            {
                if (chapter.Text.Contains(csm.Name + " ") ||
                    chapter.Text.Contains(csm.Name + ")") ||
                    chapter.Text.Contains(csm.Name + ".") ||
                    chapter.Text.Contains(csm.Name + ",") ||
                    chapter.Text.Contains(csm.Name + "\n"))
                {
                    chapterRules.Add(csm.Name, csm.ID.ToString());
                }
            }
            //check for new rules and/or csms in the rule text
            var updatedChapterRules = checkForNewItemsInChapterRules(chapterRules);
            return updatedChapterRules;
        }

        /// <summary>
        /// spins through the list of chapter rules referenced in a document and
        /// checks for rule references in the rules themselves.  New rules are
        /// added as necessary until no new rules are found
        /// </summary>
        /// <param name="chapterRules">SortedDictionary&lt;string, string&gt;</param>
        /// <returns>SortedDictionary&lt;string, string&gt;</returns>
        private SortedDictionary<string, string> checkForNewItemsInChapterRules(SortedDictionary<string, string> chapterRules)
        {
            var brList = brBLL.getAllBusinessRules();
            var csmList = csmBLL.getAllCSMs();
            var updatedChapterRules = chapterRules;
            bool newReference = false;
            do //spin through all chapter rules until no new br or csm references are found
            {
                newReference = false;
                foreach (var item in chapterRules)
                {
                    //spin through all business rules and add to chapterRules if name
                    //contained in text
                    foreach (var br in brList)
                    {
                        if (item.Value.Contains(br.Name))
                        {
                            if (!chapterRules.ContainsKey(br.Name))
                            {
                                updatedChapterRules.Add(br.Name, br.Text);
                                newReference = true;
                            }
                        }
                    }
                    //spin through all customer service messages and add to chapterRules if name
                    //contained in text
                    foreach (var csm in csmList)
                    {
                        if (item.Value.Contains(csm.Name))
                        {
                            if (!chapterRules.ContainsKey(csm.Name))
                            {
                                updatedChapterRules.Add(csm.Name, csm.Text);
                                newReference = true;
                            }
                        }
                    }
                }
                chapterRules = updatedChapterRules;
            }
            while (newReference);
            return updatedChapterRules;
        }

        /// <summary>
        /// function used to determine if a specific business rule id is referenced
        /// anywhere in a document
        /// </summary>
        /// <param name="documentID">int</param>
        /// <param name="businessRuleID">int</param>
        /// <param name="customerServiceMessageID">int</param>
        /// <returns>bool</returns>
        public bool isBRCSMReferencedInDocument(int documentID, int businessRuleID, int customerServiceMessageID)
        {
            bool retVal = false;
            viewBusinessRule br = null;
            viewCustomerServiceMessage csm = null;
            if (businessRuleID > 0)
            {
                br = brBLL.getBusinessRuleByID(businessRuleID);
            }
            else if (customerServiceMessageID > 0)
            {
                csm = csmBLL.getCSMByID(customerServiceMessageID);

            }
            viewDocument doc = this.getDocumentByID(documentID);
            //spin through all document chapters and check if chapter contains business rule name
            foreach (var chapter in doc.getChapters())
            {
                if (br != null)
                {
                    if (chapter.Text.Contains(br.Name))
                    {
                        retVal = true;
                    }
                }
                if (csm != null)
                {
                    if (chapter.Text.Contains(csm.Name))
                    {
                        retVal = true;
                    }
                }
            }
            return retVal;
        }

        /// <summary>
        /// function used to determine if a specific business rule id is referenced
        /// anywhere in a document
        /// </summary>
        /// <param name="chapterID">int</param>
        /// <param name="businessRuleID">int</param>
        /// <param name="customerServiceMessageID">int</param>
        /// <returns>bool</returns>
        public bool isBRCSMReferencedInChapter(int chapterID, int businessRuleID, int customerServiceMessageID)
        {
            bool retVal = false;
            viewBusinessRule br = null;
            viewCustomerServiceMessage csm = null;
            if (businessRuleID > 0)
            {
                br = brBLL.getBusinessRuleByID(businessRuleID);
            }
            else if (customerServiceMessageID > 0)
            {
                csm = csmBLL.getCSMByID(customerServiceMessageID);

            }
            viewChapter chapter = this.getChapterByID(chapterID);
            if (br != null)
            {
                if (chapter.Text.Contains(br.Name))
                {
                    retVal = true;
                }
            }
            if (csm != null)
            {
                if (chapter.Text.Contains(csm.Name))
                {
                    retVal = true;
                }
            }
            return retVal;
        }

        /// <summary>
        /// get url to show document pdf based on doc type
        /// </summary>
        /// <param name="documentID">int</param>
        /// <param name="testCaseID">int</param>
        /// <returns>string</returns>
        public string getViewDocumentRedirectURL(int documentID, int testCaseID = 0)
        {
            StringBuilder redirectURL = new StringBuilder();
            if (testCaseID > 0)
            {
                redirectURL.Append("ShowTestCaseDocumentPDF.aspx?");
                redirectURL.Append("tcid=");
                redirectURL.Append(testCaseID);
            }
            else
            {
                //determine doc type
                var doc = getDocumentByID(documentID);
                switch (doc.DocumentTypeID)
                {
                    case (3): // test case doc

                        break;
                    case (4): // scenario doc
                        break;
                    default:
                        redirectURL.Append("ShowFunctionalDocumentPDF.aspx?");
                        redirectURL.Append("did=");
                        redirectURL.Append(documentID);
                        break;
                }
            }
            return redirectURL.ToString();
        }

        /// <summary>
        /// get url to show report pdf based on report id
        /// </summary>
        /// <param name="reportID">int</param>
        /// <returns>string</returns>
        public string getViewReportRedirectURL(int reportID, int id = 0)
        {
            StringBuilder redirectURL = new StringBuilder();
            switch (reportID)
            {
                case ((int)Report.ReportType.TESTCASE_ORPHANED_BUSINESSRULES): // Business Rules with no test case reference
                case ((int)Report.ReportType.DOCUMENT_ORPHANED_BUSINESSRULES): // Business Rules not referenced in all documents
                    redirectURL.Append("ShowReportPDF.aspx?");
                    redirectURL.Append("rid=");
                    redirectURL.Append(reportID);
                    break;
                case ((int)Report.ReportType.FUNCTIONAL_DOCUMENT_ORPHANED_BUSINESSRULES): //Business Rules not referenced in function documents
                    redirectURL.Append("ShowReportPDF.aspx?");
                    redirectURL.Append("rid=");
                    redirectURL.Append(reportID);
                    redirectURL.Append("&");
                    redirectURL.Append("dtid=");
                    redirectURL.Append((int)documentTypeEnums.DocumentType.Functional);
                    break;
                case ((int)Report.ReportType.TECHNICAL_DOCUMENT_ORPHANED_BUSINESSRULES): //Business Rules not referenced in technical documents
                    redirectURL.Append("ShowReportPDF.aspx?");
                    redirectURL.Append("rid=");
                    redirectURL.Append(reportID);
                    redirectURL.Append("&");
                    redirectURL.Append("dtid=");
                    redirectURL.Append((int)documentTypeEnums.DocumentType.Technical);
                    break;
                case ((int)Report.ReportType.MISCELLANEOUS_DOCUMENT_ORPHANED_BUSINESSRULES): //Business Rules not referenced in miscellaneous documents
                    redirectURL.Append("ShowReportPDF.aspx?");
                    redirectURL.Append("rid=");
                    redirectURL.Append(reportID);
                    redirectURL.Append("&");
                    redirectURL.Append("dtid=");
                    redirectURL.Append((int)documentTypeEnums.DocumentType.Miscellaneous);
                    break;
                case ((int)Report.ReportType.BUSINESSRULES_BY_SECTION):
                    redirectURL.Append("ShowReportPDF.aspx?");
                    redirectURL.Append("rid=");
                    redirectURL.Append(reportID);
                    redirectURL.Append("&");
                    redirectURL.Append("dtid=");
                    redirectURL.Append((int)documentTypeEnums.DocumentType.Miscellaneous);
                    redirectURL.Append("&");
                    redirectURL.Append("sid=");
                    redirectURL.Append(id);
                    break;
                case((int)Report.ReportType.BUSINESSRULES_BY_STATUS):
                    redirectURL.Append("ShowReportPDF.aspx?");
                    redirectURL.Append("rid=");
                    redirectURL.Append(reportID);
                    redirectURL.Append("&");
                    redirectURL.Append("stid=");
                    redirectURL.Append(id);
                    break;
                default:
                    
                    break;
            }
            return redirectURL.ToString();
        }

        /// <summary>
        /// get url to show document pdf based on doc type
        /// </summary>
        /// <param name="documentID">int</param>
        /// <param name="chapterID">int</param>
        /// <returns>string</returns>
        public string getViewDocumentChapterRedirectURL(int documentID, int chapterID)
        {
            StringBuilder redirectURL = new StringBuilder();
            redirectURL.Append("ShowFunctionalDocumentPDF.aspx?");
            redirectURL.Append("did=");
            redirectURL.Append(documentID);
            redirectURL.Append("&");
            redirectURL.Append("cid=");
            redirectURL.Append(chapterID);
            return redirectURL.ToString();
        }

        /// <summary>
        /// fetch the redirect URL
        /// </summary>
        /// <param name="documentID">int</param>
        /// <returns>string</returns>
        public string getDocumentEditRedirectURL(int documentID)
        {
            StringBuilder redirectURL = new StringBuilder();
            redirectURL.Append("DocumentEdit.aspx?");
            redirectURL.Append("docID=");
            redirectURL.Append(documentID);
            return redirectURL.ToString();
        }

        /// <summary>
        /// returns list of business rule that are not referenced in any project document
        /// </summary>
        /// <returns>List&lt;viewBusinessRule&gt;</returns>
        public List<viewBusinessRule> getBusinessRulesNotInDocuments()
        {
            //get full list of business rules
            var orphanedBusinessRules = new List<viewBusinessRule>();
            BusinessRulesBLL brBLL = new BusinessRulesBLL(this.ProjectID);
            orphanedBusinessRules = brBLL.getAllBusinessRules();
            var origBRList = brBLL.getAllBusinessRules();
            //get all documents
            var docList = this.getAllDocuments();
            //spin through all documents
            foreach (var doc in docList)
            {
                //spin through all chapters
                var chapList = dal.getChaptersForDocumentID(doc.ID);
                foreach (var chapter in chapList)
                {
                    var chapText = chapter.Text;
                    foreach (var br in origBRList)
                    {
                        //if business rule is referenced in text, remove rule from list
                        if (chapText.Contains(br.Name + ",") ||
                            chapText.Contains(br.Name + " ") ||
                            chapText.Contains(br.Name + ")") ||
                            chapText.Contains(br.Name + ".") ||
                            chapText.Contains(br.Name + "\n"))
                        {
                            orphanedBusinessRules.Remove(br);
                        }
                    }
                }
            }
            return orphanedBusinessRules;
        }

        /// <summary>
        /// returns list of business rule that are not referenced in any project document 
        /// of a specific document type
        /// </summary>
        /// <returns>List&lt;viewBusinessRule&gt;</returns>
        public List<viewBusinessRule> getBusinessRulesNotInDocuments(int docTypeID)
        {
            //get full list of business rules
            var orphanedBusinessRules = new List<viewBusinessRule>();
            BusinessRulesBLL brBLL = new BusinessRulesBLL(this.ProjectID);
            orphanedBusinessRules = brBLL.getAllBusinessRules();
            var origBRList = brBLL.getAllBusinessRules();
            //get all documents
            var docList = this.getDocumentsByType(docTypeID);
            //spin through all documents
            foreach (var doc in docList)
            {
                //spin through all chapters
                var chapList = dal.getChaptersForDocumentID(doc.ID);
                foreach (var chapter in chapList)
                {
                    var chapText = chapter.Text;
                    foreach (var br in origBRList)
                    {
                        //if business rule is referenced in text, remove rule from list
                        if (chapText.Contains(br.Name + ",") ||
                            chapText.Contains(br.Name + " ") ||
                            chapText.Contains(br.Name + ")") ||
                            chapText.Contains(br.Name + ".") ||
                            chapText.Contains(br.Name + "\n"))
                        {
                            orphanedBusinessRules.Remove(br);
                        }
                    }
                }
            }
            return orphanedBusinessRules;
        }

        /// <summary>
        /// return list of business rules that are not referenced in any test case for a probject
        /// </summary>
        /// <returns>List&lt;viewBusinessRule&gt;</returns>
        public List<viewBusinessRule> getBusinessRulesWithoutTestCase()
        {
            var orphanedBusinessRules = new List<viewBusinessRule>();
            var testCases = new List<viewTestCase>();
            //start with full list of business rules
            orphanedBusinessRules = brBLL.getAllBusinessRules();
            var origBRList = brBLL.getAllBusinessRules();
            //get list of all test cases
            testCases = tcBLL.getAllTestCases();
            foreach (var testcase in testCases)
            {
                //get list of business rules references in test case
                var tcBusinessRules = tcBLL.getBusinessRulesForTestCase(testcase.ID);
                //compare list and remove matches from orphanded list
                foreach (var tcbr in tcBusinessRules)
                {
                    foreach (var br in origBRList)
                    {
                        if (br.Equals(tcbr))
                        {
                            orphanedBusinessRules.Remove(br);
                        }
                    }
                }
            }
            return orphanedBusinessRules;
        }

        public void renameBusinessRuleInDocuments(string oldBRName, string newBRName)
        {
            AdminBLL adminBLL = new AdminBLL(this.ProjectID);
            BusinessRulesBLL brBLL = new BusinessRulesBLL(this.ProjectID);
            //get all doctypes
            var docTypeList = adminBLL.getAllDocumentTypes();
            foreach (var type in docTypeList)
            {
                //get all documents for Doctype
                var docList = this.getDocumentsByType(type.ID);
                //spin through all documents
                foreach (var doc in docList)
                {
                    //spin through all chapters
                    var chapList = dal.getChaptersForDocumentID(doc.ID);
                    foreach (var chapter in chapList)
                    {
                        var chapText = chapter.Text;
                        bool changeMade = false;
                        //if business rule is referenced in text, replace old name with new name
                        if (chapText.Contains(oldBRName + ","))
                        {
                            chapText = chapText.Replace(oldBRName + ",", newBRName + ",");
                            changeMade = true;
                        }
                        if (chapText.Contains(oldBRName + " "))
                        {
                            chapText = chapText.Replace(oldBRName + " ", newBRName + " ");
                            changeMade = true;
                        }
                        if (chapText.Contains(oldBRName + ")"))
                        {
                            chapText = chapText.Replace(oldBRName + ")", newBRName + ")");
                            changeMade = true;
                        }
                        if (chapText.Contains(oldBRName + "."))
                        {
                            chapText = chapText.Replace(oldBRName + ".", newBRName + ".");
                            changeMade = true;
                        }
                        if (chapText.Contains(oldBRName + "\n"))
                        {
                            chapText = chapText.Replace(oldBRName + "\n", newBRName + "\n");
                            changeMade = true;
                        }
                        //save updated chapter if change was made
                        if (changeMade)
                        {
                            chapter.Text = chapText;
                            this.updateChapter(chapter);
                        }
                    }
                }
            }
        }

        #endregion utilities

    }
}
