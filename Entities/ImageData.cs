using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    /// <summary>
    /// class to hold image info
    /// </summary>
    public class ImageData
    {
        /// <summary>
        /// class variable to store image ID
        /// </summary>
        public int ID;//image id
        /// <summary>
        /// class variable to store image filename
        /// </summary>
        public string FileName;//name of image file
        /// <summary>
        /// class variable to store image path
        /// </summary>
        public string FilePath;
        /// <summary>
        /// class variable to store image description
        /// </summary>
        public string Description;//image description
        /// <summary>
        /// class variable to store array of images
        /// </summary>
        public byte[] ByteArray;//array of images
        /// <summary>
        /// class variable to store key of person that updated the image
        /// </summary>
        public int UpdatedBy;//

        /// <summary>
        /// constructor for image data object
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="filename">string</param>
        /// <param name="pathName">string</param>
        /// <param name="description">string</param>
        /// <param name="byteArray">byte[]</param>
        /// <param name="updatedBy">int</param>
        public ImageData(int id, string filename, string pathName, string description, byte[] byteArray, int updatedBy)
        {
            this.ID = id;
            this.FileName = filename;
            this.FilePath = pathName;
            this.ByteArray = byteArray;
            this.Description = description;
            this.UpdatedBy = updatedBy;
        }

        /// <summary>
        /// constructor for image data object 
        /// </summary>
        /// <param name="filename">string</param>
        /// <param name="pathName">string</param>
        /// <param name="description">string</param>
        /// <param name="byteArray">byte[]</param>
        /// <param name="updatedBy">int</param>
        public ImageData(string filename, string pathName, string description, byte[] byteArray, int updatedBy)
        {
            this.ID = 0;
            this.FileName = filename;
            this.FilePath = pathName;
            this.ByteArray = byteArray;
            this.Description = description;
            this.UpdatedBy = updatedBy;
        }
    }

}