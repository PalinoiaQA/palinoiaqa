using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enums
{
     /// <summary>
    /// class to hold code for search object type Enums 
    /// </summary>
    public static class searchObjectTypeEnums
    {
        /// <summary>
        /// function to store enum values for search object types
        /// </summary>
        public enum SearchObjectType : int
        {
            /// <summary>
            /// class variable to store value for business rules
            /// </summary>
            BusinessRules = 1,
            /// <summary>
            /// class variable to store value for CustomerServiceMessages
            /// </summary>
            CustomerServiceMessages = 2,
            /// <summary>
            /// class variable to store value for  Defects
            /// </summary>
            Defects = 3,
            /// <summary>
            /// class variable to store value for TestCases
            /// </summary>
            TestCases = 4,
            /// <summary>
            /// class variable to store value for Documents
            /// </summary>
            Documents = 5
        }
    }
}
