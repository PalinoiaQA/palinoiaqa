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
    /// class to hold code for SearchDAL
    /// </summary>
    public class SearchDAL
    {
        #region properties and variables

        /// <summary>
        /// an ID int to identify a particular project
        /// </summary>
        public int ProjectID { get; set; }
        applicationDAL appDAL;

        #endregion properties and variables

        #region constructors
                
        /// <summary>
        /// constructor with one parameter
        /// </summary>
        /// <param name="projectID">int</param>
        public SearchDAL(int projectID)
        {
            this.ProjectID = projectID;
            appDAL = new applicationDAL(projectID);
        }

        #endregion constructors

        #region public methods

        /// <summary>
        /// fetch DDL search object for type ID
        /// </summary>
        /// <param name="searchTypeID">int</param>
        /// <returns>List&lt;SearchObject&gt;</returns>
        public List<SearchObject> getDDLSearchObjectsForTypeID(int searchTypeID)
        {
            var objectList = new List<SearchObject>();
            using (var context = appDAL.getContextForProject())
            {
                try
                {
                    var entitySearchObjects = context.lkup_SearchObjects
                                              .Where((so) => so.fk_SearchTypeID == searchTypeID)
                                              .OrderBy((so) => so.Text);
                    foreach (var obj in entitySearchObjects)
                    {
                        objectList.Add(new SearchObject(obj.Text, obj.ID.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    appDAL.logError(ex, this.ProjectID, 0);
                }
            }
            return objectList;
        }
        /// <summary>
        /// fetch search object for type ID
        /// </summary>
        /// <param name="searchTypeID">int</param>
        /// <returns>List&lt;lkup_SearchObjects&gt;</returns>
        public List<lkup_SearchObjects> getSearchObjectsForTypeID(int searchTypeID)
        {
            var objectList = new List<lkup_SearchObjects>();
            using (var context = appDAL.getContextForProject())
            {
                try
                {
                    var entitySearchObjects = context.lkup_SearchObjects
                                              .Where((so) => so.fk_SearchTypeID == searchTypeID)
                                              .OrderBy((so) => so.ResultDisplayPriority);
                    foreach (var obj in entitySearchObjects)
                    {
                        objectList.Add(obj);
                    }
                }
                catch (Exception ex)
                {
                    appDAL.logError(ex, this.ProjectID, 0);
                }
            }
            return objectList;
        }
        /// <summary>
        /// fetch search object by ID
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>lkup_SearchObjects</returns>
        public lkup_SearchObjects getSearchObjectByID(int id)
        {
            lkup_SearchObjects searchObj = lkup_SearchObjects.Createlkup_SearchObjects(id,0,0,"","");
            using (var context = appDAL.getContextForProject())
            {
                try
                {
                    searchObj = context.lkup_SearchObjects.First((so) => so.ID == id);
                    
                }
                catch (Exception ex)
                {
                    appDAL.logError(ex, this.ProjectID, 0);
                }
            }
            return searchObj;
        }
        /// <summary>
        /// fetch defects for search results
        /// </summary>
        /// <param name="resultList">List&lt;BasicSearchResult&gt;</param>
        /// <returns>List&lt;viewDefect&gt;</returns>
        public List<viewDefect> getDefectsForSearchResults(List<BasicSearchResult> resultList)
        {
            DefectsDAL defectsDAL = new DefectsDAL(this.ProjectID);
            var results = new List<viewDefect>();
            foreach (var result in resultList)
            {
                results.Add(defectsDAL.getDefectbyID(result.ID));
            }
            return results;
        }
        /// <summary>
        /// fetch defects for search results
        /// </summary>
        /// <param name="resultList">List&lt;BasicSearchResult&gt;</param>
        /// <returns>List&lt;viewDefect&gt;</returns>
        public List<viewDefect> getDefectsForSearchResults(List<int> resultList)
        {
            DefectsDAL defectsDAL = new DefectsDAL(this.ProjectID);
            var results = new List<viewDefect>();
            foreach (var result in resultList)
            {
                results.Add(defectsDAL.getDefectbyID(result));
            }
            return results;
        }
        /// <summary>
        /// execute defect comments search
        /// </summary>
        /// <param name="searchValue">string</param>
        /// <returns>List&lt;BasicSearchResult&gt;</returns>
        public List<BasicSearchResult> executeDefectCommentsSearch(string searchValue)
        {
            var result = new List<BasicSearchResult>();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT fk_defectID FROM Defect_Comments WHERE Text LIKE '%");
            sql.Append(searchValue);
            sql.Append("%';");
            result = executeIDSearch(sql.ToString(), 3);
            return result;
        }
        /// <summary>
        /// execute ID search
        /// </summary>
        /// <param name="sql">string</param>
        /// <param name="displayPriority">int</param>
        /// <returns>List&lt;BasicSearchResult&gt;</returns>
        public List<BasicSearchResult> executeIDSearch(string sql, int displayPriority)
        {
            var result = new List<BasicSearchResult>();
            using (var context = appDAL.getContextForProject())
            {
                try
                {
                    var initResult = context.ExecuteStoreQuery<int>(sql).ToList();
                    foreach (var iResult in initResult)
                    {
                        result.Add(new BasicSearchResult(iResult, displayPriority));
                    }
                }
                catch (Exception ex)
                {
                    appDAL.logError(ex, this.ProjectID, 0);
                }
            }
            return result;
        }
        /// <summary>
        /// execute ID search
        /// </summary>
        /// <param name="rowIndex">int</param>
        /// <param name="sql">string</param>
        /// <returns>List&lt;BasicSearchResult&gt;</returns>
        public AdvancedSearchResult executeIDSearch(int rowIndex, string sql)
        {
            AdvancedSearchResult result = new AdvancedSearchResult();
            var idList = new List<int>();
            using (var context = appDAL.getContextForProject())
            {
                try
                {
                    idList = context.ExecuteStoreQuery<int>(sql).ToList();
                    result = new AdvancedSearchResult(rowIndex, idList);
                }
                catch (Exception ex)
                {
                    appDAL.logError(ex, this.ProjectID, 0);
                }
            }
            return result;
        }
        /// <summary>
        /// execute record search
        /// </summary>
        /// <param name="sql">string</param>
        /// <returns>List&lt;DDLValueItem&gt;</returns>
        public List<DDLValueItem> executeRecordSearch(string sql)
        {
            var result = new List<DDLValueItem>();
            using (var context = appDAL.getContextForProject())
            {
                try
                {
                    result = context.ExecuteStoreQuery<DDLValueItem>(sql).ToList();
                }
                catch (Exception ex)
                {
                    appDAL.logError(ex, this.ProjectID, 0);
                }
            }
            return result;
        }
        /// <summary>
        /// fetch data type for search object ID
        /// </summary>
        /// <param name="searchObjectID">int</param>
        /// <returns>int</returns>
        public int getDataTypeForSearchObjectID(int searchObjectID)
        {
            int retVal = 0;
            lkup_SearchObjects searchObj = lkup_SearchObjects.Createlkup_SearchObjects(searchObjectID, 0, 0, "", "");
            using (var context = appDAL.getContextForProject())
            {
                try
                {
                    searchObj = context.lkup_SearchObjects.First((so) => so.ID == searchObjectID);
                    retVal = (int)searchObj.fk_SearchDataTypeID;
                }
                catch (Exception ex)
                {

                }
            }
            return retVal;
        }
        /// <summary>
        /// fetch DDL values for search object
        /// </summary>
        /// <param name="searchObjectID">int</param>
        /// <returns>List&lt;DDLValueItem&gt;</returns>
        public List<DDLValueItem> getDDLValuesForSearchObject(int searchObjectID)
        {
            var itemList = new List<DDLValueItem>();
            string xrefTable;
            using (var context = appDAL.getContextForProject())
            {
                try
                {
                    //get xref table 
                    var searchObj = context.lkup_SearchObjects.First((so) => so.ID == searchObjectID);
                    xrefTable = searchObj.XREF_Table.ToString();
                    StringBuilder sql = new StringBuilder();
                    if (searchObj.XREF_Table.Equals("Users"))
                    {
                        itemList = getUsersForDDL();
                    }
                    else
                    {
                        sql.Append("SELECT ID, TEXT FROM ");
                        sql.Append(xrefTable);
                        sql.Append(" ORDER BY TEXT;");
                        itemList = this.executeRecordSearch(sql.ToString());
                    }
                }
                catch (Exception ex)
                {
                    appDAL.logError(ex, this.ProjectID, 0);
                }
            }
            return itemList;
        }
        /// <summary>
        /// fetch users for DDL
        /// </summary>
        /// <returns>List&lt;DDLValueItem&gt;</returns>
        public List<DDLValueItem> getUsersForDDL()
        {
            List<DDLValueItem> userList = new List<DDLValueItem>();
            var users = appDAL.getAllUsers();
            foreach (var user in users)
            {
                string text = user.getFullNameLNF();
                int id = user.ID;
                userList.Add(new DDLValueItem(id, text));
            }
            return userList;
        }

        //public List<DocumentSearchResult> getDocumentSearchResultForChapterText(string value)
        //{
        //    var searchResults = new List<DocumentSearchResult>();
        //    var docDAL = new DocumentsDAL(this.ProjectID);
        //    var docList = docDAL.getAllDocuments();
        //    foreach (var doc in docList)
        //    {
        //        var chapList = docDAL.getChaptersForDocumentID((int)doc.ID);
        //        var chapterHits = chapList.FindAll((c) => c.Text.

        //    }
        //}

        #region search operators

        /// <summary>
        /// fetch search operators
        /// </summary>
        /// <returns>List&lt;string&gt;</returns>
        public List<DDLValueItem> getSearchOperators()
        {
            List<DDLValueItem> operatorList = new List<DDLValueItem>();
            List<lkup_SearchOperators> soList = new List<lkup_SearchOperators>();
            using (var context = appDAL.getContextForProject())
            {
                try
                {
                    var entitySearchObjects = from so in context.lkup_SearchOperators
                                              select so;
                    soList = entitySearchObjects.ToList<lkup_SearchOperators>();
                    foreach (var op in soList)
                    {
                        operatorList.Add(new DDLValueItem((int)op.ID, op.Text.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    appDAL.logError(ex, this.ProjectID, 0);
                }
                return operatorList;
            }
        }
        /// <summary>
        /// return string value for operator for operator id
        /// </summary>
        /// <param name="searchOperatorID">int</param>
        /// <returns>string</returns>
        public string getSearchOperatorByID(int searchOperatorID)
        {
            string strOperator = "";
            using (var context = appDAL.getContextForProject())
            {
                try
                {
                    var operators = context.lkup_SearchOperators.First((so) => so.ID == searchOperatorID);
                    strOperator = operators.Text;

                }
                catch (Exception ex)
                {

                }
            }
            return strOperator;
        }

        #endregion search operators

        #endregion public methods

        #region private methods
        
        #endregion private methods
    }
}
