using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    class viewProjects
    {
        #region properties and variables

        public int ID { get; set; }
        string Name { get; set; }
        string DataSource { get; set; }

        #endregion properties and variables

        #region constructors

        public viewProjects(int id, string name, string datasource)
        {
            this.ID = id;
            this.Name = name;
            this.DataSource = datasource;
        }

        #endregion constructors

        

    }
}
