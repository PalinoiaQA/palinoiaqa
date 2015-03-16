using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using DAL;


namespace BLL
{    
    /// <summary>
    /// class to hold the code for the admistrative part of the palinoia business logic layer
    /// </summary>
    public class AdminBLL
    {
        #region properties and variables

        AdminDAL dal;
        applicationDAL appDal;
        /// <summary>
        /// Class variable to store current project id
        /// which is required to create a DAL object
        /// </summary>
        public int ProjectID { get; set; }

        #endregion properties and variables

        #region constructors
                
        /// <summary>
        /// constructor with one parameter
        /// </summary>
        /// <param name="projectID">int</param>
        public AdminBLL(int projectID)
        {
            if (projectID == 0)
            {
                projectID = 1;
            }
            this.ProjectID = projectID;
            dal = new AdminDAL(projectID);
            appDal = new applicationDAL();
        }

        #endregion constructors

        #region CSMResponseTypes
                
        /// <summary>
        /// fetch CSM Response Types from database
        /// </summary>
        /// <returns>List&lt;viewCSMResponseType&gt;</returns>
        public List<viewCSMResponseType> getCSMResponseTypes()
        {
            var rtList = dal.getAllCSMResponsesTypes();
            List<viewCSMResponseType> vRTList = new List<viewCSMResponseType>();
            foreach (lkup_CSMResponseType rt in rtList)
            {
                int id = (int)rt.ID;
                string text = rt.Text;
                bool active = rt.Active;
                int updatedBy = (int)rt.UpdatedBy;
                vRTList.Add(new viewCSMResponseType(id, text, active, updatedBy));
            }
            return vRTList;
        }
                
        /// <summary>
        /// fetch Active CSM Response Types from database
        /// </summary>
        /// <returns>List&lt;viewCSMResponseType&gt;</returns>
        public List<viewCSMResponseType> getActiveCSMResponseTypes()
        {
            var rtList = dal.getAllActiveCSMResponsesTypes();
            List<viewCSMResponseType> vRTList = new List<viewCSMResponseType>();
            foreach (lkup_CSMResponseType rt in rtList)
            {
                int id = (int)rt.ID;
                string text = rt.Text;
                bool active = rt.Active;
                int updatedBy = (int)rt.UpdatedBy;
                vRTList.Add(new viewCSMResponseType(id, text, active, updatedBy));
            }
            return vRTList;
        }
                
        /// <summary>
        /// fetch CMS response type from database by ID
        /// </summary>
        /// <param name="responseTypeID">int</param>
        /// <returns>viewCSMResponseType</returns>
        public viewCSMResponseType getCSMResponseTypeByID(int responseTypeID)
        {
            return dal.getCSMResponseTypeByID(responseTypeID);
        }
                
        /// <summary>
        /// add a new CSM Response Types to database 
        /// </summary>
        /// <param name="viewRT">viewCSMResponseType</param>
        /// <returns>string</returns>
        public string addNewCSMResponseType(viewCSMResponseType viewRT)
        {
            var result = dal.addCSMResponseType(viewRT);
            return result;
        }
                
        /// <summary>
        /// delete a CSM Response Types from database
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteCSMResponseType(int deleteID, int userID)
        {
            string result = "";
            //check of status is used in project business rule
            if (dal.CSMResponseTypeUsedInProject(deleteID))
            {
                result = appDal.getCSMByID((int)Enums.csmeEnums.CSM.CSM_RTYP_1_1);
            }
            else
            {
                result = dal.deleteCSMResponseType(deleteID, userID);
            }
            return result;
        }

        /// <summary>
        /// update a CSM Response Types in the database 
        /// </summary>
        /// <param name="editResponseType">viewCSMResponseType</param>
        /// <returns>string</returns>
        public string updateCSMResponseType(viewCSMResponseType editResponseType)
        {
            var result = dal.updateCSMResponseType(editResponseType);
            return result;
        }

        #endregion CMSResponseTypes

