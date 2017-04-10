using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIALib.Types;

namespace UIALib.Components
{
    public class ImplComps
    {
        public static List<EWComp<object, object>> ewComps =
            new List<EWComp<object, object>>
            {
            };
        public static List<EComp> eComps =
            new List<EComp>
            {
                new AppWindowDetector()
            };
        public static List<WComp<object>> wComps =
            new List<WComp<object>>
            {
                new Logger()
            };
    }
}
