using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using UIALib.Utils.Types;

namespace UIALib.Types
{
    public interface EWCompBase : WCompBase, IEComp
    {}

    public abstract class EWComp : IEComp, WComp<object>
    {
        public abstract List<string> watchedComps { get; }
        public abstract List<string> eventTypes { get; }
        public abstract string name { get; }
        public abstract string type { get; }
        public abstract Tree<string> props { get; }

        protected IObserver<Event<object>> _watcher;
        protected IObservable<Event<object>> _emitter;

        public EWComp(IObserver<Event<object>> watcher, IObservable<Event<object>> emitter) {
            this._watcher = watcher;
            this._emitter = emitter;
        }

        public virtual void OnNext(Event<object> value) {
            this._watcher.OnNext(value);
        }

        public virtual void OnCompleted() {
            this._watcher.OnCompleted();
        }

        public virtual void OnError(Exception error) {
            this._watcher.OnError(error);
        }

        public IDisposable Subscribe(WCompBase observer) {
            var wObsv = observer as WComp<object>;
            IDisposable c = null;

            if (wObsv == null) {
                CompLogger.log(this, "Network Inconsistency");
            } else {
                c = this._emitter.Subscribe(wObsv);
            }
            return c;
        }
    }
}
