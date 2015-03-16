using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enums
{
    /// <summary>
    /// class to hold code for chapterTypeEnums
    /// </summary>
    public class chapterTypeEnums
    {
        /// <summary>
        /// constructor for chapter type enums
        /// </summary>
        public enum ChapterType : int
        {
            /// <summary>
            /// value for user
            /// </summary>
            User = 1,
            /// <summary>
            /// value for system
            /// </summary>
            System = 2
        }
    }
}
