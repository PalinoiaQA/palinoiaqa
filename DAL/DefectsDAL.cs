using System;
using System.Collections.Generic;
using System.Linq;
using Entities;

namespace DAL
{
    /// <summary>
    /// data access class layer for defect objects
    /// </summary>
    public class DefectsDAL
    {
        #region properties and variables

        /// <summary>
        /// class variable to store project ID
        /// </summary>
        public int ProjectID { get; set; }
        
        /// <summary>
        /// class variable to store application DAL
        /// </summary>
        applicationDAL appDAL;

        #endregion properties and variables

        #region constructors

        /// <summary>
        /// constructor for documentsDAL
        /// </summary>
        /// <param name="projectID">int</param>
        public DefectsDAL(int projectID)
        {
            this.ProjectID = projectID;
            appDAL = new applicationDAL(projectID);
        }

        #endregion constructors

        #region public methods

        /// <summary>
        /// fetch all open (closed = false) defects in database
        /// ordered by created date descending (newest on top)
        /// </summary>
        /// <returns>List&lt;Defect&gt;</returns>
        public List<Defect> getAllOpenDefects()
        {
            List<Defect> defectList = new List<Defect>();
            using (var context = appDAL.getContextForProject())
            {
                try
                {
                    var entityDefects = from d in context.Defects
                                        where d.Closed == false
                                        orderby d.DateCreated descending
                                        select d;
                    defectList = entityDefects.ToList<Defect>();
                }
                catch (Exception ex)
                {
                    appDAL.logError(ex, this.ProjectID, 0);
                }


                return defectList;
            }
        }

        /// <summary>
        /// build viewDefect object from database record for selected ID
        /// </summary>
        /// <param name="defectID">int</param>
        /// <returns>viewDefect</returns>
        public viewDefect getDefectbyID(int defectID) 
        {
            var defect = new viewDefect();
            using (var context = appDAL.getContextForProject())
            {
                try
                {
                    var entityDefect = context.Defects
                                            .FirstOrDefault((d) => d.ID == defectID);
                    string dateCreated = entityDefect.DateCreated.ToString("MM/dd/yyyy");
                    string dateCompleted = entityDefect.DateCompleted.ToString();
                    if (entityDefect.DateCompleted != null)
                    {
                        
                        defect = new viewDefect((int)entityDefect.ID,
                                                (int)entityDefect.fk_DefectPriorityID,
                                                (int)entityDefect.fk_DefectStatusID,
                                                (int)entityDefect.fk_DefectTypeID,
                                                (int)entityDefect.OwnerID,
                                                entityDefect.DefectName,
                                                entityDefect.Description,
                                                dateCreated,
                                                dateCompleted,
                                                entityDefect.Closed);
                    }
                    else
                    {
                        defect = new viewDefect((int)entityDefect.ID,
                                                (int)entityDefect.fk_DefectPriorityID,
                                                (int)entityDefect.fk_DefectStatusID,
                                                (int)entityDefect.fk_DefectTypeID,
                                                (int)entityDefect.OwnerID,
                                                entityDefect.DefectName,
                                                entityDefect.Description,
                                                dateCreated,
                                                entityDefect.Closed);
                    }
                    return defect;
                                             
                }
                catch (Exception ex)
                {
                    appDAL.logError(ex, this.ProjectID, 0);
                }
            }
            return defect;
        }

        /// <summary>
        /// updates database record from viewDefect
        /// </summary>
        /// <param name="defect">viewDefect</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string updateDefect(viewDefect defect, int userID)
        {
            string result = "";
            using (var context = appDAL.getContextForProject())
            {
                try
                {
                    var entityDefect = context.Defects
                                            .FirstOrDefault((d) => d.ID == defect.ID);
                    entityDefect.fk_DefectPriorityID = defect.DefectPriorityID;
                    entityDefect.fk_DefectStatusID = defect.DefectStatusID;
                    entityDefect.fk_DefectTypeID = defect.DefectTypeID;
                    entityDefect.OwnerID = defect.OwnerID;
                    entityDefect.DefectName = defect.Name;
                    entityDefect.Description = defect.Description;
                    entityDefect.DateCreated = Convert.ToDateTime(defect.DateCreated);
                    if (!defect.DateCompleted.Equals("EMPTY"))
                    {
                        entityDefect.DateCompleted = Convert.ToDateTime(defect.DateCompleted);
                    }
                    else
                    {
                        entityDefect.DateCompleted = null;
                    }
                    entityDefect.Closed = defect.Closed;
                    entityDefect.UpdatedBy = userID;
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = appDAL.logError(ex, this.ProjectID, userID);
                }
            }


            return result;
        }

