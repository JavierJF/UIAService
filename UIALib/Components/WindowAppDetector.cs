using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using LanguageExt;
using UIALib.Types;

namespace UIALib.Components
{
    public class MainWsChanged : Event<string>
    {
        public string srcName => "AppWindowDetector";
        public string srcId => "Not-Implemented";
        public string type => "Log";
        public string payload => this.data;

        private string data;

        public MainWsChanged(string data)
        {
            this.data = data;
        }
    }

    public class AppWindowDetector : EComp
    {
        public override string name => "AppWindowDetector";
        public override string type => "UI-Detector";
        public override Dictionary<string, string> props =>
            new Dictionary<string, string>
            {
                { "Backend", "UI-Automation" }
            };

        public override List<string> eventTypes =>
            new List<string>
            {
               "Log"
            };

        /// <summary>
        /// We specify a empty list because we want the root node.
        /// </summary>
        private List<Either<STreeNode,CTreeNode>> rootPath =
            new List<Either<STreeNode,CTreeNode>> { };
        /// <summary>
        /// Root node element from which observ windows.
        /// </summary>
        private AutomationElement rootNode = AutomationElement.RootElement;

        private string ifEmpty(string s)
        {
            if (!s.Any())
            {
                return "[No Name]";
            }
            else
            {
                return s;
            }
        }

        private void eventHandler(object sender
                                 , StructureChangedEventArgs args)
        {
            AutomationElement auSender = sender as AutomationElement;

            if (args.StructureChangeType == StructureChangeType.ChildAdded)
            {
                var childs = TF.getChildren(rootNode);
                var childNames = from child in childs select child.Current.Name;
                var start = "START \r\n";
                var end = "END \r\n";
                var senderName = start + "Sender: ";

                try
                {
                    senderName += auSender.Current.Name + "\r\n";
                }
                catch
                {
                    senderName += "Non-available\r\n";
                }

                var sChildNames = "";

                if (!childNames.Any())
                {
                    sChildNames = "No Childs";
                }
                else
                {
                    sChildNames = "Childs : [" 
                                  + childNames.Aggregate(
                                      (s1, s2) => ifEmpty(s1) 
                                                  + ",\r\n          "
                                                  + ifEmpty(s2)
                                    );
                    sChildNames = sChildNames + "]";
                }

                var nevent = new MainWsChanged(senderName + sChildNames + "\r\n" + end);
                emit(nevent);
            }
        }

        public AppWindowDetector()
        {
            StructureChangedEventHandler handler = eventHandler;
            var res = TF.attachStEventHandler(rootPath
                                   , rootNode
                                   , TreeScope.Children
                                   , handler);
            Console.WriteLine("Component Initialized");
        }
    }
}
