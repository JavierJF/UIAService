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
        public static List<EWComp> ewComps =
            new List<EWComp>
            {
            };
        public static List<IEComp> eComps =
            new List<IEComp>
            {
                WAppDetCreator.appWindowDetector()
            };
        public static List<WComp<object>> wComps =
            new List<WComp<object>>
            {
                new Logger()
            };
    }
}
