using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using Entities;
using Enums;

namespace BLL
{    
    /// <summary>
    /// class to hold code for TestCasesBLL object
    /// </summary>
    public class TestCasesBLL
    {
        int currentProjectID;
        TestCasesDAL dal;

        #region constructors
        
        /// <summary>
        /// constructor with one parameter
        /// </summary>
        /// <param name="projectID">int</param>
        public TestCasesBLL(int projectID)
        {
            if (projectID == 0)
            {
                //set to default project
                projectID = 1;
            }
            this.currentProjectID = projectID;
            dal = new TestCasesDAL(projectID);
        }

        #endregion constructors

        #region test cases

        /// <summary>
        /// has test cases
        /// </summary>
        /// <param name="sectionID">int</param>
        /// <returns>bool</returns>
        public bool hasTestCases(int sectionID)
        {
            return dal.hasTestCases(sectionID);
        }
               
        /// <summary>
        /// fetch all test cases from the database
        /// </summary>
        /// <returns>List&lt;viewTestCase&gt;</returns>
        public List<viewTestCase> getAllTestCases()
        {
            var testCaseList = new List<viewTestCase>();
            var entityTestCaseList = dal.getAllTestCases();
            foreach (var entityTestCase in entityTestCaseList)
            {
                viewTestCase tc = new viewTestCase((int)entityTestCase.ID, 
                                                    entityTestCase.Name, 
                                                    (int)entityTestCase.fk_lkup_SectionID,
                                                    entityTestCase.Active,
                                                    (int)entityTestCase.UpdatedBy,
                                                    (int)entityTestCase.fk_TestStatusID);
                testCaseList.Add(tc);
            }
            return testCaseList;
        }
                
        /// <summary>
        /// fetch a test case from the database by ID
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <returns>viewTestCase</returns>
        public viewTestCase getTestCaseByID(int testCaseID)
        {
            var entityTestCase = dal.getTestCaseByID(testCaseID);
            viewTestCase tc = new viewTestCase((int)entityTestCase.ID, 
                                                entityTestCase.Name, 
                                                (int)entityTestCase.fk_lkup_SectionID,
                                                entityTestCase.Active,
                                                (int)entityTestCase.UpdatedBy,
                                                (int)entityTestCase.fk_TestStatusID);
            return tc;
        }
                
        /// <summary>
        /// add a test case to the database
        /// </summary>
        /// <param name="newTestCase">viewTestCase</param>
        /// <returns>string</returns>
        public string addNewTestCase(viewTestCase newTestCase)
        {
            var newTestCaseID = dal.addNewTestCase(newTestCase);
            //create test case document
            if (newTestCaseID.IndexOf("ID") != -1)
            {
                string TCID = newTestCaseID.Substring(3);
                int newTCID = 0;
                var result = int.TryParse(TCID, out newTCID);
                viewDocument newTCDoc = new viewDocument(0, 
                                                         3, 
                                                         newTestCase.Name, 
                                                         newTestCase.Name + " Test Case Document", 
                                                         true,
                                                         newTestCase.UpdatedBy);
                var docBLL = new DocumentsBLL(this.currentProjectID);
                docBLL = null;
            }
            return newTestCaseID;
        }

        /// <summary>
        /// create list of associated business rules for a test case by compiling a list of 
        /// business rule associations for each of the test steps in the test case.
        /// </summary>
        /// <param name="testCaseID">int</param>
        public void createAssociatedBusinessRulesForTestCase(int testCaseID)
        {
            List<viewTestStep> tsList = new List<viewTestStep>();
            List<viewBusinessRule> brList = new List<viewBusinessRule>();
            tsList = dal.getTestStepsForTestCase(testCaseID);
            foreach (var ts in tsList)
            {
                foreach(var rule in ts.RelatedBusinessRules) {
                    if (!brList.Contains(rule))
                    {
                        brList.Add(rule);
                    }
                }
            }
            string result = dal.createTestCaseBusinessRuleRelationships(testCaseID, brList);
        }
                
        /// <summary>
        /// delete a test case from the database 
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteTestCase(int deleteID, int userID)
        {
            return dal.deleteTestCase(deleteID, userID);
        }
                
        /// <summary>
        /// update a test case in the database
        /// </summary>
        /// <param name="testCase">viewTestCase</param>
        /// <returns>string</returns>
        public string updateTestCase(viewTestCase testCase)
        {
            return dal.updateTestCase(testCase);
        }
                
