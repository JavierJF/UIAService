using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using UIALib.Types;
using UIALib.Utils.Types;
using UIALib.Utils.DFunctions;

namespace UIALib.Components.UIA {
    public class WindowsAppDetectorWatcher : WComp<object> {
        public List<string> watchedComps => throw new NotImplementedException();
        public string name => throw new NotImplementedException();
        public string type => throw new NotImplementedException();

        public Tree<string> props => throw new NotImplementedException();

        public void OnCompleted() {
            throw new NotImplementedException();
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        private string ifEmpty(string s) {
            if (!s.Any()) {
                return "[No Name]";
            } else {
                return s;
            }
        }

        public void OnNext(Event<object> value) {

            Event<Tuple<object, StructureChangedEventArgs>> tval = value as Event<Tuple<object, StructureChangedEventArgs>>;

            var rootNode = AutomationElement.RootElement;
            var auSender = tval.payload.Item1 as AutomationElement;
            var args = tval.payload.Item2;

            if (args.StructureChangeType == StructureChangeType.ChildAdded) {
                var childs = TF.getChildren(rootNode);
                var childNames = from child in childs select child.Current.Name;
                var start = "START \r\n";
                var end = "END \r\n";
                var senderName = start + "Sender: ";

                try {
                    senderName += auSender.Current.Name + "\r\n";
                } catch {
                    senderName += "Non-available\r\n";
                }

                var sChildNames = "";

                if (!childNames.Any()) {
                    sChildNames = "No Childs";
                } else {
                    sChildNames = "Childs : [" 
                                  + childNames.Aggregate(
                                      (s1, s2) => ifEmpty(s1) 
                                                  + ",\r\n          "
                                                  + ifEmpty(s2)
                                    );
                    sChildNames = sChildNames + "]";
                }

                Console.WriteLine(sChildNames + end);

            }
        }
    }
}
