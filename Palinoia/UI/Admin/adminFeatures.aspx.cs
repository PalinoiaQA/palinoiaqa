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
    /// class to hold code for adminFeatures
    /// </summary>
    public partial class adminFeatures : basePalinoiaPage
    {
        #region properties and variables

        AdminBLL adminBLL;
        applicationBLL palinoiaBLL;
        bool disableEdit;
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
            this.addJavaScriptReference("Admin/adminFeatures.js");
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
                    populateFeaturesGrid();
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
        /// UNFINISHED
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewRowEventArgs</param>
        protected void grdFeatures_RowDataBound(object sender, GridViewRowEventArgs e)
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
                            //apply delete feature which is set in applyFeatures()
                            if (disableDelete)
                            {
                                button.Visible = false;
                            }
                        }
                        //apply edit feature which is set in applyFeatures()
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
        /// handles events when save edit feature button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnSaveFeature_Click(object sender, EventArgs e)
        {
            //get id of selected csm reponse type
            int selectedID = Convert.ToInt32(this.hdnFeatureID.Value);
            //get updated text from textbox
            string newText = this.txtAddFeature.Text;
            bool active = this.chkActive.Checked;
            //required field check
            if (newText.Length == 0)
            {
                sendMessageToClient("Feature text is required.");
            }
            else
            {
                //no special chars check
                if (palinoiaBLL.isValidText(newText))
                {
                    string result = "";
                    var feature = new viewFeature(selectedID, newText, active, this.userID);
                    if (selectedID == 0)
                    {
                        int newID = 0;
                        result = palinoiaBLL.addFeature(feature);
                        bool parseResult = int.TryParse(result, out newID);
                        if (newID > 0)
                        {
                            result = "OK";
                            this.hdnFeatureID.Value = newID.ToString();
                        }
                    }
                    else
                    {
                        result = palinoiaBLL.updateFeature(feature);
                    }
                    if (!result.Equals("OK"))
                    {
                        sendMessageToClient(result);
                    }
                    //redraw grid
                    populateFeaturesGrid();
                }
                else
                {
                    sendMessageToClient("Special characters are not allowed.");
                }
            }
        }
                
        /// <summary>
        /// handles events when grid row is deleted
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewDeleteEventArgs</param>
        protected void grdFeatures_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var strSelectedIndex = e.RowIndex;
            int selectedIndex = Convert.ToInt32(strSelectedIndex);
            int deleteID = Convert.ToInt32(grdFeatures.Rows[selectedIndex].Cells[0].Text);
            var result = palinoiaBLL.deleteFeature(deleteID, this.userID);
            //alert client if error
            if (!result.Equals("OK"))
            {
                sendHTMLMessageToClient(result);
            }
            else
            {
                //redraw grid
                populateFeaturesGrid();
            }
        }
                
        /// <summary>
        ///     Event editor for row editing
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewEditEventArgs</param>
        protected void grdFeatures_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //get id of selected csm reponse type
            int selectedIndex = e.NewEditIndex;
            string strID = this.grdFeatures.Rows[selectedIndex].Cells[0].Text;
            int selectedID = Convert.ToInt32(strID);
            viewFeature feature = palinoiaBLL.getFeatureByID(selectedID);
            //populate text box for editing
            this.txtAddFeature.Text = feature.Text;
            this.chkActive.Checked = feature.Active;
            //populate hidden field on client with response type id
            this.hdnFeatureID.Value = selectedID.ToString();
            //prevent further asp gridview events for editing
            e.Cancel = true;
            showDialog(true);
        }

        #endregion event handlers

        #region UI Control
                
        /// <summary>
        /// apply features for a particular user
        /// </summary>
        /// <param name="userID">int</param>
        public void applyFeatures(int userID)
        {
            viewUser user = palinoiaBLL.getUserByID(userID);
            List<viewFeature> userFeatureList = palinoiaBLL.getFeaturesForUser(user);
            //VIEW
            viewFeature feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminFeaturesView);
            if (feature != null)
            {

            }
            else
            {
                //user cannot view screen.  redirect to default display
                sendMessageToClient("You do not have the necessary permission to view this screen.");
                this.grdFeatures.Visible = false;
            }
            //ADD
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminFeaturesAdd);
            if (feature != null)
            {
                this.btnAddFeature.Visible = true;
            }
            else
            {
                this.btnAddFeature.Visible = false;
            }
            //EDIT
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminFeaturesEdit);
            if (feature != null)
            {
                disableEdit = false;
            }
            else
            {
                disableEdit = true;
            }
            //DELETE
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminFeaturesDelete);
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
        /// populate features grid
        /// </summary>
        public void populateFeaturesGrid()
        {
            var vFeatureList = palinoiaBLL.getAllFeatures();
            grdFeatures.DataSource = vFeatureList;
            grdFeatures.DataBind();
        }

        #endregion UI Control
     
    }
}