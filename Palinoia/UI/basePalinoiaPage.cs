using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Services;
using System.Web.Script.Services;
using Entities;
using BLL;

namespace Palinoia.UI
{    
    /// <summary>
    /// class to hold code for basePalinoiaPage
    /// </summary>
    public class basePalinoiaPage : System.Web.UI.Page
    {
        #region base methods
                
        /// <summary>
        /// adds JavaScript references to web pages as they load
        /// </summary>
        /// <param name="jsFileName">string</param>
        public void addJavaScriptReference(string jsFileName)
        {
            //make sure page is not loaded outside of the master page
            if (Page.Header != null) 
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<script src=\"../../Scripts/Palinoia/" + jsFileName + "\" type=\"text/javascript\"></script>");
                //sb.AppendLine("<script type='text/javascript'>startTimeoutMonitor();</script>");
                LiteralControl javascriptRef = new LiteralControl(sb.ToString());
                Page.Header.Controls.Add(javascriptRef);
            }
        }

        /// <summary>
        /// add CK editor reference
        /// </summary>
        public void addCKEditorReference()
        {
            //make sure page is not loaded outside of the master page
            if (Page.Header != null) 
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<script src=\"../../ckeditor/ckeditor.js\" type=\"text/javascript\"></script>");
                LiteralControl javascriptRef = new LiteralControl(sb.ToString());
                Page.Header.Controls.Add(javascriptRef);
            }
        }

        /// <summary>
        /// add Search javascript file reference
        /// </summary>
        public void addSearchReference()
        {
            //make sure page is not loaded outside of the master page
            if (Page.Header != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<script src=\"../../Scripts/Palinoia/search.js\" type=\"text/javascript\"></script>");
                LiteralControl javascriptRef = new LiteralControl(sb.ToString());
                Page.Header.Controls.Add(javascriptRef);
            }
        }

        /// <summary>
        /// add Search javascript file reference
        /// </summary>
        public void addCookieReference()
        {
            //make sure page is not loaded outside of the master page
            if (Page.Header != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<script src=\"../../Scripts/jquery-cookie-1.4.0/jquery.cookie.js\" type=\"text/javascript\"></script>");
                LiteralControl javascriptRef = new LiteralControl(sb.ToString());
                Page.Header.Controls.Add(javascriptRef);
            }
        }

        /// <summary>
        /// add JavaSctipt references
        /// </summary>
        /// <param name="jsFileName"></param>
        /// <param name="extraScripts">string</param>
        //public void addJavaScriptReferences(string jsFileName, string extraScripts)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendLine("<script src=\"../../Scripts/jquery-1.8.2.min.js\" type=\"text/javascript\"></script>");
        //    sb.AppendLine("<script src=\"../../Scripts/jquery.validate.js\" type=\"text/javascript\"></script>");
        //    sb.AppendLine("<script src=\"../../Scripts/jquery.json-2.4.min.js\" type=\"text/javascript\"></script>");
        //    sb.AppendLine("<script src=\"../../Scripts/Palinoia/utility.js\" type=\"text/javascript\"></script>");
        //    sb.AppendLine("<script src=\"../../Scripts/jquery-ui-1.9.0.min.js\" type=\"text/javascript\"></script>");
        //    sb.AppendLine("<link href=\"../../Styles/jquery-ui-1.10.2.custom.min.css\" rel=\"stylesheet\" type=\"text/css\" />");
        //    extraScripts = extraScripts.ToUpper();
                
        //    var scriptArray = extraScripts.Split(',');
        //    foreach (var item in scriptArray)
        //    {
        //        switch (item.ToUpper())
        //        {
        //            case("JSTREE"):
        //                sb.AppendLine("<script src=\"../../Scripts/JSTree/jquery.jstree.js\" type=\"text/javascript\"></script>");
        //                sb.AppendLine("<link href=\"Scripts/JSTree/themes/classic/style.css\" rel=\"stylesheet\" type=\"text/css\" />");
        //                break;
        //            case("CKEDITOR"):
        //                sb.AppendLine("<script src=\"../../ckeditor/ckeditor.js\" type=\"text/javascript\"></script>");
        //                break;
        //        }

        //    }
        //    sb.AppendLine("<script src=\"../../Scripts/Palinoia/" + jsFileName + "\" type=\"text/javascript\"></script>");
        //    LiteralControl javascriptRef = new LiteralControl(sb.ToString());
        //    Page.Header.Controls.Add(javascriptRef);
        //}
                
        /// <summary>
        /// constructor with one parameter
        /// </summary>
        /// <param name="message">string</param>
        public void sendMessageToClient(string html)
        {
            //clean Message
            html = html.Replace("\"", "\\\"");
            html = html.Replace("\r\n", "<br />");
            // Define the name and type of the client script on the page.
            String csName = "messageToClient";
            Type csType = this.GetType();
            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs = Page.ClientScript;
            // Check to see if the client script is already registered.
            if (!cs.IsClientScriptBlockRegistered(csType, csName))
            {
                ClientScript.RegisterClientScriptBlock(typeof(Page), "SymbolError", "<script type='text/javascript'>jAlert('" + html + "');</script>");
            }
            ////clean Message
            //message = message.Replace("\"", "\\\"");
            //// Define the name and type of the client script on the page.
            //String csName = "messageToClient";
            //Type csType = this.GetType();
            //// Get a ClientScriptManager reference from the Page class.
            //ClientScriptManager cs = Page.ClientScript;
            //// Check to see if the client script is already registered.
            //if (!cs.IsClientScriptBlockRegistered(csType, csName))
            //{
            //    ClientScript.RegisterClientScriptBlock(typeof(Page), "SymbolError", "<script type='text/javascript'>messageToClient(\"" + message + "\");</script>");
            //}
        }

        public void sendHTMLMessageToClient(string html)
        {
            //clean Message
            html = html.Replace("\"", "\\\"");
            html = html.Replace("\r\n", "<br />");
            // Define the name and type of the client script on the page.
            String csName = "messageToClient";
            Type csType = this.GetType();
            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs = Page.ClientScript;
            // Check to see if the client script is already registered.
            if (!cs.IsClientScriptBlockRegistered(csType, csName))
            {
                //ClientScript.RegisterClientScriptBlock(typeof(Page), "SymbolError", "<script type='text/javascript'>jAlert('KLH');</script>");
                ClientScript.RegisterClientScriptBlock(typeof(Page), "SymbolError", "<script type='text/javascript'>jAlert('" + html + "');</script>");
            }
        }
                
        /// <summary>
        /// set up client UI for validation
        /// </summary>
        public void setUpClientUIForValidation()
        {
            // Define the name and type of the client script on the page.
            String csName = "initializeClientUI";
            Type csType = this.GetType();
            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs = Page.ClientScript;
            // Check to see if the client script is already registered.
            if (!cs.IsStartupScriptRegistered(csType, csName))
            {
                ClientScript.RegisterStartupScript(typeof(Page), csName, "<script type='text/javascript'>initializeControlVerification();</script>");
            }
        }

        /// <summary>
        /// validate input text
        /// </summary>
        /// <param name="s">string</param>
        /// <returns>bool</returns>
        public bool validateInputText(string s)
        {
            bool result = false;
            Regex reg = new Regex("[!$%&*_<>\\//]");
            if (!reg.IsMatch(s))
            {
                result = true;
            }
            return result;
        }
                
        /// <summary>
        /// show or hide UI dialog on client
        /// </summary>
        public void showDialog(bool visible)
        {
            // Define the name and type of the client script on the page.
            String csName = "ShowDialog";
            Type csType = this.GetType();
            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs = Page.ClientScript;
            StringBuilder sb = new StringBuilder();
            sb.Append("$(document).ready(");
            sb.Append("function () {showDialog('");
            sb.Append(visible);
            sb.Append("');});");
            // Check to see if the client script is already registered.
            if (!cs.IsStartupScriptRegistered(csType, csName))
            {
                ClientScript.RegisterStartupScript(typeof(Page), csName, sb.ToString(), true);
            }
        }

        

        #endregion base methods

        
    }
}