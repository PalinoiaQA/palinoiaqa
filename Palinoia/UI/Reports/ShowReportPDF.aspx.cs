using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Entities;
using System.Text;

namespace Palinoia.UI.Reports
{
    public partial class ShowReportPDF : System.Web.UI.Page
    {
        /// <summary>
        /// Page_Load method
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //pull report id and project id from query string
                int projectID = Convert.ToInt32(Session.Contents["projectID"]);
                int reportID = 0;
                int docTypeID = 0;
                int sectionID = 0;
                int statusID = 0;
                bool result = int.TryParse(Request.QueryString["rid"], out reportID);
                result = int.TryParse(Request.QueryString["dtid"], out docTypeID);
                result = int.TryParse(Request.QueryString["sid"], out sectionID);
                result = int.TryParse(Request.QueryString["stid"], out statusID);
                DocumentsBLL docBLL = new DocumentsBLL(projectID);
                PostToPDF ppdf = new PostToPDF(projectID);
                switch (reportID)
                {
                    case ((int)Report.ReportType.TESTCASE_ORPHANED_BUSINESSRULES):
                        var orphanedBusinessRules = docBLL.getBusinessRulesWithoutTestCase();
                        var report1 = new Report(orphanedBusinessRules, (int)Report.ReportType.TESTCASE_ORPHANED_BUSINESSRULES);
                        var pdfDoc1 = ppdf.createReportPDF(report1);
                        this.ShowPdf(pdfDoc1, report1.Title);
                        break;
                    case ((int)Report.ReportType.DOCUMENT_ORPHANED_BUSINESSRULES):
                        var orphanedDocBusinessRules = docBLL.getBusinessRulesNotInDocuments();
                        var report2 = new Report(orphanedDocBusinessRules, (int)Report.ReportType.DOCUMENT_ORPHANED_BUSINESSRULES);
                        var pdfDoc2 = ppdf.createReportPDF(report2);
                        this.ShowPdf(pdfDoc2, report2.Title);
                        break;
                    case ((int)Report.ReportType.FUNCTIONAL_DOCUMENT_ORPHANED_BUSINESSRULES):
                        var orphanedFuncDocBusinessRules = docBLL.getBusinessRulesNotInDocuments(docTypeID);
                        var report3 = new Report(orphanedFuncDocBusinessRules, (int)Report.ReportType.FUNCTIONAL_DOCUMENT_ORPHANED_BUSINESSRULES);
                        var pdfDoc3 = ppdf.createReportPDF(report3);
                        this.ShowPdf(pdfDoc3, report3.Title);
                        break;
                    case ((int)Report.ReportType.TECHNICAL_DOCUMENT_ORPHANED_BUSINESSRULES):
                        var orphanedTechDocBusinessRules = docBLL.getBusinessRulesNotInDocuments(docTypeID);
                        var report4 = new Report(orphanedTechDocBusinessRules, (int)Report.ReportType.TECHNICAL_DOCUMENT_ORPHANED_BUSINESSRULES);
                        var pdfDoc4 = ppdf.createReportPDF(report4);
                        this.ShowPdf(pdfDoc4, report4.Title);
                        break;
                    case ((int)Report.ReportType.MISCELLANEOUS_DOCUMENT_ORPHANED_BUSINESSRULES):
                        var orphanedMiscDocBusinessRules = docBLL.getBusinessRulesNotInDocuments(docTypeID);
                        var report5 = new Report(orphanedMiscDocBusinessRules, (int)Report.ReportType.MISCELLANEOUS_DOCUMENT_ORPHANED_BUSINESSRULES);
                        var pdfDoc5 = ppdf.createReportPDF(report5);
                        this.ShowPdf(pdfDoc5, report5.Title);
                        break;
                    case ((int)Report.ReportType.BUSINESSRULES_BY_SECTION):
                        BusinessRulesBLL brBLL6 = new BusinessRulesBLL(projectID);
                        AdminBLL adminBLL6 = new AdminBLL(projectID);
                        var brList6 = brBLL6.getAllBusinessRulesBySection(sectionID);
                        StringBuilder brListText6 = new StringBuilder();
                        foreach (var br in brList6)
                        {
                            brListText6.Append(br.Name);
                            brListText6.Append(",");
                        }
                        string reportHTML = docBLL.createSummaryFromText(brListText6.ToString(), true);
                        var report6 = new Report(reportHTML, (int)Report.ReportType.BUSINESSRULES_BY_SECTION);
                        report6.Title = report6.Title.Replace("[SECTION]", adminBLL6.getSectionByID(sectionID).Text);
                        var pdfDoc6 = ppdf.createSummaryReportPDF(report6);
                        this.ShowPdf(pdfDoc6, report6.Title);
                        brBLL6 = null;
                        adminBLL6 = null;
                        break;
                    case ((int)Report.ReportType.BUSINESSRULES_BY_STATUS):
                        BusinessRulesBLL brBLL7 = new BusinessRulesBLL(projectID);
                        AdminBLL adminBLL7 = new AdminBLL(projectID);
                        var brList7 = brBLL7.getAllBusinessRulesByStatus(statusID);
                        StringBuilder brListText7 = new StringBuilder();
                        foreach (var br in brList7)
                        {
                            brListText7.Append(br.Name);
                            brListText7.Append(",");
                        }
                        string reportHTML7 = docBLL.createSummaryFromText(brListText7.ToString(), false);
                        var report7 = new Report(reportHTML7, (int)Report.ReportType.BUSINESSRULES_BY_STATUS);
                        report7.Title = report7.Title.Replace("[STATUS]", adminBLL7.getStatusByID(statusID).Text);
                        var pdfDoc7 = ppdf.createSummaryReportPDF(report7);
                        this.ShowPdf(pdfDoc7, report7.Title);
                        brBLL7 = null;
                        adminBLL7 = null;
                        break;
                }
                docBLL = null;
            }
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