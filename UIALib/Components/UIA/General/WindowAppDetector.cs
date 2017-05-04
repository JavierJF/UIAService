using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using LanguageExt;
using UIALib.Types;
using UIALib.Utils;
using UIALib.Utils.Types;
using UIALib.Utils.DFunctions;

namespace UIALib.Components
{
    public class MainWsChanged : Event<Tuple<object, StructureChangedEventArgs>>
    {
        public string srcName => "AppWindowDetector";
        public string srcId => "Not-Implemented";
        public string type => "Log";
        public Tuple<object,StructureChangedEventArgs> payload => this.data;

        private Tuple<object,StructureChangedEventArgs> data;

        public MainWsChanged(Tuple<object,StructureChangedEventArgs> data)
        {
            this.data = data;
        }
    }

    public class AppWindowDetector<T> : EComp<T>
    {
        public override string name => "AppWindowDetector";
        public override string type => "UI-Detector";
        public override Tree<string> props =>
            new Tree<string>
            {
                "Props"
                , new List<Tree<string>>
                    {
                        new Tree<string>
                            { "Backend"
                            , new List<Tree<string>>
                                { new Tree<string> { "UI-Automation" } }}
                    }
            };

        public override List<string> eventTypes =>
            new List<string>
            {
               "Log", "UI-EStructureHandler"
            };

        public AppWindowDetector(IObservable<Event<T>> obsv) : base(obsv) {}
    }

    public class WAppDetCreator {
        private static StructureChangedEventHandler cStHandler(Action<Event<Tuple<object,StructureChangedEventArgs>>> handler) {
            /// object sender, StructureChangedEventArgs args)
            StructureChangedEventHandler stHandler = (obj, stEventArgs) => {
                var evP = new Tuple<object, StructureChangedEventArgs>(obj, stEventArgs);
                var ev = new MainWsChanged(evP);

                handler(ev);
            };

            return stHandler;
        }

        public static AppWindowDetector<Tuple<object, StructureChangedEventArgs>> appWindowDetector() {
            /// <summary>
            /// We specify a empty list because we want the root node.
            /// </summary>
            var rootPath = new List<Either<STreeNode,CTreeNode>> { };
            /// <summary>
            /// Root node element from which observ windows.
            /// </summary>
            var rootNode = AutomationElement.RootElement;

            var emitter = Observable.FromEvent<StructureChangedEventHandler, Event<Tuple<object,StructureChangedEventArgs>>>(
                handler => {
                    StructureChangedEventHandler stHandler = cStHandler(handler);
                    return stHandler;
                }
                , stHandler => TF.attachStEventHandler(rootPath
                               , rootNode
                               , TreeScope.Children
                               , stHandler)
                , stHander => { });

            return new AppWindowDetector<Tuple<object,StructureChangedEventArgs>>(emitter);
        }
    }
}
