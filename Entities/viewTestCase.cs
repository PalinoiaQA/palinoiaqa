using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{    
    /// <summary>
    /// class to hold code for viewTestCase object
    /// </summary>
    public class viewTestCase
    {
        /// <summary>
        /// class variable to store test case ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable to store test case name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// class variable to store test case section ID
        /// </summary>
        public int SectionID { get; set; }
        /// <summary>
        /// class variable to store key to person updating test case
        /// </summary>
        public int UpdatedBy { get; set; }
        /// <summary>
        /// class variable to store test case active status
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// class variable to store list of precondition IDs
        /// </summary>
        public List<int> PreConditionIDs { get; set; }
        /// <summary>
        /// ckass variable to store test status id
        /// </summary>
        public int TestStatusID { get; set; }
        /// <summary>
        /// class variable to store all business rules associated with all test steps in test case
        /// </summary>
        public List<viewBusinessRule> associatedRules { get; set; }
                
        /// <summary>
        /// constructor for viewTestCase object
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="name">string</param>
        /// <param name="sectionID">int</param>
        /// <param name="active">bool</param>
        /// <param name="updatedBy">int</param>
        /// <param name="testStatusID">int</param>
        public viewTestCase(int id, string name, int sectionID, bool active, int updatedBy, int testStatusID)
        {
            this.ID = id;
            this.Name = name;
            this.SectionID = sectionID;
            this.Active = active;
            this.UpdatedBy = updatedBy;
            this.TestStatusID = testStatusID;
        }


    }

    /// <summary>
    /// class to hold code for TestCaseSearchResult
    /// </summary>
    [Serializable]
    public class TestCaseSearchResult
    {
        /// <summary>
        /// class variable to store a list of test case IDs
        /// </summary>
        public List<string> tcID { get; set; }
        /// <summary>
        /// class variable to store a list of test step IDs
        /// </summary>
        public List<string> tsID { get; set; }

        /// <summary>
        /// empty constructor for TestCaseSearchResult
        /// </summary>
        public TestCaseSearchResult()
        {

        }


        /// <summary>
        /// constructor for TestCaseSearchResult with two parameters
        /// </summary>
        /// <param name="tcid">List&lt;string&gt;</param>
        /// <param name="tsid">List&lt;string&gt;</param>
        public TestCaseSearchResult(List<string> tcid, List<string> tsid)
        {
            this.tcID = tcid;
            this.tsID = tsid;
        }
    }
}
