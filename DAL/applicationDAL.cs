using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data.EntityModel;
using System.Data.Objects;
using Entities;
using System.Data.EntityClient;
using System.IO;

namespace DAL
{    
    /// <summary>
    /// class to hold the code for the palinoia data access layer which includes application wide
    /// information that is not project specific: Projects, Features, Roles, Users
    /// </summary>
    public class applicationDAL 
    {
        
        #region Properties and Variables

        /// <summary>
        /// stores the id of the project used to create this applicationDAL object
        /// </summary>
        public int currentProjectID { get; set; }
        /// <summary>
        /// stores the file name of the SQL script used to generate 
        /// all tables and values necessary for a new project
        /// </summary>
        public string createClientTablesSQLFileName = "PalinoiaCreateTables.sql";

        #endregion Properties and Variables

        #region Constructors
                
        /// <summary>
        /// constructor with no parameters
        /// </summary>
        public applicationDAL()
        {

        }
                
        /// <summary>
        /// constructor with one parameter
        /// </summary>
        /// <param name="projectID">int</param>
        public applicationDAL(int projectID)
        {
            this.currentProjectID = projectID;
        }

        #endregion Constructors

        #region Instance Methods

        #region public methods
                
        /// <summary>
        /// creates a new entity within entity framework
        /// </summary>
        /// <returns>palinoiaEntities</returns>
        public palinoiaEntities getContextForProject()
        {
            StringBuilder sb = new StringBuilder();
            var context = new palinoiaEntities();
            var sqliteCSB = new SQLiteConnectionStringBuilder();
            sqliteCSB.DataSource = getDataSourceForCurrentProject();
            var entityCSB = new EntityConnectionStringBuilder(context.Connection.ConnectionString);
            entityCSB.Name = "";
            entityCSB.Metadata = "res://*/palinoiaModel.csdl|res://*/palinoiaModel.ssdl|res://*/palinoiaModel.msl";
            entityCSB.Provider = "System.Data.SQLite";
            entityCSB.ProviderConnectionString = sqliteCSB.ConnectionString;
            context = new palinoiaEntities(entityCSB.ConnectionString);
            return context;
        }

        /// <summary>
        /// creates a new entity within entity framework
        /// </summary>
        /// <returns>palinoiaEntities</returns>
        public palinoiaEntities getContextForPalinoia()
        {
            StringBuilder sb = new StringBuilder();
            var context = new palinoiaEntities();
            var sqliteCSB = new SQLiteConnectionStringBuilder();
            sqliteCSB.DataSource = getDataSourceForPalinoia();
            var entityCSB = new EntityConnectionStringBuilder(context.Connection.ConnectionString);
            entityCSB.Name = "";
            entityCSB.Metadata = "res://*/palinoiaModel.csdl|res://*/palinoiaModel.ssdl|res://*/palinoiaModel.msl";
            entityCSB.Provider = "System.Data.SQLite";
            entityCSB.ProviderConnectionString = sqliteCSB.ConnectionString;
            context = new palinoiaEntities(entityCSB.ConnectionString);
            return context;
        }

        public string getCSMByID(int csmID)
        {
            string csm;
            using (var context = this.getContextForPalinoia())
            {
                var entityCSM = context.CustomerServiceMessages.First((c) => c.ID == csmID);
                csm = entityCSM.Text;
                return csm;
            }
        }

        #region Navigation Menu

        /// <summary>
        /// fetch menu item by ID
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>NavigationMenuItem</returns>
        public NavigationMenuItem getMenuItemByID(int id)
        {
            using (var context = new applicationEntities())
            {
                var entityNode = context.Navigation_Menu_Items.FirstOrDefault((n) => n.ID == id);
                return new NavigationMenuItem((int)entityNode.ID, 
                                                       entityNode.Name, 
                                                       entityNode.URL, 
                                                       (int)entityNode.ParentID, 
                                                       entityNode.NodeID,
                                                       (int)entityNode.FeatureID);
            }
        }

