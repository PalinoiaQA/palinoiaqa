using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    /// <summary>
    /// class to hold viewChapterType info
    /// </summary>
    public class viewChapterType
    {
        /// <summary>
        /// class variable to store chapter type ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable to store chapter type text
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// class variable to store chapter type active status
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// class variable to store key to person updating chapter type
        /// </summary>
        public int UpdatedBy { get; set; }

        /// <summary>
        /// constructor for viewChapterType object
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="text">string</param>
        /// <param name="active">bool</param>
        /// <param name="updatedBy">int</param>
        public viewChapterType(int id, string text, bool active, int updatedBy)
        {
            this.ID = id;
            this.Text = text;
            this.Active = active;
            this.UpdatedBy = updatedBy;
        }
    }
}
