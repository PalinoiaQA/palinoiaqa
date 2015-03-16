using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using Entities;
using Enums;

namespace BLL
{
    /// <summary>
    /// class to hold code for SearchBLL
    /// </summary>
    public class SearchBLL
    {
        #region properties and variables

        SearchDAL dal;
        /// <summary>
        /// class variable to store value of ProjectID
        /// </summary>
        public int ProjectID { get; set; }

        #endregion

        #region constructors

        /// <summary>
        /// constructor for SearchBLL
        /// </summary>
        /// <param name="projectID">int</param>
        public SearchBLL(int projectID)
        {
            this.ProjectID = projectID;
            dal = new SearchDAL(this.ProjectID);
        }

        #endregion 

        #region public methods

        /// <summary>
        /// perform basic document search
        /// </summary>
        /// <param name="searchValue">string</param>
        /// <param name="searchObjectType">int</param>
        /// <returns>DocumentSearchResult</returns>
        public DocumentSearchResult doBasicDocumentSearch(string searchValue, int searchObjectType)
        {
            var docDAL = new DocumentsDAL(this.ProjectID);
            var searchObjects = dal.getSearchObjectsForTypeID(searchObjectType);
            StringBuilder sql = new StringBuilder();
            string searchTable = this.getTableFromSearchTypeid(searchObjectType);
            List<string> documentIDList = new List<string>();
            List<string> chapterIDList = new List<string>();
            foreach (var obj in searchObjects)
            {
                if(obj.Text.Equals("Chapter Text") || obj.Text.Equals("Chapter Title")) {
                    sql.Clear();
                    sql.Append("SELECT ID FROM Chapters ");
                    string whereClause = this.getBasicWhereClause(searchValue, obj);
                    sql.Append(whereClause);
                    var chapterResults = dal.executeIDSearch(sql.ToString(), (int)obj.ResultDisplayPriority);
                    foreach (var chapter in chapterResults)
                    {
                        documentIDList.Add(docDAL.getDocumentIDForChapter(chapter.ID).ToString());
                        chapterIDList.Add(chapter.ID.ToString());
                    }
                }
                else
                {
                    //build SQL search statement
                    sql.Clear();
                    sql.Append("SELECT ID FROM ");
                    sql.Append(searchTable);
                    sql.Append(" ");
                    string whereClause = this.getBasicWhereClause(searchValue, obj);
                    sql.Append(whereClause);
                    var simpleResults = dal.executeIDSearch(sql.ToString(), (int)obj.ResultDisplayPriority);
                    foreach (var result in simpleResults)
                    {
                        documentIDList.Add(result.ID.ToString());
                    }
                }
            }
            var docIDList = documentIDList.Distinct().ToList();
            var chapIDList = chapterIDList.Distinct().ToList();
            var searchResult = new DocumentSearchResult(docIDList, chapIDList);
            return searchResult;
        }

        /// <summary>
        /// perform basic testcase search
        /// </summary>
        /// <param name="searchValue">string</param>
        /// <param name="searchObjectType">int</param>
        /// <returns>TestCaseSearchResult</returns>
        public TestCaseSearchResult doBasicTestCaseSearch(string searchValue, int searchObjectType)
        {
            var tcDAL = new TestCasesDAL(this.ProjectID);
            var searchObjects = dal.getSearchObjectsForTypeID(searchObjectType);
            //StringBuilder sql = new StringBuilder();
            string searchTable = this.getTableFromSearchTypeid(searchObjectType);
            List<string> testCaseList = new List<string>();
            List<string> testStepList = new List<string>();
            foreach (var obj in searchObjects)
            {
                StringBuilder sql = new StringBuilder();
                string tableName = "";
                switch (obj.Text)
                {
                    case ("Test Step Notes"):
                    case ("Test Step Name"):
                        sql.Append("SELECT TestSteps.ID FROM TestSteps ");
                        sql.Append("JOIN TestCase_TestSteps ON TestSteps.ID = TestCase_TestSteps.fk_TestStepID");
                        sql.Append(" ");
                        sql.Append(this.getBasicWhereClause(searchValue, obj));
                        var testStepResults = dal.executeIDSearch(sql.ToString(), (int)obj.ResultDisplayPriority);
                        foreach (var ts in testStepResults)
                        {
                            testStepList.Add(ts.ID.ToString());
                        }
                        break;
                    case ("Tested By"):
                    case ("Test Date"):
                    case ("Test Result Notes"):
                        sql.Append("SELECT TestSteps.ID FROM TestSteps ");
                        sql.Append("JOIN TestResults ON TestResults.fk_TestStepID = TestSteps.ID");
                        sql.Append(" ");
                        sql.Append(this.getBasicWhereClause(searchValue, obj));
                        var testStepResults2 = dal.executeIDSearch(sql.ToString(), (int)obj.ResultDisplayPriority);
                        foreach (var ts in testStepResults2)
                        {
                            testStepList.Add(ts.ID.ToString());
                        }
                        break;
                    default:
                        sql.Append("SELECT ID FROM ");
                        tableName = this.getTableFromSearchTypeid(searchObjectType);
                        sql.Append(tableName);
                        sql.Append(" ");
                        string whereClause = this.getBasicWhereClause(searchValue, obj);
                        sql.Append(whereClause);
                        var simpleResults = dal.executeIDSearch(sql.ToString(), (int)obj.ResultDisplayPriority);
                        foreach(var tc in simpleResults)
                        {
                            testCaseList.Add(tc.ID.ToString());
                        }
                        break;
                }
            }
            var tcIDList = testCaseList.Distinct().ToList();
            var tsIDList = testStepList.Distinct().ToList();
            var searchResult = new TestCaseSearchResult(tcIDList, tsIDList);
            return searchResult;
        }

