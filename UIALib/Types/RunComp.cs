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
using System.Windows.Automation;
using LanguageExt;
using static LanguageExt.Prelude;

namespace UIALib.Types
{
    /// <summary>
    /// Runtime component created when a 'RComponent specification' is
    /// instantiated.
    /// </summary>
    class RunComp
    {
        /// <summary>
        /// Unique identifier for the component, created when instantiated.
        /// </summary>
        public CSelector uniqueId { get; set; }
        /// <summary>
        /// RComponent specification.
        /// </summary>
        Comp comp { get; set; }
        /// <summary>
        /// Automation element that works as reactivity source for the
        /// component.
        /// </summary>
        AutomationElement reactSrc { get; set; }
    }
}
