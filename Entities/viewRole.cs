using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{    
    /// <summary>
    /// class to hold code for viewRole object
    /// </summary>
    public class viewRole
    {
        /// <summary>
        /// class variable to store role ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable to store role text
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// class variable to store role active status
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// class variable to store UpdatedBy user id
        /// </summary>
        public int UpdatedBy { get; set; }
                
        /// <summary>
        /// constructor for viewRole object
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="text">string</param>
        /// <param name="active">bool</param>
        /// <param name="updatedBy">int</param>
        public viewRole(int id, string text, bool active, int updatedBy)
        {
            this.ID = id;
            this.Text = text;
            this.Active = active;
            this.UpdatedBy = updatedBy;
        }
    }
}
