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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using LanguageExt;
using static LanguageExt.Prelude;

namespace UIALib.Utils
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
        public Tuple<AutomationEvent, StructureChange> postActionEvents { get; set; }
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

    public class WalkerError : ICloneable
    {
        public NodeError nodeError;
        public List<VTreeNode> errorPath;

        public object Clone()
        {
            return new WalkerError { nodeError = this.nodeError
                                   , errorPath = this.errorPath };
        }
    }

    public class WState : ICloneable
    {
        public List<VTreeNode> relPath;
        public List<Either<STreeNode, CTreeNode>> destPath;
        public AutomationElement curAutNode;

        public object Clone()
        {
            return 
                new WState
                { relPath = new List<VTreeNode>(this.relPath)
                           , destPath = new List<Either<STreeNode, CTreeNode>>(this.destPath)
                           , curAutNode = curAutNode };
        }
    }
}