        /// <summary>
        /// get all test cases by section
        /// </summary>
        /// <param name="sectionID">int</param>
        /// <returns>List&lt;viewTestCase&gt;</returns>
        public List<viewTestCase> getAllTestCasesBySection(int sectionID)
        {
            return dal.getAllTestCasesBySection(sectionID);
        }
                
        /// <summary>
        /// get business rules for test case
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <returns>List&lt;viewBusinessRule&gt;</returns>
        public List<viewBusinessRule> getBusinessRulesForTestCase(int testCaseID)
        {
            return dal.getBusinessRulesForTestCase(testCaseID);
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
            return dal.updateTestCaseStatus(testCaseID, testStatusID, userID);
        }

        /// <summary>
        /// reset test case status
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string resetTestCaseStatus(int testCaseID, int userID)
        {
            var tr = new viewTestResult();
            tr.CurrentTestCaseID = testCaseID;
            applicationBLL appBLL = new applicationBLL();
            var user = appBLL.getUserByID(userID);
            tr.Notes = "TestCase reset to Untested by " + user.getFullNameFNF();
            tr.PrimaryTestCaseID = testCaseID;
            tr.TestDate = DateTime.Now;
            tr.TestStatusID = (int)Enums.testStatusEnums.TestStatus.Untested;
            tr.UserID = user.ID;
            var tsList = this.getTestStepsForTestCase(testCaseID);
            foreach (var ts in tsList)
            {
                tr.TestStepID = ts.ID;
                this.saveTestResult(tr);
            }
            return this.updateTestCaseStatus(testCaseID, (int)Enums.testStatusEnums.TestStatus.Untested, userID);
        }

        /// <summary>
        /// returns list of business rules that are not associated with any test cases
        /// </summary>
        /// <returns></returns>
        public List<viewBusinessRule> getBusinessRulesWithNoTestCase()
        {
            var brBLL = new BusinessRulesBLL(this.currentProjectID);
            var orphanedBusinessRules = brBLL.getAllBusinessRules();
            var tcList = this.getAllTestCases();
            //spin through all test cases
            foreach (var tc in tcList)
            {
                //remove associated business rules for each test case from orphaned list
                var tcBRList = tc.associatedRules;
                var result = tcBRList.Where(b => !orphanedBusinessRules.Any(b2 => b2.Name.Equals(b.Name)));
                orphanedBusinessRules = result.ToList();
            }
            return orphanedBusinessRules;
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
            return dal.hasTestSteps(testCaseID);
        }

        /// <summary>
        /// fetches generic list of viewTestStep records from db where Active = true
        /// </summary>
        /// <returns>List&lt;viewTestStep&gt;</returns>
        public List<viewTestStep> getActiveTestSteps()
        {
            List<viewTestStep> tsList = new List<viewTestStep>();
            var entityTestSteps = dal.getActiveTestSteps();
            foreach (var entityTestStep in entityTestSteps)
            {
                tsList.Add(new viewTestStep((int)entityTestStep.ID, entityTestStep.Text, entityTestStep.Active, (int)entityTestStep.UpdatedBy));

            }
            return tsList;
        }
                
        /// <summary>
        /// fetch a test step from the database by ID
        /// </summary>
        /// <param name="testStepID">int</param>
        /// <returns>viewTestStep</returns>
        public viewTestStep getTestStepByID(int testStepID)
        {
            return dal.getTestStepByID(testStepID);
        }
                
        /// <summary>
        /// fetch a test step from the database by ID
        /// </summary>
        /// <param name="testStepID">int</param>
        /// <param name="testCaseID">int</param>
        /// <returns>viewTestStep</returns>
        public viewTestStep getTestStepByID(int testStepID, int testCaseID)
        {
            return dal.getTestStepByID(testStepID, testCaseID);
        }

        /// <summary>
        /// Adds new test step to database including relationship  to test case id paramater
        /// </summary>
        /// <param name="newTestStep">viewTestStep</param>
        /// <param name="testCaseID">int</param>
        /// <returns>string</returns>
        public string addTestStep(viewTestStep newTestStep, int testCaseID)
        {
            return dal.addNewTestStep(newTestStep, testCaseID); 
        }
               
        /// <summary>
        /// fetch the test steps for a particular test case from the database
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <returns>List&lt;viewTestStep&gt;</returns>
        public List<viewTestStep> getTestStepsForTestCase(int testCaseID)
        {
            var testStepList = dal.getTestStepsForTestCase(testCaseID);
            return testStepList;
        }

