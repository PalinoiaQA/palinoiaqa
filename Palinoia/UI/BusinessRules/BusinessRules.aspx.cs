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


namespace Palinoia.UI.BusinessRules
{    
    /// <summary>
    /// class to hold code for BusinessRules
    /// </summary>
    public partial class BusinessRules : basePalinoiaPage
    {
        #region properties and variables

        /// <summary>
        /// class variable for bll
        /// </summary>
        BusinessRulesBLL bll;
        /// <summary>
        /// class variable for adminBLL
        /// </summary>
        AdminBLL adminBLL;
        /// <summary>
        /// class variable for palinoiaBLL
        /// </summary>
        applicationBLL palinoiaBLL;
        /// <summary>
        /// class variable for SearchBLL
        /// </summary>
        SearchBLL searchBLL;
        /// <summary>
        /// class variable for disableEdit
        /// </summary>
        bool disableEdit;
        /// <summary>
        /// class variable for 
        /// </summary>
        bool disableDelete;
        /// <summary>
        /// class variable for  ProjectID
        /// </summary>
        int ProjectID;
        /// <summary>
        /// class variable for userID
        /// </summary>
        int userID;
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
            this.addJavaScriptReference("BusinessRules/BusinessRules.js");
            this.addSearchReference();
            this.addCKEditorReference();
            this.addCookieReference();
        }
                
        /// <summary>
        /// loads the page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScript.GetPostBackEventReference(new PostBackOptions(btnEditBR));
            //check for valid user id
            userID = Convert.ToInt32(Session.Contents["userID"]);
            if (userID > 0)
            {
                //set search properties
                this.searchType = (int)Enums.searchObjectTypeEnums.SearchObjectType.BusinessRules;
                this.hdnSearchTypeID.Value = this.searchType.ToString();
                this.btnGridAdvancedSearch.Visible = false;
                this.btnTreeAdvancedSearch.Visible = true;
                //set page properties
                ProjectID = Convert.ToInt32(Session.Contents["projectID"]);
                this.hdnProjectID.Value = ProjectID.ToString();
                bll = new BusinessRulesBLL(ProjectID);
                adminBLL = new AdminBLL(ProjectID);
                palinoiaBLL = new applicationBLL(ProjectID);
                searchBLL = new SearchBLL(ProjectID);
                populateAdvancedSearchControls();
                applyFeatures(userID);
                if (!IsPostBack)
                {
                    populateSectionsDDL();
                    populateStatusDDL();
                    if (this.hdnBrowserOnly.Value.Equals("0"))
                    {
                        populateDefectOwnerDDL();
                        this.hdnDefectOwnerID.Value = "";
                        this.chkAddDefect.Checked = false;
                    }
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
        /// rowdata event handler for the BusinessRules grid
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewRowEventArgs</param>
        protected void grdBusinessRules_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void btnSaveAddBusinessRule_Click(object sender, EventArgs e)
        {
            string newText = this.CKEditor1.Text;
            string newName = getBusinessRuleNameFromClient();
            int statusID = Convert.ToInt32(ddlBusinessRuleStatus.SelectedItem.Value);
            string status = ddlBusinessRuleStatus.SelectedItem.Text;
            int sectionID = Convert.ToInt32(this.hdnSectionID.Value);
            viewSection vSection = adminBLL.getSectionByID(sectionID);
            string section = vSection.Abbreviation;
            bool active = this.chkActive.Checked;
            //required field check
            if (newText.Length == 0)
            {
                sendMessageToClient("Business Rule text is required.");
            }
            else if (newName.Length == 0)
            {
                sendMessageToClient("Business Rule name is required.");
            }
            else
            {
                viewBusinessRule newBR = new viewBusinessRule(0, newName, statusID, sectionID, newText, active, userID);
                //get defect owner?
                int ownerID = 0;
                if (this.hdnDefectOwnerID.Value != "")
                {
                   bool parseResult = int.TryParse(this.hdnDefectOwnerID.Value, out ownerID);
                }
                //call bll to save view object
                var result = bll.addBusinessRule(newBR, ownerID);
                //alert client if error
                if (!result.Equals("OK"))
                {
                    string error = result;
                    sendMessageToClient("ERROR: " + result);
                    setClientMode("add");
                }
                else
                {
                    clearBREditControls();
                    setClientMode("");
                }
            }
        }
                
        /// <summary>
        /// handles events caused by clicking save edit business rule button
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnSaveEditBusinessRule_Click(object sender, EventArgs e)
        {
            //get id of selected csm reponse type
            int selectedID = Convert.ToInt32(this.hdnBusinessRuleID.Value);
            string editName = getBusinessRuleNameFromClient();
            string editText = this.CKEditor1.Text;
            int editStatusID = Convert.ToInt32(this.ddlBusinessRuleStatus.SelectedValue);
            string editStatus = this.ddlBusinessRuleStatus.SelectedItem.Text;
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
                var editBusinessRule = new viewBusinessRule(selectedID,
                                                            editName.ToString(),
                                                            editStatusID,
                                                            sectionID,
                                                            editText,
                                                            editActive,
                                                            userID);
                //create defect?
                if (this.hdnDefectOwnerID.Value != "")
                {
                    int ownerID = 0;
                    var owner = this.hdnDefectOwnerID.Value;
                    var parseResult = int.TryParse(owner, out ownerID);
                    if (parseResult)
                    {
                        var defectsBLL = new DefectsBLL(this.ProjectID);
                        defectsBLL.createUpdateDefectFromBusinessRule(editBusinessRule, ownerID);
                        defectsBLL = null;
                    }
                }
                //check if name changed
                var originalBR = bll.getBusinessRuleByID(selectedID);
                if (!editBusinessRule.Name.Equals(originalBR.Name))
                {
                    //if name changed, update business rule references in all documents
                    var docBLL = new DocumentsBLL(this.ProjectID);
                    docBLL.renameBusinessRuleInDocuments(originalBR.Name, editBusinessRule.Name);
                }
                //save updated business rule
                string result = bll.updateBusinessRule(editBusinessRule);
                if (!result.Equals("OK"))
                {
                    sendMessageToClient("ERROR: " + result);
                    setClientMode("edit");

                }
                else
                {
                    //hide save controls
                    clearBREditControls();
                    setClientMode("");
                }
            }
        }
                
        /// <summary>
        /// handles events caused by clicking cancel button
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            clearBREditControls();
            setClientMode("");
        }
                