        /// <summary>
        /// add new defect
        /// </summary>
        /// <param name="defect">viewDefect</param>
        /// <param name="userID">int</param>
        /// <returns>int</returns>
        public int addNewDefect(viewDefect defect, int userID)
        {
            int newID = 0;
            using (var context = appDAL.getContextForProject())
            {
                try
                {
                    Defect entityNewDefect = Defect.CreateDefect((long)defect.ID,
                                                                 (long)defect.DefectPriorityID,
                                                                 (long)defect.DefectStatusID,
                                                                 (long)defect.DefectTypeID,
                                                                 defect.Name,
                                                                 defect.Description,
                                                                 Convert.ToDateTime(defect.DateCreated),
                                                                 false,
                                                                 (long)userID);
                                                                 
                    entityNewDefect.OwnerID = (long)defect.OwnerID;
                    context.Defects.AddObject(entityNewDefect);
                    context.SaveChanges();
                    newID = (int)entityNewDefect.ID;
                }
                catch (Exception ex)
                {
                    appDAL.logError(ex, this.ProjectID, userID);
                }
            }
            return newID;
        }

        /// <summary>
        /// deletes Defect record from Defects table by ID
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteDefectByID(int deleteID, int userID)
        {
            string result = "";
            using (var context = appDAL.getContextForProject())
            {
                try
                {
                    var deleteDefect = context.Defects
                           .First((d) => d.ID == deleteID);
                    context.Defects.DeleteObject(deleteDefect);
                    context.SaveChanges();
                    //delete all comments for defect id
                    result = this.deleteCommentsForDefect(deleteID);
                    if (result.Length > 0)
                    {
                        result = "OK";
                    }
                }
                catch (Exception ex)
                {
                    result = appDAL.logError(ex, this.ProjectID, userID);
                }
            }
            return result;
        }

        /// <summary>
        /// add comment
        /// </summary>
        /// <param name="comment">viewComment</param>
        /// <returns>string</returns>
        public string addComment(viewComment comment)
        {
            string result = "";
            using (var context = appDAL.getContextForProject())
            {
                try
                {
                    Defect_Comments entityComment = Defect_Comments.CreateDefect_Comments(comment.ID,
                                                                                          comment.DefectID,
                                                                                          comment.UserID,
                                                                                          comment.DateCreated,
                                                                                          comment.Text);
                    context.Defect_Comments.AddObject(entityComment);
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = appDAL.logError(ex, this.ProjectID, comment.UserID);
                }
            }
            return result;
        }

        /// <summary>
        /// fetch comments for defect
        /// </summary>
        /// <param name="defectID">int</param>
        /// <returns>List&lt;Defect_Comments&gt;</returns>
        public List<Defect_Comments> getCommentsForDefect(int defectID)
        {
            List<Defect_Comments> comments = new List<Defect_Comments>();
            using (var context = appDAL.getContextForProject())
            {
                try
                {
                    var entityComments = from c in context.Defect_Comments
                                         where c.fk_DefectID == defectID
                                         orderby c.DateCreated descending
                                         select c;
                    comments = entityComments.ToList<Defect_Comments>();
                }
                catch (Exception ex)
                {
                    appDAL.logError(ex, this.ProjectID, 0);
                }
            }
            return comments;
        }

        /// <summary>
        /// Deletes all comment records from Defect_Comments for specific defect id.
        /// This is used to delete associated comment records when a defect is deleted.
        /// </summary>
        /// <param name="defectID">int</param>
        /// <returns>string</returns>
        public string deleteCommentsForDefect(int defectID)
        {
            string result = "";
            using (var context = appDAL.getContextForProject())
            {
                try
                {
                    var deleteRecords = from d in context.Defect_Comments
                                        where d.fk_DefectID == (long)defectID
                                        select d;
                    foreach (var record in deleteRecords)
                    {
                        context.DeleteObject(record);
                    }
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = appDAL.logError(ex, this.ProjectID, 0);
                }
            }
            return result;
        }

