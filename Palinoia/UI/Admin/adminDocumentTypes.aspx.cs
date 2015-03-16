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
    public partial class adminDocumentTypes : basePalinoiaPage
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
            this.addJavaScriptReference("Admin/adminDocumentTypes.js");
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
                    populateDocumentTypesGrid();
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
        /// handles events when save edit Document type button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnSaveDocumentType_Click(object sender, EventArgs e)
        {
            //get id of selected Document type
            int selectedID = Convert.ToInt32(this.hdnDocumentTypeID.Value);
            //get updated text from textbox
            string newText = this.txtAddDocumentType.Text;
            bool active = this.chkActive.Checked;
            bool includeChapterSummary = this.chkShowSummary.Checked;
            //required field check
            if (newText.Length == 0)
            {
                sendMessageToClient("Document Type is required.");
            }
            else
            {
                //no special chars check
                if (palinoiaBLL.isValidText(newText))
                {
                    string result = "";
                    var docType = new viewDocumentType(selectedID, newText, active, includeChapterSummary, this.userID);
                    if (selectedID == 0)
                    {
                        int newID = 0;
                        result = adminBLL.addDocumentType(docType);
                        bool parseResult = int.TryParse(result, out newID);
                        if (newID > 0)
                        {
                            result = "OK";
                        }
                    }
                    else
                    {
                        result = adminBLL.updateDocumentType(docType);
                    }
                    if (!result.Equals("OK"))
                    {
                        sendMessageToClient(result);
                    }
                    //redraw grid
                    populateDocumentTypesGrid();
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
        protected void grdDocumentTypes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var docTypeName = e.Row.Cells[1].Text;
                // loop all data cells
                foreach (DataControlFieldCell cell in e.Row.Cells)
                {
                    // check all cells in one row
                    foreach (Control control in cell.Controls)
                    {
                        // Must use LinkButton here instead of ImageButton
                        // if you are having Links (not images) as the command button.
                        var button = control as LinkButton;
                        if (button != null)
                        {
                            if (button.CommandName == "Delete")
                            {
                                // Add delete confirmation
                                button.OnClientClick = "return confirm('Are you sure you want to delete this record?');";
                                if (disableDelete)
                                {
                                    button.Visible = false;
                                }
                                //remove ability to delete test case and functional doc types
                                if (docTypeName.Equals("Test Case") || docTypeName.Equals("Functional"))
                                {
                                    button.Visible = false;
                                }
                            }
                            if (disableEdit)
                            {
                                if (button.CommandName == "Edit")
                                    // Add delete confirmation
                                    button.Visible = false;
                            }
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
        protected void grdDocumentTypes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var strSelectedIndex = e.RowIndex;
            int selectedIndex = Convert.ToInt32(strSelectedIndex);
            int deleteID = Convert.ToInt32(grdDocumentTypes.Rows[selectedIndex].Cells[0].Text);
            var result = adminBLL.deleteDocumentType(deleteID, this.userID);
            //alert client if error
            if (result.Equals("OK"))
            {
                string error = result;
            }
            //redraw grid
            populateDocumentTypesGrid();
        }
                
        /// <summary>
        /// handles events when grid row is edited
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewEditEventArgs</param>
        protected void grdDocumentTypes_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //get id of selected csm reponse type
            int selectedIndex = e.NewEditIndex;
            string strID = this.grdDocumentTypes.Rows[selectedIndex].Cells[0].Text;
            int selectedID = Convert.ToInt32(strID);
            viewDocumentType DocumentType = adminBLL.getDocumentTypeByID(selectedID);
            //populate text box for editing
            this.txtAddDocumentType.Text = DocumentType.Text;
            this.chkActive.Checked = DocumentType.Active;
            this.chkShowSummary.Checked = DocumentType.IncludeBRCSMChapterSummary;
            //populate hidden field on client with response type id
            this.hdnDocumentTypeID.Value = selectedID.ToString();
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
            viewFeature feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.DocumentTypeView);
            if (feature != null)
            {

            }
            else
            {
                //user cannot view screen.  redirect to default display
                sendMessageToClient("You do not have the necessary permission to view this screen.");
                this.grdDocumentTypes.Visible = false;
            }
            //ADD
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.DocumentTypeAdd);
            if (feature != null)
            {
                this.btnAddDocumentType.Visible = true;
            }
            else
            {
                this.btnAddDocumentType.Visible = false;
            }
            //EDIT
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.DocumentTypeEdit);
            if (feature != null)
            {
                disableEdit = false;
            }
            else
            {
                disableEdit = true;
            }
            //DELETE
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.DocumentTypeDelete);
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
        /// populate Document types grid
        /// </summary>
         public void populateDocumentTypesGrid()
        {
            List<viewDocumentType> documentTypeList = adminBLL.getAllDocumentTypes();
            grdDocumentTypes.DataSource = documentTypeList;
            grdDocumentTypes.DataBind();
        }

        #endregion ui controls
    }
}