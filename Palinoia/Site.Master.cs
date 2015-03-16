using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entities;
using BLL;
using Enums;
using System.Web.Services;
using System.Web.Script.Services;
using System.Text;

namespace Palinoia
{    
    /// <summary>
    /// class to hold the code for SiteMaster
    /// </summary>
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        #region properties and variables

        applicationBLL palinoiaBLL;

        #endregion properties and variables

        /// <summary>
        /// initializes a page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            Page.Header.DataBind();
        }
        
        /// <summary>
        /// loads a page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            int userID = Convert.ToInt32(Session.Contents["userID"]);
            this.hdnUserID.Value = userID.ToString();
            int projectID = Convert.ToInt32(Session.Contents["projectID"]);
            this.hdnProjectID.Value = projectID.ToString();
            if (userID > 0)
            {
                var bll = new applicationBLL(0);
                var user = bll.getUserByID(userID);
                this.lblLoginName.Text = user.FirstName;
                this.validUserDiv.Visible = true;
                this.invalidUserDiv.Visible = false;
                if (projectID > 0)
                {
                    palinoiaBLL = new applicationBLL();
                    var proj = palinoiaBLL.getProjectByID(projectID);
                    this.ProjectLabel.Text = proj.Name;
                }
                //set session timeout variables on client
                string timeout = this.GetEnvData("SessionTimeout");
                string warning = this.GetEnvData("SessionWarning");
                startTimeoutMonitor(timeout, warning);
            }
            else
            {
                this.invalidUserDiv.Visible = true;
                this.validUserDiv.Visible = false;
            }
        }

        /// <summary>
        /// add JavaSctipt references
        /// </summary>
        /// <param name="jsFileName"></param>
        /// <param name="extraScripts">string</param>
        public void addJavaScriptReferences(string jsFileName, string extraScripts)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script src=\"../../Scripts/Palinoia/" + jsFileName + "\" type=\"text/javascript\"></script>");
            LiteralControl javascriptRef = new LiteralControl(sb.ToString());
            Page.Header.Controls.Add(javascriptRef);
        }

        /// <summary>
        /// fetch environment data
        /// </summary>
        /// <param name="library">string</param>
        /// <returns>string</returns>
        public string GetEnvData(string library)
        {
            string returnValue = @"";
            returnValue = @System.Configuration.ConfigurationManager.AppSettings[library].ToString();
            return @returnValue;
        }
                
        /// <summary>
        /// adds JavaScript references to web pages as they load
        /// </summary>
        /// <param name="timeout">string</param>
        /// <param name="warning">string</param>
        public void startTimeoutMonitor(string timeout, string warning)
        {
            //make sure page is not loaded outside of the master page
            if (Page.Header != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<script type='text/javascript'>startTimeoutMonitor(" + timeout + "," + warning + ");</script>");
                LiteralControl javascriptRef = new LiteralControl(sb.ToString());
                Page.Header.Controls.Add(javascriptRef);
            }
        }
    }
}
