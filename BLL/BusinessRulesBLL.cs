using System.Collections.Generic;
using DAL;
using Entities;
using System.Text;

namespace BLL
{    
    /// <summary>
    /// class to hold code for BusinessRulesBLL
    /// </summary>
    public class BusinessRulesBLL
    {
        #region properties and variables

        /// <summary>
        /// class variable to store project ID
        /// </summary>
        public int ProjectID { get; set; }
        /// <summary>
        /// class variable for palinoia BLL
        /// </summary>
        public applicationBLL palinoiaBLL;
        /// <summary>
        /// class variable to store dal
        /// </summary>
        public BusinessRulesDAL dal;
        
        #endregion properties and variables

        #region constructors
                
        /// <summary>
        /// constructor for business rules BLL
        /// </summary>
        /// <param name="projectID">int</param>
        public BusinessRulesBLL(int projectID)
        {
            dal = new BusinessRulesDAL(projectID);
            this.ProjectID = projectID;
            this.palinoiaBLL = new applicationBLL();
        }

        #endregion constructors

        #region public methods

        /// <summary>
        /// pass through method for hasBusinessRules
        /// </summary>
        /// <param name="sectionID">int</param>
        /// <returns>bool</returns>
        public bool hasBusinessRules(int sectionID)
        {
            return dal.hasBusinessRules(sectionID);
        }
                
        /// <summary>
        /// fetches all business rules from the database
        /// </summary>
        /// <returns>List&lt;viewBusinessRule&gt;</returns>
        public List<viewBusinessRule> getAllBusinessRules()
        {
            return dal.getAllBusinessRules();
        }
                
        /// <summary>
        /// fetch business rule from the database by ID
        /// </summary>
        /// <param name="businessRuleID">int</param>
        /// <returns>viewBusinessRule</returns>
        public viewBusinessRule getBusinessRuleByID(int businessRuleID)
        {
            return dal.getBusinessRuleByID(businessRuleID);
        }
                
        /// <summary>
        /// get all business rules by section
        /// </summary>
        /// <param name="sectionID">int</param>
        /// <returns>List&lt;viewBusinessRule&gt;</returns>
        public List<viewBusinessRule> getAllBusinessRulesBySection(int sectionID)
        {
            return dal.getAllBusinessRulesBySection(sectionID);
        }

        /// <summary>
        /// get all business rules by status
        /// </summary>
        /// <param name="statusID">int</param>
        /// <returns>List&lt;viewBusinessRule&gt;</returns>
        public List<viewBusinessRule> getAllBusinessRulesByStatus(int statusID)
        {
            return dal.getAllBusinessRulesByStatus(statusID);
        }
                
        /// <summary>
        /// add business rule to database
        /// </summary>
        /// <param name="businessRule">viewBusinessRule</param>
        /// <param name="ownerID">int</param>
        /// <returns>string</returns>
        public string addBusinessRule(viewBusinessRule businessRule, int ownerID)
        {
            string result = "";
            string newID = dal.addBusinessRule(businessRule);
            int newBRID = 0;
            bool parseResult = int.TryParse(newID, out newBRID);
            if (parseResult && newBRID > 0)
            {
                businessRule.ID = newBRID;
                result = "OK";
                if (ownerID > 0)
                {
                    var defectBLL = new DefectsBLL(this.ProjectID);
                    defectBLL.createNewDefectFromBusinessRule(businessRule, ownerID);
                }
            }
            else
            {
                //error occurred during create rule
                result = newID;
            }
            return result;
        }
                
        /// <summary>
        /// delete business rule from database
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteBusinessRule(int deleteID, int userID)
        {
            StringBuilder deleteResult = new StringBuilder();
            //check if business rule in use prior to deletion:
            //is business rule referenced in any defects?
            var referencedDefects = dal.getDefectsReferencingBusinessRule(deleteID);
            if (referencedDefects.Count > 0)
            {
                deleteResult.AppendLine(palinoiaBLL.getCSMByID((int)Enums.csmeEnums.CSM.CSM_BR_1_2));
                foreach (var defect in referencedDefects)
                {
                    deleteResult.Append("ID: ");
                    deleteResult.Append(defect.ID);
                    deleteResult.Append(" Name: ");
                    deleteResult.AppendLine(defect.Name);
                }
            }
            //is business rule referenced in any test cases?
            var referencedTestCases = dal.getTestCasesReferencingBusinessRule(deleteID);
            if (referencedTestCases.Count > 0)
            {
                var adminDAL = new AdminDAL(this.ProjectID);
                deleteResult.AppendLine(palinoiaBLL.getCSMByID((int)Enums.csmeEnums.CSM.CSM_BR_1_1));
                foreach (var tc in referencedTestCases)
                {
                    deleteResult.Append(tc.Name);
                    deleteResult.Append(" in Section: ");
                    deleteResult.AppendLine(adminDAL.getSectionByID(tc.SectionID).Text);
                }
                //clean up memory
                adminDAL = null;
            }
            //if not in use, delete business rule
            if (deleteResult.Length == 0) // no existing references in Defects or TestCases
            {
                var brDelete = dal.deleteBusinessRule(deleteID, userID);
                //delete any defect/business rule relationship
                if (brDelete.Equals("OK"))
                {
                    var defectDAL = new DefectsDAL(this.ProjectID);
                    var defectRelationshipDelete = defectDAL.deleteDefectBusinessRuleRelationshipByDefectID(deleteID, userID);
                    if (defectRelationshipDelete.Equals("OK"))
                    {
                        deleteResult.Append("OK");
                    }
                    else
                    {
                        deleteResult.Append(defectRelationshipDelete);
                    }
                }
                else
                {
                    deleteResult.Append(brDelete);
                }
            }
            return deleteResult.ToString();
        }
               
        /// <summary>
        /// update business rule in database
        /// </summary>
        /// <param name="businessRule">viewBusinessRule</param>
        /// <returns>string</returns>
        public string updateBusinessRule(viewBusinessRule businessRule)
        {
            return dal.updateBusinessRule(businessRule);
        }

        #endregion public methods
    }
}
