using System;
using System.Collections.Generic;
using System.Linq;

namespace Entities
{
    /// <summary>
    /// class to hold code for AdvancedSearch
    /// </summary>
    public class SearchEntities
    {
        const int MAX_ROWS = 4;
        /// <summary>
        /// class variable to store list of search row data
        /// </summary>
        public List<SearchRow> searchList;
        /// <summary>
        /// class variable to store list of advanced search results
        /// </summary>
        public List<AdvancedSearchResult> resultList;

        /// <summary>
        /// constructor for advanced search
        /// </summary>
        public SearchEntities()
        {
            searchList = new List<SearchRow>();
            resultList = new List<AdvancedSearchResult>();
        }

        /// <summary>
        /// add a search row
        /// </summary>
        /// <param name="sr">SearchRow</param>
        public void addSearchRow(SearchRow sr)
        {
            if (searchList.Count < MAX_ROWS)
            {
                this.searchList.Add(sr);
            }
        }
    }

    /// <summary>
    /// transport object for holding results of basic search
    /// </summary>
    public class BasicSearchResult : IComparable<BasicSearchResult>
    {
        /// <summary>
        /// class variable to store value of display search item ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable to store value of display priority
        /// </summary>
        public int DisplayPriority { get; set; }

        /// <summary>
        /// constructor for BasicSearchResult
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="displayPriority">int</param>
        public BasicSearchResult(int id, int displayPriority)
        {
            this.ID = id;
            this.DisplayPriority = displayPriority;
        }

        /// <summary>
        /// compares display search items
        /// </summary>
        /// <param name="other">BasicSearchResult</param>
        /// <returns>Int32</returns>
        public Int32 CompareTo(BasicSearchResult other)
        {
            return DisplayPriority.CompareTo(other.DisplayPriority);
        }
    }

    /// <summary>
    /// transport object for holding result of advanced search
    /// </summary>
    public class AdvancedSearchResult
    {
        /// <summary>
        /// class variable to store value of row index
        /// </summary>
        public int RowIndex { get; set; }
        /// <summary>
        /// class variable to store list of search results
        /// </summary>
        public List<int> ResultList { get; set; }

        /// <summary>
        /// empty constructor for AdvancedSearchResult
        /// </summary>
        public AdvancedSearchResult()
        {
            this.RowIndex = -1;
            this.ResultList = new List<int>();
        }

        /// <summary>
        /// constructor for AdvancedSearchResult with two parameters
        /// </summary>
        /// <param name="rowIndex">int</param>
        /// <param name="resultList">List&lt;int&gt;</param>
        public AdvancedSearchResult(int rowIndex, List<int> resultList)
        {
            this.RowIndex = rowIndex;
            this.ResultList = resultList;
        }
    }

    /// <summary>
    /// class to hold code for SearchRow
    /// </summary>
    public class SearchRow
    {
        /// <summary>
        /// class variable to store search object
        /// </summary>
        public string SearchObjectID { get; set; }
        /// <summary>
        /// class variable to store operator
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// class variable to store search value
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// class variable to store connector
        /// </summary>
        public string Connector { get; set; }

        /// <summary>
        /// constructor for SearchRow
        /// </summary>
        /// <param name="searchObjectID">string</param>
        /// <param name="op">string</param>
        /// <param name="value">string</param>
        /// <param name="connector">string</param>
        public SearchRow(string searchObjectID, string op, string value, string connector) 
        {
            this.SearchObjectID = searchObjectID;
            this.Operator = op;
            this.Value = value;
            this.Connector = connector;
        }
    }
    
    /// <summary>
    /// class to hold code for SearchObject
    /// </summary>
    public class SearchObject {
        /// <summary>
        /// class variable to store search object name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// class variable to store search object value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// constructor for SearchObject
        /// </summary>
        /// <param name="name">string</param>
        /// <param name="value">string</param>
        public SearchObject(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

    }

    /// <summary>
    /// transport object to hold list items in advanced search ui
    /// </summary>
    public class DDLValueItem
    {
        /// <summary>
        /// class variable to store ID of DDL item
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable to store text of DDL item
        /// </summary>
        public string TEXT { get; set; }

        /// <summary>
        /// empty constructor for DDLValueItem 
        /// </summary>
        public DDLValueItem()
        {

        }

        /// <summary>
        /// constructor for DDLValueItem with two parameters
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="text">string</param>
        public DDLValueItem(int id, string text)
        {
            this.ID = id;
            this.TEXT = text;
        }
    }

    /// <summary>
    /// class to hold code for ClientSearchData
    /// </summary>
    [Serializable]
    public class ClientSearchData
    {
        //var searchData = [ 
        //{ "objectID" : "1" },
        //{ "operatorID" : "2" },
        //{ "textValue" : "3" },
        //{ "calendarValue" : "4" },
        //{ "ddlValue" : "5" },
        //{ "connector" : "6" }
        //];
        /// <summary>
        /// class variable to store value of object ID
        /// </summary>
        public string objectID { get; set; }
        /// <summary>
        /// class variable to store value of operator ID
        /// </summary>
        public string operatorID { get; set; }
        /// <summary>
        /// class variable to store value of text 
        /// </summary>
        public string textValue { get; set; }
        /// <summary>
        /// class variable to store value of calendar
        /// </summary>
        public string calendarValue { get; set; }
        /// <summary>
        /// class variable to store value of the drop down list
        /// </summary>
        public string ddlValue { get; set; }
        /// <summary>
        /// class variable to store value of connector
        /// </summary>
        public string connector { get; set; }

        /// <summary>
        /// empty constructor for ClientSearchData
        /// </summary>
        public ClientSearchData()
        {

        }
    }
    
    /// <summary>
    /// class to hold code for Helper
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// UNFINISHED
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector)
        {
            var knownKeys = new HashSet<TKey>();
            return source.Where(element => knownKeys.Add(keySelector(element)));
        }
    }

}
