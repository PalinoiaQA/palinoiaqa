using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Entities;
using Enums;
using Newtonsoft.Json;

namespace Palinoia.UI.CustomerServiceMessages
{   
    /// <summary>
    /// class to hold code for CustomerServiceMessages
    /// </summary>
    public partial class CustomerServiceMessages : basePalinoiaPage
    {

        #region properties and variables

        CustomerServiceMessagesBLL bll;
        AdminBLL adminBLL;
        applicationBLL palinoiaBLL;
        SearchBLL searchBLL;
        bool disableEdit;
        bool disableDelete;
        int ProjectID;
        int userID;
        int searchType;

        #endregion properties and variables

        #region page lifecycle events

        /// <summary>
        /// initializes a page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            this.addCKEditorReference();
            this.addCookieReference();
            this.addSearchReference();
            this.addJavaScriptReference("CustomerServiceMessages/CustomerServiceMessages.js");
        }
                
        /// <summary>
        /// loads the page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScript.GetPostBackEventReference(new PostBackOptions(btnEditCSM));
            //check for valid user id
            userID = Convert.ToInt32(Session.Contents["userID"]);
            if (userID > 0)
            {
                //set search properties
                this.searchType = (int)Enums.searchObjectTypeEnums.SearchObjectType.CustomerServiceMessages;
                this.hdnSearchTypeID.Value = this.searchType.ToString();
                this.btnGridAdvancedSearch.Visible = false;
                this.btnTreeAdvancedSearch.Visible = true;
                ProjectID = Convert.ToInt32(Session.Contents["projectID"]);
                this.hdnProjectID.Value = ProjectID.ToString();
                bll = new CustomerServiceMessagesBLL(ProjectID);
                adminBLL = new AdminBLL(ProjectID);
                palinoiaBLL = new applicationBLL(ProjectID);
                searchBLL = new SearchBLL(ProjectID);
                populateAdvancedSearchControls();
                applyFeatures(userID);
                if (!IsPostBack)
                {
                    populateSectionsDDL();
                    populateStatusDDL();
                    populateCSMResponseTypesDDL();
                    populateCSMTypesDDL();
                    this.setClientMode("");
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
        /// rowdata event handler for the CSMs grid
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewRowEventArgs</param>
        protected void grdCSMs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // loop all data cells
                foreach (DataControlFieldCell cell in e.Row.Cells)
                {
                    // check all cells in one row
                    foreach (Control control in cell.Controls)
                    {
                        // Must use LinkButton here instead of ImageButton
                        // if you are having Links (not images) as the command button.
                        var button = control as LinkButton;

                        if (button != null && button.CommandName == "Delete")
                        {
                            // Add delete confirmation
                            button.OnClientClick = "return confirm('Are you sure you want to delete this record?');";
                            if (disableDelete)
                            {
                                button.Visible = false;
                            }
                        }
                        if (disableEdit)
                        {
                            if (button != null && button.CommandName == "Edit")
                                // Add delete confirmation
                                button.Visible = false;
                        }
                    }
                }
            }
        }
                
        /// <summary>
        /// handles events caused by clicking save add business rule button
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnSaveAddCSM_Click(object sender, EventArgs e)
        {
            int csmTypeID = Convert.ToInt32(this.ddlCSMType.SelectedItem.Value);
            int csmResponseTypeID = Convert.ToInt32(this.ddlCSMResponseType.SelectedItem.Value);
            string csmType = this.ddlCSMType.SelectedItem.Value;
            string csmResponseType = this.ddlCSMResponseType.SelectedItem.Value;
            string newText = this.CKEditor1.Text;
            string newName = getCSMNameFromClient();
            int statusID = Convert.ToInt32(ddlCSMStatus.SelectedItem.Value);
            string status = ddlCSMStatus.SelectedItem.Text;
            int sectionID = Convert.ToInt32(this.hdnSectionID.Value);
            viewSection vSection = adminBLL.getSectionByID(sectionID);
            string section = vSection.Abbreviation;
            bool active = this.chkActive.Checked;
            //required field check
            if (newText.Length == 0)
            {
                sendMessageToClient("CSM text is required.");
            }
            else if (newName.Length == 0)
            {
                sendMessageToClient("CSM name is required.");
            }
            else
            {
                //create new viewCSM object
                viewCustomerServiceMessage newCSM = new viewCustomerServiceMessage(0, 
                                                                                   newName, 
                                                                                   statusID, 
                                                                                   csmTypeID,
                                                                                   csmResponseTypeID,
                                                                                   newText, 
                                                                                   sectionID, 
                                                                                   active,
                                                                                   this.userID);
                //call bll to save view object
                var result = bll.addCSM(newCSM);
                //alert client if error
                if (!result.Equals("OK"))
                {
                    string error = result;
                    sendMessageToClient("ERROR: " + result);
                    setClientMode("add");
                }
                else
                {
                    clearCSMEditControls();
                    setClientMode("");
                }
            }
        }
                
