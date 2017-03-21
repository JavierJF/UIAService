using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using LanguageExt;
using static LanguageExt.Prelude;

namespace UIAService
{
    class UIAUtils
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

        public class CTreeNode
        {
            public String name { get; set; }
            public Move nextMove { get; set; }
            public NodeAction action { get; set; }
            public List<VTreeNode> path { get; set; }
            public Tuple<AutomationProperty, StructureChangeType> postActionEvents { get; set; }
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

        public Either<NodeError, AutomationElement> nextNode(STreeNode node, AutomationElement nodeElem)
        {
            if (node.nextMove == Move.Parent)
            {
                var parent = TreeWalker.RawViewWalker.GetParent(nodeElem) ;

                if(parent == null)
                {
                    return new NodeError { errCode = NodeErrCode.WrongMove, descr = "No parent"};
                }
                else
                {
                    return parent;
                }
            }
            else if(node.nextMove == Move.Child)
            {
                var child = TreeWalker.RawViewWalker.GetFirstChild(nodeElem) ;

                if(child == null)
                {
                    return new NodeError { errCode = NodeErrCode.WrongMove, descr = "No child"};
                }
                else
                {
                    return child;
                }
            }
            else if(node.nextMove == Move.Sibling)
            {
                var sibling = TreeWalker.RawViewWalker.GetNextSibling(nodeElem) ;

                if(TreeWalker.RawViewWalker.GetParent(nodeElem) == null)
                {
                    return new NodeError { errCode = NodeErrCode.WrongMove, descr = "No sibling"};
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
                return new NodeError { errCode = NodeErrCode.WrongNode
                                     , descr = "Name doesn't match" };
            }
            else
            {
                return None;
            }
        }

        Task<Option<NodeError>> addStructureChangedEventHandler(AutomationElement elem, TreeScope scope, StructureChangedEventHandler handler)
        {
            var t = new TaskCompletionSource<Option<NodeError>>();

            throw new NotImplementedException();
        }

        public Either<WalkerError, AutomationElement> walkTree(AutomationElement curNode
                                                              , List<VTreeNode> relPath 
                                                              , List<Either<STreeNode, CTreeNode>> dstPath 
                                                              , TreeScope scope
                                                              , EventHandler handler)
        {
            var _dstPath = dstPath;
            var _curNode = curNode;
            var _relPath = relPath; 

            while(_dstPath.Any())
            {
                var nxtNode = _dstPath.First();

                _dstPath.RemoveAt(0);
                _relPath.Add(new VTreeNode{ name = curNode.Current.Name });

                Option<NodeError> procdNodeRes =
                    nxtNode.Match(
                        Left: (sNode) =>
                        {
                            var verifyRes = verifyCrrNode(sNode, _curNode);

                            var nodeProcRes =
                                verifyRes.Match<Option<NodeError>>
                                    ( 
                                        Some: (nodeErr) =>
                                        {
                                            return Some(nodeErr);
                                        },
                                        None: () =>
                                        {
                                            var nNode = nextNode(sNode, _curNode);

                                            var nextMoveRes = nNode.Match<Option<NodeError>>(
                                                Left: (_nodeError) =>
                                                {
                                                    return Some(_nodeError);
                                                },
                                                Right: (_nxtNode) =>
                                                {
                                                    _curNode = _nxtNode;

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
                            TaskCompletionSource<Option<WalkerError>> compNodeAction;
                            return None;
                        }
                );
            }

            // In progress
            throw new NotImplementedException();
        } 

        public AutomationElement attachHandler(List<Either<STreeNode, CTreeNode>> elmPath
                                              , AutomationElement startNode
                                              , TreeScope scope
                                              , EventHandler handler)
        {
            return null;
        }

        public static void structureEventHandler(object sender, StructureChangedEventArgs e)
        {
            AutomationElement srcElement;

            try
            {
                srcElement = sender as AutomationElement;

                if(e.StructureChangeType == StructureChangeType.ChildrenBulkRemoved)
                {
                    Console.WriteLine("Children Removed");
                }
                else if (e.StructureChangeType == StructureChangeType.ChildRemoved)
                {
                    Console.WriteLine("Child Removed");
                    // var child = TreeWalker.ContentViewWalker.GetFirstChild(srcElement);
                    
                    if (srcElement != null && srcElement.Current.Name != null)

                    {
                        Console.WriteLine(srcElement.Current.Name);
                    }
                }
                else if (e.StructureChangeType == StructureChangeType.ChildAdded)
                {
                    Console.WriteLine("Child Added");

                    if (srcElement != null && srcElement.Current.Name != null)
                    {
                        Console.WriteLine(srcElement.Current.Name);
                    }
                }
                else if (e.StructureChangeType == StructureChangeType.ChildrenBulkAdded)
                {
                    Console.WriteLine("Childs Added");
                }
                else if (e.StructureChangeType == StructureChangeType.ChildrenInvalidated)
                {
                    Console.WriteLine("Child Invalidated");
                }
                else
                {
                    Console.WriteLine("`-(.-.)-´");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Something wrong:" + ex.Message);
                return;
            }
        }


        public void invokeNodeAction(NodeAction action, AutomationElement node)
        {
            if (action == NodeAction.Invoke)
            {
                var inkPattern = node.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
                inkPattern.Invoke();
            }
            else if (action == NodeAction.Expand)
            {
                var expPattern = node.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
                expPattern.Expand();
            }
            else if (action == NodeAction.Collapse)
            {
                var collPattern = node.GetCachedPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
                collPattern.Collapse();
            }
            else if (action == NodeAction.Toggle)
            {
                var tggPattern = node.GetCachedPattern(TogglePattern.Pattern) as TogglePattern;
                tggPattern.Toggle();
            }
        }
    }
}