        /// <summary>
        /// perform an advanced search for test cases
        /// </summary>
        /// <param name="advSearch">AdvancedSearch</param>
        /// <param name="searchType">int</param>
        /// <returns>TestCaseSearchResult</returns>
        public TestCaseSearchResult doAdvancedTestCaseSearch(SearchEntities advSearch, int searchType)
        {
            int rowIndex = 0;
            var searchResults = new List<int>();
            List<string> testCaseList = new List<string>();
            List<string> testStepList = new List<string>();
            foreach (var row in advSearch.searchList)
            {
                int objID = 0;
                bool parseResult = int.TryParse(row.SearchObjectID, out objID);
                var searchObject = dal.getSearchObjectByID(objID);
                StringBuilder sql = new StringBuilder();
                AdvancedSearchResult result = new AdvancedSearchResult();
                string tableName = "";
                switch (searchObject.Text)
                {
                    case ("Test Step Notes"):
                    case("Test Step Name"):
                        sql.Append("SELECT TestSteps.ID FROM TestSteps ");
                        sql.Append("JOIN TestCase_TestSteps ON TestSteps.ID == TestCase_TestSteps.fk_TestStepsID");
                        sql.Append(" ");
                        sql.Append(this.getAdvancedWhereClause(row, searchType));
                        result = dal.executeIDSearch(rowIndex, sql.ToString());
                        foreach(var id in result.ResultList) {
                            testStepList.Add(id.ToString());
                        }
                        break;
                    case("Tested By"):
                    case("Test Date"):
                    case("Test Result Notes"):
                        sql.Append("SELECT TestSteps.ID FROM TestSteps ");
                        sql.Append("JOIN TestResults ON TestResults.fk_TestStepID = TestSteps.ID");
                        sql.Append(" ");
                        sql.Append(this.getAdvancedWhereClause(row, searchType));
                        result = dal.executeIDSearch(rowIndex, sql.ToString());
                        foreach (var id in result.ResultList)
                        {
                            testStepList.Add(id.ToString());
                        }
                        break;
                    default:
                        sql.Append("SELECT ID FROM ");
                        tableName = this.getTableFromSearchTypeid(searchType);
                        sql.Append(tableName);
                        sql.Append(" ");
                        sql.Append(this.getAdvancedWhereClause(row, searchType));
                        result = dal.executeIDSearch(rowIndex, sql.ToString());
                        foreach (var id in result.ResultList)
                        {
                            testCaseList.Add(id.ToString());
                        }
                        advSearch.resultList.Add(result);
                        break;
                }
                rowIndex++;
            }
            var tcIDList = testCaseList.Distinct().ToList();
            var tsIDList = testStepList.Distinct().ToList();
            var searchResult = new TestCaseSearchResult(tcIDList, tsIDList);
            return searchResult;
        }

