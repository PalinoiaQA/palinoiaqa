using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    /// <summary>
    /// class to hold viewDefectStatus info
    /// </summary>
    public class viewDefectStatus
    {
        /// <summary>
        /// class variable to store defect ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable to store defect text
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// class variable to store defect active status
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// class variable to store key to person updating defect
        /// </summary>
        public int UpdatedBy { get; set; }

        /// <summary>
        /// constructor for viewDefectStatus
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="text">string</param>
        /// <param name="active">bool</param>
        /// <param name="updatedBy">int</param>
        public viewDefectStatus(int id, string text, bool active, int updatedBy)
        {
            this.ID = id;
            this.Text = text;
            this.Active = active;
            this.UpdatedBy = updatedBy;
        }
    }
}
