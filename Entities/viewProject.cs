using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{    
    /// <summary>
    /// class to hold the code for the viewProject object
    /// </summary>
    public class viewProject
    {
        #region properties and variables

        /// <summary>
        /// class variable to store project ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable to store project name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// class variable to store project data source
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// class variable to stored updatedby user id
        /// </summary>
        public int UpdatedBy { get; set; }

        /// <summary>
        /// class variable to stored Active data source
        /// </summary>
        public bool Active { get; set; }

        #endregion properties and variables

        #region constructors
                
        /// <summary>
        /// constructor for viewProject object
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="name">string</param>
        /// <param name="datasource">string</param>
        /// <param name="active">bool</param>
        /// <param name="updatedBy">int</param>
        public viewProject(int id, string name, string datasource, bool active, int updatedBy)
        {
            this.ID = id;
            this.Name = name;
            this.DataSource = datasource;
            this.Active = active;
            this.UpdatedBy = updatedBy;
        }

        #endregion constructors

        

    }
}