        /// <summary>
        /// perform an advanced search
        /// </summary>
        /// <param name="advSearch">AdvancedSearch</param>
        /// <param name="searchType">int</param>
        /// <returns>List&lt;BasicSearchResult&gt;</returns>
        public DocumentSearchResult doAdvancedDocumentSearch(SearchEntities advSearch, int searchType)
        {
            var docDAL = new DocumentsDAL(this.ProjectID);
            int rowIndex = 0;
            var searchResults = new List<int>();
            StringBuilder sql = new StringBuilder();
            List<string> documentIDList = new List<string>();
            List<string> chapterIDList = new List<string>();
            foreach (var row in advSearch.searchList)
            {
                //check if search objects is "Comments" - special search required for defect comments
                int objID = 0;
                bool parseResult = int.TryParse(row.SearchObjectID, out objID);
                var searchObject = dal.getSearchObjectByID(objID);
                if (searchObject.Text.Equals("Chapter Text") || searchObject.Text.Equals("Chapter Title"))
                {
                    sql.Clear();
                    sql.Append("SELECT ID FROM Chapters ");
                    string whereClause = this.getBasicWhereClause(row.Value, searchObject);
                    sql.Append(whereClause);
                    var chapterResults = dal.executeIDSearch(sql.ToString(), (int)searchObject.ResultDisplayPriority);
                    foreach (var chapter in chapterResults)
                    {
                        documentIDList.Add(docDAL.getDocumentIDForChapter(chapter.ID).ToString());
                        chapterIDList.Add(chapter.ID.ToString());
                    }
                }
                else
                {
                    sql.Clear();
                    sql.Append("SELECT ID FROM ");
                    sql.Append(this.getTableFromSearchTypeid(searchType));
                    sql.Append(" ");
                    sql.Append(this.getAdvancedWhereClause(row, searchType));
                    var simpleResults = dal.executeIDSearch(sql.ToString(), (int)searchObject.ResultDisplayPriority);
                    foreach (var result in simpleResults)
                    {
                        documentIDList.Add(result.ID.ToString());
                    }
                    rowIndex++;
                }
            }
            var docIDList = documentIDList.Distinct().ToList();
            var chapIDList = chapterIDList.Distinct().ToList();
            var searchResult = new DocumentSearchResult(docIDList, chapIDList);
            return searchResult;
        }