        /// <summary>
        /// fetch child nodes for parent ID
        /// </summary>
        /// <param name="parentID">int</param>
        /// <returns>List&lt;NavigationMenuItem&gt;</returns>
        public List<NavigationMenuItem> getChildNodesForParentID(int parentID)
        {
            List<NavigationMenuItem> childNodes = new List<NavigationMenuItem>();
            using (var context = new applicationEntities())
            {
                var nodes = from cn in context.Navigation_Menu_Items
                            where cn.ParentID == parentID
                            orderby cn.Name
                            select cn;
                foreach (var node in nodes)
                {
                    childNodes.Add(new NavigationMenuItem((int)node.ID, node.Name, node.URL, (int)node.ParentID, node.NodeID, (int)node.FeatureID));
                }
            }
            return childNodes;
        }

        /// <summary>
        /// has child nodes
        /// </summary>
        /// <param name="nodeID">int</param>
        /// <returns>bool</returns>
        public bool hasChildNodes(int nodeID)
        {
            bool hasChildren = false;
            using (var context = new applicationEntities())
            {
                var children = context.Navigation_Menu_Items
                                .FirstOrDefault((n) => n.ParentID == (long)nodeID);
                if (children != null)
                {
                    hasChildren = true;
                }
            }
            return hasChildren;
        }

        #endregion Navigation Menu

        #region projects
                
        /// <summary>
        /// creates a separate database for a specified project
        /// </summary>
        /// <param name="SQL">string</param>
        /// <param name="projectDataSource">string</param>
        /// <returns>string</returns>
        public string createNewProjectDatabase(string SQL, string projectDataSource)
        {
            string result = "";
            SQLiteConnection sqliteCon = new SQLiteConnection();
            try
            {
                string dbConnectionString = @projectDataSource;
                //SQLiteConnection.CreateFile(projectFilePath);
                sqliteCon = new SQLiteConnection(dbConnectionString);
                sqliteCon.Open();
                SQLiteCommand command = new SQLiteCommand(SQL, sqliteCon);
                command.ExecuteNonQuery();
                result = "OK";
            }
            catch (Exception ex)
            {
                logError(ex, 0, 0);
                result = ex.Message;
            }
            finally
            {
                sqliteCon.Close();
                sqliteCon.Dispose();
            }
            return result;
        }
        
        /// <summary>
        /// fetches a list all palinoia projects
        /// </summary>
        /// <returns>List&lt;viewProject&gt;</returns>
        public List<viewProject> getAllProjects()
        {
            var projectList = new List<viewProject>();
            using (var context = new applicationEntities())
            {
                var projects = context.Palinoia_Projects.OrderBy((p) => p.NAME);
                foreach (var project in projects)
                {
                    int id = (int)project.ID;
                    string name = project.NAME;
                    string datasource = project.DataSource;
                    bool active = project.Active;
                    int updatedBy = (int)project.UpdatedBy;
                    projectList.Add(new viewProject(id, name, datasource, active, updatedBy));
                }
            }
            return projectList;
        }
        
        /// <summary>
        /// save new project to the database
        /// </summary>
        /// <param name="project">viewProject</param>
        /// <returns>string</returns>
        public string saveNewProject(viewProject project)
        {
            string result = "";
            using (var context = new applicationEntities())
            {
                try
                {
                    Palinoia_Projects newProject = Palinoia_Projects.CreatePalinoia_Projects(0, project.Name, project.DataSource, project.Active, (int)project.UpdatedBy);
                    context.Palinoia_Projects.AddObject(newProject);
                    context.SaveChanges();
                    result = newProject.ID.ToString(); 
                }
                catch (Exception ex)
                {
                    result = logError(ex, 0, project.UpdatedBy);
                }
                return result;
            }
        }
        
        /// <summary>
        /// update a project in the database
        /// </summary>
        /// <param name="project">viewProject</param>
        /// <returns>string</returns>
        public string updateProject(viewProject project)
        {
            string result = "";
            using (var context = new applicationEntities())
            {
                try
                {
                    Palinoia_Projects proj = context.Palinoia_Projects
                                                     .First((p) => p.ID == project.ID);
                    proj.NAME = project.Name;
                    proj.DataSource = project.DataSource;
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = logError(ex, 0, project.UpdatedBy);
                 }
                return result;
            }
        }
        
        /// <summary>
        /// delete a project from the database 
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteProject(int deleteID, int userID)
        {
            string result = "";
            using (var context = new applicationEntities())
            {
                try
                {
                    var deleteProject = context.Palinoia_Projects
                           .First((rt) => rt.ID == deleteID);
                    context.Palinoia_Projects.DeleteObject(deleteProject);
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = logError(ex, 0, userID);
                }
            }
            return result;
        }

