using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Entities;
using Enums;
using System.IO;

namespace Palinoia.UI.Admin
{   
    /// <summary>
    /// class to hold code for adminRoleFeatures
    /// </summary>
    public partial class adminRoleFeatures : basePalinoiaPage
    {
        #region properties and variables

        AdminBLL adminBLL;
        applicationBLL palinoiaBLL;
        int roleID;
        viewRole role;
        int projectID;

        #endregion properties and variables

        #region page lifecycle events
                
        /// <summary>
        /// initializes a page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs </param>
        protected void Page_Init(object sender, EventArgs e)
        {
            this.addJavaScriptReference("Admin/adminRoleFeatures.js");
        }
                
        /// <summary>
        /// loads a page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //check for valid user id
            int userID = Convert.ToInt32(Session.Contents["userID"]);
            if (userID > 0)
            {
                adminBLL = new AdminBLL(projectID);
                palinoiaBLL = new applicationBLL();
                applyFeatures(userID);
                projectID = Convert.ToInt32(Session.Contents["ProjectID"]);
                roleID = Convert.ToInt32(Session.Contents["RoleID"]);
                role = palinoiaBLL.getRoleByID(roleID);
                lblRoleName.Text = role.Text;
                if (!IsPostBack)
                {
                    populateControls();
                }
            }
            else
            {
                //user invalid.  redirect to login
                Response.Redirect("~/Account/Login.aspx");
            }
        }

        #endregion page lifecycle events

        #region event handlers
                
        /// <summary>
        /// handles events when add feature button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnAddFeature_Click(object sender, EventArgs e)
        {
            foreach (var item in listAvailableFeatures.GetSelectedIndices())
            {
                int featureID = Convert.ToInt32(listAvailableFeatures.Items[item].Value);
                palinoiaBLL.addFeatureToRole(this.roleID, featureID);
            }
            populateControls();
        }
                
        /// <summary>
        /// handles events when remove feature button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs </param>
        protected void btnRemoveFeature_Click(object sender, EventArgs e)
        {
            foreach (var item in listAssociatedFeatures.GetSelectedIndices())
            {
                int featureID = Convert.ToInt32(listAssociatedFeatures.Items[item].Value);
                palinoiaBLL.removeFeatureFromRole(this.roleID, featureID);
            }
            populateControls();
        }
                
        /// <summary>
        /// handles events when done button ic clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnDone_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/UI/Admin/adminRoles.aspx");
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
            viewFeature feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminRoleFeaturesView);
            if (feature != null)
            {

            }
            else
            {
                //user cannot view screen.  redirect to default display
                sendMessageToClient("You do not have the necessary permission to view this screen.");
                this.listAssociatedFeatures.Visible = false;
                this.listAvailableFeatures.Visible = false;
                this.lblAssociatedFeatures.Visible = false;
                this.lblAvailableFeatures.Visible = false;
            }
            //EDIT
            feature = userFeatureList.Find((f) => f.ID == (int)featureEnums.Feature.AdminRoleFeaturesEdit);
            if (feature != null)
            {
                this.btnAddFeature.Visible = true;
                this.btnRemoveFeature.Visible = true;
            }
            else
            {
                this.btnAddFeature.Visible = false;
                this.btnRemoveFeature.Visible = false;
            }
        }
                
        /// <summary>
        /// populate controls
        /// </summary>
        public void populateControls()
        {
            populateAssociatedFeaturesListBox();
            populateAvailableFeaturesListBox();
        }
                
        /// <summary>
        /// populate associatedfeatures listbox
        /// </summary>
        private void populateAssociatedFeaturesListBox()
        {
            listAssociatedFeatures.Items.Clear();
            var associatedFeaturesList = palinoiaBLL.getAssociatedFeaturesForRole(this.roleID);
            //string fileName = @"C:\palinoia\featurelist.txt";
            //FileInfo fi = new FileInfo(fileName);
            // Check if file already exists. If yes, delete it. 
            //if (fi.Exists)
            //{
            //    fi.Delete();
            //}

            // Create a new file 
            //using (StreamWriter sw = fi.CreateText())
            //{
                
                foreach (var feature in associatedFeaturesList)
                {
                    listAssociatedFeatures.Items.Add(new ListItem(feature.Text, feature.ID.ToString()));
            //        sw.WriteLine(feature.Text);
                }
            //}
            
        }
                
        /// <summary>
        /// populate available features listbox
        /// </summary>
        private void populateAvailableFeaturesListBox()
        {
            listAvailableFeatures.Items.Clear();
            var availableFeaturesList = palinoiaBLL.getAvailableFeaturesForRole(this.roleID);
            foreach (var feature in availableFeaturesList)
            {
                listAvailableFeatures.Items.Add(new ListItem(feature.Text, feature.ID.ToString()));
            }
        }

        #endregion ut controls

    }
}