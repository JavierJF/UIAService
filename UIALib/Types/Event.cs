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
    /// Data type that is transmited to other component, in response to an
    /// event.
    /// </summary>
    public class Payload
    {
        public string data;
        public string type;
    }

    /// <summary>
    /// Events are the changes to which every component can subscribe, and
    /// respond to their effects, when receiving their payloads.
    /// </summary>
    public class Event
    {
        /// <summary>
        /// This is the name of the component which is the source of the
        /// event.
        /// </summary>
        public string srcName;
        /// <summary>
        /// Unique runtime identifier for the component.
        /// </summary>
        public string srcId;
        /// <summary>
        /// We could define the real 'types' of events we allow, using the
        /// UI Automation API.
        /// </summary>
        public string type;
        /// <summary>
        /// Information that the event transmit to the listeners.
        /// </summary>
        public Payload data;
    }
}
