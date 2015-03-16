using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{    
    /// <summary>
    /// class to hold code for viewTestStep object
    /// </summary>
    public class viewTestStep
    {
        /// <summary>
        /// class variable to store test step ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable to store test step sequence number
        /// </summary>
        public int SeqNum { get; set; }
        /// <summary>
        /// class variable to store test step name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// class variable to store list of business rules related to the test step 
        /// </summary>
        public List<viewBusinessRule> RelatedBusinessRules { get; set; }
        /// <summary>
        /// class variable to store test step notes
        /// </summary>
        public string Notes { get; set; }
        /// <summary>
        /// class variable to store test step active status
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// class variable to store key to person updating test step 
        /// </summary>
        public int UpdatedBy { get; set; }
        /// <summary>
        /// class variable to store parent test case ID
        /// </summary>
        public int ParentTestCaseID { get; set; }
                
        /// <summary>
        /// constructor for viewTestStep object
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="seqnum">int</param>
        /// <param name="name">string</param>
        /// <param name="relatedBusinessRules">string</param>
        /// <param name="notes">string</param>
        /// <param name="active">bool</param>
        /// <param name="updatedBy">int</param>
        public viewTestStep(int id, 
                            string name, 
                            int seqnum, 
                            List<viewBusinessRule> relatedBusinessRules, 
                            string notes, 
                            bool active, 
                            int updatedBy)
        {
            this.ID = id;
            this.SeqNum = seqnum;
            this.Name = name;
            this.RelatedBusinessRules = relatedBusinessRules;
            this.Notes = notes;
            this.Active = active;
            this.UpdatedBy = updatedBy;
        }

        /// <summary>
        /// constructor for viewTestStep object
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="name">string</param>
        /// <param name="relatedBusinessRules">List&lt;viewBusinessRule&gt;</param>
        /// <param name="notes"></param>
        /// <param name=" updatedBy">int</param>
        public viewTestStep(int id, 
                            string name, 
                            List<viewBusinessRule> relatedBusinessRules, 
                            string notes,
                            int updatedBy)
        {
            this.ID = id;
            this.Name = name;
            this.RelatedBusinessRules = relatedBusinessRules;
            this.Notes = notes;
            this.UpdatedBy = updatedBy;
        }
                
        /// <summary>
        /// constructor for viewTestStep object
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="name">string</param>
        /// <param name="active">bool</param>
        /// <param name="updatedBy">int</param>
        public viewTestStep(int id, string name, bool active, int updatedBy)
        {
            this.ID = id;
            this.Name = name;
            this.Active = active;
            this.UpdatedBy = updatedBy;
        }

        /// <summary>
        /// empty constructor for viewTestStep object
        /// </summary>
        public viewTestStep()
        {

        }

    }
}
