using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Entities;

namespace Palinoia.Account
{    
    /// <summary>
    /// class to hold code for Login
    /// </summary>
    public partial class Login : UI.basePalinoiaPage
    {
        applicationBLL bll;

        #region page lifecycle events
                
        /// <summary>
        /// loads a page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //clear user id from session
            Session["userID"] = 0;
            bll = new applicationBLL();
        }

        #endregion page lifecycle events

        #region event handlers
                
        /// <summary>
        /// handles events when login button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string userEmail = txtUserName.Text.Trim();
            string userPassword = txtPassword.Text.Trim();
            viewUser user = new viewUser(0, 0, "", "", "", userEmail, userPassword, true, "", 0);
            int userID = bll.loginUser(user);
            if (userID == 0)
            {
                Session.Add("userid", 0);
                sendMessageToClient("Invalid Login!");
            }
            else // valid user
            {
                Session.Add("userid", userID);
                int lastProjectIDForUser = bll.getProjectForUser(userID);
                Session.Add("projectID", lastProjectIDForUser);
                if (lastProjectIDForUser > 0)
                {
                    var project = bll.getProjectByID(lastProjectIDForUser);
                    Label projLabel = (Label)Master.FindControl("ProjectLabel");
                    projLabel.Text = project.Name;
                    Response.Redirect("~/UI/Default.aspx");
                }
                else
                {
                    Label projLabel = (Label)Master.FindControl("ProjectLabel");
                    projLabel.Text = "Palinoia";
                    Response.Redirect("~/UI/Admin/adminProject.aspx");
                }
            }

        }

       
        /// <summary>
        /// handles events when the auto log in button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void AutoLogin_Click(object sender, EventArgs e)
        {
            string userEmail = "administrator@palinoia.org";
            string userPassword = "letmein1";
            viewUser user = new viewUser(0, 0, "", "", "", userEmail, userPassword, true, "", 1);
            int userID = bll.loginUser(user);
            Session.Add("userid", userID);
            int lastProjectIDForUser = bll.getProjectForUser(userID);
            Session.Add("projectID", lastProjectIDForUser);
            
            if (lastProjectIDForUser > 0)
            {
                var project = bll.getProjectByID(lastProjectIDForUser);
                Label projLabel = (Label)Master.FindControl("ProjectLabel");
                projLabel.Text = project.Name;
                Response.Redirect("~/UI/Default.aspx");
            }
            else
            {
                Label projLabel = (Label)Master.FindControl("ProjectLabel");
                projLabel.Text = "Palinoia";
                Response.Redirect("~/UI/Admin/adminProject.aspx");
            }
        }
        #endregion event handlers
    }
}