        /// <summary>
        /// remove relationship link in db between test step and test case
        /// </summary>
        /// <param name="testStepID">int</param>
        /// <param name="testCaseID">int</param>
        /// <returns>string</returns>
        public string removeTestStepTestCaseRelationship(int testStepID, int testCaseID)
        {
            string retVal = dal.removeTestStepTestCaseRelationship(testStepID, testCaseID);
            return retVal;
        }

        /// <summary>
        /// remove relationship link in db between test step and business rule
        /// </summary>
        /// <param name="testStepID">int</param>
        /// <param name="businessRuleID">int</param>
        /// <returns>string</returns>
        public string removeTestStepBusinessRuleRelationship(int testStepID, int businessRuleID)
        {
            string retVal = dal.removeTestStepBusinessRuleRelationship(testStepID, businessRuleID);
            var ts = dal.getTestStepByID(testStepID);
            this.createAssociatedBusinessRulesForTestCase(ts.ParentTestCaseID);
            return retVal;
        }

        /// <summary>
        /// create relationship link in db betweek test step and business rule
        /// </summary>
        /// <param name="testStepID">int</param>
        /// <param name="businessRuleID">int</param>
        /// <returns>string</returns>
        public string addTestStepBusinessRuleRelationship(int testStepID, int businessRuleID)
        {
            string retVal = dal.addTestStep_BusinessRuleRelationship(testStepID, businessRuleID);
            return retVal;
        }

        /// <summary>
        /// remove all test step business rule relationships for test case
        /// </summary>
        /// <param name="testStepID">int</param>
        /// <param name="testCaseID">int</param>
        public void removeAllTestStepBusinessRuleRelationshipsForTestCaseID(int testStepID, int testCaseID)
        {
            dal.removeAllTestStepBusinessRuleRelationshipsForTestCaseID(testStepID, testCaseID);
        }

        /// <summary>
        /// update test step for test case
        /// </summary>
        /// <param name="updateTestStep">viewTestStep </param>
        /// <param name="testCaseID">int</param>
        public void updateTestStepForTestCase(viewTestStep updateTestStep, int testCaseID)
        {
            dal.updateTestStepForTestCase(updateTestStep, testCaseID);
            this.createAssociatedBusinessRulesForTestCase(testCaseID);
        }

        /// <summary>
        /// update test step business rule relationship
        /// </summary>
        /// <param name="testStepID">int</param>
        /// <param name="brList">List&lt;viewBusinessRule&gt;</param>
        public void updateTestStepBusinessRuleRelationships(int testStepID, List<viewBusinessRule> brList)
        {
            dal.updateTestStepBusinessRuleRelationships(testStepID, brList);
        }

        /// <summary>
        /// resequence test steps
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <param name="sequence">List&lt;int&gt;</param>
        public void resequenceTestSteps(int testCaseID, List<int> sequence)
        {
            dal.resequenceTestSteps(testCaseID, sequence);
        }

        /// <summary>
        /// update sequence number for test step 
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <param name="testStepID">int</param>
        /// <param name="seqNum">int</param>
        public void updateSeqNumForTestStep(int testCaseID, int testStepID, int seqNum)
        {
            dal.updateSeqNumForTestStep(testCaseID, testStepID, seqNum);

        }

        /// <summary>
        /// calls DAL method to get comma delimeted list of business rule ids related to other test steps 
        /// with the same name
        /// </summary>
        /// <param name="testName">string</param>
        /// <returns>string</returns>
        public string getRelatedBusinessRulesForTestStepName(string testName)
        {
            string relatedBusinessRules =  this.dal.getRelatedBusinessRulesForTestStepName(testName);
            List<string> noDupes = new List<string>();
            //remove duplicates
            var arrBRList = relatedBusinessRules.Split(',');
            foreach (var rule in arrBRList)
            {
                var unique = true;
                foreach (var record in noDupes)
                {
                    if (rule.Equals(record))
                    {
                        unique = false;
                    }
                }
                if (unique)
                {
                    noDupes.Add(rule);
                }
            }
            //convert noDupes object to comma delimeted string
            var counter = 0;
            relatedBusinessRules = "";
            foreach (var record in noDupes)
            {
                relatedBusinessRules += record;
                //add comma if not last record
                counter++;
                if (counter < noDupes.Count)
                {
                    relatedBusinessRules += ",";
                }
                
            }
            return relatedBusinessRules;
        }
        
        #endregion test steps

        #region pre-conditions
                
