using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Subjects;
using UIALib.Types;
using UIALib.Utils;

namespace UIALib.Components.UIA {
    /// <summary>
    /// Seach elegant way to hide kinds for clients.
    /// </summary>
    /// <typeparam name="U"></typeparam>
    public abstract class TComp<U> : EComp<U> {
        public TComp(IObservable<Event<U>> obsv): base(obsv)
        {}
    }

    public static class TCompK {
        public static TComp<U> tComp<C,T,U>(IObservable<Event<T>> obsvl
                                           , Func<IObservable<Event<T>>
                                                 , IObservable<Event<U>>> f) where C : TComp<U>, new() 
        {
            var newObsvl = f(obsvl);
            return Activator.CreateInstance(typeof(C), new object [] { newObsvl }) as C;
        }
    }
}
