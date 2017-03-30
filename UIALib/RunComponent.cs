using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using LanguageExt;
using static LanguageExt.Prelude;

namespace UIALib
{
    /// <summary>
    /// Runtime component created when a 'RComponent specification' is
    /// instantiated.
    /// </summary>
    class RunComponent
    {
        /// <summary>
        /// Unique identifier for the component, created when instantiated.
        /// </summary>
        public CSelector uniqueId { get; set; }
        /// <summary>
        /// RComponent specification.
        /// </summary>
        Component comp { get; set; }
        /// <summary>
        /// Automation element that works as reactivity source for the
        /// component.
        /// </summary>
        AutomationElement reactSrc { get; set; }
    }
}
