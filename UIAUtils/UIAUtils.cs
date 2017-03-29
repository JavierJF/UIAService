using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Automation;
using LanguageExt;
using static LanguageExt.Prelude;

namespace UIAService
{
    public class StruChange
    {
        public LinkedList<STreeNode> path;
        public AutomationProperty autoPropertyReceiv;
    }

    public class VTreeNode
    {
        public String name { get; set; }
    }

    public class STreeNode
    {
        public String name { get; set; }
        public Move nextMove { get; set; }
    }

    public class StructureChange
    {
        public TreeScope scope;
        public StructureChangeType changeType;
    }

    public class CTreeNode
    {
        public STreeNode sNode { get; set; }
        public NodeAction action { get; set; }
        public List<VTreeNode> path { get; set; }
        public Tuple<AutomationProperty, StructureChange> postActionEvents { get; set; }
    }

    public enum Move
    {
        Sibling,
        Child,
        Parent,
        Path
    }

    public enum NodeAction
    {
        Invoke,
        Expand,
        Collapse,
        Toggle,
        Nothing
    }

    public enum NodeErrCode
    {
        Correct,
        InvalidOp,
        WrongNode,
        WrongMove
    }

    public class NodeError
    {
        public NodeErrCode errCode;
        public string descr;
    }

    public class WalkerError
    {
        public NodeError nodeError;
        public List<VTreeNode> errorPath;
    }

    class UIAUtils
    {
        public Either<NodeError, AutomationElement> nextNode(STreeNode node, AutomationElement nodeElem)
        {
            if (node.nextMove == Move.Parent)
            {
                var parent = TreeWalker.RawViewWalker.GetParent(nodeElem);

                if (parent == null)
                {
                    return new NodeError { errCode = NodeErrCode.WrongMove, descr = "No parent" };
                }
                else
                {
                    return parent;
                }
            }
            else if (node.nextMove == Move.Child)
            {
                var child = TreeWalker.RawViewWalker.GetFirstChild(nodeElem);

                if (child == null)
                {
                    return new NodeError { errCode = NodeErrCode.WrongMove, descr = "No child" };
                }
                else
                {
                    return child;
                }
            }
            else if (node.nextMove == Move.Sibling)
            {
                var sibling = TreeWalker.RawViewWalker.GetNextSibling(nodeElem);

                if (TreeWalker.RawViewWalker.GetParent(nodeElem) == null)
                {
                    return new NodeError { errCode = NodeErrCode.WrongMove, descr = "No sibling" };
                }
                else
                {
                    return sibling;
                }
            }
            else
            {
                return new NodeError { errCode = NodeErrCode.WrongNode, descr = "Impossible Move" };
            }
        }

        public Option<NodeError> invkNodeAction(CTreeNode node
                                               , AutomationElement nodeElem
                                               , NodeAction id)
        {
            try
            {
                if (node.action == NodeAction.Invoke)
                {
                    nodeElem.GetCurrentPattern(InvokePattern.Pattern);
                }
                else if (node.action == NodeAction.Expand || node.action == NodeAction.Collapse)
                {
                    nodeElem.GetCurrentPattern(ExpandCollapsePattern.Pattern);
                }
                else if (node.action == NodeAction.Toggle)
                {
                    nodeElem.GetCurrentPattern(TogglePattern.Pattern);
                }
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                }
                else if (ex is ElementNotAvailableException)
                {
                }
            }
            return None;
        }

        public Option<NodeError> verifyCrrNode(STreeNode node, AutomationElement autNode)
        {
            if (node.name != autNode.Current.Name)
            {
                return new NodeError
                {
                    errCode = NodeErrCode.WrongNode
                                     ,
                    descr = "Name doesn't match"
                };
            }
            else
            {
                return None;
            }
        }

        Task<Option<NodeError>> addSyncPropertyEventWatcher(AutomationElement node
                                                           , TreeScope scope
                                                           , AutomationProperty property)
        {
            var t = new TaskCompletionSource<Option<NodeError>>();

            Automation.AddAutomationPropertyChangedEventHandler(
                node
                , scope
                , (sender, args) =>
                  {
                      t.TrySetResult(None);
                  }
                , property
            );

            return t.Task;
        }

