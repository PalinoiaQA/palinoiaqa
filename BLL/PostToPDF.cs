using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Entities;

//for converting HTML to PDF 

using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html.simpleparser;
using iTextSharp;

using System.IO;
using System.util;
using System.Text.RegularExpressions;
using System.Collections;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.html;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.parser;


namespace BLL
{
    /// <summary>
    /// class to hold code for PostToPDF
    /// </summary>
    public partial class PostToPDF : System.Web.UI.Page
    {
        #region properties and variables

        int ProjectID = 1;
        Hashtable imageHash = new Hashtable();
        List<ImageInfo> imageInfoList = new List<ImageInfo>();
        DocumentsBLL docBLL;

        #endregion properties and variables

        #region constructors

        /// <summary>
        /// constructor for PostToPDF
        /// </summary>
        /// <param name="projectID"></param>
        public PostToPDF(int projectID)
        {
            this.ProjectID = projectID;
            this.docBLL = new DocumentsBLL(projectID);
        }

        #endregion constructors

        #region public methods

        /// <summary>
        /// create pdf for test case document
        /// </summary>
        /// <param name="tc">viewTestCase</param>
        /// <param name="showSummary">bool</param>
        /// <returns>byte[]</returns>
        public byte[] createTestCasePDF(viewTestCase tc, bool showSummary)
        {
            TestCasesBLL tcBLL = new TestCasesBLL(this.ProjectID);
            DocumentsBLL docBLL = new DocumentsBLL(this.ProjectID);
            StringBuilder testStepText = new StringBuilder();
            using (MemoryStream ms = new MemoryStream())
            {
                using (Document doc = new Document(PageSize.LETTER))
                {
                    using (PdfWriter writer = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();
                        var tcList = tcBLL.getPreConditionsForTestCase(tc.ID);
                        tcList.Add(tc);
                        int totalTestCases = tcList.Count;
                        int tcCounter = 0;
                        foreach (var tcase in tcList)
                        {
                            #region add test case to doc
                            tcCounter++;
                            string title = "";
                            if (tcCounter < totalTestCases)
                            {
                                title = "Prerequisite Test Case: ";
                            }
                            else
                            {
                                title = "Test Case: ";
                            }
                            Chunk c = new Chunk(title + tcase.Name, FontFactory.GetFont("HELVETICA", 18));
                            Paragraph p = new Paragraph();
                            p.Add(c);
                            doc.Add(p);
                            p = new Paragraph();
                            p.SpacingAfter = 15f;
                            doc.Add(p);
                            ////create bulleted list
                            iTextSharp.text.List list = new iTextSharp.text.List(List.UNORDERED, 10f);
                            p = new Paragraph();
                            p.SpacingAfter = 10f;
                            doc.Add(p);
                            p = new Paragraph();
                            c = new Chunk("Test Steps:", FontFactory.GetFont("HELVETICA", 14));
                            p.Add(c);
                            doc.Add(p);
                            doc.Add(new Paragraph(""));
                            //create bulleted list
                            list = new List(List.ORDERED, 10f);
                            list.SetListSymbol("\u2022");
                            list.SymbolIndent = 15f;
                            list.IndentationLeft = 30f;
                            var tsList = tcBLL.getTestStepsForTestCase(tcase.ID);
                            foreach (var ts in tsList)
                            {
                                StringBuilder testStep = new StringBuilder();
                                testStep.Append(ts.Name);
                                // spin through all related business rules
                                int totalRelatedBusinessRules = ts.RelatedBusinessRules.Count;
                                int ruleCounter = 0;
                                if (ts.RelatedBusinessRules.Count > 0)
                                {
                                    testStep.Append(" ");
                                    testStep.Append("(");
                                    foreach (var br in ts.RelatedBusinessRules)
                                    {
                                        testStep.Append(br.Name);
                                        ruleCounter++;
                                        if (ruleCounter < totalRelatedBusinessRules)
                                        {
                                            testStep.Append(", ");
                                        }
                                    }
                                    testStep.Append(")");
                                }
                                testStepText.Append(testStep.ToString());

                                if (ts.Notes.Length > 0)
                                {
                                    testStep.AppendLine("");
                                    testStep.Append(" NOTE: ");
                                    testStep.Append(ts.Notes);
                                }
                                list.Add(testStep.ToString());
                            }
                            //add bulleted list to doc
                            doc.Add(list);
                            p = new Paragraph();
                            p.SpacingAfter = 10f;
                            doc.Add(p);
                            p = new Paragraph();
                            p.SpacingAfter = 15f;
                            doc.Add(p);
                            #endregion add test case to doc
                        }
                        if (showSummary)
                        {
                            #region add summary table
                            string text = docBLL.createSummaryFromText(testStepText.ToString(), true);
                            text = this.fixTableRowBackgroundTags(text);
                            using (StringReader sr = new StringReader(text))
                            {
                                try
                                {
                                    //Create a style sheet
                                    var htmlContext = new HtmlPipelineContext(null);
                                    htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());
                                    ICSSResolver cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(true);
                                    IPipeline pipeline = new CssResolverPipeline(cssResolver, new HtmlPipeline(htmlContext, new PdfWriterPipeline(doc, writer)));
                                    var worker = new XMLWorker(pipeline, true);
                                    var xmlParse = new XMLParser(true, worker);
                                    xmlParse.Parse(sr);
                                    xmlParse.Flush();
                                }
                                catch (Exception ex)
                                {

                                }
                            }

                            #endregion add summary table
                        }
                        doc.Close();
                    }
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// create PDF from HTML using iTextSharp XMLWorker obj
        /// </summary>
        /// <param name="document">viewDocument</param>
        /// <param name="chapterID">int</param>
        /// <returns>byte</returns>
        public byte[] createPDFFromHTML(viewDocument document, int chapterID)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Document doc = new Document(PageSize.LETTER))
                {
                    using (PdfWriter writer = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();
                        //spin through all chapters in document
                        int chapterCount = 0;
                        if (chapterID == 0)
                        {
                            //create pdf from document
                            foreach (var chapter in document.getChapters())
                            {
                                chapterCount++;
                                //get html for current chapter
                                string html = "<html>";
                                html += chapter.Text;
                                html += "<br />";
                                html += docBLL.createSummaryFromText(chapter.Text, true);
                                html = fixClosingP(html);
                                html = fixImageTags(html);
                                html = fixTableRowBackgroundTags(html);
                                html = fixSpaces(html);
                                html += "</html>";
                                using (StringReader sr = new StringReader(html))
                                {
                                    try
                                    {
                                        //Create a style sheet
                                        var htmlContext = new HtmlPipelineContext(null);
                                        htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());
                                        ICSSResolver cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(true);
                                        IPipeline pipeline = new CssResolverPipeline(cssResolver, new HtmlPipeline(htmlContext, new PdfWriterPipeline(doc, writer)));
                                        var worker = new XMLWorker(pipeline, true);
                                        var xmlParse = new XMLParser(true, worker);
                                        xmlParse.Parse(sr);
                                        xmlParse.Flush();
                                        //add page break at the end of each chapter
                                        doc.NewPage();
                                    }
                                    catch (Exception ex)
                                    {
                                        var error = chapterCount + " " + ex.Message;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //create pdf from single chapter 
                            DocumentsBLL bll = new DocumentsBLL(this.ProjectID);
                            var chapter = bll.getChapterByID(chapterID);
                            string html = "<html>";
                            html += chapter.Text;
                            html += "<br />";
                            html += docBLL.createSummaryFromText(chapter.Text, true);
                            html = fixClosingP(html);
                            html = fixImageTags(html);
                            html = fixTableRowBackgroundTags(html);
                            html = fixSpaces(html);
                            html += "</html>";
                            using (StringReader sr = new StringReader(html))
                            {
                                try
                                {
                                    //Create a style sheet
                                    var htmlContext = new HtmlPipelineContext(null);
                                    htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());
                                    ICSSResolver cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(true);
                                    IPipeline pipeline = new CssResolverPipeline(cssResolver, new HtmlPipeline(htmlContext, new PdfWriterPipeline(doc, writer)));
                                    var worker = new XMLWorker(pipeline, true);
                                    var xmlParse = new XMLParser(true, worker);
                                    xmlParse.Parse(sr);
                                    xmlParse.Flush();
                                    //add page break at the end of each chapter
                                    doc.NewPage();
                                }
                                catch (Exception ex)
                                {
                                    var error = chapterCount + " " + ex.Message;
                                }
                            }
                        }
                        doc.Close();
                    }
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// create pdf for report
        /// </summary>
        /// <param name="report">Report</param>
        /// <returns>byte[]</returns>
        public byte[] createReportPDF(Report report)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Document doc = new Document(PageSize.LETTER))
                {
                    using (PdfWriter writer = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();
                        Chunk c = new Chunk(report.Title, FontFactory.GetFont("HELVETICA", 18));
                        Paragraph p = new Paragraph();
                        p.Add(c);
                        doc.Add(p);
                        p = new Paragraph();
                        p.SpacingAfter = 15f;
                        doc.Add(p);
                        doc.Add(new Paragraph(""));
                        p = new Paragraph();
                        c = new Chunk(report.Content, FontFactory.GetFont("HELVETICA", 14));
                        p.Add(c);
                        doc.Add(p);
                        doc.Close();
                    }
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// create pdf for report that contains only BR Summary
        /// </summary>
        /// <param name="report">Report</param>
        /// <returns>byte[]</returns>
        public byte[] createSummaryReportPDF(Report report)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Document doc = new Document(PageSize.LETTER))
                {
                    using (PdfWriter writer = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();
                        DocumentsBLL bll = new DocumentsBLL(this.ProjectID);
                        string html = "<html>";
                        html += "<h2 style=\"margin-left: 0px;\">";
                        html += report.Title;
                        html += "</h2>";
                        //html += docBLL.createSummaryFromText(report.Content);
                        html += report.Content;
                        html = fixClosingP(html);
                        html = fixImageTags(html);
                        html = fixTableRowBackgroundTags(html);
                        html = fixSpaces(html);
                        html = removeSummaryTitle(html);
                        html += "</html>";
                        using (StringReader sr = new StringReader(html))
                        {
                            try
                            {
                                //Create a style sheet
                                var htmlContext = new HtmlPipelineContext(null);
                                htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());
                                ICSSResolver cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(true);
                                IPipeline pipeline = new CssResolverPipeline(cssResolver, new HtmlPipeline(htmlContext, new PdfWriterPipeline(doc, writer)));
                                var worker = new XMLWorker(pipeline, true);
                                var xmlParse = new XMLParser(true, worker);
                                xmlParse.Parse(sr);
                                xmlParse.Flush();
                            }
                            catch (Exception ex)
                            {
                                var error = ex.Message;
                            }
                        }
                        doc.Close();
                    }
                }
                return ms.ToArray();
            }
        }

