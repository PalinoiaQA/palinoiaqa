using System;
using System.Collections.Generic;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Entities;
using Enums;
using System.Text;
using Newtonsoft.Json;

namespace Palinoia.UI.TestCases
{
    /// <summary>
    /// class to hold code for TestCases object
    /// </summary>
    public partial class TestCases : Palinoia.UI.basePalinoiaPage
    {
        #region properties and variables

        /// <summary>
        /// class variable for bll
        /// </summary>
        TestCasesBLL bll;
        /// <summary>
        /// class variable for adminBLL
        /// </summary>
        AdminBLL adminBLL;
        /// <summary>
        /// class variable for palinoiaBLL
        /// </summary>
        applicationBLL palinoiaBLL;
        /// <summary>
        /// class variable for disableTSEdit
        /// </summary>
        bool disableTSEdit;
        /// <summary>
        /// class variable for disableTSDelete
        /// </summary>
        bool disableTSDelete;

        /// <summary>
        /// class variable for editTestCaseID
        /// </summary>
        int editTestCaseID;
        /// <summary>
        /// class variable for ProjectID
        /// </summary>
        int ProjectID;
        /// <summary>
        /// class variable for userID
        /// </summary>
        int userID;
        int searchType;
        SearchBLL searchBLL;

        #endregion properties and variables

        #region page lifecycle events
                
