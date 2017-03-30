using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIALib
{
    /// <summary>
    /// This represents a reactive component. Maybe in the future, we can avoid
    /// the use of "path" when the full 'work-load' of finding a valid component
    /// could fall in the match-making. But right now, for speed needs, we are
    /// going to fully specify the path walkers need to follow, to reach a component.
    /// The inner list of 'Components' a component have, describe the "Expansion tree"
    /// of components that should be done in a reactive way. It's describe the child
    /// components in the UI Automation tree that expands from this particular one.
    /// And that should be observed when this particular one is instanciated.
    /// </summary>
    class Component
    {
        /// <summary>
        /// Name for component identification.
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Type of the component, we need to decide which will be a good, "set of
        /// values" for this type, as we don't know how many possible "types" of
        /// components we want, or even if this distinction is valuable.
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// List of the components that depends on this one, and which instantiation
        /// depends on this one.
        /// </summary>
        public List<Component> innerExpansion { get; set; }
        /// <summary>
        /// Te components can listen to the events of other components, those events,
        /// needs to be fully matchable with its component source. Neither
        /// this channel creation, or the target is responsability of each component.
        /// This inversion of control allows us to instanciate components without any
        /// need or reference of "who" will listen.
        /// </summary>
        public List<Event> eventSources { get; set; }
    }
}
