using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using UIALib.Utils.Types;

namespace UIALib.Types
{
    public interface TypeMatcher {
        Func<IObserver<Event<object>>,bool>  wMatch { get; }
        Func<Event<object>,bool>  eMatch { get; }
    }

    public class sTMatch<T> : TypeMatcher where T : class {
        public Func<IObserver<Event<object>>, bool> wMatch =>
            (obj) =>
            {
                var wcomp = obj as WComp<T>;

                if (wcomp != null) {
                    return true;
                } else {
                    return false;
                }
            };
        public Func<Event<object>, bool> eMatch =>
            (obj) =>
            {
                var evnt = obj as Event<T>;

                if (evnt != null) {
                    return true;
                } else {
                    return false;
                }
            };
    }

    public class DynDepMap
    {
        private List<TypeMatcher> _matchers;

        public void add(TypeMatcher matcher) {
            this._matchers.Add(matcher);
        }

        public DynDepMap(List<TypeMatcher> matchers) {
            this._matchers = matchers;
        }

        public UInt32 getPos(Event<object> e) {
            UInt32 pos = 0;
            foreach(var matcher in this._matchers) {
                if (matcher.eMatch(e)) {
                    break;
                } else {
                    pos += 1;
                }
            }
            return pos;
        }

        public UInt32 getPos(WComp<object> w) {
            UInt32 pos = 0;
            foreach(var matcher in this._matchers) {
                if (matcher.wMatch(w)) {
                    break;
                } else {
                    pos += 1;
                }
            }
            return pos;
        }
    }

    public interface IECompX : IObservable<Event<object>>, Comp{
        List<string> eventTypes { get; }
    }

    public abstract class ECompX : IECompX
    {
        public abstract string name { get; }
        public abstract string type { get; }
        public abstract Tree<string> props { get; }
        public abstract List<string> eventTypes { get; }

        private List<List<WComp<Event<object>>>> obsvs;
        private DynDepMap map;

        public ECompX(List<TypeMatcher> lms) {
            map = new DynDepMap(lms);
        }
        
        public IDisposable Subscribe(IObserver<Event<object>> observer) {
            var wcomp = (WComp<object>)observer;
            obsvs.ElementAt((int)map.getPos(wcomp)).Add(wcomp);
            return Disposable.Empty;
        }

        public void emit(Event<object> e) {
            var sLW = obsvs.ElementAt((int)map.getPos(e));
            foreach(var wcomp in sLW) {
                // wcomp.OnNext(e);
            }
        }
    }

    public interface IEComp {
        List<string> eventTypes { get; }
        IDisposable Subscribe(WCompBase observer);
    }

    /// <summary>
    /// This represent a observable component that emits events.
    /// </summary>
    public abstract class EComp<T> : Comp, IEComp {
        /// <summary>
        /// Source of component reactivity.
        /// </summary>
        private List<WCompBase> watchers = new List<WCompBase>();
        public abstract string name { get; }
        public abstract string type { get; }
        public abstract List<string> eventTypes { get; }
        public abstract Tree<string> props { get; }
        private IObservable<Event<T>> _obsv { get; }

        public IDisposable Subscribe(WCompBase observer) {
            var wObsv = observer as WComp<T>;
            IDisposable c = null;

            if (wObsv == null) {
                CompLogger.log(this, "Network Inconsistency");
            } else {
                c = this._obsv.Subscribe(wObsv);
            }
            return c;
        }

        public IDisposable SubscribeOn(IScheduler schd, WCompBase observer) {
            var wObsv = observer as WComp<T>;
            IDisposable c = null;

            if (wObsv == null) {
                CompLogger.log(this, "Network Inconsistency");
            } else {
                c = this._obsv.SubscribeOn(schd).Subscribe(wObsv);
            }
            return c;
        }

        public EComp(IObservable<Event<T>> obsv) {
            this._obsv = obsv;
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
