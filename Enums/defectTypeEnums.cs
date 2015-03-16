using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enums
{
    /// <summary>
    /// class to hold code for defectTypeEnums 
    /// </summary>
    public class defectTypeEnums
    {
        /// <summary>
        /// constructor for defect type enums from lkup_DefectType
        /// </summary>
        public enum DefectType : int
        {
            /// <summary>
            /// value for defect
            /// </summary>
            Defect = 1,
            /// <summary>
            /// value for enhancement
            /// </summary>
            Enhancement = 2,
            /// <summary>
            /// value for newconstruction
            /// </summary>
            NewConstruction = 3
        }
    }
}