        /// <summary>
        /// fetch project by ID
        /// </summary>
        /// <param name="projectID">int</param>
        /// <returns></returns>
        public Palinoia_Projects getProjectByID(int projectID)
        {
            using (var context = new applicationEntities())
            {
                var project = context.Palinoia_Projects
                    .First((p) => p.ID == projectID);
                return project;
            }
        }
        
        #endregion projects

        #region users
                
        /// <summary>
        /// fetches user ID from database
        /// </summary>
        /// <param name="user">viewUser</param>
        /// <returns>int</returns>
        public int loginUser(viewUser user)
        {
            int result = 0;
            using (var context = new applicationEntities())
            {
                try
                {
                    var entityUser = context.Users
                                     .FirstOrDefault((u) => ((u.Email.Trim().ToLower().Equals(user.Email.Trim().ToLower())) && (u.PW.Trim().Equals(user.PW))));
                    if (entityUser != null)
                    {
                        user = new viewUser((int)entityUser.ID, (int)entityUser.fk_UserRoleID, entityUser.FirstName, entityUser.LastName, entityUser.MiddleInitial, entityUser.Email, entityUser.PW, (bool)entityUser.Active, entityUser.lkup_Roles.Text, user.UpdatedBy);
                        result = user.ID;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    logError(ex, this.currentProjectID, 0);
                }
            }
            return result;
        }
        
        /// <summary>
        /// fetches features for user from database
        /// </summary>
        /// <param name="user">viewUser</param>
        /// <returns>List&lt;lkup_Features&gt;</returns>
        public List<lkup_Features> getFeaturesForUser(viewUser user)
        {
            var featureList = new List<lkup_Features>();
            //get role id assigned to user
            var userRoleID = user.UserRoleID;
            //get all features assigned to that role
            using (var context = new applicationEntities())
            {
                var features = from f in context.lkup_Features
                               where context.Role_Features.Any((rf) => (rf.fk_FeatureID == f.ID) && (rf.fk_RoleID == userRoleID))
                               select f;
                //create generic list of entity objects
                featureList = features.ToList<lkup_Features>();
            }
            return featureList;
        }
                
        /// <summary>
        /// fetch all users from the database
        /// </summary>
        /// <returns>List&lt;viewUser&gt;</returns>
        public List<viewUser> getAllUsers()
        {
            using (var context = new applicationEntities())
            {
                List<viewUser> userList = new List<viewUser>();
                var users = context.Users.OrderBy((u) => u.LastName);
                foreach (var user in users)
                {
                    userList.Add(new viewUser((int)user.ID,
                                              (int)user.lkup_Roles.ID,
                                              user.FirstName,
                                              user.LastName,
                                              user.MiddleInitial,
                                              user.Email,
                                              user.PW,
                                              (bool)user.Active,
                                              user.lkup_Roles.Text,
                                              (int)user.UpdatedBy));

                }
                return userList; ;
            }
        }
                
        /// <summary>
        /// fetch a user from the database by ID
        /// </summary>
        /// <param name="userID">int</param>
        /// <returns>viewUser</returns>
        public viewUser getUserByID(int userID)
        {
            if (userID == 0) { userID = 1; }
            viewUser vUser;
            using (var context = new applicationEntities())
            {
                List<viewUser> userList = new List<viewUser>();
                var user = context.Users.First((u) => u.ID == userID);
                vUser = new viewUser((int)user.ID,
                                     (int)user.lkup_Roles.ID,
                                     user.FirstName,
                                     user.LastName,
                                     user.MiddleInitial,
                                     user.Email,
                                     user.PW,
                                     (bool)user.Active,
                                     user.lkup_Roles.Text,
                                     (int)user.UpdatedBy);
                return vUser;
            }
        }
                
        /// <summary>
        /// add a user to the database 
        /// </summary>
        /// <param name="vUser">viewUser</param>
        /// <returns>string</returns>
        public string addUser(viewUser vUser)
        {
            string result = "";
            using (var context = new applicationEntities())
            {
                try
                {
                    User newUser = User.CreateUser(0, vUser.UserRoleID, vUser.PW, vUser.Active, (long)currentProjectID, (long)vUser.UpdatedBy);
                    newUser.FirstName = vUser.FirstName;
                    newUser.LastName = vUser.LastName;
                    newUser.MiddleInitial = vUser.MiddleInitial;
                    newUser.Email = vUser.Email.ToLower();
                    context.Users.AddObject(newUser);
                    context.SaveChanges();
                    result = "OK";
                }

                catch (Exception ex)
                {
                    result = logError(ex, this.currentProjectID, vUser.UpdatedBy);
                }
                return result;
            }
        }
                
        /// <summary>
        /// update a user in the database
        /// </summary>
        /// <param name="vUser">viewUser</param>
        /// <returns>string</returns>
        public string updateUser(viewUser vUser)
        {
            string result = "";
            using (var context = new applicationEntities())
            {
                try
                {
                    var editUser = context.Users.First((u) => u.ID == vUser.ID);
                    editUser.FirstName = vUser.FirstName;
                    editUser.LastName = vUser.LastName;
                    editUser.MiddleInitial = vUser.MiddleInitial;
                    editUser.Email = vUser.Email.ToLower();
                    editUser.fk_UserRoleID = vUser.UserRoleID;
                    if (vUser.PW.Length > 0)
                    {
                        editUser.PW = vUser.PW;
                    }
                    editUser.Active = vUser.Active;
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = logError(ex, currentProjectID, vUser.UpdatedBy);
                    result = ex.Message;
                }
            }
            return result;
        }
                
        /// <summary>
        /// delete a user from the database
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteUser(int deleteID, int userID)
        {
            string result = "";
            using (var context = new applicationEntities())
            {
                try
                {
                    var deleteUser = context.Users
                           .First((u) => u.ID == deleteID);
                    context.Users.DeleteObject(deleteUser);
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = logError(ex, currentProjectID, userID);
                }

            }
            return result;
        }

        /// <summary>
        /// update user project
        /// </summary>
        /// <param name="userID">int</param>
        /// <param name="projectID">int</param>
        /// <returns>string</returns>
        public string updateUserProject(int userID, int projectID)
        {
            string result = "";
            try
            {
                using (var context = new applicationEntities())
                {
                    var user = context.Users.First((u) => u.ID == userID);
                    user.fk_ProjectID = projectID;
                    context.SaveChanges();
                    result = "OK";
                }
            }
            catch (Exception ex)
            {
                var appDAL = new applicationDAL();
                result = appDAL.logError(ex, projectID, userID);
                appDAL = null;
            }
            return result;
        }

        /// <summary>
        /// fetch project for user
        /// </summary>
        /// <param name="userID">int</param>
        /// <returns>int</returns>
        public int getProjectForUser(int userID)
        {
            int projectID = 0; // default to zero
            try
            {
                using (var context = new applicationEntities())
                {
                    var user = context.Users.First((u) => u.ID == userID);
                    projectID = (int)user.fk_ProjectID;
                }
            }
            catch (Exception ex)
            {
                var appDAL = new applicationDAL();
                appDAL.logError(ex, projectID, userID);
                appDAL = null;
            }
            return projectID;
        }

        #endregion users

        #region roles
                
        /// <summary>
        /// fetch all roles from the database
        /// </summary>
        /// <returns>List&lt;lkup_Roles&gt;</returns>
        public List<lkup_Roles> getAllRoles()
        {
            using (var context = new applicationEntities())
            {
                var roles = context.lkup_Roles;
                List<lkup_Roles> roleList = roles.ToList<lkup_Roles>();
                return roleList; ;
            }
        }
                
        /// <summary>
        /// fetch a role from the database by ID
        /// </summary>
        /// <param name="roleID">int</param>
        /// <returns>viewRole</returns>
        public viewRole getRoleByID(int roleID)
        {
            viewRole role;
            using (var context = new applicationEntities())
            {
                var entityRole = context.lkup_Roles
                                          .First((ro) => ro.ID == roleID);
                role = new viewRole((int)entityRole.ID, entityRole.Text, entityRole.Active, (int)entityRole.UpdatedBy);
                return role;
            }
        }
                
        /// <summary>
        /// add a role to the database
        /// </summary>
        /// <param name="vRole">viewRole</param>
        /// <returns>string</returns>
        public string addRole(viewRole vRole)
        {
            string result = "";
            using (var context = new applicationEntities())
            {
                try
                {
                    lkup_Roles newRole = lkup_Roles.Createlkup_Roles(0, vRole.Text, vRole.Active, (long)vRole.UpdatedBy);
                    context.lkup_Roles.AddObject(newRole);
                    context.SaveChanges();
                    result = "OK";
                }

                catch (Exception ex)
                {
                    result = logError(ex, currentProjectID, vRole.UpdatedBy);
                }
                return result;
            }
        }
               
        /// <summary>
        /// update a role in the database
        /// </summary>
        /// <param name="vRole">viewRole</param>
        /// <returns>string</returns>
        public string updateRole(viewRole vRole)
        {
            string result = "";
            using (var context = new applicationEntities())
            {
                try
                {
                    var editRole = context.lkup_Roles.First((r) => r.ID == vRole.ID);
                    editRole.Text = vRole.Text;
                    editRole.Active = vRole.Active;
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = logError(ex, currentProjectID, vRole.UpdatedBy);
                }
            }
            return result;
        }
                
        /// <summary>
        /// delete a role from the database
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteRole(int deleteID, int userID)
        {
            string result = "";
            using (var context = new applicationEntities())
            {
                try
                {
                    var deleteRole = context.lkup_Roles
                           .First((r) => r.ID == deleteID);
                    context.lkup_Roles.DeleteObject(deleteRole);
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = logError(ex, currentProjectID, userID);
                }

            }
            return result;
        }
                
        /// <summary>
        /// fetch the features associated with a particular role from the database
        /// </summary>
        /// <param name="roleID">int</param>
        /// <returns>List&lt;lkup_Features&gt;</returns>
        public List<lkup_Features> getRoleFeatures(int roleID)
        {
            using (var context = new applicationEntities())
            {
                var feature = from f in context.lkup_Features
                              where context.Role_Features.Any((rf) => (rf.fk_FeatureID == f.ID) && (rf.fk_RoleID == roleID))
                              orderby f.Text
                              select f;
                return feature.ToList<lkup_Features>();
            }
        }
                
        /// <summary>
        /// fetch the available features associated with a particular role from the database
        /// </summary>
        /// <param name="roleID">int</param>
        /// <returns>List&lt;lkup_Features&gt;</returns>
        public List<lkup_Features> getAvailableFeaturesForRole(int roleID)
        {
            using (var context = new applicationEntities())
            {
                var features = from f in context.lkup_Features
                               where !context.Role_Features.Any((rf) => (rf.fk_FeatureID == f.ID) && (rf.fk_RoleID == roleID))
                               orderby f.Text
                               select f;

                return features.ToList<lkup_Features>();
            }
        }
                
        /// <summary>
        /// associate a particular feature with a particular role in the database
        /// </summary>
        /// <param name="roleID">int</param>
        /// <param name="featureID">int</param>
        /// <returns>string</returns>
        public string addFeatureToRole(int roleID, int featureID)
        {
            string result = "";
            using (var context = new applicationEntities())
            {
                try
                {
                    Role_Features roleFeature = Role_Features.CreateRole_Features(0, roleID, featureID);
                    context.Role_Features.AddObject(roleFeature);
                    context.SaveChanges();
                    result = "OK";
                }

                catch (Exception ex)
                {
                    result = logError(ex, 0, 0);
                }
                return result;
            }
        }
                
        /// <summary>
        /// remove the association of a particular feature with a particular role in the database
        /// </summary>
        /// <param name="roleID">int</param>
        /// <param name="featureID">int</param>
        /// <returns>string</returns>
        public string removeFeatureFromRole(int roleID, int featureID)
        {
            string result = "";
            using (var context = new applicationEntities())
            {
                try
                {
                    Role_Features removeRoleFeature = context.Role_Features
                                                        .First((rf) => (rf.fk_RoleID == roleID) &&
                                                                       (rf.fk_FeatureID == featureID));
                    context.Role_Features.DeleteObject(removeRoleFeature);
                    context.SaveChanges();
                    result = "OK";
                }

                catch (Exception ex)
                {
                    result = logError(ex, 0, 0);
                }
                return result;
            }
        }

        #endregion roles

        #region features
                
        /// <summary>
        /// fetch all features from the database
        /// </summary>
        /// <returns>List&lt;lkup_Features&gt;</returns>
        public List<lkup_Features> getAllFeatures()
        {
            using (var context = new applicationEntities())
            {
                var features = context.lkup_Features.OrderBy((f) => f.Text);
                List<lkup_Features> featureList = features.ToList<lkup_Features>();
                return featureList; ;
            }
        }
                
        /// <summary>
        /// fetch a feature from the database by ID
        /// </summary>
        /// <param name="featureID">int</param>
        /// <returns>viewFeature</returns>
        public viewFeature getFeatureByID(int featureID)
        {
            viewFeature feature;
            using (var context = new applicationEntities())
            {
                var entityFeature = context.lkup_Features
                                          .First((s) => s.ID == featureID);
                feature = new viewFeature((int)entityFeature.ID, entityFeature.Text, (bool)entityFeature.Active, 0);
                return feature;
            }
        }
                
        /// <summary>
        /// add a feature to the database
        /// </summary>
        /// <param name="vFeature">viewFeature</param>
        /// <returns>string</returns>
        public string addFeature(viewFeature vFeature)
        {
            string result = "";
            using (var context = new applicationEntities())
            {
                try
                {
                    lkup_Features newFeature = lkup_Features.Createlkup_Features(0, vFeature.Text, vFeature.Active, vFeature.UpdatedBy);
                    context.lkup_Features.AddObject(newFeature);
                    context.SaveChanges();
                    result = "OK";
                }

                catch (Exception ex)
                {
                    result = logError(ex, currentProjectID, vFeature.UpdatedBy);
                }
                return result;
            }
        }
                
        /// <summary>
        /// update a feature in the database
        /// </summary>
        /// <param name="vFeature">viewFeature</param>
        /// <returns>string</returns>
        public string updateFeature(viewFeature vFeature)
        {
            string result = "";
            using (var context = new applicationEntities())
            {
                try
                {
                    var editFeature = context.lkup_Features.First((f) => f.ID == vFeature.ID);
                    editFeature.Text = vFeature.Text;
                    editFeature.Active = vFeature.Active;
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = logError(ex, currentProjectID, vFeature.UpdatedBy);
                }
            }
            return result;
        }
                
        /// <summary>
        /// delete a feature from the database
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteFeature(int deleteID, int userID)
        {
            string result = "";
            using (var context = new applicationEntities())
            {
                try
                {
                    var deleteFeature = context.lkup_Features
                           .First((f) => f.ID == deleteID);
                    context.lkup_Features.DeleteObject(deleteFeature);
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = logError(ex, currentProjectID, userID);
                }

            }
            return result;
        }

        public List<string> featureUsedInProject(int objID)
        {
            //return list of project(s) if feature is assigned to role in any project
            List<string> projectList = new List<string>();
            using (var context = new applicationEntities())
            {
                var projects = context.Palinoia_Projects;
                foreach (var project in projects)
                {
                    var featureRoles = context.Role_Features
                                        .FirstOrDefault((f) => f.fk_FeatureID == objID);
                    if (featureRoles != null)
                    {
                        projectList.Add(project.NAME);
                    }

                }
            }
            return projectList;
        }

        #endregion features

        #region error log

        /// <summary>
        /// fetches all error log records from Application_Errors table
        /// </summary>
        /// <returns>List&lt;Application_Errors&gt;</returns>
        public List<Application_Errors> getAllActiveErrors()
        {
            var errorList = new List<Application_Errors>();
            using (var context = new applicationEntities())
            {
                var errors = from e in context.Application_Errors
                             where e.Active == true
                             orderby e.Date descending
                             select e;
                errorList = errors.ToList<Application_Errors>();
            }
            return errorList;
        }

        /// <summary>
        /// get a single error log record by id
        /// </summary>
        /// <param name="errorID">int</param>
        /// <returns>viewError</returns>
        public viewError getErrorByID(int errorID)
        {
            viewError vError = new viewError(0, DateTime.Now, "", "", "", "", 0, 0);
            using (var context = new applicationEntities())
            {
                var entityError = context.Application_Errors
                                          .First((er) => er.ID == errorID);
                vError = new viewError((int)entityError.ID,
                                       entityError.Date,
                                       entityError.Source,
                                       entityError.Message,
                                       entityError.InnerException,
                                       entityError.StackTrace,
                                       (int)entityError.fk_ProjectID,
                                       (int)entityError.fk_UserID);
            }

            return vError;
        }

        /// <summary>
        /// delete all error log records from Application_Errors table
        /// </summary>
        /// <returns>string</returns>
        public string clearErrors()
        {
            string result = "";
            
            using (var context = new applicationEntities())
            {
                var activeErrors = from e in context.Application_Errors
                                   where e.Active == true
                                   select e;
                foreach (var error in activeErrors)
                {
                    error.Active = false;
                }
                context.SaveChanges();
            }
            return result;
        }
                
        /// <summary>
        /// logs a particular error message
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="projectID">int</param>
        /// <param name="userID">int</param>
        public string logError(Exception ex, int projectID, int userID)
        {
            string retVal = "";
            using (var context = new applicationEntities())
            {
                Application_Errors errorRecord = Application_Errors.CreateApplication_Errors(0,
                                                                                    (long)userID,
                                                                                    (long)projectID,
                                                                                    DateTime.Now,
                                                                                    true);
                errorRecord.Message = ex.Message;
                errorRecord.Source = ex.Source;
                errorRecord.StackTrace = ex.StackTrace;
                if (ex.InnerException != null)
                {
                    errorRecord.InnerException = ex.InnerException.Message;
                }
                else
                {
                    errorRecord.InnerException = "";
                }
                errorRecord.Date = DateTime.Now;
                context.Application_Errors.AddObject(errorRecord);
                context.SaveChanges();
                StringBuilder sb = new StringBuilder();
                sb.Append("Source: ");
                sb.Append(cleanStringForJSAlertbox(ex.Source));
                sb.Append("\\n\\r");
                sb.Append("\\n\\r");
                sb.Append("Message: ");
                sb.Append(cleanStringForJSAlertbox(ex.Message));
                sb.Append("\\n\\r");
                sb.Append("\\n\\r");
                if (ex.InnerException != null)
                {
                    sb.Append("InnerException: ");
                    sb.Append(cleanStringForJSAlertbox(ex.InnerException.Message));
                }
                retVal = sb.ToString();
            }
            return retVal;
        }

        #endregion error log

        #endregion public methods

        #region private methods

        /// <summary>
        /// class to pull webconfig values by key
        /// </summary>
        /// <param name="library">key</param>
        /// <returns>string</returns>
        public string GetEnvData(string library)
        {
            string returnValue = @"";
            returnValue = @System.Configuration.ConfigurationManager.AppSettings[library].ToString();
            return @returnValue;
        }
                
        /// <summary>
        /// fetches a data source for a selected project from the database
        /// </summary>
        /// <returns>string</returns>
        private string getDataSourceForCurrentProject()
        {
            StringBuilder projectDataSource = new StringBuilder();
            using (var context = new applicationEntities())
            {
                
                var projects = context.Palinoia_Projects
                                .Where((project) => project.ID == this.currentProjectID);
                foreach (var project in projects)
                {
                    projectDataSource.Append(this.GetEnvData("WorkingDirectory"));
                    projectDataSource.Append("\\");
                    projectDataSource.Append(project.NAME);
                    projectDataSource.Append("\\");
                    projectDataSource.Append(project.DataSource);
                }
            }
            return projectDataSource.ToString();
        }

        /// <summary>
        /// fetches a data source for a selected project from the database
        /// </summary>
        /// <returns>string</returns>
        private string getDataSourceForPalinoia()
        {
            StringBuilder projectDataSource = new StringBuilder();
            projectDataSource.Append(this.GetEnvData("WorkingDirectory"));
            projectDataSource.Append("\\Palinoia.s3db");
            return projectDataSource.ToString();
        }

        private string cleanStringForJSAlertbox(string s)
        {
            //Javascript alert box will not show on client if '\n' or '\r' is
            //contained in the string sent to be displayed
            string retString = s.Replace("\n", "\\n").Replace("\r", "\\r");
            return retString;
        }

        #endregion private methods

        #endregion Instance Methods
    }
}
