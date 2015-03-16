using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using Entities;
using OpenSmtp.Mail;

namespace BLL
{
    /// <summary>
    /// class to hold code for DefectsBLL
    /// </summary>
    public class DefectsBLL
    {
        #region properties and variables

        DefectsDAL dal;
        AdminDAL adminDAL;
        applicationDAL appDAL;
        int projectID;

        #endregion properties and variables

        #region constructors

        /// <summary>
        /// constructor for DefectsBLL
        /// </summary>
        /// <param name="projectID">int</param>
        public DefectsBLL(int projectID)
        {
            this.projectID = projectID;
            dal = new DefectsDAL(projectID);
            adminDAL = new AdminDAL(projectID);
            appDAL = new applicationDAL();
        }

        #endregion constructors

        #region public methods

        /// <summary>
        /// fetch all open defects
        /// </summary>
        /// <returns>List&lt;string&gt;</returns>
        public List<viewDefect> getAllOpenDefects()
        {
            List<viewDefect> defectList = new List<viewDefect>();
            var entityDefects = dal.getAllOpenDefects();
            foreach (var defect in entityDefects)
            {
                string dateCreated = defect.DateCreated.ToString("MM/dd/yyyy");
                var vDefect = new viewDefect((int)defect.ID,
                                             (int)defect.fk_DefectPriorityID,
                                             (int)defect.fk_DefectStatusID,
                                             (int)defect.fk_DefectTypeID,
                                             (int)defect.OwnerID,
                                             defect.DefectName,
                                             defect.Description,
                                             dateCreated,
                                             "",
                                             //(DateTime)defect.DateCompleted,
                                             defect.Closed);
                defectList.Add(vDefect);

            }
            return defectList;
        }

        /// <summary>
        /// fetch defect by ID
        /// </summary>
        /// <param name="defectID">int</param>
        /// <returns>viewDefect</returns>
        public viewDefect getDefectByID(int defectID)
        {
            return dal.getDefectbyID(defectID);
        }

        /// <summary>
        /// add new defect
        /// </summary>
        /// <param name="defect">viewDefect</param>
        /// <param name="userID">int</param>
        /// <returns>int</returns>
        public int addNewDefect(viewDefect defect, int userID)
        {
            var result = dal.addNewDefect(defect, userID);
            if (result.Equals("OK"))
            {
                
            }
            return result;
        }

