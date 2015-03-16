using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enums
{
    /// <summary>
    /// class to hold enumeration code for testStatusEnums
    /// </summary>
    public class testStatusEnums
    {
        /// <summary>
        /// constructor for test status enums
        /// </summary>
        public enum TestStatus : int
        {
            /// <summary>
            /// Pass value
            /// </summary>
            Pass = 1,
            /// <summary>
            /// Fail value
            /// </summary>
            Fail = 2,
            /// <summary>
            /// Untested value
            /// </summary>
            Untested = 3
        }
    }
}