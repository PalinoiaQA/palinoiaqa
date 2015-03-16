using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    /// <summary>
    /// class to hold ImageInfo information
    /// </summary>
    public class ImageInfo
    {
        /// <summary>
        /// class variable to store image ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable to store image width
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// class variable to store image height
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// class variable to store image fuilename
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// constructor with four parameters
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="width">int</param>
        /// <param name="height">int</param>
        /// <param name="fileName">string</param>
        public ImageInfo(int id, int width, int height, string fileName)
        {
            this.ID = id;
            this.Width = width;
            this.Height = height;
            this.FileName = fileName;
        }
    }
}