        /// <summary>
        /// Page Init Event
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            this.addCookieReference();
            this.addSearchReference();
            this.addJavaScriptReference("TestCases/TestCases.js");
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
                //set search properties
                this.searchType = (int)Enums.searchObjectTypeEnums.SearchObjectType.TestCases;
                this.hdnSearchTypeID.Value = this.searchType.ToString();
                this.btnGridAdvancedSearch.Visible = false;
                this.btnTreeAdvancedSearch.Visible = true;
                //set page properties
                ProjectID = Convert.ToInt32(Session.Contents["projectID"]);
                this.hdnProjectID.Value = ProjectID.ToString();
                adminBLL = new AdminBLL(ProjectID);
                palinoiaBLL = new applicationBLL(ProjectID);
                bll = new TestCasesBLL(ProjectID);
                applyFeatures(userID);
                var strTestCaseID = this.hdnTestCaseID.Value;
                int testCaseID = 0;
                bool result = int.TryParse(strTestCaseID, out testCaseID);
                if (result)
                {
                    this.hdnTestCaseMode.Value = "edit";
                    populateSectionsDDL();
                    if (testCaseID > 0)
                    {
                        var tc = bll.getTestCaseByID(testCaseID);
                        if (tc.TestStatusID == (int)Enums.testStatusEnums.TestStatus.Pass)
                        {
                            this.btnNeedRetest.Visible = true;
                        }
                        else
                        {
                            this.btnNeedRetest.Visible = false;
                        }
                    }
                    else
                    {
                        this.btnNeedRetest.Visible = false;
                    }
                }
                else
                {
                    this.hdnTestCaseMode.Value = "add";
                }
                var strTestStepID = this.hdnTestStepID.Value;
                if (strTestStepID == "")
                {
                    int testStepID = 0;
                    result = int.TryParse(strTestStepID, out testCaseID);
                    if (testStepID > 0)
                    {
                        this.hdnTestStepMode.Value = "edit";
                    }
                    else
                    {
                        this.hdnTestStepMode.Value = "add";
                    }
                }
                searchBLL = new SearchBLL(ProjectID);
                populateAdvancedSearchControls();
                if (!IsPostBack)
                {
                    this.hdnTestCaseID.Value = "0";
                    this.hdnEditPreConditionID.Value = "0";
                    this.hdnEditPostConditionID.Value = "0";
                    populateTestStepsGrid();
                    this.listboxPreConditions.Items.Clear();
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
        /// handles events when need retest button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnNeedRetest_Click(object sender, EventArgs e)
        {
            int testCaseID = 0;
            bool parseResult = int.TryParse(this.hdnTestCaseID.Value, out testCaseID);
            var result = bll.resetTestCaseStatus(testCaseID, this.userID);
            if (!result.Equals("OK"))
            {
                sendMessageToClient(result);
            }
            else
            {
                sendMessageToClient("Test Case and all test steps reset to Untested status.");
            }
        }
                
        /// <summary>
        /// handles events when save precondition button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnSavePreCondition_Click(object sender, EventArgs e)
        {
            var testCaseID = 0;
            string saveResult = "";
            //get the test case id of the test case currently being edited
            var result = int.TryParse(this.hdnTestCaseID.Value, out testCaseID);
            if (testCaseID > 0)
            {
                this.editTestCaseID = testCaseID;
            }
            //get list of pre condition ids from client
            var pcIDs = this.hdnPreConditionIDs.Value;
            if (pcIDs.Length > 0)
            {
                saveResult = bll.savePreConditionsForTestCase(testCaseID, pcIDs);
            }
            else
            {
                //remove all preconditions
                saveResult = bll.removeAllPreConditionTestCases(testCaseID);
            }
            if (saveResult != "OK")
            {
                sendMessageToClient(saveResult);
            }
        }
                
        /// <summary>
        /// handles events when the edit test case button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnEditTC_Click(object sender, EventArgs e)
        {
            //pull test case id for editing from client
            int testCaseID = 0;
            bool result = int.TryParse(this.hdnTestCaseID.Value, out testCaseID);
            this.editTestCaseID = testCaseID;
            var ts = bll.getTestCaseByID(testCaseID);
            this.txtTestCaseName.Text = ts.Name;
            populateTestStepsGrid();
        }

        /// <summary>
        /// Button click fired from jquery that saves current test step in database
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnSaveTestStep_Click(object sender, EventArgs e)
        {
            int testCaseID = Convert.ToInt32(this.hdnTestCaseID.Value);
            string testStepText = "";
            string relatedBusinessRuleIDs;
            string[] arrRelatedBusinessRules;
            int testStepID = Convert.ToInt32(this.hdnTestStepID.Value);
            if (testStepID == 0) // adding new test step
            {
                //check that test step text is populated
                if (this.txtTestStep.Text.Length > 0)
                {
                    testStepText = txtTestStep.Text;
                    //create test step/business rules and test step/notes relationships from UI
                    var viewTestStep = new viewTestStep(0, testStepText, true, this.userID);
                    relatedBusinessRuleIDs = this.hdnRelatedBusinessRules.Value;
                    List<viewBusinessRule> relatedBusinessRules = new List<viewBusinessRule>();
                    if (relatedBusinessRuleIDs.Length > 0)
                    {
                        arrRelatedBusinessRules = relatedBusinessRuleIDs.Split(',');
                        for (int i = 0; i < arrRelatedBusinessRules.Length; i++)
                        {
                            relatedBusinessRules.Add(new viewBusinessRule(Convert.ToInt32(arrRelatedBusinessRules[i]),
                                                                          " ",
                                                                          0,
                                                                          0,
                                                                          " ",
                                                                          true,
                                                                          0));
                        }
                    }
                    viewTestStep.RelatedBusinessRules = relatedBusinessRules;
                    string notes = this.txtTestStepNotes.Text;
                    viewTestStep.Notes = notes;
                    //add new test step
                    string newID = bll.addTestStep(viewTestStep, testCaseID);
                    //try to parse as int to verify save
                    int saveTestID = 0;
                    bool saveResult = int.TryParse(newID, out saveTestID);
                    if (saveResult)
                    {
                        this.hdnTestStepID.Value = newID;
                    }
                    else
                    {
                        sendMessageToClient(newID);
                    }
                }
                else
                {
                    sendMessageToClient("Test Step text is required.");
                }
                populateTestStepsGrid();
            }
            else //updating an existing test step
            {
                //delete all test step/business rules and test step/notes relationships
                bll.removeAllTestStepBusinessRuleRelationshipsForTestCaseID(testStepID, testCaseID);
                //create test step/business rules and test step/notes relationships from UI
                relatedBusinessRuleIDs = this.hdnRelatedBusinessRules.Value;
                List<viewBusinessRule> relatedBusinessRules = new List<viewBusinessRule>();
                if (relatedBusinessRuleIDs.Length > 0)
                {
                    arrRelatedBusinessRules = relatedBusinessRuleIDs.Split(',');
                    for (int i = 0; i < arrRelatedBusinessRules.Length; i++)
                    {
                        relatedBusinessRules.Add(new viewBusinessRule(Convert.ToInt32(arrRelatedBusinessRules[i]),
                                                                      " ",
                                                                      0,
                                                                      0,
                                                                      " ",
                                                                      true,
                                                                      0));
                    }
                    bll.updateTestStepBusinessRuleRelationships(testStepID, relatedBusinessRules);
                }
                string notes = this.txtTestStepNotes.Text;
                string name = this.txtTestStep.Text;
                viewTestStep updateTestStep = new viewTestStep(testStepID,
                                                               name,
                                                               relatedBusinessRules,
                                                               notes,
                                                               this.userID);
                bll.updateTestStepForTestCase(updateTestStep, testCaseID);
                populateTestStepsGrid();
            }
        }

        /// <summary>
        /// event handler for delete test step grid command
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewDeleteEventArgs</param>
        protected void grdTestSteps_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var deletedRowIndex = e.RowIndex;
            var testCaseID = 0;
            //get the test case id of the test case currently being edited
            var result = int.TryParse(this.hdnTestCaseID.Value, out testCaseID);
            if (testCaseID > 0)
            {
                this.editTestCaseID = testCaseID;
                //get teststep id to be removed
                int deleteID = 0;
                var success = int.TryParse(grdTestSteps.Rows[e.RowIndex].Cells[0].Text, out deleteID);
                if (deleteID > 0)
                {
                    var error = bll.removeTestStepTestCaseRelationship(deleteID, testCaseID);
                    if (!error.Equals("OK"))
                    {
                        sendMessageToClient(error);
                    }
                    else
                    {
                        populateTestStepsGrid();
                        resequenceTestSteps(testCaseID);
                        populateTestStepsGrid();
                    }
                }
            }
        }

        /// <summary>
        /// event handler for edit test step grid command
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewEditEventArgs</param>
        protected void grdTestSteps_RowEditing(object sender, GridViewEditEventArgs e)
        {
            var testCaseID = 0;
            //get the test case id of the test case currently being edited
            var result = int.TryParse(this.hdnTestCaseID.Value, out testCaseID);
            if (testCaseID > 0)
            {
                this.editTestCaseID = testCaseID;
                //get teststep id 
                int testStepID = 0;
                var success = int.TryParse(grdTestSteps.Rows[e.NewEditIndex].Cells[0].Text, out testStepID);
                if (testStepID > 0)
                {
                    this.hdnOriginalTestStepID.Value = testStepID.ToString();
                    this.hdnTestStepID.Value = testStepID.ToString();
                    var editTestStep = bll.getTestStepByID(testStepID, testCaseID);
                    //populate ui controls
                    this.txtTestStep.Text = editTestStep.Name;
                    this.txtTestStepNotes.Text = editTestStep.Notes;
                    this.lbRelatedBusinessRules.Items.Clear();
                    foreach (var br in editTestStep.RelatedBusinessRules)
                    {
                        this.lbRelatedBusinessRules.Items.Add(new ListItem(br.Name, br.ID.ToString()));
                    }
                }
                //prevent further asp gridview events for editing
                e.Cancel = true;
                //call javascript function to show dialog
                showEditTestStepDialog();
            }
        }