        /// <summary>
        /// Create new defect based for a test step that was indicated as failed in the test runner.
        /// Defect is emailed to owner of original business rule defect.
        /// </summary>
        /// <param name="tr">viewTestResult</param>
        /// <returns>string</returns>
        public string createNewDefectForTestStepFail(viewTestResult tr)
        {
            string result = "";
            //get needed BLL objects
            var brBLL = new BusinessRulesBLL(this.projectID);
            var tcBLL = new TestCasesBLL(this.projectID);
            viewBusinessRule failedBR = new viewBusinessRule();
            var failedTC = tcBLL.getTestCaseByID(tr.PrimaryTestCaseID);
            var failedTS = tcBLL.getTestStepByID(tr.TestStepID);
            try
            {
                //determine defect owner id
                int ownerID = 0;
                var originalBRDefect = this.getDefectForBusinessRuleID(tr.FailedBusinessRuleID);
                if (originalBRDefect != null)
                {
                    //defect owner will be the user originally assigned to the defect create when business rule was added to system
                    if (ownerID > 0)
                    {
                        ownerID = originalBRDefect.OwnerID;
                    }
                    else
                    {
                        ownerID = tr.UserID;
                    }
                }
                else
                {
                    //no associated business rule or no original defect business rule
                    //assign defect to tester
                    ownerID = tr.UserID;
                }
                viewDefect newDefect = new viewDefect();
                newDefect.OwnerID = ownerID;
                newDefect.DateCreated = DateTime.Now.ToString("u");
                newDefect.DefectPriorityID = (int)Enums.defectPriorityEnums.DefectPriority.Medium;
                newDefect.DefectTypeID = (int)Enums.defectTypeEnums.DefectType.Defect;
                newDefect.DefectStatusID = (int)Enums.defectStatusEnums.DefectStatus.New;
                newDefect.Name = "Test Step Failure: " + failedTS.Name;
                StringBuilder desc = new StringBuilder();
                desc.Append("TestRunner failure in Test Case: ");
                desc.Append(failedTC.Name);
                desc.Append(".");
                desc.AppendLine("<BR />");
                desc.AppendLine("<BR />");
                desc.AppendLine("Note from testing: ");
                desc.AppendLine("<BR />");
                desc.AppendLine(tr.Notes);
                desc.AppendLine("<BR />");
                desc.AppendLine("Test Steps:");
                desc.AppendLine("<BR />");
                //list all test steps ups for parent test case
                var testSteps = tcBLL.getTestStepsForTestCase(failedTC.ID);
                foreach (var step in testSteps)
                {
                    //indicated FAILED test step
                    if (step.ID == failedTS.ID)
                    {
                        desc.Append("FAILED ");
                    }
                    desc.Append("Step ");
                    desc.Append(step.SeqNum);
                    desc.Append(" : ");
                    desc.Append(step.Name);
                    if (step.Notes.Length > 0)
                    {
                        desc.Append(" NOTE: " + step.Notes);
                    }
                    desc.AppendLine("<BR />");
                }
                //list failed business rule 
                if (tr.FailedBusinessRuleID > 0)
                {
                    failedBR = brBLL.getBusinessRuleByID(tr.FailedBusinessRuleID);
                    desc.AppendLine("<BR />");
                    desc.AppendLine("Related business rule:");
                    desc.AppendLine("<BR />");
                    desc.Append(failedBR.Name);
                    desc.Append(" : ");
                    desc.Append(failedBR.Text);
                    desc.AppendLine("<BR />");
                    desc.AppendLine("<BR />");
                }
                newDefect.Description = desc.ToString();
                var newID = dal.addNewDefect(newDefect, tr.UserID);

                if (newID > 0)
                {
                    result = "OK";
                    newDefect.ID = newID;
                    this.sendNewDefectEmail(newDefect, ownerID);
                }
                else
                {
                    result = "There was a problem creating a new defect.  Please contact your system administrator.";
                }
            }
            catch (Exception ex)
            {
                appDAL.logError(ex, this.projectID, 0);
            }
            finally
            {
                brBLL = null;
                tcBLL = null;
                failedTC = null;
                failedTS = null;
                failedBR = null;
            }
            return result;
        }

        /// <summary>
        /// create new defect for new business rule
        /// </summary>
        /// <param name="rule">viewBusinessRule</param>
        /// <param name="ownerID">int</param>
        /// <returns>string</returns>
        public string createNewDefectFromBusinessRule(viewBusinessRule rule, int ownerID)
        {
            string result = "";
            viewDefect newDefect = new viewDefect();
            newDefect.OwnerID = ownerID;
            newDefect.DateCreated = DateTime.Now.ToString("u");
            newDefect.DefectPriorityID = (int)Enums.defectPriorityEnums.DefectPriority.Medium;
            newDefect.DefectTypeID = (int)Enums.defectTypeEnums.DefectType.NewConstruction;
            newDefect.DefectStatusID = (int)Enums.defectStatusEnums.DefectStatus.New;
            newDefect.Name = "New Business Rule: " + rule.Name;
            newDefect.Description = rule.Text;
            var newID = dal.addNewDefect(newDefect, rule.UpdatedBy);
            
            if (newID > 0)
            {
                result = "OK";
                newDefect.ID = newID;
                this.createDefectBusinessRuleRelationship(rule.ID, newID, rule.UpdatedBy);
                this.sendNewDefectEmail(newDefect, ownerID);
            }
            else
            {
                result = "There was a problem creating a new defect.  Please contact your system administrator.";
            }
            return result;
        }

        /// <summary>
        /// create update defect for business rule
        /// </summary>
        /// <param name="rule">viewBusinessRule</param>
        /// <param name="ownerID">int</param>
        /// <returns>string</returns>
        public string createUpdateDefectFromBusinessRule(viewBusinessRule rule, int ownerID)
        {
            string result = "";
            viewDefect newDefect = new viewDefect();
            newDefect.OwnerID = ownerID;
            newDefect.DateCreated = DateTime.Now.ToString("u");
            newDefect.DefectPriorityID = (int)Enums.defectPriorityEnums.DefectPriority.Medium;
            newDefect.DefectTypeID = (int)Enums.defectTypeEnums.DefectType.NewConstruction;
            newDefect.DefectStatusID = (int)Enums.defectStatusEnums.DefectStatus.New;
            newDefect.Name = "Updated Business Rule: " + rule.Name;
            newDefect.Description = rule.Text;
            var newID = dal.addNewDefect(newDefect, rule.UpdatedBy);
            if (newID > 0)
            {
                result = "OK";
                newDefect.ID = newID;
                //check for dupes in defect_businessRules table
                int existingID = this.getDefectForBusinessRuleID(rule.ID).ID;
                if (existingID == 0)
                {
                    this.createDefectBusinessRuleRelationship(rule.ID, newID, rule.UpdatedBy);
                }
                sendNewDefectEmail(newDefect, ownerID);
            }
            else
            {
                result = "There was a problem creating a new defect.  Please contact your system administrator.";
            }
            return result;
        }

