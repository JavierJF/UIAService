using System;
using System.Collections.Generic;
using UIALib.UIAUtils.TreeTypes;
using System.Windows.Automation;
using LanguageExt;
using System.Linq;

namespace UIALib.UIAUtils.Functions {
    public class TF {
        /// <summary>
        /// Returns the relative path of a element to a window.
        /// IMPORTANT
        /// * For now it returns the path from Root.
        /// </summary>
        /// <param name="e">Element from which path will be searched.</param>
        /// <returns>The relative path of the element to the specified window.</returns>
        public static TreePath relWinPath(AutomationElement e) {
            TreePath resPath = 
                new TreePath { Path = new List<Either<STreeNode, CTreeNode>>() };

            if (e != null) {
                var parent = TreeWalker.RawViewWalker.GetParent(e);

                while (parent != null
                       || parent == AutomationElement.RootElement)
                {
                    var sTNode = new STreeNode { Name = parent.Current.Name
                                               , NextMove = Move.Child };
                    resPath.Path.Add(sTNode);
                    parent = TreeWalker.ContentViewWalker.GetParent(parent);
                }
            }

            return resPath;
        }

        /// <summary>
        /// Returns a tuple with a bool representing if last element is the desired
        /// one, ontherwise returns null as first element.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Tuple<CTreeNode,bool> checkIfExpMenu(Either<STreeNode,CTreeNode> node) {
            CTreeNode cNode = null;

            var res =
                node.Match(
                    Left: (l) => {
                        return false;
                    },
                    Right: (r) => {
                        if (r.Action == NodeAction.Expand) {
                            cNode = r;
                            return true;
                        }
                        else {
                            return false;
                        }
                    });

            return new Tuple<CTreeNode,bool>(cNode, res);
        }

        public static Tuple<TreePath, bool> relPath(TreePath st, TreePath rel) {
            var resPath = new TreePath { Path = new List<Either<STreeNode, CTreeNode>>() };
            var badRes = new Tuple<TreePath, bool>(resPath, false);

            if (!st.Path.Any()) {
                return badRes;
            }

            var sLast = st.Path.Last();
            var checkRes = checkIfExpMenu(sLast);

            if (!checkRes.Item2) {
                return badRes;
            } 

            var sSimpleLast =
                new STreeNode { Name = checkRes.Item1.Name
                              , NextMove = checkRes.Item1.NextMove };

            var index =
                rel.Path.FindIndex(
                    (elem) => {
                        var res =
                            elem.Match(
                                Left: (l) => {
                                    return sSimpleLast == l;
                                },
                                Right: (r) => {
                                    return false;
                                }
                        );
                        return res;
                    }
                );
            
            if (index == -1) {
                return badRes;
            } else {
                var stList = st.Path;
                var relEnd = rel.Path.Skip(Math.Max(0, rel.Path.Count() + 1 - index));

                stList.AddRange(relEnd);
                resPath.Path = stList;

                var gRes = new Tuple<TreePath, bool>(resPath, true);

                return gRes;
            }
        }
    }
}