        /// <summary>
        /// handles events when the edit business rule button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnEditBR_Click(object sender, EventArgs e)
        {
            if (!disableEdit)
            {
                int editBusinessRuleID = Convert.ToInt32(this.hdnBusinessRuleID.Value);
                int projectID = Convert.ToInt32(this.hdnProjectID.Value);
                var bll = new BusinessRulesBLL(projectID);
                var br = bll.getBusinessRuleByID(editBusinessRuleID);
                this.CKEditor1.Text = br.Text;
                this.lblBRName.Text = br.Name;
                populateSectionsDDL();
                populateStatusDDL(br.StatusID);
                this.chkActive.Checked = br.Active;
                //populate Text parts
                var nameArray = br.Name.Split('.');
                this.txtBRName1.Text = nameArray[0];
                foreach (ListItem item in ddlBRNameSection.Items)
                {
                    if (item.Text.Equals(nameArray[1]))
                    {
                        item.Selected = true;
                        break;
                    }
                }
                this.txtBRName3.Text = nameArray[2];
                if (nameArray.Length > 3)
                {
                    this.txtBRName4.Text = nameArray[3];
                }
                this.hdnSectionID.Value = br.SectionID.ToString();
                setClientMode("edit");
            }
            else
            {
                setClientMode("");
                sendMessageToClient("You do not have permission to edit business rules");
            }
        }

        /// <summary>
        /// deletes business rule selected in grid
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnDeleteBR_Click(object sender, EventArgs e)
        {
            if (!disableDelete)
            {
                int deleteBusinessRuleID = Convert.ToInt32(this.hdnBusinessRuleID.Value);
                this.hdnBusinessRuleID.Value = "0";
                int projectID = Convert.ToInt32(this.hdnProjectID.Value);
                var bll = new BusinessRulesBLL(projectID);
                string result = bll.deleteBusinessRule(deleteBusinessRuleID, this.userID);
                if (result != "OK")
                {
                    sendMessageToClient(result);
                }
                setClientMode("");
            }
            else
            {
                sendMessageToClient("You do not have permission to delete business rules.");
            }
        }

        #endregion event handlers

        #region web methods
                
