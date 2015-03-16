using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Entities;
using Enums;

namespace Palinoia.UI.Admin
{    
    /// <summary>
    /// class to hold code for adminUsers
    /// </summary>
    public partial class adminUsers : basePalinoiaPage
    {
        #region properties and variables

        AdminBLL adminBLL;
        applicationBLL palinoiaBLL;
        bool disableDelete;
        bool disableEdit;
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
            this.addJavaScriptReference("Admin/adminUsers.js");
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
                adminBLL = new AdminBLL(projectID);
                palinoiaBLL = new applicationBLL();
                applyFeatures(userID);
                if (!IsPostBack)
                {
                    populateUsersGrid();
                    populateRolesDDL();
                    //showAddEditControls(false, "");
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
        /// handles events when save edit user button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnSaveUser_Click(object sender, EventArgs e)
        {
            int userID = Convert.ToInt32(hdnUserID.Value.ToString());
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            string middleInitial = txtMiddleInitial.Text;
            string email = txtEmail.Text;
            string role = ddlUserRole.SelectedItem.Text;
            string pw = txtPassword.Text;
            int roleID = Convert.ToInt32(ddlUserRole.SelectedValue);
            bool active = chkActive.Checked;
            //required field check
            if (firstName.Length == 0)
            {
                sendMessageToClient("First Name is required.");
            }
            else
            {
                //special chars check
                if (palinoiaBLL.isValidText(firstName))
                {
                    string result = "";
                    //create new viewCSMResponse type object
                    viewUser user = new viewUser(userID, roleID, firstName, lastName, middleInitial, email, pw, active, role, this.userID);
                    if (userID == 0)
                    {
                        //call bll to add view user
                        result = palinoiaBLL.addUser(user);
                        int newUserID = 0;
                        bool parseResult = int.TryParse(result, out newUserID);
                        if (newUserID > 0)
                        {
                            this.hdnUserID.Value = newUserID.ToString();
                            result = "OK";
                        }
                    }
                    else
                    {
                        //call bll to save view object
                        result = palinoiaBLL.updateUser(user);
                    }
                    //redraw grid
                    populateUsersGrid();
                    //alert client if error
                    if (!result.Equals("OK"))
                    {
                        sendMessageToClient(result);
                    }
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
        protected void grdUsers_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void grdUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var strSelectedIndex = e.RowIndex;
            int selectedIndex = Convert.ToInt32(strSelectedIndex);
            int deleteID = Convert.ToInt32(grdUsers.Rows[selectedIndex].Cells[0].Text);
            var result = palinoiaBLL.deleteUser(deleteID, this.userID);
            //alert client if error
            if (result.Equals("OK"))
            {
                sendMessageToClient(result);
            }
            //redraw grid
            populateUsersGrid();
        }
                
        /// <summary>
        /// handles events when grid row is edited
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewEditEventArgs</param>
        protected void grdUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //get id of selected csm reponse type
            int selectedIndex = e.NewEditIndex;
            string strID = this.grdUsers.Rows[selectedIndex].Cells[0].Text;
            int selectedID = Convert.ToInt32(strID);
            var vUser = palinoiaBLL.getUserByID(selectedID);
            //populate text box for editing
            this.txtFirstName.Text = vUser.FirstName;
            this.txtLastName.Text = vUser.LastName;
            this.txtMiddleInitial.Text = vUser.MiddleInitial;
            this.txtEmail.Text = vUser.Email;
            this.txtPassword.Text = "";
            this.chkActive.Checked = vUser.Active;
            this.ddlUserRole.SelectedItem.Text = vUser.RoleName;
            this.ddlUserRole.SelectedValue = vUser.UserRoleID.ToString();
            //populate hidden field on client with response type id
            this.hdnUserID.Value = selectedID.ToString();
            //prevent further asp gridview events for editing
            e.Cancel = true;
            //show edit dialog on client
            showDialog(true);
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
            viewFeature feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminUsersView);
            if (feature != null)
            {

            }
            else
            {
                //user cannot view screen.  redirect to default display
                sendMessageToClient("You do not have the necessary permission to view this screen.");
                this.grdUsers.Visible = false;
            }
            //ADD
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminUsersAdd);
            if (feature != null)
            {
                this.btnAddUser.Visible = true;
            }
            else
            {
                this.btnAddUser.Visible = false;
            }
            //EDIT
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminUsersEdit);
            if (feature != null)
            {
                disableEdit = false;
            }
            else
            {
                disableEdit = true;
            }
            //DELETE
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminUsersDelete);
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
        /// populate user grid
        /// </summary>
        public void populateUsersGrid()
        {
            var vUserList = palinoiaBLL.getAllUsers();
            grdUsers.DataSource = vUserList;
            grdUsers.DataBind();
        }
                
        /// <summary>
        /// populate roles DDL
        /// </summary>
        public void populateRolesDDL()
        {
            //clear ddl
            ddlUserRole.Items.Clear();
            List<viewRole> roleList = palinoiaBLL.getAllRoles();
            foreach (var role in roleList)
            {
                ddlUserRole.Items.Add(new ListItem(role.Text, role.ID.ToString()));
            }
        }
        
        #endregion UI Control
    }
}