        /// <summary>
        /// fetch preconditions for test case
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <returns>List&lt;viewPreCondition&gt;</returns>
        public List<viewTestCase> getPreConditionsForTestCase(int testCaseID)
        {
            List<viewTestCase> pcList = new List<viewTestCase>();
            var entityPCList = dal.getPreConditionsForTestCase(testCaseID);
            foreach (var eTC in entityPCList)
            {
                pcList.Add(new viewTestCase((int)eTC.ID,
                                                eTC.Name.ToString(),
                                                (int)eTC.fk_lkup_SectionID,
                                                eTC.Active,
                                                (int)eTC.UpdatedBy,
                                                (int)eTC.fk_TestStatusID));
            }
            return pcList;
        }

        /// <summary>
        /// save preconditions fot test case
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <param name="preConditionIDs">string</param>
        /// <returns>string</returns>
        public string savePreConditionsForTestCase(int testCaseID, string preConditionIDs)
        {
            string result = "";
            //convert comma delimeted string to int array
            List<int> preConIDList = new List<int>();
            if (preConditionIDs.Length > 0)
            {
                var arrPreConditions = preConditionIDs.Split(',');
                for (int i = 0; i < arrPreConditions.Length; i++)
                {
                    string strPCID = arrPreConditions[i];
                    int pcID = 0;
                    bool parseResult = int.TryParse(strPCID, out pcID);
                    if (parseResult && pcID > 0)
                    {
                        preConIDList.Add(pcID);
                    }
                }
                if (preConIDList.Count > 0)
                {
                    result = dal.savePreConditionsForTestCase(testCaseID, preConIDList);
                }
            }
            return result;
        }

        /// <summary>
        /// remove all precondition test cases
        /// </summary>
        /// <param name="tcID">int</param>
        /// <returns>string</returns>
        public string removeAllPreConditionTestCases(int tcID)
        {
            return dal.removeAllPreConditionTestCases(tcID);
        }
        
        #endregion pre-conditions

        #region test runner

        /// <summary>
        /// Retrieve list of test cases for testing.  return list includes
        /// all preconditions ordered by sequence number.
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <returns>List&lt;vieTestCase&gt;</returns>
        public List<viewTestCase> getTestCaseListForTesting(int testCaseID)
        {
            List<viewTestCase> testList = new List<viewTestCase>();
            var entityPrimaryTC = dal.getTestCaseByID(testCaseID);
            //get all precondition test cases first
            var preConditions = dal.getPreConditionsForTestCase(testCaseID);
            foreach (var pc in preConditions)
            {
                var entitytc = dal.getTestCaseByID((int)pc.ID);
                viewTestCase tc = new viewTestCase((int)entitytc.ID,
                                                   entitytc.Name,
                                                   (int)entitytc.fk_lkup_SectionID,
                                                   entitytc.Active,
                                                   (int)entitytc.UpdatedBy,
                                                   (int)entitytc.fk_TestStatusID);
                testList.Add(tc);

            }
            //add primary test case last
            viewTestCase primaryTC = new viewTestCase((int)entityPrimaryTC.ID,
                                                      entityPrimaryTC.Name,
                                                      (int)entityPrimaryTC.fk_lkup_SectionID,
                                                      entityPrimaryTC.Active,
                                                      (int)entityPrimaryTC.UpdatedBy,
                                                      (int)entityPrimaryTC.fk_TestStatusID);
            testList.Add(primaryTC);
            return testList;
        }

        /// <summary>
        /// forward test result to dal to be saved to db
        /// </summary>
        /// <param name="tr">viewTestResult</param>
        /// <returns>string</returns>
        public string saveTestResult(viewTestResult tr)
        {
            string result = dal.saveTestResult(tr);
            if (result.Equals("OK"))
            {
                //if result = fail, update test case status to fail
                if (tr.TestStatusID == (int)Enums.testStatusEnums.TestStatus.Fail)
                {
                    var tc = this.getTestCaseByID(tr.CurrentTestCaseID);
                    tc.TestStatusID = (int)Enums.testStatusEnums.TestStatus.Fail;
                    this.updateTestCase(tc);
                    // TODO: add new defect for failed test step
                    var defectBLL = new DefectsBLL(this.currentProjectID);
                    var brule = new viewBusinessRule();
                    var failedTestStep = this.getTestStepByID(tr.TestStepID);
                    defectBLL.createNewDefectForTestStepFail(tr);
                }
                //get next test step for test runner
                var nextTS = this.getNextTestStepForTestRunner(tr);
                //if result = pass and this is the last test step of the last test case, update test case status to pass
                if (nextTS.last && (tr.TestStatusID == (int)Enums.testStatusEnums.TestStatus.Pass))
                {
                    //full test case passed, update test case
                    viewTestCase tc = this.getTestCaseByID(nextTS.CurrentTestCaseID);
                    tc.TestStatusID = (int)Enums.testStatusEnums.TestStatus.Pass;
                    this.updateTestCase(tc);
                    result = "FINALPASS";
                }
            }
            return result;
        }

