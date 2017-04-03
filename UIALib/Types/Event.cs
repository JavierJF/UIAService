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