        #endregion public methods

        #region private methods

        private string removeSummaryTitle(string html)
        {
            string updatedHTML = html;
            updatedHTML = html.Replace("<br /><div id=\"BRCSMSummary\"><p align=\"center\"><b>Business Rule and CSM Summary</b></p><br /><br />", "");
            return updatedHTML;
        }

        /// <summary>
        /// adds proper code after closing paragraph
        /// </summary>
        /// <param name="html">string</param>
        /// <returns>string</returns>
        private string fixClosingP(string html) {
            string updatedHTML;
            updatedHTML = html.Replace("</p>", "</p><br />");
            updatedHTML = updatedHTML.Replace("</h1>", "</h1><br />");
            updatedHTML = updatedHTML.Replace("</h2>", "</h2><br />");
            updatedHTML = updatedHTML.Replace("</h3>", "</h3><br />");
            updatedHTML = updatedHTML.Replace("</h4>", "</h4><br />");
            updatedHTML = updatedHTML.Replace("</h5>", "</h5><br />");
            return updatedHTML;
        }
        /// <summary>
        /// fix image tags
        /// </summary>
        /// <param name="html">string</param>
        /// <returns>string</returns>
        private string fixImageTags(string html)
        {
            string updatedHTML = html;
            bool moreImageTags = true;
            int startIndex = 0;
            int endIndex = 0;
            int iWidth = 0;
            int iHeight = 0;
            string imageSRC = "";
            int imageID = 0;
            DocumentsBLL bll = new DocumentsBLL(this.ProjectID);
            startIndex = html.IndexOf("<img");
            if (startIndex > -1)
            {
                moreImageTags = true;
                while (moreImageTags)
                {
                    moreImageTags = false;
                    startIndex = html.IndexOf("<img", endIndex);
                    if (startIndex > -1 && startIndex != endIndex )
                    {
                        imageSRC = "";
                        imageID = 0;
                        moreImageTags = true;
                        int tagEndIndex = html.IndexOf("/>", startIndex);
                        tagEndIndex += 2;
                        string imageTag = html.Substring(startIndex, tagEndIndex - startIndex);
                        //parse image width
                        int widthStartIndex = imageTag.IndexOf("width=");
                        int widthEndIndex = imageTag.IndexOf("/>");
                        if (widthStartIndex > 0 && widthEndIndex > 0)
                        {
                            widthStartIndex += 6;
                            string width = imageTag.Substring(widthStartIndex, widthEndIndex - widthStartIndex);
                            width = width.Replace("\"", "");
                            width = width.Replace("\\","");
                            bool parseWidthResult = int.TryParse(width.Trim(), out iWidth);
                        }
                        //parse image height
                        int heightStartIndex = imageTag.IndexOf("height=");
                        int heightEndIndex = imageTag.IndexOf("src=");
                        if (heightStartIndex > 0 && heightEndIndex > 0)
                        {
                            heightStartIndex += 7;
                            string height = imageTag.Substring(heightStartIndex, heightEndIndex - heightStartIndex);
                            height = height.Replace("\"", "");
                            height = height.Replace("\\", "");
                            bool parseHeightResult = int.TryParse(height.Trim(), out iHeight);
                        }
                        //parse image source
                        int srcStartIndex = imageTag.IndexOf("src=\"");
                        if (srcStartIndex > -1)
                        {
                            srcStartIndex += 5;
                            int srcEndIndex = imageTag.IndexOf("\"", srcStartIndex + 1);
                            imageSRC = imageTag.Substring(srcStartIndex, srcEndIndex - srcStartIndex);
                        }
                        //pull imageID from tag                        
                        int idStartIndex = imageSRC.IndexOf("id=");
                        if (idStartIndex > -1)
                        {
                            idStartIndex += 3;
                            int idEndIndex = imageSRC.IndexOf("&", idStartIndex + 1);
                            string strImageID = imageSRC.Substring(idStartIndex, idEndIndex - idStartIndex);
                            bool parseResult = int.TryParse(strImageID, out imageID);
                            if (parseResult)
                            {
                                //get image path/filename
                                string fileName = bll.getImageFileNameByID(imageID);
                                //store image filename/id pair in hash
                                if (!imageHash.Contains(fileName))
                                {
                                    imageHash.Add(fileName, imageID);
                                }
                                //create new image tag
                                string newImageTag = getNewImageTag(fileName, imageID, iHeight, iWidth);
                                //store image info
                                var newImageInfo = new ImageInfo(imageID, iWidth, iHeight, fileName);
                                imageInfoList.Add(newImageInfo);
                                //replace image tag with new
                                updatedHTML = updatedHTML.Replace(imageTag, newImageTag);
                            }
                        }
                        startIndex = tagEndIndex;
                        endIndex = tagEndIndex + 1;
                    }
                }
            }
            return updatedHTML;
        }

