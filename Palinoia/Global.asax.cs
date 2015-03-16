using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Palinoia
{
    /// <summary>
    /// class to hold code for Global
    /// </summary>
    public class Global : System.Web.HttpApplication
    {

        /// <summary>
        /// Code that runs on application startup
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        void Application_Start(object sender, EventArgs e)
        {
            System.Reflection.Assembly.Load("System.Data.SQLite.Linq");

        }

        /// <summary>
        /// Code that runs on application shutdown
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        /// <summary>
        /// Code that runs when an unhandled error occurs
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        /// <summary>
        /// Code that runs when a new session is started
        /// 
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        /// <summary>
        /// Code that runs when a session ends.
        /// Note: The Session_End event is raised only when the sessionstate mode
        /// is set to InProc in the Web.config file. If session mode is set to StateServer 
        /// or SQLServer, the event is not raised.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

    }
}
