using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Entities
{
    /// <summary>
    /// Report class used to transfer report data to pdf builder
    /// </summary>
    public class Report
    {
        #region properties and variables

        /// <summary>
        /// report title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// report content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// constructor for chapter type enums
        /// </summary>
        public enum ReportType : int
        {
            TESTCASE_ORPHANED_BUSINESSRULES = 1,
            DOCUMENT_ORPHANED_BUSINESSRULES = 2,
            FUNCTIONAL_DOCUMENT_ORPHANED_BUSINESSRULES = 3,
            TECHNICAL_DOCUMENT_ORPHANED_BUSINESSRULES = 4,
            MISCELLANEOUS_DOCUMENT_ORPHANED_BUSINESSRULES = 5,
            BUSINESSRULES_BY_SECTION = 6,
            BUSINESSRULES_BY_STATUS = 7
        }

        #endregion properties and variables

        #region constructors

        public Report(List<viewBusinessRule> orphanedBusinessRules, int reportType)
        {
            this.buildReport(orphanedBusinessRules, reportType);
        }

        public Report(string content, int reportType)
        {
            this.buildReport(content, reportType);
        }

        #endregion constructors

        #region private methods

        private void buildReport(string text, int reportType)
        {
            StringBuilder content = new StringBuilder();
            string title = "";
            switch (reportType)
            {
                case ((int)ReportType.BUSINESSRULES_BY_SECTION):
                    title = "Business Rule list for [SECTION] Section";
                    content.AppendLine(text);
                    break;
                case ((int)ReportType.BUSINESSRULES_BY_STATUS):
                    title = "[STATUS] Business Rule list";
                    content.AppendLine(text);
                    break;
            }
            this.Title = title;
            this.Content = content.ToString();
        }

        private void buildReport(List<viewBusinessRule> orphanedBusinessRules, int reportType)
        {
            StringBuilder content = new StringBuilder();
            string title = "";
            string openingParagraph = "";
            switch (reportType)
            {
                case ((int)ReportType.TESTCASE_ORPHANED_BUSINESSRULES):
                    title = "Test Cases Orphaned Business Rules Report";
                    if (orphanedBusinessRules.Count > 0)
                    {
                        openingParagraph = "The following " + orphanedBusinessRules.Count + " Business Rules are not referenced in any project test case:";
                        content.AppendLine();
                        content.AppendLine(openingParagraph);
                        content.AppendLine();
                        foreach (var br in orphanedBusinessRules)
                        {
                            content.AppendLine(br.Name);
                        }
                    }
                    else
                    {
                        openingParagraph = "All business rules are referenced in a project test case.";
                        content.AppendLine();
                        content.AppendLine(openingParagraph);
                    }
                    break;
                case((int)ReportType.DOCUMENT_ORPHANED_BUSINESSRULES):
                    title = "All Documents Orphaned Business Rules Report";
                    if (orphanedBusinessRules.Count > 0)
                    {
                        openingParagraph = "The following " + orphanedBusinessRules.Count + " Business Rules are not referenced in any project documents:";
                        content.AppendLine();
                        content.AppendLine(openingParagraph);
                        content.AppendLine();
                        foreach (var br in orphanedBusinessRules)
                        {
                            content.AppendLine(br.Name);
                        }
                    }
                    else
                    {
                        openingParagraph = "All business rules are referenced in project documents.";
                        content.AppendLine();
                        content.AppendLine(openingParagraph);
                    }
                    break;
                case ((int)ReportType.FUNCTIONAL_DOCUMENT_ORPHANED_BUSINESSRULES):
                    title = "Functional Documents Orphaned Business Rules Report";
                    if (orphanedBusinessRules.Count > 0)
                    {
                        openingParagraph = "The following " + orphanedBusinessRules.Count + " Business Rules are not referenced in any functional documents:";
                        content.AppendLine();
                        content.AppendLine(openingParagraph);
                        content.AppendLine();
                        foreach (var br in orphanedBusinessRules)
                        {
                            content.AppendLine(br.Name);
                        }
                    }
                    else
                    {
                        openingParagraph = "All business rules are referenced in functional documents.";
                        content.AppendLine();
                        content.AppendLine(openingParagraph);
                    }
                    break;
                case ((int)ReportType.TECHNICAL_DOCUMENT_ORPHANED_BUSINESSRULES):
                    title = "Technical Documents Orphaned Business Rules Report";
                    if (orphanedBusinessRules.Count > 0)
                    {
                        openingParagraph = "The following " + orphanedBusinessRules.Count + " Business Rules are not referenced in any technical documents:";
                        content.AppendLine();
                        content.AppendLine(openingParagraph);
                        content.AppendLine();
                        foreach (var br in orphanedBusinessRules)
                        {
                            content.AppendLine(br.Name);
                        }
                    }
                    else
                    {
                        openingParagraph = "All business rules are referenced in technical documents.";
                        content.AppendLine();
                        content.AppendLine(openingParagraph);
                    }
                    break;
                case ((int)ReportType.MISCELLANEOUS_DOCUMENT_ORPHANED_BUSINESSRULES):
                    title = "Miscellaneous Documents Orphaned Business Rules Report";
                    if (orphanedBusinessRules.Count > 0)
                    {
                        openingParagraph = "The following " + orphanedBusinessRules.Count + " Business Rules are not referenced in any miscellenous documents:";
                        content.AppendLine();
                        content.AppendLine(openingParagraph);
                        content.AppendLine();
                        foreach (var br in orphanedBusinessRules)
                        {
                            content.AppendLine(br.Name);
                        }
                    }
                    else
                    {
                        openingParagraph = "All business rules are referenced in miscellanous documents.";
                        content.AppendLine();
                        content.AppendLine(openingParagraph);
                    }
                    break;
                
            }
            this.Title = title;
            this.Content = content.ToString();
        }

        #endregion private methods
    }
}
