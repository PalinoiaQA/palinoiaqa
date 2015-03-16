using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Entities;
using Enums;
using System.Web.UI.HtmlControls;

namespace Palinoia.UI.Admin
{    
    /// <summary>
    /// class to hold the code for adminProject
    /// </summary>
    public partial class adminProject : basePalinoiaPage
    {
        #region properties and variables

        applicationBLL bll;
        bool disableEdit = true;
        bool disableDelete = true;
        int userID, projectID;

        #endregion properties and variables

        #region page lifecycle methods
                
        /// <summary>
        /// initializes a page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            this.addJavaScriptReference("Admin/adminProject.js");
        }
                
        /// <summary>
        /// loads a page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //check for valid user id
            userID = Convert.ToInt32(Session.Contents["userID"]);
            //projectID = Convert.ToInt32(Session.Contents["projectID"]);
           // this.hdnProjectID.Value = projectID.ToString();
            projectID = 0;
            var parseResult = int.TryParse(this.hdnProjectID.Value, out projectID);
            if (userID > 0)
            {
                bll = new applicationBLL();
                applyFeatures(userID);
                if (!IsPostBack)
                {
                    populateProjectsGrid();
                }
            }
            else
            {
                //user invalid.  redirect to login
                Response.Redirect("~/Account/Login.aspx");
            }
        }

        #endregion page lifecycle methods

        #region event handlers
                
        /// <summary>
        /// grdProjects_RowDataBound event - apply features and delete confirm message 
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewRowEventArgs</param>
        protected void grdProjects_RowDataBound(object sender, GridViewRowEventArgs e)
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
        /// handles events when grid row is deleted
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewDeleteEventArgs</param>
        protected void grdProjects_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //get project id from grid
            int selectedIndex = e.RowIndex;
            int deleteProjectID = Convert.ToInt32(grdProjects.Rows[selectedIndex].Cells[0].Text);
            string projectName = grdProjects.Rows[selectedIndex].Cells[1].Text;
            string result = bll.deleteProject(deleteProjectID, this.userID);
            if (result.Equals("OK"))
            {
                StringBuilder message = new StringBuilder();
                message.Append("Project");
                message.Append(" ");
                message.Append(projectName);
                message.Append(" ");
                message.Append("has been deleted.");
                sendMessageToClient(message.ToString());
                //is user currently in the project being deleted?
                var currentProjectID = Convert.ToInt32(Session.Contents["projectID"]);
                if (currentProjectID == deleteProjectID)
                {
                    //reset projectID session variable and hidden field on page to zero
                    Session.Add("projectID", "0");
                    this.hdnProjectID.Value = "0";
                    bll.updateUserProject(userID, 0);
                    Label projLabel = (Label)Master.FindControl("ProjectLabel");
                    projLabel.Text = "Palinoia";
                    Panel navMenu = ((Panel)Master.FindControl("Panel1"));
                    navMenu.Style.Add("display", "none");
                }
            }
            else
            {
                sendMessageToClient(result);
            }
            populateProjectsGrid();
        }
                
        /// <summary>
        /// handles events when grid row is edited
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewEditEventArgs</param>
        protected void grdProjects_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //get project id from grid
            int selectedIndex = e.NewEditIndex;
            int projectID = Convert.ToInt32(grdProjects.Rows[selectedIndex].Cells[0].Text);
            hdnProjectID.Value = projectID.ToString();
            txtNewProjectName.Text = grdProjects.Rows[selectedIndex].Cells[1].Text;
            //prevent further asp gridview events for editing
            e.Cancel = true;
            showDialog(true);
        }
                
        /// <summary>
        /// handles events when selected index within grid changes
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewSelectEventArgs</param>
        protected void grdProjects_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            int selectedIndex = e.NewSelectedIndex;
            int projectID = Convert.ToInt32(grdProjects.Rows[selectedIndex].Cells[0].Text);
            this.hdnProjectID.Value = projectID.ToString();
            string projectName = grdProjects.Rows[selectedIndex].Cells[1].Text;
            Session.Add("projectID", projectID);
            //sendMessageToClient("Working project is now " + projectName);
            Label projLabel = (Label)Master.FindControl("ProjectLabel");
            projLabel.Text = projectName;
            //update user project in DB
            string result = bll.updateUserProject(this.userID, projectID);
            if (!result.Equals("OK"))
            {
                sendMessageToClient(result);
            }
            //populateProjectsGrid();
            Response.Redirect("~/UI/Default.aspx");
        }
                
        /// <summary>
        /// handles events when save add project button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnSaveProject_Click(object sender, EventArgs e)
        {
            int projectID = 0;
            bool parseResult = int.TryParse(this.hdnProjectID.Value, out projectID);
            string projectName = this.txtNewProjectName.Text;
            //string appPath = HttpRuntime.AppDomainAppPath;
            string workingDir = ConfigurationManager.AppSettings["WorkingDirectory"];
            //string projectPath = appPath + projectDataPath + "\\";
            //string projectPath = projectDataPath + "//" + projectName;
            string result = "";
            bool showNewMessage = false;
            if (!bll.isValidText(projectName))
            {
                result = "Invalid text";
            }
            else
            {
                if (projectID == 0)
                {
                    result = bll.createNewProject(projectName, workingDir, this.userID);
                    int newProjectID = 0;
                    parseResult = int.TryParse(result, out newProjectID);
                    if (parseResult)
                    {
                        Session.Add("projectID", newProjectID);
                        showNewMessage = true;
                        result = bll.updateUserProject(this.userID, newProjectID);
                    }
                }
                else
                {
                    viewProject project = new viewProject(projectID, projectName, "", true, this.userID);
                    result = bll.updateProject(project);
                    var currentProjectID = Convert.ToInt32(Session.Contents["projectID"]);
                    if (currentProjectID == projectID)
                    {
                        Label projLabel = (Label)Master.FindControl("ProjectLabel");
                        projLabel.Text = projectName;
                    }
                }
            }
            if (!result.Equals("OK"))
            {
                StringBuilder errorSB = new StringBuilder();
                errorSB.Append(" ");
                errorSB.Append(result);
                string error = cleanError(errorSB.ToString());
                sendHTMLMessageToClient(error);
            }
            else
            {
                if (showNewMessage)
                {
                    sendMessageToClient("New Project " + projectName + " created.");
                    Label projLabel = (Label)Master.FindControl("ProjectLabel");
                    projLabel.Text = projectName;
                }
            }
            populateProjectsGrid();
        }
        
        #endregion event handlers

        #region UI controls
                
        /// <summary>
        /// populate projects grid
        /// </summary>
        public void populateProjectsGrid()
        {
            grdProjects.DataSource = null;
            grdProjects.DataBind();
            var projectList = bll.getAllProjects();
            grdProjects.DataSource = projectList;
            grdProjects.DataBind();
        }

        #endregion UI controls

        #region private methods

        private void reloadMasterPage() {
            // Define the name and type of the client script on the page.
            String csName = "ReloadPageScript";
            Type csType = this.GetType();
            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs = Page.ClientScript;
            // Check to see if the client script is already registered.
            if (!cs.IsStartupScriptRegistered(csType, csName))
            {
                ClientScript.RegisterStartupScript(typeof(Page), csName, "<script type='text/javascript'>reloadPage();</script>");
            }
        }

                
        /// <summary>
        /// replaces non-text characters with a space
        /// </summary>
        /// <param name="s">string</param>
        /// <returns></returns>
        private string cleanError(string s)
        {
            string error = s.Replace("\r", " ");
            error = error.Replace("\n", " ");
            return error;
        }
                
        /// <summary>
        /// apply features
        /// </summary>
        /// <param name="userID">int</param>
        private void applyFeatures(int userID)
        {
            viewUser user = bll.getUserByID(userID);
            List<viewFeature> userFeatureList = bll.getFeaturesForUser(user);
            //VIEW
            viewFeature feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminProjectsView);
            if (feature == null)
            {
                //user cannot view screen.  redirect to default display
                sendMessageToClient("You do not have the necessary permission to view this screen.");
                grdProjects.Visible = false;
            }
            //ADD
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminProjectsAdd);
            if (feature != null)
            {
                this.btnNewProject.Visible = true;
            }
            else
            {
                this.btnNewProject.Visible = false;
            }
            //EDIT
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminProjectsEdit);
            if (feature != null)
            {
                disableEdit = false;
            }
            else
            {
                disableEdit = true;
            }
            //DELETE
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminProjectsDelete);
            if (feature != null)
            {
                disableDelete = false;
            }
            else
            {
                disableDelete = true;
            }
        }

        #endregion private methods

    }
}