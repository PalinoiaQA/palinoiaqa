using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entities;
using BLL;
using Enums;
using System.Text;

namespace Palinoia.UI.Admin
{    
    /// <summary>
    /// class to hold code for adminStatus
    /// </summary>
    public partial class adminStatus : basePalinoiaPage
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
            this.addJavaScriptReference("../ColorPicker/jquery.colorPicker.min.js");
            this.addJavaScriptReference("Admin/adminStatus.js");
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
                    populateStatusGrid();
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
        /// handles events when save add status button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnSaveStatus_Click(object sender, EventArgs e)
        {
            string newText = txtAddStatus.Text;
            string colorText = this.hdnColor.Value;
            if (colorText == null || colorText == "" || colorText == " ")
            {
                colorText = "#FFFFFF";
            }
            bool active = chkActive.Checked;
            bool displayInSummary = this.chkDisplayInSummary.Checked;
            int statusID = 0;
            bool parseResult = int.TryParse(this.hdnStatusID.Value, out statusID);
            //required field check
            if (newText.Length == 0)
            {
                sendMessageToClient("Status text is required.");
            }
            else
            {
                //no special chars check
                if (palinoiaBLL.isValidText(newText))
                {
                    //create new viewCSMResponse type object
                    viewStatus vStatus = new viewStatus(statusID, 
                                                        newText, 
                                                        active, 
                                                        colorText, 
                                                        displayInSummary, 
                                                        this.userID);
                    //call bll to save view object
                    string result;
                    if (statusID == 0)
                    {
                        result = adminBLL.addNewStatus(vStatus);
                    }
                    else
                    {
                        result = adminBLL.updateStatus(vStatus);
                    }
                    //alert client if error
                    if (!result.Equals("OK"))
                    {
                        string error = result;
                        sendMessageToClient(error);
                    }
                    //redraw grid
                    populateStatusGrid();
                }
                else
                {
                    sendMessageToClient("Special characters are not allowed.");
                }
            }
        }
                
        /// <summary>
        /// grdStatus_RowDataBound - event to apply features and delete verification
        /// message to grid 
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewRowEventArgs</param>
        protected void grdStatus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //set highlight color for cell 2
                string colorValue = e.Row.Cells[2].Text;
                e.Row.Cells[2].Text = "";
                e.Row.Cells[2].BackColor = System.Drawing.ColorTranslator.FromHtml(colorValue);
                // loop all data cells
                foreach (DataControlFieldCell cell in e.Row.Cells)
                {
                    // check all cells in one row
                    foreach (Control control in cell.Controls)
                    {
                        // Must use LinkButton here instead of ImageButton
                        // if you are having Links (not images) as the command button.
                        var button = control as LinkButton;

                        if (button != null && button.CommandName == "Delete") {
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
        protected void grdStatus_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var strSelectedIndex = e.RowIndex;
            int selectedIndex = Convert.ToInt32(strSelectedIndex);
            int deleteID = Convert.ToInt32(grdStatus.Rows[selectedIndex].Cells[0].Text);
            var result = adminBLL.deleteStatus(deleteID, this.userID);
            //alert client if error
            if (result.Equals("OK"))
            {
                string error = result;
            }
            else
            {
                sendMessageToClient(result);
            }
            //redraw grid
            populateStatusGrid();
        }
                
        /// <summary>
        /// handles events when grid row is edited
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewEditEventArgs</param>
        protected void grdStatus_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //get id of selected status grid row
            int selectedIndex = e.NewEditIndex;
            string strID = this.grdStatus.Rows[selectedIndex].Cells[0].Text;
            int selectedID = Convert.ToInt32(strID);
            viewStatus status = adminBLL.getStatusByID(selectedID);
            //populate text box for editing
            this.txtAddStatus.Text = status.Text;
            this.chkActive.Checked = status.Active;
            this.chkDisplayInSummary.Checked = status.DisplayInChapterSummary;
            //this.txtColor.Text = status.Color;
            this.hdnColor.Value = status.Color;
            //populate hidden field on client with response type id
            this.hdnStatusID.Value = selectedID.ToString();
            //show dialog on client
            showDialog(true);
            //prevent further asp gridview events for editing
            e.Cancel = true;
        }
                
        /// <summary>
        /// handles events when cancel button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            populateStatusGrid();
        }

        #endregion event handlers

        #region UI Control
                
        /// <summary>
        /// app;y features
        /// </summary>
        /// <param name="userID">int</param>
        public void applyFeatures(int userID)
        {
            viewUser user = palinoiaBLL.getUserByID(userID);
            List<viewFeature> userFeatureList = palinoiaBLL.getFeaturesForUser(user);
            //VIEW
            viewFeature feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminStatusView);
            if (feature != null)
            {

            }
            else
            {
                //user cannot view screen.  redirect to default display
                sendMessageToClient("You do not have the necessary permission to view this screen.");
                this.grdStatus.Visible = false;
            }
            //ADD
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminStatusAdd);
            if (feature != null)
            {
                this.btnAddStatus.Visible = true;
            }
            else
            {
                this.btnAddStatus.Visible = false;
            }
            //EDIT
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminStatusEdit);
            if (feature != null)
            {
                disableEdit = false;
            }
            else
            {
                disableEdit = true;
            }
            //DELETE
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminStatusDelete);
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
        /// populate status grid
        /// </summary>
        public void populateStatusGrid()
        {
            var vStatusList = adminBLL.getAllStatuses();
            grdStatus.DataSource = vStatusList;
            grdStatus.DataBind();
        }

        #endregion UI Control
    }
}