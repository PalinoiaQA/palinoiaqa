using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using Entities;

namespace BLL
{    
    /// <summary>
    /// class to hold code for CustomerServiceMessagesBLL
    /// </summary>
        public class CustomerServiceMessagesBLL
    {
         #region properties and variables

        /// <summary>
        /// class variable to store project ID
        /// </summary>
        public int ProjectID { get; set; }
        /// <summary>
        /// class variable to store palinoia BLL
        /// </summary>
        public applicationBLL palinoiaBLL;
        /// <summary>
        /// class variable to store dal
        /// </summary>
        public CustomerServiceMessagesDAL dal;

        #endregion properties and variables

        #region constructors
                    
        /// <summary>
        /// constructor for customer service message BLL
        /// </summary>
        /// <param name="projectID">int</param>
        public CustomerServiceMessagesBLL(int projectID)
        {
            dal = new CustomerServiceMessagesDAL(projectID);
            this.ProjectID = projectID;
            this.palinoiaBLL = new applicationBLL();
        }

        #endregion constructors

        #region public methods

        /// <summary>
        /// pass through method from Data logic layer
        /// </summary>
        /// <param name="sectionID">int</param>
        /// <returns>bool</returns>
        public bool hasCSM(int sectionID)
        {
            return dal.hasCSM(sectionID);
        }
                    
        /// <summary>
        /// fetches all customer's service messages from the database
        /// </summary>
        /// <returns>List&lt;viewCustomerServiceMessage&gt;</returns>
        public List<viewCustomerServiceMessage> getAllCSMs()
        {
            return dal.getAllCSMs();
        }
                   
        /// <summary>
        /// fetches customer service message from database by ID
        /// </summary>
        /// <param name="csmID">int</param>
        /// <returns>viewCustomerServiceMessage</returns>
        public viewCustomerServiceMessage getCSMByID(int csmID)
        {
            return dal.getCSMByID(csmID);
        }
                    
        /// <summary>
        /// get all csms by section
        /// </summary>
        /// <param name="sectionID">int</param>
        /// <returns>List&lt;viewBusinessRule&gt;</returns>
        public List<viewCustomerServiceMessage> getAllCSMsBySection(int sectionID)
        {
            return dal.getAllCSMsBySection(sectionID);
        }
                    
        /// <summary>
        /// adds customer service message to database
        /// </summary>
        /// <param name="csm">viewCustomerServiceMessage</param>
        /// <returns>string</returns>
        public string addCSM(viewCustomerServiceMessage csm)
        {
            return dal.addCSM(csm);
        }
                    
        /// <summary>
        /// deletes customer service message from database
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteCSM(int deleteID, int userID)
        {
            return dal.deleteCSM(deleteID, userID);
        }
                    
        /// <summary>
        /// update customer service message in database
        /// </summary>
        /// <param name="csm">viewCustomerServiceMessage</param>
        /// <returns>string</returns>
        public string updateCSM(viewCustomerServiceMessage csm)
        {
            return dal.updateCSM(csm);
        }

        #endregion public methods
    }
}
