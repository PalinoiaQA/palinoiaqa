using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entities;
using BLL;

namespace Palinoia.UI.Documents
{
    /// <summary>
    /// class to hold code for ShowFunctionalDocumentPDF
    /// </summary>
    public partial class ShowFunctionalDocumentPDF : System.Web.UI.Page
    {
        /// <summary>
        /// loads a page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //pull document id from query string
            int projectID = Convert.ToInt32(Session.Contents["projectID"]);
            int documentID = 0;
            bool result = int.TryParse(Request.QueryString["did"], out documentID);
            int chapterID = 0;
            result = int.TryParse(Request.QueryString["cid"], out chapterID);
            DocumentsBLL bll = new DocumentsBLL(projectID);
            PostToPDF ppdf = new PostToPDF(projectID);
            var viewDoc = bll.getDocumentByID(documentID);
            var docHTML = viewDoc.getDocumentHTML();
            string documentName = "";
            if (chapterID > 0)
            {
                var chapter = bll.getChapterByID(chapterID);
                this.Title = chapter.Title;
                documentName = chapter.Title;
            }
            else
            {
                this.Title = viewDoc.Name;
                documentName = viewDoc.Name;
            }
            var pdfDoc = ppdf.createPDFFromHTML(viewDoc, chapterID);
            this.ShowPdf(pdfDoc, documentName);
        }

        /// <summary>
        /// show pdf
        /// <param name="strS">byte[]</param>
        /// <param name="fileName">string</param>
        /// </summary>
        private void ShowPdf(byte[] strS, string fileName)
        {
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "inline;filename=" + fileName + ".pdf");
            Response.BinaryWrite(strS);
            Response.End();
            Response.Flush();
            Response.Clear();
        }
    }
}