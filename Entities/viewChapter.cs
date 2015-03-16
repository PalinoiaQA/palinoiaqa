using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    /// <summary>
    /// class to hold viewchapter object
    /// </summary>
    public class viewChapter
    {
        /// <summary>
        /// class variable to store chapter ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable to store chapter title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// class variable to store chapter text
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// class variable to store chapter type ID
        /// </summary>
        public int TypeID { get; set; }
        /// <summary>
        /// class variable to store chapter sequence number
        /// </summary>
        public int SeqNum { get; set; }
        /// <summary>
        /// class variable to store chapter active status
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// class variable to store key to person updating chapter
        /// </summary>
        public int UpdatedBy { get; set; }

        /// <summary>
        /// constructor for viewChapter object
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="title">string</param>
        /// <param name="text">string</param>
        /// <param name="typeID">int</param>
        /// <param name="updatedBy">int</param>
        /// <param name="active">bool</param>
        /// <param name="seqNum">int</param>
        public viewChapter(int id, string title, string text, int typeID, int updatedBy, bool active, int seqNum = 0)
        {
            this.ID = id;
            this.Title = title;
            this.Text = text;
            this.TypeID = typeID;
            this.SeqNum = seqNum;
            this.UpdatedBy = updatedBy;
            this.Active = active;
        }

        /// <summary>
        /// empty constructor
        /// </summary>
        public viewChapter()
        {

        }
    }
}
