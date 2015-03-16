using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{    
    /// <summary>
    /// class to hold the code for the viewCSMResponseType object
    /// </summary>
    public class viewCSMResponseType
    {
       
        #region Properties and Variables

        /// <summary>
        /// class variable to store CSM response type ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable to store CSM response type text
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// class variable to store CSM response type active status
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// class variable to store key to person updating CSM response type 
        /// </summary>
        public int UpdatedBy { get; set; }

        #endregion Properties and Variables

        #region Constructors
                
        /// <summary>
        /// constructor for viewCSMResponseType Object
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="text">string</param>
        /// <param name="active">bool</param>
        /// <param name="updatedBy">int</param>
        public viewCSMResponseType(int id, string text, bool active, int updatedBy)
        {
            this.ID = id;
            this.Text = text;
            this.Active = active;
            this.UpdatedBy = updatedBy;
        }

        #endregion Constructors
    }
}
