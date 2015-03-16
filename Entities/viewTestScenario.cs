using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{    
    /// <summary>
    /// class to hold code for viewTestScenario object
    /// </summary>
    public class viewTestScenario
    {
        /// <summary>
        /// class variable to store test scenario ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable to store test scenario name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// class variable to store test scenario notes
        /// </summary>
        public string Notes { get; set; }
        /// <summary>
        /// class variable to store key to person updating test scenario
        /// </summary>
        public int UpdatedBy { get; set; }
                
        /// <summary>
        /// constructor for viewTestScenario object
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="name">string</param>
        /// <param name="notes">string</param>
        /// <param name="updatedBy">int</param>
        public viewTestScenario(int id, string name, string notes, int updatedBy)
        {
            this.ID = id;
            this.Name = name;
            this.Notes = notes;
            this.UpdatedBy = updatedBy;
        }
    }
}
