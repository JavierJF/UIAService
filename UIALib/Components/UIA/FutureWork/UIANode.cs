using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Subjects;
using System.Windows.Automation;
using UIALib.Types;
using LanguageExt;
using static LanguageExt.Prelude;
using UIALib.Utils.DFunctions;
using UIALib.Utils;
using UIALib.Utils.Types;

namespace UIALib.Components
{
    public enum ETypeV
    {
        StructChng
        , InvokedElem
        , menuOpened
        , selectedMenuItem
    }

    public delegate Event<P> eventProc<H,P>(AutomationElement e, H h);

    public class UIANode<H,P> : EWComp
    {
        public override string name => _name;
        public override string type => this._type;
        public override Tree<string> props => this._props;
        public override List<string> eventTypes => _eventTypes;
        public override List<string> watchedComps => this._watchedComps;

        private Tree<string> _props;
        private string _name;
        private string _type;
        private List<string> _eventTypes = new List<string> { "Log" };
        private List<string> _watchedComps;
        private List<Either<STreeNode, CTreeNode>> _path;
        private eventProc<H,P> procEvent;

        public UIANode(string name
                      , string type
                      , string eventType
                      , Tree<string> props
                      , List<Either<STreeNode, CTreeNode>> path
                      , Subject<Event<object>> obj) : base(obj, obj)
        {
            this._name = name;
            this._type = type;
            this._eventTypes.Add(eventType);
            this._props = props;
            this._path = path;
            this._eventTypes = eventTypes;
        }

        public void OnNext(object value)
        {
            var auElem = value as Event<Tuple<AutomationElement, H>>;

            if (auElem != null)
            {
                var dstElem = TF.walkTree(auElem.payload.Item1
                                         , new List<VTreeNode> { }
                                         , _path);

                var nodeRes =
                    dstElem.Match<Either<string, Event<P>>>(
                        Left: (err) =>
                        {
                            return err.ToString();
                        },
                        Right: (elem) =>
                        {
                            var nextPayload = procEvent(elem, auElem.payload.Item2);

                            return Right<string,Event<P>>(nextPayload);
                        });

                nodeRes.Match(
                    Left: (err) =>
                    {
                    },
                    Right: (elem) =>
                    {
                        // this.emit(elem);
                    }
                );
            }
            else
            {
                CompLogger.log(this, "Types not matching");
            }
        }
    }
}
