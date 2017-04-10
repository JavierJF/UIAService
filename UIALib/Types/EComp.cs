using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Disposables;
using Utilities;

namespace UIALib.Types
{
    public interface IEComp<out T> : IObservable<T>, Comp
    {}

    /// <summary>
    /// This represent a observable component that emits events.
    /// </summary>
    public abstract class EComp : IEComp<Event<object>>
    {
        /// <summary>
        /// Source of component reactivity.
        /// </summary>
        public List<WComp<Event<object>>> watchers =
            new List<WComp<Event<object>>>();

        public abstract string name { get; }
        public abstract string type { get; }
        public abstract Dictionary<string, string> props { get; }
        public abstract List<string> eventTypes { get;  }

        public IDisposable Subscribe(IObserver<Event<object>> observer)
        {
            var tObserver = observer as WComp<Event<object>>;

            if (tObserver == null)
            {
                CompLogger.log(this, "Network Inconsistency");
            }
            else
            {
                watchers.Add(tObserver);
            }
            return Disposable.Empty;
        }

        protected void emit(Event<object> e)
        {
            foreach (var w in watchers)
            {
                if (w.type == e.type)
                {
                    w.OnNext(e);
                }
            }
        }
    }

//     The approach of emiting events could be statically typed. But we would need
//     to have a fixed size number of different events types a component can emit. For now
//     we are going to work with leaving the control of processing events to components that
//     receive them. Without previous classification.

//     public interface IEComp<out T, out U> : IObservable<object>, Comp
//     {}

//     public abstract class EComp<T, U> : IEComp<T, U>
//     {
//         public abstract string name { get; }
//         public abstract string type { get; }
//         public abstract Dictionary<string, string> props { get; }

//         public List<WComp<Event<T>>> tWatchers;
//         public List<WComp<Event<U>>> uWatchers;

//         public IDisposable Subscribe(IObserver<object> observer)
//         {
//             var tWatcher = observer as WComp<Event<T>>;
//             var uWatcher = observer as WComp<Event<U>>;
//             var typeList = new List<Type> { typeof(T), typeof(U) };

//             if (tWatcher == null && uWatcher == null)
//             {
//                 CompLogger.log(this, FU.noTypesMatch(typeList));
//             }
//             else
//             {
//                 if (tWatcher != null)
//                 {
//                     tWatchers.Add(tWatcher);
//                 }
//                 else
//                 {
//                     uWatchers.Add(uWatcher);
//                 }
//             }

//             return Disposable.Empty;
//         }

//         private void emit(Event<T> e)
//         {
//             foreach (var w in tWatchers)
//             {
//                 w.OnNext(e);
//             }
//         }

//         private void emit(Event<U> e)
//         {
//             foreach (var w in uWatchers)
//             {
//                 w.OnNext(e);
//             }
//         }
}