        #region CSMTypes
                
        /// <summary>
        /// fetches all CSM types from the database 
        /// </summary> 
        /// <returns>List&lt;viewCSMType&gt;</returns>
        public List<viewCSMType> getAllCSMTypes()
        {
            var viewCSMTypeList = new List<viewCSMType>();
            List<lkup_CSMType> entityCSMTypeList = dal.getAllCSMTypes();
            foreach (var entityCSMType in entityCSMTypeList)
            {
                viewCSMType csmType = new viewCSMType((int)entityCSMType.ID, 
                                                      entityCSMType.Text, 
                                                      (bool)entityCSMType.Active,
                                                      (int)entityCSMType.UpdatedBy);
                viewCSMTypeList.Add(csmType);
            }
            return viewCSMTypeList;
        }
                
        /// <summary>
        /// fetches all active CSM types from the database 
        /// </summary> 
        /// <returns>List&lt;viewCSMType&gt;</returns>
        public List<viewCSMType> getAllActiveCSMTypes()
        {
            var viewCSMTypeList = new List<viewCSMType>();
            List<lkup_CSMType> entityCSMTypeList = dal.getAllActiveCSMTypes();
            foreach (var entityCSMType in entityCSMTypeList)
            {
                viewCSMType csmType = new viewCSMType((int)entityCSMType.ID, 
                                                      entityCSMType.Text, 
                                                      (bool)entityCSMType.Active,
                                                      (int)entityCSMType.UpdatedBy);
                viewCSMTypeList.Add(csmType);
            }
            return viewCSMTypeList;
        }
                
        /// <summary>
        /// fetches CSM types info from database by ID
        /// </summary>
        /// <param name="csmTypeID">int</param>
        /// <returns>viewCSMType</returns>
        public viewCSMType getCSMTypesByID(int csmTypeID)
        {
            return dal.getCSMTypesByID(csmTypeID);
        }
                
        /// <summary>
        /// add a new CSMType to the database
        /// </summary>
        /// <param name="CSMType">viewCSMType</param>
        /// <returns>string</returns>
        public string addNewCSMType(viewCSMType CSMType)
        {
            string result = "";
            result = dal.addCSMType(CSMType);
            return result;
        }
                
        /// <summary>
        /// delete a CSMType from the database
        /// </summary>
        /// <param name="CSMTypeID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteCSMType(int deleteID, int userID)
        {
            string result = "";
            //check of status is used in project business rule
            if (dal.CSMTypeUsedInProject(deleteID))
            {
                result = appDal.getCSMByID((int)Enums.csmeEnums.CSM.CSM_CTYPE_1_1);
            }
            else
            {
                result = dal.deleteCSMResponseType(deleteID, userID);
            }
            return result;
        }
                
        /// <summary>
        /// update a CSMType in the database
        /// </summary>
        /// <param name="CSMType">viewCSMType </param>
        /// <returns>string</returns>
        public string updateCSMType(viewCSMType CSMType)
        {
            string result = dal.updateCSMType(CSMType);
            return result;
        }

        #endregion CSMTypes

        #region status
                
        /// <summary>
        /// fetch all status info from database
        /// </summary>
        /// <returns>List&lt;viewStatus&gt;</returns>
        public List<viewStatus> getAllStatuses()
        {
            List<viewStatus> viewStatusList = new List<viewStatus>();
            List<lkup_Status> entityStatusList = dal.getAllStatuses();
            foreach (var entityStatus in entityStatusList)
            {
                viewStatus status = new viewStatus((int)entityStatus.ID, 
                                                   entityStatus.Text, 
                                                   (bool)entityStatus.Active, 
                                                   entityStatus.Color, 
                                                   (bool)entityStatus.DisplayInChapterSummary,
                                                   (int)entityStatus.UpdatedBy);
                viewStatusList.Add(status);
            }
            return viewStatusList;
        }
                
