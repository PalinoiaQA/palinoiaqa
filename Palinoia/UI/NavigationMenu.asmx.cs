using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using Entities;
using System.Web.Script.Services;
using BLL;

namespace Palinoia
{
    /// <summary>
    /// Web service that returns nodes int JSON format used 
    /// to populate the navigation JSTree on client.  Returned 
    /// nodes are based on user features.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class NavigationMenu : System.Web.Services.WebService
    {
        #region web methods

        /// <summary>
        /// fetch navigation URL
        /// </summary>
        /// <param name="id">string</param>
        /// <returns>string</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetNavigationURL(string id)
        {
            int navID = Convert.ToInt32(id);
            var bll = new applicationBLL();
            NavigationMenuItem item = bll.getMenuItemByID(navID);
            //var url = item.URL.Replace("~", "");
            applicationBLL appBLL = new applicationBLL();
            if (appBLL.isDevelopmentMode())
            {
                item.URL = item.URL.Replace("/Palinoia", "");
            }
            var url = VirtualPathUtility.ToAbsolute(item.URL);
            appBLL = null;
            return url;
        }
                
        /// <summary>
        /// fetches test cases from database for tree
        /// </summary>
        /// <param name="nodeID">int</param>
        /// <param name="userID">string</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<JSTree_Node> GetNavigationMenuItems(string nodeID, string userID)
        {
            int usrID = Convert.ToInt32(userID);
            List<JSTree_Node> nodes = new List<JSTree_Node>();
            if (!nodeID.Equals("0")) // user is attempting to open a node in the tree
            {
                var idArray = nodeID.Split('_');
                var objectAbbv = idArray[0];
                int objectID = 0;
                bool result = int.TryParse(idArray[1], out objectID);
                switch (objectAbbv)
                {
                    // only populate children for root nodes
                    case ("ROOT"):
                        nodes = AddChildNodes(objectID, usrID);
                        break;
                }
            }
            else // screen is loading; populate root node with sections
            {
                var bll = new applicationBLL();
                var nodeList = bll.getChildNodesForParentID(1, usrID);
                JSTree_Node rootNode = new JSTree_Node();
                rootNode.data = new JsTreeNodeData { title = "Home" };
                rootNode.state = "open";
                rootNode.IdServerUse = 0;
                rootNode.attr = new JsTreeAttribute { id = "0", selected = false };
                rootNode.children = AddChildNodes(nodeList).ToArray();
                nodes.Add(rootNode);
                bll = null;
            }
            return nodes;
        }

        #endregion web methods

        #region private methods
                
        /// <summary>
        /// add section child nodes to tree root
        /// </summary>
        /// <param name="nodeList">List&lt;NavigationMenuItem&gt;</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        private List<JSTree_Node> AddChildNodes(List<NavigationMenuItem> nodeList)
        {
            List<JSTree_Node> JSTreeArray = new List<JSTree_Node>();
            foreach (var node in nodeList)
            {
                //check if user has feature to view node

                var bll = new applicationBLL();
                var hasTestCases = bll.hasChildNodes(node.ID);
                string nodeID = node.NodeID;
                JSTree_Node jsTreeNode = new JSTree_Node();
                jsTreeNode.data = new JsTreeNodeData { title = node.Name };
                if (hasTestCases)
                {
                    jsTreeNode.state = "closed";
                }
                jsTreeNode.IdServerUse = node.ID;
                jsTreeNode.attr = new JsTreeAttribute { id = nodeID, type = "ITEM", selected = false };
                JSTreeArray.Add(jsTreeNode);
            }
            return JSTreeArray;
        }
                
        /// <summary>
        /// add section child nodes to tree root
        /// </summary>
        /// <param name="nodeID">int</param>
        /// <param name="userID">int</param>
        /// <returns>List&lt;JSTree_Node&gt;</returns>
        private List<JSTree_Node> AddChildNodes(int nodeID, int userID)
        {
            List<JSTree_Node> JSTreeArray = new List<JSTree_Node>();
            var bll = new applicationBLL();
            var nodeList = bll.getChildNodesForParentID(nodeID, userID);
            foreach (var node in nodeList)
            {
                var hasChildren = bll.hasChildNodes(node.ID);
                string nID = node.NodeID;
                JSTree_Node jsTreeNode = new JSTree_Node();
                jsTreeNode.data = new JsTreeNodeData { title = node.Name };
                if (hasChildren)
                {
                    jsTreeNode.state = "closed";
                }
                jsTreeNode.IdServerUse = node.ID;
                jsTreeNode.attr = new JsTreeAttribute { id = nID, type = "ITEM", selected = false };
                JSTreeArray.Add(jsTreeNode);
            }
            return JSTreeArray;
        }

        #endregion private methods
    }
}
