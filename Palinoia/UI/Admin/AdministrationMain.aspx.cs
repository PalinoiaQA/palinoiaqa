//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

//namespace Palinoia.UI.Admin
//{//    
//    /// <summary>
//    /// class to hold the code for AdministrationMain
//    /// </summary>
//    //    public partial class AdministrationMain : System.Web.UI.Page
//    {//        
//        /// <summary>
//        /// loads a page
//        /// </summary>
//        /// <param name="sender">object</param>
//        /// <param name="e">EventArgs</param>
//        
//        protected void Page_Load(object sender, EventArgs e)
//        {
//            //check for valid user id
//            int userID = Convert.ToInt32(Session.Contents["userID"]);
//            if (userID > 0)
//            {

//            }
//            else
//            {
//                //user invalid.  redirect to login
//                Response.Redirect("~/Account/Login.aspx");
//            }
//        }
//    }
//}