        /// <summary>
        /// perform a basic search
        /// </summary>
        /// <param name="searchValue">string</param>
        /// <param name="searchObjectType">int</param>
        /// <returns>List&lt;BasicSearchResult&gt;</returns>
        public List<BasicSearchResult> doBasicSearch(string searchValue, int searchObjectType)
        {
            var searchResults = new List<BasicSearchResult>();
            var commentSearchResults = new List<BasicSearchResult>();
            var searchObjects = dal.getSearchObjectsForTypeID(searchObjectType);
            searchObjects = searchObjects.FindAll((o) => o.fk_SearchDataTypeID == (int)Enums.SearchDataTypeEnums.SearchDataType.String);
            foreach (var obj in searchObjects)
            {
                if(obj.Text.Equals("Comments")) {
                    searchResults.AddRange(dal.executeDefectCommentsSearch(searchValue));
                }
                else {
                StringBuilder sql = new StringBuilder();
                //build SQL search statement
                sql.Append("SELECT ID FROM ");
                sql.Append(this.getTableFromSearchTypeid(searchObjectType));
                sql.Append(" ");
                string whereClause = this.getBasicWhereClause(searchValue, obj);
                sql.Append(whereClause);
                searchResults.AddRange(dal.executeIDSearch(sql.ToString(), (int)obj.ResultDisplayPriority));
                }
            }
            var distinctItems = searchResults.DistinctBy(x => x.ID).ToList<BasicSearchResult>();
            distinctItems.Sort();
            return distinctItems;
        }
        /// <summary>
        /// perform an advanced search
        /// </summary>
        /// <param name="advSearch">AdvancedSearch</param>
        /// <param name="searchType">int</param>
        /// <returns>List&lt;BasicSearchResult&gt;</returns>
        public List<int> doAdvancedSearch(SearchEntities advSearch, int searchType)
        {
            int rowIndex = 0;
            var searchResults = new List<int>();
            foreach (var row in advSearch.searchList)
            {
                //check if search objects is "Comments" - special search required for defect comments
                int objID = 0;
                bool parseResult = int.TryParse(row.SearchObjectID, out objID);
                var searchObject = dal.getSearchObjectByID(objID);
                StringBuilder sql = new StringBuilder();
                AdvancedSearchResult result = new AdvancedSearchResult();
                switch (searchObject.Text)
                {
                    case ("Comments"):
                        var commentSearchResults = dal.executeDefectCommentsSearch(row.Value);
                        var resultIDs = new List<int>();
                        foreach (var res in commentSearchResults)
                        {
                            resultIDs.Add(res.ID);
                        }
                        advSearch.resultList.Add(new AdvancedSearchResult(rowIndex, resultIDs));
                        break;
                    default:
                        sql.Append("SELECT ID FROM ");
                        sql.Append(this.getTableFromSearchTypeid(searchType));
                        sql.Append(" ");
                        sql.Append(this.getAdvancedWhereClause(row, searchType));
                        result = dal.executeIDSearch(rowIndex, sql.ToString());
                        advSearch.resultList.Add(result);
                        break;
                }
                
                rowIndex++;
            }
            searchResults = advSearch.resultList[0].ResultList;
            if (advSearch.resultList.Count > 1)
            {
                for(int i=1;i<advSearch.resultList.Count;i++) 
                {
                    //get comparer
                    var comparer = advSearch.searchList[i-1].Connector;
                    //get results
                    searchResults = this.compareLists(searchResults, advSearch.resultList[i].ResultList, comparer);
                }
            }
            return searchResults;
        }
        /// <summary>
        /// fetch defects for search results
        /// </summary>
        /// <param name="resultList">List&lt;BasicSearchResult&gt;</param>
        /// <returns>List&lt;viewDefect&gt;</returns>
        public List<viewDefect> getDefectsForSearchResults(List<BasicSearchResult> resultList)
        {
            return dal.getDefectsForSearchResults(resultList);
        }
        /// <summary>
        /// returns generic list of viewDefect objects from a 
        /// generic list of ids identified during search
        /// </summary>
        /// <param name="resultList">List&lt;int&gt;</param>
        /// <returns>List&lt;viewDefect&gt;</returns>
        public List<viewDefect> getDefectsForSearchResults(List<int> resultList)
        {
            return dal.getDefectsForSearchResults(resultList);
        }
        /// <summary>
        /// fetch DDL objects for type ID
        /// </summary>
        /// <param name="searchTypeID">int</param>
        /// <returns>List&lt;SearchObject&gt;</returns>
        public List<SearchObject> getDDLSearchObjectsForTypeID(int searchTypeID)
        {
            return dal.getDDLSearchObjectsForTypeID(searchTypeID);
        }
        /// <summary>
        /// fetch data for search object
        /// </summary>
        /// <param name="searchObjectID">int</param>
        /// <returns>int</returns>
        public int getDataTypeForSearchObjectID(int searchObjectID)
        {
            return dal.getDataTypeForSearchObjectID(searchObjectID);
        }
        /// <summary>
        /// returns list of objects to populate the search object DDL in the advanced
        /// search based on a search object id
        /// </summary>
        /// <param name="searchObjectID">int</param>
        /// <returns>List&lt;DDLValueItem&gt;</returns>
        public List<DDLValueItem> getDDLValuesForSearchObject(int searchObjectID)
        {
            return dal.getDDLValuesForSearchObject(searchObjectID);
        }
        /// <summary>
        /// fetch search operators
        /// </summary>
        /// <returns>List&lt;string&gt;</returns>
        public List<DDLValueItem> getSearchOperators()
        {
            return dal.getSearchOperators();
        }

        #endregion public methods

        #region private methods

