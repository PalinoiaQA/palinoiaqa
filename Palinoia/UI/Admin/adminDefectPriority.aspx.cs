using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entities;
using BLL;
using Enums;

namespace Palinoia.UI.Admin
{
    /// <summary>
    /// class to hold code for adminDefectPriority object
    /// </summary>
    public partial class adminDefectPriority : basePalinoiaPage
    {
        #region Properties and Variables

        AdminBLL adminBLL;
        applicationBLL palinoiaBLL;
        bool disableDelete;
        bool disableEdit;
        int userID;

        #endregion Properties and Variables

        #region Page lifecycle events
                
        /// <summary>
        /// initializes a page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            this.addJavaScriptReference("Admin/adminDefectPriority.js");
        }
                
        /// <summary>
        /// loads a page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            userID = Convert.ToInt32(Session.Contents["userID"]);
            if (userID > 0)
            {
                int projectID = Convert.ToInt32(Session.Contents["projectID"]);
                //int projectID = 7;
                adminBLL = new AdminBLL(projectID);
                palinoiaBLL = new applicationBLL();
                applyFeatures(userID);
                if (!IsPostBack)
                {
                    populateDefectPriorityGrid();
                }
            }
            else
            {
                //user invalid.  redirect to login
                Response.Redirect("~/Account/Login.aspx");
            }

        }
        #endregion Page lifecycle events

        #region event handlers
                
        /// <summary>
        /// handles events when save edit Defect Priority button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnSaveDefectPriority_Click(object sender, EventArgs e)
        {
            //get id of selected Defect Priority
            int selectedID = Convert.ToInt32(this.hdnDefectPriorityID.Value);
            //get updated text from textbox
            string newText = this.txtAddDefectPriority.Text;
            bool active = this.chkActive.Checked;
            int importance = 1; // TODO: ADD importance textfield to ui
            //required field check
            if (newText.Length == 0)
            {
                sendMessageToClient("Defect Priority is required.");
            }
            else
            {
                //no special chars check
                if (palinoiaBLL.isValidText(newText))
                {
                    string result = "";
                    var docPriority = new viewDefectPriority(selectedID, newText, active, this.userID, importance);
                    if (selectedID == 0)
                    {
                        int newID = 0;
                        result = adminBLL.addDefectPriority(docPriority);
                        bool parseResult = int.TryParse(result, out newID);
                        if (newID > 0)
                        {
                            result = "OK";
                        }
                    }
                    else
                    {
                        result = adminBLL.updateDefectPriority(docPriority);
                    }
                    if (!result.Equals("OK"))
                    {
                        sendMessageToClient(result);
                    }
                    //redraw grid
                    populateDefectPriorityGrid();
                }
                else
                {
                    sendMessageToClient("Special characters are not allowed.");
                }
            }
        }
                
        /// <summary>
        /// UNFINISHED
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewRowEventArgs</param>
        protected void grdDefectPriority_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void grdDefectPriority_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var strSelectedIndex = e.RowIndex;
            int selectedIndex = Convert.ToInt32(strSelectedIndex);
            int deleteID = Convert.ToInt32(grdDefectPriority.Rows[selectedIndex].Cells[0].Text);
            var result = adminBLL.deleteDefectPriority(deleteID, this.userID);
            //alert client if error
            if (result.Equals("OK"))
            {
                string error = result;
            }
            //redraw grid
            populateDefectPriorityGrid();
        }
                
        /// <summary>
        /// handles events when grid row is edited
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewEditEventArgs</param>
        protected void grdDefectPriority_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //get id of selected csm reponse Priority
            int selectedIndex = e.NewEditIndex;
            string strID = this.grdDefectPriority.Rows[selectedIndex].Cells[0].Text;
            int selectedID = Convert.ToInt32(strID);
            viewDefectPriority DefectPriority = adminBLL.getDefectPriorityByID(selectedID);
            //populate text box for editing
            this.txtAddDefectPriority.Text = DefectPriority.Text;
            this.chkActive.Checked = DefectPriority.Active;
            //populate hidden field on client with response Priority id
            this.hdnDefectPriorityID.Value = selectedID.ToString();
            //prevent further asp gridview events for editing
            e.Cancel = true;
            showDialog(true);
        }

        #endregion event handlers

        #region ui controls
               
        /// <summary>
        /// apply features
        /// </summary>
        /// <param name="userID">int</param>
        public void applyFeatures(int userID)
        {
            viewUser user = palinoiaBLL.getUserByID(userID);
            List<viewFeature> userFeatureList = palinoiaBLL.getFeaturesForUser(user);
            //VIEW
            viewFeature feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminDefectPriorityView);
            if (feature != null)
            {

            }
            else
            {
                //user cannot view screen.  redirect to default display
                sendMessageToClient("You do not have the necessary permission to view this screen.");
                this.grdDefectPriority.Visible = false;
            }
            //ADD
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminDefectPriorityAdd);
            if (feature != null)
            {
                this.btnAddDefectPriority.Visible = true;
            }
            else
            {
                this.btnAddDefectPriority.Visible = false;
            }
            //EDIT
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminDefectPriorityEdit);
            if (feature != null)
            {
                disableEdit = false;
            }
            else
            {
                disableEdit = true;
            }
            //DELETE
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminDefectPriorityDelete);
            if (feature != null)
            {
                disableDelete = false;
            }
            else
            {
                disableDelete = true;
            }
        }
                
        /// <summary>
        /// populate Defect Priority grid
        /// </summary>
        public void populateDefectPriorityGrid()
        {
            List<viewDefectPriority> DefectPriorityList = adminBLL.getAllDefectPriorities();
            grdDefectPriority.DataSource = DefectPriorityList;
            grdDefectPriority.DataBind();
        }

        #endregion ui controls
    }
}