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

namespace TestUIAut
{
    public class NBTree<T>
    {
        public NBTree<T> parent { get; set; }
        public List<NBTree<T>> children { get; set; }

        public NBTree()
        {}

        public NBTree(NBTree<T> parent, List<NBTree<T>> children)
        {
            this.parent = parent;
            this.children = children;

            foreach(var child in children)
            {
                child.parent = this;
            }
        }

        public bool IsRoot { get { return parent == null; } }
        public bool IsLeaf { get { return children.Count==0; } }
    }
}