        /// <summary>
        /// fetches status info from database by ID
        /// </summary>
        /// <param name="statusID">int</param>
        /// <returns>viewStatus</returns>
        public viewStatus getStatusByID(int statusID)
        {
            return dal.getStatusByID(statusID);
        }
                
        /// <summary>
        /// add a new status to the database
        /// </summary>
        /// <param name="status">viewStatus</param>
        /// <returns>string</returns>
        public string addNewStatus(viewStatus status) {
            string result = "";
            result = dal.addStatus(status);
            return result;
        }
                
        /// <summary>
        /// delete a status from the database
        /// </summary>
        /// <param name="statusID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteStatus(int statusID, int userID)
        {
            string result = "";
            //check of status is used in project business rule
            if (dal.StatusUsedInProject(statusID))
            {
                result = appDal.getCSMByID((int)Enums.csmeEnums.CSM.CSM_STAT_1_1);
            }
            else
            {
                result = dal.deleteStatus(statusID, userID);
            }
            return result;
        }
                
        /// <summary>
        /// update a status in the database
        /// </summary>
        /// <param name="status">viewStatus</param>
        /// <returns>string</returns>
        public string updateStatus(viewStatus status)
        {
            string result = dal.updateStatus(status);
            return result;
        }

        #endregion status

        #region sections
                
        /// <summary>
        /// fetches all sections from the database
        /// </summary>
        /// <returns>List&lt;viewSection&gt;</returns>
        public List<viewSection> getAllSections()
        {
            List<viewSection> sectionList = new List<viewSection>();
            List<lkup_Sections> entitySectionList = dal.getAllSections();
            foreach (var entitySection in entitySectionList)
            {
                viewSection section = new viewSection((int)entitySection.ID, 
                                                      entitySection.Text, 
                                                      entitySection.Abbreviation, 
                                                      entitySection.Active,
                                                      (int)entitySection.UpdatedBy);
                sectionList.Add(section);
            }
            return sectionList;
        }
                
        /// <summary>
        /// fetches all active sections from the database
        /// </summary>
        /// <returns>List&lt;viewSection&gt;</returns>
        public List<viewSection> getAllActiveSections()
        {
            List<viewSection> sectionList = new List<viewSection>();
            List<lkup_Sections> entitySectionList = dal.getAllActiveSections();
            foreach (var entitySection in entitySectionList)
            {
                viewSection section = new viewSection((int)entitySection.ID, 
                                                      entitySection.Text, 
                                                      entitySection.Abbreviation, 
                                                      entitySection.Active,
                                                      (int)entitySection.UpdatedBy);
                sectionList.Add(section);
            }
            return sectionList;
        }
                
        /// <summary>
        /// fetches a section from the database by ID
        /// </summary>
        /// <param name="sectionID">int</param>
        /// <returns>viewSection</returns>
        public viewSection getSectionByID(int sectionID)
        {
            return dal.getSectionByID(sectionID);
        }
                
        /// <summary>
        /// adds a section in the database
        /// </summary>
        /// <param name="section">viewSection</param>
        /// <returns>string</returns>
        public string addSection(viewSection section)
        {
            string result = dal.addSection(section);
            return result;
        }
        
        /// <summary>
        /// updates a section in the database
        /// </summary>
        /// <param name="section">viewSection</param>
        /// <returns>string</returns>
        public string updateSection(viewSection section)
        {
            string result = dal.updateSection(section);
            return result;
        }
                
        /// <summary>
        /// deletes a section from the database
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteSection(int deleteID, int userID)
        {
            string result = "";
            //check of status is used in project business rule
            if (dal.SectionUsedInProject(deleteID))
            {
                result = appDal.getCSMByID((int)Enums.csmeEnums.CSM.CSM_SECT_1_1);
            }
            else
            {
                result = dal.deleteSection(deleteID, userID);
            }
            return result;
        }

        #endregion sections

        #region teststeps

