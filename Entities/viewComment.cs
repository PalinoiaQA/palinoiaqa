using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    /// <summary>
    /// class to hold code for viewComment
    /// </summary>
    public class viewComment
    {
        /// <summary>
        /// class variable to store value of comment ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable to store value of Defect ID
        /// </summary>
        public int DefectID { get; set; }
        /// <summary>
        /// class variable to store value of user ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// class variable to store date that comment was created
        /// </summary>
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// class variable to store value of comment text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// constructor for viewComment
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="defectID">int</param>
        /// <param name="userID">int</param>
        /// <param name="dateCreated">DateTime</param>
        /// <param name="text">string</param>
        public viewComment(int id, int defectID, int userID, DateTime dateCreated, string text)
        {
            this.ID = id;
            this.DefectID = defectID;
            this.UserID = userID;
            this.DateCreated = dateCreated;
            this.Text = text;
        }
    }
}
