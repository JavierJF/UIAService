/*
 * The R&D leading to these results received funding from the
 * Department of Education - Grant H421A150005 (GPII-APCP). However,
 * these results do not necessarily represent the policy of the
 * Department of Education, and you should not assume endorsement by the
 * Federal Government.
 */

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Subjects;
using System.Reactive.Disposables;
using System;

namespace UIALib.Types
{
    public delegate Event<T> ReactSrc<T>();
    public delegate Event<T> EventSinker<T>(Event<T> e);

    /// <summary>
    /// Base type with the 'mininum component info' for construction
    /// </summary>
    public interface Comp
    {
        /// <summary>
        /// Name for component identification.
        /// </summary>
        string name { get; }
        /// <summary>
        /// Type of the component, we need to decide which will be a good, "set of
        /// values" for this type, as we don't know how many possible "types" of
        /// components we want, or even if this distinction is valuable.
        /// </summary>
        string type { get; }
        /// <summary>
        /// Possible additional list of properties to complete component description
        /// for the matching.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> props { get; }
    }
}