        /// <summary>
        /// forwards request for latest test result (by date desc) to DAL 
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <param name="testStepID">int</param>
        /// <returns>viewTestResult</returns>
        public string getLatestTestResultString(int testCaseID, int testStepID)
        {
            string result = "";
            var testResult = dal.getLatestTestResult(testCaseID, testStepID);
            switch (testResult.TestStatusID)
            {
                case((int)testStatusEnums.TestStatus.Fail):
                    result = "FAIL";
                    break;
                case ((int)testStatusEnums.TestStatus.Pass):
                    result = "PASS";
                    break;
                case ((int)testStatusEnums.TestStatus.Untested):
                    result = "UNTESTED";
                    break;
            }
            return result;
        }

        /// <summary>
        /// fetch latest test result
        /// </summary>
        /// <param name="testCaseID">int</param>
        /// <param name="testStepID">int</param>
        /// <returns>viewTestResult</returns>
        public viewTestResult getLatestTestResult(int testCaseID, int testStepID)
        {
            return dal.getLatestTestResult(testCaseID, testStepID);
        }
        
        /// <summary>
        /// fetch next test step for test runner
        /// </summary>
        /// <param name="tr">TestRunnerTestStep</param>
        /// <returns>viewTestStep</returns>
        public viewTestResult getNextTestStepForTestRunner(viewTestResult tr)
        {
            viewTestResult retObj = new viewTestResult();
            //set primary test case id from test runner object because it won't change
            retObj.PrimaryTestCaseID = tr.PrimaryTestCaseID;
            //get parent test case object
            var currentTestCase = this.getTestCaseByID(tr.CurrentTestCaseID);
            //get list of test steps for parent test case
            var currentTestStepList = this.getTestStepsForTestCase(tr.CurrentTestCaseID);
            //identity current test step in list by currentTestStepID
            var listTestStep = currentTestStepList.Find((ts) => ts.ID == tr.TestStepID);
            //if current test step sequence number < total test steps, return next test step
            if (listTestStep.SeqNum < currentTestStepList.Count)
            {
                retObj.TestStepID = currentTestStepList.Find((ts) => ts.SeqNum == listTestStep.SeqNum + 1).ID;
                retObj.CurrentTestCaseID = tr.CurrentTestCaseID;
                retObj.PrimaryTestCaseID = tr.PrimaryTestCaseID;
            }
            //else (current test step == total test steps)
            else
            {
                //get list of test cases (preconditions + primary)
                var testCaseList = this.getTestCaseListForTesting(tr.PrimaryTestCaseID);
                for (int i = 0; i < testCaseList.Count; i++)
                {
                    if (testCaseList[i].ID == currentTestCase.ID)
                    {
                        if (i < testCaseList.Count - 1)
                        {
                            //identify next test case from by test step parent test case id
                            currentTestCase = testCaseList[i + 1];
                            retObj.CurrentTestCaseID = currentTestCase.ID;
                            var newTestStepList = this.getTestStepsForTestCase(currentTestCase.ID);
                            //return first test step from next test case only if test result == PASS or FAIL
                            var testResult = this.getLatestTestResult(currentTestCase.ID, newTestStepList[0].ID);
                            retObj.TestStepID = newTestStepList[0].ID;
                            break;
                        }
                        else
                        {
                            currentTestCase = testCaseList[i];
                            retObj.CurrentTestCaseID = currentTestCase.ID;
                            var newTestStepList = this.getTestStepsForTestCase(currentTestCase.ID);
                            //return last test step from last test case only if test result == PASS or FAIL
                            var testResult = this.getLatestTestResult(currentTestCase.ID, newTestStepList[newTestStepList.Count - 1].ID);
                            retObj.TestStepID = newTestStepList[newTestStepList.Count-1].ID;
                            break;
                        }
                        
                    }
                }
            }
            retObj.last = isLastTestStep(retObj);
            return retObj;
        }

