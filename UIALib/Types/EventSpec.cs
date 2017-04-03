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
