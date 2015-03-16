using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DAL;
using Entities;
using OpenSmtp.Mail;

namespace BLL
{
    
    /// <summary>
    /// class to hold the code for the palinoia business logic layer
    /// </summary>
    public class applicationBLL
    {
        #region Properties and Variables

        applicationDAL dal;
        EncryptionHandler eh;
        
        #endregion Properties and Variables

        #region Constructors

        
        /// <summary>
        /// constructor with no parameters
        /// </summary>
        public applicationBLL()
        {
            dal = new applicationDAL();
            eh = new EncryptionHandler();
        }

        
        /// <summary>
        /// constructor with one parameter
        /// </summary>
        /// <param name="projectID">int</param>
        public applicationBLL(int projectID)
        {
            if (projectID == 0)
            {
                projectID = 1;
            }
            dal = new applicationDAL(projectID);
        }

        #endregion Constructors

        #region Instance Methods

        #region csm

        public string getCSMByID(int csmID)
        {
            return dal.getCSMByID(csmID);
        }

        #endregion csm

        #region navigation tree

        /// <summary>
        /// fetch NavigationMenuItem object for a specific id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>NavigationMenuItem</returns>
        public NavigationMenuItem getMenuItemByID(int id)
        {
            return dal.getMenuItemByID(id);
        }

        /// <summary>
        /// return list of navigation nodes based on user features
        /// used to populate the navigation tree
        /// </summary>
        /// <param name="parentID">int</param>
        /// <param name="userID">int</param>
        /// <returns>List&lt;NavigationMenuItem&gt;</returns>
        public List<NavigationMenuItem> getChildNodesForParentID(int parentID, int userID)
        {
            List<NavigationMenuItem> childNodes = new List<NavigationMenuItem>();
            var user = dal.getUserByID(userID);
            var userFeatures = dal.getFeaturesForUser(user);
            var fullNodeList = dal.getChildNodesForParentID(parentID);
            foreach(var node in fullNodeList) {
                if(userFeatures.Exists((n) => n.ID == node.FeatureID))
                {
                    //user has feature for menu item
                    childNodes.Add(node);
                }
            }
            //add all features (DEBUG only!)
            //childNodes = fullNodeList;
            return childNodes;
            
        }

        /// <summary>
        /// returns true if specific node id has related child nodes in db
        /// </summary>
        /// <param name="nodeID">int</param>
        /// <returns>bool</returns>
        public bool hasChildNodes(int nodeID)
        {
            return dal.hasChildNodes(nodeID);
        }

        #endregion navigation tree

        #region projects
                
        /// <summary>
        /// pass-through procedure to permit the DAL to fetch all projects from the database
        /// </summary>
        /// <returns>List&lt;viewProject&gt;</returns>
        public List<viewProject> getAllProjects()
        {
            var projectList = dal.getAllProjects();
            return projectList;
        }

        /// <summary>
        /// fetch project by ID
        /// </summary>
        /// <param name="projectID">int</param>
        /// <returns>viewProject</returns>
        public viewProject getProjectByID(int projectID)
        {
            var entityProject = dal.getProjectByID(projectID);
            viewProject project = new viewProject((int)entityProject.ID,
                                                  entityProject.NAME,
                                                  entityProject.DataSource,
                                                  entityProject.Active,
                                                  (int)entityProject.UpdatedBy);
            return project;
        }
                
