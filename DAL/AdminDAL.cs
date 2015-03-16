using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using Entities;
using DAL;

namespace DAL
{    
    /// <summary>
    /// class to hold the code for the admistrative part of the palinoia data access layer
    /// </summary>
    public class AdminDAL
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
        /// constructor with one parameter
        /// </summary>
        /// <param name="projectID">int</param>
        public AdminDAL(int projectID)
        {
            this.ProjectID = projectID;
            palinoiaDAL = new applicationDAL(projectID);
        }

        #endregion constructors

        #region CSMResponseTypes
                
        /// <summary>
        /// fetch all response types from the database
        /// </summary>
        /// <returns>List&lt;lkup_CSMResponseType&gt;</returns>
        public List<lkup_CSMResponseType> getAllCSMResponsesTypes()
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var csmResponseTypes = context.lkup_CSMResponseType.OrderBy((rt) => rt.Text);
                    List<lkup_CSMResponseType> rtList = csmResponseTypes.ToList<lkup_CSMResponseType>();
                    return rtList;
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.ProjectID, 0);
                    
                }
                return new List<lkup_CSMResponseType>();
            }
        }
                
        /// <summary>
        /// fetch all active response types from the database
        /// </summary>
        /// <returns>List&lt;lkup_CSMResponseType&gt;</returns>
        
        public List<lkup_CSMResponseType> getAllActiveCSMResponsesTypes()
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var csmResponseTypes = context.lkup_CSMResponseType
                                                .Where((rt) => rt.Active == true)
                                                .OrderBy((rt) => rt.Text);
                    List<lkup_CSMResponseType> rtList = csmResponseTypes.ToList<lkup_CSMResponseType>();
                    return rtList;
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.ProjectID, 0);

                }
                return new List<lkup_CSMResponseType>();
            }
        }
                
        /// <summary>
        /// fetch a response type from the database by ID
        /// </summary>
        /// <param name="csmResponseTypeID">int</param>
        /// <returns>viewCSMResponseType</returns>
        public viewCSMResponseType getCSMResponseTypeByID(int csmResponseTypeID)
        {
            viewCSMResponseType responseType = new viewCSMResponseType(0, "", false, 0);
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var entityResponseType = context.lkup_CSMResponseType
                                              .First((rt) => rt.ID == csmResponseTypeID);
                    responseType = new viewCSMResponseType((int)entityResponseType.ID, entityResponseType.Text, entityResponseType.Active, (int)entityResponseType.UpdatedBy);
                    
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.ProjectID, 0);
                }
                return responseType;
            }
        }
                
        /// <summary>
        /// add a response types to the database
        /// </summary>
        /// <param name="viewRT">viewCSMResponseType</param>
        /// <returns>string</returns>
        public string addCSMResponseType(viewCSMResponseType viewRT)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    lkup_CSMResponseType newRT = lkup_CSMResponseType.Createlkup_CSMResponseType(0, viewRT.Text, viewRT.Active, (long)viewRT.UpdatedBy);
                    context.lkup_CSMResponseType.AddObject(newRT);
                    context.SaveChanges();
                    result = newRT.ID.ToString();
                }

                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, viewRT.UpdatedBy);
                }
                return result;
            }
        }
                
        /// <summary>
        /// delete a response types from the database
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteCSMResponseType(int deleteID, int userID)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var deleteCSMResponseType = context.lkup_CSMResponseType
                           .First((rt) => rt.ID == deleteID);
                    context.lkup_CSMResponseType.DeleteObject(deleteCSMResponseType);
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
                
        /// <summary>
        /// update a response types in the database
        /// </summary>
        /// <param name="editResponseType">viewCSMResponseType</param>
        /// <returns>string</returns>
        public string updateCSMResponseType(viewCSMResponseType editResponseType)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var editCSMResponseType = context.lkup_CSMResponseType.First((rt) => rt.ID == editResponseType.ID);
                    editCSMResponseType.Text = editResponseType.Text;
                    editCSMResponseType.Active = editResponseType.Active;
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, editResponseType.UpdatedBy);
                }
            }
            return result;
        }

        public bool CSMResponseTypeUsedInProject(int objID)
        {
            //return true if any customer service messages use this csmType
            bool usedInProject = false;
            using (var context = palinoiaDAL.getContextForProject())
            {
                var entityObj = context.CustomerServiceMessages
                                          .FirstOrDefault((t) => t.fk_CSMResponseTypeID == objID);
                if (entityObj != null)
                {
                    usedInProject = true;
                }
            }
            return usedInProject;
        }

        #endregion CSMResponseTypes

        #region status
                
        /// <summary>
        /// fetch all status types from the database
        /// </summary>
        /// <returns>List&lt;lkup_Status&gt;</returns>
        public List<lkup_Status> getAllStatuses()
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                var sList = context.lkup_Status.OrderBy((st) => st.Text);
                List < lkup_Status > statusList = sList.ToList<lkup_Status>();
                return statusList;
            }
        }
                
        /// <summary>
        /// fetch a status from the database by ID
        /// </summary>
        /// <param name="statusID">int</param>
        /// <returns>viewStatus</returns>
        public viewStatus getStatusByID(int statusID)
        {
            viewStatus status;
            using (var context = palinoiaDAL.getContextForProject())
            {
                var entityStatus = context.lkup_Status
                                          .First((s) => s.ID == statusID);
                status = new viewStatus((int)entityStatus.ID, 
                                        entityStatus.Text, 
                                        (bool)entityStatus.Active, 
                                        entityStatus.Color, 
                                        (bool)entityStatus.DisplayInChapterSummary,
                                        (int)entityStatus.UpdatedBy);
                return status;
            }
        }
                
        /// <summary>
        /// add a new status types to the database
        /// </summary>
        /// <param name="status">viewStatus</param>
        /// <returns>string</returns>
        public string addStatus(viewStatus status)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    lkup_Status newStatus = lkup_Status.Createlkup_Status(status.ID,
                                                                          status.Text,
                                                                          status.Active,
                                                                          status.Color,
                                                                          status.DisplayInChapterSummary,
                                                                          status.UpdatedBy);
                    context.lkup_Status.AddObject(newStatus);
                    context.SaveChanges();
                    result = "OK";
                }

                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, status.UpdatedBy);
                }
                return result;
            }
        }

        
        /// <summary>
        /// delete a status types from the database
        /// </summary>
        /// <param name="statusID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteStatus(int statusID, int userID)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var deleteStatus = context.lkup_Status
                               .First((status) => status.ID == statusID);
                    context.lkup_Status.DeleteObject(deleteStatus);
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, userID);
                }
                return result;
            }
        }
                
        /// <summary>
        /// update a status type in the database
        /// </summary>
        /// <param name="status">viewStatus</param>
        /// <returns>string</returns>
        public string updateStatus(viewStatus status)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var editStatus = context.lkup_Status
                              .First((st) => st.ID == status.ID);
                    editStatus.Text = status.Text;
                    editStatus.Active = status.Active;
                    editStatus.Color = status.Color;
                    editStatus.DisplayInChapterSummary = status.DisplayInChapterSummary;
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, status.UpdatedBy);
                }
                return result;
            }
        }

        /// <summary>
        /// returns true if status has been used for a business rule in a project
        /// </summary>
        /// <param name="statusID"></param>
        /// <returns></returns>
        public bool StatusUsedInProject(int statusID)
        {
            //return true if any business rules use this status
            bool usedInProject = false;
            using (var context = palinoiaDAL.getContextForProject())
            {
                var entityStatus = context.BusinessRules
                                          .FirstOrDefault((s) => s.fk_StatusID == statusID);
                if (entityStatus != null)
                {
                    usedInProject = true;
                }
            }
            return usedInProject;
        }

        #endregion status

        #region CSMType
                
        /// <summary>
        /// fetch all CSM types from the database
        /// </summary>
        /// <returns>List&lt;lkup_CSMType&gt;</returns>
        public List<lkup_CSMType> getAllCSMTypes()
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var csmTypes = context.lkup_CSMType
                                    .OrderBy((t) => t.Text);
                    List<lkup_CSMType> rtList = csmTypes.ToList<lkup_CSMType>();
                    return rtList;
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.ProjectID, 0);
                }
                return new List<lkup_CSMType>();
            }
        }
                
        /// <summary>
        /// fetch all active CSM types from the database
        /// </summary>
        /// <returns>List&lt;lkup_CSMType&gt;</returns>
        public List<lkup_CSMType> getAllActiveCSMTypes()
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var csmTypes = context.lkup_CSMType
                                    .Where ((t) => t.Active == true)
                                    .OrderBy((t) => t.Text);
                    List<lkup_CSMType> rtList = csmTypes.ToList<lkup_CSMType>();
                    return rtList;
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.ProjectID, 0);
                }
                return new List<lkup_CSMType>();
            }
        }
                
        /// <summary>
        /// fetch a CSMType from the database by ID
        /// </summary>
        /// <param name="CSMTypeID">int</param>
        /// <returns>viewCSMType</returns>
        public viewCSMType getCSMTypesByID(int CSMTypeID)
        {
            viewCSMType CSMType;
            using (var context = palinoiaDAL.getContextForProject())
            {
                var entityCSMType = context.lkup_CSMType
                                          .First((s) => s.ID == CSMTypeID);
                CSMType = new viewCSMType((int)entityCSMType.ID, 
                                           entityCSMType.Text, 
                                           (bool)entityCSMType.Active,
                                           (int)entityCSMType.UpdatedBy);
                return CSMType;
            }
        }
                
        /// <summary>
        /// add a new CSMTypes to the database
        /// </summary>
        /// <param name="csmType">viewCSMType</param>
        /// <returns>string</returns>
        public string addCSMType(viewCSMType csmType)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    lkup_CSMType newCSMType = lkup_CSMType.Createlkup_CSMType(csmType.ID,
                                                                                csmType.Text,
                                                                                csmType.Active, 
                                                                                csmType.UpdatedBy);
                    context.lkup_CSMType.AddObject(newCSMType);
                    context.SaveChanges();
                    result = "OK";
                }

                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, csmType.UpdatedBy);
                }
                return result;
            }
        }
                
        /// <summary>
        /// delete a CSMType from the database
        /// </summary>
        /// <param name="csmTypeID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteCSMType(int csmTypeID, int userID)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var deleteCSMType = context.lkup_CSMType
                               .First((csmType) => csmType.ID == csmTypeID);
                    context.lkup_CSMType.DeleteObject(deleteCSMType);
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, userID);
                }
                return result;
            }
        }
                
        /// <summary>
        /// update a CSMType in the database
        /// </summary>
        /// <param name="csmType">viewCSMType</param>
        /// <returns>string</returns>
        public string updateCSMType(viewCSMType csmType)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var editCSMType = context.lkup_CSMType
                              .First((st) => st.ID == csmType.ID);
                    editCSMType.Text = csmType.Text;
                    editCSMType.Active = csmType.Active;
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, csmType.UpdatedBy);
                }
                return result;
            }
        }

        public bool CSMTypeUsedInProject(int csmTypeID)
        {
            //return true if any customer service messages use this csmType
            bool usedInProject = false;
            using (var context = palinoiaDAL.getContextForProject())
            {
                var entityObj = context.CustomerServiceMessages
                                          .FirstOrDefault((t) => t.fk_CSMTypeID == csmTypeID);
                if (entityObj != null)
                {
                    usedInProject = true;
                }
            }
            return usedInProject;
        }
       
        #endregion CSMType

        #region sections
                
        /// <summary>
        /// fetch all sections from the database
        /// </summary>
        /// <returns>List&lt;lkup_Sections&gt;</returns>
        public List<lkup_Sections> getAllSections()
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                List<lkup_Sections> sectionList = new List<lkup_Sections>();
                try
                {
                    //make sure project has sections
                    var count = (from o in context.lkup_Sections
                                 where o.ID > 0 select o).Count();
                    if (count > 0)
                    {
                        var sections = context.lkup_Sections;
                        sectionList = sections.OrderBy((section) => section.Text).ToList<lkup_Sections>();
                    }
                }
                catch (Exception ex)
                {
                    applicationDAL appDAL = new applicationDAL();
                    appDAL.logError(ex, this.ProjectID, 0);
                }
                return sectionList;
            }
        }
                
        /// <summary>
        /// fetch all sections from the database
        /// </summary>
        /// <returns>List&lt;lkup_Sections&gt;</returns>
        public List<lkup_Sections> getAllActiveSections()
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                var sections = context.lkup_Sections;
                var sectionList = new List<lkup_Sections>();
                try
                {
                    if (sections.ToList().Count > 0)
                    {
                        //List<lkup_Sections> sectionList = sections.OrderBy((section) => section.Text).ToList<lkup_Sections>();
                        var activeSections = sections.OrderBy((section) => section.Text).Where((section) => section.Active == true);
                        sectionList = activeSections.ToList<lkup_Sections>();
                    }
                }
                catch (Exception ex)
                {
                    string result = palinoiaDAL.logError(ex, this.ProjectID, 0);
                }
                return sectionList; ;
            }
        }
                
        /// <summary>
        /// fetch a section from the database by ID
        /// </summary>
        /// <param name="sectionID">int</param>
        /// <returns>viewSection</returns>
        public viewSection getSectionByID(int sectionID)
        {
            viewSection section;
            using (var context = palinoiaDAL.getContextForProject())
            {
                var entitySection = context.lkup_Sections
                                          .First((s) => s.ID == sectionID);
                section = new viewSection((int)entitySection.ID, 
                                          entitySection.Text, 
                                          entitySection.Abbreviation, 
                                          (bool)entitySection.Active,
                                          (int)entitySection.UpdatedBy);
                return section;
            }
        }
                
        /// <summary>
        /// add a section to the database
        /// </summary>
        /// <param name="vSection">viewSection</param>
        /// <returns>string</returns>
        public string addSection(viewSection vSection)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    lkup_Sections newSection = lkup_Sections.Createlkup_Sections(0, 
                                                                                vSection.Text,
                                                                                vSection.Abbreviation, 
                                                                                vSection.Active,
                                                                                vSection.UpdatedBy);
                    context.lkup_Sections.AddObject(newSection);
                    context.SaveChanges();
                    result = "OK";
                }

                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, vSection.UpdatedBy);
                }
                return result;
            }
        }
                
        /// <summary>
        /// update a section in the database
        /// </summary>
        /// <param name="vSection">viewSection</param>
        /// <returns>string</returns>
        public string updateSection(viewSection vSection)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var editSection = context.lkup_Sections.First((s) => s.ID == vSection.ID);
                    editSection.Text = vSection.Text;
                    editSection.Abbreviation = vSection.Abbreviation;
                    editSection.Active = vSection.Active;
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, vSection.UpdatedBy);
                }
            }
            return result;
        }
                
        /// <summary>
        /// delete a section from the database
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteSection(int deleteID, int userID)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var deleteSection = context.lkup_Sections
                           .First((s) => s.ID == deleteID);
                    context.lkup_Sections.DeleteObject(deleteSection);
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

        public bool SectionUsedInProject(int objID)
        {
            //return true if any business rules or customer service messages use this csmType
            bool usedInProject = false;
            using (var context = palinoiaDAL.getContextForProject())
            {
                var entityObj1 = context.BusinessRules
                                          .FirstOrDefault((t) => t.fk_SectionID == objID);
                if (entityObj1 != null)
                {
                    usedInProject = true;
                }
                else
                {
                    var entityObj2 = context.CustomerServiceMessage_Sections
                        .First((s) => s.fk_SectionID == objID);
                    if (entityObj2 != null)
                    {
                        usedInProject = true;
                    }
                }
            }
            return usedInProject;
        }

        #endregion sections

        #region TestSteps

        /// <summary>
        /// fetch all test steps
        /// </summary>
        /// <returns></returns>
        public List<TestStep> getAllTestSteps()
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var entityTestSteps = context.TestSteps
                                            .OrderBy((ts) => ts.Text);
                    List<TestStep> tsList = entityTestSteps.ToList<TestStep>();
                    return tsList;
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.ProjectID, 0);
                }

                return new List<TestStep>();
            }
        }

        /// <summary>
        /// delete test step by ID
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteTestStepByID(int deleteID, int userID)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var deleteTestStep = context.TestSteps
                                            .First((ts) => ts.ID == deleteID);
                    context.TestSteps.DeleteObject(deleteTestStep);
                    context.SaveChanges();
                    //delete all test step/business rule relationships
                    var testStepRelationships = from tsbr in context.TestStep_BusinessRules
                                                    where tsbr.fk_TestStepID == deleteID
                                                    select tsbr;
                    foreach (var testStepRelationship in testStepRelationships)
                    {
                        context.TestStep_BusinessRules.DeleteObject(testStepRelationship);
                    }
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
                
        /// <summary>
        /// add test step to a particular test case in the database
        /// </summary>
        /// <param name="ts">viewTestStep</param>
        /// <returns>string</returns>
        public string addNewTestStep(viewTestStep ts)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    TestStep newTestStep = TestStep.CreateTestStep(0, ts.Name, ts.Active, (long)ts.UpdatedBy);
                    newTestStep.Active = ts.Active;
                    context.TestSteps.AddObject(newTestStep);
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, ts.UpdatedBy);
                }
                return result;
            }
        }
                
        /// <summary>
        /// update test steps in the database
        /// </summary>
        /// <param name="vtestStep">viewTestStep</param>
        /// <returns>string</returns>
        public string updateTestStep(viewTestStep vtestStep)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    TestStep testStep = context.TestSteps
                                        .First((ts) => ts.ID == vtestStep.ID);
                    testStep.Text = vtestStep.Name;
                    testStep.Active = vtestStep.Active;
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, vtestStep.UpdatedBy);
                }
                return result;
            }
        }

        #endregion TestSteps

        #region document types

        /// <summary>
        /// returns true if specific DocumentType has been used in a Document for current project
        /// </summary>
        /// <param name="objID">int</param>
        /// <returns>bool</returns>
        public bool DocumentTypeUsedInProject(int objID)
        {
            //return true if any document has objID set as document type
            bool usedInProject = false;
            using (var context = palinoiaDAL.getContextForProject())
            {
                var entityObj1 = context.Documents
                                          .FirstOrDefault((d) => d.fk_DocumentType == (long)objID);
                if (entityObj1 != null)
                {
                    usedInProject = true;
                }
            }
            return usedInProject;
        }

        /// <summary>
        /// return list fo all document type records in lkup_DocumentTypes
        /// </summary>
        /// <returns>List&lt;lkup_DocumentType&gt;</returns>
        public List<lkup_DocumentType> getAllDocumentTypes()
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var docTypes = context.lkup_DocumentType
                                    .OrderBy((dt) => dt.Text);
                    List<lkup_DocumentType> dtList = docTypes.ToList<lkup_DocumentType>();
                    return dtList;
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.ProjectID, 0);
                }
                return new List<lkup_DocumentType>();
            }
        }

        /// <summary>
        /// return generic list of all document types from lkup_DocumentTypes
        /// where Active column is true
        /// </summary>
        /// <returns>List&lt;lkup_DocumentType&gt;</returns>
        public List<lkup_DocumentType> getAllActiveDocumentTypes()
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var docTypes = context.lkup_DocumentType
                                    .Where ((t) => t.Active == true)
                                    .OrderBy((dt) => dt.Text);
                    List<lkup_DocumentType> dtList = docTypes.ToList<lkup_DocumentType>();
                    return dtList;
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.ProjectID, 0);
                }
                return new List<lkup_DocumentType>();
            }
        }

        /// <summary>
        /// deletes document type record from lkup_DocumentTypes by ID
        /// and returns OK if successful, error if not
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteDocumentType(int deleteID, int userID)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var deleteDocType = context.lkup_DocumentType
                               .First((docType) => docType.ID == deleteID);
                    context.lkup_DocumentType.DeleteObject(deleteDocType);
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, userID);
                }
                return result;
            }
        }

        /// <summary>
        /// updates DocumentType record in lkup_DocumentType 
        /// returns Ok if successful or error if not
        /// </summary>
        /// <param name="dt">viewDocumentType</param>
        /// <returns>string</returns>
        public string updateDocumentType(viewDocumentType dt)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var editDocType = context.lkup_DocumentType
                              .First((st) => st.ID == dt.ID);
                    editDocType.Text = dt.Text;
                    editDocType.Active = dt.Active;
                    editDocType.IncludeBRCSMSummaryTable = dt.IncludeBRCSMChapterSummary;
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, dt.UpdatedBy);
                }
                return result;
            }
        }

        /// <summary>
        /// creates new lkup_DocumentType record based on viewDocumentType object
        /// returns OK if successful, error text if not.
        /// </summary>
        /// <param name="docType">viewDocumentType</param>
        /// <returns>string</returns>
        public string addDocumentType(viewDocumentType docType)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    lkup_DocumentType newDocType = lkup_DocumentType.Createlkup_DocumentType(docType.ID, 
                                                                                   docType.Text,
                                                                                   docType.Active,
                                                                                   docType.IncludeBRCSMChapterSummary,
                                                                                   docType.UpdatedBy);
                    context.lkup_DocumentType.AddObject(newDocType);
                    context.SaveChanges();
                    result = "OK";
                }

                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, docType.UpdatedBy);
                }
                return result;
            }
        }
                
        /// <summary>
        /// fetch a DocumentType from the database by ID
        /// </summary>
        /// <param name="DocumentTypeID">int</param>
        /// <returns>viewDocumentType</returns>
        public viewDocumentType getDocumentTypesByID(int DocumentTypeID)
        {
            viewDocumentType documentType;
            using (var context = palinoiaDAL.getContextForProject())
            {
                var entityDocumentType = context.lkup_DocumentType
                                          .First((s) => s.ID == DocumentTypeID);
                documentType = new viewDocumentType((int)entityDocumentType.ID, 
                                                     entityDocumentType.Text, 
                                                     (bool)entityDocumentType.Active,
                                                     (bool)entityDocumentType.IncludeBRCSMSummaryTable,
                                                     (int)entityDocumentType.UpdatedBy);
                return documentType;
            }
        }

        #endregion document types

        #region chapter types

        public bool ChapterTypeUsedInProject(int objID)
        {
            //return true if any business rules or customer service messages use this csmType
            bool usedInProject = false;
            using (var context = palinoiaDAL.getContextForProject())
            {
                var entityObj1 = context.Chapters
                                          .First((c) => c.fk_ChapterTypeID == objID);
                if (entityObj1 != null)
                {
                    usedInProject = true;
                }
            }
            return usedInProject;
        }

        /// <summary>
        /// return list fo all chapter type records in lkup_ChapterTypes
        /// </summary>
        /// <returns>List&lt;lkup_ChapterType&gt;</returns>
        public List<lkup_ChapterType> getAllChapterTypes()
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var chapterTypes = context.lkup_ChapterType
                                    .OrderBy((ct) => ct.Text);
                    List<lkup_ChapterType> ctList = chapterTypes.ToList<lkup_ChapterType>();
                    return ctList;
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.ProjectID, 0);
                }
                return new List<lkup_ChapterType>();
            }
        }

        /// <summary>
        /// return generic list of all Chapter types from lkup_ChapterTypes
        /// where Active column is true
        /// </summary>
        /// <returns>List&lt;lkup_ChapterType&gt;</returns>
        public List<lkup_ChapterType> getAllActiveChapterTypes()
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var chapterTypes = context.lkup_ChapterType
                                    .Where((t) => t.Active == true)
                                    .OrderBy((ct) => ct.Text);
                    List<lkup_ChapterType> ctList = chapterTypes.ToList<lkup_ChapterType>();
                    return ctList;
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.ProjectID, 0);

                }
                return new List<lkup_ChapterType>();
            }
        }

        /// <summary>
        /// deletes Chapter type record from lkup_ChapterTypes by ID
        /// and returns OK if successful, error if not
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteChapterType(int deleteID, int userID)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var deleteChapterType = context.lkup_ChapterType
                               .First((chapterType) => chapterType.ID == deleteID);
                    context.lkup_ChapterType.DeleteObject(deleteChapterType);
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, userID);
                }
                return result;
            }
        }

        /// <summary>
        /// updates ChapterType record in lkup_ChapterType 
        /// returns Ok if successful or error if not
        /// </summary>
        /// <param name="chapterType">viewChapterType</param>
        /// <returns>string</returns>
        public string updateChapterType(viewChapterType chapterType)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var editChapterType = context.lkup_ChapterType
                              .First((ct) => ct.ID == chapterType.ID);
                    editChapterType.Text = chapterType.Text;
                    editChapterType.Active = chapterType.Active;
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, chapterType.UpdatedBy);
                }
                return result;
            }
        }

        /// <summary>
        /// creates new lkup_ChapterType record based on viewChapterType object
        /// returns OK if successful, error text if not.
        /// </summary>
        /// <param name="chapterType">viewChapterType</param>
        /// <returns>string</returns>
        public string addChapterType(viewChapterType chapterType)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    lkup_ChapterType newChapterType = lkup_ChapterType.Createlkup_ChapterType(chapterType.ID,
                                                                                   chapterType.Text,
                                                                                   chapterType.Active,
                                                                                   chapterType.UpdatedBy);
                    context.lkup_ChapterType.AddObject(newChapterType);
                    context.SaveChanges();
                    result = newChapterType.ID.ToString();
                }

                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, chapterType.UpdatedBy);
                }
                return result;
            }
        }
                
        /// <summary>
        /// fetch a ChapterType from the database by ID
        /// </summary>
        /// <param name="chapterTypeID">int</param>
        /// <returns>viewChapterType</returns>
        public viewChapterType getChapterTypesByID(int chapterTypeID)
        {
            viewChapterType chapterType;
            using (var context = palinoiaDAL.getContextForProject())
            {
                var entityChapterType = context.lkup_ChapterType
                                          .First((s) => s.ID == chapterTypeID);
                chapterType = new viewChapterType((int)entityChapterType.ID,
                                                   entityChapterType.Text,
                                                   (bool)entityChapterType.Active,
                                                   (int)entityChapterType.UpdatedBy);
                return chapterType;
            }
        }

        #endregion chapter types

        #region Defects

        #region DefectStatus

        /// <summary>
        /// return list fo all defect status records in lkup_DefectStatus
        /// </summary>
        /// <returns>List&lt;lkup_DefectStatus&gt;</returns>
        public List<lkup_DefectStatus> getAllDefectStatus()
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var defectStatus = context.lkup_DefectStatus
                                    .OrderBy((ds) => ds.Text);
                    List<lkup_DefectStatus> dsList = defectStatus.ToList<lkup_DefectStatus>();
                    return dsList;
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.ProjectID, 0);
                }
                return new List<lkup_DefectStatus>();
            }
        }

        /// <summary>
        /// return generic list of all defect status records from lkup_DefectStatus
        /// where Active column is true
        /// </summary>
        /// <returns>List&lt;lkup_DefectStatus&gt;</returns>
        public List<lkup_DefectStatus> getAllActiveDefectStatus()
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var defectStatus = context.lkup_DefectStatus
                                    .Where((t) => t.Active == true)
                                    .OrderBy((ds) => ds.Text);
                    List<lkup_DefectStatus> dsList = defectStatus.ToList<lkup_DefectStatus>();
                    return dsList;
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.ProjectID, 0);
                }
                return new List<lkup_DefectStatus>();
            }
        }

        /// <summary>
        /// creates new lkup_DefectStatus record based on viewDefectStatus object
        /// returns OK if successful, error text if not.
        /// </summary>
        /// <param name="defectStatus">viewDefectStatus</param>
        /// <returns>string</returns>
        public string addDefectStatus(viewDefectStatus defectStatus)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    lkup_DefectStatus newDefectStatus = lkup_DefectStatus.Createlkup_DefectStatus(defectStatus.ID,
                                                                                   defectStatus.Text,
                                                                                   defectStatus.Active,
                                                                                   defectStatus.UpdatedBy);
                    context.lkup_DefectStatus.AddObject(newDefectStatus);
                    context.SaveChanges();
                    result = "OK";
                }

                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, defectStatus.UpdatedBy);
                }
                return result;
            }
        }

        /// <summary>
        /// updates DefectStatus record in lkup_DefectStatus 
        /// returns Ok if successful or error if not
        /// </summary>
        /// <param name="ds">viewDefectStatus</param>
        /// <returns>string</returns>
        public string updateDefectStatus(viewDefectStatus ds)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var editDefectStatus = context.lkup_DefectStatus
                              .First((st) => st.ID == ds.ID);
                    editDefectStatus.Text = ds.Text;
                    editDefectStatus.Active = ds.Active;
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, ds.UpdatedBy);
                }
                return result;
            }
        }

        /// <summary>
        /// deletes document type record from lkup_DocumentTypes by ID
        /// and returns OK if successful, error if not
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteDefectStatus(int deleteID, int userID)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var deleteDefectStatus = context.lkup_DefectStatus
                               .First((ds) => ds.ID == deleteID);
                    context.lkup_DefectStatus.DeleteObject(deleteDefectStatus);
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, userID);
                }
                return result;
            }
        }
                
        /// <summary>
        /// fetch a DefectStatus from the database by ID
        /// </summary>
        /// <param name="defectStatusID">int</param>
        /// <returns>viewDefectStatus</returns>
        public viewDefectStatus getDefectStatusByID(int defectStatusID)
        {
            viewDefectStatus defectStatus;
            using (var context = palinoiaDAL.getContextForProject())
            {
                var entityDefectStatus = context.lkup_DefectStatus
                                          .FirstOrDefault((p) => p.ID == defectStatusID);
                defectStatus = new viewDefectStatus((int)entityDefectStatus.ID,
                                                     entityDefectStatus.Text,
                                                     (bool)entityDefectStatus.Active,
                                                     (int)entityDefectStatus.UpdatedBy);
                return defectStatus;
            }
        }

        /// <summary>
        /// returns true if any defects in db have this defect status assigned
        /// </summary>
        /// <param name="objID"></param>
        /// <returns>bool</returns>
        public bool DefectStatusUsedInProject(int objID)
        {
            //return true if any defects are assigned this defect status
            bool usedInProject = false;
            using (var context = palinoiaDAL.getContextForProject())
            {
                var entityObj = context.Defects
                                          .FirstOrDefault((d) => d.fk_DefectStatusID == objID);
                if (entityObj != null)
                {
                    usedInProject = true;
                }
            }
            return usedInProject;
        }

        #endregion DefectStatus

        #region DefectType

        /// <summary>
        /// return list fo all defect type records in lkup_DefectType
        /// </summary>
        /// <returns>List&lt;lkup_DefectType&gt;</returns>
        public List<lkup_DefectType> getAllDefectTypes()
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var defectTypes = context.lkup_DefectType
                                    .OrderBy((dt) => dt.Text);
                    List<lkup_DefectType> dtList = defectTypes.ToList<lkup_DefectType>();
                    return dtList;
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.ProjectID, 0);
                }
                return new List<lkup_DefectType>();
            }
        }

        /// <summary>
        /// return generic list of all defect types from lkup_DefectTypes
        /// where Active column is true
        /// </summary>
        /// <returns>List&lt;lkup_DefectType&gt;</returns>
        public List<lkup_DefectType> getAllActiveDefectTypes()
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var defectTypes = context.lkup_DefectType
                                    .Where((t) => t.Active == true)
                                    .OrderBy((dt) => dt.Text);
                    List<lkup_DefectType> dtList = defectTypes.ToList<lkup_DefectType>();
                    return dtList;
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.ProjectID, 0);
                }
                return new List<lkup_DefectType>();
            }
        }

        /// <summary>
        /// deletes Defect type record from lkup_DefectTypes by ID
        /// and returns OK if successful, error if not
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteDefectType(int deleteID, int userID)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var deleteDefectType = context.lkup_DefectType
                               .First((docType) => docType.ID == deleteID);
                    context.lkup_DefectType.DeleteObject(deleteDefectType);
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, userID);
                }
                return result;
            }
        }

        /// <summary>
        /// updates DefectType record in lkup_DefectType 
        /// returns Ok if successful or error if not
        /// </summary>
        /// <param name="dt">viewDefectType</param>
        /// <returns>string</returns>
        public string updateDefectType(viewDefectType dt)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var editDefectType = context.lkup_DefectType
                              .First((st) => st.ID == dt.ID);
                    editDefectType.Text = dt.Text;
                    editDefectType.Active = dt.Active;
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, dt.UpdatedBy);
                }
                return result;
            }
        }

        /// <summary>
        /// creates new lkup_DefectType record based on viewDefectType object
        /// returns OK if successful, error text if not.
        /// </summary>
        /// <param name="dt">viewDocumentType</param>
        /// <returns>string</returns>
        public string addDefectType(viewDefectType dt)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    lkup_DefectType newDefectType = lkup_DefectType.Createlkup_DefectType(dt.ID,
                                                                                   dt.Text,
                                                                                   dt.Active,
                                                                                   dt.UpdatedBy);
                    context.lkup_DefectType.AddObject(newDefectType);
                    context.SaveChanges();
                    result = "OK";
                }

                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, dt.UpdatedBy);
                }
                return result;
            }
        }
                
        /// <summary>
        /// fetch a DefectType from the database by ID
        /// </summary>
        /// <param name="defectTypeID">int</param>
        /// <returns>viewDefectType</returns>
        public viewDefectType getDefectTypeByID(int defectTypeID)
        {
            viewDefectType defectType;
            using (var context = palinoiaDAL.getContextForProject())
            {
                var entityDefectType = context.lkup_DefectType
                                          .First((p) => p.ID == defectTypeID);
                defectType = new viewDefectType((int)entityDefectType.ID,
                                                     entityDefectType.Text,
                                                     (bool)entityDefectType.Active,
                                                     (int)entityDefectType.UpdatedBy);
                return defectType;
            }
        }

        /// <summary>
        /// returns true if any defects in db have this defect type assigned
        /// </summary>
        /// <param name="objID"></param>
        /// <returns>bool</returns>
        public bool DefectTypeUsedInProject(int objID)
        {
            //return true if any defects are assigned this defect type
            bool usedInProject = false;
            using (var context = palinoiaDAL.getContextForProject())
            {
                var entityObj = context.Defects
                                          .FirstOrDefault((d) => d.fk_DefectTypeID == objID);
                if (entityObj != null)
                {
                    usedInProject = true;
                }
            }
            return usedInProject;
        }

        #endregion DefectType

        #region DefectPriority

        /// <summary>
        /// return list fo all DefectPriority type records in lkup_DefectPriority
        /// </summary>
        /// <returns>List&lt;lkup_DefectPriority&gt;</returns>
        public List<lkup_DefectPriority> getAllDefectPriorities()
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var defectPriority = context.lkup_DefectPriority
                                    .OrderBy((dp) => dp.Text);
                    List<lkup_DefectPriority> dpList = defectPriority.ToList<lkup_DefectPriority>();
                    return dpList;
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.ProjectID, 0);
                }
                return new List<lkup_DefectPriority>();
            }
        }

        /// <summary>
        /// return generic list of all DefectPriority records from lkup_DefectPriority
        /// where Active column is true
        /// </summary>
        /// <returns>List&lt;lkup_DefectPriority&gt;</returns>
        public List<lkup_DefectPriority> getAllActiveDefectPriorities()
        {
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var defectPriorities = context.lkup_DefectPriority
                                    .Where((p) => p.Active == true)
                                    .OrderBy((dp) => dp.Text);
                    List<lkup_DefectPriority> dpList = defectPriorities.ToList<lkup_DefectPriority>();
                    return dpList;
                }
                catch (Exception ex)
                {
                    palinoiaDAL.logError(ex, this.ProjectID, 0);
                }
                return new List<lkup_DefectPriority>();
            }
        }

        /// <summary>
        /// deletes DefectPriority record from lkup_DefectPriority by ID
        /// and returns OK if successful, error if not
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteDefectPriority(int deleteID, int userID)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var deleteDefectPriority = context.lkup_DefectPriority
                               .First((dp) => dp.ID == deleteID);
                    context.lkup_DefectPriority.DeleteObject(deleteDefectPriority);
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, userID);
                }
                return result;
            }
        }

        /// <summary>
        /// updates DefectPriority record in lkup_DefectPriority 
        /// returns Ok if successful or error if not
        /// </summary>
        /// <param name="dp">viewDefectPriority</param>
        /// <returns>string</returns>
        public string updateDefectPriority(viewDefectPriority dp)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    var editDefectPriority = context.lkup_DefectPriority
                              .First((p) => p.ID == dp.ID);
                    editDefectPriority.Text = dp.Text;
                    editDefectPriority.Active = dp.Active;
                    context.SaveChanges();
                    result = "OK";
                }
                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, dp.UpdatedBy);
                }
                return result;
            }
        }

        /// <summary>
        /// creates new lkup_DefectPriority record based on viewDefectPriority object
        /// returns OK if successful, error text if not.
        /// </summary>
        /// <param name="dp">viewDocumentType</param>
        /// <returns>string</returns>
        public string addDefectPriority(viewDefectPriority dp)
        {
            string result = "";
            using (var context = palinoiaDAL.getContextForProject())
            {
                try
                {
                    lkup_DefectPriority newDefectPriority = lkup_DefectPriority.Createlkup_DefectPriority(dp.ID,
                                                                                   dp.Text,
                                                                                   dp.Active,
                                                                                   dp.UpdatedBy,
                                                                                   dp.Importance);
                    context.lkup_DefectPriority.AddObject(newDefectPriority);
                    context.SaveChanges();
                    result = "OK";
                }

                catch (Exception ex)
                {
                    result = palinoiaDAL.logError(ex, this.ProjectID, dp.UpdatedBy);
                }
                return result;
            }
        }
                
        /// <summary>
        /// fetch a DefectPriority from the database by ID
        /// </summary>
        /// <param name="defectPriorityID">int</param>
        /// <returns>viewDefectPriority</returns>
        public viewDefectPriority getDefectPriorityByID(int defectPriorityID)
        {
            viewDefectPriority defectPriority;
            using (var context = palinoiaDAL.getContextForProject())
            {
                var entityDefectPriority = context.lkup_DefectPriority
                                          .First((p) => p.ID == defectPriorityID);
                defectPriority = new viewDefectPriority((int)entityDefectPriority.ID,
                                                        entityDefectPriority.Text,
                                                        (bool)entityDefectPriority.Active,
                                                        (int)entityDefectPriority.UpdatedBy,
                                                        (int)entityDefectPriority.Importance);
                return defectPriority;
            }
        }

        /// <summary>
        /// returns true if any defects in db have this defect priority assigned
        /// </summary>
        /// <param name="objID"></param>
        /// <returns>bool</returns>
        public bool DefectPriorityUsedInProject(int objID)
        {
            //return true if any defects are assigned this defect status
            bool usedInProject = false;
            using (var context = palinoiaDAL.getContextForProject())
            {
                var entityObj = context.Defects
                                          .FirstOrDefault((d) => d.fk_DefectPriorityID == objID);
                if (entityObj != null)
                {
                    usedInProject = true;
                }
            }
            return usedInProject;
        }

        #endregion DefectPriority

        #endregion Defects

        #region private methods



        #endregion private methods
    }
}
