using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace DAL
{    
    /// <summary>
    /// class to hold code for CustomerServiceMessagesDAL
    /// </summary>
    public class CustomerServiceMessagesDAL
    {
        #region properties and variables

        /// <summary>
        /// an ID int to identify a particular project
        /// </summary>
        public int ProjectID { get; set; }
        applicationDAL palinoiaDAL;

        #endregion properties and variables

        #region constructors
                
        /// <summary>
        /// constructor with one param
        /// </summary>
        /// <param name="projectID">int</param>
        public CustomerServiceMessagesDAL(int projectID)
        {
            if (projectID == 0)
            {
                projectID = 1;
            }
            this.ProjectID = projectID;
            palinoiaDAL = new applicationDAL(projectID);
        }

        #endregion constructors

        #region customer service messages

        /// <summary>
        /// has customer service message
        /// </summary>
        /// <param name="sectionID">int</param>
        /// <returns>bool</returns>
        public bool hasCSM(int sectionID)
        {
            bool hasCSM = false;
            using (var context = palinoiaDAL.getContextForProject())
            {
                var csm = context.CustomerServiceMessage_Sections
                                .FirstOrDefault((c) => c.fk_SectionID == (long)sectionID);
                if (csm != null)
                {
                    hasCSM = true;
                }
            }
            return hasCSM;
        }
                
        /// <summary>
        /// fetches a list of all customer service messages from the database
        /// </summary>
        /// <returns>List&lt;viewCustomerServiceMessage&gt;</returns>
        public List<viewCustomerServiceMessage> getAllCSMs()
        {
            var vCSMList = new List<viewCustomerServiceMessage>();
            using (var context = palinoiaDAL.getContextForProject())
            {
                var customerServiceMessages = context.CustomerServiceMessages
                                                .OrderBy((c) => c.Name);
                foreach (CustomerServiceMessage csm in customerServiceMessages)
                {
                    var vCSM = new viewCustomerServiceMessage((int)csm.ID, csm.Name, 
                                                              (int)csm.fk_StatusID, 
                                                              (int)csm.fk_CSMTypeID, 
                                                              (int)csm.fk_CSMResponseTypeID, 
                                                              csm.Text, 
                                                              (int)csm.fk_SectionID, 
                                                              (bool)csm.Active,
                                                              (int)csm.UpdatedBy);
                    vCSMList.Add(vCSM);
                }
            }
            return vCSMList;
        }
                
        /// <summary>
        /// fetches a customer service message from the database by ID
        /// </summary>
        /// <param name="csmID">int</param>
        /// <returns>viewCustomerServiceMessage</returns>
        public viewCustomerServiceMessage getCSMByID(int csmID)
        {
            viewCustomerServiceMessage viewCSM;
            using (var context = palinoiaDAL.getContextForProject())
            {
                var entityCSM = context.CustomerServiceMessages
                                          .First((c) => c.ID == csmID);
                viewCSM = new viewCustomerServiceMessage((int)entityCSM.ID, 
                                                         entityCSM.Name, 
                                                         (int)entityCSM.fk_StatusID, 
                                                         (int)entityCSM.fk_CSMTypeID, 
                                                         (int)entityCSM.fk_CSMResponseTypeID, 
                                                         entityCSM.Text, 
                                                         (int)entityCSM.fk_SectionID, 
                                                         (bool)entityCSM.Active,
                                                         (int)entityCSM.UpdatedBy);
                return viewCSM;
            }
        }
                
        /// <summary>
        /// get all customer service messages by section id
        /// </summary>
        /// <param name="sectionID">int</param>
        /// <returns>List&lt;viewCustomerServiceMessages&gt;</returns>
        public List<viewCustomerServiceMessage> getAllCSMsBySection(int sectionID)
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                List<viewCustomerServiceMessage> csmList = new List<viewCustomerServiceMessage>();
                var CSMs = context.CustomerServiceMessages
                    .Where((csm) => csm.fk_SectionID == sectionID).OrderBy((csm) => csm.Name);
                foreach (var csm in CSMs)
                {
                    csmList.Add(new viewCustomerServiceMessage(
                                                                (int)csm.ID, 
                                                                csm.Name, 
                                                                (int)csm.fk_StatusID, 
                                                                (int)csm.fk_CSMTypeID, 
                                                                (int)csm.fk_CSMResponseTypeID, 
                                                                csm.Text, 
                                                                (int)csm.fk_SectionID, 
                                                                (bool)csm.Active,
                                                                (int)csm.UpdatedBy)
                                                                );

                }
                return csmList;
            }
        }
                
        /// <summary>
        /// adds a new csm record to the database
        /// </summary>
        /// <param name="vCSM">viewCustomerServiceMessage</param>
        /// <returns>string</returns>
        public string addCSM(viewCustomerServiceMessage vCSM)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    context.Connection.Open();
                    CustomerServiceMessage newCSM = CustomerServiceMessage.CreateCustomerServiceMessage(0, 
                                                        vCSM.Name, 
                                                        vCSM.StatusID, 
                                                        vCSM.CSMTypeID, 
                                                        vCSM.CSMResponse_TypeID, 
                                                        vCSM.Text,
                                                        vCSM.SectionID,
                                                        vCSM.Active,
                                                        vCSM.UpdatedBy);
                    //newBR.lkup_Status.Text = vBusinessRule.Status;
                    newCSM.Active = vCSM.Active;
                    context.CustomerServiceMessages.AddObject(newCSM);
                    //delete relationship between csm and section?
                    var deleteCSMSection = context.CustomerServiceMessage_Sections.FirstOrDefault((csm) => csm.ID == vCSM.ID);
                    if (deleteCSMSection != null)
                    {
                        context.CustomerServiceMessage_Sections.DeleteObject(deleteCSMSection);
                    }
                    //add business rule section relationship
                    var csmSection = CustomerServiceMessage_Sections.CreateCustomerServiceMessage_Sections(0, vCSM.ID, vCSM.SectionID);
                    context.CustomerServiceMessage_Sections.AddObject(csmSection);
                    context.SaveChanges();
                    result = "OK";
                }

                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, vCSM.UpdatedBy);
                }
                return result;
            }
        }
                
        /// <summary>
        /// update a customer service message record in the database
        /// </summary>
        /// <param name="vCSM">viewCustomerServiceMessage</param>
        /// <returns>string</returns>
        public string updateCSM(viewCustomerServiceMessage vCSM)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var editCSM = context.CustomerServiceMessages.First((br) => br.ID == vCSM.ID);
                    editCSM.Name = vCSM.Name;
                    editCSM.Text = vCSM.Text;
                    editCSM.fk_StatusID = vCSM.StatusID;
                    editCSM.fk_CSMResponseTypeID = vCSM.CSMResponse_TypeID;
                    editCSM.fk_CSMTypeID = vCSM.CSMTypeID;
                    editCSM.fk_SectionID = vCSM.SectionID;
                    editCSM.Active = vCSM.Active;
                    //delete relationship between csm and section?
                    var deleteCSMSection = context.CustomerServiceMessage_Sections.FirstOrDefault((csm) => csm.fk_CustomerServiceMessageID == vCSM.ID);
                    if (deleteCSMSection != null)
                    {
                        context.CustomerServiceMessage_Sections.DeleteObject(deleteCSMSection);
                    }
                    //add csm section relationship
                    var csmSection = CustomerServiceMessage_Sections.CreateCustomerServiceMessage_Sections(0, vCSM.ID, vCSM.SectionID);
                    context.CustomerServiceMessage_Sections.AddObject(csmSection);
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, vCSM.UpdatedBy);
                }
            }
            return result;
        }
                
        /// <summary>
        /// delete a customer service message record from the database
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteCSM(int deleteID, int userID)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var deleteCSM = context.CustomerServiceMessages
                           .First((br) => br.ID == deleteID);
                    context.CustomerServiceMessages.DeleteObject(deleteCSM);
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, userID);
                }

            }
            return result;
        }

        #endregion customer service messages
    }
}
