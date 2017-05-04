/*
 * The R&D leading to these results received funding from the
 * Department of Education - Grant H421A150005 (GPII-APCP). However,
 * these results do not necessarily represent the policy of the
 * Department of Education, and you should not assume endorsement by the
 * Federal Government.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Automation;
using LanguageExt;
using static LanguageExt.Prelude;
using static Utilities.FU;

namespace UIALib.Utils.DFunctions
{
    /// <summary>
    /// Class that stores the functions to trasverse the tree. Needed for
    /// component instantiation.
    /// </summary>
    public class TF
    {
        /// <summary>
        /// Function that tries to make the move in the Automation Tree that is
        /// reflected in the STreeNode that is passed to it.
        /// </summary>
        /// <param name="node">
        /// The node tree whose movement is going to be performed.
        /// </param>
        /// <param name="nodeElem">
        /// The actual Automation node in which we are going to perform the
        /// movement action.
        /// </param>
        /// <returns></returns>
        public static Either<NodeError, AutomationElement> makeMove(STreeNode node, AutomationElement nodeElem)
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

        /// <summary>
        /// Function that invokes a particular action on a Automation Element.
        /// </summary>
        /// <param name="node">
        /// The complex node that contains the action which needs to be performed
        /// in order to keep traversing the three.
        /// </param>
        /// <param name="nodeElem">
        /// The actual Automation Element in which we are going to invoke an action.
        /// </param>
        /// <returns>
        /// Returns a NodeError if something went wrong, and Nothing, if all was OK.
        /// </returns>
        public static Option<NodeError> invkNodeAction(CTreeNode node
                                                      , AutomationElement nodeElem)
        {
            try
            {
                if (node.action == NodeAction.Invoke)
                {
                    var invk = nodeElem.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
                    invk.Invoke();
                }
                else if (node.action == NodeAction.Expand || node.action == NodeAction.Collapse)
                {
                    var expand = nodeElem.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
                    expand.Expand();
                }
                else if (node.action == NodeAction.Toggle)
                {
                    var toggle = nodeElem.GetCurrentPattern(TogglePattern.Pattern) as TogglePattern;
                    toggle.Toggle();
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

        /// <summary>
        /// Functions that attacth a property listerner to an Automation Element.
        /// </summary>
        /// <param name="node">
        /// The Automation Element in which we are going to attach a listener.
        /// </param>
        /// <param name="scope">
        /// The scope of the three that the listener should take into account for
        /// event filtering.
        /// </param>
        /// <param name="eventID">
        /// The specific property ID we want to listen.
        /// </param>
        /// <returns></returns>
        public static Task<Option<NodeError>> addSyncPropertyEventWatcher(AutomationElement node
                                                                         , TreeScope scope
                                                                         , AutomationEvent eventID)
        {
            var t = new TaskCompletionSource<Option<NodeError>>();

            Automation.AddAutomationEventHandler(
                eventID
                , node
                , scope
                , (sender, args) =>
                  {
                      Console.WriteLine("Something");
                      t.TrySetResult(None);
                  }
            );

            return t.Task;
        }

        public static Task<Option<NodeError>> addSyncStructureEventWatcher(AutomationElement node
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

        /// <summary>
        /// Function that consumes one element from actual destination path.
        /// </summary>
        /// <param name="curPath">
        /// Path from which an element is going to be consumed.
        /// </param>
        /// <returns></returns>
        private static List<Either<STreeNode, CTreeNode>> updatePath(List<Either<STreeNode, CTreeNode>> curPath)
        {
            curPath.RemoveAt(0);
            return curPath;
        }

        /// <summary>
        /// Make a step throught the UI Automation Tree
        /// </summary>
        /// <param name="curState">
        /// State to be computed and actualized.
        /// </param>
        /// <returns>
        /// Either a new state or a error.
        /// </returns>
        public static Either<WState, WalkerError> walkNode(WState curState)
        {
            if (!curState.destPath.Any())
            {
                var error = 
                    new WalkerError { nodeError =
                                        new NodeError { errCode = NodeErrCode.WrongMove
                                                      , descr = "No destination path." }
                                    , errorPath = curState.relPath};
            }

            var curAutNode = curState.curAutNode;
            var curDestPath = curState.destPath;
            var curRelPath = curState.relPath;

            // Update variables for current iteration.
            var nextNode = curDestPath.First();
            curRelPath.Add(new VTreeNode { name = curAutNode.Current.Name });

            Either<NodeError, AutomationElement> procNodeRes =
                nextNode.Match<Either<NodeError, AutomationElement>>(
                    Left: (sNode) =>
                    {
                        var nNode = makeMove(sNode, curAutNode);

                        var nextMoveRes = nNode.Match<Either<NodeError, AutomationElement>>(
                            Left: (nodeError) =>
                            {
                                return nodeError;
                            },
                            Right: (nxtNode) =>
                            {
                                return nxtNode;
                            }
                        );

                        return nextMoveRes;
                    },
                    Right: (cNode) =>
                    {
                        var t = new TaskCompletionSource<Option<WalkerError>>();

                        var lScope = cNode.postActionEvents.Item2.scope;
                        var eventType = cNode.postActionEvents.Item2.changeType;
                        var property = cNode.postActionEvents.Item1;

                        var tP = addSyncPropertyEventWatcher(curAutNode, TreeScope.Children, property);
                        var tS = addSyncStructureEventWatcher(curAutNode, lScope, eventType);

                        // Return not used, need to refactor this
                        var inkRes = invkNodeAction(cNode, curAutNode);

                        var cRes = inkRes.Match<Either<NodeError, AutomationElement>>(
                            Some: (nodeError) =>
                            {
                                return nodeError;
                            },
                            None: () =>
                            {
                                var pChangeRes = tP.Result;

                                if (pChangeRes == None)
                                {
                                    var sChangeRes = tS.Result;

                                    if (sChangeRes == None)
                                    {
                                        var nNode = makeMove(cNode.sNode, curAutNode);

                                        var nextMoveRes =
                                            nNode.Match<Either<NodeError, AutomationElement>>(
                                                Left: (_nodeError) =>
                                                {
                                                    return _nodeError;
                                                },
                                                Right: (_nxtNode) =>
                                                {
                                                    return _nxtNode;
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

                        var resVal = cRes.Match<Either<NodeError, AutomationElement>>(
                            Left: (nError) =>
                            {
                                return nError;
                            },
                            Right: (autElem) =>
                            {
                                return autElem;
                            }
                        );

                        return resVal;
                    }
            );

            var itRes = procNodeRes.Match<Either<WalkerError, AutomationElement>>(
                Left: (nodeError) =>
                {
                    return new WalkerError { nodeError = nodeError, errorPath = curRelPath };
                },
                Right: (autNode) =>
                {
                    return autNode;
                }
            );

            var fRes = itRes.Match<Either<WState, WalkerError>>(
                Left: (walkerError) =>
                {
                    return walkerError;
                },
                Right: (autElem) =>
                {
                    curDestPath = updatePath(curDestPath);

                    return new WState { relPath = curRelPath
                                      , destPath = curDestPath
                                      , curAutNode = curAutNode };
                }
            );

            return fRes;
        }     

        /// <summary>
        /// Function that check the actual state, and return true if the previous
        /// statehave been well computed. And return false, if we are either
        /// in the final state, or in a wrong state
        /// </summary>
        /// <param name="st">
        /// The state to be checked.
        /// </param>
        /// <returns>
        /// True if correct state, 'false' in case of wrong or final state.
        /// </returns>
        public static bool checkState(Either<WState,WalkerError> st)
        {
            return st.Match<bool>(
                Left: (state) =>
                {
                    var destPath = state.destPath;
                    
                    if (destPath.Any())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                },
                Right: (error) =>
                {
                    return false;
                }
            );
        }

        /// <summary>
        /// Function that retrieves the Automation element in which we want to act.
        /// </summary>
        /// <param name="curNode">The current node of the tree in which we start.</param>
        /// <param name="relPath">The relative path at the starting point.</param>
        /// <param name="dstPath">The destination path we want to reach.</param>
        /// <returns>Either and error or the Node that was being looking for.</returns>
        public static
            Either<WalkerError, AutomationElement> walkTree(AutomationElement curNode
                                                           , List<VTreeNode> relPath
                                                           , List<Either<STreeNode, CTreeNode>> dstPath)
        {
            var iniState = new WState { relPath = new List<VTreeNode>(relPath)
                                      , destPath = new List<Either<STreeNode, CTreeNode>>(dstPath)
                                      , curAutNode = curNode  };

            var fState = whileS(walkNode, checkState, iniState );

            var res = fState.Match<Either<WalkerError,AutomationElement>>(
                Left: (wstate) =>
                {
                    return wstate.curAutNode;
                },
                Right: (werror) =>
                {
                    return werror;
                }
            );

            return res;
        }

        /// <summary>
        /// Function that attach an event handler at the end of the desired supplied path.
        /// </summary>
        /// <param name="elmPath">
        /// Path in which we want to attach
        /// </param>
        /// <param name="id">
        /// Id of the event we want the handler to respond.
        /// </param>
        /// <param name="startNode">
        /// The node we are going to use at start point, from we are going to start trasversing
        /// the three.
        /// </param>
        /// <param name="scope">
        /// The part of the three we want the handler to take into account for event
        /// events.
        /// </param>
        /// <param name="handler">
        /// The handler itself that we want to attach.
        /// </param>
        /// <returns>
        /// Nothing if the 'handler' has beeing attached correctly, or an error if not.
        /// </returns>
        public static Option<WalkerError> attachAuEventHandler(List<Either<STreeNode, CTreeNode>> elmPath
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

        /// <summary>
        /// Function that attach an structure event handler at the end of the desired supplied path.
        /// </summary>
        /// <param name="elmPath">
        /// Path in which we want to attach
        /// </param>
        /// <param name="id">
        /// Id of the event we want the handler to respond.
        /// </param>
        /// <param name="startNode">
        /// The node we are going to use at start point, from we are going to start trasversing
        /// the three.
        /// </param>
        /// <param name="scope">
        /// The part of the three we want the handler to take into account for event
        /// events.
        /// </param>
        /// <param name="handler">
        /// The handler itself that we want to attach.
        /// </param>
        /// <returns>
        /// Nothing if the 'handler' has beeing attached correctly, or an error if not.
        /// </returns>
        public static Option<WalkerError> attachStEventHandler(List<Either<STreeNode, CTreeNode>> elmPath
                                                              , AutomationElement startNode
                                                              , TreeScope scope
                                                              , StructureChangedEventHandler handler)
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
                        Automation.AddStructureChangedEventHandler(node, scope, handler);
                        return None;
                    }
                );
        }

        /// <summary>
        /// Possible movements that you can perform.
        /// </summary>
        public enum MAction
        {
            nSibling
            , pSibling
            , child
            , parent
            , attach
            , notSupported
        }

        /// <summary>
        /// Dictionary for parsing manual movements from Console.
        /// </summary>
        public static readonly Dictionary<string, MAction> sActions =
            new Dictionary<string, MAction> {
                {"ns", MAction.nSibling }
                , {"ps", MAction.pSibling }
                , {"c", MAction.child }
                , {"p", MAction.parent }
                , {"a", MAction.attach }
            };

        /// <summary>
        /// Simpel function for parsing movements obtained through the
        /// standard input.
        /// </summary>
        /// <param name="move">
        /// The action that the user has input.
        /// </param>
        /// <returns></returns>
        public static MAction parseAction(string move)
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


        /// <summary>
        /// Simple function for manual tree trasversing actions execution.
        /// </summary>
        /// <param name="action">
        /// Action to be performed.
        /// </param>
        /// <param name="node">
        /// Node in which it will be performed.
        /// </param>
        /// <returns></returns>
        public static Option<AutomationElement> execMove(MAction action
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

        public static List<AutomationElement> getChildren(AutomationElement e)
        {
            var children = new List<AutomationElement>();
            var child = TreeWalker.RawViewWalker.GetFirstChild(e);

            if (child == null)
            {
                return children;
            }
            else
            {
                bool nSibling = true;

                while(nSibling)
                {
                    var nextSibling = TreeWalker.RawViewWalker.GetNextSibling(child);

                    if (nextSibling != null)
                    {
                        children.Add(nextSibling);
                        child = nextSibling;
                    }
                    else
                    {
                        nSibling = false;
                    }
                }
            }

            return children;
        }

        /// <summary>
        /// Function for manually testing tree trasversing.
        /// </summary>
        /// <param name="relPath"></param>
        public static void manualTreeTrasversing(List<Either<STreeNode, CTreeNode>> relPath)
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
                        var res = attachAuEventHandler(relPath
                                     , AutomationEvent.LookupById(20003)
                                     , currNode
                                     , TreeScope.Children
                                     , (sender, args) => { Console.WriteLine("Something changed"); });
                        if (res == None)
                        {
                            Console.WriteLine("Attached correctly");
                        }
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
