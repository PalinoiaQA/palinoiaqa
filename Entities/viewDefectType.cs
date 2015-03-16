using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    /// <summary>
    /// class to hold viewDefectType object
    /// </summary>
    public class viewDefectType
    {
        /// <summary>
        /// class variable to store defect type ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable to store defect type text
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// class variable to store defect type active status
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// class variable to store key to person updating defect type
        /// </summary>
        public int UpdatedBy { get; set; }

        /// <summary>
        /// constructor for viewDefectType object
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="text">string</param>
        /// <param name="active">bool</param>
        /// <param name="updatedBy">int</param>
        public viewDefectType(int id, string text, bool active, int updatedBy)
        {
            this.ID = id;
            this.Text = text;
            this.Active = active;
            this.UpdatedBy = updatedBy;
        }
    }
}