        /// <summary>
        /// fetches business rule text by rule id for read-only display on client
        /// </summary>
        /// <param name="projID">string</param>
        /// <param name="brID">string</param>
        /// <returns>string</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetBusinessTextByRuleID(string projID, string brID)
        {
            StringBuilder ruleText = new StringBuilder();
            int projectID = Convert.ToInt32(projID);
            int businessRuleID = Convert.ToInt32(brID);
            var bll = new BusinessRulesBLL(projectID);
            var br = bll.getBusinessRuleByID(businessRuleID);
            ruleText.Append("<b>");
            ruleText.Append(br.Name);
            ruleText.Append("</b>");
            ruleText.Append("<br />");
            ruleText.Append(br.Text);
            bll = null;
            return ruleText.ToString();
        }
                
        /// <summary>
        /// fetches test cases from database for tree
        /// </summary>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<JSTree_Node> GetBusinessRulesForTree(string nodeID, string projID)
        {
            int projectID = Convert.ToInt32(projID);
            var adminBLL = new AdminBLL(projectID);
            List<JSTree_Node> nodes = new List<JSTree_Node>();
            if (!nodeID.Equals("0"))
            {
                var idArray = nodeID.Split('_');
                var objectAbbv = idArray[0];
                int objectID = 0;
                if (idArray != null)
                {
                    bool result = int.TryParse(idArray[1], out objectID);
                }
                var bll = new TestCasesBLL(projectID);
                switch (objectAbbv)
                {
                    case ("SEC"):
                        //create nodes for all test cases associated with section id
                        nodes = AddBusinessRuleChildNodes(projectID, objectID);
                        break;
                }
            }
            else // screen is loading; populate root node with sections
            {
                JSTree_Node rootNode = new JSTree_Node();
                var sectionList = adminBLL.getAllSections();
                rootNode.data = new JsTreeNodeData { title = "Sections" };
                rootNode.state = "open";
                rootNode.IdServerUse = 0;
                rootNode.attr = new JsTreeAttribute { id = "ROOT_0", selected = false };
                rootNode.children = AddSectionChildNodes(projectID, sectionList).ToArray();
                nodes.Add(rootNode);
            }
            adminBLL = null;
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
            var bll = new BusinessRulesBLL(projectID);
            List<JSTree_Node> JSTreeArray = new List<JSTree_Node>();
            foreach (var section in sectionList)
            {
                var hasBusinessRules = bll.hasBusinessRules(section.ID);
                int CurrChildId = section.ID;
                JSTree_Node jsTreeNode = new JSTree_Node();
                jsTreeNode.data = new JsTreeNodeData { title = section.Text };
                if (hasBusinessRules)
                {
                    jsTreeNode.state = "closed";  //For async to work
                }
                jsTreeNode.IdServerUse = CurrChildId;
                //get all business rules for section id
                var brList = bll.getAllBusinessRulesBySection(section.ID);
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
        private static List<JSTree_Node> AddBusinessRuleChildNodes(int projectID, int sectionID)
        {
            var bll = new BusinessRulesBLL(projectID);
            var brList = bll.getAllBusinessRulesBySection(sectionID);
            List<JSTree_Node> JSTreeArray = new List<JSTree_Node>();
            foreach (var br in brList)
            {
                int CurrChildId = br.ID;
                JSTree_Node jsTreeNode = new JSTree_Node();
                jsTreeNode.data = new JsTreeNodeData { title = br.Name };
                jsTreeNode.IdServerUse = CurrChildId;
                jsTreeNode.children = null;
                jsTreeNode.attr = new JsTreeAttribute { id = "BR_" + CurrChildId.ToString(), type = "BusinessRule", selected = false };
                JSTreeArray.Add(jsTreeNode);
            }
            return JSTreeArray;
        }
                
        /// <summary>
        /// add business rule text node
        /// </summary>
        /// <param name="br">viewBusinessRule</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        private static List<JSTree_Node> AddBusinessRuleTextNode(viewBusinessRule br)
        {
            List<JSTree_Node> JSTreeArray = new List<JSTree_Node>();
            JSTree_Node jsTreeNode = new JSTree_Node();
            jsTreeNode.data = new JsTreeNodeData { title = stripHTMLFromText(br.Text) };
            //jsTreeNode.state = "closed";  //For async to work
            jsTreeNode.IdServerUse = br.ID;
            //add root nodes to test case node for BusinessRules and Test Steps
            jsTreeNode.children = null;
            jsTreeNode.attr = new JsTreeAttribute { id = "BRText_" + br.ID.ToString(), type = "BusinessRuleText", selected = false };
            JSTreeArray.Add(jsTreeNode);
            return JSTreeArray;
        }
                
        /// <summary>
        /// get next number by section
        /// </summary>
        /// <param name="secID">string</param>
        /// <param name="projID">string</param>
        /// <returns>string</returns>
        [WebMethod]
        public static string getNextBRNumberBySection(string secID, string projID)
        {
            int projectID = Convert.ToInt32(projID);
            int sectionID = Convert.ToInt32(secID);
            var bll = new BusinessRulesBLL(projectID);
            var brList = bll.getAllBusinessRulesBySection(sectionID);
            int nextNumber = 1;
            //is next number already used in the name?
            bool nextNumberUsed = true;
            while (nextNumberUsed)
            {
                bool foundInList = false;
                foreach (var br in brList)
                {
                    string latestNum = br.Name.Substring(br.Name.LastIndexOf(".") + 1);
                    if (latestNum.Equals(nextNumber.ToString()))
                    {
                        foundInList = true;
                    }
                }
                if (foundInList)
                {
                    nextNumberUsed = true;
                    nextNumber++;
                }
                else
                {
                    nextNumberUsed = false;
                }
            }
            brList = null;
            return nextNumber.ToString(); ;
        }

        /// <summary>
        /// get next available number for subSection by section
        /// </summary>
        /// <param name="secID">string</param>
        /// <param name="subSectID">string</param>
        /// <param name="projID">string</param>
        /// <returns>string</returns>
        [WebMethod]
        public static string getNextBRNumberForSubSection(string secID, string subSectID, string projID)
        {
            int projectID = Convert.ToInt32(projID);
            int sectionID = Convert.ToInt32(secID);
            var bll = new BusinessRulesBLL(projectID);
            var brList = bll.getAllBusinessRulesBySection(sectionID);
            int nextNumber = 1;
            //is next number already used in the name?
            bool nextNumberUsed = true;
            while (nextNumberUsed)
            {
                bool foundInList = false;
                foreach (var br in brList)
                {
                    string parse1, subSectionNum;
                    parse1  = br.Name.Substring(0, br.Name.LastIndexOf("."));
                    subSectionNum = parse1.Substring(parse1.LastIndexOf(".") + 1);
                    string latestNum = br.Name.Substring(br.Name.LastIndexOf(".") + 1);
                    //only test those rules with the same subsection
                    if (subSectionNum.Equals(subSectID))
                    {
                        if (latestNum.Equals(nextNumber.ToString()))
                        {
                            foundInList = true;
                        }
                    }
                }
                if (foundInList)
                {
                    nextNumberUsed = true;
                    nextNumber++;
                }
                else
                {
                    nextNumberUsed = false;
                }
            }
            brList = null;
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
                
        /// <summary>
        /// applies features to a particular user
        /// </summary>
        /// <param name="userID">int</param>
        private void applyFeatures(int userID)
        {
            viewUser user = palinoiaBLL.getUserByID(userID);
            List<viewFeature> userFeatureList = palinoiaBLL.getFeaturesForUser(user);
            //VIEW
            viewFeature feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.BusinessRulesView);
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
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.BusinessRulesAdd);
            if (feature != null)
            {
                this.hdnDisableAdd.Value = "false";
            }
            else
            {
                this.hdnDisableAdd.Value = "true";
            }
            //EDIT
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.BusinessRulesEdit);
            if (feature != null)
            {
                this.hdnDisableEdit.Value = "false";
            }
            else
            {
                this.hdnDisableEdit.Value = "true";
            }
            //DELETE
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.BusinessRulesDelete);
            if (feature != null)
            {
                this.hdnDisableDelete.Value = "false";
            }
            else
            {
                this.hdnDisableDelete.Value = "true";
            }
        }
                
        /// <summary>
        /// reset all business rule edit textboxes and dropdowns in BusinessRules edit section
        /// </summary>
        private void clearBREditControls()
        {
            this.lblBRName.Text = "";
            this.CKEditor1.Text = "";
            this.ddlBusinessRuleStatus.SelectedIndex = 0;
            this.txtBRName1.Text = "";
            this.ddlBRNameSection.SelectedIndex = 0;
            this.txtBRName3.Text = "";
            this.txtBRName4.Text = "";
            this.hdnDefectOwnerID.Value = "";
        }
                
        /// <summary>
        /// populate the status info
        /// </summary>
        private void populateStatusDDL()
        {
            //clear ddl
            ddlBusinessRuleStatus.Items.Clear();
            //get list of statuses from bll
            var statusList = adminBLL.getAllStatuses();
            foreach (var status in statusList)
            {
                ddlBusinessRuleStatus.Items.Add(new ListItem(status.Text, status.ID.ToString()));
            }
        }
                
        /// <summary>
        /// populate the status info with selected item
        /// </summary>
        private void populateStatusDDL(int statusID)
        {
            int itemCounter = 0;
            //clear ddl
            ddlBusinessRuleStatus.Items.Clear();
            //get list of statuses from bll
            var statusList = adminBLL.getAllStatuses();
            foreach (var status in statusList)
            {
                ddlBusinessRuleStatus.Items.Add(new ListItem(status.Text, status.ID.ToString()));
                if (status.ID == statusID)
                {
                    ddlBusinessRuleStatus.Items[itemCounter].Selected = true;
                }
                itemCounter++;
            }
        }

        /// <summary>
        /// populates the drop down list with all users in the DefectOwnerDDL
        /// </summary>
        private void populateDefectOwnerDDL()
        {
            var appBLL = new applicationBLL();
            var userList = appBLL.getAllUsers();
            this.ddlDefectOwner.Items.Clear();
            foreach (var user in userList)
            {
                this.ddlDefectOwner.Items.Add(new ListItem(user.getFullNameLNF(), user.ID.ToString()));
            }
            userList = null;
            appBLL = null;
        }
                
        /// <summary>
        /// UNFINISHED
        /// </summary>
        private void populateSectionsDDL()
        {
            //clear ddl
            this.ddlBRNameSection.Items.Clear();
            //get list of statuses from bll
            var sectionList = adminBLL.getAllActiveSections();
            foreach (var section in sectionList)
            {
                ddlBRNameSection.Items.Add(new ListItem(section.Abbreviation, section.ID.ToString()));
            }
        }
                
        /// <summary>
        /// get business rule name from client
        /// </summary>
        /// <returns>string</returns>
        private string getBusinessRuleNameFromClient()
        {
            string BRName;
            //get updated name from textboxes
            string name1 = txtBRName1.Text;
            //string name2 = ddlBRNameSection.SelectedItem.Text;
            int sectionID = Convert.ToInt32(this.hdnSectionID.Value);
            viewSection section = adminBLL.getSectionByID(sectionID);
            string name2 = section.Abbreviation;
            string name3 = txtBRName3.Text;
            string name4 = txtBRName4.Text;
            StringBuilder editName = new StringBuilder();
            editName.Append(name1);
            editName.Append(".");
            editName.Append(name2);
            editName.Append(".");
            editName.Append(name3);
            editName.Append(".");
            editName.Append(name4);
            BRName = editName.ToString();
            return BRName;
        }
                
        /// <summary>
        /// set client mode
        /// </summary>
        /// <param name="mode">string</param>
        private void setClientMode(string mode)
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
            this.setClientMode("");
        }

        /// <summary>
        /// event handler for Clear button.  Clears all advanced search textboxes and
        /// ddls and reloads defects grid with full defect list
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnClearAdvancedSearch_Click(object sender, EventArgs e)
        {
            lblNoResults.Text = "";
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
        /// fetch search results for tree
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
            var bll = new BusinessRulesBLL(projectID);
            var brList = new List<viewBusinessRule>();
            foreach (var id in searchResult)
            {
                brList.Add(bll.getBusinessRuleByID(id));
            }
            List<JSTree_Node> JSTreeArray = new List<JSTree_Node>();
            foreach (var br in brList)
            {
                int CurrChildId = br.ID;
                JSTree_Node jsTreeNode = new JSTree_Node();
                jsTreeNode.data = new JsTreeNodeData { title = br.Name };
                jsTreeNode.IdServerUse = CurrChildId;
                jsTreeNode.children = null;
                jsTreeNode.attr = new JsTreeAttribute { id = "BR_" + CurrChildId.ToString(), type = "BusinessRule", selected = false };
                JSTreeArray.Add(jsTreeNode);
            }
            bll = null;
            return JSTreeArray;
        }

        #endregion web methods

        #endregion search

    }
}