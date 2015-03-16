using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Entities;

namespace Palinoia.UI.Admin
{
    /// <summary>
    /// class to hold code for adminTestSteps object
    /// </summary>
    public partial class adminTestSteps : basePalinoiaPage
    {
        #region properties and variables

        /// <summary>
        /// class variable for testCasesBLL
        /// </summary>
        TestCasesBLL testCasesBLL;
        /// <summary>
        /// class variable for adminBLL
        /// </summary>
        AdminBLL adminBLL;
        /// <summary>
        /// class variable for disableEdit
        /// </summary>
        bool disableEdit;
        /// <summary>
        /// class variable for userID
        /// </summary>
        /// <summary>
        /// class variable for disableDelete
        /// </summary>
        bool disableDelete;
        int userID;

        #endregion properties and variables

        #region page lifecycle events

        /// <summary>
        /// loads page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.addJavaScriptReference("Admin/adminTestSteps.js");
            //check for valid user id
            userID = Convert.ToInt32(Session.Contents["userID"]);
            if (userID > 0)
            {
                int projectID = Convert.ToInt32(Session.Contents["projectID"]);
                this.hdnProjectID.Value = projectID.ToString();
                adminBLL = new AdminBLL(projectID);
                testCasesBLL = new TestCasesBLL(projectID);
                populateTestStepsGrid();
            }
        }

        #endregion pagelifecycle events

        #region event handlers

        /// <summary>
        /// UNFINISHED
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewRowEventArgs</param>
        protected void grdTestSteps_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void grdTestSteps_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //get TestStep id to be deleted
            int deleteID = 0;
            var success = int.TryParse(grdTestSteps.Rows[e.RowIndex].Cells[0].Text, out deleteID);
            if (deleteID > 0)
            {
                var error = adminBLL.deleteTestStepByID(deleteID, this.userID);
                if (!error.Equals("OK"))
                {
                    sendMessageToClient(error);
                }
                else
                {
                    populateTestStepsGrid();
                }
            }

        }

        /// <summary>
        /// handles events when grid row is edited
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewEditEventArgs</param>
        protected void grdTestSteps_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //get TestStep id to be edited
            var TestStepID = grdTestSteps.Rows[e.NewEditIndex].Cells[0].Text;
            var active = Convert.ToBoolean(grdTestSteps.Rows[e.NewEditIndex].Cells[2].Text);
            //store edit id in hidden field on form
            this.hdnTestStepID.Value = TestStepID;
            //convert to int to get view entity from bll
            int tsID = Convert.ToInt32(TestStepID);
            viewTestStep ts = testCasesBLL.getTestStepByID(tsID);
            this.txtAddTestStep.Text = ts.Name;
            this.chkActive.Checked = active;
            //call client function to display edit modal
            showClientEditTestStepModal(ts.Name);
            //prevent further asp gridview events for editing
            e.Cancel = true;
        }

        /// <summary>
        /// handles events when save test step button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnSaveTestStep_Click(object sender, EventArgs e)
        {
            string tsName = this.txtAddTestStep.Text;
            bool active = this.chkActive.Checked;
            int tsID = 0;
            string addEditResult = "";
            bool result = Int32.TryParse(this.hdnTestStepID.Value, out tsID);
            viewTestStep ts = new viewTestStep(tsID, tsName, active, this.userID);
            if (tsID > 0)
            {
                //updating record
                addEditResult = adminBLL.updateTestStep(ts);
            }
            else
            {
                //add new record
                addEditResult = adminBLL.addTestStep(ts);
            }
            if (!addEditResult.Equals("OK"))
            {
                sendMessageToClient(addEditResult);
            }
            else
            {
                populateTestStepsGrid();
            }
        }

        #endregion event handlers

        /// <summary>
        /// show client edit test steo modal
        /// </summary>
        /// <param name="text">string</param>
        private void showClientEditTestStepModal(string text)
        {
            // Define the name and type of the client script on the page.
            String csName = "showClientEditTestStepModal";
            Type csType = this.GetType();
            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs = Page.ClientScript;
            // Check to see if the client script is already registered.
            if (!cs.IsStartupScriptRegistered(csType, csName))
            {
                ClientScript.RegisterStartupScript(typeof(Page), csName, "<script type='text/javascript'>showClientEditTestStepModal('" + text + "');</script>");
            }
        }

        #region populate controls

        private void populateTestStepsGrid()
        {
            //clear grid
            this.grdTestSteps.DataSource = null;
            this.grdTestSteps.DataBind();
            var TestStepList = adminBLL.getAllTestSteps();
            this.grdTestSteps.DataSource = TestStepList;
            this.grdTestSteps.DataBind();
        }

        #endregion populate controls


    }
}