using System;
using System.Collections.Generic;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using BLL;
using Entities;
using Enums;
using System.Text;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Web;
using System.Configuration;

namespace Palinoia.UI.Documents
{
    public partial class DocumentManager : basePalinoiaPage
    {
        #region properties and variables

        /// <summary>
        /// class variable for adminBLL 
        /// </summary>
        AdminBLL adminBLL;
        /// <summary>
        /// class variable for palinoiaBLL 
        /// </summary>
        applicationBLL palinoiaBLL;
        /// <summary>
        /// class variable for docBLL
        /// </summary>
        DocumentsBLL docBLL;
        int userID;
        int projectID;
        SearchBLL searchBLL;
        int searchType;
        
        #endregion properties and variables

        #region page lifecycle events
                
        /// <summary>
        /// Page Init Event
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            //this.addJavaScriptReference("Documents/ChapterEdit.js");
            this.addJavaScriptReference("Documents/DocumentManager.js");
            this.addSearchReference();
            this.addCKEditorReference();
        }
                
        /// <summary>page load event</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //check for valid user id
            userID = Convert.ToInt32(Session.Contents["userID"]);
            if (userID > 0)
            {
                this.searchType = (int)Enums.searchObjectTypeEnums.SearchObjectType.Documents;
                this.btnGridAdvancedSearch.Visible = false;
                this.btnTreeAdvancedSearch.Visible = true;
                this.hdnUserID.Value = userID.ToString();
                projectID = Convert.ToInt32(Session.Contents["projectID"]);
                this.hdnProjectID.Value = projectID.ToString();
                adminBLL = new AdminBLL(projectID);
                palinoiaBLL = new applicationBLL(projectID);
                docBLL = new DocumentsBLL(projectID);
                searchBLL = new SearchBLL(projectID);
                applyFeatures(userID);
                populateAdvancedSearchControls();
                populateDocumentTypeDDL();
                //chapter edit screen setup
                int documentID = 0;
                bool result1 = int.TryParse(this.hdnDocumentID.Value, out documentID);
                int chapterID = 0;
                bool nullChapter = false;
                bool result2 = int.TryParse(this.hdnChapterID.Value, out chapterID);
                if (!result2)
                {
                    nullChapter = true;
                }
                if (chapterID == 0)
                {
                    var result3 = int.TryParse(this.hdnChapterID.Value, out chapterID);
                }
                if (result1 && result2)
                {
                    this.hdnProjectID.Value = projectID.ToString();
                    this.hdnChapterID.Value = chapterID.ToString();
                    this.hdnDocumentID.Value = documentID.ToString();
                    this.hdnDisableChapterDelete.Value = "false";
                    this.hdnDisableChapterEdit.Value = "false";
                    if (chapterID > 0)
                    {
                        //edit mode
                        var chapter = docBLL.getChapterByID(chapterID);
                        this.txtChapterName.Text = chapter.Title;
                        //this.setClientMode("edit");
                        this.hdnChapterEditMode.Value = "edit";

                    }
                    else
                    {
                        //add mode
                    }
                    if (nullChapter)
                    {
                        this.hdnChapterEditMode.Value = "";
                    }
                }
            }
            else
            {
                //user invalid.  redirect to login
                Response.Redirect("~/Account/Login.aspx");
            }
        }

        #endregion page lifecycle events

        #region event handlers

        /// <summary>
        /// handles events when the edit document button is clicked
        /// </summary>
        /// <param name="sender">objevt</param>
        /// <param name="e">EventArgs</param>
        protected void btnEditDocument_Click(object sender, EventArgs e)
        {
            int editDocumentID = 0;
            //pull doc id from hidden field
            var docID = this.hdnDocumentID.Value;
            //convert to int
            bool result = int.TryParse(docID, out editDocumentID);
            if (result)
            {
                this.openDocumentEditWindow(docBLL.getDocumentEditRedirectURL(editDocumentID));
            }
        }

        /// <summary>
        /// handles events when the delete document button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnDeleteDocument_Click(object sender, EventArgs e)
        {
            int documentID = 0;
            bool result = int.TryParse(this.hdnDocumentID.Value, out documentID);
            if (result)
            {
                var deleteResult = docBLL.deleteDocumentByID(documentID);
                if (!deleteResult.Equals("OK"))
                {
                    sendMessageToClient(deleteResult);
                }
            }
        }

        /// <summary>
        /// handles events when the add document button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnAddDocument_Click(object sender, EventArgs e)
        {
            this.openDocumentEditWindow(docBLL.getDocumentEditRedirectURL(0));
        }

        /// <summary>
        /// handles evenys when the delete chapter button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnDeleteChapter_Click(object sender, EventArgs e)
        {
            int chapterID = 0;
            bool result = int.TryParse(this.hdnChapterID.Value, out chapterID);
            if (result)
            {
                var deleteResult = docBLL.deleteChapterByID(chapterID, this.userID);
                this.hdnChapterID.Value = "";
                this.hdnChapterEditMode.Value = "";
                this.setClientMode("");
                if (!deleteResult.Equals("OK"))
                {
                    sendMessageToClient(deleteResult);
                }
            }
        }

        /// <summary>
        /// handles events when the add chapter button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnAddChapter_Click(object sender, EventArgs e)
        {
            int documentID = 0;
            bool result = int.TryParse(this.hdnDocumentID.Value, out documentID);
            if (result)
            {
                openChapterEditWindow(getRedirectURL(documentID, 0));
            }
        }

        /// <summary>
        /// handles events when the save documents button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnSaveDocument_Click(object sender, EventArgs e)
        {
            string clientMessage = "";
            string saveResult = "";
            //get doc id from client
            int documentID = 0;
            int documentTypeID = 0;
            bool docActive = true;
            bool result = int.TryParse(this.hdnDocumentTypeID.Value, out documentTypeID);
            result = int.TryParse(this.hdnDocumentID.Value, out documentID);
            if (result)
            {
                //pull data from client
                string docTitle = this.txtDocumentTitle.Text;
                string docDescription = this.txtDocumentDescription.Text;

                //validate required fields
                if (docTitle.Length == 0)
                {
                    clientMessage = "Document title is required.";
                }
                if (docDescription.Length == 0)
                {
                    clientMessage = "Document description is required.";
                }
                if (validateInputText(docTitle) == false)
                {
                    clientMessage = "Invalid text in document title.";
                }
                if (validateInputText(docDescription) == false)
                {
                    clientMessage = "Invalid text in document description";
                }
                if (documentTypeID == 0)
                {
                    clientMessage = "A selection is required for document type.";
                }
                if (clientMessage.Length > 0)
                {
                    sendMessageToClient(clientMessage);
                }
                else //validation ok, save document info
                {
                    viewDocument doc = new viewDocument(documentID,
                                                        documentTypeID,
                                                        docTitle,
                                                        docDescription,
                                                        docActive,
                                                        this.userID);
                    //no errors.  proceed with save
                    if (documentID == 0)
                    { //add new document
                        saveResult = docBLL.addNewDocument(doc);
                        //new doc id or error will be returned
                        int newID = 0;
                        bool parseOK = int.TryParse(saveResult, out newID);
                        if (parseOK)
                        {
                            saveResult = "OK";
                            this.hdnDocumentID.Value = newID.ToString();
                            //enable add chapter button for newly added document
                            //this.btnAddChapter.Enabled = true;
                        }
                    }
                    else // update existing document
                    {
                        saveResult = docBLL.updateDocument(doc);
                        //update chapter sequence
                        var chapterNodes = this.hdnChapterSequence.Value;
                        //remove last comma
                        var deleteIndex = chapterNodes.LastIndexOf(",");
                        chapterNodes = chapterNodes.Remove(deleteIndex, 1);
                        //convert comma delimeted string from client to array
                        var chapterArray = chapterNodes.Split(',');
                        int sequence = 1;
                        foreach (var chapter in chapterArray)
                        {
                            int chapterID = 0;
                            bool parseResult = int.TryParse(chapter, out chapterID);
                            if (parseResult)
                            {
                                docBLL.updateChapterSequence(documentID, chapterID, sequence);
                                sequence++;
                            }
                        }
                    }
                    if (!saveResult.Equals("OK"))
                    {
                        sendMessageToClient(saveResult);
                    }
                }
            }
        }

        /// <summary>
        /// handles events when save add image button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnSaveAddImage_Click(object sender, EventArgs e)
        {
            var appBLL = new applicationBLL();
            string fileName = this.fileAddImage.FileName;
            var pathName = new StringBuilder();
            pathName.Append(appBLL.GetEnvData("WorkingDirectory"));
            pathName.Append("/");
            pathName.Append(appBLL.getProjectByID(this.projectID).Name);
            pathName.Append(appBLL.GetEnvData("DocumentImagePath"));
            var byteArray = this.fileAddImage.FileBytes;
            string description = this.txtImageDescription.Text;
            //var savePath = Server.MapPath("~\\Data\\Documents\\DocumentImages\\");
            //string pathName = savePath + fileName;
            ImageData iData = new ImageData(fileName, pathName.ToString(), description, byteArray, this.userID);
            string result = docBLL.addImage(iData);
        }

        #endregion event handlers

        #region private methods

        private void openPDFWindow(string redirectURL)
        {
            //// Define the name and type of the client scripts on the page.
            String csname = "OpenPDFTabScript";
            Type cstype = this.GetType();
            String cstext = "window.open(\"" + redirectURL + "\");";
            ScriptManager.RegisterStartupScript(this, cstype, csname, cstext, true);
        }

        /// <summary>
        /// open dpocument edit window
        /// </summary>
        /// <param name="redirectURL">string</param>
        private void openDocumentEditWindow(string redirectURL)
        {
            //// Define the name and type of the client scripts on the page.
            String csname = "OpenDocumentEditScript";
            Type cstype = this.GetType();
            String cstext = "window.location = location.href;window.open(\"" + redirectURL + "\", name='_self', '', false);";
            ScriptManager.RegisterStartupScript(this, cstype, csname, cstext, true);
        }

        /// <summary>
        /// apply features
        /// </summary>
        /// <param name="userID">int</param>
        private void applyFeatures(int userID)
        {
            viewUser user = palinoiaBLL.getUserByID(userID);
            List<viewFeature> userFeatureList = palinoiaBLL.getFeaturesForUser(user);
            //VIEW
            viewFeature feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.DocumentsView);
            if (feature != null)
            {
                this.hdnDisableDocumentView.Value = "false";
            }
            else
            {
                this.hdnDisableDocumentView.Value = "true";
            }
            //ADD
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.DocumentsAdd);
            if (feature != null)
            {
                this.hdnDisableDocumentAdd.Value = "false";
            }
            else
            {
                this.hdnDisableDocumentAdd.Value = "true";
            }
            //EDIT
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.DocumentsEdit);
            if (feature != null)
            {
                this.hdnDisableDocumentEdit.Value = "false";
            }
            else
            {
                this.hdnDisableDocumentEdit.Value = "true";
            }
            //DELETE
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.DocumentsDelete);
            if (feature != null)
            {
                this.hdnDisableDocumentDelete.Value = "false";
            }
            else
            {
                this.hdnDisableDocumentDelete.Value = "true";
            }
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.ChapterView);
            if (feature != null)
            {
                this.hdnDisableChapterView.Value = "false";
            }
            else
            {
                this.hdnDisableChapterView.Value = "true";
            }
            //ADD
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.ChapterAdd);
            if (feature != null)
            {
                this.hdnDisableChapterAdd.Value = "false";
            }
            else
            {
                this.hdnDisableChapterAdd.Value = "true";
            }
            //EDIT
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.ChapterEdit);
            if (feature != null)
            {
                this.hdnDisableChapterEdit.Value = "false";
            }
            else
            {
                this.hdnDisableChapterEdit.Value = "true";
            }
            //DELETE
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.ChapterDelete);
            if (feature != null)
            {
                this.hdnDisableChapterDelete.Value = "false";
            }
            else
            {
                this.hdnDisableChapterDelete.Value = "true";
            }
        }

        /// <summary>
        /// open chapter edit window
        /// </summary>
        /// <param name="redirectURL">string</param>
        public void openChapterEditWindow(string redirectURL)
        {
            //// Define the name and type of the client scripts on the page.
            String csname = "OpenChapterEditScript";
            Type cstype = this.GetType();
            String cstext = "window.open(\"" + redirectURL + "\", name='_self', '', false);";
            ScriptManager.RegisterStartupScript(this, cstype, csname, cstext, true);
        }

        /// <summary>
        /// fetch redirect URL
        /// </summary>
        /// <param name="documentID">int</param>
        /// <param name="chapterID">int</param>
        /// <returns>string</returns>
        public string getRedirectURL(int documentID, int chapterID)
        {
            StringBuilder redirectURL = new StringBuilder();
            redirectURL.Append("ChapterEdit.aspx?");
            redirectURL.Append("chapID=");
            redirectURL.Append(chapterID);
            redirectURL.Append("&");
            redirectURL.Append("docID=");
            redirectURL.Append(documentID);
            return redirectURL.ToString();
        }

        /// <summary>
        /// populate document type DDL
        /// </summary>
        private void populateDocumentTypeDDL()
        {
            List<viewDocumentType> dtList = new List<viewDocumentType>();
            dtList = adminBLL.getAllActiveDocumentTypes();
            this.ddlDocumentType.Items.Clear();
            this.ddlDocumentType.Items.Add(new ListItem("select one", "0"));
            foreach (var type in dtList)
            {
                this.ddlDocumentType.Items.Add(new ListItem(type.Text, type.ID.ToString()));
            }
        }

        /// <summary>
        /// build IMG tag
        /// </summary>
        /// <param name="imageID">int</param>
        /// <param name="projectID">int</param>
        /// <returns>string</returns>
        private static string buildIMGTag(int imageID, int projectID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<IMG src=\"");
            sb.Append("LoadImage.ashx?id=");
            sb.Append(imageID);
            sb.Append("&pid=");
            sb.Append(projectID);
            sb.Append("\" />");
            return sb.ToString();
        }
                
        /// <summary>
        /// set client mode
        /// </summary>
        /// <param name="mode">string</param>
        public void setClientMode(string mode)
        {
            // Define the name and type of the client script on the page.
            String csName = "setClientMode";
            Type csType = this.GetType();
            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs = Page.ClientScript;
            // Check to see if the client script is already registered.
            if (!cs.IsStartupScriptRegistered(csType, csName))
            {
                ClientScript.RegisterStartupScript(typeof(Page), csName, "<script type='text/javascript'>setMode('" + mode + "');</script>");
            }
        }
        
        #endregion private methods

        #region web methods
                
        /// <summary>
        /// fetches documents from database for tree
        /// </summary>
        /// <param name="nodeID">string</param>
        /// <param name="projID">string</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<JSTree_Node> GetDocumentsForTree(string nodeID, string projID)
        {
            int projectID = Convert.ToInt32(projID);
            var adminBLL = new AdminBLL(projectID);
            var docBLL = new DocumentsBLL(projectID);
            //determine which node is being opened
            //DOC = document
            //CH = chapter
             List<JSTree_Node> nodes = new List<JSTree_Node>();
             if (!nodeID.Equals("0"))
             {
                 var idArray = nodeID.Split('_');
                 var objectAbbv = idArray[0];
                 var objectID = idArray[1];
                 bool parseResult = false;
                 switch (objectAbbv)
                 {
                     case ("DOC"):
                         nodes = GetChapterNodesForDocument(projID, objectID);
                         break;
                     case("DTYP"):
                         int documentTypeID = 0;
                         parseResult = int.TryParse(objectID, out documentTypeID);
                         if (documentTypeID == (int)Enums.documentTypeEnums.DocumentType.TestCase)
                         {
                             nodes = GetSectionNodes(projectID);
                         }
                         else
                         {
                             nodes = GetDocumentNodesForType(projectID, documentTypeID);
                         }
                         break;
                     case("SEC"):
                         int sectionID = 0;
                         parseResult = int.TryParse(objectID, out sectionID);
                         nodes = GetTCDocumentNodesForSection(projectID, sectionID);
                         break;
                 }
             }
             else // screen is loading; populate root node with document types
             {
                 var docTypeList = adminBLL.getAllDocumentTypes();
                 JSTree_Node rootNode = new JSTree_Node();
                 rootNode.data = new JsTreeNodeData { title = "Documents" };
                 rootNode.state = "open";
                 rootNode.IdServerUse = 0;
                 rootNode.attr = new JsTreeAttribute { id = "ROOT_", rel="Root", selected = false };
                 rootNode.children = GetDocumentTypeNodes(projectID, docTypeList).ToArray();
                 nodes.Add(rootNode);
            }
            adminBLL = null;
            docBLL = null;
            return nodes;
        }
                
        /// <summary>
        /// fetches documents from database for tree
        /// </summary>
        /// <param name="projectID">int</param>
        /// <param name="docTypeList">List&lt;viewDocumentType&gt;</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        public static List<JSTree_Node> GetDocumentTypeNodes(int projectID, List<viewDocumentType> docTypeList)
        {
            var bll = new DocumentsBLL(projectID);
            
            List<JSTree_Node> JSTreeArray = new List<JSTree_Node>();
            foreach (var docType in docTypeList)
            {
                int CurrChildId = docType.ID;
                JSTree_Node jsTreeNode = new JSTree_Node();
                jsTreeNode.data = new JsTreeNodeData { title = docType.Text };
                jsTreeNode.state = "closed";  //For async to work
                jsTreeNode.IdServerUse = CurrChildId;
                jsTreeNode.attr = new JsTreeAttribute { id = "DTYP_" + CurrChildId.ToString(), rel = "DocumentType", selected = false };
                JSTreeArray.Add(jsTreeNode);
            }
            return JSTreeArray;
        }
                
        /// <summary>
        /// add child nodes to a tree
        /// </summary>
        /// <param name="projectID">int</param>
        /// <param name="documentTypeID">int</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        private static List<JSTree_Node> GetDocumentNodesForType(int projectID, int documentTypeID)
        {
            var docBLL = new DocumentsBLL(projectID);
            var documentList = docBLL.getDocumentsByType(documentTypeID);
            List<JSTree_Node> JSTreeArray = new List<JSTree_Node>();
            foreach (var document in documentList)
            {
                int CurrChildId = document.ID;
                JSTree_Node jsTreeNode = new JSTree_Node();
                var iconPath = "../../Scripts/JSTree/icons/pdf.gif";
                jsTreeNode.data = new JsTreeNodeData { title = document.Name, icon = iconPath };
                if (docBLL.documentHasChapters(CurrChildId))
                {
                    jsTreeNode.state = "closed";  //For async to work
                }
                jsTreeNode.IdServerUse = CurrChildId;
                string docDescription = document.Description;
                jsTreeNode.children = null;
                jsTreeNode.attr = new JsTreeAttribute { id = "DOC_" + CurrChildId.ToString(), rel = "Document", selected = false };
                JSTreeArray.Add(jsTreeNode);
            }
            docBLL = null;
            documentList = null;
            return JSTreeArray;
        }
                
        /// <summary>
        /// add child nodes to a tree
        /// </summary>
        /// <param name="projectID">int</param>
        /// <param name="sectionID">int</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        private static List<JSTree_Node> GetTCDocumentNodesForSection(int projectID, int sectionID)
        {
            var tcBLL = new TestCasesBLL(projectID);
            var tcList = tcBLL.getAllTestCasesBySection(sectionID);
            List<JSTree_Node> JSTreeArray = new List<JSTree_Node>();
            try
            {
                foreach (var tc in tcList)
                {
                    int CurrChildId = tc.ID;
                    JSTree_Node jsTreeNode = new JSTree_Node();
                    var iconPath = "../../Scripts/JSTree/icons/pdf.gif";
                    jsTreeNode.data = new JsTreeNodeData { title = tc.Name, icon = iconPath };
                    jsTreeNode.IdServerUse = CurrChildId;
                    jsTreeNode.children = null;
                    jsTreeNode.attr = new JsTreeAttribute { id = "TCD_" + CurrChildId.ToString(), rel = "TestCase", selected = false };
                    JSTreeArray.Add(jsTreeNode);
                }
            }
            catch (Exception ex)
            {
                var appBLL = new applicationBLL();
                appBLL.logError(ex, projectID, 0);
                appBLL = null;
            }
            finally
            {
                //docBLL = null;
                tcBLL = null;
                tcList = null;
                //tcDocumentList = null;
            }
            return JSTreeArray;
        }

        /// <summary>
        /// add child nodes to a tree
        /// </summary>
        /// <param name="projectID">int</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        private static List<JSTree_Node> GetSectionNodes(int projectID)
        {
            var adminBLL = new AdminBLL(projectID);
            var tcBLL = new TestCasesBLL(projectID);
            var sectionList = adminBLL.getAllSections();
            List<JSTree_Node> JSTreeArray = new List<JSTree_Node>();
            foreach (var section in sectionList)
            {
                int CurrChildId = section.ID;
                JSTree_Node jsTreeNode = new JSTree_Node();
                jsTreeNode.data = new JsTreeNodeData { title = section.Text };
                if (tcBLL.hasTestCases(section.ID))
                {
                    jsTreeNode.state = "closed";  //For async to work
                }
                jsTreeNode.IdServerUse = CurrChildId;
                jsTreeNode.children = null;
                jsTreeNode.attr = new JsTreeAttribute { id = "SEC_" + CurrChildId.ToString(), rel = "Section", selected = false };
                JSTreeArray.Add(jsTreeNode);
            }
            return JSTreeArray;
        }
                
        /// <summary>
        /// fetches chapters from database for tree
        /// </summary>
        /// <param name="projID">string</param>
        /// <param name="docID">string</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        public static List<JSTree_Node> GetChapterNodesForDocument(string projID, string docID)
        {
            int projectID = Convert.ToInt32(projID);
            int documentID = Convert.ToInt32(docID);
            var docBLL = new DocumentsBLL(projectID);
            var chapterList = docBLL.getChaptersForDocumentID(documentID);
            List<JSTree_Node> JSTreeArray = new List<JSTree_Node>();
            foreach (var chapter in chapterList)
            {
                int CurrChildId = chapter.ID;
                JSTree_Node jsTreeNode = new JSTree_Node();
                var iconPath = "../../Scripts/JSTree/icons/chapter.gif";
                jsTreeNode.data = new JsTreeNodeData { title = chapter.Title, icon = iconPath };
                jsTreeNode.IdServerUse = CurrChildId;
                jsTreeNode.children = null;
                jsTreeNode.attr = new JsTreeAttribute { id = "CHP_" + CurrChildId.ToString(), rel = "Chapter", selected = false };
                JSTreeArray.Add(jsTreeNode);
            }
            docBLL = null;
            chapterList = null;
            return JSTreeArray;
        }

        /// <summary>
        /// fetch edit details for document
        /// </summary>
        /// <param name="projID">string</param>
        /// <param name="docID">string</param>
        /// <returns>EditDocumentDetails</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static EditDocumentDetails getEditDetailsForDocument(string projID, string docID)
        {
            int documentID = Convert.ToInt32(docID);
            int projectID = Convert.ToInt32(projID);
            var docBLL = new DocumentsBLL(projectID);
            EditDocumentDetails edd = docBLL.getEditDetailsForDocument(documentID);
            docBLL = null;
            return edd;
        }

        /// <summary>
        /// save edit document details
        /// </summary>
        /// <param name="projID">string</param>
        /// <param name="userID">string</param>
        /// <param name="details">EditDocumentDetails</param>
        /// <returns>string</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string saveEditDocumentDetails(string projID, string userID, EditDocumentDetails details)
        {
            string result = "";
            int projectID = Convert.ToInt32(projID);
            int usrID = Convert.ToInt32(userID);
            var docBLL = new DocumentsBLL(projectID);
            var appBLL = new applicationBLL();
            int incomingDocID = 0;
            bool parseResult = int.TryParse(details.id, out incomingDocID);
            try
            {
                //TODO: Validate text from client
                viewDocument doc = new viewDocument(incomingDocID,
                                                    Convert.ToInt32(details.typeID),
                                                    details.title,
                                                    details.desc,
                                                    true,
                                                    usrID);
                if (incomingDocID == 0)
                {
                    result = docBLL.addNewDocument(doc);
                }
                else
                {
                    result = docBLL.updateDocument(doc);
                }
            }
            catch (Exception ex)
            {
                
                appBLL.logError(ex, projectID, usrID);
            }
            finally
            {
                docBLL = null;
                appBLL = null;
            }
            return result;
        }

        /// <summary>
        /// save chapter order
        /// </summary>
        /// <param name="projID">string</param>
        /// <param name="userID">string</param>
        /// <param name="docID">string</param>
        /// <param name="children">List&lt;string&gt;</param>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static void saveChapterOrder(string projID, string userID, string docID, List<string> children)
        {
            int documentID = 0;
            int projectID = 0;
            int usrID = 0;
            bool parseResult = int.TryParse(docID, out documentID);
            parseResult = int.TryParse(projID, out projectID);
            parseResult = int.TryParse(userID, out usrID);
            //convert children to int list
            List<int> chapterIDList = new List<int>();
            foreach (var child in children)
            {
                int id = 0;
                parseResult = int.TryParse(child, out id);
                chapterIDList.Add(id);
            }
            var docBLL = new DocumentsBLL(projectID);
            docBLL.reorderDocumentChapters(documentID, chapterIDList);
        }

        /// <summary>
        /// save chapter text
        /// </summary>
        /// <param name="did">string</param>
        /// <param name="cid">string</param>
        /// <param name="pid">string</param>
        /// <param name="title">string</param>
        /// <param name="text">string</param>
        /// <param name="uid">string</param>
        /// <returns>string</returns>
        [WebMethod]
        public static string saveChapterText(string did, string cid, string pid, string title, string text, string uid)
        {
            int documentID, chapterID, projectID, userID;
            bool result1 = int.TryParse(cid, out chapterID);
            bool result2 = int.TryParse(pid, out projectID);
            bool result3 = int.TryParse(did, out documentID);
            bool result4 = int.TryParse(uid, out userID);
            if (result1 && result2 && result4)
            {
                viewChapter chapter = new viewChapter(chapterID,
                                                      title,
                                                      text,
                                                      userID,
                                                      (int)Enums.chapterTypeEnums.ChapterType.User,
                                                      true);
                DocumentsBLL bll = new DocumentsBLL(projectID);
                //determine if chapter is being added or updated
                if (chapterID == 0) //new chapter
                {
                    cid = bll.addNewChapter(documentID, chapter);
                }
                else
                {
                    string updateResult = bll.updateChapter(chapter);
                    if (!updateResult.Equals("OK"))
                    {
                        //error
                        cid = updateResult;
                    }
                }
                bll = null;
            }
            return cid;
        }

        /// <summary>
        /// fetch chapter text
        /// </summary>
        /// <param name="cid">string</param>
        /// <param name="pid">string</param>
        /// <returns>string</returns>
        [WebMethod]
        public static string getChapterText(string cid, string pid)
        {
            string chapterText = "";
            int projectID, chapterID;
            bool result1 = int.TryParse(cid, out chapterID);
            bool result2 = int.TryParse(pid, out projectID);
            if (result1 && result2)
            {
                DocumentsBLL bll = new DocumentsBLL(projectID);
                viewChapter chapter = bll.getChapterByID(chapterID);
                chapterText = chapter.Text;
            }
            return chapterText;
        }

        /// <summary>
        /// fetch chapter title
        /// </summary>
        /// <param name="cid">string</param>
        /// <param name="pid">string</param>
        /// <returns>string</returns>
        [WebMethod]
        public static string getChapterTitle(string cid, string pid)
        {
            string chapterTitle = "";
            int projectID, chapterID;
            bool result1 = int.TryParse(cid, out chapterID);
            bool result2 = int.TryParse(pid, out projectID);
            if (result1 && result2)
            {
                DocumentsBLL bll = new DocumentsBLL(projectID);
                viewChapter chapter = bll.getChapterByID(chapterID);
                chapterTitle = chapter.Title;
            }
            return chapterTitle;
        }

        /// <summary>
        /// fetch image HTML
        /// </summary>
        /// <param name="pid">string</param>
        /// <param name="fn">string</param>
        /// <returns>string</returns>
        [WebMethod]
        public static string getImageHTML(string pid, string fn)
        {
            string html = "";
            int nextImageID = 0;
            int projectID = 0;
            bool result = int.TryParse(pid, out projectID);
            if (result)
            {
                //pull filename from path
                var index = fn.LastIndexOf("\\");
                var fileName = fn.Substring(index + 1);
                string workingDirectory = ConfigurationManager.AppSettings["WorkingDirectory"];
                applicationBLL appBLL = new applicationBLL();
                string projectName = appBLL.getProjectByID(projectID).Name;
                string imagePath = ConfigurationManager.AppSettings["DocumentImagePath"];
                string dbDataSource = workingDirectory + "/" + projectName + "/";
                DocumentsBLL bll = new DocumentsBLL(projectID);
                nextImageID = bll.getNextImageID(dbDataSource, fileName);
                html = buildIMGTag(nextImageID, projectID);
                appBLL = null;
            }

            return html;
        }

        /// <summary>
        /// returns url with query string to open window for display of pdf
        /// created for full document(did > 0), test document(tcid > 0), or single chapter (chapterID > 0)
        /// </summary>
        /// <param name="pid">string</param>
        /// <param name="did">string</param>
        /// <param name="cid">string</param>
        /// <param name="tcid">string</param>
        /// <returns>string</returns>
        [WebMethod]
        public static string getURLForViewDocument(string pid, string did, string cid, string tcid)
        {
            string docURL = "";
            int projectID = 0;
            int documentID = 0;
            int chapterID = 0;
            int testCaseID = 0;
            var result1 = int.TryParse(pid, out projectID);
            var result2 = int.TryParse(did, out documentID);
            var result3 = int.TryParse(cid, out chapterID);
            var result4 = int.TryParse(tcid, out testCaseID);
            if(projectID > 0) {
                var docBLL = new DocumentsBLL(projectID);
                if (testCaseID > 0)
                {
                    docURL = docBLL.getViewDocumentRedirectURL(0, testCaseID);
                }
                else if (chapterID > 0)
                {
                    docURL = docBLL.getViewDocumentChapterRedirectURL(documentID, chapterID);
                }
                else if (documentID > 0)
                {
                    docURL = docBLL.getViewDocumentRedirectURL(documentID);
                }
            }
            return docURL;
        }
        
        #endregion web methods

        #region search

        #region event handlers

        /// <summary>
        /// event handler for Clear button.  Clears basic search textbox and
        /// reloads defects grid with full defect list
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnClearBasicSearch_Click(object sender, EventArgs e)
        {
            txtBasicSearch.Text = "";
        }

        /// <summary>
        /// event handler for Clear button.  Clears all advanced search textboxes and
        /// ddls and reloads defects grid with full defect list
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnClearAdvancedSearch_Click(object sender, EventArgs e)
        {
            this.txtValue1.Text = "";
            this.txtValue2.Text = "";
            this.txtValue3.Text = "";
            this.txtValue4.Text = "";
            this.ddlConnector2.Items[0].Selected = true;
            this.ddlConnector3.Items[0].Selected = true;
            this.ddlConnector4.Items[0].Selected = true;
            //clear hidden fields for advanced search info
            this.hdnSearchObjectDDL1.Value = "";
            this.hdnSearchObjectDDL2.Value = "";
            this.hdnSearchObjectDDL3.Value = "";
            this.hdnSearchObjectDDL4.Value = "";
            this.hdnOperator1.Value = "";
            this.hdnOperator2.Value = "";
            this.hdnOperator3.Value = "";
            this.hdnOperator4.Value = "";
            this.hdnConnector2.Value = "";
            this.hdnConnector3.Value = "";
            this.hdnConnector4.Value = "";
            //repopulate search controls and search results
            populateAdvancedSearchControls();
        }

        #endregion event handlers

        #region advanced search controls

        private void populateAdvancedSearchControls()
        {
            populateConnectorDDLs();
            populateOperatorDDLs();
            populateSearchObjectDDLs();
        }

        private void populateConnectorDDLs()
        {
            ddlConnector2.Items.Clear();
            ddlConnector2.Items.Add(new ListItem("", "0"));
            ddlConnector2.Items.Add(new ListItem("OR", "1"));
            ddlConnector2.Items.Add(new ListItem("AND", "2"));
            ddlConnector3.Items.Clear();
            ddlConnector3.Items.Add(new ListItem("", "0"));
            ddlConnector3.Items.Add(new ListItem("OR", "1"));
            ddlConnector3.Items.Add(new ListItem("AND", "2"));
            ddlConnector4.Items.Clear();
            ddlConnector4.Items.Add(new ListItem("", "0"));
            ddlConnector4.Items.Add(new ListItem("OR", "1"));
            ddlConnector4.Items.Add(new ListItem("AND", "2"));
        }

        private void populateSearchObjectDDLs()
        {
            var soList = searchBLL.getDDLSearchObjectsForTypeID(this.searchType);
            //cleear search objects ddls
            this.ddlSearchObject1.Items.Clear();
            this.ddlSearchObject2.Items.Clear();
            this.ddlSearchObject3.Items.Clear();
            this.ddlSearchObject4.Items.Clear();
            //insert empty item on top
            this.ddlSearchObject1.Items.Add(new ListItem("Select one", "0"));
            this.ddlSearchObject2.Items.Add(new ListItem("Select one", "0"));
            this.ddlSearchObject3.Items.Add(new ListItem("Select one", "0"));
            this.ddlSearchObject4.Items.Add(new ListItem("Select one", "0"));
            foreach (var searchItem in soList)
            {
                this.ddlSearchObject1.Items.Add(new ListItem(searchItem.Name, searchItem.Value));
                this.ddlSearchObject2.Items.Add(new ListItem(searchItem.Name, searchItem.Value));
                this.ddlSearchObject3.Items.Add(new ListItem(searchItem.Name, searchItem.Value));
                this.ddlSearchObject4.Items.Add(new ListItem(searchItem.Name, searchItem.Value));
            }
        }

        private void populateOperatorDDLs()
        {
            var opList = searchBLL.getSearchOperators();
            //clear operator ddls
            this.ddlOperator1.Items.Clear();
            this.ddlOperator2.Items.Clear();
            this.ddlOperator3.Items.Clear();
            this.ddlOperator4.Items.Clear();
            //add empty item on top
            this.ddlOperator1.Items.Add(new ListItem("Select one", "0"));
            this.ddlOperator2.Items.Add(new ListItem("Select one", "0"));
            this.ddlOperator3.Items.Add(new ListItem("Select one", "0"));
            this.ddlOperator4.Items.Add(new ListItem("Select one", "0"));
            //populate ddl items
            foreach (var item in opList)
            {
                this.ddlOperator1.Items.Add(new ListItem(item.TEXT, item.ID.ToString()));
                this.ddlOperator2.Items.Add(new ListItem(item.TEXT, item.ID.ToString()));
                this.ddlOperator3.Items.Add(new ListItem(item.TEXT, item.ID.ToString()));
                this.ddlOperator4.Items.Add(new ListItem(item.TEXT, item.ID.ToString()));
            }
        }

        private bool isValidSearchRow(string obj, string op, string value)
        {
            var rowValid = false; ;
            if (obj != null && op != null && value != null &&
               obj != "" && op != "" && value != null)
            {
                rowValid = true;
            }
            return rowValid;
        }

        #endregion advanced search controls

        #region web methods

        /// <summary>
        /// fetch datatype for search object
        /// </summary>
        /// <param name="searchObjectID">int</param>
        /// <param name="projectID">int</param>
        /// <returns>int</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static int getDataTypeForSearchObject(int searchObjectID, int projectID)
        {
            List<ListItem> items = new List<ListItem>();
            SearchBLL bll = new SearchBLL(projectID);
            var dataTypeID = bll.getDataTypeForSearchObjectID(searchObjectID);
            bll = null;
            return dataTypeID;
        }
        /// <summary>
        /// returns list of items for lkup type search objects
        /// used to populate value DDL in advanced search UI
        /// </summary>
        /// <param name="searchObjectID">int</param>
        /// <param name="projectID">int</param>
        /// <returns>string</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string getSearchObjectValuesForDDL(int searchObjectID, int projectID)
        {
            SearchBLL bll = new SearchBLL(projectID);
            // CALL DAL to fetch all records from XREF table
            List<DDLValueItem> items = bll.getDDLValuesForSearchObject(searchObjectID);
            JsonSerializer serializer = new JsonSerializer();
            string json = JsonConvert.SerializeObject(items, Formatting.Indented);
            bll = null;

            return json;
        }

        /// <summary>
        /// fetch advanced search results
        /// </summary>
        /// <param name="data">List&lt;ClientSearchData&gt;</param>
        /// <param name="projID">string</param>
        /// <param name="searchType">string</param>
        /// <returns>DocumentSearchResult</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static DocumentSearchResult getAdvancedSearchResults(List<ClientSearchData> data, string projID, string searchType)
        {
            List<int> results = new List<int>();
            int projectID = 0;
            int searchTypeID = 0;
            bool result = int.TryParse(projID, out projectID);
            result = int.TryParse(searchType, out searchTypeID);
            SearchBLL bll = new SearchBLL(projectID);
            //convert ClientSearchData List to SearchEntities object
            SearchEntities search = new SearchEntities();
            foreach (var item in data)
            {
                string value = "";
                if (item.calendarValue.Length > 0)
                {
                    value = item.calendarValue;
                }
                else if (item.ddlValue.Length > 0)
                {
                    value = item.ddlValue;
                }
                else
                {
                    value = item.textValue;
                }
                var row = new SearchRow(item.objectID,
                                              item.operatorID,
                                              value,
                                              item.connector);
                search.addSearchRow(row);
            }
            var searchResult = bll.doAdvancedDocumentSearch(search, searchTypeID);
            bll = null;
            return searchResult;
        }

        /// <summary>
        /// fetch basic search results
        /// </summary>
        /// <param name="data">List&lt;ClientSearchData&gt;</param>
        /// <param name="projID">string</param>
        /// <param name="searchType">string</param>
        /// <returns>DocumentSearchResult</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static DocumentSearchResult getBasicSearchResults(List<ClientSearchData> data, string projID, string searchType)
        {
            int projectID = 0;
            int searchTypeID = 0;
            bool parseResult = int.TryParse(projID, out projectID);
            parseResult = int.TryParse(searchType, out searchTypeID);
            SearchBLL bll = new SearchBLL(projectID);
            SearchEntities search = new SearchEntities();
            var value = data[0].textValue;
            var searchResult = bll.doBasicDocumentSearch(value, searchTypeID);
            bll = null;
            return searchResult;
        }

        /// <summary>
        /// fetch search results for tree
        /// </summary>
        /// <param name="nodeID">string</param>
        /// <param name="searchResult">DocumentSearchResult</param>
        /// <param name="projID">string</param>
        /// <param name="searchType">string</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<JSTree_Node> getSearchResultsForTree(string nodeID, DocumentSearchResult searchResult, string projID, string searchType)
        {
            int projectID = 0;
            int searchTypeID = 0;
            var adminBLL = new AdminBLL(projectID);
            bool result = int.TryParse(projID, out projectID);
            result = int.TryParse(searchType, out searchTypeID);
            List<JSTree_Node> nodes = new List<JSTree_Node>();
            if (nodeID.Equals("0"))
            {
                JSTree_Node rootNode = new JSTree_Node();
                var docTypeList = adminBLL.getAllDocumentTypes();
                int resultCount = searchResult.chapID.Count + searchResult.docID.Count;
                string rootTitle = resultCount + " Search Results";
                //string iconPath = "../../Scripts/JSTree/icons/folder-arrow-down-icon-24.png";
                string iconPath = ConfigurationManager.AppSettings["DocumentManagerIcon"];
                rootNode.data = new JsTreeNodeData { title = rootTitle, icon = iconPath };
                rootNode.state = "open";
                rootNode.IdServerUse = 0;
                rootNode.attr = new JsTreeAttribute { id = "ROOT", selected = false };
                rootNode.children = GetSearchDocumentTypeNodes(projectID, docTypeList, searchResult).ToArray();
                nodes.Add(rootNode);
            }
            else
            {
                var idArray = nodeID.Split('_');
                var objectAbbv = idArray[0];
                var objectID = idArray[1];
                switch (objectAbbv)
                {
                    case("DTYP"):
                        int documentTypeID = 0;
                        var parseResult = int.TryParse(objectID, out documentTypeID);
                        if (documentTypeID == (int)Enums.documentTypeEnums.DocumentType.TestCase)
                        {
                            nodes = GetSearchSectionNodes(projectID, searchResult);
                        }
                        else
                        {
                            nodes = GetSearchDocumentNodesForType(projectID, documentTypeID, searchResult);
                        }
                        break;
                    case("DOC"):
                        nodes = GetSearchResultChapterNodesForDocument(projectID.ToString(), objectID, searchResult);
                        break;
                    case ("SEC"):
                        int sectionID = 0;
                        parseResult = int.TryParse(objectID, out sectionID);
                        nodes = GetSearchTCDocumentNodesForSection(projectID, sectionID, searchResult);
                        break;
               }
            }
            return nodes;
        }
                
        /// <summary>
        /// fetches documents from database for tree
        /// </summary>
        /// <param name="projectID">int</param>
        /// <param name="docTypeList">List&lt;viewDocumentType&gt;</param>
        /// <param name="searchResult">DocumentSearchResult</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        public static List<JSTree_Node> GetSearchDocumentTypeNodes(int projectID, List<viewDocumentType> docTypeList, DocumentSearchResult searchResult)
        {
            var bll = new DocumentsBLL(projectID);

            List<JSTree_Node> JSTreeArray = new List<JSTree_Node>();
            foreach (var docType in docTypeList)
            {
                int CurrChildId = docType.ID;
                JSTree_Node jsTreeNode = new JSTree_Node();
                string iconPath = "";
                if (docTypeHasResults(projectID, docType.ID, searchResult))
                {
                    jsTreeNode.state = "closed";  //For async to work
                    //iconPath = "../../Scripts/JSTree/icons/folder-check-icon-24.png";
                    iconPath = ConfigurationManager.AppSettings["DM_DocumentTypeCheckIcon"];
                    jsTreeNode.data = new JsTreeNodeData { title = docType.Text, icon = iconPath };
                    jsTreeNode.IdServerUse = CurrChildId;
                    jsTreeNode.attr = new JsTreeAttribute { id = "DTYP_" + CurrChildId.ToString(), rel = "DocumentType", selected = false };
                    JSTreeArray.Add(jsTreeNode);
                }
            }
            return JSTreeArray;
        }
                
        /// <summary>
        /// fetches chapters from database for tree
        /// </summary>
        /// <param name="projID">string</param>
        /// <param name="docID">string</param>
        /// <param name="searchResults">DocumentSearchResult</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        public static List<JSTree_Node> GetSearchResultChapterNodesForDocument( string projID, 
                                                                                string docID, 
                                                                                DocumentSearchResult searchResults)
        {
            int projectID = Convert.ToInt32(projID);
            int documentID = Convert.ToInt32(docID);
            var docBLL = new DocumentsBLL(projectID);
            var chapterList = docBLL.getChaptersForDocumentID(documentID);
            List<JSTree_Node> JSTreeArray = new List<JSTree_Node>();
            foreach (var chapter in chapterList)
            {
                int CurrChildId = chapter.ID;
                JSTree_Node jsTreeNode = new JSTree_Node();
                string iconPath = "";
                if(searchResults.chapID.Contains(chapter.ID.ToString())) {
                    //iconPath = "../../Scripts/JSTree/icons/document-check-icon-24.png";
                    iconPath = ConfigurationManager.AppSettings["DM_ChapterCheckIcon"];
                }
                else {
                    //iconPath = "../../Scripts/JSTree/icons/document-write-icon-24.png";
                    iconPath = ConfigurationManager.AppSettings["DM_ChapterIcon"];
                }
                jsTreeNode.data = new JsTreeNodeData { title = chapter.Title, icon = iconPath };
                jsTreeNode.IdServerUse = CurrChildId;
                jsTreeNode.children = null;
                jsTreeNode.attr = new JsTreeAttribute { id = "CHP_" + CurrChildId.ToString(), rel = "Chapter", selected = false };
                JSTreeArray.Add(jsTreeNode);
            }
            docBLL = null;
            chapterList = null;
            return JSTreeArray;
        }
                
        /// <summary>
        /// add child nodes to a tree
        /// </summary>
        /// <param name="projectID">int</param>
        /// <param name="documentTypeID">int</param>
        /// <param name="searchResult">DocumentSearchResult</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        private static List<JSTree_Node> GetSearchDocumentNodesForType( int projectID, 
                                                                        int documentTypeID,
                                                                        DocumentSearchResult searchResult)
        {
            var docBLL = new DocumentsBLL(projectID);
            var fullDocumentList = docBLL.getDocumentsByType(documentTypeID);
            List<viewDocument> documentList = new List<viewDocument>();
            foreach (var doc in fullDocumentList)
            {
                if(documentHasResults(projectID, doc.ID, searchResult)) {
                    documentList.Add(doc);
                }
            }
            List<JSTree_Node> JSTreeArray = new List<JSTree_Node>();
            foreach (var document in documentList)
            {
                int CurrChildId = document.ID;
                JSTree_Node jsTreeNode = new JSTree_Node();
                //var iconPath = "../../Scripts/JSTree/icons/document-check-icon-24.png";
                var iconPath = ConfigurationManager.AppSettings["DM_ChapterCheckIcon"];
                jsTreeNode.data = new JsTreeNodeData { title = document.Name, icon = iconPath };
                if (docBLL.documentHasChapters(CurrChildId))
                {
                    jsTreeNode.state = "closed";  //For async to work
                }
                jsTreeNode.IdServerUse = CurrChildId;
                string docDescription = document.Description;
                jsTreeNode.children = null;
                jsTreeNode.attr = new JsTreeAttribute { id = "DOC_" + CurrChildId.ToString(), rel = "Document", selected = false };
                JSTreeArray.Add(jsTreeNode);
            }
            docBLL = null;
            documentList = null;
            return JSTreeArray;
        }
                
        /// <summary>
        /// add child nodes to a tree
        /// </summary>
        /// <param name="projectID">int</param>
        /// <param name="sectionID">int</param>
        /// <param name="searchResult">DocumentSearchResult</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        private static List<JSTree_Node> GetSearchTCDocumentNodesForSection(int projectID,
                                                                            int sectionID, 
                                                                            DocumentSearchResult searchResult)
        {
            //var tcBLL = new TestCasesBLL(projectID);
            //var docBLL = new DocumentsBLL(projectID);
            //var tcList = tcBLL.getAllTestCasesBySection(sectionID);
            //List<viewDocument> tcDocumentList = new List<viewDocument>();
            //foreach (var tc in tcList)
            //{
            //    var tcDocID = docBLL.getDocumentIDForTestCaseID(tc.ID);
            //    var tcDoc = docBLL.getDocumentByID(tcDocID);
            //    tcDocumentList.Add(tcDoc);
            //}
            //List<viewDocument> resultDocList = new List<viewDocument>();
            //if (tcDocumentList.Count > 0)
            //{
            //    foreach (var doc in tcDocumentList)
            //    {
            //        if (searchResult.docID.Contains(doc.ID.ToString()))
            //        {
            //            resultDocList.Add(doc);
            //        }
            //    }
            //}
            List<JSTree_Node> JSTreeArray = new List<JSTree_Node>();
            //foreach (var document in resultDocList)
            //{
            //    int CurrChildId = document.ID;
            //    JSTree_Node jsTreeNode = new JSTree_Node();
            //    string iconPath = "";
            //    if (documentHasResults(projectID, document.ID, searchResult))
            //    {
            //        //iconPath = "../../Scripts/JSTree/icons/document-check-icon-24.png";
            //        iconPath = ConfigurationManager.AppSettings["DM_ChapterCheckIcon"];
            //    }
            //    else
            //    {
            //        //iconPath = "../../Scripts/JSTree/icons/document-write-icon-24.png";
            //        iconPath = ConfigurationManager.AppSettings["DM_ChapterIcon"];
            //    }
            //    jsTreeNode.data = new JsTreeNodeData { title = document.Name, icon = iconPath };
            //    jsTreeNode.IdServerUse = CurrChildId;
            //    string docDescription = document.Description;
            //    jsTreeNode.children = null;
            //    jsTreeNode.attr = new JsTreeAttribute { id = "DOC_" + CurrChildId.ToString(), rel = "Document", selected = false };
            //    JSTreeArray.Add(jsTreeNode);
            //}
            //docBLL = null;
            //tcDocumentList = null;
            //sectionDocumentList = null;
            return JSTreeArray;
        }
                
        /// <summary>
        /// add child nodes to a tree
        /// </summary>
        /// <param name="projectID">int</param>
        /// <param name="searchResult">DocumentSearchResult</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        private static List<JSTree_Node> GetSearchSectionNodes(int projectID, DocumentSearchResult searchResult)
        {
            var adminBLL = new AdminBLL(projectID);
            var sectionList = adminBLL.getAllSections();
            List<JSTree_Node> JSTreeArray = new List<JSTree_Node>();
            foreach (var section in sectionList)
            {
                int CurrChildId = section.ID;
                var iconPath = "";
                if (sectionHasResults(projectID, section.ID, searchResult))
                {
                    //iconPath = "../../Scripts/JSTree/icons/folder-check-icon-24.png";
                    iconPath = ConfigurationManager.AppSettings["DM_SectionCheckIcon"];
                    JSTree_Node jsTreeNode = new JSTree_Node();
                    jsTreeNode.data = new JsTreeNodeData { title = section.Text, icon = iconPath };
                    if (sectionHasResults(projectID, section.ID, searchResult))
                    {
                        jsTreeNode.state = "closed";  //For async to work
                    }
                    jsTreeNode.IdServerUse = CurrChildId;
                    jsTreeNode.children = null;
                    jsTreeNode.attr = new JsTreeAttribute { id = "SEC_" + CurrChildId.ToString(), rel = "Section", selected = false };
                    JSTreeArray.Add(jsTreeNode);
                }
            }
            return JSTreeArray;
        }

        #endregion web methods

        #region private

        private static bool sectionHasResults(int projectID, int sectionID, DocumentSearchResult result)
        {
            bool hasResults = false;
            //var tcBLL = new TestCasesBLL(projectID);
            //var docBLL = new DocumentsBLL(projectID);
            //var sectionTestCases = tcBLL.getAllTestCasesBySection(sectionID);
            //foreach (var tc in sectionTestCases)
            //{
            //    if (documentHasResults(projectID, docBLL.getDocumentIDForTestCaseID(tc.ID), result))
            //    {
            //        hasResults = true;
            //    }
            //}
            //tcBLL = null;
            return hasResults;
        }

        private static bool documentHasResults(int projectID, int documentID, DocumentSearchResult result)
        {
            var docBLL = new DocumentsBLL(projectID);
            bool hasResults = false;
            //check for doc id match in search results doc ids
            if (result.docID.Contains(documentID.ToString()))
            {
                hasResults = true;
            }
            if (!hasResults)
            {
                //check for doc id match in search result chapter ids
                foreach (var id in result.chapID)
                {
                    int chapterID = Convert.ToInt32(id);
                    var resultDocID = docBLL.getDocumentIDForChapter(chapterID);
                    if (documentID == resultDocID)
                    {
                        hasResults = true;
                    }
                }
            }
            docBLL = null;
            return hasResults;
        }

        private static bool docTypeHasResults(int projectID, int docTypeID, DocumentSearchResult result)
        {
            var docBLL = new DocumentsBLL(projectID);
            bool hasResults = false;
            var docs = docBLL.getDocumentsByType(docTypeID);
            foreach (var doc in docs)
            {
                if (documentHasResults(projectID, doc.ID, result))
                {
                    hasResults = true;
                }
            }
            docBLL = null;
            return hasResults;
        }

        #endregion private

        #endregion search
    }
}