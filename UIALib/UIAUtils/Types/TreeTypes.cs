using System;
using System.Text;
using System.Collections.Generic;
using LanguageExt;

namespace UIALib.UIAUtils.TreeTypes {

    public enum Move {
        Child,
        Parent
    }

    public class STreeNode {
        public String Name { get; set; }
        public Move NextMove { get; set; }
    }

    public enum NodeAction {
        Invoke,
        Expand,
        Collapse,
        Toggle
    }

    public class CTreeNode : STreeNode {
        public NodeAction Action { get; set; }
    }

    public class TreePath {
        public List<Either<STreeNode, CTreeNode>> Path { get; set; }

        public string toString() {
            List<string> elemR = new List<string>();

            foreach (var elem in Path) {
                elem.Match(
                    Right: (r) => {
                        var cNode = "(" + r.Name
                                    + ", " 
                                    + r.NextMove
                                    + ", "
                                    + r.Action + ")";

                        elemR.Add(cNode);
                    },
                    Left: (l) => {
                        var sNode = "(" + l.Name
                                    + ", "
                                    + l.NextMove + ")";

                        elemR.Add(sNode);
                    }
                );
            }

            return String.Join("\r\n -> ", elemR);
        }
    }
}
