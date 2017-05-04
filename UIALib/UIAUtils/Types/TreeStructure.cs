/*
 * The R&D leading to these results received funding from the
 * Department of Education - Grant H421A150005 (GPII-APCP). However,
 * these results do not necessarily represent the policy of the
 * Department of Education, and you should not assume endorsement by the
 * Federal Government.
 */

using System.Collections;
using System.Collections.Generic;

namespace UIALib.Utils.Types
{
    public class Tree<T> : IEnumerable<Tree<T>>
    {
        public Tree<T> parent { get; set; }
        public List<Tree<T>> children { get; set; }
        public T val;

        public Tree()
        {
            parent = null;
        }

        public Tree(T val)
        {
            this.val = val;
            this.children = new List<Tree<T>>();
        }

        public Tree(T val, List<Tree<T>> children)
        {
            this.children = children;
            this.val = val;

            foreach(var child in this.children)
            {
                child.parent = this;
            }
        }

        public bool IsRoot { get { return parent == null; } }
        public bool IsLeaf { get { return children.Count==0; } }

        public IEnumerator<Tree<T>> GetEnumerator()
        {
            return ((IEnumerable<Tree<T>>)children).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Tree<T>>)children).GetEnumerator();
        }

        public void Add(T elem)
        {
            val = elem;
        }

        public void Add(List<Tree<T>> children)
        {
            this.children = children;

            foreach(var child in this.children)
            {
                child.parent = this;
            }
        }
    }
}
