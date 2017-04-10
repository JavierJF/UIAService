using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using UIALib.Types;
using LanguageExt;
using static LanguageExt.Prelude;
using UIALib.UIAUtils;

namespace UIALib.Components
{
    public class UIANode : EWComp<object, object>
    {
        public override string name => this._name;
        public override string type => this._type;
        public override Dictionary<string, string> props => this._props;
        public override List<string> eventTypes => _eventTypes;
        public override List<string> watchedComps => this.watchedComps;

        private string _name;
        private string _type;
        private Dictionary<string,string> _props;
        private List<string> _eventTypes;
        private List<Either<STreeNode, CTreeNode>> _path;

        public UIANode(string name
                      , string type
                      , Dictionary<string,string> props
                      , List<string> eventTypes
                      , List<Either<STreeNode, CTreeNode>> path)
        {
            this._name = name;
            this._type = type;
            this._props = props;
            this._eventTypes = eventTypes;
            this._path = path;
        }

        public override void OnCompleted()
        {}

        public override void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public override void OnNext(object value)
        {
            var auElem = value as Event<AutomationElement>;

            if (auElem != null)
            {
                var dstElem = TF.walkTree(auElem.payload
                                         , new List<VTreeNode> { }
                                         , _path);

                var strError =
                    dstElem.Match<Option<string>>(
                        Left: (err) =>
                        {
                            return err.ToString();
                        },
                        Right: (elem) =>
                        {
                            AttacherFn.listenToProperty(
                                "InvokeNode"
                                , elem
                                , (obj, args) =>
                                  {
                                  }
                            );

                            return None;
                        });
            }
            else
            {
                CompLogger.log(this, "Types not matching");
            }
        }

        public override IDisposable Subscribe(IObserver<Event<object>> observer)
        {
            throw new NotImplementedException();
        }
    }
}
