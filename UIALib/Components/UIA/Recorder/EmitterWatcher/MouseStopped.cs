using System;
using System.Collections.Generic;
using UIALib.Types;
using UIALib.Utils;
using EventHook;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Disposables;
using System.Timers;
using UIALib.Utils.Types;

namespace UIALib.Components.UIA {
    public class MouseStopped : EWComp {
        Timer timer;
        Event<MouseEventArgs> lastVal;

        private MouseStopped(IObserver<Event<object>> watcher
                            , IObservable<Event<object>> emitter
                            , Timer t
                            , Event<MouseEventArgs> m) : base(watcher, emitter)
        {
            base._emitter = Observable.Create<Event<object>>(
                (observer) => {
                    timer.Elapsed += (obj, args) => {
                        if (lastVal != null) {
                            observer.OnNext(lastVal);
                        } else {
                            Console.WriteLine("Last was NULL");
                        }
                    };
                    return Disposable.Empty;
                });

            timer = t;
            lastVal = m;
        }
        
        /// <summary>
        /// Very difficult approach, because you need to behave like a new source. Not working from actual events.
        /// </summary>
        /// <returns></returns>
        public static MouseStopped mouseStopped() {
            Event<MouseEventArgs> last = null;

            Timer timer = new Timer();
            timer.Interval = 300;
            timer.AutoReset = false;

            var observable = Observable.Create<Event<MouseEventArgs>>(
                (observer) => {
                    timer.Elapsed += (obj, args) => {
                        if (last != null) {
                            observer.OnNext(last);
                        } else {
                            Console.WriteLine("Last was NULL");
                        }
                    };
                    return Disposable.Empty;
                });

            var fakeObserv = new Subject<Event<object>>();

            return new MouseStopped(fakeObserv, fakeObserv, timer, last);
        }

        public override void OnNext(Event<object> next) {
            var last = next as Event<MouseEventArgs>;
            // Console.WriteLine("Position: " + last.payload.Point.x + " " + last.payload.Point.y + " M: " + last.payload.Message);
            lastVal = last;
            timer.Stop();
            timer.Start();
        }

        public override List<string> watchedComps =>
            throw new NotImplementedException();
        public override string name => throw new NotImplementedException();
        public override string type => throw new NotImplementedException();
        public override Tree<string> props =>
            throw new NotImplementedException();
        public override List<string> eventTypes =>
            throw new NotImplementedException();
    }

    public class MouseWatcherC : WComp<object> {
        public string name => throw new NotImplementedException();
        public string type => throw new NotImplementedException();
        public List<string> watchedComps => throw new NotImplementedException();
        public Tree<string> props => throw new NotImplementedException();

        public void OnCompleted() {
            Console.WriteLine("Watch ended");
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnNext(Event<object> value) {
            var mouseArgs = value as Event<MouseEventArgs>;
            
            Console.WriteLine("Position: " + mouseArgs.payload.Point.x + " " + mouseArgs.payload.Point.y + " M: " + mouseArgs.payload.Message);
        }
    }
}
