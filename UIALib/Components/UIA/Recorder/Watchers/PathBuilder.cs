using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIALib.Types;
using UIALib.UIAUtils.TreeTypes;
using UIALib.Utils.Types;
using LanguageExt;

namespace UIALib.Components.UIA.Recorder.Watchers {
    public class PathBuilder : EWComp {
        public override string name =>
            "PathBuilder";
        public override string type =>
            "TreePath";
        public override List<string> watchedComps =>
            new List<string> { "UIACompActionRecorder" };
        public override List<string> eventTypes =>
            new List<string> {"TreePath"};
        public override Tree<string> props =>
            new Tree<string>();


        TreePath finalPath = 
            new TreePath { Path = new List<Either<STreeNode, CTreeNode>>{} };

        TreePath lastPath = null;

        public PathBuilder(IObserver<Event<object>> watcher
                          , IObservable<Event<object>> emitter) : base(watcher, emitter)
        {
        }

        public void OnCompleted() {
            Console.WriteLine("Path builded");
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnNext(Event<object> value) {
            var finalState = false;
            Event<TreePath> ePath = value as Event<TreePath>;

            if (lastPath == null) {
                finalPath.Path = ePath.payload.Path;
            } else {
            }
            if (!finalState) {
                base.OnNext((Event<object>)finalPath);
            } 
        }
    }
}
