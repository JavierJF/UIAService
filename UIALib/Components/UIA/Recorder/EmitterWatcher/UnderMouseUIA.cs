using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Automation;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using UIALib.Types;
using UIALib.Utils.Types;
using MouseEventArgs = EventHook.MouseEventArgs;

namespace UIALib.Components.UIA {
    public class MouseOverCE : Event<AutomationElement> {
        public string srcName =>
            throw new NotImplementedException();
        public string srcId =>
            throw new NotImplementedException();
        public string type =>
            throw new NotImplementedException();
        public AutomationElement payload => _payload;

        private AutomationElement _payload;

        public MouseOverCE(AutomationElement payload) {
            this._payload = payload;
        }
    }

    public class UnderMouseUIA : EWComp {
        public UnderMouseUIA(IObserver<Event<object>> watcher
                            , IObservable<Event<object>> emitter) : base(watcher
                                                                        , emitter)
        {
        }
        
        /// <summary>
        /// Windows behaviour is inconsistent in the element retrieved, this needs more
        /// research, also it's remarkable that even Microsoft tool fails under certain conditions.
        /// </summary>
        /// <returns></returns>
        public static UnderMouseUIA underMouseUIA() {
            var subject = new Subject<Event<object>>();
            var auElems = subject.Select(
                (mArgs) => {
                    var cMArgs = mArgs as Event<MouseEventArgs>;
                    var p = cMArgs.payload;
                    var np = new Point(p.Point.x, p.Point.y);

                    Point npp = new Point(Cursor.Position.X, Cursor.Position.Y);
                    Console.WriteLine("Cursor Point: " + npp.X + " " + npp.Y);
                    AutomationElement aue = null;
                    try {
                        // aue = AutomationElement.FromPoint(np);
                        aue = AutomationElement.FromPoint(npp);
                    } catch {
                        Console.WriteLine("Failed to catch AuElement");
                    }
                    return new MouseOverCE(aue);
                }
            );

            return new UnderMouseUIA(subject, auElems);
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

    public class UnderMouseWatcher : WComp<object> {
        public List<string> watchedComps => throw new NotImplementedException();
        public string name => throw new NotImplementedException();
        public string type => throw new NotImplementedException();
        public Tree<string> props => throw new NotImplementedException();

        public void OnCompleted() {
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnNext(Event<object> value) {
            var aue = value as Event<AutomationElement>;
            
            if (aue != null) {
                if (aue.payload != null) {

                    /*
                     * Can produce a NOCOMVisibleBaseClass Exception
                     * Inspect.exe seems to not handle this correctly also
                     * further testing is required.
                     */
                    var cName = aue.payload.Current.Name;

                    if (cName != null) {
                        Console.WriteLine(aue.payload.Current.Name);
                    }
                }
            } else {
                Console.WriteLine("Under Mouse NULL");
            }
        }
    }
}
