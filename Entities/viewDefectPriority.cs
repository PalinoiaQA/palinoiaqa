using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    /// <summary>
    /// class to hold viewDefectPriorty info
    /// </summary>
    public class viewDefectPriority
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
        /// class variable to store ID of person that updated the defect info
        /// </summary>
        public int UpdatedBy { get; set; }
        /// <summary>
        /// class variable to store importance value which is used to sort
        /// the defects in the grid
        /// </summary>
        public int Importance { get; set; }

        /// <summary>
        /// constructor for viewDefectPriority object
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="text">string</param>
        /// <param name="active">bool</param>
        /// <param name="updatedBy">int</param>
        /// <param name="importance">int</param>
        public viewDefectPriority(int id, string text, bool active, int updatedBy, int importance)
        {
            this.ID = id;
            this.Text = text;
            this.Active = active;
            this.UpdatedBy = updatedBy;
            this.Importance = importance;
        }
    }
}