        /// <summary>
        /// handles events caused by clicking save edit business rule button
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnSaveEditCSM_Click(object sender, EventArgs e)
        {
            int csmTypeID = Convert.ToInt32(this.ddlCSMType.SelectedItem.Value);
            int csmResponseTypeID = Convert.ToInt32(this.ddlCSMResponseType.SelectedItem.Value);
            string csmType = this.ddlCSMType.SelectedItem.Value;
            string csmResponseType = this.ddlCSMResponseType.SelectedItem.Text;
            //get id of selected csm reponse type
            int selectedID = Convert.ToInt32(this.hdnCSMID.Value);
            string editName = getCSMNameFromClient();
            string editText = this.CKEditor1.Text;
            int editStatusID = Convert.ToInt32(this.ddlCSMStatus.SelectedValue);
            string editStatus = this.ddlCSMStatus.SelectedItem.Text;
            int sectionID = Convert.ToInt32(this.hdnSectionID.Value);
            viewSection vSection = adminBLL.getSectionByID(sectionID);
            string section = vSection.Abbreviation;
            bool editActive = this.chkActive.Checked;
            //required field check
            if (editName.Length == 0)
            {
                sendMessageToClient("Business Rule name is required.");
            }
            else if (editText.Length == 0)
            {
                sendMessageToClient("Business Rule text is required.");
            }
            else
            {
                //no special chars check
                //if (palinoiaBLL.isValidText(editName.ToString()) && palinoiaBLL.isValidText(editText))
                //{
                var editCSM = new viewCustomerServiceMessage(selectedID,
                                                            editName.ToString(),
                                                            editStatusID,
                                                            csmTypeID,
                                                            csmResponseTypeID,
                                                            editText,
                                                            sectionID,
                                                            editActive,
                                                            userID);
                string result = bll.updateCSM(editCSM);
                if (!result.Equals("OK"))
                {
                    sendMessageToClient(result);
                    setClientMode("edit");
                }
                else
                {
                    //hide save controls
                    clearCSMEditControls();
                    setClientMode("");
                }
            }
        }
                
        /// <summary>
        /// handles events when the edit business rule button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnEditCSM_Click(object sender, EventArgs e)
        {
            if (!disableEdit)
            {
                int editCSMID = Convert.ToInt32(this.hdnCSMID.Value);
                int projectID = Convert.ToInt32(this.hdnProjectID.Value);
                var bll = new CustomerServiceMessagesBLL(projectID);
                var csm = bll.getCSMByID(editCSMID);
                this.CKEditor1.Text = csm.Text;
                this.lblCSMName.Text = csm.Name;
                this.chkActive.Checked = csm.Active;
                populateStatusDDL(csm.StatusID);
                //populate Text parts
                var nameArray = csm.Name.Split('.');
                this.txtCSMName1.Text = nameArray[0];
                foreach (ListItem item in ddlCSMNameSection.Items)
                {
                    if (item.Text.Equals(nameArray[1]))
                    {
                        item.Selected = true;
                        break;
                    }
                    else
                    {
                        item.Selected = false;
                    }
                }
                this.txtCSMName3.Text = nameArray[2];
                if (nameArray.Length > 3)
                {
                    this.txtCSMName4.Text = nameArray[3];
                }
                this.hdnSectionID.Value = csm.SectionID.ToString();
                setClientMode("edit");
            }
            else
            {
                sendMessageToClient("You do not have permission to edit customer service messages.");
            }
        }
                
