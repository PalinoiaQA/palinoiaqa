using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{    
    /// <summary>
    /// class to hold code for viewCSMType object
    /// </summary>
    public class viewCSMType
    {
        /// <summary>
        /// class variable to store CSM type ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable to store CSM type text
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// class variable to store CSM type active status
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// class variable to store key to person updating CSM type 
        /// </summary>
        public int UpdatedBy { get; set; }
                
        /// <summary>
        /// constructor for viewCSMType object
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="text">string</param>
        /// <param name="active">bool</param>
        /// <param name="updatedBy">int</param>
        public viewCSMType(int id, string text, bool active, int updatedBy)
        {
            this.ID = id;
            this.Text = text;
            this.Active = active;
            this.UpdatedBy = updatedBy;
        }
    }
}
