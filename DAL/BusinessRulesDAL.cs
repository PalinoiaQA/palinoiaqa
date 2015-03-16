using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace DAL
{    
    /// <summary>
    /// class to hold code for Business Rules data access layer
    /// </summary>
    public class BusinessRulesDAL
    {
        #region properties and variables

        /// <summary>
        /// an ID int to identify a particular project
        /// </summary>
        public int ProjectID { get; set; }
        applicationDAL palinoiaDAL;
        
        #endregion properties and variables

        #region constructors
                
        /// <summary>
        /// constructor with one parameter
        /// </summary>
        /// <param name="projectID">int</param>
        public BusinessRulesDAL(int projectID)
        {
            if (projectID == 0)
            {
                projectID = 1;
            }
            this.ProjectID = projectID;
            palinoiaDAL = new applicationDAL(projectID);
        }

        #endregion constructors

        #region instance methods

        /// <summary>
        /// has business rules
        /// </summary>
        /// <param name="sectionID">int</param>
        /// <returns>bool</returns>
        public bool hasBusinessRules(int sectionID)
        {
            bool hasBusinessRules = false;
            using (var context = palinoiaDAL.getContextForProject())
            {
                var businessRules = context.BusinessRules
                                .FirstOrDefault((br) => br.fk_SectionID == (long)sectionID);
                if (businessRules != null)
                {
                    hasBusinessRules = true;
                }
            }
            return hasBusinessRules;
        }
                
        /// <summary>
        /// fetches a list of all business rules from the database
        /// </summary>
        /// <returns>List&lt;viewBusinessRule&gt;</returns>
        public List<viewBusinessRule> getAllBusinessRules()
        {
            var vBRList = new List<viewBusinessRule>();
            using (var context = palinoiaDAL.getContextForProject())
            {
                var businessRules = context.BusinessRules.OrderBy((br) => br.Name);
                foreach (BusinessRule rule in businessRules)
                {
                    var vRule = new viewBusinessRule((int)rule.ID, 
                                                          rule.Name, 
                                                          (int)rule.fk_StatusID, 
                                                          (int)rule.fk_SectionID,
                                                          rule.Text, 
                                                          (bool)rule.Active,
                                                          (int)rule.UpdatedBy);
                    vBRList.Add(vRule);
                }
            }
            return vBRList;
        }
                
        /// <summary>
        /// fetches a business rule from the database by ID
        /// </summary>
        /// <param name="businessRuleID">int</param>
        /// <returns>viewBusinessRule</returns>
        public viewBusinessRule getBusinessRuleByID(int businessRuleID)
        {
            viewBusinessRule businessRule;
            using (var context = palinoiaDAL.getContextForProject())
            {
                var entityBR = context.BusinessRules
                                          .FirstOrDefault((br) => br.ID == businessRuleID);
                businessRule = new viewBusinessRule((int)entityBR.ID, 
                                                    entityBR.Name, 
                                                    (int) entityBR.fk_StatusID, 
                                                    (int) entityBR.fk_SectionID,
                                                    entityBR.Text, 
                                                    (bool)entityBR.Active,
                                                    (int)entityBR.UpdatedBy);
                return businessRule;
            }
        }
        
        /// <summary>
        /// get all business rules by section
        /// </summary>
        /// <param name="sectionID">int</param>
        /// <returns>List&lt;viewBusinessRule&gt;</returns>
        public List<viewBusinessRule> getAllBusinessRulesBySection(int sectionID)
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                List<viewBusinessRule> brList = new List<viewBusinessRule>();
                var businessRules = context.BusinessRules
                                .Where((br) => br.fk_SectionID == sectionID).OrderBy((br) => br.Name);
                foreach (var br in businessRules)
                {
                    brList.Add(new viewBusinessRule((int)br.ID, 
                                                    br.Name, 
                                                    (int)br.fk_StatusID, 
                                                    (int)br.fk_SectionID, 
                                                    br.Text, 
                                                    (bool)br.Active,
                                                    (int)br.UpdatedBy));

                }
                return brList;
            }
        }

        /// <summary>
        /// get all business rules by status
        /// </summary>
        /// <param name="statusID">int</param>
        /// <returns>List&lt;viewBusinessRule&gt;</returns>
        public List<viewBusinessRule> getAllBusinessRulesByStatus(int statusID)
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                List<viewBusinessRule> brList = new List<viewBusinessRule>();
                var businessRules = context.BusinessRules
                                .Where((br) => br.fk_StatusID == statusID).OrderBy((br) => br.Name);
                foreach (var br in businessRules)
                {
                    brList.Add(new viewBusinessRule((int)br.ID,
                                                    br.Name,
                                                    (int)br.fk_StatusID,
                                                    (int)br.fk_SectionID,
                                                    br.Text,
                                                    (bool)br.Active,
                                                    (int)br.UpdatedBy));

                }
                return brList;
            }
        }
                
        /// <summary>
        /// adds a business rule to the database
        /// </summary>
        /// <param name="vBusinessRule">viewBusinessRule</param>
        /// <returns>string</returns>
        public string addBusinessRule(viewBusinessRule vBusinessRule)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    context.Connection.Open();
                    BusinessRule newBR = BusinessRule.CreateBusinessRule(0, 
                                                                         vBusinessRule.Name, 
                                                                         (int)vBusinessRule.StatusID,
                                                                         vBusinessRule.Text,
                                                                         vBusinessRule.Active,
                                                                         (int)vBusinessRule.SectionID,
                                                                         (int)vBusinessRule.UpdatedBy);
                    //newBR.lkup_Status.Text = vBusinessRule.Status;
                    newBR.Active = vBusinessRule.Active;
                    newBR.fk_SectionID = vBusinessRule.SectionID;
                    context.BusinessRules.AddObject(newBR);
                    //delete relationship between business rule and section?
                    var deleteBRSection = context.BusinessRule_Sections.FirstOrDefault((br) => br.ID == vBusinessRule.ID);
                    if (deleteBRSection != null)
                    {
                        context.BusinessRule_Sections.DeleteObject(deleteBRSection);
                    }
                    //add business rule section relationship
                    var brSection = BusinessRule_Sections.CreateBusinessRule_Sections(0, vBusinessRule.ID, vBusinessRule.SectionID);
                    context.BusinessRule_Sections.AddObject(brSection);
                    context.SaveChanges();
                    result = newBR.ID.ToString();
                }

                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, vBusinessRule.UpdatedBy);
                }
                return result;
            }
        }
                
        /// <summary>
        /// update a business rule in the database
        /// </summary>
        /// <param name="vBusinessRule">viewBusinessRule</param>
        /// <returns>string</returns>
        public string updateBusinessRule(viewBusinessRule vBusinessRule)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var editBR = context.BusinessRules.First((br) => br.ID == vBusinessRule.ID);
                    editBR.Name = vBusinessRule.Name;
                    editBR.Text = vBusinessRule.Text;
                    editBR.fk_StatusID = vBusinessRule.StatusID;
                    editBR.fk_SectionID = vBusinessRule.SectionID;
                    editBR.Active = vBusinessRule.Active;
                    //delete relationship between business rule and section?
                    var deleteBRSection = context.BusinessRule_Sections.FirstOrDefault((br) => br.fk_BusinessRuleID == vBusinessRule.ID);
                    if (deleteBRSection != null)
                    {
                        context.BusinessRule_Sections.DeleteObject(deleteBRSection);
                    }
                    //add business rule section relationship
                    var brSection = BusinessRule_Sections.CreateBusinessRule_Sections(0, vBusinessRule.ID, vBusinessRule.SectionID);
                    context.BusinessRule_Sections.AddObject(brSection);
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, vBusinessRule.UpdatedBy);
                }
            }
            return result;
        }
                
        /// <summary>
        /// delete a business rule from the database
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteBusinessRule(int deleteID, int userID)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var deleteBusinessRule = context.BusinessRules
                           .First((br) => br.ID == deleteID);
                    context.BusinessRules.DeleteObject(deleteBusinessRule);
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, userID);
                }

            }
            return result;
        }
                
        /// <summary>
        /// get business rules by section
        /// </summary>
        /// <param name="sectionID">int</param>
        /// <returns>List&lt;viewBusinessRule&gt;</returns>
        public List<viewBusinessRule> getBusinessRulesBySection(int sectionID)
        {
            var brList = new List<viewBusinessRule>();
            using (var context = palinoiaDAL.getContextForProject())
            {
                //var businessRules = from br in context.BusinessRules
                //                    where context.
                                    

                                    


                //    var businessRules = from br in context.BusinessRules
                //                    where context.TestCase_BusinessRules.Any((tcbr) => (tcbr.fk_BusinessRuleID == br.ID) &&
                //                        (tcbr.fk_TestCaseID == testCaseID))
                //                    select br;
            }
            return brList;
        }

        public List<viewTestCase> getTestCasesReferencingBusinessRule(int businessRuleID)
        {
            var tcDAL = new TestCasesDAL(this.ProjectID);
            var testCases = new List<viewTestCase>();
            var testSteps = new List<viewTestStep>();
            using (var context = palinoiaDAL.getContextForProject())
            {
                var entityTestStepBusinessRules = context.TestStep_BusinessRules
                                    .Where((tsbr) => tsbr.fk_BusinessRuleID == businessRuleID);
                //get list of test steps
                foreach (var record in entityTestStepBusinessRules)
                {
                    //get test step
                    var step = tcDAL.getTestStepByID((int)record.fk_TestStepID);
                    if (step != null)
                    {
                        //get parent test case
                        var testcase = tcDAL.getTestCaseForTestStep(step.ID);
                        if (testcase != null)
                        {
                            //add to return list
                            testCases.Add(testcase);
                        }
                    }
                }
            }

            return testCases;
        }

        public List<viewDefect> getDefectsReferencingBusinessRule(int businessRuleID)
        {
            var defects = new List<viewDefect>();
            var defectDAL = new DefectsDAL(this.ProjectID);
            using (var context = palinoiaDAL.getContextForProject())
            {
                var entityDefectBusinessRules = context.Defect_BusinessRules
                                                .Where((d) => d.fk_BusinessRuleID == businessRuleID);
                foreach (var record in entityDefectBusinessRules)
                {
                    var defect = new viewDefect();
                    defect.ID = (int)record.fk_DefectID;
                    defect.Name = defectDAL.getDefectbyID(defect.ID).Name;
                    defects.Add(defect);
                }
            }


            return defects;
        }

        #endregion instance methods
    }
}
