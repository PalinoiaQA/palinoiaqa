using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enums
{
    /// <summary>
    /// class to hold code for SearchDataTypeEnums
    /// </summary>
    public static class SearchDataTypeEnums
    {
        /// <summary>
        /// class variable to store value of enum
        /// </summary>
        public enum SearchDataType : int
        {
            /// <summary>
            /// class variable to store value for ID
            /// </summary>
            ID = 0,
            /// <summary>
            /// class variable to store value for interger
            /// </summary>
            Integer = 1,
            /// <summary>
            /// class variable to store value for string
            /// </summary>
            String = 2,
            /// <summary>
            /// class variable to store value for date
            /// </summary>
            Date = 3,
            /// <summary>
            /// class variable to store value for bool
            /// </summary>
            Bool = 4
        }
    }
}
