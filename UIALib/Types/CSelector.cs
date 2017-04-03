/*
 * The R&D leading to these results received funding from the
 * Department of Education - Grant H421A150005 (GPII-APCP). However,
 * these results do not necessarily represent the policy of the
 * Department of Education, and you should not assume endorsement by the
 * Federal Government.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIALib
{
    /// <summary>
    /// This class holds the identification of the 'real' components. This information
    /// is needed for the 'runtime LComponent' creation.
    /// </summary>
    public class CSelector
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