        Task<Option<NodeError>> addSyncStructureEventWatcher(AutomationElement node
                                                               , TreeScope scope
                                                               , StructureChangeType chgType)
        {
            var t = new TaskCompletionSource<Option<NodeError>>();

            Automation.AddStructureChangedEventHandler(
                node
                , scope
                , (sender, args) =>
                {
                    try
                    {
                        var autElem = sender as AutomationElement;

                        if (chgType == StructureChangeType.ChildrenBulkRemoved)
                        {
                            t.TrySetResult(None);
                        }
                        else if (chgType == StructureChangeType.ChildRemoved)
                        {
                            t.TrySetResult(None);
                        }
                        else if (chgType == StructureChangeType.ChildAdded)
                        {
                            t.TrySetResult(None);
                        }
                        else if (chgType == StructureChangeType.ChildrenBulkAdded)
                        {
                            t.TrySetResult(None);
                        }
                        else if (chgType == StructureChangeType.ChildrenInvalidated)
                        {
                            t.TrySetResult(None);
                        }
                        else
                        {
                        }
                    }
                    catch (Exception)
                    {
                        throw new NotImplementedException();
                    }
                }
                );

            return t.Task;
        }

        public Either<WalkerError, AutomationElement> walkTree(AutomationElement curNode
                                                              , List<VTreeNode> relPath
                                                              , List<Either<STreeNode, CTreeNode>> dstPath)
        {
            bool stop = false;
            Either<WalkerError, AutomationElement> itRes
                = new WalkerError { nodeError = new NodeError { errCode = NodeErrCode.InvalidOp
                                                              , descr = "No operation realized" }
                                  , errorPath = relPath};

            while (dstPath.Any())
            {
                var nxtNode = dstPath.First();

                dstPath.RemoveAt(0);
                relPath.Add(new VTreeNode { name = curNode.Current.Name });

                Option<NodeError> procNodeRes =
                    nxtNode.Match(
                        Left: (sNode) =>
                        {
                            var verifyRes = verifyCrrNode(sNode, curNode);

                            var nodeProcRes =
                                verifyRes.Match<Option<NodeError>>
                                    (
                                        Some: (nodeErr) =>
                                        {
                                            return Some(nodeErr);
                                        },
                                        None: () =>
                                        {
                                            var nNode = nextNode(sNode, curNode);

                                            var nextMoveRes = nNode.Match<Option<NodeError>>(
                                                Left: (_nodeError) =>
                                                {
                                                    return Some(_nodeError);
                                                },
                                                Right: (_nxtNode) =>
                                                {
                                                    curNode = _nxtNode;

                                                    return None;
                                                }
                                            );

                                            return nextMoveRes;
                                        }
                                    );

                            return nodeProcRes;
                        },
                        Right: (cNode) =>
                        {
                            var t = new TaskCompletionSource<Option<WalkerError>>();

                            var lScope = cNode.postActionEvents.Item2.scope;
                            var eventType = cNode.postActionEvents.Item2.changeType;
                            var property = cNode.postActionEvents.Item1;

                            var tP = addSyncPropertyEventWatcher(curNode, TreeScope.Element, property);
                            var tS = addSyncStructureEventWatcher(curNode, lScope, eventType);

                            var pChangeRes = tP.Result;

                            if (pChangeRes == None)
                            {
                                var sChangeRes = tS.Result;

                                if (sChangeRes = None)
                                {
                                    var nNode = nextNode(cNode.sNode, curNode);

                                    var nextMoveRes = nNode.Match<Option<NodeError>>(
                                        Left: (_nodeError) =>
                                        {
                                            return Some(_nodeError);
                                        },
                                        Right: (_nxtNode) =>
                                        {
                                            curNode = _nxtNode;

                                            return None;
                                        }
                                    );

                                    return nextMoveRes;
                                }
                                else
                                {
                                    return new NodeError { errCode = NodeErrCode.InvalidOp
                                                         , descr = "Structure Change Error" };
                                }
                            }
                            else
                            {
                                return new NodeError { errCode = NodeErrCode.InvalidOp
                                                     , descr = "Property Change Error" };
                            }
                        }
                );

                itRes = procNodeRes.Match<Either<WalkerError, AutomationElement>>(
                    Some: (nodeError) =>
                    {
                        stop = true;
                        return new WalkerError { nodeError = nodeError, errorPath = relPath };
                    },
                    None: () =>
                    {
                        return curNode;
                    }
                );

                if (stop)
                {
                    break;
                }
            }
            return itRes;
        }