        /// <summary>
        /// fetch all test step records from db
        /// </summary>
        /// <returns>List&lt;viewTestStep&gt;</returns>
        public List<viewTestStep> getAllTestSteps()
        {
            List<viewTestStep> tsList = new List<viewTestStep>();
            var entityTestSteps =  dal.getAllTestSteps();
            foreach (var entityTestStep in entityTestSteps)
            {
                tsList.Add(new viewTestStep((int)entityTestStep.ID, entityTestStep.Text, entityTestStep.Active, (int)entityTestStep.UpdatedBy));
            }
            return tsList;
        }

        /// <summary>
        /// delete test step record from db
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteTestStepByID(int deleteID, int userID)
        {
            return dal.deleteTestStepByID(deleteID, userID);
        }
                
        /// <summary>
        /// add a test step in the database 
        /// </summary>
        /// <param name="newTestStep">viewTestStep</param>
        /// <returns>string</returns>
        public string addTestStep(viewTestStep newTestStep)
        {
            return dal.addNewTestStep(newTestStep);
        }
                
        /// <summary>
        /// update a test step in the database
        /// </summary>
        /// <param name="testStep">viewTestStep</param>
        /// <returns>string</returns>
        public string updateTestStep(viewTestStep testStep)
        {
            return dal.updateTestStep(testStep);
        }

        #endregion teststeps

        #region DocumentTypes

        /// <summary>
        /// calls dal method to return list of all entity objects in lkup_DocumentTypes
        /// and converts list to generic list of viewDocumentType objects before returning
        /// </summary>
        /// <returns>List&lt;viewDocumentType&gt;</returns>
        public List<viewDocumentType> getAllDocumentTypes()
        {
            var entityDTList = dal.getAllDocumentTypes();
            List<viewDocumentType> dtList = new List<viewDocumentType>();
            foreach (var dt in entityDTList)
            {
                dtList.Add(new viewDocumentType((int)dt.ID, 
                                                dt.Text, 
                                                (bool)dt.Active, 
                                                (bool)dt.IncludeBRCSMSummaryTable,
                                                (int)dt.UpdatedBy));
            }
            return dtList;
        }

        /// <summary>
        /// calls dal method to return list of all active entity objects in lkup_DocumentTypes
        /// and converts list to generic list of viewDocumentType objects before returning
        /// </summary>
        /// <returns>List&lt;viewDocumentType&gt;</returns>
        public List<viewDocumentType> getAllActiveDocumentTypes()
        {
            var entityDTList = dal.getAllActiveDocumentTypes();
            List<viewDocumentType> dtList = new List<viewDocumentType>();
            foreach (var dt in entityDTList)
            {
                dtList.Add(new viewDocumentType((int)dt.ID, 
                                                dt.Text, 
                                                (bool)dt.Active, 
                                                (bool)dt.IncludeBRCSMSummaryTable,
                                                (int)dt.UpdatedBy));
            }
            return dtList;
        }

        /// <summary>
        /// pass through to call DAL method to delete document type record
        /// in lkup_DocumentTypes by ID
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteDocumentType(int deleteID, int userID)
        {
            return dal.deleteDocumentType(deleteID, userID);
        }

        /// <summary>
        /// pass through to call DAL method to update document type record
        /// by viewDocumentType object
        /// </summary>
        /// <param name="dt">viewDocumentType</param>
        /// <returns>string</returns>
        public string updateDocumentType(viewDocumentType dt)
        {
            return dal.updateDocumentType(dt);
        }

        /// <summary>
        /// pass through to DAL method for adding new lkup_DocumentType
        /// record from a viewDocumentType object
        /// </summary>
        /// <param name="dt">viewDocumentType</param>
        /// <returns>string</returns>
        public string addDocumentType(viewDocumentType dt)
        {
            return dal.addDocumentType(dt);
        }

        /// <summary>
        /// pass through to dal.  returns viewDocumentType object by ID
        /// </summary>
        /// <param name="documentTypeID">int</param>
        /// <returns>viewDocumentType</returns>
        public viewDocumentType getDocumentTypeByID(int documentTypeID)
        {
            return dal.getDocumentTypesByID(documentTypeID);
        }

