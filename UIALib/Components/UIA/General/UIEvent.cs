using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using UIALib.Utils;
using UIALib.Types;

namespace UIALib.Components
{
    public class UIEvent : Event<Tuple<List<STreeNode>, AutomationElement>>
    {
        public string srcId => _srcId;
        public string srcName => _srcName;
        public string type => _type;
        public Tuple<List<STreeNode>, AutomationElement> payload => _payload;

        private string _srcId;
        private string _srcName;
        private string _type;
        private Tuple<List<STreeNode>, AutomationElement> _payload;

        public UIEvent()
        { }

        public UIEvent(string srcId
                      , string srcName
                      , string type
                      , Tuple<List<STreeNode>, AutomationElement> payload)
        {
            this._srcId = srcId;
            this._srcName = srcName;
            this._type = type;
            this._payload = payload;
        }
    }

    public class UIEvent<T> : Event<Tuple<List<STreeNode>, AutomationElement, T>>
    {
        public string srcId => _srcId;
        public string srcName => _srcName;
        public string type => _type;
        public Tuple<List<STreeNode>, AutomationElement, T> payload => _payload;

        private string _srcId;
        private string _srcName;
        private string _type;
        private Tuple<List<STreeNode>, AutomationElement, T> _payload;

        public UIEvent(string srcId
                      , string srcName
                      , string type
                      , Tuple<List<STreeNode>, AutomationElement, T> payload)
        {
            this._srcId = srcId;
            this._srcName = srcName;
            this._type = type;
            this._payload = payload;
        }
    }
}