        /// <summary>
        /// called DAL method to create a defect/business rule relationship in the Defect_BusinessRule table
        /// </summary>
        /// <param name="businessRuleID">int</param>
        /// <param name="defectID">int</param>
        /// <param name="userID">int</param>
        public void createDefectBusinessRuleRelationship(int businessRuleID, int defectID, int userID)
        {
            var result = dal.createDefectBusinessRuleRelationship(businessRuleID, defectID, userID);
        }

        /// <summary>
        /// return business rules associated with defect
        /// </summary>
        /// <param name="defectID">int</param>
        /// <returns>viewBusinessRule</returns>
        public viewBusinessRule getBusinessRuleForDefectID(int defectID)
        {
            return dal.getBusinessRuleForDefectID(defectID);
        }

        /// <summary>
        /// return viewDefect object associated with a Business Rule
        /// </summary>
        /// <param name="businessRuleID">int</param>
        /// <returns>viewDefect</returns>
        public viewDefect getDefectForBusinessRuleID(int businessRuleID)
        {
            return dal.getDefectForBusinessRuleID(businessRuleID);
        }

        /// <summary>
        /// add comment
        /// </summary>
        /// <param name="projectID">int</param>
        /// <param name="comment">viewComment</param>
        /// <returns>string</returns>
        public string addComment(int projectID, viewComment comment)
        {
            string result = "";
            try
            {
                //add id line to first line of comment
                StringBuilder sb = new StringBuilder();
                sb.Append("Entered by: ");
                //get formatted user name from id
                var user = appDAL.getUserByID(comment.UserID);
                sb.Append(user.FirstName);
                sb.Append(" ");
                sb.Append(user.LastName);
                sb.Append(" on ");
                sb.Append(comment.DateCreated.ToString());
                sb.Append(":");
                sb.AppendLine("<br /><br />");
                
                sb.AppendLine(comment.Text);
                comment.Text = sb.ToString();
                result = dal.addComment(comment);
                var defect = dal.getDefectbyID(comment.DefectID);
                this.sendDefectUpdateEmail(comment);
            }
            catch(Exception ex)
            {
                appDAL.logError(ex, projectID, comment.UserID);
            }
            return result;
        }

        /// <summary>
        /// add system comment
        /// </summary>
        /// <param name="comment">viewComment</param>
        /// <returns>string</returns>
        public string addSystemComment(viewComment comment)
        {
            string result = dal.addComment(comment);
            return result;
        }

        /// <summary>
        /// fetch comments for defect
        /// </summary>
        /// <param name="defectID">int</param>
        /// <returns>List&lt;viewComment&gt;</returns>
        public List<viewComment> getCommentsForDefect(int defectID)
        {
            var comments = new List<viewComment>();
            var entityComments = dal.getCommentsForDefect(defectID);
            foreach (var entityComment in entityComments)
            {
                var comment = new viewComment((int)entityComment.ID,
                                              (int)entityComment.fk_DefectID,
                                              (int)entityComment.UserID,
                                              entityComment.DateCreated,
                                              entityComment.Text);
                comments.Add(comment);
            }
            return comments;
        }

        /// <summary>
        /// update defect
        /// </summary>
        /// <param name="updatedDefect">viewDefect</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string updateDefect(viewDefect updatedDefect, int userID)
        {
            string retVal = "";
            viewDefect originalDefect = dal.getDefectbyID(updatedDefect.ID);
            checkForChangeComments(originalDefect, updatedDefect, userID);
            //update closed and status if completion date entered
            if (updatedDefect.DateCompleted.Length > 0)
            {
                updatedDefect.Closed = true;
                updatedDefect.DefectStatusID = (int)Enums.defectStatusEnums.DefectStatus.Completed;
            }
            //update defect
            retVal = dal.updateDefect(updatedDefect, userID);
            return retVal;
        }