        #endregion DocumentTypes

        #region Chapter Types

        /// <summary>
        /// pass through to DAL.  Get all Chapter type records from db
        /// </summary>
        /// <returns>List&lt;viewChapterType&gt;</returns>
        public List<viewChapterType> getAllChapterTypes()
        {
            var entityCTList = dal.getAllChapterTypes();
            List<viewChapterType> ctList = new List<viewChapterType>();
            foreach (var ct in entityCTList)
            {
                ctList.Add(new viewChapterType((int)ct.ID, ct.Text, (bool)ct.Active, (int)ct.UpdatedBy));
            }
            return ctList;
        }

        /// <summary>
        /// calls dal method to return list of all active entity objects in lkup_ChapterTypes
        /// and converts list to generic list of viewChapterType objects before returning
        /// </summary>
        /// <returns>List&lt;viewChapterType&gt;</returns>
        public List<viewChapterType> getAllActiveChapterTypes()
        {
            var entityCTList = dal.getAllActiveChapterTypes();
            List<viewChapterType> ctList = new List<viewChapterType>();
            foreach (var ct in entityCTList)
            {
                ctList.Add(new viewChapterType((int)ct.ID, ct.Text, (bool)ct.Active, (int)ct.UpdatedBy));
            }
            return ctList;
        }

        /// <summary>
        /// pass through to call DAL method to delete Chapter type record
        /// in lkup_ChapterTypes by ID
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteChapterType(int deleteID, int userID)
        {
            string result = "";
            //check of status is used in project business rule
            if (dal.ChapterTypeUsedInProject(deleteID))
            {
                result = appDal.getCSMByID((int)Enums.csmeEnums.CSM.CSM_CTYPE_1_1);
            }
            else
            {
                result = dal.deleteChapterType(deleteID, userID);
            }
            return result;
        }

        /// <summary>
        /// pass through to call DAL method to update Chapter type record
        /// by viewChapterType object
        /// </summary>
        /// <param name="ct">viewChapterType</param>
        /// <returns>string</returns>
        public string updateChapterType(viewChapterType ct)
        {
            return dal.updateChapterType(ct);
        }

        /// <summary>
        /// pass through to DAL method for adding new lkup_ChapterType
        /// record from a viewChapterType object
        /// </summary>
        /// <param name="ct">viewChapterType</param>
        /// <returns>string</returns>
        public string addChapterType(viewChapterType ct)
        {
            return dal.addChapterType(ct);
        }

        /// <summary>
        /// pass through to dal.  returns viewChapterType object by ID
        /// </summary>
        /// <param name="chapterTypeID">int</param>
        /// <returns>viewDChapterType</returns>
        public viewChapterType getChapterTypeByID(int chapterTypeID)
        {
            return dal.getChapterTypesByID(chapterTypeID);
        }

        #endregion Chapter Types

        #region defects

        #region defecttype

        /// <summary>
        /// calls dal method to return list of all entity objects in lkup_DefectType
        /// and converts list to generic list of viewDefectType objects before returning
        /// </summary>
        /// <returns>List&lt;viewDefectType&gt;</returns>
        public List<viewDefectType> getAllDefectTypes()
        {
            var entityDTList = dal.getAllDefectTypes();
            List<viewDefectType> dtList = new List<viewDefectType>();
            foreach (var dt in entityDTList)
            {
                dtList.Add(new viewDefectType((int)dt.ID, dt.Text, (bool)dt.Active, (int)dt.UpdatedBy));
            }
            return dtList;
        }

        /// <summary>
        /// calls dal method to return list of all active entity objects in lkup_DefectTypes
        /// and converts list to generic list of viewDefectType objects before returning
        /// </summary>
        /// <returns>List&lt;viewDocumentType&gt;</returns>
        public List<viewDefectType> getAllActiveDefectTypes()
        {
            var entityDTList = dal.getAllActiveDefectTypes();
            List<viewDefectType> dtList = new List<viewDefectType>();
            foreach (var dt in entityDTList)
            {
                dtList.Add(new viewDefectType((int)dt.ID, dt.Text, (bool)dt.Active, (int)dt.UpdatedBy));
            }
            return dtList;
        }

