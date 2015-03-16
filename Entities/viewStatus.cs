using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{    
    /// <summary>
    /// class to hold the code for the viewStatus object
    /// </summary>
    public class viewStatus
    {
        #region Properties and Variables

        /// <summary>
        /// class variable to store status ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable to store status text
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// class variable to store status active status
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// class variable to store status color
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// class variable to store status DisplayInChapterSummary
        /// </summary>
        public bool DisplayInChapterSummary { get; set; }
        /// <summary>
        /// class variable to store UserID as UpdatedBy
        /// </summary>
        public int UpdatedBy { get; set; }

        #endregion Properties and Variables

        #region Constructors
                
        /// <summary>
        /// constructor for viewStatus object
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="text">string</param>
        /// <param name="active">bool</param>
        /// <param name="color">string</param>
        /// <param name="displayInChapterSummary">bool</param>
        /// <param name="updatedBy">int</param>
        public viewStatus(int id, string text, bool active, string color, bool displayInChapterSummary, int updatedBy)
        {
            this.ID = id;
            this.Text = text;
            this.Active = active;
            this.Color = color;
            this.DisplayInChapterSummary = displayInChapterSummary;
            this.UpdatedBy = updatedBy;
        }

        #endregion Constructors
    }
}
