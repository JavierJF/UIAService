using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIALib.Types
{
    /// <summary>
    /// 'Watcher component' that can subscribe to any 'EComp'
    /// </summary>
    public interface WComp<in T> : IObserver<T>, Comp
    {
        List<string> watchedComps { get; }
    }
}