        /// <summary>
        /// pass through to call DAL method to delete DefectType record
        /// in lkup_DefectType by ID
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteDefectType(int deleteID, int userID)
        {
            string result = "";
            //check of status is used in project business rule
            if (dal.DefectTypeUsedInProject(deleteID))
            {
                result = appDal.getCSMByID((int)Enums.csmeEnums.CSM.CSM_DTYP_1_1);
            }
            else
            {
                result = dal.deleteDefectType(deleteID, userID);
            }
            return result;
        }

        /// <summary>
        /// pass through to call DAL method to update DefectType record
        /// by viewDefectType object
        /// </summary>
        /// <param name="dt">viewDefectType</param>
        /// <returns>string</returns>
        public string updateDefectType(viewDefectType dt)
        {
            return dal.updateDefectType(dt);
        }

        /// <summary>
        /// pass through to DAL method for adding new lkup_DefectType
        /// record from a viewDefectType object
        /// </summary>
        /// <param name="dt">viewDefectType</param>
        /// <returns>string</returns>
        public string addDefectType(viewDefectType dt)
        {
            return dal.addDefectType(dt);
        }

        /// <summary>
        /// pass through to dal.  returns viewDefectType object by ID
        /// </summary>
        /// <param name="defectTypeID">int</param>
        /// <returns>viewDefectType</returns>
        public viewDefectType getDefectTypeByID(int defectTypeID)
        {
            return dal.getDefectTypeByID(defectTypeID);
        }

        #endregion defecttype

        #region defectstatus

        /// <summary>
        /// calls dal method to return list of all entity objects in lkup_DefectStatus
        /// and converts list to generic list of viewDefectStatus objects before returning
        /// </summary>
        /// <returns>List&lt;viewDefectStatus&gt;</returns>
        public List<viewDefectStatus> getAllDefectStatus()
        {
            var entityDSList = dal.getAllDefectStatus();
            List<viewDefectStatus> dsList = new List<viewDefectStatus>();
            foreach (var ds in entityDSList)
            {
                dsList.Add(new viewDefectStatus((int)ds.ID, ds.Text, (bool)ds.Active, (int)ds.UpdatedBy));
            }
            return dsList;
        }

        /// <summary>
        /// calls dal method to return list of all active entity objects in lkup_DefectTypes
        /// and converts list to generic list of viewDefectStatus objects before returning
        /// </summary>
        /// <returns>List&lt;viewDocumentStatus&gt;</returns>
        public List<viewDefectStatus> getAllActiveDefectStatus()
        {
            var entityDSList = dal.getAllActiveDefectStatus();
            List<viewDefectStatus> dsList = new List<viewDefectStatus>();
            foreach (var ds in entityDSList)
            {
                dsList.Add(new viewDefectStatus((int)ds.ID, ds.Text, (bool)ds.Active, (int)ds.UpdatedBy));
            }
            return dsList;
        }

        /// <summary>
        /// pass through to call DAL method to delete DefectStatus record
        /// in lkup_DefectStatus by ID
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteDefectStatus(int deleteID, int userID)
        {
            string result = "";
            if (dal.DefectStatusUsedInProject(deleteID))
            {
                result = appDal.getCSMByID((int)Enums.csmeEnums.CSM.CSM_DEF_1_1);
            }
            else
            {
                result = dal.deleteDefectStatus(deleteID, userID);
            }
            return result;
        }

        /// <summary>
        /// pass through to call DAL method to update DefectStatus record
        /// by viewDefectStatus object
        /// </summary>
        /// <param name="ds">viewDefectStatus</param>
        /// <returns>string</returns>
        public string updateDefectStatus(viewDefectStatus ds)
        {
            return dal.updateDefectStatus(ds);
        }

