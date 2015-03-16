using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Entities;
using Enums;
using System.Web.Services;

namespace Palinoia.UI.Admin
{
    /// <summary>
    /// class to hold code for adminErrorLog 
    /// </summary>
    public partial class adminErrorLog : basePalinoiaPage
    {
        #region properties and variables

        applicationBLL palinoiaBLL;
        int userID;
        int projectID;

        #endregion properties and variables

        #region page lifecycle events

        /// <summary>
        /// initializes a page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            this.addJavaScriptReference("Admin/adminErrorLog.js");
        }

        /// <summary>
        /// load a page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //check for valid user id
            userID = Convert.ToInt32(Session.Contents["userID"]);
            projectID = Convert.ToInt32(Session.Contents["projectID"]);
            if (userID > 0)
            {
                palinoiaBLL = new applicationBLL();
                this.lblNoErrors.Text = "";
                populateErrorsGrid();
            }
            else
            {
                //user invalid.  redirect to login
                Response.Redirect("~/Account/Login.aspx");
            }
        }

        #endregion page lifecycle events

        #region private methods

        /// <summary>
        /// populate the errors grid
        /// </summary>
        private void populateErrorsGrid()
        {
            this.grdErrors.DataSource = null;
            this.grdErrors.DataBind();
            var errorList = palinoiaBLL.getAllActiveErrors();
            if (errorList.Count > 0)
            {
                this.grdErrors.DataSource = errorList;
                this.grdErrors.DataBind();
            }
            else
            {
                this.lblNoErrors.Text = "No Errors";
            }
        }

        #endregion private methods

        #region event handlers

        /// <summary>
        /// Adds details link to each error record in grid
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewRowEventArgs</param>
        protected void grdErrors_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var errorID = 0;
            if (e.Row.RowIndex > -1)
            {
                bool parseResult = int.TryParse(e.Row.Cells[0].Text, out errorID);
                // loop all data cells
                e.Row.Cells.Add(new TableCell());
                LinkButton details = new LinkButton();
                details.Text = "details";
                details.OnClientClick = "showErrorDetails(" + errorID + "); return false;";
                e.Row.Cells[5].Controls.Add(details);
            }
        }

        #endregion event handlers

        #region webmethods

        /// <summary>
        /// sets active flag to false for all active error records in Application_Errors
        /// </summary>
        /// <returns>string</returns>
        [WebMethod]
        public static string clearErrors()
        {
            applicationBLL appBLL = new applicationBLL();
            appBLL.clearErrors();
            return "OK";
        }

        /// <summary>
        /// returns viewError records for specific error selected in grid
        /// </summary>
        /// <param name="strErrorID">string</param>
        /// <returns>viewError</returns>
        [WebMethod]
        public static viewError getErrorDetails(string strErrorID)
        {
            var error = new viewError();
            var errorID = 0;
            var parseResult = int.TryParse(strErrorID, out errorID);
            var appBLL = new applicationBLL();
            error = appBLL.getErrorByID(errorID);
            appBLL = null;
            return error;
        }

        /// <summary>
        /// get project for id to display on client
        /// </summary>
        /// <param name="strProjectID">string</param>
        /// <returns>string</returns>
        [WebMethod]
        public static string getProjectName(string strProjectID)
        {
            string projectName = "";
            int projectID = 0;
            bool parseResult = int.TryParse(strProjectID, out projectID);
            if (projectID > 0)
            {
                var appBLL = new applicationBLL();
                var project = appBLL.getProjectByID(projectID);
                projectName = project.Name;
                project = null;
                appBLL = null;
            }
            else
            {
                projectName = "UNKNOWN";
            }
            return projectName;
        }

        /// <summary>
        /// get user name for id to display on client
        /// </summary>
        /// <param name="strUserID">string</param>
        /// <returns>string</returns>
        [WebMethod]
        public static string getUserName(string strUserID)
        {
            string userName = "";
            int userID = 0;
            bool parseResult = int.TryParse(strUserID, out userID);
            if (userID > 0)
            {
                var appBLL = new applicationBLL();
                var user = appBLL.getUserByID(userID);
                userName = user.getFullNameFNF();
                user = null;
                appBLL = null;
            }
            else
            {
                userName = "UNKNOWN";
            }
            return userName;
        }

        #endregion webmethods
        
    }
}