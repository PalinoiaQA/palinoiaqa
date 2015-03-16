using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enums
{
    /// <summary>
    /// class to hold code for documentTypeEnums 
    /// </summary>
    public static class documentTypeEnums
    {
        /// <summary>
        /// function to store enum values for document types
        /// </summary>
        public enum DocumentType : int
        {
            /// <summary>
            /// value for functional document type
            /// </summary>
            Functional = 1,
            /// <summary>
            /// value for technical document type
            /// </summary>
            Technical = 2,
            /// <summary>
            /// value for test case document type
            /// </summary>
            TestCase = 3,
            /// <summary>
            /// value for miscellaneous document type
            /// </summary>
            Miscellaneous = 4
        }
    }
}
