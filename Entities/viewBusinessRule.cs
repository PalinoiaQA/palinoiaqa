using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{    
    /// <summary>
    /// class to hold the code for the viewBusinessRule object
    /// </summary>
    public class viewBusinessRule : IEquatable<viewBusinessRule>
    {
        #region Properties and Variables

        /// <summary>
        /// class variable to store business rule ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable to store business rule name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// class variable to store business rule status ID
        /// </summary>
        public int StatusID { get; set; }
        /// <summary>
        /// class variable to store business rule section ID
        /// </summary>
        public int SectionID { get; set; }
        /// <summary>
        /// class variable to store business rule text
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// class variable to store business rule active status
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// class variable to store key to person updating business rule 
        /// </summary>
        public int UpdatedBy { get; set; }

        #endregion Properties and Variables

        #region Constructors

        /// <summary>
        /// empty constructor for viewBusinessRule Object
        /// </summary>
        public viewBusinessRule()
        {

        }
                
        /// <summary>
        /// constructor for viewBusinessRule object
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="name">string</param>
        /// <param name="statusID">int</param>
        /// <param name="sectionID">int</param>
        /// <param name="text">string</param>
        /// <param name="active">bool</param>
        /// <param name="updatedBy">int</param>
        public viewBusinessRule(int id, string name, int statusID, int sectionID, string text, bool active, int updatedBy)
        {
            this.ID = id;
            this.Name = name;
            this.StatusID = statusID;
            this.SectionID = sectionID;
            this.Text = text;
            this.Active = active;
            this.UpdatedBy = updatedBy;
        }

        #endregion Constructors

        public bool Equals(viewBusinessRule other)
        {
            if (other == null) return false;
            return (this.Name.Equals(other.Name));
        }
        // Should also override == and != operators.
    }
}
