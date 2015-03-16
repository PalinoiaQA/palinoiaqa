using System.IO;
using System.Web;
using BLL;

namespace Palinoia.UI.Documents
{
    /// <summary>
    /// web handler class to stream document images in document editor
    /// </summary>
    public class LoadImage : IHttpHandler
    {
        /// <summary>
        /// process request
        /// </summary>
        /// <param name="context">HttpContext</param>
        public void ProcessRequest(HttpContext context)
        {
            //pull image id and project id from querystring
            const int IMAGENOTFOUNDID = 1;
            int imageID = 0;
            int projectID = 0;
            string strImageID = context.Request.QueryString["id"];
            string strProjectID = context.Request.QueryString["pid"];
            bool result1 = int.TryParse(strImageID, out imageID);
            bool result2 = int.TryParse(strProjectID, out projectID);
            if (result1 && result2)
            {
                var appBLL = new applicationBLL();
                try
                {
                    DocumentsBLL bll = new DocumentsBLL(projectID);
                    string imageFileName = bll.getImageFileNameByID(imageID);
                    context.Response.ContentType = "application/octet-stream";
                    var workingDirectory = appBLL.GetEnvData("WorkingDirectory");
                    var projectName = appBLL.getProjectByID(projectID).Name;
                    var docImagePath = appBLL.GetEnvData("DocumentImagePath");
                    System.Text.StringBuilder imagePath = new System.Text.StringBuilder();
                    imagePath.Append(workingDirectory);
                    imagePath.Append("/");
                    imagePath.Append(projectName);
                    imagePath.Append(docImagePath);
                    imagePath.Append(imageFileName);
                    if (!File.Exists(imagePath.ToString()))
                    {
                        imagePath = new System.Text.StringBuilder();
                        imagePath.Append(workingDirectory);
                        imagePath.Append("/");
                        imagePath.Append(projectName);
                        imagePath.Append(docImagePath);
                        imageFileName = bll.getImageFileNameByID(IMAGENOTFOUNDID);
                        imagePath.Append(imageFileName);
                    }
                    context.Response.ContentType = "application/octet-stream";
                    context.Response.WriteFile(imagePath.ToString());
                }
                catch (System.Exception ex)
                {

                }
                finally
                {
                    appBLL = null;
                }
            }
        }

        /// <summary>
        /// marker for reuseability
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}