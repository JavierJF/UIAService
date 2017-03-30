using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIALib
{
    /// <summary>
    /// This class holds the identification of the 'real' components.
    /// </summary>
    public class CSelectors
    {
        /// <summary>
        /// Field that is created in the moment of instantiation for the component.
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// List of the tree nodes of the UI Automation tree that leads to the node
        /// that provides de reactivity for this component.
        /// </summary>
        public List<STreeNode> path { get; set; }
    }
}
