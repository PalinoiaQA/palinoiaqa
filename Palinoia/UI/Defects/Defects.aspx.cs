using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Entities;
using System.Text;
using Enums;
using System.Web.Services;
using System.Web.Script.Services;
using Newtonsoft.Json;

namespace Palinoia.UI.Defects
{
    /// <summary>
    /// class to hold code for Defects
    /// </summary>
    public partial class Defects : basePalinoiaPage
    {
        #region properties and variables

        DefectsBLL defectsBLL;
        AdminBLL adminBLL;
        SearchBLL searchBLL;
        int searchType;
        int userID;
        int projectID;
        int defectID;
        bool disableDelete;

        #endregion properties and variables

        #region page lifecycle events

        /// <summary>
        /// initializes a page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            this.addJavaScriptReference("Defects/Defects.js");
            this.addCookieReference();
            this.addSearchReference();
            
        }
        /// <summary>
        /// load a page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            
            userID = Convert.ToInt32(Session.Contents["userID"]);
            projectID = Convert.ToInt32(Session.Contents["projectID"]);
            this.hdnProjectID.Value = projectID.ToString();
            defectsBLL = new DefectsBLL(projectID);
            adminBLL = new AdminBLL(projectID);
            searchBLL = new SearchBLL(projectID);
            this.searchType = (int)Enums.searchObjectTypeEnums.SearchObjectType.Defects;
            this.hdnSearchTypeID.Value = this.searchType.ToString();
            this.btnGridAdvancedSearch.Visible = true;
            this.btnTreeAdvancedSearch.Visible = false;
            //set id textbox to ready only
            this.txtID.Attributes.Add("readonly", "true");
            populateAdvancedSearchControls();
            if (!IsPostBack)
            {
                applyFeatures(userID);
                populateDefectsGrid(userID);
                populateDefectDetailsDDLs();
            }
        }

        #endregion page lifecycle events

        #region event handlers

        /// <summary>
        /// UNFINISHED
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewRowEventArgs</param>
        protected void grdDefects_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var defectsBLL = new AdminBLL(this.projectID);
            var appBLL = new applicationBLL();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var statusID = 0;
                bool parseSuccess = int.TryParse(e.Row.Cells[1].Text, out statusID);
                if (parseSuccess)
                {
                    var status = defectsBLL.getDefectStatusByID(statusID);
                    e.Row.Cells[1].Text = status.Text;
                }
                var typeID = 0;
                parseSuccess = int.TryParse(e.Row.Cells[2].Text, out typeID);
                if (parseSuccess)
                {
                    var type = defectsBLL.getDefectTypeByID(typeID);
                    e.Row.Cells[2].Text = type.Text;
                }
                var userID = 0;
                parseSuccess = int.TryParse(e.Row.Cells[3].Text, out userID);
                if (parseSuccess)
                {
                    var user = appBLL.getUserByID(userID);
                    e.Row.Cells[3].Text = user.FirstName + " " + user.LastName;
                }
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
                            button.OnClientClick = "return confirm('Are you sure you want to delete this defect?');";
                            if (disableDelete)
                            {
                                button.Visible = false;
                            }
                        }
                    }
                }
                defectsBLL = null;
                appBLL = null;
            }
        }

        /// <summary>
        /// handles events when row is deleted from grid
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewDeleteEventArgs</param>
        protected void grdDefects_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            this.defectID = 0;
            var success = int.TryParse(this.grdDefects.Rows[e.RowIndex].Cells[0].Text, out defectID);
            if (defectID > 0)
            {
                string result = defectsBLL.deleteDefectByID(defectID, userID);
                if (!result.Equals("OK"))
                {
                    sendMessageToClient(result);
                }
                else
                {
                    populateDefectsGrid();
                }
            }
        }

        /// <summary>
        /// envent handler for clicking the view link in the defects grid
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewEditEventArgs</param>
        protected void grdDefects_RowEditing(object sender, GridViewEditEventArgs e)
        {
            this.defectID = 0;
            var success = int.TryParse(this.grdDefects.Rows[e.NewEditIndex].Cells[0].Text, out defectID);
            if (defectID > 0)
            {
                this.hdnDefectID.Value = defectID.ToString();
                var defect = defectsBLL.getDefectByID(defectID);
                populateDefectDetails(defect);
                displayDetailsOnClient("edit");
            }
            //prevent further asp gridview events for editing
            e.Cancel = true;
        }

        /// <summary>
        /// event handler for save button click on defect details div
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            var valid = "";
            int defectID = 0;
            bool result = int.TryParse(this.hdnDefectID.Value, out defectID);
            var defectName = txtName.Text;
            var defectDescription = this.CKEditor1.Text;
            int defectStatusID = 0;
            result = int.TryParse(ddlStatus.SelectedValue, out defectStatusID);
            int defectTypeID = 0;
            result = int.TryParse(ddltype.SelectedValue, out defectTypeID);
            int defectPriorityID = 0;
            result = int.TryParse(ddlPriority.SelectedValue, out defectPriorityID);
            if (!this.hdnDateCompleted.Value.Equals(""))
            {
                var dateCompleted = Convert.ToDateTime(this.hdnDateCompleted.Value);
            }
            int ownerID = 0;
            result = int.TryParse(this.ddlOwner.SelectedValue, out ownerID);

            if (defectID == 0)
            {
                //create new defect record
                var newDefect = new viewDefect(0, defectPriorityID, defectStatusID, defectTypeID, ownerID,
                                               defectName, defectDescription, DateTime.Now.ToString("MM/dd/yyyy"), false);
                valid = validateDefect(newDefect);
                if (valid.Equals("valid"))
                {
                    int newID = defectsBLL.addNewDefect(newDefect, userID);
                    this.hdnDefectID.Value = newID.ToString();
                    newDefect = defectsBLL.getDefectByID(newID);
                    populateDefectDetails(newDefect);
                }
                else
                {
                    sendMessageToClient(valid);
                }
                
            }
            else
            {
                var dateCreated = this.hdnDateCreated.Value;
                var dateCompleted = this.hdnDateCompleted.Value;
                //edit existing record
                var editDefect = new viewDefect(defectID, defectPriorityID, defectStatusID, defectTypeID, ownerID,
                                               defectName, defectDescription, dateCreated, dateCompleted, false);
                valid = validateDefect(editDefect);
                if (valid.Equals("valid"))
                {
                    string saveResult = defectsBLL.updateDefect(editDefect, userID);
                    if (!saveResult.Equals("OK"))
                    {
                        sendMessageToClient(saveResult);
                    }
                    else
                    {
                        populateDefectDetails(editDefect);
                    }
                }
                else
                {
                    sendMessageToClient(valid);
                }
            }
            populateDefectsGrid();
            displayDetailsOnClient("edit");
        }

        /// <summary>
        /// handles events when save comment button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnSaveComment_Click(object sender, EventArgs e)
        {
            var newComment = this.txtComment.Text;
            var validationMessage = "";
            var appBLL = new applicationBLL();
            if (newComment.Length == 0)
            {
                validationMessage = "Comment text is required! Comment not saved.";
            }
            else if (appBLL.isValidText(newComment))
            {
                int defectID = 0;
                var result = int.TryParse(this.hdnDefectID.Value, out defectID);
                var comment = new viewComment(0, defectID, this.userID, DateTime.Now, newComment);
                var saveResult = defectsBLL.addComment(this.projectID, comment);
                var defect = defectsBLL.getDefectByID(defectID);
                if (!saveResult.Equals("OK"))
                {
                    sendMessageToClient(saveResult);
                }
                populateDefectDetails(defect);
                displayDetailsOnClient("edit");
            }
            else
            {
                validationMessage = "Invalid text!  Comment not saved.";
            }
            if (validationMessage.Length > 0)
            {
                sendMessageToClient(validationMessage);
            }
        }
                
        /// <summary>
        /// handles events caused by clicking Close button
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnClose_Click(object sender, EventArgs e)
        {
            //clearBREditControls();
            setClientMode("");
        }

        #endregion event handlers

        #region private methods

        private void applyFeatures(int userID)
        {
            applicationBLL palinoiaBLL = new applicationBLL();
            viewUser user = palinoiaBLL.getUserByID(userID);
            List<viewFeature> userFeatureList = palinoiaBLL.getFeaturesForUser(user);
            //VIEW
            viewFeature feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.DefectsView);
            if (feature != null)
            {

            }
            else
            {
                //user cannot view screen.  redirect to default display
                sendMessageToClient("You do not have the necessary permission to view this screen.");
                this.grdDefects.Visible = false;
                this.divSearch.Visible = false;
            }
            //ADD
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.DefectsAdd);
            if (feature != null)
            {
                this.btnAddDefect.Visible = true;
            }
            else
            {
                this.btnAddDefect.Visible = false;
            }
            //EDIT
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.DefectsEdit);
            if (feature != null)
            {
                this.btnSave.Visible = true;
            }
            else
            {
                this.btnSave.Visible = false;
            }
            //DELETE
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.DefectsDelete);
            if (feature != null)
            {
                disableDelete = false;
            }
            else
            {
                disableDelete = true;
            }
        }

        private void populateDefectsGrid()
        {
            //clear grid
            this.grdDefects.DataSource = null;
            this.grdDefects.DataBind();
            //get open defects from db sorted by created date descending
            var defectList = defectsBLL.getAllOpenDefects();
            //populate grid with list
            this.grdDefects.DataSource = defectList;
            this.grdDefects.DataBind();
        }

        private void populateDefectsGrid(int userID)
        {
            //get user name
            var appBLL = new applicationBLL();
            string searchValue = appBLL.getUserByID(userID).getFullNameFNF();
            //clear grid
            this.grdDefects.DataSource = null;
            this.grdDefects.DataBind();
            //show defects assigned to current user
            SearchEntities search = new SearchEntities();
            SearchRow row = new SearchRow("6", "1", userID.ToString(), ""); // 6 = ownerID, 1 = "="
            search.searchList.Add(row);
            var searchResultIDs = searchBLL.doAdvancedSearch(search, 3);
            
            this.grdDefects.DataSource = searchBLL.getDefectsForSearchResults(searchResultIDs);
            this.grdDefects.DataBind();
            if (searchResultIDs.Count == 0)
            {
                this.lblNoResults.Text = "No Results Found.";
            }
        }

        private void populateDefectDetailsDDLs() {
            populateOwnersDDL();
            populatePriorityDDL();
            populateStatusDDL();
            populateTypeDDL();
        }

        private void populateOwnersDDL(viewDefect defect)
        {
            //clear ddl
            this.ddlOwner.Items.Clear();
            var appBLL = new applicationBLL();
            var userList = new List<Entities.viewUser>();
            userList = appBLL.getAllUsers();
            int indexCounter = 0;
            this.ddlOwner.Items.Add(new ListItem("Select One", "0"));
            foreach (var user in userList)
            {
                indexCounter++;
                this.ddlOwner.Items.Add(new ListItem(user.getFullNameLNF(), user.ID.ToString()));
                if (user.ID == defect.OwnerID)
                {
                    this.ddlOwner.SelectedIndex = indexCounter;
                }
            }
        }

        private void populateOwnersDDL() {
            //clear ddl
            this.ddlOwner.Items.Clear();
            var appBLL = new applicationBLL();
            var userList = new List<Entities.viewUser>();
            userList = appBLL.getAllUsers();
            this.ddlOwner.Items.Add(new ListItem("Select One", "0"));
            foreach (var user in userList)
            {
                this.ddlOwner.Items.Add(new ListItem(user.getFullNameLNF(), user.ID.ToString()));
            }
        }

        private void populateStatusDDL(viewDefect defect)
        {
            this.ddlStatus.Items.Clear();
            var statusList = adminBLL.getAllDefectStatus();
            var indexCounter = 0;
            this.ddlStatus.Items.Add(new ListItem("Select One", "0"));
            foreach (var status in statusList)
            {
                indexCounter++;
                this.ddlStatus.Items.Add(new ListItem(status.Text, status.ID.ToString()));
                if (status.ID == defect.DefectStatusID)
                {
                    this.ddlStatus.SelectedIndex = indexCounter;
                }
            }
        }

        private void populateStatusDDL()
        {
            this.ddlStatus.Items.Clear();
            var statusList = adminBLL.getAllDefectStatus();
            this.ddlStatus.Items.Add(new ListItem("Select One", "0"));
            foreach (var status in statusList)
            {
                this.ddlStatus.Items.Add(new ListItem(status.Text, status.ID.ToString()));
            }
        }

        private void populateTypeDDL(viewDefect defect)
        {
            this.ddltype.Items.Clear();
            var typeList = adminBLL.getAllDefectTypes();
            var indexCounter = 0;
            this.ddltype.Items.Add(new ListItem("Select One", "0"));
            foreach (var type in typeList)
            {
                indexCounter++;
                this.ddltype.Items.Add(new ListItem(type.Text, type.ID.ToString()));
                if(type.ID == defect.DefectTypeID) {
                    this.ddltype.SelectedIndex = indexCounter;
                }
            }
        }

        private void populateTypeDDL()
        {
            this.ddltype.Items.Clear();
            var typeList = adminBLL.getAllDefectTypes();
            this.ddltype.Items.Add(new ListItem("Select One", "0"));
            foreach (var type in typeList)
            {
                this.ddltype.Items.Add(new ListItem(type.Text, type.ID.ToString()));
            }
        }

        private void populatePriorityDDL(viewDefect defect)
        {
            this.ddlPriority.Items.Clear();
            var priorityList = adminBLL.getAllDefectPriorities();
            var indexCounter = 0;
            this.ddlPriority.Items.Add(new ListItem("Select One", "0"));
            foreach (var priority in priorityList)
            {
                indexCounter++;
                this.ddlPriority.Items.Add(new ListItem(priority.Text, priority.ID.ToString()));
                if (priority.ID == defect.DefectPriorityID)
                {
                    this.ddlPriority.SelectedIndex = indexCounter;
                }
            }
        }

        private void populatePriorityDDL()
        {
            this.ddlPriority.Items.Clear();
            var priorityList = adminBLL.getAllDefectPriorities();
            this.ddlPriority.Items.Add(new ListItem("Select One", "0"));
            foreach (var priority in priorityList)
            {
                this.ddlPriority.Items.Add(new ListItem(priority.Text, priority.ID.ToString()));
            }
        }

        private void populateComments(viewDefect defect)
        {
            var comments = defectsBLL.getCommentsForDefect(defect.ID);
            StringBuilder sb = new StringBuilder();
            foreach (var comment in comments)
            {
                sb.AppendLine(comment.Text);
                sb.AppendLine("<br /><br />");
            }
            this.divComments.InnerHtml = sb.ToString();
        }

        private void populateDefectDetails(viewDefect defect)
        {
            populateOwnersDDL(defect);
            populateStatusDDL(defect);
            populatePriorityDDL(defect);
            populateTypeDDL(defect);
            this.txtID.Text = defect.ID.ToString();
            this.txtName.Text = defect.Name;
            this.CKEditor1.Text = defect.Description;
            if (defect.DateCompleted != null)
            {
                if (defect.DateCompleted.Equals("EMPTY"))
                {
                    this.hdnDateCompleted.Value = "";
                }
                else
                {
                    this.hdnDateCompleted.Value = defect.DateCompleted;
                }
            }
            this.hdnDateCreated.Value = defect.DateCreated;
            populateComments(defect);
            displayDetailsOnClient("edit");
        }

        private void displayDetailsOnClient(string mode)
        {
            // Define the name and type of the client script on the page.
            String csName = "showDefectDetails";
            Type csType = this.GetType();
            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs = Page.ClientScript;
            // Check to see if the client script is already registered.
            if (!cs.IsStartupScriptRegistered(csType, csName))
            {
                //ClientScript.RegisterStartupScript(typeof(Page), csName, "$(document).ready(function() {showDefectDetails(\"" + visible + "\");});", true);
                ClientScript.RegisterStartupScript(typeof(Page), csName, "$(document).ready(function() {setMode(\"" + mode + "\");});", true);
            }
        }

        private string validateDefect(viewDefect defect)
        {
            string retVal = "valid";
            if (defect.Name.Length == 0)
            {
                retVal = "Defect name is required.";
            }
            else if (defect.Description.Length == 0)
            {
                retVal = "Defect description is required.";
            }
            else if (defect.OwnerID == 0)
            {
                retVal = "Defect owner is required.";
            }
            else if(defect.DefectPriorityID == 0) {
                retVal = "Defect priority is required.";
            }
            else if (defect.DefectTypeID == 0)
            {
                retVal = "Defect type is required.";
            }
            else if (defect.DefectStatusID == 0)
            {
                retVal = "Defect status is required.";
            }
            else if (validateInputText(defect.Name) == false)
            {
                retVal = "Invalid text in defect name.";
            }
            //Can't validate description text because it contains HTML
            //else if (validateInputText(defect.Description) == false)
            //{
            //    retVal = "Invalid text in defect description.";
            //}
            return retVal;
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
            userID = Convert.ToInt32(Session.Contents["userID"]);
            populateDefectsGrid(userID);
        }

        /// <summary>
        /// event handler for Clear button.  Clears all advanced search textboxes and
        /// ddls and reloads defects grid with full defect list
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        //protected void btnClearAdvancedSearch_Click(object sender, EventArgs e)
        //{
        //    lblNoResults.Text = "";
        //    this.txtValue1.Text = "";
        //    this.txtValue2.Text = "";
        //    this.txtValue3.Text = "";
        //    this.txtValue4.Text = "";
        //    this.ddlConnector2.Items[0].Selected = true;
        //    this.ddlConnector3.Items[0].Selected = true;
        //    this.ddlConnector4.Items[0].Selected = true;
        //    //clear hidden fields for advanced search info
        //    this.hdnSearchObjectDDL1.Value = "";
        //    this.hdnSearchObjectDDL2.Value = "";
        //    this.hdnSearchObjectDDL3.Value = "";
        //    this.hdnSearchObjectDDL4.Value = "";
        //    this.hdnOperator1.Value = "";
        //    this.hdnOperator2.Value = "";
        //    this.hdnOperator3.Value = "";
        //    this.hdnOperator4.Value = "";
        //    this.hdnConnector2.Value = "";
        //    this.hdnConnector3.Value = "";
        //    this.hdnConnector4.Value = "";
        //    //repopulate search controls and search results
        //    populateAdvancedSearchControls();
        //    populateDefectsGrid();
        //}

        /// <summary>
        /// event handler for seach button click for a basic search
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnBasicSearch_Click(object sender, EventArgs e)
        {
            this.lblNoResults.Text = "";
            string searchValue = this.txtBasicSearch.Text;
            if (searchValue.Length > 0)
            {
                var searchBLL = new SearchBLL(this.projectID);
                var searchResultIDs = searchBLL.doBasicSearch(searchValue, (int)Enums.searchObjectTypeEnums.SearchObjectType.Defects);
                this.grdDefects.DataSource = searchBLL.getDefectsForSearchResults(searchResultIDs);
                this.grdDefects.DataBind();
                if (searchResultIDs.Count == 0)
                {
                    this.lblNoResults.Text = "No Results Found.";
                }
            }
            else
            {
                //search value is null.  show all open defects
                populateDefectsGrid();
            }
        }

        /// <summary>
        /// event handler for search button click for an advanced search 
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnGridAdvancedSearch_Click(object sender, EventArgs e)
        {
            //clear results grid
            this.grdDefects.DataSource = null;
            this.grdDefects.DataBind();
            var error = false;
            var advSearch = new SearchEntities();
            //pull data from search rows
            var searchObjectID = this.hdnSearchObjectDDL1.Value;
            var searchOperator = this.hdnOperator1.Value;
            var dataType = this.hdnDataType1.Value;
            var value = "";
            switch (dataType)
            {
                case("1"):
                    value = this.hdnDDLValue1.Value;
                    break;
                case("2"):
                case("0"):
                    value = this.hdnTextValue1.Value;
                    break;
                case("3"):
                    value = this.hdnCalendarValue1.Value;
                    break;
                case("4"):
                    value = this.hdnDDLValue1.Value;
                    if (value.Equals("1"))
                    {
                        value = "true";
                    }
                    else
                    {
                        value = "false";
                    }
                    break;
            }
            
          
            var connector = this.hdnConnector2.Value;
            if (isValidSearchRow(searchObjectID, searchOperator, value))
            {
                var row = new SearchRow(searchObjectID, searchOperator, value, connector);
                advSearch.addSearchRow(row);
            }
            else
            {
                error = true;
                sendMessageToClient("Row 1: Search Object, Operator, and Search Value are all required.");
            }
            // row 2
            if (!connector.Equals("0"))
            {
                //pull data from search rows
                searchObjectID = this.hdnSearchObjectDDL2.Value;
                searchOperator = this.hdnOperator2.Value;
                dataType = this.hdnDataType2.Value;
                value = "";
                switch (dataType)
                {
                    case ("1"):
                        value = this.hdnDDLValue2.Value;
                        break;
                    case ("2"):
                    case ("0"):
                        value = this.hdnTextValue2.Value;
                        break;
                    case ("3"):
                        value = this.hdnCalendarValue2.Value;
                        break;
                    case ("4"):
                        value = this.hdnDDLValue2.Value;
                        if (value.Equals("1"))
                        {
                            value = "true";
                        }
                        else
                        {
                            value = "false";
                        }
                        break;
                }
                connector = this.hdnConnector3.Value;
                if (isValidSearchRow(searchObjectID, searchOperator, value))
                {
                    var row = new SearchRow(searchObjectID, searchOperator, value, connector);
                    advSearch.addSearchRow(row);
                }
                else
                {
                    error = true;
                    sendMessageToClient("Row 2: Search Object, Operator, and Search Value are all required.");
                }
            }
            // row 3
            if (!connector.Equals("0"))
            {
                if (!connector.Equals("0"))
                {
                    //pull data from search rows
                    searchObjectID = this.hdnSearchObjectDDL3.Value;
                    searchOperator = this.hdnOperator3.Value;
                    dataType = this.hdnDataType3.Value;
                    value = "";
                    switch (dataType)
                    {
                        case ("1"):
                            value = this.hdnDDLValue3.Value;
                            break;
                        case ("2"):
                        case ("0"):
                            value = this.hdnTextValue3.Value;
                            break;
                        case ("3"):
                            value = this.hdnCalendarValue3.Value;
                            break;
                        case ("4"):
                            value = this.hdnDDLValue3.Value;
                            if (value.Equals("1"))
                            {
                                value = "true";
                            }
                            else
                            {
                                value = "false";
                            }
                            break;
                    }
                    connector = this.hdnConnector4.Value;
                    if (isValidSearchRow(searchObjectID, searchOperator, value))
                    {
                        var row = new SearchRow(searchObjectID, searchOperator, value, connector);
                        advSearch.addSearchRow(row);
                    }
                    else
                    {
                        error = true;
                        sendMessageToClient("Row 3: Search Object, Operator, and Search Value are all required.");
                    }
                }
            }
                // row 4
            if (!connector.Equals("0"))
            {
                //pull data from search rows
                searchObjectID = this.hdnSearchObjectDDL4.Value;
                searchOperator = this.hdnOperator4.Value;
                dataType = this.hdnDataType4.Value;
                value = "";
                switch (dataType)
                {
                    case ("1"):
                        value = this.hdnDDLValue4.Value;
                        break;
                    case ("2"):
                    case ("0"):
                        value = this.hdnTextValue4.Value;
                        break;
                    case ("3"):
                        value = this.hdnCalendarValue4.Value;
                        break;
                    case ("4"):
                        value = this.hdnDDLValue4.Value;
                        if (value.Equals("1"))
                        {
                            value = "true";
                        }
                        else
                        {
                            value = "false";
                        }
                        break;
                }
                connector = "0";
                if (isValidSearchRow(searchObjectID, searchOperator, value))
                {
                    var row = new SearchRow(searchObjectID, searchOperator, value, connector);
                    advSearch.addSearchRow(row);
                }
                else
                {
                    error = true;
                    sendMessageToClient("Row 4: Search Object, Operator, and Search Value are all required.");
                }
            }
            
            if (!error)
            {
                //UI search info valid, proceed with search
                var searchResultIDs = searchBLL.doAdvancedSearch(advSearch, (int)Enums.searchObjectTypeEnums.SearchObjectType.Defects);
                this.grdDefects.DataSource = searchBLL.getDefectsForSearchResults(searchResultIDs);
                this.grdDefects.DataBind();
                if (searchResultIDs.Count == 0)
                {
                    this.lblNoResults.Text = "No Results Found.";
                }
                populateAdvancedSearchControls();
            }
            
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
            //this.ddlOperator1.Items[0].Selected = true;
            //this.ddlOperator2.Items[0].Selected = true;
            //this.ddlOperator3.Items[0].Selected = true;
            //this.ddlOperator4.Items[0].Selected = true;
            //populate ddl items
            foreach (var item in opList)
            {
                this.ddlOperator1.Items.Add(new ListItem(item.TEXT, item.ID.ToString()));
                this.ddlOperator2.Items.Add(new ListItem(item.TEXT, item.ID.ToString()));
                this.ddlOperator3.Items.Add(new ListItem(item.TEXT, item.ID.ToString()));
                this.ddlOperator4.Items.Add(new ListItem(item.TEXT, item.ID.ToString()));
            }
        }

        private bool isValidSearchRow(string obj, string op, string value) {
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

        #endregion web methods

        

        #endregion search

    }

    
}