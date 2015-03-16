using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Entities;
using Enums;


namespace Palinoia.UI.Admin
{    
    /// <summary>
    /// class to hold code for adminRoles
    /// </summary>
    public partial class adminRoles : basePalinoiaPage
    {
        #region properties and variables

        /// <summary>
        /// class variable for adminBLL
        /// </summary>
        AdminBLL adminBLL;
        /// <summary>
        /// class variable for palinoiaBLL
        /// </summary>
        applicationBLL palinoiaBLL;
        /// <summary>
        /// class variable for disableEdit
        /// </summary>
        bool disableEdit;
        /// <summary>
        /// class variable for disableDelete
        /// </summary>
        bool disableDelete;
        int userID;

        #endregion properties and variables

        #region Page lifecycle events
                
        /// <summary>
        /// initializes a page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            this.addJavaScriptReference("Admin/adminRoles.js");
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
            if (userID > 0)
            {
                int projectID = Convert.ToInt32(Session.Contents["projectID"]);
                //int projectID = 7;
                adminBLL = new AdminBLL(projectID);
                palinoiaBLL = new applicationBLL();
                applyFeatures(userID);
                if (!IsPostBack)
                {
                    populateRolesGrid();
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
        /// handles events when grid row is deleted
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewDeleteEventArgs</param>
        protected void grdRoles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var strSelectedIndex = e.RowIndex;
            int selectedIndex = Convert.ToInt32(strSelectedIndex);
            int deleteID = Convert.ToInt32(grdRoles.Rows[selectedIndex].Cells[0].Text);
            var result = palinoiaBLL.deleteRole(deleteID, this.userID);
            //alert client if error
            if (result.Equals("OK"))
            {
                string error = result;
            }
            //redraw grid
            populateRolesGrid();
        }
                
        /// <summary>
        /// handles events when grid row is edited
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewEditEventArgs</param>
        protected void grdRoles_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //get id of selected csm reponse type
            int selectedIndex = e.NewEditIndex;
            string strID = this.grdRoles.Rows[selectedIndex].Cells[0].Text;
            int selectedID = Convert.ToInt32(strID);
            Session.Add("RoleID", selectedID);
            viewRole role = palinoiaBLL.getRoleByID(selectedID);
            //populate text box for editing
            this.txtAddRole.Text = role.Text;
            this.chkActive.Checked = role.Active;
            //populate hidden field on client with response type id
            this.hdnRoleID.Value = selectedID.ToString();
            //prevent further asp gridview events for editing
            e.Cancel = true;
            showDialog(true);
        }
                
        /// <summary>
        /// UNFINISHED
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewRowEventArgs</param>
        protected void grdRoles_RowDataBound(object sender, GridViewRowEventArgs e)
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
        /// handles events when save edit role button is clicked 
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnSaveRole_Click(object sender, EventArgs e)
        {
            //get id of selected csm reponse type
            int selectedID = Convert.ToInt32(this.hdnRoleID.Value);
            //get updated text from textbox
            string newText = this.txtAddRole.Text;
            bool active = this.chkActive.Checked;
            //required field check
            if (newText.Length == 0)
            {
                sendMessageToClient("Role Name is required.");
            }
            else
            {
                //no special chars check
                if (palinoiaBLL.isValidText(newText))
                {
                    string result = "";
                    var role = new viewRole(selectedID, newText, active, this.userID);
                    if (selectedID > 0)
                    {
                        result = palinoiaBLL.updateRole(role);
                    }
                    else
                    {
                        int newID = 0;
                        result = palinoiaBLL.addRole(role);
                        bool parseResult = int.TryParse(result, out newID);
                        if (newID > 0)
                        {
                            result = "OK";
                            this.hdnRoleID.Value = newID.ToString();
                        }
                    }
                    if (!result.Equals("OK"))
                    {
                        sendMessageToClient(result);
                    }
                    //redraw grid
                    populateRolesGrid();
                }
                else
                {
                    sendMessageToClient("Special characters are not allowed.");
                }
            }
        }
                
        /// <summary>
        /// handles events when cancel button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        //protected void btnCancel_Click(object sender, EventArgs e)
        //{
        //    populateRolesGrid();
        //    showAddEditControls(false, "");
        //}
        protected void btnSetFeatures_Click(object sender, EventArgs e)
        {
            if (!this.hdnRoleID.Value.Equals("0"))
            {
                Response.Redirect("~/UI/Admin/adminRoleFeatures.aspx");
            }
            else
            {
                sendMessageToClient("Role ID not set");
            }
        }

        #endregion event handlers

        #region UI Control
                
        /// <summary>
        /// apply features
        /// </summary>
        /// <param name="userID">int</param>
        public void applyFeatures(int userID)
        {
            viewUser user = palinoiaBLL.getUserByID(userID);
            List<viewFeature> userFeatureList = palinoiaBLL.getFeaturesForUser(user);
            //VIEW
            viewFeature feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminRolesView);
            if (feature != null)
            {

            }
            else
            {
                //user cannot view screen.  redirect to default display
                sendMessageToClient("You do not have the necessary permission to view this screen.");
                this.grdRoles.Visible = false;
            }
            //ADD
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminRolesAdd);
            if (feature != null)
            {
                this.btnAddRole.Visible = true;
            }
            else
            {
                this.btnAddRole.Visible = false;
            }
            //EDIT
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminRolesEdit);
            if (feature != null)
            {
                disableEdit = false;
            }
            else
            {
                disableEdit = true;
            }
            //DELETE
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminRolesDelete);
            if (feature != null)
            {
                disableDelete = false;
            }
            else
            {
                disableDelete = true;
            }
            //Set RoleFeatures
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminRoleFeaturesView);
            if (feature != null)
            {
                this.btnSetFeatures.Visible = true;
            }
            else
            {
                this.btnSetFeatures.Visible = false;
            }
        }
                
        /// <summary>
        /// populate roles grid
        /// </summary>
        public void populateRolesGrid()
        {
            var vRoleList = palinoiaBLL.getAllRoles();
            grdRoles.DataSource = vRoleList;
            grdRoles.DataBind();
        }

        #endregion UI Control
    }
}