using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    /// <summary>
    /// classtoholdcode for TestRunnerTestStep object
    /// </summary>
    public class TestRunnerTestStep
    {
        /// <summary>
        /// class variable to store value of currentTestStep 
        /// </summary>
        public viewTestStep currentTestStep { get; set; }
        /// <summary>
        /// class variable to store value of primaryTestCaseID
        /// </summary>
        public int primaryTestCaseID { get; set; }
        /// <summary>
        /// class variable to store value of currentTestCaseID
        /// </summary>
        public int currentTestCaseID { get; set; }

        /// <summary>
        /// boolean var to indicate if object is first test step of the first test case being tested
        /// </summary>
        public bool first { get; set; }

        /// <summary>
        /// boolean var to indicate if object is last test step of the last test case being tested
        /// </summary>
        public bool last { get; set; }

        /// <summary>
        /// empty constructor for TestRunnerTestStep
        /// </summary>
        public TestRunnerTestStep()
        {
            this.first = false;
            this.last = false;
        }
    }
}
