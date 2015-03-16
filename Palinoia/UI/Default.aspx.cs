using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data.Entity;
using Entities;
using Enums;
using System.Web.Services;
using System.Web.Script.Services;
using System.Text;

namespace Palinoia
{    
    /// <summary>
    /// class to hold code for _Default
    /// </summary>
    public partial class _Default : UI.basePalinoiaPage
    {
        #region Properties and Variables

        applicationBLL appBLL;
        AdminBLL adminBLL;
        int userID;
        int projectID;

        #endregion Properties and Variables

        #region page lifetime events
                
        /// <summary>
        /// loads a page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //check for valid user id
            userID = Convert.ToInt32(Session.Contents["userID"]);
            projectID = Convert.ToInt32(Session.Contents["projectID"]);
            this.hdnProjectID.Value = projectID.ToString();
            if (userID > 0)
            {
                appBLL = new applicationBLL();
                adminBLL = new AdminBLL(projectID);
                if (!IsPostBack)
                {
                    populateProjectData();
                    var project = appBLL.getProjectByID(projectID);
                    Label projLabel = (Label)Master.FindControl("ProjectLabel");
                    projLabel.Text = project.Name;
                }
            }
            else
            {
                //user invalid.  redirect to login
                Response.Redirect("~/Account/Login.aspx");
            }
        }

        #endregion page lifetime events

        #region Event Handlers

        
        #endregion Event Handlers

        #region UI Controls

        private void populateProjectData() {
            var project = appBLL.getProjectByID(projectID);
            this.lblProjectName.Text = project.Name;
            this.lblBusinessRulesTotal.Text = getTotalBusinessRules();
            this.lblCustomerServiceMessagesTotal.Text = getTotalCustomerServiceMessages();
            populateTotalTestCases();
            populateAssociatedDocuments();
        }

        private string getTotalBusinessRules()
        {
            string total = "";
            var brBLL = new BusinessRulesBLL(projectID);
            var brList = brBLL.getAllBusinessRules();
            total = brList.Count.ToString();
            brList = null;
            brBLL = null;
            return total;
        }

        private string getTotalCustomerServiceMessages()
        {
            string total = "";
            var csmBLL = new CustomerServiceMessagesBLL(projectID);
            var csmList = csmBLL.getAllCSMs();
            total = csmList.Count.ToString();
            csmList = null;
            csmBLL = null;
            return total;
        }

        private void populateTotalTestCases()
        {
            string total = "";
            var tcBLL = new TestCasesBLL(projectID);
            var tcList = tcBLL.getAllTestCases();
            total = tcList.Count.ToString();
            this.lblTestCasesTotal.Text = total;
            this.lblPassedTotal.Text = getTotalPassed(tcList);
            this.lblFailedTotal.Text = getTotalFailed(tcList);
            this.lblUntestedTotal.Text = getTotalUntested(tcList);
            tcList = null;
            tcBLL = null;
        }

        private string getTotalPassed(List<viewTestCase> tcList)
        {
            string total = "";
            var passList = tcList.FindAll((tc) => tc.TestStatusID == (int)Enums.testStatusEnums.TestStatus.Pass);
            total = passList.Count.ToString();
            return total;
        }

        private string getTotalFailed(List<viewTestCase> tcList)
        {
            string total = "";
            var failList = tcList.FindAll((tc) => tc.TestStatusID == (int)Enums.testStatusEnums.TestStatus.Fail);
            total = failList.Count.ToString();
            return total;
        }

        private string getTotalUntested(List<viewTestCase> tcList)
        {
            string total = "";
            var untestedList = tcList.FindAll((tc) => tc.TestStatusID == (int)Enums.testStatusEnums.TestStatus.Untested);
            total = untestedList.Count.ToString();
            return total;
        }

        private void populateAssociatedDocuments()
        {
            var docBLL = new DocumentsBLL(projectID);
            var docList = docBLL.getAllActiveDocuments();
            //filter out test case docs
            docList = docList.FindAll((d) => d.DocumentTypeID != (int)Enums.documentTypeEnums.DocumentType.TestCase);
            if (docList.Count == 0)
            {
                lbAssociatedDocuments.Items.Add("<None>");
            }
            foreach (var doc in docList)
            {
                lbAssociatedDocuments.Items.Add(doc.Name);
            }
        }


        #endregion UI Controls

        
    }
}
