using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIALib.Types
{
    public interface WCompBase : Comp {
        List<string> watchedComps { get; }
    }

    /// <summary>
    /// 'Watcher component' that can subscribe to any 'EComp'
    /// </summary>
    public interface WComp<in T> : IObserver<Event<T>>, WCompBase {
    }
}
