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
    /// Parsed 'Emited Event'
    /// </summary>
    public class EEventSpec
    {
        public string type { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            EEventSpec other = obj as EEventSpec;

            return this.type == other.type;
        }
    }

    /// <summary>
    /// Parsed 'Listened Event'
    /// </summary>
    public class LEventSpec
    {
        public string type { get; set; }
        public string source { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            LEventSpec other = obj as LEventSpec;

            return (this.type == other.type && this.source == other.source);
        }
    }

}
