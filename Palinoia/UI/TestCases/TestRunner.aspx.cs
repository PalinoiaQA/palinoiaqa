using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Web.UI;
using BLL;
using Entities;
using Enums;
using System.Web.UI.WebControls;

namespace Palinoia.UI.TestCases
{
    /// <summary>
    /// class to hold code for TestRunner
    /// </summary>
    public partial class TestRunner : basePalinoiaPage
    {
        #region properties and variables

        int testCaseID = 0;
        List<viewTestCase> tcList;
        viewTestCase primaryTestCase;
        List<viewTestStep> tsList;
        TestCasesBLL tcBLL;
        viewTestCase currentTestCase;
        viewTestStep currentTestStep;
        int projectID;
        
        #endregion properties and variables

        #region page lifecycle events

        /// <summary>
        /// initialize page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            this.addJavaScriptReference("TestCases/TestRunner.js");
        }

        /// <summary>
        /// load  a page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            tcBLL = new TestCasesBLL(projectID);
            if (!IsPostBack)
            {
                int userID = Convert.ToInt32(Session.Contents["userID"]);
                projectID = Convert.ToInt32(Session.Contents["projectID"]);
                if (userID > 0 && projectID > 0)
                {
                    bool result = int.TryParse(Request.QueryString["tcid"], out testCaseID);
                    if (!result)
                    {
                        //show error message in client
                    }
                    else
                    {
                        //get list of all precondition test cases and primary test case
                        tcList = tcBLL.getTestCaseListForTesting(testCaseID);
                        tsList = tcBLL.getTestStepsForTestCase(tcList[0].ID);
                        //initialize class variables
                        primaryTestCase = tcBLL.getTestCaseByID(testCaseID);
                        currentTestCase = tcList[0];
                        currentTestStep = tsList[0];
                        //populate hiddenn fields
                        this.hdnCurrentTestCaseID.Value = tcList[0].ID.ToString();
                        this.hdnCurrentTestStepID.Value = currentTestStep.ID.ToString();
                        this.hdnPrimaryTestCaseID.Value = testCaseID.ToString();
                        //display first test step
                        this.txtTestStep.Text = tsList[0].Name + " " + tsList[0].Notes;
                        //populate failed BR dropdown
                        this.ddlSelectFailedBR.Items.Clear();
                        this.hdnShowSelectBR.Value = "";
                        if (tsList[0].RelatedBusinessRules.Count > 0)
                        {
                            if (tsList[0].RelatedBusinessRules.Count > 1)
                            {
                                this.hdnShowSelectBR.Value = "true";
                            }
                            foreach (var br in tsList[0].RelatedBusinessRules)
                            {
                                this.ddlSelectFailedBR.Items.Add(new ListItem(br.Name, br.ID.ToString()));
                            }
                            this.hdnFailedBusinessRuleID.Value = this.ddlSelectFailedBR.Items[0].Value;
                        }
                        this.lblPrimaryTestCase.Text = primaryTestCase.Name;
                        this.lblCurrentTestCase.Text = currentTestCase.Name;
                        //disable back button
                        
                        this.btnBack.Attributes.Add("disabled", "disabled");
                        displayTestResults(currentTestCase.ID, currentTestStep.ID);
                        //disable next button if first test step is untested or fail
                        var testResult = tcBLL.getLatestTestResult(currentTestCase.ID, currentTestStep.ID);
                        if (testResult.TestStatusID == (int)Enums.testStatusEnums.TestStatus.Pass)
                        {
                            this.btnNext.Attributes.Remove("disabled");
                        }
                        else
                        {
                            this.btnNext.Attributes.Add("disabled", "disabled");
                        }
                    }
                }
                else
                {
                    //show error message in client
                }
            }
            else
            {
                //populate class variables from hidden client fields
                int tcID = 0;
                int tsID = 0;
                bool result = int.TryParse(this.hdnCurrentTestCaseID.Value, out tcID);
                result = int.TryParse(this.hdnCurrentTestStepID.Value, out tsID);
                if (tcID > 0 && tsID > 0)
                {
                    this.currentTestCase = tcBLL.getTestCaseByID(tcID);
                    this.currentTestStep = tcBLL.getTestStepByID(tsID);
                }
            }
        }

        #endregion page lifecycle events

        #region eventhandlers

        /// <summary>
        /// Click event for Pass button.  Save test result to db
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnPass_Click(object sender, EventArgs e)
        {
            int userID = Convert.ToInt32(Session.Contents["userID"]);
            int currentTestCaseID = 0;
            int currentTestStepID = 0;
            int primaryTestCaseID = 0;
            int failedBusinessRuleID = 0;
            bool result2 = int.TryParse(this.hdnCurrentTestCaseID.Value, out currentTestCaseID);
            result2 = int.TryParse(this.hdnCurrentTestStepID.Value, out currentTestStepID);
            result2 = int.TryParse(this.hdnPrimaryTestCaseID.Value, out primaryTestCaseID);
            result2 = int.TryParse(this.hdnFailedBusinessRuleID.Value, out failedBusinessRuleID);
            viewTestResult tr = new viewTestResult(0,
                                                   primaryTestCaseID,
                                                   currentTestCase.ID,
                                                   currentTestStep.ID,
                                                   (int)testStatusEnums.TestStatus.Pass,
                                                   userID,
                                                   DateTime.Now,
                                                   "",
                                                   failedBusinessRuleID);
            string result = tcBLL.saveTestResult(tr);
            viewTestResult nextTestStep = tcBLL.getNextTestStepForTestRunner(tr);
            if(!result.Equals("OK") && !result.Equals("FINALPASS"))
            {
                sendMessageToClient(result);
            }
            //make sure this is final pass and that final test step is displayed on the screen
            else if (result.Equals("FINALPASS") && nextTestStep.TestStepID == currentTestStepID)
            {
                //update test case status to Passed
                string updateResult = tcBLL.updateTestCaseStatus(currentTestCaseID, 
                                                           (int)Enums.testStatusEnums.TestStatus.Pass, 
                                                           userID);
                if (updateResult.Equals("OK"))
                {
                    //get primary test case so we can use the name in the message to user
                    var tc = tcBLL.getTestCaseByID(primaryTestCaseID);
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Test Case: ");
                    sb.Append(tc.Name);
                    sb.Append(" has completed successfully.");
                    sendMessageToClient(sb.ToString());
                    ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
                }
                else
                {
                    sendMessageToClient(result);
                    ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
                }
            }
            else
            {
                this.hdnCurrentTestCaseID.Value = nextTestStep.CurrentTestCaseID.ToString();
                this.hdnCurrentTestStepID.Value = nextTestStep.TestStepID.ToString();
                this.hdnPrimaryTestCaseID.Value = nextTestStep.PrimaryTestCaseID.ToString();
                //get test step object
                var ts = tcBLL.getTestStepByID(nextTestStep.TestStepID);
                //populate failed BR dropdown
                this.ddlSelectFailedBR.Items.Clear();
                this.hdnShowSelectBR.Value = "";
                if (ts.RelatedBusinessRules.Count > 0)
                {
                    if (ts.RelatedBusinessRules.Count > 1)
                    {
                        this.hdnShowSelectBR.Value = "true";
                    }
                    foreach (var br in ts.RelatedBusinessRules)
                    {
                        this.ddlSelectFailedBR.Items.Add(new ListItem(br.Name, br.ID.ToString()));
                    }
                }
                displayTestStep(nextTestStep);
                displayTestResults(nextTestStep.CurrentTestCaseID, nextTestStep.TestStepID);
            }
        }

        /// <summary>
        /// handles events when the fail button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnSaveFail_Click(object sender, EventArgs e)
        {
            int userID = Convert.ToInt32(Session.Contents["userID"]);
            //validate text
            var failText = this.txtNotes.Text;
            string validationMessage = "";
            //required field
            if (failText.Length == 0)
            {
                validationMessage = "SERVER ERROR: Text is required.";
            }
            //valid text
            if (!this.validateInputText(failText))
            {
                validationMessage = "SERVER ERROR: Invalid Text!";
            }
            if (validationMessage.Length > 0)
            {
                sendMessageToClient(validationMessage);
            }
            else
            {
                string result = this.saveFailedTestResults();
                int currentTestCaseID = 0;
                bool result2 = int.TryParse(this.hdnCurrentTestCaseID.Value, out currentTestCaseID);
                //update status of test case to Failed
                if (!result.Equals("OK"))
                {
                    sendMessageToClient(result);
                }
                else
                {   //test result saved.  update test case result to Failed
                    string updateResult = tcBLL.updateTestCaseStatus(currentTestCaseID,
                                                           (int)Enums.testStatusEnums.TestStatus.Fail,
                                                           userID);
                    ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
                }
            }
        }

        /// <summary>
        /// handles events when the save button is clicked after selecting failed business rule
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnSaveFailedBR_Click(object sender, EventArgs e)
        {
            int userID = Convert.ToInt32(Session.Contents["userID"]);
            //validate text
            var failText = this.txtNotes.Text;
            string validationMessage = "";
            //required field
            if (failText.Length == 0)
            {
                validationMessage = "SERVER ERROR: Text is required.";
            }
            //valid text
            if (!this.validateInputText(failText))
            {
                validationMessage = "SERVER ERROR: Invalid Text!";
            }
            if (validationMessage.Length > 0)
            {
                sendMessageToClient(validationMessage);
            }
            else
            {
                string result = this.saveFailedTestResults();
                int currentTestCaseID = 0;
                bool result2 = int.TryParse(this.hdnCurrentTestCaseID.Value, out currentTestCaseID);
                //update status of test case to Failed
                if (!result.Equals("OK"))
                {
                    sendMessageToClient(result);
                }
                else
                {   //test result saved.  update test case result to Failed
                    string updateResult = tcBLL.updateTestCaseStatus(currentTestCaseID,
                                                           (int)Enums.testStatusEnums.TestStatus.Fail,
                                                           userID);
                    ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
                }
            }
        }

        /// <summary>
        /// displays a test step
        /// </summary>
        /// <param name="tr">viewTestResult</param>
        public void displayTestStep(viewTestResult tr)
        {
            viewTestStep ts = tcBLL.getTestStepByID(tr.TestStepID, tr.CurrentTestCaseID);
            StringBuilder sb = new StringBuilder();
            sb.Append(ts.Name);
            if (ts.Notes.Length > 0)
            {
                sb.Append("\r\n");
                sb.Append("NOTE: ");
                sb.Append(ts.Notes);
            }
            this.txtTestStep.Text = sb.ToString();
            var currentTC = tcBLL.getTestCaseByID(tr.CurrentTestCaseID);
            var primaryTC = tcBLL.getTestCaseByID(tr.PrimaryTestCaseID);
            this.lblPrimaryTestCase.Text = primaryTC.Name;
            this.lblCurrentTestCase.Text = currentTC.Name;
            currentTC = null;
            primaryTC = null;
            if (tr.last)
            {
                this.btnNext.Attributes.Add("disabled", "disabled");
            }
            else 
            {
                this.btnNext.Attributes.Remove("disabled");
            }
            if (tr.first)
            {
                this.btnBack.Attributes.Add("disabled", "disabled");
            }
            else
            {
                this.btnBack.Attributes.Remove("disabled");
            }
        }

        /// <summary>
        /// event handler for the back button on the test runner.
        /// user can move back through previously tested test steps
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            viewTestResult tr = new viewTestResult();
            int currentTestCaseID = 0;
            int currentTestStepID = 0;
            int primaryTestCaseID = 0;
            bool result2 = int.TryParse(this.hdnCurrentTestCaseID.Value, out currentTestCaseID);
            result2 = int.TryParse(this.hdnCurrentTestStepID.Value, out currentTestStepID);
            result2 = int.TryParse(this.hdnPrimaryTestCaseID.Value, out primaryTestCaseID);
            //viewTestStep ts = new viewTestStep();
            //ts.ID = currentTestStepID;
            //TestRunnerTestStep trts = new TestRunnerTestStep();
            //trts.currentTestCaseID = currentTestCaseID;
            //trts.primaryTestCaseID = primaryTestCaseID;
            //trts.currentTestStep = ts;
            tr.CurrentTestCaseID = currentTestCaseID;
            tr.PrimaryTestCaseID = primaryTestCaseID;
            tr.TestStepID = currentTestStepID;
            viewTestResult previousTestStep = tcBLL.getPreviousTestStepForTestRunner(tr);
            this.hdnCurrentTestCaseID.Value = previousTestStep.CurrentTestCaseID.ToString();
            this.hdnCurrentTestStepID.Value = previousTestStep.TestStepID.ToString();
            this.hdnPrimaryTestCaseID.Value = previousTestStep.PrimaryTestCaseID.ToString();
            //get test step object
            var ts = tcBLL.getTestStepByID(previousTestStep.TestStepID);
            //populate failed BR dropdown
            this.ddlSelectFailedBR.Items.Clear();
            if (ts.RelatedBusinessRules.Count > 0)
            {
                if (ts.RelatedBusinessRules.Count > 1)
                {
                    this.hdnShowSelectBR.Value = "true";
                }
                foreach (var br in ts.RelatedBusinessRules)
                {
                    this.ddlSelectFailedBR.Items.Add(new ListItem(br.Name, br.ID.ToString()));
                }
            }
            displayTestStep(previousTestStep);
            displayTestResults(previousTestStep.CurrentTestCaseID, previousTestStep.TestStepID);
        }

        /// <summary>
        /// handles events when next button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnNext_Click(object sender, EventArgs e)
        {
            viewTestResult tr = new viewTestResult();
            int currentTestCaseID = 0;
            int currentTestStepID = 0;
            int primaryTestCaseID = 0;
            bool result1 = int.TryParse(this.hdnCurrentTestCaseID.Value, out currentTestCaseID);
            bool result2 = int.TryParse(this.hdnCurrentTestStepID.Value, out currentTestStepID);
            bool result3 = int.TryParse(this.hdnPrimaryTestCaseID.Value, out primaryTestCaseID);
            if (result1 && result2 && result3)
            {
                tr.CurrentTestCaseID = currentTestCaseID;
                tr.PrimaryTestCaseID = primaryTestCaseID;
                tr.TestStepID = currentTestStepID;
                viewTestResult nextTestStep = tcBLL.getNextTestStepForTestRunner(tr);
                this.hdnCurrentTestCaseID.Value = nextTestStep.CurrentTestCaseID.ToString();
                this.hdnCurrentTestStepID.Value = nextTestStep.TestStepID.ToString();
                this.hdnPrimaryTestCaseID.Value = nextTestStep.PrimaryTestCaseID.ToString();
                //get test step object
                var ts = tcBLL.getTestStepByID(nextTestStep.TestStepID);
                //populate failed BR dropdown
                this.ddlSelectFailedBR.Items.Clear();
                this.hdnShowSelectBR.Value = "";
                if (ts.RelatedBusinessRules.Count > 0)
                {
                    if (ts.RelatedBusinessRules.Count > 1)
                    {
                        this.hdnShowSelectBR.Value = "true";
                    }
                    foreach (var br in ts.RelatedBusinessRules)
                    {
                        this.ddlSelectFailedBR.Items.Add(new ListItem(br.Name, br.ID.ToString()));
                    }
                }
                displayTestStep(nextTestStep);
                displayTestResults(nextTestStep.CurrentTestCaseID, nextTestStep.TestStepID);
            }
            else
            {
                this.txtTestStep.Text = "Internal Error.  Contact administrator.";
            }
        }

        #endregion event handlers

        #region private methods

        private void displayTestResults(int testCaseID, int testStepID) 
        {
            string testResult = tcBLL.getLatestTestResultString(testCaseID, testStepID);
            this.lblTestResult.Text = testResult;
            if (testResult.Equals("PASS"))
            {
                this.lblTestResult.ForeColor = Color.Green;
            }
            else if (testResult.Equals("FAIL"))
            {
                this.lblTestResult.ForeColor = Color.Red;
            }
            else
            {
                this.lblTestResult.ForeColor = Color.Black;
            }
        }

        private string saveFailedTestResults()
        {
            int userID = Convert.ToInt32(Session.Contents["userID"]);
            string failNotes = this.txtNotes.Text;
            int currentTestCaseID = 0;
            int currentTestStepID = 0;
            int primaryTestCaseID = 0;
            int failedBusinessRuleID = 0;
            bool result2 = int.TryParse(this.hdnCurrentTestCaseID.Value, out currentTestCaseID);
            result2 = int.TryParse(this.hdnCurrentTestStepID.Value, out currentTestStepID);
            result2 = int.TryParse(this.hdnPrimaryTestCaseID.Value, out primaryTestCaseID);
            result2 = int.TryParse(this.hdnFailedBusinessRuleID.Value, out failedBusinessRuleID);
            viewTestResult tr = new viewTestResult(0,
                                                   primaryTestCaseID,
                                                   currentTestCase.ID,
                                                   currentTestStep.ID,
                                                   (int)Enums.testStatusEnums.TestStatus.Fail,
                                                   userID,
                                                   DateTime.Now,
                                                   failNotes,
                                                   failedBusinessRuleID);
            string result = tcBLL.saveTestResult(tr);
            return result;
        }

        #endregion private methods

    }
}