using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using System.Text;
using Enums;

namespace DAL
{    
    /// <summary>
    /// class to provide data access for TestCasesBLL object
    /// </summary>
    public class TestCasesDAL
    {
        #region properties and variables

        /// <summary>
        /// an ID int to identify the current project being worked on 
        /// </summary>
        public int currentProjectID { get; set; }
        applicationDAL palinoiaDAL;

        #endregion properties and variables

        #region constructors
                
        /// <summary>
        /// constructor with one parameter
        /// </summary>
        /// <param name="projectID">int</param>
        public TestCasesDAL(int projectID)
        {
            this.currentProjectID = projectID;
            palinoiaDAL = new applicationDAL(projectID);
        }

        #endregion constructors

        #region instance methods

        #region test cases
                
        /// <summary>
        /// fetch all test cases from the database
        /// </summary>
        /// <returns>List&lt;TestCase&gt;</returns>
        public List<TestCase> getAllTestCases()
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                List<TestCase> testCasesList = new List<TestCase>();
                try
                {
                    var testCases = context.TestCases
                                    .OrderBy((tc) => tc.Name);
                    testCasesList = testCases.ToList<TestCase>();
                    
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.currentProjectID, 0);
                }
                return testCasesList;
            }
        }
                
        /// <summary>
        /// fetch a test case from the database by ID
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <returns>TestCase</returns>
        public TestCase getTestCaseByID(int testCaseID)
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                var testCase = context.TestCases
                                .First((tc) => tc.ID == testCaseID);
                return testCase; 
            }
        }
                
        /// <summary>
        /// get all test cases by section
        /// </summary>
        /// <param name="sectionID">int</param>
        /// <returns>List&lt;viewTestCase>&gt;</returns>
        public List<viewTestCase> getAllTestCasesBySection(int sectionID)
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                List<viewTestCase> tcList = new List<viewTestCase>();
                var testCases = context.TestCases
                                .Where((tc) => tc.fk_lkup_SectionID == sectionID)
                                .OrderBy((tc) => tc.Name);
                foreach (var testCase in testCases)
                {
                    tcList.Add(new viewTestCase((int)testCase.ID, 
                                                testCase.Name, 
                                                (int)testCase.fk_lkup_SectionID, 
                                                testCase.Active,
                                                (int)testCase.UpdatedBy,
                                                (int)testCase.fk_TestStatusID));

                }
                return tcList;
            }
        }
                
        /// <summary>
        /// add a test case to the database
        /// </summary>
        /// <param name="tc">viewTestCase</param>
        /// <returns>string</returns>
        public string addNewTestCase(viewTestCase tc)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    TestCase newTestCase = TestCase.CreateTestCase(0,tc.Name, tc.Active, tc.UpdatedBy, (long)Enums.testStatusEnums.TestStatus.Untested);
                    newTestCase.fk_lkup_SectionID = tc.SectionID;
                    context.TestCases.AddObject(newTestCase);
                    context.SaveChanges();
                    result = "ID_" + newTestCase.ID;
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.currentProjectID, tc.UpdatedBy);
                }
                return result;
            }
        }
                
        /// <summary>
        /// delete a test case and its' steps from the database
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteTestCase(int deleteID, int userID)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var deleteTestCase = context.TestCases
                           .First((tc) => tc.ID == deleteID);
                    context.TestCases.DeleteObject(deleteTestCase);
                    context.SaveChanges();
                    //delete all test steps for test case
                    var testSteps = from ts in context.TestSteps
                                    where context.TestCase_TestSteps.Any((tcts) => tcts.fk_TestStepID == ts.ID &&
                                                                                   tcts.fk_TestCaseID == deleteID)
                                    select ts;
                    foreach (var testStep in testSteps)
                    {
                        context.TestSteps.DeleteObject(testStep);
                    }
                    //delete all test case/test step relationships
                    var testStepRelationships = from tcts in context.TestCase_TestSteps
                                                where tcts.fk_TestCaseID == deleteID
                                                select tcts;
                    foreach (var testStepRelationship in testStepRelationships)
                    {
                        context.TestCase_TestSteps.DeleteObject(testStepRelationship);
                    }
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.currentProjectID, userID);
                }

            }
            return result;
        }
                
        /// <summary>
        /// update a test case in the database
        /// </summary>
        /// <param name="vtestCase">viewTestCase</param>
        /// <returns>string</returns>
        public string updateTestCase(viewTestCase vtestCase)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    TestCase testCase = context.TestCases
                                        .First((tc) => tc.ID == vtestCase.ID);
                    testCase.Name = vtestCase.Name;
                    testCase.fk_lkup_SectionID = vtestCase.SectionID;
                    testCase.UpdatedBy = vtestCase.UpdatedBy;
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.currentProjectID, vtestCase.UpdatedBy);
                }
                return result;
            }
        }
                
        /// <summary>
        /// get business rules for test case
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <returns>List&lt;viewBusinessRule&gt;</returns>
        public List<viewBusinessRule> getBusinessRulesForTestCase(int testCaseID)
        {
            var brList = new List<viewBusinessRule>();
            using (var context = palinoiaDAL.getContextForProject())
            {
                var businessRules = from br in context.BusinessRules
                                    where context.TestCase_BusinessRules.Any((tcbr) => (tcbr.fk_BusinessRuleID == br.ID) &&
                                        (tcbr.fk_TestCaseID == testCaseID))
                                    select br;
                foreach (var businessRule in businessRules)
                {
                    brList.Add(new viewBusinessRule((int)businessRule.ID,
                                                    businessRule.Name,
                                                    (int)businessRule.fk_StatusID,
                                                    (int)businessRule.fk_SectionID,
                                                    businessRule.Text,
                                                    (bool)businessRule.Active,
                                                    (int)businessRule.UpdatedBy));
                }
            }
            return brList;
        }

        /// <summary>
        /// create test case/business rule relationship records in db from generic list
        /// of associated view business rules
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <param name="brList">List&lt;viewBusinessRule&gt;</param>
        /// <returns></returns>
        public string createTestCaseBusinessRuleRelationships(int testCaseID, List<viewBusinessRule> brList)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    //delete all existing relationships before creating new ones
                    result = this.deleteBusinessRuleRelationshipsForTestCaseID(testCaseID);
                     if(result.Equals("OK")) {
                        foreach(var rule in brList) {
                            TestCase_BusinessRules relationship = TestCase_BusinessRules.CreateTestCase_BusinessRules(
                                    0,
                                    (long)testCaseID,
                                    (long)rule.ID
                                    );
                            context.TestCase_BusinessRules.AddObject(relationship);
                            context.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.currentProjectID, 0);
                }
            }
            return result;
        }

        /// <summary>
        /// delete all test case/business rule relationship for a specific test case id
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <returns>string</returns>
        public string deleteBusinessRuleRelationshipsForTestCaseID(int testCaseID)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    //delete all test case/business rule relationships
                    var testCaseRelationships = from tcbr in context.TestCase_BusinessRules
                                                where tcbr.fk_TestCaseID == testCaseID
                                                select tcbr;
                    foreach (var relationship in testCaseRelationships)
                    {
                        context.TestCase_BusinessRules.DeleteObject(relationship);
                    }
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.currentProjectID, 0);
                }
            }
            return result;
        }

        /// <summary>
        /// delete all test case/business rule relationships for a specific business rule id
        /// </summary>
        /// <param name="businessRuleID">int</param>
        /// <returns>string</returns>
        public string deleteTestCaseRelationshipsForBusinessRuleID(int businessRuleID)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    //delete all test case/business rule relationships
                    var testCaseRelationships = from tcbr in context.TestCase_BusinessRules
                                                where tcbr.fk_BusinessRuleID == businessRuleID
                                                select tcbr;
                    foreach (var relationship in testCaseRelationships)
                    {
                        context.TestCase_BusinessRules.DeleteObject(relationship);
                    }
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.currentProjectID, 0);
                }
            }
            return result;
        }

        /// <summary>
        /// update test case status
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <param name="testStatusID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string updateTestCaseStatus(int testCaseID, int testStatusID, int userID)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    TestCase testCase = context.TestCases
                                        .First((tc) => tc.ID == testCaseID);
                    testCase.fk_TestStatusID = testStatusID;
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.currentProjectID, userID);
                }
                return result;
            }
        }

        /// <summary>
        /// has test cases
        /// </summary>
        /// <param name="sectionID">int</param>
        /// <returns>bool</returns>
        public bool hasTestCases(int sectionID)
        {
            bool hasTestCases = false;
            using (var context = palinoiaDAL.getContextForProject())
            {
                var testCases = context.TestCases
                                .FirstOrDefault((tc) => tc.fk_lkup_SectionID == (long)sectionID);
                if (testCases != null)
                {
                    hasTestCases = true;
                }
            }
            return hasTestCases;
        }

        #endregion test cases

        #region test steps

        /// <summary>
        /// has test steps
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <returns>bool</returns>
        public bool hasTestSteps(int testCaseID)
        {
            bool hasTestSteps = false;
            using (var context = palinoiaDAL.getContextForProject())
            {
                var testCases = context.TestCase_TestSteps
                                .FirstOrDefault((ts) => ts.fk_TestCaseID == (long)testCaseID);
                if (testCases != null)
                {
                    hasTestSteps = true;
                }
            }
            return hasTestSteps;
        }

        /// <summary>
        /// return generic list of viewTestStep objects where Active = true
        /// </summary>
        /// <returns>List&lt;viewTestStep&gt;</returns>
        public List<TestStep> getActiveTestSteps()
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var entityTestSteps = context.TestSteps
                                            .Where((s) => s.Active == true)
                                            .OrderBy((ts) => ts.Text);
                    List<TestStep> tsList = entityTestSteps.ToList<TestStep>();
                    return tsList;
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.currentProjectID, 0);
                }
                return new List<TestStep>();
            }
        }
                
        /// <summary>
        /// fetch test step from the database by ID
        /// </summary>
        /// <param name="testStepID">int</param>
        /// <returns>TestStep</returns>
         public viewTestStep getTestStepByID(int testStepID)
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                List<viewBusinessRule> relatedBRs = new List<viewBusinessRule>();
                var entityTestStep = context.TestSteps
                                .FirstOrDefault((ts) => ts.ID == testStepID);
                var relatedBusinessRules = from br in context.BusinessRules
                                           where context.TestStep_BusinessRules.Any((tsbr) => tsbr.fk_BusinessRuleID == br.ID &&
                                                                              tsbr.fk_TestStepID == testStepID)
                                           select br;
                foreach (var relatedBusinessRule in relatedBusinessRules)
                {
                    viewBusinessRule br = new viewBusinessRule((int)relatedBusinessRule.ID,
                                                               relatedBusinessRule.Name,
                                                               0,
                                                               0,
                                                               " ",
                                                               true,
                                                               0);
                    relatedBRs.Add(br);
                }
                viewTestStep viewTS = new viewTestStep((int)entityTestStep.ID, 
                                                        entityTestStep.Text, 
                                                        0, 
                                                        relatedBRs, 
                                                        "", 
                                                        entityTestStep.Active,
                                                        (int)entityTestStep.UpdatedBy);
                return viewTS; 
            }
        }
                
        /// <summary>
        /// overloaded method to fetch test step info by id including teststep/testcase
        /// specific info since testcase id is passed in as a parameter
        /// </summary>
        /// <param name="testStepID">int</param>
        /// <param name="testCaseID">int</param>
        /// <returns>viewTestStep</returns>
        public viewTestStep getTestStepByID(int testStepID, int testCaseID)
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                List<viewBusinessRule> relatedBRs = new List<viewBusinessRule>();
                string notes = "";
                int seqNum = 0;
                var entityTestStep = context.TestSteps
                                .First((ts) => ts.ID == testStepID);
                //get seq number and notes (per testcase id)
                var tcSpecificTestSteps = from tcts in context.TestCase_TestSteps
                                          where tcts.fk_TestCaseID == testCaseID &&
                                            tcts.fk_TestStepID == entityTestStep.ID
                                          select tcts;
                foreach (var ts in tcSpecificTestSteps)//there will be only one
                {
                   seqNum = (int)ts.SeqNum;
                   notes = ts.Notes;
                }
                var relatedBusinessRules = from br in context.BusinessRules
                                           where context.TestStep_BusinessRules.Any((tsbr) => tsbr.fk_BusinessRuleID == br.ID &&
                                                                              tsbr.fk_TestStepID == testStepID)
                                           select br;
                foreach (var relatedBusinessRule in relatedBusinessRules)
                {
                    viewBusinessRule br = new viewBusinessRule((int)relatedBusinessRule.ID,
                                                               relatedBusinessRule.Name,
                                                               0,
                                                               0,
                                                               " ",
                                                               true,
                                                               0);
                    relatedBRs.Add(br);

                }
                viewTestStep viewTS = new viewTestStep((int)entityTestStep.ID, 
                                                        entityTestStep.Text, 
                                                        seqNum, 
                                                        relatedBRs, 
                                                        notes, 
                                                        entityTestStep.Active,
                                                        (int)entityTestStep.UpdatedBy);
                return viewTS;
            }
        }
                
        /// <summary>
        /// add test step to a particular test case in the database
        /// </summary>
        /// <param name="ts">viewTestStep</param>
        /// <param name="testCaseID">int</param>
        /// <returns>string</returns>
        public string addNewTestStep(viewTestStep ts, int testCaseID)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    TestStep newTestStep = TestStep.CreateTestStep(0, ts.Name, ts.Active, (long)ts.UpdatedBy);
                    newTestStep.Active = ts.Active;
                    context.TestSteps.AddObject(newTestStep);
                    context.SaveChanges();
                    ts.ID = (int)newTestStep.ID;
                    addTestCaseTestStepRelationship(testCaseID, ts);
                    if (ts.RelatedBusinessRules.Count > 0)
                    {
                        foreach (var businessRule in ts.RelatedBusinessRules)
                        {
                            addTestStep_BusinessRuleRelationship((int)newTestStep.ID, businessRule.ID);
                        }
                    }
                    result = newTestStep.ID.ToString();
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.currentProjectID, ts.UpdatedBy);
                }
                return result;
            }
        }
                
        /// <summary>
        /// remove relationship link between test step and business rule
        /// </summary>
        /// <param name="testStepID">int</param>
        /// <param name="businessRuleID">int</param>
        /// <returns>string</returns>
        public string removeTestStepBusinessRuleRelationship(int testStepID, int businessRuleID)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    //delete all test step/business rule relationships
                    var testStepRelationships = from tsbr in context.TestStep_BusinessRules
                                                    where tsbr.fk_BusinessRuleID == businessRuleID &&
                                                          tsbr.fk_TestStepID == testStepID
                                                    select tsbr;
                    foreach (var testStepRelationship in testStepRelationships)
                    {
                        context.TestStep_BusinessRules.DeleteObject(testStepRelationship);
                    }
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.currentProjectID, 0);
                }

            }
            return result;
        }
                
        /// <summary>
        /// remove relationship link between test step and test case
        /// </summary>
        /// <param name="testStepID">int</param>
        /// <param name="testCaseID">int</param>
        /// <returns>string</returns>
        public string removeTestStepTestCaseRelationship(int testStepID, int testCaseID)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    //delete all test step/business rule relationships
                    var testCaseRelationships = from tcts in context.TestCase_TestSteps
                                                where tcts.fk_TestCaseID == testCaseID &&
                                                      tcts.fk_TestStepID == testStepID
                                                select tcts;
                    foreach (var testCaseRelationship in testCaseRelationships)
                    {
                        context.TestCase_TestSteps.DeleteObject(testCaseRelationship);
                    }
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.currentProjectID, 0);
                }

            }
            return result;
        }

        /// <summary>
        /// Deletes all test step/business rule relationship records in db.  Typically called prior 
        /// to saving new list of relationships
        /// </summary>
        /// <param name="testStepID">int</param>
        /// <param name="testCaseID">int</param>
        public void removeAllTestStepBusinessRuleRelationshipsForTestCaseID(int testStepID, int testCaseID)
        {
            viewTestStep ts = this.getTestStepByID(testStepID, testCaseID);
            foreach (var br in ts.RelatedBusinessRules)
            {
                this.removeTestStepBusinessRuleRelationship(testStepID, br.ID);
            }
        }

        /// <summary>
        /// updates test step if test case/test step relationship exists.
        /// adds test case/test step relationship if it doesn't already exist before updating
        /// </summary>
        /// <param name="updateTestStep">viewTestStep</param>
        /// <param name="testCaseID">int</param>
        public void updateTestStepForTestCase(viewTestStep updateTestStep, int testCaseID)
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                //try to select entity object for test step/test case relationship
                var tcTestStep = context.TestCase_TestSteps
                                    .FirstOrDefault((tcts) => ((tcts.fk_TestStepID == updateTestStep.ID) &&
                                                     (tcts.fk_TestCaseID == testCaseID))
                                    );
                //is the test step associated with the test case?
                if (tcTestStep == null) // no
                {
                    //add test case/test step relationship
                    this.addTestCaseTestStepRelationship(testCaseID, updateTestStep);
                    //try selecting the entity object again
                    tcTestStep = context.TestCase_TestSteps
                                    .FirstOrDefault((tcts) => ((tcts.fk_TestStepID == updateTestStep.ID) &&
                                                     (tcts.fk_TestCaseID == testCaseID))
                                    );
                }
                //save notes
                tcTestStep.Notes = updateTestStep.Notes;
                string retVal = this.updateTestStepText(updateTestStep);
                context.SaveChanges();
                //save business rule relationships
                removeAllTestStepBusinessRuleRelationshipsForTestCaseID(updateTestStep.ID, testCaseID);
                foreach (var br in updateTestStep.RelatedBusinessRules)
                {
                    addTestStep_BusinessRuleRelationship(updateTestStep.ID, br.ID);
                }
            }
        }

        /// <summary>
        /// adds test step/business rule relationships to db from geneeric list
        /// </summary>
        /// <param name="testStepID">int</param>
        /// <param name="brList">List&lt;viewBusinessRule&gt;</param>
        public void updateTestStepBusinessRuleRelationships(int testStepID, List<viewBusinessRule> brList)
        {
            foreach (var br in brList)
            {
                this.addTestStep_BusinessRuleRelationship(testStepID, br.ID);
            }
        }
                
        /// <summary>
        /// resequence test steps in the database
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <param name="sequence">List&lt;int&gt;</param>
        public void resequenceTestSteps(int testCaseID, List<int> sequence)
        {
            int seqCounter = 0;
            foreach (var testStepID in sequence)
            {
                seqCounter++;
                updateSeqNumForTestStep(testCaseID, testStepID, seqCounter);
            }
        }

        /// <summary>
        /// handles events when test steps must be re-sequenced
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <param name="testStepID">int</param>
        /// <param name="seqNum">int</param>
        public void updateSeqNumForTestStep(int testCaseID, int testStepID, int seqNum)
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var testStep = context.TestCase_TestSteps
                                    .FirstOrDefault((tcts) => ((tcts.fk_TestStepID == testStepID) &&
                                                                tcts.fk_TestCaseID == testCaseID));
                    testStep.SeqNum = seqNum;
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.currentProjectID, 0);
                }
            }
        }

        /// <summary>
        /// handles events when a test step is updated
        /// </summary>
        /// <param name="ts">viewTestStep</param>
        /// <returns>string</returns>
        public string updateTestStepText(viewTestStep ts)
        {
            string retVal = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var testStep = context.TestSteps
                                    .FirstOrDefault((tstep) => (tstep.ID == ts.ID));
                    testStep.Text = ts.Name;
                    context.SaveChanges();
                    retVal = "OK";
                }
                catch (Exception ex)
                {
                    retVal = palinoiaDAL.logError(ex, this.currentProjectID, ts.UpdatedBy);
                }
            }
            return retVal;
        }
                
        /// <summary>
        /// fetch test steps for a particular test case from the database
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <returns>List&lt;TestStep&gt;</returns>
        public List<viewTestStep> getTestStepsForTestCase(int testCaseID)
        {
            var testStepList = new List<viewTestStep>();
            
            //get all test steps associated with test case
            using (var context = palinoiaDAL.getContextForProject())
            {
                var testCaseTestSteps = from tcts in context.TestCase_TestSteps
                                        where tcts.fk_TestCaseID == testCaseID
                                        orderby tcts.SeqNum 
                                        select tcts;
                foreach (var testCaseTestStep in testCaseTestSteps)
                {
                    viewTestStep vTestStep = this.getTestStepByID((int)testCaseTestStep.fk_TestStepID);
                    vTestStep.SeqNum = (int)testCaseTestStep.SeqNum;
                    vTestStep.Notes = testCaseTestStep.Notes;
                    //get seq number and notes (per testcase id)
                    //var tcSpecificTestSteps = from tcts in context.TestCase_TestSteps
                    //                         where tcts.fk_TestCaseID == testCaseID &&
                    //                           tcts.fk_TestStepID == refTestStep.ID
                    //                     select tcts;
                    ////update reference test step
                    //foreach (var ts in tcSpecificTestSteps)
                    //{
                    //    refTestStep.SeqNum = (int)ts.SeqNum;
                    //    refTestStep.Notes = ts.Notes;
                    //}
                    //var vTestStep = new viewTestStep((int)refTestStep.ID, refTestStep.Text, (int)testCaseTestStep.SeqNum, refTestStep.RelatedBusinessRules, refTestStep.Notes, (bool)refTestStep.Active);
                    testStepList.Add(vTestStep);
                }
                //create generic list of entity objects
                //testStepList = ;// testSteps.ToList<TestStep>();
            }
            return testStepList;
        }

        /// <summary>
        /// add a test step to a particular test case in the database
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <param name="testStep">viewTestStep</param>
        private void addTestCaseTestStepRelationship(int testCaseID, viewTestStep testStep)
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    testStep.SeqNum = this.getNextTestStepSequenceNumber(testCaseID);
                    TestCase_TestSteps newRelationship = TestCase_TestSteps.CreateTestCase_TestSteps(
                                                            0, 
                                                            (long) testStep.SeqNum,
                                                            (long)testCaseID, 
                                                            (long)testStep.ID
                                                            );
                    //save notes
                    newRelationship.Notes = testStep.Notes;
                    context.TestCase_TestSteps.AddObject(newRelationship);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.currentProjectID, 0);
                }
            }
        }

        /// <summary>
        /// handles events when a test step - business rule relationship is added to the database
        /// </summary>
        /// <param name="testStepID">int</param>
        /// <param name="businessRuleID">int</param>
        /// <returns>string</returns>
        public string addTestStep_BusinessRuleRelationship(int testStepID, int businessRuleID)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    TestStep_BusinessRules newRelationship = TestStep_BusinessRules.CreateTestStep_BusinessRules(
                                                                0,
                                                                (long)testStepID,
                                                                (long)businessRuleID
                                                                );
                    context.TestStep_BusinessRules.AddObject(newRelationship);
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.currentProjectID, 0);
                }
            }
            return result;
        }

        /// <summary>
        /// fetches the next test step sequence number by test case ID
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <returns>int</returns>
        public int getNextTestStepSequenceNumber(int testCaseID)
        {
            int nextSeqNum = 0;

            //get all test steps associated with test case
            using (var context = palinoiaDAL.getContextForProject())
            {
                var testSteps = context.TestCase_TestSteps
                               .Where((tcts) => tcts.fk_TestCaseID == testCaseID)
                               .OrderBy ((ts) => ts.SeqNum);
                nextSeqNum = testSteps.Count();
            }
            nextSeqNum = nextSeqNum + 1; //add one due to zero based list;
            return nextSeqNum; 
        }

        /// <summary>
        /// return partent test case for test step id
        /// </summary>
        /// <param name="testStepID">int</param>
        /// <returns>viewTestCase</returns>
        public viewTestCase getTestCaseForTestStep(int testStepID)
        {
            var testCase = new viewTestCase(0,"", 0, true, 0, 0);
            using (var context = palinoiaDAL.getContextForProject())
            {
                
                var testCaseTestStep = from tcts in context.TestCase_TestSteps
                                       where tcts.fk_TestStepID == testStepID
                                       select tcts;
                foreach(var record in testCaseTestStep) {
                    var entityTestCase = this.getTestCaseByID((int)record.fk_TestCaseID);
                    testCase.ID = (int)entityTestCase.ID;
                    testCase.Name = entityTestCase.Name;
                    testCase.SectionID = (int)entityTestCase.fk_lkup_SectionID;
                }
            }
            return testCase;
        }

        public string getRelatedBusinessRulesForTestStepName(string testStepName)
        {
            string relatedBusinessRules = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    //get all test steps with same name
                    var entityTestSteps = context.TestSteps
                                            .Where((s) => s.Active == true && s.Text.Equals(testStepName))
                                            .OrderBy((ts) => ts.Text);
                    List<TestStep> tsList = entityTestSteps.ToList<TestStep>();
                    //spin though list
                    foreach (var step in tsList)
                    {
                        //are there related business rules
                        if (step.TestStep_BusinessRules.Count > 0)
                        {
                            //spin through related business rules
                            foreach (var rule in step.TestStep_BusinessRules)
                            {
                                //add related business rules to list if it is not already there
                                
                                relatedBusinessRules += rule.fk_BusinessRuleID;
                                relatedBusinessRules += ",";
                            }
                        }
                    }
                    //remove last comma, if present
                    if (relatedBusinessRules.Length > 0)
                    {
                        relatedBusinessRules = relatedBusinessRules.Substring(0, relatedBusinessRules.Length - 1);
                    }
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.currentProjectID, 0);
                }
                return relatedBusinessRules;
            }


            return relatedBusinessRules;
        }

        #endregion test steps

        #region PreConditions

        /// <summary>
        /// save preconditions for test case
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <param name="preConditionTestCaseIDs">List&lt;int&gt;</param>
        /// <returns>string</returns>
        public string savePreConditionsForTestCase(int testCaseID, List<int> preConditionTestCaseIDs)
        {
            string result = "";
            try
            {
                //remove all test case precondition relationship before saving new list
                removeAllPreConditionTestCases(testCaseID);
                if (preConditionTestCaseIDs.Count > 0)
                {
                    int sequenceCounter = 0;
                    foreach (var tcID in preConditionTestCaseIDs)
                    {
                        addPreConditionTestCaseRelationship(testCaseID, tcID, sequenceCounter);
                        sequenceCounter++;
                    }
                }
                result = "OK";
            }
            catch (Exception ex)
            {
                result = palinoiaDAL.logError(ex, this.currentProjectID, 0);
            }
            return result;
        }

        /// <summary>
        /// fetch preconditions for tese case
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <returns>List&lt;PreCondition&gt;</returns>
        public List<TestCase> getPreConditionsForTestCase(int testCaseID)
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                List<TestCase> pctcList = new List<TestCase>();
                try
                {
                    var pcTestCases = from tcpc in context.TestCase_PreConditions
                                      where tcpc.fk_TestCaseID == testCaseID
                                      orderby tcpc.SeqNum
                                      select tcpc;

                    foreach (var pc in pcTestCases)
                    {
                        var tc = this.getTestCaseByID((int)pc.PreConditionTestCaseID);
                        pctcList.Add(tc);
                    }
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.currentProjectID, 0);
                }
                return pctcList;
            }
        }
                
        /// <summary>
        /// add precondition test case relationship
        /// </summary>
        /// <param name="tcID">int</param>
        /// <param name="pctcID">int</param>
        /// <param name="seqnum">int</param>
        /// <returns>string</returns>
        private void addPreConditionTestCaseRelationship(int tcID, int pctcID, int seqnum)
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                TestCase_PreConditions tcpc = TestCase_PreConditions.CreateTestCase_PreConditions(0, 
                                                                        (long)tcID, 
                                                                        (long)pctcID, 
                                                                        (long)seqnum);
                context.TestCase_PreConditions.AddObject(tcpc);
                context.SaveChanges();
                
            }
        }
                
        /// <summary>
        /// remove precondition test case relationship
        /// </summary>
        /// <param name="tcID">int</param>
        /// <returns>string</returns>
        public string removeAllPreConditionTestCases(int tcID)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var pcTestCases = context.TestCase_PreConditions
                                        .Where((tcpc) => tcpc.fk_TestCaseID == tcID);
                    foreach (var pctc in pcTestCases)
                    {
                        context.TestCase_PreConditions.DeleteObject(pctc);
                        context.SaveChanges();
                    }
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, currentProjectID, 0);
                }
            }
            return result;
        }

        #endregion PreConditions

        #region test runner

        /// <summary>
        /// Saves test result record to db
        /// </summary>
        /// <param name="tr">viewTestResult</param>
        /// <returns>string</returns>
        public string saveTestResult(viewTestResult tr)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    TestResult entityTR = TestResult.CreateTestResult(0,
                                                                      (long)tr.CurrentTestCaseID,
                                                                      (long)tr.TestStepID,
                                                                      (long)tr.TestStatusID,
                                                                      (long)tr.UserID,
                                                                      tr.TestDate);
                    entityTR.Notes = tr.Notes;
                    entityTR.FailedBusinessRuleID = tr.FailedBusinessRuleID;
                    context.TestResults.AddObject(entityTR);
                    context.SaveChanges();
                    //update test case to fail
                    //this.updateTestCase(
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.currentProjectID, tr.UserID);
                }
            }

            return result;
        }

        /// <summary>
        /// gets latest(by date desc) test result for specific test case/test step
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <param name="testStepID">int</param>
        /// <returns>viewTestResult</returns>
        public viewTestResult getLatestTestResult(int testCaseID, int testStepID)
        {
            viewTestResult tr = null;
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var entityTRList = from testResults in context.TestResults
                                       where testResults.fk_TestCaseID == testCaseID &&
                                             testResults.fk_TestStepID == testStepID 
                                       orderby testResults.TestDate descending
                                       select testResults;
                    if (entityTRList != null)
                    {
                        List<TestResult> list = entityTRList.ToList<TestResult>();
                        if (list.Count > 0)
                        {
                            var latestResult = list[0];
                            tr = new viewTestResult((int)latestResult.ID,
                                                    (int)latestResult.fk_TestCaseID,
                                                    (int)latestResult.fk_TestCaseID,
                                                    (int)latestResult.fk_TestStepID,
                                                    (int)latestResult.fk_TestStatusID,
                                                    (int)latestResult.UserID,
                                                    latestResult.TestDate,
                                                    latestResult.Notes,
                                                    (int)latestResult.FailedBusinessRuleID);
                        }
                        else
                        {
                            //no result found return object with teststatus = untested
                            tr = new viewTestResult(0,
                                                    0,
                                                    0,
                                                    0,
                                                    (int)Enums.testStatusEnums.TestStatus.Untested,
                                                    0,
                                                    DateTime.Now,
                                                    "",
                                                    0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.currentProjectID, 0);
                }
            }
            return tr;
        }

        #endregion test runner

        #endregion instance methods
    }
}
