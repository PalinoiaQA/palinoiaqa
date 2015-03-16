using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enums
{
    /// <summary>
    /// class to hold code for defectStatusEnums
    /// </summary>
    public class defectStatusEnums
    {
        /// <summary>
        /// constructor for defect status enums from lkup_DefectStatus
        /// </summary>
        public enum DefectStatus : int
        {
            /// <summary>
            /// value for new
            /// </summary>
            New = 1,
            /// <summary>
            /// value for reviewed
            /// </summary>
            Reviewed = 2,
            /// <summary>
            /// value for inprogress
            /// </summary>
            InProgress = 3,
            /// <summary>
            /// value for onhold
            /// </summary>
            OnHold = 4,
            /// <summary>
            /// value for rejected
            /// </summary>
            Rejected = 5,
            /// <summary>
            /// value for completed
            /// </summary>
            Completed = 6,
            /// <summary>
            /// value for cancelled
            /// </summary>
            Cancelled = 7
        }
    }
}
