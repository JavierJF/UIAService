using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using UIALib.Types;
using UIALib.Utils.Types;
using EventHook;

namespace UIALib.Components.UIA {
    public class MouseEvent : Event<MouseEventArgs> {
        public string srcName => "MouseMoved";
        public string srcId => "Not used";
        public string type => "Mouse Event";
        public MouseEventArgs payload => data;

        private MouseEventArgs data;

        public MouseEvent(MouseEventArgs data) {
            this.data = data;
        }
    }

    public class MouseMoved : EComp<MouseEventArgs> {
        public override string name => "MouseMoved";
        public override string type => "UI-Event";
        public override List<string> eventTypes => new List<string> { "Mouse-pos" };
        public override Tree<string> props =>
            new Tree<string> {
                "Props"
                , new List<Tree<string>>
                    {
                        new Tree<string>
                            { "Backend"
                            , new List<Tree<string>>
                                { new Tree<string> { "UI-Automation" } }}
                    }
            };

        public MouseMoved(IObservable<Event<MouseEventArgs>> obsv) : base(obsv) {}

        public static MouseMoved mouseMoved() {
            var obsv = Observable.FromEvent<EventHandler<MouseEventArgs>, Event<MouseEventArgs>>(
                (handler) => {
                    EventHandler<MouseEventArgs> msHandler = (obj, args) => {
                        if (args.Message == EventHook.Hooks.MouseMessages.WM_MOUSEMOVE) {
                            handler(new MouseEvent(args));
                        }
                    };

                    return msHandler;
                }
                , msHandler => { MouseWatcher.OnMouseInput += msHandler;
                                 MouseWatcher.Start(); }
                , msHandler => { MouseWatcher.OnMouseInput -= msHandler;
                                 MouseWatcher.Stop(); });

            return new MouseMoved(obsv);
        }
    }
}