        /// <summary>
        /// delete defect by ID
        /// </summary>
        /// <param name="defectID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteDefectByID(int defectID, int userID)
        {
            string deleteResult = "";
            var defectDelete = dal.deleteDefectByID(defectID, userID);
            if (defectDelete.Equals("OK"))
            {
                var businessRuleRelationshipDelete = dal.deleteDefectBusinessRuleRelationshipByDefectID(defectID, userID);
                if (businessRuleRelationshipDelete.Equals("OK"))
                {
                    deleteResult = "OK";
                }
                else
                {
                    deleteResult = businessRuleRelationshipDelete;
                }
            }
            else
            {
                deleteResult = defectDelete;
            }
            return deleteResult;
        }

        /// <summary>
        /// create mail message for new defect and call send method from applicationBLL
        /// </summary>
        /// <param name="defect">viewDefect</param>
        /// <param name="ownerID">int</param>
        public void sendNewDefectEmail(viewDefect defect, int ownerID)
        {
            MailMessage mail = new MailMessage();
            //get owner email for MailTo
            var owner = appDAL.getUserByID(defect.OwnerID);
            mail.To.Add(new EmailAddress(owner.Email));
            mail.Body = removeHTML(defect.Description);
            //set mail subject and priority
            mail.Subject = "New Defect " + defect.ID + ": " + defect.Name;
            mail.Priority = MailPriority.Normal;
            //send mail
            applicationBLL appBLL = new applicationBLL();
            appBLL.sendEmail(mail);
            appBLL = null;
        }

        #endregion public methods

        #region private methods

        /// <summary>
        /// check for changes
        /// </summary>
        /// <param name="origDefect">viewDefect</param>
        /// <param name="updatedDefect">viewDefect</param>
        /// <param name="userID">int</param>
        private void checkForChangeComments(viewDefect origDefect, viewDefect updatedDefect, int userID)
        {
            StringBuilder emailBody = new StringBuilder();
            string changeText = "";
            viewUser user = appDAL.getUserByID(userID);
            changeText = updateChangesInName(origDefect, updatedDefect, user);
            if (changeText.Length > 0) { emailBody.AppendLine(changeText); }
            changeText = updateChangesInDescription(origDefect, updatedDefect, user);
            if (changeText.Length > 0) { emailBody.AppendLine(changeText); }
            changeText = updateChangesInOwner(origDefect, updatedDefect, user);
            if (changeText.Length > 0) { emailBody.AppendLine(changeText); }
            changeText = updateChangesInPriority(origDefect, updatedDefect, user);
            if (changeText.Length > 0) { emailBody.AppendLine(changeText); }
            changeText = updateChangesInType(origDefect, updatedDefect, user);
            if (changeText.Length > 0) { emailBody.AppendLine(changeText); }
            changeText = updateChangesInStatus(origDefect, updatedDefect, user);
            if (changeText.Length > 0) { emailBody.AppendLine(changeText); }
            changeText = updateChangesInCompletedDate(origDefect, updatedDefect, user);
            if (changeText.Length > 0) { emailBody.AppendLine(changeText); }
            changeText = updateChangesInCreatedDate(origDefect, updatedDefect, user);
            if (changeText.Length > 0) { emailBody.AppendLine(changeText); }
            if (emailBody.ToString().Length > 0)
            {
                sendDefectUpdateEmail(emailBody.ToString(), updatedDefect, userID);
            }
        }

        /// <summary>
        /// check for changes in name
        /// </summary>
        /// <param name="origDefect">viewDefect</param>
        /// <param name="updatedDefect">viewDefect</param>
        /// <param name="user">viewUser</param>
        private string updateChangesInName(viewDefect origDefect, viewDefect updatedDefect, viewUser user)
        {
            string updateText = "";
            if (!origDefect.Name.Equals(updatedDefect.Name))
            {
                updateText = addChangeComment(updatedDefect.ID, "Name", origDefect.Name, updatedDefect.Name, user);
            }
            return updateText;
        }

        private string updateChangesInDescription(viewDefect origDefect, viewDefect updatedDefect, viewUser user)
        {
            string updateText = "";
            if (!origDefect.Description.Equals(updatedDefect.Description))
            {
                updateText = addChangeComment(updatedDefect.ID, "Description", origDefect.Description, updatedDefect.Description, user);
            }
            return updateText;
        }

