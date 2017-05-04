using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using UIALib.Types;
using UIALib.Utils.Types;

namespace UIALib.Components.UIA {
    public class CompPathRecorder : EWComp {
        public CompPathRecorder
            (IObserver<Event<object>> watcher
            , IObservable<Event<object>> emitter) : base(watcher
                                                        , emitter)
        {
        }

        public static CompPathRecorder comPathRecorder() {
            var subject = new Subject<Event<object>>();

            return null;
        }

        public override List<string> watchedComps =>
            throw new NotImplementedException();
        public override List<string> eventTypes =>
            throw new NotImplementedException();
        public override string name =>
            throw new NotImplementedException();
        public override string type =>
            throw new NotImplementedException();
        public override Tree<string> props =>
            throw new NotImplementedException();
    }
}