        /// <summary>
        /// pass through to DAL method for adding new lkup_DefectStatus
        /// record from a viewDefectStatus object
        /// </summary>
        /// <param name="ds">viewDefectStatus</param>
        /// <returns>string</returns>
        public string addDefectStatus(viewDefectStatus ds)
        {
            return dal.addDefectStatus(ds);
        }

        /// <summary>
        /// pass through to dal.  returns viewDefectStatus object by ID
        /// </summary>
        /// <param name="defectStatusID">int</param>
        /// <returns>viewDefectStatus</returns>
        public viewDefectStatus getDefectStatusByID(int defectStatusID)
        {
            return dal.getDefectStatusByID(defectStatusID);
        }

        #endregion defectstatus

        #region defectpriority

        /// <summary>
        /// calls dal method to return list of all entity objects in lkup_DefectPriority
        /// and converts list to generic list of viewDefectPriority objects before returning
        /// </summary>
        /// <returns>List&lt;viewDefectStatus&gt;</returns>
        public List<viewDefectPriority> getAllDefectPriorities()
        {
            var entityDPList = dal.getAllDefectPriorities();
            List<viewDefectPriority> dpList = new List<viewDefectPriority>();
            foreach (var dp in entityDPList)
            {
                dpList.Add(new viewDefectPriority((int)dp.ID, dp.Text, (bool)dp.Active, (int)dp.UpdatedBy, (int)dp.Importance));
            }
            return dpList;
        }

        /// <summary>
        /// calls dal method to return list of all active entity objects in lkup_DefectPriority
        /// and converts list to generic list of viewDefectPriority objects before returning
        /// </summary>
        /// <returns>List&lt;viewDocumentPriority&gt;</returns>
        public List<viewDefectPriority> getAllActiveDefectPriorities()
        {
            var entityDPList = dal.getAllActiveDefectPriorities();
            List<viewDefectPriority> dpList = new List<viewDefectPriority>();
            foreach (var dp in entityDPList)
            {
                dpList.Add(new viewDefectPriority((int)dp.ID, dp.Text, (bool)dp.Active, (int)dp.UpdatedBy, (int)dp.Importance));
            }
            return dpList;
        }

        /// <summary>
        /// pass through to call DAL method to delete DefectPriority record
        /// in lkup_DefectPriority by ID
        /// </summary>
        /// <param name="deleteID">int</param>
        /// <param name="userID">int</param>
        /// <returns>string</returns>
        public string deleteDefectPriority(int deleteID, int userID)
        {
            string result = "";
            if (dal.DefectPriorityUsedInProject(deleteID))
            {
                result = appDal.getCSMByID((int)Enums.csmeEnums.CSM.CSM_DEF_1_1);
            }
            else
            {
                result = dal.deleteDefectPriority(deleteID, userID);
            }
            return result;
        }

        /// <summary>
        /// pass through to call DAL method to update DefectPriority record
        /// by viewDefectPriority object
        /// </summary>
        /// <param name="dp">viewDefectPriority</param>
        /// <returns>string</returns>
        public string updateDefectPriority(viewDefectPriority dp)
        {
            return dal.updateDefectPriority(dp);
        }

        /// <summary>
        /// pass through to DAL method for adding new lkup_DefectPriority
        /// record from a viewDefectPriority object
        /// </summary>
        /// <param name="dp">viewDefectPriority</param>
        /// <returns>string</returns>
        public string addDefectPriority(viewDefectPriority dp)
        {
            return dal.addDefectPriority(dp);
        }

        /// <summary>
        /// pass through to dal.  returns viewDefectPriority object by ID
        /// </summary>
        /// <param name="defectPriorityID">int</param>
        /// <returns>viewDefectPriority</returns>
        public viewDefectPriority getDefectPriorityByID(int defectPriorityID)
        {
            return dal.getDefectPriorityByID(defectPriorityID);
        }

        #endregion defectpriority

        #endregion defects
    }
}