        private string updateChangesInOwner(viewDefect origDefect, viewDefect updatedDefect, viewUser user)
        {
            string updateText = "";
            if (origDefect.OwnerID != updatedDefect.OwnerID)
            {
                updateText = addDDLChangeComment(updatedDefect.ID, "Owner", origDefect.OwnerID, updatedDefect.OwnerID, user);
            }
            return updateText;
        }

        private string updateChangesInPriority(viewDefect origDefect, viewDefect updatedDefect, viewUser user)
        {
            string updateText = "";
            if (origDefect.DefectPriorityID != updatedDefect.DefectPriorityID)
            {
                updateText = addDDLChangeComment(updatedDefect.ID, "Priority", origDefect.DefectPriorityID, updatedDefect.DefectPriorityID, user);
            }
            return updateText;
        }

        private string updateChangesInType(viewDefect origDefect, viewDefect updatedDefect, viewUser user)
        {
            string updateText = "";
            if (origDefect.DefectTypeID != updatedDefect.DefectTypeID)
            {
                updateText = addDDLChangeComment(updatedDefect.ID, "Type", origDefect.DefectTypeID, updatedDefect.DefectTypeID, user);
            }
            return updateText;
        }

        private string updateChangesInStatus(viewDefect origDefect, viewDefect updatedDefect, viewUser user)
        {
            string updateText = "";
            if (origDefect.DefectStatusID != updatedDefect.DefectStatusID)
            {
                updateText = addDDLChangeComment(updatedDefect.ID, "Status", origDefect.DefectStatusID, updatedDefect.DefectStatusID, user);
            }
            return updateText;
        }

        private string updateChangesInCreatedDate(viewDefect origDefect, viewDefect updatedDefect, viewUser user)
        {
            string updateText = "";
            if (origDefect.DateCreated == null) { origDefect.DateCreated = "EMPTY"; }
            if (updatedDefect.DateCreated == null) { updatedDefect.DateCreated = "EMPTY"; }
            if (!origDefect.DateCreated.Equals(updatedDefect.DateCreated))
            {
                updateText = addChangeComment(updatedDefect.ID, "Date Created", origDefect.DateCreated, updatedDefect.DateCreated, user);
            }
            return updateText;
        }

        private string updateChangesInCompletedDate(viewDefect origDefect, viewDefect updatedDefect, viewUser user)
        {
            string updateText = "";
            if (origDefect.DateCompleted == null || origDefect.DateCompleted.Equals("")) { origDefect.DateCompleted = "EMPTY"; }
            if (updatedDefect.DateCompleted == null || updatedDefect.DateCompleted.Equals("")) { updatedDefect.DateCompleted = "EMPTY"; }
            if (!origDefect.DateCompleted.Equals(updatedDefect.DateCompleted))
            {
                updateText = addChangeComment(updatedDefect.ID, "Date Completed", origDefect.DateCompleted, updatedDefect.DateCompleted, user);
            }
            return updateText;
        }

        /// <summary>
        /// add change comment
        /// </summary>
        /// <param name="defectID">int</param>
        /// <param name="changeName">string</param>
        /// <param name="origChange">string</param>
        /// <param name="updatedChange">string</param>
        /// <param name="user">viewUser</param>
        /// <returns>string</returns>
        private string addChangeComment(int defectID, string changeName, string origChange, string updatedChange, viewUser user)
        {
            string retVal = "";
            //add comment
            DateTime createdDate = DateTime.Now;
            StringBuilder changeComment = new StringBuilder();
            changeComment.Append(changeName);
            changeComment.Append(" changed from ");
            changeComment.Append(origChange);
            changeComment.Append(" to ");
            changeComment.Append(updatedChange);
            changeComment.Append(" by ");
            changeComment.Append(user.FirstName);
            changeComment.Append(" ");
            changeComment.Append(user.LastName);
            changeComment.Append(" on ");
            changeComment.Append(createdDate.ToString());
            changeComment.Append(".");
            changeComment.AppendLine();
            viewComment comment = new viewComment(0,
                                                  defectID,
                                                  user.ID,
                                                  createdDate,
                                                  changeComment.ToString());
            retVal = this.addSystemComment(comment);
            if(retVal == "OK") {
                retVal = comment.Text;
            }
            return retVal;
         }

