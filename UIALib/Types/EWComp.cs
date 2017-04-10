using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIALib.Types
{
    public interface IEWComp<out T, in U> : IEComp<Event<T>>, WComp<U>
    {}

    public abstract class EWComp<T, U> : EComp, IEWComp<T, U> 
    {
        public abstract List<string> watchedComps { get; }
        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(U value);
        public abstract IDisposable Subscribe(IObserver<Event<T>> observer);
    }
}
