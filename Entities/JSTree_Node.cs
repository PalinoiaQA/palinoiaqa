using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{    
    /// <summary>
    /// class to hold code for JSTree_Node object
    /// </summary>
    public class JSTree_Node
    {
        
        /// <summary>
        /// class variable to store array of JSTree_Node children
        /// </summary>
        public JSTree_Node[] children;
        /// <summary>
        /// class variable to store JSTree_Node data
        /// </summary>
        public JsTreeNodeData data { get; set; }
        /// <summary>
        /// class variable to store JSTree_Node IdServerUse
        /// </summary>
        public int IdServerUse { get; set; }
        /// <summary>
        /// class variable to store JSTree_Node state
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// class variable to store JSTree_Node attr
        /// </summary>
        public JsTreeAttribute attr;
    }

    /// <summary>
    /// class to hold code for JsTreeNodeData
    /// </summary>
    public class JsTreeNodeData
    {
        /// <summary>
        /// class variable to store title
        /// </summary>
        public string title;
        /// <summary>
        /// class variable to store icon
        /// </summary>
        public string icon;
    }

    /// <summary>
    /// class to hold JsTreeAttributes object
    /// </summary>
    public class JsTreeAttribute
    {
        /// <summary>
        /// class variable to store JsTreeAttribute ID
        /// </summary>
        public string id;
        /// <summary>
        /// class variable to store JsTreeAttribute type
        /// </summary>
        public string type;
        /// <summary>
        /// class variable to store JsTreeAttribute selected status
        /// </summary>
        public bool selected;
        /// <summary>
        /// class variable to store rel
        /// </summary>
        public string rel;
    }

}
