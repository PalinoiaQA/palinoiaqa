using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{    
    /// <summary>
    /// class to hold code for viewCustomerServiceMessage object
    /// </summary>
    public class viewCustomerServiceMessage
    {
        /// <summary>
        /// class variable to store customer service message ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable to store customer service message name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// class variable to store customer service message status ID
        /// </summary>
        public int StatusID { get; set; }
        /// <summary>
        /// class variable to store customer service message type ID
        /// </summary>
        public int CSMTypeID { get; set; }
        /// <summary>
        /// class variable to store customer service message response type ID
        /// </summary>
        public int CSMResponse_TypeID { get; set; }
        /// <summary>
        /// class variable to store customer service message text
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// class variable to store customer service message active status
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// class variable to store customer service message section ID
        /// </summary>
        public int SectionID { get; set; }
        /// <summary>
        /// class variable to store key to person updating customer service message 
        /// </summary>
        public int UpdatedBy { get; set; }
                
        /// <summary>
        /// constructor for viewCustomerServiceMessage object
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="name">string</param>
        /// <param name="statusID">int</param>
        /// <param name="csmTypeID">string</param>
        /// <param name="csmResponseTypeID">int</param>
        /// <param name="text">string</param>
        /// <param name="sectionID">int</param>
        /// <param name="active">bool</param>
        /// <param name="updatedBy">int</param>
        public viewCustomerServiceMessage(int id, 
                                          string name, 
                                          int statusID, 
                                          int csmTypeID, 
                                          int csmResponseTypeID, 
                                          string text, 
                                          int sectionID, 
                                          bool active,
                                          int updatedBy)
        {
            this.ID = id;
            this.Name = name;
            this.StatusID = statusID;
            this.CSMTypeID = csmTypeID;
            this.CSMResponse_TypeID = csmResponseTypeID;
            this.Text = text;
            this.SectionID = sectionID;
            this.Active = active;
            this.UpdatedBy = updatedBy;
        }
    }
}
