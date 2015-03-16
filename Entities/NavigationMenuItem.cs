using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    /// <summary>
    /// class that defines a NavigationMenuItem which is used
    /// to transport data for each menu item in the main navigation
    /// tree(JSTree)
    /// </summary>
    public class NavigationMenuItem
    {
        #region properties and variables

        /// <summary>
        /// class variable to store value of navigation menu item ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable to store value of navigation menu item name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// class variable to store value of navigation menu item URL
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// class variable to store value of navigation menu item parent ID
        /// </summary>
        public int ParentID { get; set; }
        /// <summary>
        /// class variable to store value of navigation menu item node ID
        /// </summary>
        public string NodeID { get; set; }
        /// <summary>
        /// class variable to store value of menu item feature ID
        /// </summary>
        public int FeatureID { get; set; }

        #endregion properties and variables

        #region constructors

        /// <summary>
        /// class variable to store value of navigation menu item object
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="name">string</param>
        /// <param name="url">string</param>
        /// <param name="parentID">int</param>
        /// <param name="nodeID">string</param>
        /// /// <param name="featureID">int</param>
        public NavigationMenuItem(int id,
                                  string name,
                                  string url,
                                  int parentID,
                                  string nodeID,
                                  int featureID)
        {
            this.ID = id;
            this.Name = name;
            this.URL = url;
            this.ParentID = parentID;
            this.NodeID = nodeID;
            this.FeatureID = featureID;
        }

        #endregion constructors
    }
}
