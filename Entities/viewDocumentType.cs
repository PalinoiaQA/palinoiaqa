using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    /// <summary>
    /// class to hold code for viewDocumentType
    /// </summary>
    public class viewDocumentType
    {
        /// <summary>
        /// class variable to store document type ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable to store document type name
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// class variable to store document type active status
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// class variable to store status of IncludeBRCSMChapterSummary
        /// </summary>
        public bool IncludeBRCSMChapterSummary { get; set; }
        /// <summary>
        /// class variable to store key to person updating document type
        /// </summary>
        public int UpdatedBy { get; set; }

        /// <summary>
        /// constructor for viewDocumentType object
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="text">string</param>
        /// <param name="active">bool</param>
        /// <param name="includeBRCSMChapterSummary">bool</param>
        /// <param name="updatedBy">int</param>
        public viewDocumentType(int id, string text, bool active, bool includeBRCSMChapterSummary, int updatedBy)
        {
            this.ID = id;
            this.Text = text;
            this.Active = active;
            this.IncludeBRCSMChapterSummary = includeBRCSMChapterSummary;
            this.UpdatedBy = updatedBy;
        }
    }
}
