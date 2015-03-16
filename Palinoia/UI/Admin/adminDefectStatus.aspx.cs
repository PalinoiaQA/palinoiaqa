﻿using System;
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
    /// class to hold code for adminDefectStatus object
    /// </summary>
    public partial class adminDefectStatus : basePalinoiaPage
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
            this.addJavaScriptReference("Admin/adminDefectStatus.js");
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
                    populateDefectStatusGrid();
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
        /// handles events when save edit Defect Status button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnSaveDefectStatus_Click(object sender, EventArgs e)
        {
            //get id of selected Defect Status
            int selectedID = Convert.ToInt32(this.hdnDefectStatusID.Value);
            //get updated text from textbox
            string newText = this.txtAddDefectStatus.Text;
            bool active = this.chkActive.Checked;
            //required field check
            if (newText.Length == 0)
            {
                sendMessageToClient("Defect Status is required.");
            }
            else
            {
                //no special chars check
                if (palinoiaBLL.isValidText(newText))
                {
                    string result = "";
                    var docStatus = new viewDefectStatus(selectedID, newText, active, this.userID);
                    if (selectedID == 0)
                    {
                        int newID = 0;
                        result = adminBLL.addDefectStatus(docStatus);
                        bool parseResult = int.TryParse(result, out newID);
                        if (newID > 0)
                        {
                            result = "OK";
                        }
                    }
                    else
                    {
                        result = adminBLL.updateDefectStatus(docStatus);
                    }
                    if (!result.Equals("OK"))
                    {
                        sendMessageToClient(result);
                    }
                    //redraw grid
                    populateDefectStatusGrid();
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
        protected void grdDefectStatus_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void grdDefectStatus_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var strSelectedIndex = e.RowIndex;
            int selectedIndex = Convert.ToInt32(strSelectedIndex);
            int deleteID = Convert.ToInt32(grdDefectStatus.Rows[selectedIndex].Cells[0].Text);
            var result = adminBLL.deleteDefectStatus(deleteID, this.userID);
            //alert client if error
            if (result.Equals("OK"))
            {
                string error = result;
            }
            //redraw grid
            populateDefectStatusGrid();
        }
                
        /// <summary>
        /// handles events when grid row is edited
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewEditEventArgs</param>
        protected void grdDefectStatus_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //get id of selected csm reponse Status
            int selectedIndex = e.NewEditIndex;
            string strID = this.grdDefectStatus.Rows[selectedIndex].Cells[0].Text;
            int selectedID = Convert.ToInt32(strID);
            viewDefectStatus DefectStatus = adminBLL.getDefectStatusByID(selectedID);
            //populate text box for editing
            this.txtAddDefectStatus.Text = DefectStatus.Text;
            this.chkActive.Checked = DefectStatus.Active;
            //populate hidden field on client with response Status id
            this.hdnDefectStatusID.Value = selectedID.ToString();
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
            viewFeature feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminDefectStatusView);
            if (feature != null)
            {

            }
            else
            {
                //user cannot view screen.  redirect to default display
                sendMessageToClient("You do not have the necessary permission to view this screen.");
                this.grdDefectStatus.Visible = false;
            }
            //ADD
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminDefectStatusAdd);
            if (feature != null)
            {
                this.btnAddDefectStatus.Visible = true;
            }
            else
            {
                this.btnAddDefectStatus.Visible = false;
            }
            //EDIT
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminDefectStatusEdit);
            if (feature != null)
            {
                disableEdit = false;
            }
            else
            {
                disableEdit = true;
            }
            //DELETE
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminDefectStatusDelete);
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
        /// populate Defect Status grid
        /// </summary>
        public void populateDefectStatusGrid()
        {
            List<viewDefectStatus> DefectStatusList = adminBLL.getAllDefectStatus();
            grdDefectStatus.DataSource = DefectStatusList;
            grdDefectStatus.DataBind();
        }

        #endregion ui controls
    }
}