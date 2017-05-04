using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIALib.Types;
using UIALib.Utils.Types;

namespace UIALib.Components.UIA.Recorder.EmitterWatcher {
    class UIAElemPaths : EWComp {
        public UIAElemPaths(
            IObserver<Event<object>> watcher
            , IObservable<Event<object>> emitter) : base(watcher, emitter)
        {}

        public override void OnNext(Event<object> value) {
            base.OnNext(value);
        }

        public override List<string> watchedComps => throw new NotImplementedException();
        public override List<string> eventTypes => throw new NotImplementedException();
        public override string name => throw new NotImplementedException();
        public override string type => throw new NotImplementedException();
        public override Tree<string> props => throw new NotImplementedException();
    }
}
