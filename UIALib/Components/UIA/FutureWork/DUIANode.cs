using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Subjects;
using System.Windows.Automation;
using UIALib.Types;
using UIALib.Utils;
using UIALib.Utils.Types;

namespace UIALib.Components
{
    public class Exp : Event<Tuple<List<STreeNode>, AutomationElement>>
    {
        public string srcId => _srcId;
        public string srcName => _srcName;
        public string type => _type;
        public Tuple<List<STreeNode>, AutomationElement> payload => _payload;

        private string _srcId;
        private string _srcName;
        private string _type;
        private Tuple<List<STreeNode>, AutomationElement> _payload;

        public Exp(string srcId, string srcName, Tuple<List<STreeNode>, AutomationElement> p)
        {
            this._srcId = srcId;
            this._srcName = srcName;
            this._payload = p;
        }
    }

    public class DUIANode : EWComp
    {
        public override string name => "DUIANode";
        public override string type => "UI-Element";
        public override Tree<string> props =>
            new Tree<string> { "Properties",
                new List<Tree<string>> {
                    new Tree<string> { "Backend" ,
                    new List<Tree<string>> { new Tree<string> { "UI-Automation" } } } } };
        public override List<string> eventTypes =>
            new List<string> { "AUElem", "AUElem-Path" };
        public override List<string> watchedComps => _watchedComps; 

        private List<string> _watchedComps = new List<string>();
        private List<STreeNode> pathToNxt;

        public DUIANode(Subject<Event<object>> sbj) : base(sbj,sbj) {
        }
    }
}
