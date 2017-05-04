using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Reactive.Subjects;
using UIALib.Types;
using UIALib.Utils.Types;

namespace UIALib.Components.UIA {
    class FocusEvent : Event<Tuple<AutomationElement, Action>> {
        public string srcName => _srcName;
        public string srcId => _srcId;
        public string type => "FocusEvent";
        public Tuple<AutomationElement,Action> payload => _payload;

        private string _srcName;
        private string _srcId;
        private Tuple<AutomationElement,Action> _payload;

        public FocusEvent(string srcName
                         , string srcId
                         , Tuple<AutomationElement,Action> payload)
        {
            this._srcName = srcName;
            this._srcId = srcId;
            this._payload = payload;
        }
    }

    public class ActionRecorder : EWComp {
        public override string name => "Action Recorder";
        public override string type => "UIA-Service";
        public override List<string> eventTypes =>
            new List<string>{ "Log", "Focus", "UserInteraction" };
        public override List<string> watchedComps => _watchedComps;
        public override Tree<string> props => _props;

        private List<string> _watchedComps;
        private Tree<string> _props;

        public ActionRecorder(Subject<Event<object>> subj) : base(subj, subj) {
        }
    }
}
