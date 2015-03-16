using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    /// <summary>
    /// class to hold code for viewTestResult
    /// </summary>
    public class viewTestResult
    {
        /// <summary>
        /// class variable to store value of test result ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable to store value of primaryTestCaseID
        /// </summary>
        public int PrimaryTestCaseID { get; set; }
        /// <summary>
        /// class variable to store value of test case ID
        /// </summary>
        public int CurrentTestCaseID { get; set; }
        /// <summary>
        /// class variable to store value of test step ID
        /// </summary>
        public int TestStepID { get; set; }
        /// <summary>
        /// class variable to store value of test status ID
        /// </summary>
        public int TestStatusID { get; set; }
        /// <summary>
        /// class variable to store value of user ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// class variable to store value of date and time that test was run
        /// </summary>
        public DateTime TestDate { get; set; }
        /// <summary>
        /// class variable to store notes about test 
        /// </summary>
        public string Notes { get; set; }
        /// <summary>
        /// boolean var to indicate if object is first test step of the first test case being tested
        /// </summary>
        public bool first { get; set; }
        /// <summary>
        /// boolean var to indicate if object is last test step of the last test case being tested
        /// </summary>
        public bool last { get; set; }
        /// <summary>
        /// int var to store the specific related business rule involved with test failure
        /// </summary>
        public int FailedBusinessRuleID { get; set; }

        /// <summary>
        /// constructor for viewTestResult
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="primaryTestCaseID">int</param>
        /// <param name="currentTestCaseID">int</param>
        /// <param name="testStepID">int</param>
        /// <param name="testStatusID">int</param>
        /// <param name="userID">int</param>
        /// <param name="testDate">DateTime</param>
        /// <param name="notes">string</param>
        /// <param name="failedBusinessRuleID">int</param>
        public viewTestResult(int id,
                              int primaryTestCaseID,
                              int currentTestCaseID,
                              int testStepID,
                              int testStatusID,
                              int userID,
                              DateTime testDate,
                              string notes,
                              int failedBusinessRuleID)
        {
            this.ID = id;
            this.PrimaryTestCaseID = primaryTestCaseID;
            this.CurrentTestCaseID = currentTestCaseID;
            this.TestStepID = testStepID;
            this.TestStatusID = testStatusID;
            this.UserID = userID;
            this.TestDate = testDate;
            this.Notes = notes;
            this.FailedBusinessRuleID = failedBusinessRuleID;
            this.first = false;
            this.last = false;
        }

        /// <summary>
        /// empty constructor for viewTestResult
        /// </summary>
        public viewTestResult()
        {

        }
    }
}