        /// <summary>
        /// fetch previous test step for test runner
        /// </summary>
        /// <param name="tr">int</param>
        /// <returns>viewTestResult</returns>
        public viewTestResult getPreviousTestStepForTestRunner(viewTestResult tr)
        {
            viewTestResult retObj = new viewTestResult();
            //set primary test case id from test runner object because it won't change
            retObj.PrimaryTestCaseID = tr.PrimaryTestCaseID;
            //get parent test case object
            var currentTestCase = this.getTestCaseByID(tr.CurrentTestCaseID);
            //get list of test steps for parent test case
            var currentTestStepList = this.getTestStepsForTestCase(tr.CurrentTestCaseID);
            //identity current test step in list by currentTestStepID
            var listTestStep = currentTestStepList.Find((ts) => ts.ID == tr.TestStepID);
            //if current test step sequence number < total test steps, return next test step
            if (listTestStep.SeqNum > 1)
            {
                retObj.TestStepID = currentTestStepList.Find((ts) => ts.SeqNum == listTestStep.SeqNum - 1).ID;
                retObj.CurrentTestCaseID = tr.CurrentTestCaseID;
                retObj.PrimaryTestCaseID = tr.PrimaryTestCaseID;
            }
            //else (current test step == 1)
            else
            {
                //get list of test cases (preconditions + primary)
                var testCaseList = this.getTestCaseListForTesting(tr.PrimaryTestCaseID);
                for (int i = 0; i < testCaseList.Count; i++)
                {
                    if (testCaseList[i].ID == currentTestCase.ID)
                    {
                        if (i > 0)
                        {
                            //identify next test case from by test step parent test case id
                            currentTestCase = testCaseList[i - 1];
                            retObj.CurrentTestCaseID = currentTestCase.ID;
                            var newTestStepList = this.getTestStepsForTestCase(currentTestCase.ID);
                            //return first test step from next test case only if test result == PASS or FAIL
                            var testResult = this.getLatestTestResult(currentTestCase.ID, newTestStepList[newTestStepList.Count - 1].ID);
                            retObj.TestStepID = newTestStepList[newTestStepList.Count - 1].ID;
                            break;
                        }
                        else
                        {
                            currentTestCase = testCaseList[0];
                            retObj.CurrentTestCaseID = currentTestCase.ID;
                            var newTestStepList = this.getTestStepsForTestCase(currentTestCase.ID);
                            //return last test step from last test case only if test result == PASS or FAIL
                            var testResult = this.getLatestTestResult(currentTestCase.ID, newTestStepList[0].ID);
                            retObj.TestStepID = newTestStepList[0].ID;
                            break;
                        }

                    }
                }
            }
            retObj.first = isFirstTestStep(retObj);
            return retObj;
        }

        private bool isFirstTestStep(viewTestResult tr)
        {
            //is this the first test step of the first test case?
            bool firstTestStep = false;
            var ts = this.getTestStepByID(tr.TestStepID);
            var testCaseList = this.getTestCaseListForTesting(tr.PrimaryTestCaseID);
            var testStepList = this.getTestStepsForTestCase(tr.CurrentTestCaseID);
            if (tr.CurrentTestCaseID == testCaseList[0].ID &&
                ts.SeqNum == 1)
            {
                firstTestStep = true;
            }
            return firstTestStep;
        }

        private bool isLastTestStep(viewTestResult tr)
        {
            //is this the last test step of the last test case?
            bool lastTestStep = false;
            //var ts = this.getTestStepByID(tr.TestStepID);
            var tsList = this.getTestStepsForTestCase(tr.CurrentTestCaseID);
            var ts = tsList.Find((testStep) => testStep.ID == tr.TestStepID);
            var testCaseList = this.getTestCaseListForTesting(tr.PrimaryTestCaseID);
            var testStepList = this.getTestStepsForTestCase(tr.CurrentTestCaseID);
            //is this actually the last test step
            if (tr.CurrentTestCaseID == testCaseList[testCaseList.Count - 1].ID &&
                ts.SeqNum == (testStepList.Count))
            {
                lastTestStep = true;
            }
            //is the test result Fail or Untested?
            var testResult = this.getLatestTestResult(tr.CurrentTestCaseID, ts.ID);
            if (testResult.TestStatusID == (int)Enums.testStatusEnums.TestStatus.Fail ||
                testResult.TestStatusID == (int)Enums.testStatusEnums.TestStatus.Untested)
            {
                //yes, set last test step boolean to true so that user 
                //cannot Next past failed and untested test steps
                lastTestStep = true;
            }
            return lastTestStep;
        }

        #endregion test runner

    }
}
