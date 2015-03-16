using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enums
{
    /// <summary>
    /// class to hold code for defect priority Enums from lkup_DefectPriority
    /// </summary>
    public class defectPriorityEnums
    {
        /// <summary>
        /// constructor for chapter type enums
        /// </summary>
        public enum DefectPriority : int
        {
            /// <summary>
            /// value for critical
            /// </summary>
            Critical = 1,
            /// <summary>
            /// value for high
            /// </summary>
            High = 2,
            /// <summary>
            /// value for medium
            /// </summary>
            Medium = 3,
            /// <summary>
            /// value for low
            /// </summary>
            Low = 4
        }
    }
}