        private string addDDLChangeComment(int defectID, string changeName, int origID, int updatedID, viewUser user)
        {
            string retVal = "";
            string origChange = "";
            string updatedChange = "";
            //add comment
            DateTime createdDate = DateTime.Now;
            StringBuilder changeComment = new StringBuilder();
            changeComment.Append(changeName);
            changeComment.Append(" changed from ");
            switch (changeName)
            {
                case("Owner"):
                    var owner = appDAL.getUserByID(origID);
                    var updatedOwner = appDAL.getUserByID(updatedID);
                    origChange = owner.FirstName + " " + owner.LastName;
                    updatedChange = updatedOwner.FirstName + " " + updatedOwner.LastName;
                    break;
                case("Priority"):
                    var priority = adminDAL.getDefectPriorityByID(origID);
                    var updatedPriority = adminDAL.getDefectPriorityByID(updatedID);
                    origChange = priority.Text;
                    updatedChange = updatedPriority.Text;
                    break;
                case("Type"):
                    var type = adminDAL.getDefectTypeByID(origID);
                    var updatedType = adminDAL.getDefectTypeByID(updatedID);
                    origChange = type.Text;
                    updatedChange = updatedType.Text;
                    break;
                case("Status"):
                    var status = adminDAL.getDefectStatusByID(origID);
                    var updatedStatus = adminDAL.getDefectStatusByID(updatedID);
                    origChange = status.Text;
                    updatedChange = updatedStatus.Text;
                    break;
            }
            changeComment.Append(origChange);
            changeComment.Append(" to ");
            changeComment.Append(updatedChange);
            changeComment.Append(" by ");
            changeComment.Append(user.FirstName);
            changeComment.Append(" ");
            changeComment.Append(user.LastName);
            changeComment.Append(" on ");
            changeComment.Append(createdDate.ToString());
            changeComment.Append(".");
            changeComment.AppendLine();
            viewComment comment = new viewComment(0,
                                                  defectID,
                                                  user.ID,
                                                  createdDate,
                                                  changeComment.ToString());
            retVal = this.addSystemComment(comment);
            if (retVal == "OK")
            {
                retVal = comment.Text;
            }
            return retVal;
        }

        /// <summary>
        /// create mail message for updated defect and call method to send from applicationBLL
        /// </summary>
        /// <param name="emailBody">string</param>
        /// <param name="defect">viewDefect</param>
        /// <param name="ownerID">int</param>
        private void sendDefectUpdateEmail(string emailBody, viewDefect defect, int ownerID)
        {
            MailMessage mail = new MailMessage();
            string mailFrom = GetEnvData("MailFrom");
            //Smtp smtp = null;
            mail.From = new EmailAddress(mailFrom);
            //get owner email for MailTo
            var owner = appDAL.getUserByID(defect.OwnerID);
            mail.To.Add(new EmailAddress(owner.Email));
            //set mail body
            mail.Body = removeHTML(emailBody);
            //set mail subject and priority
            mail.Subject = "Defect " + defect.ID + ": " + defect.Name + " update.";
            mail.Priority = MailPriority.Normal;
            //send mail
            applicationBLL appBLL = new applicationBLL();
            appBLL.sendEmail(mail);
            appBLL = null;
        }

        /// <summary>
        /// create mail message for comment added to defect and call method to send from applicationBLL
        /// </summary>
        /// <param name="comment">viewComment</param>
        private void sendDefectUpdateEmail(viewComment comment)
        {
            MailMessage mail = new MailMessage();
            string mailFrom = GetEnvData("MailFrom");
            //get owner email for MailTo
            var defect = dal.getDefectbyID(comment.DefectID);
            var owner = appDAL.getUserByID(defect.OwnerID);
            mail.To.Add(new EmailAddress(owner.Email));
            
            //set mail body
            mail.Body = removeHTML(defect.Description);
            //set mail subject and priority
            mail.Subject = "Comment added to Defect " + defect.ID + ": " + defect.Name;
            mail.Priority = MailPriority.Normal;
            //send mail
            applicationBLL appBLL = new applicationBLL();
            appBLL.sendEmail(mail);
            appBLL = null;
        }

        private string GetEnvData(string library)
        {
            string returnValue = @"";
            returnValue = @System.Configuration.ConfigurationManager.AppSettings[library].ToString();
            return @returnValue;
        }

        private string removeHTML(string source)
        {
            string result = source.Replace("<BR />", "");
            result = result.Replace("<br />", "");
            result = result.Replace("<p>", "");
            result = result.Replace("</p>", "");
            result = result.Replace("<P>", "");
            result = result.Replace("</P>", "");
            return result;
        }

        #endregion private methods
    }
}