        /// <summary>
        /// handles events when the edit business rule button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnDeleteCSM_Click(object sender, EventArgs e)
        {
            if (!disableDelete)
            {
                int deleteCSMID = Convert.ToInt32(this.hdnCSMID.Value);
                this.hdnCSMID.Value = "0";
                int projectID = Convert.ToInt32(this.hdnProjectID.Value);
                var bll = new CustomerServiceMessagesBLL(projectID);
                string result = bll.deleteCSM(deleteCSMID, this.userID);
                if (result != "OK")
                {
                    sendMessageToClient(result);
                }
                setClientMode("");
            }
            else
            {
                sendMessageToClient("You do not have permission to delete customer service messages.");
            }
        }

        #endregion event handlers

        #region ui controls
                
        /// <summary>
        /// applies features to a particular user
        /// </summary>
        /// <param name="userID">int</param>
        public void applyFeatures(int userID)
        {
            viewUser user = palinoiaBLL.getUserByID(userID);
            List<viewFeature> userFeatureList = palinoiaBLL.getFeaturesForUser(user);
            //VIEW
            viewFeature feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.CustomerServiceMessagesView);
            if (feature != null)
            {

            }
            else
            {
                //user cannot view screen.  redirect to default display
                sendMessageToClient("You do not have the necessary permission to view this screen.");
                this.enableViewDIV.Visible = false;
            }
            //ADD
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.CustomerServiceMessagesAdd);
            if (feature != null)
            {
                this.hdnDisableAdd.Value = "false";
            }
            else
            {
                this.hdnDisableAdd.Value = "true";
            }
            //EDIT
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.CustomerServiceMessagesEdit);
            if (feature != null)
            {
                disableEdit = false;
                this.hdnDisableEdit.Value = "false";
            }
            else
            {
                disableEdit = true;
                this.hdnDisableEdit.Value = "true";
            }
            //DELETE
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.CustomerServiceMessagesDelete);
            if (feature != null)
            {
                disableDelete = false;
                this.hdnDisableDelete.Value = "false";
            }
            else
            {
                disableDelete = true;
                this.hdnDisableDelete.Value = "true";
            }
        }

        /// <summary>
        /// reset all business rule edit textboxes and dropdowns in CSMs edit section
        /// </summary>
        public void clearCSMEditControls()
        {
            this.lblCSMName.Text = "";
            this.CKEditor1.Text = "";
            this.txtCSMName1.Text = "";
            this.ddlCSMNameSection.SelectedIndex = 0;
            this.txtCSMName3.Text = "";
            this.txtCSMName4.Text = "";
            this.hdnCSMID.Value = "";
        }
                
        /// <summary>
        /// populate the status info
        /// </summary>
        private void populateStatusDDL()
        {
            //clear ddl
            ddlCSMStatus.Items.Clear();
            //get list of statuses from bll
            var statusList = adminBLL.getAllStatuses();
            foreach (var status in statusList)
            {
                ddlCSMStatus.Items.Add(new ListItem(status.Text, status.ID.ToString()));
            }
            ddlCSMStatus.SelectedIndex = 0;
        }

        /// <summary>
        /// populate the status info with selected id
        /// </summary>
        private void populateStatusDDL(int statusID)
        {
            int itemCounter = 0;
            //clear ddl
            ddlCSMStatus.Items.Clear();
            //get list of statuses from bll
            var statusList = adminBLL.getAllStatuses();
            foreach (var status in statusList)
            {
                ddlCSMStatus.Items.Add(new ListItem(status.Text, status.ID.ToString()));
                if (status.ID == statusID)
                {
                    ddlCSMStatus.Items[itemCounter].Selected = true;
                }
                else
                {
                    ddlCSMStatus.Items[itemCounter].Selected = false;
                }
                itemCounter++;
            }
        }
                
        /// <summary>
        /// populate the CSMResponseTypesDDL
        /// </summary>
        private void populateCSMResponseTypesDDL()
        {
            //clear ddl
            this.ddlCSMResponseType.Items.Clear();
            //get list of statuses from bll
            var rtList = adminBLL.getActiveCSMResponseTypes();
            foreach (var responseType in rtList)
            {
                ddlCSMResponseType.Items.Add(new ListItem(responseType.Text, responseType.ID.ToString()));
            }
        }
                
        /// <summary>
        /// populate the CSMTypesDDL
        /// </summary>
        private void populateCSMTypesDDL()
        {
            //clear ddl
            this.ddlCSMType.Items.Clear();
            //get list of statuses from bll
            var tList = adminBLL.getAllActiveCSMTypes();
            foreach (var type in tList)
            {
                ddlCSMType.Items.Add(new ListItem(type.Text, type.ID.ToString()));
            }
        }
                
        /// <summary>
        /// UNFINISHED
        /// </summary>
        private void populateSectionsDDL()
        {
            //clear ddl
            this.ddlCSMNameSection.Items.Clear();
            //get list of statuses from bll
            var sectionList = adminBLL.getAllSections();
            foreach (var section in sectionList)
            {
                ddlCSMNameSection.Items.Add(new ListItem(section.Abbreviation, section.ID.ToString()));
            }
        }
                
        /// <summary>
        /// get business rule name from client
        /// </summary>
        /// <returns>string</returns>
        private string getCSMNameFromClient()
        {
            string CSMName;
            //get updated name from textboxes
            string name1 = txtCSMName1.Text;
            //string name2 = ddlCSMNameSection.SelectedItem.Text;
            int sectionID = Convert.ToInt32(this.hdnSectionID.Value);
            viewSection section = adminBLL.getSectionByID(sectionID);
            string name2 = section.Abbreviation;
            string name3 = txtCSMName3.Text;
            string name4 = txtCSMName4.Text;
            StringBuilder editName = new StringBuilder();
            editName.Append(name1);
            editName.Append(".");
            editName.Append(name2);
            editName.Append(".");
            editName.Append(name3);
            editName.Append(".");
            editName.Append(name4);
            CSMName = editName.ToString();
            return CSMName;
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

        #endregion ui controls

        #region web methods
                
        /// <summary>
        /// fetches csm text by id for read-only display on client
        /// </summary>
        /// <param name="projID">string</param>
        /// <param name="csmID">string</param>
        /// <returns>string</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetCSMTextByID(string projID, string csmID)
        {
            StringBuilder csmText = new StringBuilder();
            int projectID = Convert.ToInt32(projID);
            int messageID = Convert.ToInt32(csmID);
            var bll = new CustomerServiceMessagesBLL(projectID);
            var csm = bll.getCSMByID(messageID);
            csmText.Append("<b>");
            csmText.Append(csm.Name);
            csmText.Append("</b>");
            csmText.Append("<br />");
            csmText.Append(csm.Text);
            bll = null;
            return csmText.ToString();
        }
                
        /// <summary>
        /// fetches test cases from database for tree
        /// </summary>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<JSTree_Node> GetCSMsForTree(string nodeID, string projID)
        {
            int projectID = Convert.ToInt32(projID);
            var adminBLL = new AdminBLL(projectID);
            List<JSTree_Node> nodes = new List<JSTree_Node>();
            if (!nodeID.Equals("0"))
            {
                var idArray = nodeID.Split('_');
                var objectAbbv = idArray[0];
                int objectID = 0;
                bool result = int.TryParse(idArray[1], out objectID);
                var bll = new TestCasesBLL(projectID);
                switch (objectAbbv)
                {
                    case ("SEC"):
                        //create nodes for all test cases associated with section id
                        nodes = AddCSMChildNodes(projectID, objectID);
                        break;
                }
            }
            else // screen is loading; populate root node with sections
            {
                JSTree_Node rootNode = new JSTree_Node();
                var sectionList = adminBLL.getAllSections();
                rootNode.data = new JsTreeNodeData { title = "Sections" };
                if (sectionList.Count > 0)
                {
                    rootNode.state = "open";
                    rootNode.IdServerUse = 0;
                    rootNode.attr = new JsTreeAttribute { id = "ROOT", selected = false };
                    rootNode.children = AddSectionChildNodes(projectID, sectionList).ToArray();
                }
                nodes.Add(rootNode);
                adminBLL = null;
            }
            return nodes;
        }
                
        /// <summary>
        /// add child nodes to a tree
        /// </summary>
        /// <param name="projectID">int</param>
        /// <param name="sectionList">List&lt;viewSection&gt;</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        private static List<JSTree_Node> AddSectionChildNodes(int projectID, List<viewSection> sectionList)
        {
            var bll = new CustomerServiceMessagesBLL(projectID);
            List<JSTree_Node> JSTreeArray = new List<JSTree_Node>();
            foreach (var section in sectionList)
            {
                var hasCSM = bll.hasCSM(section.ID);
                int CurrChildId = section.ID;
                JSTree_Node jsTreeNode = new JSTree_Node();
                jsTreeNode.data = new JsTreeNodeData { title = section.Text };
                if (hasCSM)
                {
                    jsTreeNode.state = "closed";  //For async to work
                }
                jsTreeNode.IdServerUse = CurrChildId;
                jsTreeNode.attr = new JsTreeAttribute { id = "SEC_" + CurrChildId.ToString(), type = "Section", selected = false };
                JSTreeArray.Add(jsTreeNode);
            }
            bll = null;
            return JSTreeArray;
        }
                
        /// <summary>
        /// add child nodes to a tree
        /// </summary>
        /// <param name="projectID">int</param>
        /// <param name="sectionID">int</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        private static List<JSTree_Node> AddCSMChildNodes(int projectID, int sectionID)
        {
            var bll = new CustomerServiceMessagesBLL(projectID);
            var csmList = bll.getAllCSMsBySection(sectionID);
            List<JSTree_Node> JSTreeArray = new List<JSTree_Node>();
            foreach (var csm in csmList)
            {
                int CurrChildId = csm.ID;
                JSTree_Node jsTreeNode = new JSTree_Node();
                jsTreeNode.data = new JsTreeNodeData { title = csm.Name };
                jsTreeNode.IdServerUse = CurrChildId;
                jsTreeNode.children = null;
                jsTreeNode.attr = new JsTreeAttribute { id = "CSM_" + CurrChildId.ToString(), type = "CSM", selected = false };
                JSTreeArray.Add(jsTreeNode);
            }
            bll = null;
            return JSTreeArray;
        }
                
        /// <summary>
        /// get next number by section
        /// </summary>
        /// <param name="secID">string</param>
        /// <param name="projID">string</param>
        /// <returns>string</returns>
        [WebMethod]
        public static string getNextCSMNumberBySection(string secID, string projID)
        {
            int projectID = Convert.ToInt32(projID);
            int sectionID = Convert.ToInt32(secID);
            var bll = new CustomerServiceMessagesBLL(projectID);
            var csmList = bll.getAllCSMsBySection(sectionID);
            int nextNumber = 1;
            //is next number already used in the name?
            bool nextNumberUsed = true;
            while (nextNumberUsed)
            {
                bool foundInList = false;
                foreach (var csm in csmList)
                {
                    string latestNum = csm.Name.Substring(csm.Name.LastIndexOf(".") + 1);
                    if(latestNum.Equals(nextNumber.ToString())) {
                        foundInList = true;
                    }
                }
                if(foundInList) { 
                    nextNumberUsed = true;
                    nextNumber++;
                }
                else { 
                    nextNumberUsed = false; 
                }
            }
            csmList = null;
            return nextNumber.ToString(); ;
        }
                
        /// <summary>
        /// strip HTML from text
        /// </summary>
        /// <param name="text">string</param>
        /// <returns>string</returns>
        private static string stripHTMLFromText(string text)
        {
            string cleanText = text;
            bool moreHTMLTags = true;
            int indexOfOpenBracket = 0;
            int indexOfCloseBracket = 0;
            while (moreHTMLTags)
            {
                indexOfOpenBracket = cleanText.IndexOf("<");
                if (indexOfOpenBracket == -1)
                {
                    moreHTMLTags = false;
                }
                else
                {
                    indexOfCloseBracket = cleanText.IndexOf(">", indexOfOpenBracket);
                    if (indexOfCloseBracket == -1)
                    {
                        moreHTMLTags = false;
                    }
                    else
                    {
                        cleanText = cleanText.Remove(indexOfOpenBracket, (indexOfCloseBracket - indexOfOpenBracket) + 1);
                    }
                }
            }
            cleanText = cleanText.Replace("&nbsp;", " ");
            cleanText = cleanText.Replace("\r\n", " ");
            return cleanText;
        }

        #endregion web methods

        #region private methods

        #endregion private methods

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
            lblNoResults.Text = "";
            setClientMode("");
        }

        /// <summary>
        /// event handler for seach button click for a basic search
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnBasicSearch_Click(object sender, EventArgs e)
        {
            //this.lblNoResults.Text = "";
            //string searchValue = this.txtBasicSearch.Text;
            //var searchBLL = new SearchBLL(this.projectID);
            //var searchResultIDs = searchBLL.doBasicSearch(searchValue, (int)Enums.searchObjectTypeEnums.SearchObjectType.Defects);
            //this.grdDefects.DataSource = searchBLL.getDefectsForSearchResults(searchResultIDs);
            //this.grdDefects.DataBind();
            //if (searchResultIDs.Count == 0)
            //{
            //    this.lblNoResults.Text = "No Results Found.";
            //}
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
        /// fetch data type for search object
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
        /// <returns>List&lt;int&gt;</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<int> getAdvancedSearchResults(List<ClientSearchData> data, string projID, string searchType)
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
            var searchResult = bll.doAdvancedSearch(search, searchTypeID);
            bll = null;
            return searchResult;
        }

        /// <summary>
        /// fetch basic search results
        /// </summary>
        /// <param name="data">List&lt;ClientSearchData&gt;</param>
        /// <param name="projID">string</param>
        /// <param name="searchType">string</param>
        /// <returns>List&lt;int&gt;</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<int> getBasicSearchResults(List<ClientSearchData> data, string projID, string searchType)
        {
            int projectID = 0;
            int searchTypeID = 0;
            bool parseResult = int.TryParse(projID, out projectID);
            parseResult = int.TryParse(searchType, out searchTypeID);
            SearchBLL bll = new SearchBLL(projectID);
            SearchEntities search = new SearchEntities();
            var value = data[0].textValue;
            var searchResult = bll.doBasicSearch(value, searchTypeID);
            bll = null;
            //convert List<SearchResult> into List<int>
            var result = new List<int>();
            foreach (var item in searchResult)
            {
                result.Add(item.ID);
            }
            return result;
        }

        /// <summary>
        /// fetch searh results for tree
        /// </summary>
        /// <param name="searchResult">List&lt;int&gt;</param>
        /// <param name="projID">string</param>
        /// <param name="searchType">string</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<JSTree_Node> getSearchResultsForTree(List<int> searchResult, string projID, string searchType)
        {
            int projectID = 0;
            int searchTypeID = 0;
            bool result = int.TryParse(projID, out projectID);
            result = int.TryParse(searchType, out searchTypeID);
            List<JSTree_Node> nodes = new List<JSTree_Node>();
            //create business rule results tree
            JSTree_Node rootNode = new JSTree_Node();
            string rootTitle = searchResult.Count.ToString() + " Search Results";
            rootNode.data = new JsTreeNodeData { title = rootTitle };
            if (searchResult.Count > 0)
            {
                rootNode.state = "open";
                rootNode.children = AddSearchResultChildNodes(projectID, searchResult).ToArray();
            }
            rootNode.IdServerUse = 0;
            rootNode.attr = new JsTreeAttribute { id = "ROOT", selected = false };
            nodes.Add(rootNode);
            return nodes;
        }
                
        /// <summary>
        /// add search result nodes to a tree
        /// </summary>
        /// <param name="projectID">int</param>
        /// <param name="searchResult">List&lt;int&gt;</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        private static List<JSTree_Node> AddSearchResultChildNodes(int projectID, List<int> searchResult)
        {
            var bll = new CustomerServiceMessagesBLL(projectID);
            var csmList = new List<viewCustomerServiceMessage>();
            foreach (var id in searchResult)
            {
                csmList.Add(bll.getCSMByID(id));
            }
            List<JSTree_Node> JSTreeArray = new List<JSTree_Node>();
            foreach (var csm in csmList)
            {
                int CurrChildId = csm.ID;
                JSTree_Node jsTreeNode = new JSTree_Node();
                jsTreeNode.data = new JsTreeNodeData { title = csm.Name };
                jsTreeNode.IdServerUse = CurrChildId;
                jsTreeNode.children = null;
                jsTreeNode.attr = new JsTreeAttribute { id = "CSM_" + CurrChildId.ToString(), type = "CustomerServiceMessage", selected = false };
                JSTreeArray.Add(jsTreeNode);
            }
            bll = null;
            return JSTreeArray;
        }

        #endregion web methods

        #endregion search

    }
}