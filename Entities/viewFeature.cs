using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
        
    /// <summary>
    /// class to hold code for viewFeature object
    /// </summary>
    public class viewFeature
    {
        /// <summary>
        /// class variable to store feature ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable to store feature text
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// class variable to store feature active status
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// class variable to store UserID as UpdatedBy
        /// </summary>
        public int UpdatedBy { get; set; }

        /// <summary>
        /// constructor for viewFeature object with three parameters
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="text">string</param>
        /// <param name="active">bool</param>
        /// <param name="updatedBy">int</param>
        public viewFeature(int id, string text, bool active, int updatedBy)
        {
            this.ID = id;
            this.Text = text;
            this.Active = active;
            this.UpdatedBy = updatedBy;
        }
    }
}