        /// <summary>
        /// event handler for databound event on teststeps grid
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewEventArgs</param>
        protected void grdTestSteps_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //get hidden value for selected test step 
                int selectedTestStepID = 0;
                bool result = int.TryParse(this.hdnSelectedTestStepID.Value, out selectedTestStepID);
                if (result)
                {
                    if (e.Row.Cells[0].Text.Equals(selectedTestStepID.ToString()))
                    {
                        grdTestSteps.SelectedIndex = e.Row.RowIndex;
                        e.Row.Attributes.Add("class", "selectedRow");
                    }
                }
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
                        button.OnClientClick = "return confirm('Are you sure you want to delete this record?');";
                        if (disableTSDelete)
                        {
                            button.Visible = false;
                        }
                    }
                    if (disableTSEdit)
                    {
                        if (button != null && button.CommandName == "Edit")
                            button.Visible = false;
                    }
                }
            }
        }
       
        /// <summary>
        /// handles events when the move up button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnMoveUp_Click(object sender, EventArgs e)
        {
            moveTestStepAction("up");
        }

        /// <summary>
        /// handles events when the move down button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnMoveDown_Click(object sender, EventArgs e)
        {
            moveTestStepAction("down");
        }

        /// <summary>
        /// handles events when the test case save button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnTestCaseSave_Click(object sender, EventArgs e)
        {
            int testCaseID = 0;
            int sectionID = 0;
            string saveResult = "";
            bool result = int.TryParse(this.hdnTestCaseID.Value, out testCaseID);
            if (result)
            {
                var testCaseName = this.txtTestCaseName.Text;
                if (testCaseName.Length > 0)
                {
                    result = int.TryParse(this.hdnSectionID.Value, out sectionID);
                    if (result)
                    {
                        if (testCaseID == 0)
                        { //add new
                            viewTestCase tc = new viewTestCase(0, testCaseName, sectionID, true, this.userID, (int)Enums.testStatusEnums.TestStatus.Untested);
                            saveResult = bll.addNewTestCase(tc);
                            if (saveResult.IndexOf("ID") != -1)
                            {
                                var arrSaveResult = saveResult.Split('_');
                                var newTestCaseID = arrSaveResult[1];
                                this.hdnTestCaseID.Value = newTestCaseID;
                                saveResult = "OK";
                            }
                        }
                        else
                        { // update existing
                            viewTestCase tc = bll.getTestCaseByID(testCaseID);
                            tc.Name = testCaseName;
                            tc.SectionID = sectionID;
                            tc.UpdatedBy = this.userID;
                            saveResult = bll.updateTestCase(tc);
                        }
                        if (saveResult.Equals("OK"))
                        {
                            saveResult = "";
                            //turn on additional edit controls
                            populateTestStepsGrid(); //called so move up/down buttons will be hidden
                            this.afterSaveEditDIV.Visible = true;
                            //change text of done button to Done
                            this.btnDone.Text = "Done";
                        }
                    }
                    else
                    {
                        saveResult = "Invalid Section ID";
                    }
                }
                else
                {
                    saveResult = "TestCase name is required.";
                }
            }
            else
            {
                saveResult = "Invalid Test Case ID";
            }
            if (saveResult.Length > 0)
            {
                sendMessageToClient(saveResult);
            }
        }

        /// <summary>
        /// handles event for add new test case button
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnAddTC_Click(object sender, EventArgs e)
        {
            populateSectionsDDL();
            //no test steps will be found so this method will clear grid and hide move up/down buttons
            populateTestStepsGrid();
            //hide all buttons until test name/section is saved
            //this.btnAddTestStep.Visible = false;
            //this.btnEditPreConditions.Visible = false;
            this.afterSaveEditDIV.Visible = false;
            //rename done button to cancel
            this.btnDone.Text = "Cancel";
            this.hdnTestCaseID.Value = "0";
        }

        /// <summary>
        /// handles event for delete test case button click
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnDeleteTC_Click(object sender, EventArgs e)
        {
            string deleteResult = "";
            //get delete id from hidden clent field
            int deleteID = 0;
            bool result = int.TryParse(this.hdnDeleteID.Value, out deleteID);
            if (result)
            {
                deleteResult = bll.deleteTestCase(deleteID, this.userID);
                if (!deleteResult.Equals("OK"))
                {
                    sendMessageToClient(deleteResult);
                }
            }
        }

        #endregion event handlers

        #region private methods
                
        /// <summary>
        /// apply features
        /// </summary>
        /// <param name="userID">int</param>
        private void applyFeatures(int userID)
        {
            viewUser user = palinoiaBLL.getUserByID(userID);
            List<viewFeature> userFeatureList = palinoiaBLL.getFeaturesForUser(user);
            #region test case features
            //Test Case VIEW
            viewFeature feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.TestCasesView);
            if (feature != null)
            {

            }
            else
            {
                //user cannot view screen.  redirect to default display
                sendMessageToClient("You do not have the necessary permission to view this screen.");
                this.TestCasesTreePanel.Visible = false;
            }
            //Test Case ADD
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.TestCasesAdd);
            if (feature != null)
            {
                this.hdnDisableTCAdd.Value = "false";
            }
            else
            {
                this.hdnDisableTCAdd.Value = "true";
            }
            //Test Case EDIT
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.TestCasesEdit);
            if (feature != null)
            {
                this.hdnDisableTCEdit.Value = "false";
            }
            else
            {
                this.hdnDisableTCEdit.Value = "true";
            }
            //Test Case DELETE
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.TestCasesDelete);
            if (feature != null)
            {
                this.hdnDisableTCDelete.Value = "false";
            }
            else
            {
                this.hdnDisableTCDelete.Value = "true";
            }
            #endregion test case features
            #region precondition features
            //PreCondition VIEW
            //feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.PreConditionsView);
            //if (feature != null)
            //{
            //    //
            //}
            //else
            //{
            //    //this.grdPreConditions.Visible = false;
            //}
            // PreCondition ADD
            feature = userFeatureList.Find((f) => (f.ID == (int)featureEnums.Feature.PreConditionsAdd ||
                                                   f.ID == (int)featureEnums.Feature.PreConditionsEdit));
            if (feature != null)
            {
                this.btnEditPreConditions.Visible = true;
            }
            else
            {
                this.btnEditPreConditions.Visible = false;
            }

            // PreCondition Delete
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.PreConditionsDelete);
            if (feature != null)
            {
                this.btnRemovePreCTestCase.Visible = true;
            }
            else
            {
                this.btnRemovePreCTestCase.Visible = false;
            }
            #endregion precondition features
            #region test step features
            //Test Step VIEW
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.TestStepsView);
            if (feature != null)
            {
                this.grdTestSteps.Visible = true;
            }
            else
            {
                this.grdTestSteps.Visible = false;
            }
            //Test Step ADD
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.TestStepsAdd);
            if (feature != null)
            {
                this.hdnDisableTSAdd.Value = "false";
            }
            else
            {
                this.hdnDisableTSAdd.Value = "true";
            }
            //Test Step EDIT
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.TestStepsEdit);
            if (feature != null)
            {
                this.hdnDisableTSEdit.Value = "false";
            }
            else
            {
                this.hdnDisableTSEdit.Value = "true";
            }
            //Test Step DELETE
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.TestStepsDelete);
            if (feature != null)
            {
                this.hdnDisableTSDelete.Value = "false";
            }
            else
            {
                this.hdnDisableTSDelete.Value = "true";
            }
            #endregion test step features
       }
                
        /// <summary>
        /// display client edit precondition modal
        /// </summary>
        /// <param name="text">string</param>
        private void showClientEditPreConditionModal(string text)
        {
            // Define the name and type of the client script on the page.
            String csName = "showClientEditPreConditionModal";
            Type csType = this.GetType();
            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs = Page.ClientScript;
            // Check to see if the client script is already registered.
            if (!cs.IsStartupScriptRegistered(csType, csName))
            {
                ClientScript.RegisterStartupScript(typeof(Page), csName, "<script type='text/javascript'>showEditPreConditionModal('" + text + "');</script>");
            }
        }

        /// <summary>
        /// populate test steps grid
        /// </summary>
        private void populateTestStepsGrid()
        {
            int testCaseID = Convert.ToInt32(this.hdnTestCaseID.Value);
            List<viewTestStep> tsList = bll.getTestStepsForTestCase(testCaseID);
            this.grdTestSteps.DataSource = tsList;
            this.grdTestSteps.DataBind();
            //hide move up/down buttons if no test steps are present in grid
            bool showButtons = true;
            if (tsList.Count == 0)
            {
                showButtons = false;
            }
            this.btnMoveDown.Visible = showButtons;
            this.btnMoveUp.Visible = showButtons;
        }

        /// <summary>
        /// clear stest step dialog
        /// </summary>
        private void clearTestStepDialog()
        {
            txtTestStep.Text = "";
            lbRelatedBusinessRules.Items.Clear();
            txtTestStepNotes.Text = "";
        }

        /// <summary>
        /// show edit test step dialog
        /// </summary>
        private void showEditTestStepDialog()
        {
            // Define the name and type of the client script on the page.
            String csName = "showClientEditTestStepDialog";
            Type csType = this.GetType();
            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs = Page.ClientScript;
            // Check to see if the client script is already registered.
            if (!cs.IsStartupScriptRegistered(csType, csName))
            {
                ClientScript.RegisterStartupScript(typeof(Page), csName, "$(document).ready(function() {editTestStepLink_click();});", true);
            }
        }

        /// <summary>
        /// resequence tst steps
        /// </summary>
        /// <param name="testCaseID">int</param>
        private void resequenceTestSteps(int testCaseID)
        {
            //get list of IDs from grid
            List<int> sequence = new List<int>();
            foreach (GridViewRow row in this.grdTestSteps.Rows)
            {
                int tsID = 0;
                var id = row.Cells[0].Text;
                bool result = int.TryParse(id, out tsID);
                if (result)
                {
                    sequence.Add(tsID);
                }
            }
            bll.resequenceTestSteps(testCaseID, sequence);
        }

        /// <summary>
        /// move test step action
        /// </summary>
        /// <param name="direction">string</param>
        private void moveTestStepAction(string direction)
        {
            int selectedIndex = -1;
            int targetIndex = -1;
            int selectedSeqNum = -1;
            int targetSeqNum = -1;
            int selectedTestStepID = 0;
            int testCaseID = 0;
            bool result = int.TryParse(hdnTestCaseID.Value, out testCaseID);
            if (result)
            {
                result = int.TryParse(hdnSelectedTestStepID.Value, out selectedTestStepID);
                if (result)
                {
                    //get index for selected row
                    foreach (GridViewRow row in grdTestSteps.Rows)
                    {
                        if (row.Cells[0].Text.Equals(selectedTestStepID.ToString()))
                        {
                            selectedIndex = row.RowIndex;
                            result = int.TryParse(row.Cells[1].Text, out selectedSeqNum);
                        }
                    }
                    //get test step id of row above or below selected row
                    switch (direction)
                    {
                        case "up":
                            targetIndex = selectedIndex - 1;
                            targetSeqNum = selectedSeqNum - 1;
                            break;
                        case "down":
                            targetIndex = selectedIndex + 1;
                            targetSeqNum = selectedSeqNum + 1;
                            break;
                    }
                    int targetTestStepID = 0;
                    result = int.TryParse(grdTestSteps.Rows[targetIndex].Cells[0].Text, out targetTestStepID);
                    if (result)
                    {
                        //update selected and target test steps and reorder them
                        bll.updateSeqNumForTestStep(testCaseID, selectedTestStepID, targetSeqNum);
                        bll.updateSeqNumForTestStep(testCaseID, targetTestStepID, selectedSeqNum);
                        this.hdnSelectedTestStepID.Value = selectedTestStepID.ToString();
                        populateTestStepsGrid();
                        enableDisableMoveButtons();
                    }
                }
            }
        }

        /// <summary>
        /// enable or disable move button
        /// </summary>
        private void enableDisableMoveButtons()
        {
            int selectedIndex = -1;
            int selectedTestStepID = 0;
            bool result = int.TryParse(hdnSelectedTestStepID.Value, out selectedTestStepID);
            if (result)
            {
                int rowCounter = 0;
                //get index for selected row
                foreach (GridViewRow row in grdTestSteps.Rows)
                {
                    rowCounter++;

                    if (row.Cells[0].Text.Equals(selectedTestStepID.ToString()))
                    {
                        selectedIndex = row.RowIndex;
                    }
                }
                if (selectedIndex == (rowCounter - 1))
                {
                    this.btnMoveDown.Attributes.Add("disabled", "disabled");
                }
                else
                {
                    this.btnMoveDown.Attributes.Remove("disabled");
                }
                if (selectedIndex == 0)
                {
                    this.btnMoveUp.Attributes.Add("disabled", "disabled");
                }
                else
                {
                    this.btnMoveUp.Attributes.Remove("disabled");
                }
            }
        }

        /// <summary>
        /// populate sections DDL
        /// </summary>
        private void populateSectionsDDL()
        {
            this.ddlSections.Items.Clear();
            var sectionList = adminBLL.getAllSections();
            var sectionID = 0;
            bool result = int.TryParse(this.hdnSectionID.Value, out sectionID);
            if (result)
            {
                int index = 0;
                foreach (var section in sectionList)
                {
                    ddlSections.Items.Add(new ListItem(section.Text, section.ID.ToString()));
                    if (section.ID == sectionID)
                    {
                        ddlSections.SelectedIndex = index;
                        ddlSections.Items[index].Selected = true;
                    }
                    else
                    {
                        ddlSections.Items[index].Selected = false;
                    }
                    index++;
                }
            }
        }

        #endregion private methods

        #region web methods
                
        /// <summary>
        /// fetches test cases from database for tree
        /// </summary>
        /// <param name="nodeID">int</param>
        /// <param name="projID">string</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<JSTree_Node> GetTestCasesForTree(string nodeID, string projID)
        {
            int projectID = Convert.ToInt32(projID);
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
                        nodes = AddTestCaseChildNodes(projectID, objectID);
                        break;
                    case ("TC"):
                        //create nodes for all test steps associated with test case id
                        nodes = AddTestStepChildNodes(projectID, objectID);
                        break;
                }
            }
            else // screen is loading; populate root node with sections
            {
                var bll = new AdminBLL(projectID);
                var sectionList = bll.getAllSections();
                JSTree_Node rootNode = new JSTree_Node();
                rootNode.data = new JsTreeNodeData { title = "Sections" };
                if (sectionList.Count > 0)
                {
                    rootNode.state = "open";
                    rootNode.IdServerUse = 0;
                    rootNode.attr = new JsTreeAttribute { id = "0", selected = false };
                    rootNode.children = AddSectionChildNodes(projectID, sectionList).ToArray();
                }
                nodes.Add(rootNode);
                bll = null;
            }
            return nodes;
        }
                
        /// <summary>
        /// add section child nodes to tree root
        /// </summary>
        /// <param name="sectionList">List&lt;viewSection&gt;</param>
        /// <param name="projectID">int</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        private static List<JSTree_Node> AddSectionChildNodes(int projectID, List<viewSection> sectionList)
        {
            List<JSTree_Node> JSTreeArray = new List<JSTree_Node>();
            foreach (var section in sectionList)
            {
                var bll = new TestCasesBLL(projectID);
                var hasTestCases = bll.hasTestCases(section.ID);
                int CurrChildId = section.ID;
                JSTree_Node jsTreeNode = new JSTree_Node();
                jsTreeNode.data = new JsTreeNodeData { title = section.Text };
                if (hasTestCases)
                {
                    jsTreeNode.state = "closed";  
                }
                jsTreeNode.IdServerUse = CurrChildId;
                jsTreeNode.attr = new JsTreeAttribute { id = "SEC_" + CurrChildId.ToString(), type = "Section", selected = false };
                JSTreeArray.Add(jsTreeNode);
            }
            return JSTreeArray;
        }
                
        /// <summary>
        /// add test case child nodes to each section node
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <param name="projectID">int</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        private static List<JSTree_Node> AddTestCaseChildNodes(int projectID, int testCaseID)
        {
            var bll = new TestCasesBLL(projectID);
            var testCaseList = bll.getAllTestCasesBySection(testCaseID);
            List<JSTree_Node> JSTreeArray = new List<JSTree_Node>();
            foreach (var viewTestCase in testCaseList)
            {
                var hasTestSteps = bll.hasTestSteps(viewTestCase.ID);
                int CurrChildId = viewTestCase.ID;
                JSTree_Node jsTreeNode = new JSTree_Node();
                //set pass, fail, or untested icon for test case
                string iconPath = "../../Scripts/JSTree/icons/faq_16.png"; // default to untested
                if (viewTestCase.TestStatusID == (int)Enums.testStatusEnums.TestStatus.Pass)
                {
                    iconPath = "../../Scripts/JSTree/icons/tick_16.png";
                }
                if (viewTestCase.TestStatusID == (int)Enums.testStatusEnums.TestStatus.Fail)
                {
                    iconPath = "../../Scripts/JSTree/icons/delete_16.png";
                }
                jsTreeNode.data = new JsTreeNodeData { title = viewTestCase.Name, icon = iconPath };
                if (hasTestSteps)
                {
                    jsTreeNode.state = "closed";  //For async to work
                }
                jsTreeNode.IdServerUse = CurrChildId;
                jsTreeNode.attr = new JsTreeAttribute { id = "TC_" + CurrChildId.ToString(), type = "TestCase", selected = false };
                JSTreeArray.Add(jsTreeNode);

            }
            bll = null;
            return JSTreeArray;
        }
                
        /// <summary>
        /// add test step child nodes to each test case node
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <param name="projectID">int</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        private static List<JSTree_Node> AddTestStepChildNodes(int projectID, int testCaseID)
        {
            List<JSTree_Node> JSTreeArray = new List<JSTree_Node>();
            var bll = new TestCasesBLL(projectID);
            var testStepList = bll.getTestStepsForTestCase(testCaseID);
            foreach (var testStep in testStepList)
            {
                int CurrChildId = testStep.ID;
                JSTree_Node jsTreeNode = new JSTree_Node();
                //set pass, fail, or untested icon for test case
                string iconPath = "../../Scripts/JSTree/icons/faq_16.png"; // default to untested
                var testStepResult = bll.getLatestTestResult(testCaseID, testStep.ID); 
                if (testStepResult.TestStatusID == (int)Enums.testStatusEnums.TestStatus.Pass)
                {
                    iconPath = "../../Scripts/JSTree/icons/tick_16.png";
                }
                if (testStepResult.TestStatusID == (int)Enums.testStatusEnums.TestStatus.Fail)
                {
                    iconPath = "../../Scripts/JSTree/icons/delete_16.png";
                }
                jsTreeNode.data = new JsTreeNodeData { title = testStep.Name, icon = iconPath };
                jsTreeNode.IdServerUse = CurrChildId;
                jsTreeNode.children = null;
                jsTreeNode.attr = new JsTreeAttribute { id = "TS_" + CurrChildId.ToString(), type = "TCTestStep", selected = false };
                JSTreeArray.Add(jsTreeNode);
            }
            bll = null;
            return JSTreeArray;
        }

        /// <summary>
        /// fetch business rules for tree in edit test step dialog
        /// </summary>
        /// <param name="nodeID">string</param>
        /// <param name="projID">string</param>
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
                bool result = int.TryParse(idArray[1], out objectID);
                var bll = new TestCasesBLL(projectID);
                switch (objectAbbv)
                {
                    case ("SEC"):
                        //create nodes for all test cases associated with section id
                        nodes = AddBusinessRuleChildNodes(projectID, objectID);
                        break;
                    case("BR"):
                        //create node for business rule text
                        nodes = AddBusinessRuleTextNode(projectID, objectID);
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
                rootNode.attr = new JsTreeAttribute { id = "ROOT", selected = false };
                rootNode.children = AddBRSectionChildNodes(projectID, sectionList).ToArray();
                nodes.Add(rootNode);
                adminBLL = null;
            }
            return nodes;
        }
                
        /// <summary>
        /// add section nodes to tree root
        /// </summary>
        /// <param name="projectID">int</param>
        /// <param name="sectionList">List&lt;viewSection&gt;</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        private static List<JSTree_Node> AddBRSectionChildNodes(int projectID, List<viewSection> sectionList)
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
                    jsTreeNode.state = "closed";  
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
        /// add business rule nodes to each section
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
                jsTreeNode.state = "closed";
                jsTreeNode.attr = new JsTreeAttribute { id = "BR_" + CurrChildId.ToString(), type = "BusinessRule", selected = false };
                JSTreeArray.Add(jsTreeNode);
            }
            return JSTreeArray;
        }
                
        /// <summary>
        /// add business rule text node to each business rule node
        /// </summary>
        /// <param name ="projectID">int</param>
        /// <param name="businessRuleID">int</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        private static List<JSTree_Node> AddBusinessRuleTextNode(int projectID, int businessRuleID)
        {
            var bll = new BusinessRulesBLL(projectID);
            var br = bll.getBusinessRuleByID(businessRuleID);
            List<JSTree_Node> JSTreeArray = new List<JSTree_Node>();
            JSTree_Node jsTreeNode = new JSTree_Node();
            jsTreeNode.data = new JsTreeNodeData { title = stripHTMLFromText(br.Text) };
            //jsTreeNode.state = "closed";  //For async to work
            jsTreeNode.IdServerUse = br.ID;
            //add root nodes to test case node for BusinessRules and Test Steps
            jsTreeNode.children = null;
            jsTreeNode.attr = new JsTreeAttribute { id = "BRText_" + br.ID.ToString(), type = "BusinessRuleText", selected = false };
            JSTreeArray.Add(jsTreeNode);
            bll = null;
            return JSTreeArray;
        }
               
        /// <summary>
        /// web method to get next unused business rule number by section
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
            int nextNumber = brList.Count + 1;
            brList = null;
            return nextNumber.ToString(); ;
        }

        /// <summary>
        /// web method to get preconditions for test case
        /// </summary>
        /// <param name="projID">string</param>
        /// <param name="tcID">string</param>
        /// <returns>string</returns>
        [WebMethod]
        public static string getPreConditionsForTestCase(string projID, string tcID)
        {
            StringBuilder sb = new StringBuilder();
            int projectID = 0;
            bool parseResult = int.TryParse(projID, out projectID);
            int testCaseID = 0;
            parseResult = int.TryParse(tcID, out testCaseID);
            var bll = new TestCasesBLL(projectID);
            List<viewTestCase> pcTestCases = bll.getPreConditionsForTestCase(testCaseID);
            int pcCounter = pcTestCases.Count;
            foreach (var tc in pcTestCases)
            {
                pcCounter--;
                sb.Append(tc.Name);
                sb.Append(",");
                sb.Append(tc.ID);
                if (pcCounter > 0)
                {
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }
                
        /// <summary>
        /// web method to get next unused business rule number by section
        /// </summary>
        /// <param name="brID">string</param>
        /// <param name="projID">string</param>
        /// <returns>string</returns>
        [WebMethod]
        public static string getBusinessRuleNameForID(string brID, string projID)
        {
            int projectID = Convert.ToInt32(projID);
            int businessRuleID = Convert.ToInt32(brID);
            var bll = new BusinessRulesBLL(projectID);
            var br = new viewBusinessRule();
            string name = "";
            try
            {
                br = bll.getBusinessRuleByID(businessRuleID);
                name = br.Name;
            }
            catch (Exception ex)
            {
                name = "* ERROR *";
            }
            finally
            {
                bll = null;
                br = null;
            }
            return name;
        }

        [WebMethod]
        public static string getRelatedBusinessRulesForTSName(string projID, string tsName)
        {
            string businessRuleIDlist = "";
            int projectID = Convert.ToInt32(projID);
            var bll = new TestCasesBLL(projectID);
            if (tsName.Length > 0)
            {
                businessRuleIDlist = bll.getRelatedBusinessRulesForTestStepName(tsName);
            }
            return businessRuleIDlist;
        }
                
        /// <summary>
        /// strip HTML from text for display in tree node
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
        /// returns data type ID value stored in lkup_Objects for the object id to client
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
        /// performs search from client advanced search data and return integer list of result ids
        /// </summary>
        /// <param name="data">List&lt;ClientSearchData&gt;</param>
        /// <param name="projID">string</param>
        /// <param name="searchType">string</param>
        /// <returns>List&lt;int&gt;</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static TestCaseSearchResult getAdvancedSearchResults(List<ClientSearchData> data, string projID, string searchType)
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
            var searchResult = bll.doAdvancedTestCaseSearch(search, searchTypeID);
            bll = null;
            return searchResult;
        }
        /// <summary>
        /// performs search from client basic search data and return integer list of result ids
        /// </summary>
        /// <param name="data">List&lt;ClientSearchData&gt;</param>
        /// <param name="projID">string</param>
        /// <param name="searchType">string</param>
        /// <returns>List&lt;int&gt;</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static TestCaseSearchResult getBasicSearchResults(List<ClientSearchData> data, string projID, string searchType)
        {
            int projectID = 0;
            int searchTypeID = 0;
            bool parseResult = int.TryParse(projID, out projectID);
            parseResult = int.TryParse(searchType, out searchTypeID);
            SearchBLL bll = new SearchBLL(projectID);
            SearchEntities search = new SearchEntities();
            var value = data[0].textValue;
            var searchResult = bll.doBasicTestCaseSearch(value, searchTypeID);
            bll = null;
            return searchResult;
        }
        /// <summary>
        /// returns search results as collection of tree nodes customized for Test Cases
        /// </summary>
        /// <param name="nodeID">string</param>
        /// <param name="searchResult">List&lt;int&gt;</param>
        /// <param name="projID">string</param>
        /// <param name="searchType">string</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<JSTree_Node> getSearchResultsForTree(string nodeID, TestCaseSearchResult searchResult, string projID, string searchType)
        {
            int projectID = 0;
            int searchTypeID = 0;
            bool result = int.TryParse(projID, out projectID);
            result = int.TryParse(searchType, out searchTypeID);
            List<JSTree_Node> nodes = new List<JSTree_Node>();
            //create business rule results tree
            if (nodeID.Equals("0"))
            {
                JSTree_Node rootNode = new JSTree_Node();
                int searchResultCount = searchResult.tcID.Count + searchResult.tsID.Count;
                string rootTitle = searchResultCount.ToString() + " Search Results";
                rootNode.data = new JsTreeNodeData { title = rootTitle };
                rootNode.state = "open";
                rootNode.IdServerUse = 0;
                rootNode.attr = new JsTreeAttribute { id = "ROOT", selected = false };
                rootNode.children = AddSearchSectionChildNodes(projectID, searchResult).ToArray();
                nodes.Add(rootNode);

            }
            else
            {
                var idArray = nodeID.Split('_');
                var objectAbbv = idArray[0];
                int objectID = 0;
                result = int.TryParse(idArray[1], out objectID);
                var bll = new TestCasesBLL(projectID);
                switch (objectAbbv)
                {
                    case ("TC"):
                        //create nodes for all test steps associated with test case id
                        nodes = AddSearchTestStepChildNodes(projectID, objectID, searchResult);
                        break;
                    case("SEC"):
                        nodes = AddSearchTestCaseChildNodes(projectID, objectID, searchResult);
                        break;
                }


            }
            return nodes;
        }
                
        /// <summary>
        /// add section child nodes to tree root
        /// </summary>
        /// <param name="projectID">int</param>
        /// <param name="result">TestCaseSearchResult r</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        private static List<JSTree_Node> AddSearchSectionChildNodes(int projectID, TestCaseSearchResult result)
        {
            List<JSTree_Node> JSTreeArray = new List<JSTree_Node>();
            var adminBLL = new AdminBLL(projectID);
            var sectionList = adminBLL.getAllSections();
            foreach (var section in sectionList)
            {
                if (sectionHasResults(projectID, section.ID, result))
                {
                    var bll = new TestCasesBLL(projectID);
                    var hasTestCases = bll.hasTestCases(section.ID);
                    int CurrChildId = section.ID;
                    JSTree_Node jsTreeNode = new JSTree_Node();
                    jsTreeNode.data = new JsTreeNodeData { title = section.Text };
                    if (hasTestCases)
                    {
                        jsTreeNode.state = "closed";
                    }
                    jsTreeNode.IdServerUse = CurrChildId;
                    jsTreeNode.attr = new JsTreeAttribute { id = "SEC_" + CurrChildId.ToString(), type = "Section", selected = false };
                    JSTreeArray.Add(jsTreeNode);
                }
            }
            return JSTreeArray;
        }
                
        /// <summary>
        /// add test case child nodes to each section node
        /// </summary>
        /// <param name="projectID">int</param>
        /// <param name="sectionID">int</param>
        /// <param name="result">TestCaseSearchResult</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        private static List<JSTree_Node> AddSearchTestCaseChildNodes(int projectID, int sectionID, TestCaseSearchResult result)
        {
            var bll = new TestCasesBLL(projectID);
            var testCaseList = bll.getAllTestCasesBySection(sectionID);
            List<JSTree_Node> JSTreeArray = new List<JSTree_Node>();
            foreach (var viewTestCase in testCaseList)
            {
                if (TestCaseHasResults(projectID, viewTestCase.ID, result))
                {
                    var hasTestSteps = bll.hasTestSteps(viewTestCase.ID);
                    int CurrChildId = viewTestCase.ID;
                    JSTree_Node jsTreeNode = new JSTree_Node();
                    //set pass, fail, or untested icon for test case
                    string iconPath = "../../Scripts/JSTree/icons/faq_16.png"; // default to untested
                    if (viewTestCase.TestStatusID == (int)Enums.testStatusEnums.TestStatus.Pass)
                    {
                        iconPath = "../../Scripts/JSTree/icons/tick_16.png";
                    }
                    if (viewTestCase.TestStatusID == (int)Enums.testStatusEnums.TestStatus.Fail)
                    {
                        iconPath = "../../Scripts/JSTree/icons/delete_16.png";
                    }
                    jsTreeNode.data = new JsTreeNodeData { title = viewTestCase.Name, icon = iconPath };
                    if (hasTestSteps)
                    {
                        jsTreeNode.state = "closed";  //For async to work
                    }
                    jsTreeNode.IdServerUse = CurrChildId;
                    jsTreeNode.attr = new JsTreeAttribute { id = "TC_" + CurrChildId.ToString(), type = "TestCase", selected = false };
                    JSTreeArray.Add(jsTreeNode);
                }
            }
            bll = null;
            return JSTreeArray;
        }
                
        /// <summary>
        /// add test step child nodes to each test case node
        /// </summary>
        /// <param name="projectID">int</param>
        /// <param name="testCaseID">int</param>
        /// <param name="result">TestCaseSearchResult</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        private static List<JSTree_Node> AddSearchTestStepChildNodes(int projectID, int testCaseID, TestCaseSearchResult result)
        {
            List<JSTree_Node> JSTreeArray = new List<JSTree_Node>();
            var bll = new TestCasesBLL(projectID);
            var testStepList = bll.getTestStepsForTestCase(testCaseID);
            foreach (var testStep in testStepList)
            {
                int CurrChildId = testStep.ID;
                JSTree_Node jsTreeNode = new JSTree_Node();
                //set pass, fail, or untested icon for test case
                string iconPath = "../../Scripts/JSTree/icons/faq_16.png"; // default to untested
                var testStepResult = bll.getLatestTestResult(testCaseID, testStep.ID);
                if (testStepResult.TestStatusID == (int)Enums.testStatusEnums.TestStatus.Pass)
                {
                    iconPath = "../../Scripts/JSTree/icons/tick_16.png";
                }
                if (testStepResult.TestStatusID == (int)Enums.testStatusEnums.TestStatus.Fail)
                {
                    iconPath = "../../Scripts/JSTree/icons/delete_16.png";
                }
                jsTreeNode.data = new JsTreeNodeData { title = testStep.Name, icon = iconPath };
                jsTreeNode.IdServerUse = CurrChildId;
                jsTreeNode.children = null;
                jsTreeNode.attr = new JsTreeAttribute { id = "TS_" + CurrChildId.ToString(), type = "TCTestStep", selected = false };
                JSTreeArray.Add(jsTreeNode);
            }
            bll = null;
            return JSTreeArray;
        }
        
        #endregion web methods

        #region private

        private static bool sectionHasResults(int projectID, int sectionID, TestCaseSearchResult result)
        {
            bool hasResults = false;
            var tcBLL = new TestCasesBLL(projectID);
            var sectionTestCases = tcBLL.getAllTestCasesBySection(sectionID);
            foreach (var tc in sectionTestCases)
            {
                if (TestCaseHasResults(projectID, tc.ID, result))
                {
                    hasResults = true;
                }
            }
            tcBLL = null;
            return hasResults;
        }

        private static bool TestCaseHasResults(int projectID, int testCaseID, TestCaseSearchResult result)
        {
            var tcBLL = new TestCasesBLL(projectID);
            bool hasResults = false;
            if (result.tcID.Contains(testCaseID.ToString()))
            {
                hasResults = true;
            }
            if (!hasResults)
            {
                var testStepList = tcBLL.getTestStepsForTestCase(testCaseID);
                foreach (var testStep in testStepList)
                {
                    if (result.tsID.Contains(testStep.ID.ToString()))
                    {
                        hasResults = true;
                    }
                }
            }
            tcBLL = null;
            return hasResults;
        }

        #endregion private

        #endregion search

    }
}