        /// <summary>
        /// stores businessRuleID and defectID in Defect_BusinessRule table
        /// </summary>
        /// <param name="businessRuleID">int</param>
        /// <param name="defectID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string createDefectBusinessRuleRelationship(int businessRuleID, int defectID, int userID)
        {
            string result = "";
            using (var context = appDAL.getContextForProject())
            {
                try
                {
                    var entityDefectBusinessRuleRecord = context.Defect_BusinessRules.CreateObject<Defect_BusinessRules>();
                    entityDefectBusinessRuleRecord.fk_BusinessRuleID = businessRuleID;
                    entityDefectBusinessRuleRecord.fk_DefectID = defectID;
                    context.Defect_BusinessRules.AddObject(entityDefectBusinessRuleRecord);
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = appDAL.logError(ex, this.ProjectID, userID);
                }
            }
            return result;
        }

        /// <summary>
        /// delete all relationship records in Defect_BusinessRules that contain business rule id parameter
        /// </summary>
        /// <param name="businessRuleID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteDefectBusinessRuleRelationshipByBRID(int businessRuleID, int userID)
        {
            string result = "";
            using (var context = appDAL.getContextForProject())
            {
                try
                {
                    var deleteRecords = from d in context.Defect_BusinessRules
                                        where d.fk_BusinessRuleID == (long)businessRuleID
                                        select d;
                    foreach (var record in deleteRecords)
                    {
                        context.DeleteObject(record);
                    }
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    result = appDAL.logError(ex, this.ProjectID, userID);
                }
            }
            return result;
        }

        /// <summary>
        /// delete all relationship records in Defect_BusinessRules that contain defect id parameter
        /// </summary>
        /// <param name="defectID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteDefectBusinessRuleRelationshipByDefectID(int defectID, int userID)
        {
            string result = "";
            using (var context = appDAL.getContextForProject())
            {
                try
                {
                    var deleteRecords = from d in context.Defect_BusinessRules
                                        where d.fk_DefectID == (long)defectID
                                        select d;
                    foreach (var record in deleteRecords)
                    {
                        context.DeleteObject(record);
                    }
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = appDAL.logError(ex, this.ProjectID, userID);
                }
            }
            return result;
        }

        /// <summary>
        /// get a single viewBusinessRule record associated with the defect id parameter
        /// in Defect_BusinessRule table
        /// </summary>
        /// <param name="defectID">int</param>
        /// <returns>viewBusinessRule</returns>
        public viewBusinessRule getBusinessRuleForDefectID(int defectID)
        {
            viewBusinessRule rule = new viewBusinessRule();
            using (var context = appDAL.getContextForProject())
            {
                try
                {
                    var record = context.Defect_BusinessRules
                        .FirstOrDefault((d) => d.fk_DefectID == defectID);
                    var brID = (int)record.fk_DefectID;
                    var brDAL = new BusinessRulesDAL(this.ProjectID);
                    rule = brDAL.getBusinessRuleByID((int)brID);
                    brDAL = null;
                }
                catch (Exception ex)
                {
                    string result = appDAL.logError(ex, this.ProjectID, 0);
                }
            }
            return rule;
        }

        /// <summary>
        /// get a single viewDefect record for businessRuleID parameter in the
        /// Defect_BusinessRule table
        /// </summary>
        /// <param name="businessRuleID">int</param>
        /// <returns>viewDefect</returns>
        public viewDefect getDefectForBusinessRuleID(int businessRuleID)
        {
            viewDefect defect = new viewDefect();
            using (var context = appDAL.getContextForProject())
            {
                try
                {
                    var record = context.Defect_BusinessRules
                        .FirstOrDefault((d) => d.fk_BusinessRuleID == businessRuleID);
                    if (record != null)
                    {
                        var defectID = (int)record.fk_DefectID;
                        defect = this.getDefectbyID((int)defectID);
                    }
                }
                catch (Exception ex)
                {
                    string result = appDAL.logError(ex, this.ProjectID, 0);
                }
            }
            return defect;
        }

        #endregion public methods

        #region private methods



        #endregion private methods

    }
}
