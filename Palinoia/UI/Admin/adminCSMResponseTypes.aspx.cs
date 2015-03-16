using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entities;
using BLL;
using System.Text;
using Enums;

namespace Palinoia.UI.Admin
{    
    /// <summary>
    /// class to hold the code for adminCSMResponseTypes
    /// </summary>
    public partial class adminCSMResponseTypes : basePalinoiaPage
    {
        #region Properties and Variables

        AdminBLL adminBLL;
        applicationBLL palinoiaBLL;
        bool disableDelete;
        bool disableEdit;
        int userID;

        #endregion Properties and Variables

        #region Page Lifecycle Events
                
        /// <summary>
        /// initializes a page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            this.addJavaScriptReference("Admin/adminCSMResponseType.js");
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
                    populateCSMResponseTypeGrid();
                    showAddEditControls(false, "");
                }
            }
            else
            {
                //user invalid.  redirect to login
                Response.Redirect("~/Account/Login.aspx");
            }
        }

        #endregion Page Lifecycle Events

        #region Event Handlers
                
        /// <summary>
        /// rowdata event handler for the CSMResponseTypes  grid
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewRowEventArgs</param>
        protected void grdCSMResponseTypes_RowDataBound(object sender, GridViewRowEventArgs e)
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
        /// handles events when a grid row is deleted
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewDeleteEventArgs</param>
        protected void grdCSMResponseTypes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var strSelectedIndex = e.RowIndex;
            int selectedIndex = Convert.ToInt32(strSelectedIndex);
            int deleteID = Convert.ToInt32(grdCSMResponseTypes.Rows[selectedIndex].Cells[0].Text);
            var result = adminBLL.deleteCSMResponseType(deleteID, this.userID);
            //alert client if error
            if (result.Equals("OK"))
            {
                string error = result;
            }
            //redraw grid
            populateCSMResponseTypeGrid();
        }
                
        /// <summary>
        /// handles events when a grid row is edited
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewEditEventArgs</param>
        protected void grdCSMResponseTypes_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //get id of selected csm reponse type
            int selectedIndex = e.NewEditIndex;
            string strID = this.grdCSMResponseTypes.Rows[selectedIndex].Cells[0].Text;
            int selectedID = Convert.ToInt32(strID);
            viewCSMResponseType responseType = adminBLL.getCSMResponseTypeByID(selectedID);
            //populate text box for editing
            this.txtAddResponseType.Text = responseType.Text;
            this.chkActive.Checked = responseType.Active;
            //populate hidden field on client with response type id
            this.hdnEditResponseTypeID.Value = responseType.ID.ToString();
            //prevent further asp gridview events for editing
            e.Cancel = true;
            showDialog(true);
        }
                
        /// <summary>
        /// handles events when the save edit CSM response type button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnSaveCSMResponseType_Click(object sender, EventArgs e)
        {
            //get id of selected csm reponse type
            int selectedID = Convert.ToInt32(this.hdnEditResponseTypeID.Value);
            //get updated text from textbox
            string newText = this.txtAddResponseType.Text;
            bool active = this.chkActive.Checked;
            //required field check
            if (newText.Length == 0)
            {
                sendMessageToClient("Response Type is required.");
            }
            else
            {
                //no special chars check
                if (palinoiaBLL.isValidText(newText))
                {
                    string result = "";
                    //valid text, go ahead and save
                    var responseType = new viewCSMResponseType(selectedID, newText, active, this.userID);
                    if (selectedID == 0)
                    {
                        result = adminBLL.addNewCSMResponseType(responseType);
                        int newID = 0;
                        bool parseResult = int.TryParse(result, out newID);
                        if (newID > 0)
                        {
                            this.hdnEditResponseTypeID.Value = newID.ToString();
                            result = "OK";
                        }
                    }
                    else
                    {
                        result = adminBLL.updateCSMResponseType(responseType);
                    }
                    if (!result.Equals("OK"))
                    {
                        sendMessageToClient(result);
                    }
                    //hide save controls
                    showAddEditControls(false, "");
                    //redraw grid
                    populateCSMResponseTypeGrid();
                }
                else
                {
                    sendMessageToClient("Special characters are not allowed.");
                }
            }
        }

        #endregion Event Handlers

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
            viewFeature feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminCSMResponseTypesView);
            if (feature != null)
            {

            }
            else
            {
                //user cannot view screen.  redirect to default display
                sendMessageToClient("You do not have the necessary permission to view this screen.");
                this.grdCSMResponseTypes.Visible = false;
            }
            //ADD
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminCSMResponseTypesAdd);
            if (feature != null)
            {
                this.btnAddResponseType.Visible = true;
            }
            else
            {
                this.btnAddResponseType.Visible = false;
            }
            //EDIT
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminCSMResponseTypesEdit);
            if (feature != null)
            {
                disableEdit = false;
            }
            else
            {
                disableEdit = true;
            }
            //DELETE
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminCSMResponseTypesDelete);
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
        /// populates a grid
        /// </summary>
        public void populateCSMResponseTypeGrid()
        {
            var vRTList = adminBLL.getCSMResponseTypes();
            grdCSMResponseTypes.DataSource = vRTList;
            grdCSMResponseTypes.DataBind();
        }
                
        /// <summary>
        /// makes certain buttons visible in particular modes
        /// </summary>
        /// <param name="visible">bool</param>
        /// <param name="mode">string</param>
        public void showAddEditControls(bool visible, string mode)
        {
            //this.txtAddResponseType.Visible = visible;
            //this.chkActive.Visible = visible;
            //if (mode.Equals("add"))
            //{
            //    this.btnSaveAddCSMResponseType.Visible = visible;
            //}
            //else if (mode.Equals("edit"))
            //{
            //    this.btnSaveEditCSMResponseType.Visible = visible;
            //}
            //else
            //{
            //    this.btnSaveEditCSMResponseType.Visible = visible;
            //    this.btnSaveAddCSMResponseType.Visible = visible;
            //}
            //this.btnCancel.Visible = visible;
            //this.lblResponseTypeText.Visible = visible;
        }

        #endregion UI Control

    }
}