        /// <summary>
        /// creates a new project in the database
        /// </summary>
        /// <param name="projectName">string</param>
        /// <param name="projectPath">string</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string createNewProject(string projectName, string workingDir, int userID)
        {
            string result = "";
            //return error if directory for new project already exists which could happen with
            //deleted projects.  Deleting a project only removed the project record from the db
            //the directory and files for the project are not deleted.
            if (!Directory.Exists(workingDir + "//" + projectName))
            {
                Directory.CreateDirectory(workingDir + "//" + projectName);
                Directory.CreateDirectory(workingDir + "//" + projectName + "//" + "DocumentImages");
                StringBuilder projectPath = new StringBuilder();
                projectPath.Append(workingDir);
                projectPath.Append("\\");
                projectPath.Append(projectName);
                projectPath.Append("\\");
                projectPath.Append(projectName);
                projectPath.Append(".s3db");
                StreamReader reader = File.OpenText(workingDir + "\\PalinoiaCreateTablesForNewProject.sql");
                try
                {
                    StringBuilder projectDataSource = new StringBuilder();
                    projectDataSource.Append("data source=\"");
                    projectDataSource.Append(projectPath.ToString());
                    projectDataSource.Append("\"");
                    string createTableSQL = reader.ReadToEnd();
                    reader.Close();
                    reader.Dispose();
                    result = dal.createNewProjectDatabase(createTableSQL, projectDataSource.ToString());
                    if (result.Equals("OK"))
                    {
                        viewProject project = new viewProject(0, projectName, projectName + ".s3db", true, userID);
                        result = dal.saveNewProject(project);
                    }
                }
                catch (IOException ex)
                {
                    result = ex.Message;
                }
                finally
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            else
            {
                //directory with new project name already exists in working directory
                //display CSM.PROJ.1.1 to user
                string message = this.getCSMByID((int)Enums.csmeEnums.CSM.CSM_PROJ_1_1);
                result  = message;
            }
            return result;
        }
                
        /// <summary>
        /// updates a project in the database
        /// </summary>
        /// <param name="project">viewProject</param>
        /// <returns>string</returns>
        public string updateProject(viewProject project)
        {
            var proj = dal.getProjectByID(project.ID);

            var originalProjectName = proj.NAME;
            var originalProjectDataSource = originalProjectName + ".s3db";
            var originalProjectPath = this.GetEnvData("WorkingDirectory") + "/" + originalProjectName + "/";

            var newProjectName = project.Name;
            var newProjectDataSource = newProjectName + ".s3db";
            var newProjectPath = this.GetEnvData("WorkingDirectory") + "/" + newProjectName + "/";
            
            //change db filename
            System.IO.File.Move(@originalProjectPath + originalProjectDataSource, @originalProjectPath + newProjectDataSource);
            //change project directory
            System.IO.Directory.Move(@originalProjectPath, @newProjectPath);
            project.DataSource = newProjectDataSource;
            var result = dal.updateProject(project);
            return result;
        }
                
        /// <summary>
        /// deletes a project in the database
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteProject(int deleteID, int userID)
        {
            return dal.deleteProject(deleteID, userID);
        }

        #endregion projects

        #region users
                
        /// <summary>
        /// log user info in database
        /// </summary>
        /// <param name="user">viewUser</param>
        /// <returns>int</returns>
        public int loginUser(viewUser user)
        {
            //encrypt password before checking against db
            string encryptedPW = eh.EncryptToString(user.PW);
            user.PW = encryptedPW;
            return dal.loginUser(user);
        }
                
        /// <summary>
        /// fetches features for user from database
        /// </summary>
        /// <param name="user">viewUser</param>
        /// <returns>List&lt;viewFeature&gt;</returns>
        public List<viewFeature> getFeaturesForUser(viewUser user)
        {
            List<lkup_Features> entityFeatures =  dal.getFeaturesForUser(user);
            List<viewFeature> featureList = new List<viewFeature>();
            foreach (var entityFeature in entityFeatures)
            {
                viewFeature feature = new viewFeature((int) entityFeature.ID, entityFeature.Text, entityFeature.Active, 0);
                featureList.Add(feature);
            }
            return featureList;
        }
                
        /// <summary>
        /// fetches all users from the database
        /// </summary>
        /// <returns>List&lt;viewUser&gt;</returns>
        public List<viewUser> getAllUsers()
        {
            List<viewUser> userList = dal.getAllUsers();
            return userList;
        }
                
        /// <summary>
        /// fetches user info from the database by ID
        /// </summary>
        /// <param name="userID">int</param>
        /// <returns>viewUser</returns>
        public viewUser getUserByID(int userID)
        {
            var user = dal.getUserByID(userID);
            return user;
        }
                
        /// <summary>
        /// adds user info to the database
        /// </summary>
        /// <param name="user">viewUser</param>
        /// <returns>string</returns>
        public string addUser(viewUser user)
        {
            //encrypt password before storing in db
            var eh = new EncryptionHandler();
            string encryptedPW = eh.EncryptToString(user.PW);
            user.PW = encryptedPW;
            string result = dal.addUser(user);
            return result;
        }
                
        /// <summary>
        /// updates user info in the database
        /// </summary>
        /// <param name="user">viewUser</param>
        /// <returns>string</returns>
        public string updateUser(viewUser user)
        {
            //encrypt password before storing in db
            var eh = new EncryptionHandler();
            if (user.PW.Length > 0)
            {
                //if admin has updated user info but left PW blank
                //do not encrypt blank PW and save it.
                string encryptedPW = eh.EncryptToString(user.PW);
                user.PW = encryptedPW;
            }
            string result = dal.updateUser(user);
            return result;
        }
                
        /// <summary>
        /// deletes user info from the database
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteUser(int deleteID, int userID)
        {
            string result = dal.deleteUser(deleteID, userID);
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
            return dal.updateUserProject(userID, projectID);
        }

        /// <summary>
        /// fetch project for user
        /// </summary>
        /// <param name="userID">int</param>
        /// <returns>int</returns>
        public int getProjectForUser(int userID)
        {
            return dal.getProjectForUser(userID);
        }

        #endregion users

        #region roles
                
        /// <summary>
        /// fetches all roles from the database
        /// </summary>
        /// <returns>List&lt;viewRole&gt;</returns>
        public List<viewRole> getAllRoles()
        {
            List<viewRole> roleList = new List<viewRole>();
            List<lkup_Roles> entityRoleList = dal.getAllRoles();
            foreach (var entityRole in entityRoleList)
            {
                viewRole role = new viewRole((int)entityRole.ID, entityRole.Text, entityRole.Active, 0);
                roleList.Add(role);
            }
            return roleList;
        }
                
        /// <summary>
        /// adds role to the database
        /// </summary>
        /// <param name="role">viewRole</param>
        /// <returns>string</returns>
        public string addRole(viewRole role)
        {
            string result = dal.addRole(role);
            return result;
        }
                
        /// <summary>
        /// updates role info in the database
        /// </summary>
        /// <param name="role">viewRole</param>
        /// <returns>string</returns>
        public string updateRole(viewRole role)
        {
            string result = dal.updateRole(role);
            return result;
        }
                
        /// <summary>
        /// deletes a role from the database
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteRole(int deleteID, int userID)
        {
            string result = dal.deleteRole(deleteID, userID);
            return result;
        }
                
        /// <summary>
        /// fetches all features associated with a particular role from the database
        /// </summary>
        /// <param name="roleID">int</param>
        /// <returns>List&lt;viewFeature&gt;</returns>
        public List<viewFeature> getAssociatedFeaturesForRole(int roleID)
        {
            List<viewFeature> associatedFeatures = new List<viewFeature>();
            List<lkup_Features> entityAssociatedFeatures = dal.getRoleFeatures(roleID);
            foreach (var entityAssociatedFeature in entityAssociatedFeatures)
            {
                viewFeature feature = new viewFeature((int)entityAssociatedFeature.ID,
                                                      entityAssociatedFeature.Text,
                                                      entityAssociatedFeature.Active,
                                                      0);
                associatedFeatures.Add(feature);
            }

            return associatedFeatures;
        }
                
        /// <summary>
        /// fetches all available features for a particular role from the database
        /// </summary>
        /// <param name="roleID">int</param>
        /// <returns>List&lt;viewFeature&gt;</returns>
        public List<viewFeature> getAvailableFeaturesForRole(int roleID)
        {
            List<viewFeature> availableFeatures = new List<viewFeature>();
            List<lkup_Features> entityAvailableFeatures = dal.getAvailableFeaturesForRole(roleID);
            foreach (var entityAvailableFeature in entityAvailableFeatures)
            {
                viewFeature feature = new viewFeature((int)entityAvailableFeature.ID,
                                                      entityAvailableFeature.Text,
                                                      entityAvailableFeature.Active,
                                                      0);
                availableFeatures.Add(feature);
            }
            return availableFeatures;
        }
                
        /// <summary>
        /// fetches role from the database by ID
        /// </summary>
        /// <param name="roleID">int</param>
        /// <returns>viewRole</returns>
        public viewRole getRoleByID(int roleID)
        {
            return dal.getRoleByID(roleID);
        }
                
        /// <summary>
        /// associates a particular feature with a particular role in the database
        /// </summary>
        /// <param name="roleID">int</param>
        /// <param name="featureID">int</param>
        /// <returns>string</returns>
        public string addFeatureToRole(int roleID, int featureID)
        {
            return dal.addFeatureToRole(roleID, featureID);
        }
                
        /// <summary>
        /// remove association between feature and role in the database
        /// </summary>
        /// <param name="roleID">int</param>
        /// <param name="featureID">int</param>
        /// <returns>string</returns>
        public string removeFeatureFromRole(int roleID, int featureID)
        {
            return dal.removeFeatureFromRole(roleID, featureID);
        }

        #endregion roles

        #region features
                
        /// <summary>
        /// fetches all features from the database
        /// </summary>
        /// <returns>List&lt;viewFeature&gt;</returns>
        public List<viewFeature> getAllFeatures()
        {
            List<viewFeature> featureList = new List<viewFeature>();
            List<lkup_Features> entityFeatureList = dal.getAllFeatures();
            foreach (var entityFeature in entityFeatureList)
            {
                viewFeature feature = new viewFeature((int)entityFeature.ID, entityFeature.Text, entityFeature.Active, 0);
                featureList.Add(feature);
            }
            return featureList;
        }
                
        /// <summary>
        /// fetches feature from the database by ID
        /// </summary>
        /// <param name="featureID">int</param>
        /// <returns>viewFeature</returns>
        public viewFeature getFeatureByID(int featureID)
        {
            return dal.getFeatureByID(featureID);
        }
                
        /// <summary>
        /// adds a feature from the database by ID
        /// </summary>
        /// <param name="feature">viewFeature</param>
        /// <returns>string</returns>
        public string addFeature(viewFeature feature)
        {
            string result = dal.addFeature(feature);
            return result;
        }
                
        /// <summary>
        /// updates a feature in the database
        /// </summary>
        /// <param name="feature">viewFeature</param>
        /// <returns>string</returns>
        public string updateFeature(viewFeature feature)
        {
            string result = dal.updateFeature(feature);
            return result;
        }
                
        /// <summary>
        /// deletes a feature from the database by ID
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteFeature(int deleteID, int userID)
        {
            string result = "";
            var projectList = dal.featureUsedInProject(deleteID);
            int listCount = 0;
            if (projectList.Count > 0)
            {
                var csm = this.getCSMByID((int)Enums.csmeEnums.CSM.CSM_FEAT_1_1);
                foreach (var project in projectList)
                {
                    csm += project;
                    listCount ++;
                    if (listCount < projectList.Count)
                    {
                        csm += "<br />";
                    }
                }
                result = csm;
            }
            else
            {
                result = dal.deleteFeature(deleteID, userID);
            }
            return result;
        }

        #endregion features

        #region error log

        /// <summary>
        /// fetches all error log records from Application_Errors table
        /// </summary>
        /// <returns>List&lt;viewError&gt;</returns>
        public List<viewError> getAllActiveErrors()
        {
            var errorList = new List<viewError>();
            var entityErrorList = dal.getAllActiveErrors();
            foreach (var entityError in entityErrorList)
            {
                errorList.Add(new viewError((int)entityError.ID,
                                            entityError.Date,
                                            entityError.Source,
                                            entityError.Message,
                                            entityError.InnerException,
                                            entityError.StackTrace,
                                            (int)entityError.fk_ProjectID,
                                            (int)entityError.fk_UserID));
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
            return dal.getErrorByID(errorID);
        }

        /// <summary>
        /// delete all error log records from Application_Errors table
        /// </summary>
        /// <returns>string</returns>
        public string clearErrors()
        {
            return dal.clearErrors();
        }

        /// <summary>
        /// log error
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="projectID">int</param>
        /// <param name="userID">int</param>
        public void logError(Exception ex, int projectID, int userID)
        {
            dal.logError(ex, projectID, userID);
        }

        #endregion error log

        #region validation
                
        /// <summary>
        /// tests for invalid characters within within a specified string
        /// </summary>
        /// <param name="text">string</param>
        /// <returns>bool</returns>
        public bool isValidText(string text)
        {
            bool validText = false;
            if (Regex.IsMatch(text, @"^[a-zA-Z0-9\s.\?\,\'\;\:\!\-]+$"))
            {
                validText = true;
            }
            else
            {
                validText = false;
            }
            return validText;
        }
        
        #endregion validation

        #region email

        /// <summary>
        /// class to send MailMessage object to SMTP server
        /// </summary>
        /// <param name="mail">MailMessage</param>
        public void sendEmail(MailMessage mail)
        {
            string smtpPort = GetEnvData("SMTPPort");
            var smtpUserName = GetEnvData("SMTPUserName");
            var smtpPassword = GetEnvData("SMTPPassword");
            var smtpServer = GetEnvData("SMTPServer");
            string mailFrom = GetEnvData("MailFrom");
            mail.From = new EmailAddress(mailFrom);
            //send mail
            Smtp smtp = new Smtp(smtpServer, smtpUserName, smtpPassword, Convert.ToInt32(smtpPort));
            try
            {
                smtp.SendMail(mail);
            }
            catch (Exception ex)
            {
                logError(ex, 0, 0);
            }
        }

        #endregion email

        #region utilities

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
        /// return a bool to indicate whether development mode is on or off
        /// </summary>
        /// <returns>bool</returns>
        public bool isDevelopmentMode()
        {
            bool devMode = false;
            if (GetEnvData("development").ToLower().Equals("true"))
            {
                devMode = true;
            }
            return devMode;
        }

        #endregion utilities

        #endregion Instance Methods
    }
}
