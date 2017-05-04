using System;
using System.Collections.Generic;
using System.Windows.Automation;
using System.Reactive.Subjects;
using UIALib.Types;
using UIALib.Utils.Types;
using UIALib.Utils.Functions;
using UIALib.UIAUtils.Types;
using UIALib.UIAUtils.TreeTypes;
using UIALib.UIAUtils.Functions;

namespace UIALib.Components.UIA {

    class MouseAction : Event<TreePath> {
        public string srcName => _srcName;
        public string srcId => "Not used";
        public string type => "FocusEvent";
        public TreePath payload => _payload;

        private string _srcName;
        private TreePath _payload;

        public MouseAction(string srcName
                          , TreePath payload)
        {
            this._srcName = srcName;
            this._payload = payload;
        }
    }

    public enum Action {
        None,
        ExpandMenu,
        PExpandMenu,
        InvokeItem,
        ToggleItem,
        SelectMenuElement
    };

    public class UIAActionRecorder : EWComp {
        private AutomationElement lastElem = null;
        private Action action = Action.None;
        private Action<AutomationElement> detacher = (elem) => { };

        public UIAActionRecorder(IObserver<Event<object>> watcher
                                , IObservable<Event<object>> emitter) : base(watcher, emitter) { }

        // Called inside the event we set.
        // base.OnNext(value);

        public override void OnNext(Event<object> value) {
            var uiElemE = value as Event<AutomationElement>;

            if (uiElemE != null) {
                // Detach last handler, focus has changed. Now its unuseful.
                if (lastElem != null) {
                    detacher(lastElem);
                }

                var uiElem = uiElemE.payload;
                var uiElemName = (string)uiElem.Current.Name.Clone();
                var type = UICTypes.getType(uiElem);
                var currElemPath = TF.relWinPath(uiElem);

                // Console.WriteLine("Path: " + currElemPath.toString());
                // Console.WriteLine("");

                if (type == UIControlType.ExpandableMenu) {
                    var pattern =
                        uiElem.GetCurrentPattern(ExpandCollapsePattern.Pattern)
                            as ExpandCollapsePattern;

                    currElemPath.Path.Add(new CTreeNode { Action = NodeAction.Expand
                                                        , Name = uiElemName
                                                        , NextMove = Move.Child});
                    var e = new MouseAction(this.name
                                           , currElemPath);

                    base.OnNext(e);
                } else if (type == UIControlType.Toggleable) {
                    this.detacher = EA.toggledItem(
                        uiElem,
                        (obj, args) => {
                            currElemPath.Path.Add(new CTreeNode { Action = NodeAction.Toggle
                                                                , Name = uiElemName
                                                                , NextMove = Move.Child});

                            var e = new MouseAction(this.name
                                                   , currElemPath);
                            base.OnNext(e);
                        }
                    );
                } else if (type == UIControlType.SelectableMenu) {
                    // This case is not correctly considered.
                    this.detacher = EA.selectedMenuItem(
                        uiElem,
                        (obj, args) => {
                            currElemPath.Path.Add(new CTreeNode { Action = NodeAction.Invoke
                                                                , Name = uiElemName
                                                                , NextMove = Move.Child});

                            var e = new MouseAction(this.name
                                                   , currElemPath);
                            base.OnNext(e);
                        }
                    );
                } else if (type == UIControlType.Value) {
                    // This case is not correctly considered.
                    this.detacher = EA.selectedMenuItem(
                        uiElem,
                        (obj, args) => {
                            currElemPath.Path.Add(new CTreeNode { Action = NodeAction.Invoke
                                                                , Name = uiElem.Current.Name
                                                                , NextMove = Move.Child});

                            var e = new MouseAction(this.name
                                                    , currElemPath);
                            base.OnNext(e);
                        }
                    );
                }
                lastElem = uiElem;
            }
        }

        public override List<string> watchedComps =>
            new List<string> { "MouseMoved" };
        public override List<string> eventTypes =>
            new List<string> { "TreePath" };
        public override string name =>
            "UIACompActionRecorder";
        public override string type =>
            "ActionRecorded";
        public override Tree<string> props =>
            new Tree<string> {};
    }

    public static class UIAACompActionRecorderK {
        public static UIAActionRecorder uiAComActionRecorder() {
            var subject = new Subject<Event<object>>();

            return new UIAActionRecorder(subject, subject);
        }
    }

    public class UICompActionRecorderW : WComp<object> {
        public List<string> watchedComps =>
            new List<string> { "UICompActionRecorder" };
        public string name =>
            "UICompActionRecorderW";
        public string type =>
            "ActionRecorded";
        public Tree<string> props =>
            new Tree<string>();

        public void OnCompleted() {
            throw new NotImplementedException();
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnNext(Event<object> val) {
            var value = val as Event<TreePath>;
            Console.WriteLine("Path: " + value.payload.toString());
            Console.WriteLine("");
        }
    }
}
