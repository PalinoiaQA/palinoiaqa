using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Entities;

namespace Palinoia.UI.Documents
{
    /// <summary>
    /// class to hold code for ShowTestCaseDocumentPDF object
    /// </summary>
    public partial class ShowTestCaseDocumentPDF : System.Web.UI.Page
    {
        /// <summary>
        /// load page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //pull document id from query string
            int projectID = Convert.ToInt32(Session.Contents["projectID"]);
            int testCaseID = 0;
            bool result2 = int.TryParse(Request.QueryString["tcid"], out testCaseID);
            AdminBLL adminBLL = new AdminBLL(projectID);
            DocumentsBLL docBLL = new DocumentsBLL(projectID);
            TestCasesBLL bll = new TestCasesBLL(projectID);
            //get the doc type to determine if chapter summaries should be included
            var docType = adminBLL.getDocumentTypeByID(3); // 3 = Test Case Document
            var showSummary = docType.IncludeBRCSMChapterSummary;
            PostToPDF ppdf = new PostToPDF(projectID);
            var viewTC = bll.getTestCaseByID(testCaseID);
            this.Title = viewTC.Name;
            var pdfDoc = ppdf.createTestCasePDF(viewTC, showSummary);
            this.ShowPdf(pdfDoc, viewTC.Name);
        }

        /// <summary>
        /// show pdf
        /// </summary>
        /// <param name="strS">byte[]</param>
        /// <param name="fileName">string</param>
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