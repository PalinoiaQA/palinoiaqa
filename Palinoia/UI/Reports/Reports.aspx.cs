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

namespace Palinoia.UI.Reports
{
    /// <summary>
    /// Reports screen
    /// </summary>
    public partial class Reports : basePalinoiaPage
    {
        #region Properties and Variables

        AdminBLL adminBLL;
        int userID;
        int projectID;
        int sectionID;
        int statusID;

        #endregion Properties and Variables

        #region Page Lifecycle events

        /// <summary>
        /// Page_Init Method.  Adds necessary JS references
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            this.addJavaScriptReference("Reports/Reports.js");
        }

        /// <summary>
        /// Page_Load method.  Sets up UI and class variables
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            sectionID = 0;
            int.TryParse(this.hdnSectionID.Value, out sectionID);
            statusID = 0;
            int.TryParse(this.hdnStatusID.Value, out statusID);
            projectID = Convert.ToInt32(Session.Contents["projectID"]);
            userID = Convert.ToInt32(Session.Contents["userID"]);
            adminBLL = new AdminBLL(projectID);
            if (userID > 0)
            {
                if (!IsPostBack)
                {
                    //no features applied, all users can view all reports - KLH 9-26-2014
                    populateSectionsDDL();
                    populateStatusDDL();
                }
            }
            else
            {
                //user invalid.  redirect to login
                Response.Redirect("~/Account/Login.aspx");
            }
        }

        #endregion Page Lifecycle Events

        #region Event Handlers

        /// <summary>
        /// Event handler for report link 1
        /// Business Rules not referenced in test cases report
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            //Business Rules without a Test Case
            projectID = Convert.ToInt32(Session.Contents["projectID"]);
            var docBLL = new DocumentsBLL(projectID);
            openPDFWindow(docBLL.getViewReportRedirectURL((int)Report.ReportType.TESTCASE_ORPHANED_BUSINESSRULES));

        }

        /// <summary>
        /// Event handler for report link 2
        /// //Business Rules not referenced in documents
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            //Business Rules not referenced in documents
            projectID = Convert.ToInt32(Session.Contents["projectID"]);
            var docBLL = new DocumentsBLL(projectID);
            openPDFWindow(docBLL.getViewReportRedirectURL((int)Report.ReportType.DOCUMENT_ORPHANED_BUSINESSRULES));
        }

        /// <summary>
        /// Event handler for report link 3
        /// Business Rules not referenced in Functional documents
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            //Business Rules not referenced in Functional documents
            projectID = Convert.ToInt32(Session.Contents["projectID"]);
            var docBLL = new DocumentsBLL(projectID);
            openPDFWindow(docBLL.getViewReportRedirectURL((int)Report.ReportType.FUNCTIONAL_DOCUMENT_ORPHANED_BUSINESSRULES));
        }

        /// <summary>
        /// Event handler for report link 4
        /// Business Rules not referenced in Technical documents
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            //Business Rules not referenced in Technical documents
            projectID = Convert.ToInt32(Session.Contents["projectID"]);
            var docBLL = new DocumentsBLL(projectID);
            openPDFWindow(docBLL.getViewReportRedirectURL((int)Report.ReportType.TECHNICAL_DOCUMENT_ORPHANED_BUSINESSRULES));
        }

        /// <summary>
        /// Event handler for report link 5
        /// Business Rules not referenced in Miscellaneous documents
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void LinkButton5_Click(object sender, EventArgs e)
        {
            //Business Rules not referenced in Miscellaneous documents
            projectID = Convert.ToInt32(Session.Contents["projectID"]);
            var docBLL = new DocumentsBLL(projectID);
            openPDFWindow(docBLL.getViewReportRedirectURL((int)Report.ReportType.MISCELLANEOUS_DOCUMENT_ORPHANED_BUSINESSRULES));
        }

        /// <summary>
        /// view button click event handler for Business Rule List by Section report
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnBRbySection_Click(object sender, EventArgs e)
        {
            //get section id
            if (sectionID == 0)
            {
                sendMessageToClient("Section is required");
            }
            else
            {
                projectID = Convert.ToInt32(Session.Contents["projectID"]);
                DocumentsBLL docBLL = new DocumentsBLL(projectID);
                openPDFWindow(docBLL.getViewReportRedirectURL((int)Report.ReportType.BUSINESSRULES_BY_SECTION, sectionID));
            }
        }

        /// <summary>
        /// Event handler for Business Rules by Status report view button
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnBRbyStatus_Click(object sender, EventArgs e)
        {
            //get status id
            if (statusID == 0)
            {
                sendMessageToClient("Status is required");
            }
            else
            {
                projectID = Convert.ToInt32(Session.Contents["projectID"]);
                DocumentsBLL docBLL = new DocumentsBLL(projectID);
                openPDFWindow(docBLL.getViewReportRedirectURL((int)Report.ReportType.BUSINESSRULES_BY_STATUS, statusID));
            }
        }

        #endregion Event Handlers

        #region Private methods

        private void openPDFWindow(string redirectURL)
        {
            //// Define the name and type of the client scripts on the page.
            String csname = "OpenPDFTabScript";
            Type cstype = this.GetType();
            String cstext = "window.open(\"" + redirectURL + "\");";
            ScriptManager.RegisterStartupScript(this, cstype, csname, cstext, true);
        }

        /// <summary>
        /// populates the sections ddl for the Business Rules by Sections report
        /// </summary>
        private void populateSectionsDDL()
        {
            ddlSections.Items.Clear();
            ddlSections.Items.Add(new ListItem("Select one", "0"));
            List<viewSection> sectionList = adminBLL.getAllSections();
            foreach (var section in sectionList)
            {
                ddlSections.Items.Add(new ListItem(section.Text, section.ID.ToString()));
            }
        }

        /// <summary>
        /// populates the status ddl for the Business Rules by status report
        /// </summary>
        private void populateStatusDDL()
        {
            ddlStatus.Items.Clear();
            ddlStatus.Items.Add(new ListItem("Select one", "0"));
            List<viewStatus> statusList = adminBLL.getAllStatuses();
            foreach (var status in statusList)
            {
                ddlStatus.Items.Add(new ListItem(status.Text, status.ID.ToString()));
            }
        }

        #endregion Private methods
    }
}