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
    /// Specification of a component.
    /// </summary>
    public class CompSpec
    {
        public string name { get; set; }
        public string type { get; set; }
        public List<LEventSpec> watch { get; set; }
        public List<EEventSpec> emit { get; set; }

        private bool equalLists<T>(List<T> w1, List<T> w2)
        {
            if (w1 == null && w2 == null)
            {
                return true;
            }
            else if (w1 != null && w2 != null)
            {
                return w1.SequenceEqual(w2);
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            CompSpec other = obj as CompSpec;

            if (this.name != other.name)
            {
                return false;
            }
            else if (this.type != other.type)
            {
                return false;
            }
            else if (!equalLists(this.watch, other.watch))
            {
                return false;
            }
            else if (!equalLists(this.emit, other.emit))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