        private string fixTableRowBackgroundTags(string html)
        {
            string updatedHTML = html;
            bool moreBackgroundTags = true;
            string originalBGTag = "";
            string newBGTag = "";
            string color = "";
            int startIndex = 0;
            int endIndex = 0;
            while (moreBackgroundTags)
            {
                moreBackgroundTags = false;
                startIndex = html.IndexOf("bgcolor=\"", endIndex);
                if (startIndex > -1 && startIndex != endIndex)
                {
                    moreBackgroundTags = true;
                    int subIndex = startIndex + 9;
                    endIndex = html.IndexOf('\"', subIndex);
                    color = html.Substring(subIndex, endIndex - subIndex);
                    originalBGTag = html.Substring(startIndex, (endIndex + 1) - startIndex);
                    newBGTag = "style=\"background-color:" + color + ";\"";
                    //replace background tag info in html
                    updatedHTML = updatedHTML.Replace(originalBGTag, newBGTag);
                    startIndex = endIndex;
                    endIndex++;

                }
            }
            return updatedHTML;
        }

        private string fixSpaces(string html)
        {
            string updatedHTML = html;
            updatedHTML = updatedHTML.Replace("&nbsp;", " ");
            return updatedHTML;
        }

        private string getNewImageTag(string filename, int imageID, float imageHeight, float imageWidth)
        {
            applicationBLL appBLL = new applicationBLL();
            string workingDir = appBLL.GetEnvData("WorkingDirectory");
            string documentImagesPath = appBLL.GetEnvData("DocumentImagePath");
            StringBuilder newTag = new StringBuilder();
            newTag.Append("<img id=\"" + imageID + "\" src=\"");
            newTag.Append(workingDir);
            newTag.Append("/");
            newTag.Append(appBLL.getProjectByID(this.ProjectID).Name);
            newTag.Append(documentImagesPath);
            newTag.Append(filename);
            newTag.Append("\" ");
            if (imageHeight > 0)
            {
                newTag.Append("height=\"");
                newTag.Append(imageHeight);
                newTag.Append("\" ");
            }
            if (imageWidth > 0)
            {
                newTag.Append("width=\"");
                newTag.Append(imageWidth);
                newTag.Append("\" ");
            }
            newTag.Append("/>");
            appBLL = null;
            return newTag.ToString();
        }

        #endregion private methods
    }
}