        public Option<WalkerError> attachHandler(List<Either<STreeNode, CTreeNode>> elmPath
                                              , AutomationEvent id
                                              , AutomationElement startNode
                                              , TreeScope scope
                                              , AutomationEventHandler handler)
        {
            var nodeE = walkTree(startNode, new List<VTreeNode> { }, elmPath);

            return
                nodeE.Match<Option<WalkerError>>(
                    Left: (error) =>
                    {
                        return error;
                    },
                    Right: (node) =>
                    {
                        Automation.AddAutomationEventHandler(id, node, scope, handler);
                        return None;
                    }
                );
        }

        public enum MAction
        {
            nSibling
            , pSibling
            , child
            , parent
            , attach
            , notSupported
        }

        public static readonly Dictionary<string, MAction> sActions =
            new Dictionary<string, MAction> {
                {"ns", MAction.nSibling }
                , {"ps", MAction.pSibling }
                , {"c", MAction.child }
                , {"p", MAction.parent }
                , {"a", MAction.attach }
            };

        public MAction parseAction(string move)
        {
            if (move != null)
            {
                if(sActions.ContainsKey(move))
                {
                    return sActions[move];
                }
                else
                {
                    return MAction.notSupported;
                }
            }
            else
            {
                return MAction.notSupported;
            }
        }

        public Option<AutomationElement> execMove(MAction action
                                                 , AutomationElement node)
        {
            if (action == MAction.nSibling)
            {
                var nSibling = TreeWalker.ContentViewWalker.GetNextSibling(node);

                if (notnull(nSibling))
                {
                    return nSibling;
                }
                else
                {
                    Console.WriteLine("No next element");

                    return None;
                }
            }
            else if (action == MAction.pSibling)
            {
                var pSibling = TreeWalker.ContentViewWalker.GetPreviousSibling(node);

                if (notnull(pSibling))
                {
                    return pSibling;
                }
                else
                {
                    Console.WriteLine("No previous element");

                    return None;
                }
            }
            else if (action == MAction.child)
            {
                var child = TreeWalker.ContentViewWalker.GetFirstChild(node);

                if (notnull(child))
                {
                    return child;
                }
                else
                {
                    Console.WriteLine("No childs, you are in a leaf");

                    return None;
                }
            }
            else if (action == MAction.parent)
            {
                var parent = TreeWalker.ContentViewWalker.GetParent(node);

                if (notnull(parent))
                {
                    return parent;
                }
                else
                {
                    Console.WriteLine("Root node, no more parents");

                    return None;
                }
            }
            else
            {
                Console.WriteLine("Non supported action: " + action);

                return None;
            }
        }

        public void manualTreeTrasversing(List<Either<STreeNode, CTreeNode>> relPath)
        {
            AutomationElement currNode;
            currNode = TreeWalker.ContentViewWalker.GetFirstChild(AutomationElement.RootElement);

            while (true)
            {
                string action = Console.ReadLine();
                MAction pAction = parseAction(action);
                Option<AutomationElement> actionRes;

                if (pAction != MAction.notSupported)
                {
                    if(pAction != MAction.attach)
                    {
                        actionRes = execMove(pAction, currNode);
                        actionRes.Match(
                            Some: (node) => currNode = node
                            , None: () => { }
                        );
                    }
                    else
                    {
                        attachHandler(relPath
                                     , AutomationEvent.LookupById(20002)
                                     , currNode
                                     , TreeScope.Element
                                     , (sender, args) => { Console.WriteLine("Something changed"); });
                    }
                }
                else if (action == "b")
                {
                    Console.WriteLine("Closing...");
                    break;
                }
                else
                {
                    Console.WriteLine("Not supported move: `" + pAction + "`");
                }
                Console.WriteLine("Actual Node: " + currNode.Current.Name);
            }
        }
    }
}
