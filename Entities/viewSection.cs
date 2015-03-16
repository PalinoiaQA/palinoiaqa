using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
        
    /// <summary>
    /// class to hold code for viewSection object
    /// </summary>
    public class viewSection
    {
        #region properties and variables

        /// <summary>
        /// class variable to store section ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable to store section name
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// class variable to store section abbreviation
        /// </summary>
        public string Abbreviation { get; set; }
        /// <summary>
        /// class variable to store section active status
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// class variable to store userID as UpdatedBy
        /// </summary>
        public int UpdatedBy { get; set; }

        #endregion properties and variables

        #region constructors
                
        /// <summary>
        /// constructor for viewSection object
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="name">string</param>
        /// <param name="abbreviation">string</param>
        /// <param name="active">bool</param>
        /// <param name="updatedBy">int</param>
        public viewSection(int id, string name, string abbreviation, bool active, int updatedBy)
        {
            this.ID = id;
            this.Text = name;
            this.Abbreviation = abbreviation;
            this.Active = active;
            this.UpdatedBy = updatedBy;
        }

        #endregion constructors
    }
}
