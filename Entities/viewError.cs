using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    /// <summary>
    /// class to hold code for viewError
    /// </summary>
    public class viewError
    {
        /// <summary>
        /// class variable for error ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable for error date
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// string version of Date property for display on client
        /// </summary>
        public string DisplayDate {
            get {
                return Date.ToString("F");
                //StringBuilder displayDate = new StringBuilder();
                //displayDate.Append(Date.Month.ToString());
                //displayDate.Append("/");
                //displayDate.Append(Date.Day.ToString());
                //displayDate.Append("/");
                //displayDate.Append(Date.Year.ToString());
                //displayDate.Append(" - ");
                //displayDate.Append(Date.TimeOfDay.ToString());
                //return displayDate.ToString();
            }
        }
        /// <summary>
        /// class variable for error message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// class variable for error source
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// class variable for error inner exception
        /// </summary>
        public string InnerException { get; set; }
        /// <summary>
        /// class variable for error stack trace
        /// </summary>
        public string StackTrace { get; set; }
        /// <summary>
        /// class variable for project ID that contained the error
        /// </summary>
        public int ProjectID { get; set; }
        /// <summary>
        /// class variable for user ID associated with error
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// constructor for viewError
        /// </summary>
        /// <param name="id"></param>
        /// <param name="date"></param>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <param name="stackTrace"></param>
        /// <param name="projectID"></param>
        /// <param name="userID"></param>
        public viewError(int id, DateTime date, string source, string message, string innerException, string stackTrace, int projectID, int userID)
        {
            this.ID = id;
            this.Date = date;
            this.Source = source;
            this.Message = message;
            this.InnerException = innerException;
            this.StackTrace = stackTrace;
            this.ProjectID = projectID;
            this.UserID = userID;
        }

        public viewError()
        {
        }
    }
}