        /// <summary>
        /// returns table name for search object type id
        /// </summary>
        /// <param name="searchObjectType">int</param>
        /// <returns>string</returns>
        private string getTableFromSearchTypeid(int searchObjectType)
        {
            string tableName = "";
            switch (searchObjectType)
            {
                case((int)Enums.searchObjectTypeEnums.SearchObjectType.BusinessRules):
                    tableName = "BusinessRules";
                    break;
                
                case((int)Enums.searchObjectTypeEnums.SearchObjectType.CustomerServiceMessages):
                    tableName = "CustomerServiceMessages";
                    break;

                case((int)Enums.searchObjectTypeEnums.SearchObjectType.Defects):
                    tableName = "Defects";
                    break;

                case((int)Enums.searchObjectTypeEnums.SearchObjectType.TestCases):
                    tableName = "TestCases";
                    break;

                case((int)Enums.searchObjectTypeEnums.SearchObjectType.Documents):
                    tableName = "Documents";
                    break;
            }
            return tableName;
        }
        /// <summary>
        /// returns single int list based on filtering 2 int lists on AND or OR comparer
        /// </summary>
        /// <param name="list1">List&lt;int&gt;</param>
        /// <param name="list2">List&lt;int&gt;</param>
        /// <param name="comparer">string</param>
        /// <returns>List&lt;int&gt;</returns>
        private List<int> compareLists(List<int> list1, List<int> list2, string comparer)
        {
            //comparer 0 = null
            //comparer 1 = OR
            //comparer 2 = AND
            List<int> returnList = new List<int>();
            switch(comparer) {
                case("0"):
                    returnList = list1;
                    break;
                case("1"):
                    returnList = list1.Concat(list2.Where(x => !list1.Contains(x))).ToList<int>();
                    break;
                case("2"):
                    for (int i = 0; i < list1.Count; i++)
                    {
                        for (int j = 0; j < list2.Count; j++)
                        {
                            if (list1[i] == list2[j])
                            {
                                returnList.Add(list1[i]);
                            }
                        }
                    }
                    break;
                default:
                    returnList = list1;
                    break;
            }
            return returnList;
        }
        /// <summary>
        /// fetch advanced clause
        /// </summary>
        /// <param name="row">SearchRow</param>
        /// <param name="searchObjectType">int</param>
        /// <returns>string</returns>
        private string getAdvancedWhereClause(SearchRow row, int searchObjectType)
        {
            var whereClause = new StringBuilder();
            whereClause.Append("WHERE ");
            int searchObjID = 0;
            bool result = int.TryParse(row.SearchObjectID, out searchObjID);
            var searchObj = dal.getSearchObjectByID(searchObjID);
            whereClause.Append(searchObj.SearchColumn);
            whereClause.Append(this.getOperatorValueSQL(row.Operator, (int)searchObj.fk_SearchDataTypeID, row.Value));
            whereClause.AppendLine();
            return whereClause.ToString();
        }
        /// <summary>
        /// fetch basic where clause
        /// </summary>
        /// <param name="basicItem">string</param>
        /// <param name="obj">lkup_SearchObjects</param>
        /// <returns>string</returns>
        private string getBasicWhereClause(string basicItem, lkup_SearchObjects obj)
        {
            var whereClause = new StringBuilder();
            whereClause.AppendLine("WHERE ");
            whereClause.Append(obj.SearchColumn);
            whereClause.Append(" LIKE('%");
            whereClause.Append(basicItem);
            whereClause.Append("%') ");
            return whereClause.ToString();
        }
        /// <summary>
        /// fetch operator value SQL
        /// </summary>
        /// <param name="operatorID">string</param>
        /// <param name="searchDataType">int</param>
        /// <param name="value">string</param>
        /// <returns>string</returns>
        private string getOperatorValueSQL(string operatorID, int searchDataType, string value)
        {
            StringBuilder operatorSQL = new StringBuilder();
            int opID = 0;
            var result = int.TryParse(operatorID, out opID);
            var strOperator = dal.getSearchOperatorByID(opID);
            switch (searchDataType)
            {
                case((int)Enums.SearchDataTypeEnums.SearchDataType.ID):
                case ((int)Enums.SearchDataTypeEnums.SearchDataType.Integer):
                    switch (strOperator)
                    {
                        case ("="):
                            operatorSQL.Append(strOperator + value);
                            break;
                        case (">"):
                            operatorSQL.Append(strOperator + value);
                            break;
                        case ("<"):
                            operatorSQL.Append(strOperator + value);
                            break;
                        case ("<="):
                            operatorSQL.Append(strOperator + value);
                            break;
                        case (">="):
                            operatorSQL.Append(strOperator + value);
                            break;
                        case ("!="):
                            operatorSQL.Append("<>" + value);
                            break;
                        case ("Contains"):
                            operatorSQL.Append("=" + value);
                            break;
                        case ("Starts with"):
                            operatorSQL.Append("=" + value);
                            break;
                        case ("Ends with"):
                            operatorSQL.Append("=" + value);
                            break;
                    }
                    break;
                case ((int)Enums.SearchDataTypeEnums.SearchDataType.String):
                    switch (strOperator)
                    {
                        case ("="):
                            operatorSQL.Append(strOperator);
                            operatorSQL.Append("'");
                            operatorSQL.Append(value);
                            operatorSQL.Append("'");
                            break;
                        case (">"):
                            operatorSQL.Append(" LIKE ");
                            break;
                        case ("<"):
                            operatorSQL.Append(" LIKE ");
                            break;
                        case ("<="):
                            operatorSQL.Append(" LIKE '");
                            operatorSQL.Append(value);
                            operatorSQL.Append("'");
                            break;
                        case (">="):
                            operatorSQL.Append(" LIKE '");
                            operatorSQL.Append(value);
                            operatorSQL.Append("'");
                            break;
                        case ("!="):
                            operatorSQL.Append("<>'");
                            operatorSQL.Append(value);
                            operatorSQL.Append("'");
                            break;
                        case ("Contains"):
                            operatorSQL.Append(" LIKE '%");
                            operatorSQL.Append(value);
                            operatorSQL.Append("%'");
                            break;
                        case ("Starts with"):
                            operatorSQL.Append(" LIKE '");
                            operatorSQL.Append(value);
                            operatorSQL.Append("%'");
                            break;
                        case ("Ends with"):
                            operatorSQL.Append(" LIKE '%");
                            operatorSQL.Append(value);
                            operatorSQL.Append("'");
                            break;
                    }
                    break;
                case ((int)Enums.SearchDataTypeEnums.SearchDataType.Date):
                    //convert string date to a format used by SQLite
                    string sqliteDate = value;
                    DateTime date = new DateTime();
                    var parseResult = DateTime.TryParse(value, out date);
                    if (parseResult)
                    {
                        sqliteDate = date.ToString("yyyy-MM-dd");
                    }
                    switch (strOperator)
                    {
                        case ("="):
                            operatorSQL.Append(strOperator);
                            operatorSQL.Append("'");
                            operatorSQL.Append(sqliteDate);
                            operatorSQL.Append("'");
                            break;
                        case (">"):
                            operatorSQL.Append(strOperator);
                            operatorSQL.Append("'");
                            operatorSQL.Append(sqliteDate);
                            operatorSQL.Append("'");
                            break;
                        case ("<"):
                            operatorSQL.Append(strOperator);
                            operatorSQL.Append("'");
                            operatorSQL.Append(sqliteDate);
                            operatorSQL.Append("'");
                            break;
                        case ("<="):
                            operatorSQL.Append(strOperator);
                            operatorSQL.Append("'");
                            operatorSQL.Append(sqliteDate);
                            operatorSQL.Append("'");
                            break;
                        case (">="):
                            operatorSQL.Append(strOperator);
                            operatorSQL.Append("'");
                            operatorSQL.Append(sqliteDate);
                            operatorSQL.Append("'");
                            break;
                        case ("!="):
                            operatorSQL.Append("<>'");
                            operatorSQL.Append(sqliteDate);
                            operatorSQL.Append("'");
                            break;
                        case ("Contains"):
                            operatorSQL.Append("='");
                            operatorSQL.Append(sqliteDate);
                            operatorSQL.Append("'");
                            break;
                        case ("Starts with"):
                            operatorSQL.Append("='");
                            operatorSQL.Append(sqliteDate);
                            operatorSQL.Append("'");
                            break;
                        case ("Ends with"):
                            operatorSQL.Append("='");
                            operatorSQL.Append(sqliteDate);
                            operatorSQL.Append("'");
                            break;
                    }
                    break;
                case ((int)Enums.SearchDataTypeEnums.SearchDataType.Bool):
                    switch (strOperator)
                    {
                        case ("="):
                            operatorSQL.Append(strOperator + value);
                            break;
                        case (">"):
                            operatorSQL.Append("<>" + value);
                            break;
                        case ("<"):
                            operatorSQL.Append("<>" + value);
                            break;
                        case ("<="):
                            operatorSQL.Append("=" + value);
                            break;
                        case (">="):
                            operatorSQL.Append("=" + value);
                            break;
                        case ("!="):
                            operatorSQL.Append("<>" + value);
                            break;
                        case ("Contains"):
                            operatorSQL.Append("=" + value);
                            break;
                        case ("Starts with"):
                            operatorSQL.Append("=" + value);
                            break;
                        case ("Ends with"):
                            operatorSQL.Append("=" + value);
                            break;
                    }
                    break;
            }
            return operatorSQL.ToString();
        }
        
        #endregion private methods
    }
}
