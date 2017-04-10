using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using LanguageExt;
using static LanguageExt.Prelude;
using UIALib;

namespace UIAUtils_Tes
{
    class Program
    {
        public static List<VTreeNode> logPath =
            new List<VTreeNode>
            {
                { new VTreeNode {name = "Word"} }
                , { new VTreeNode {name = "MsoDockTop"} }
                , { new VTreeNode {name = "&Menú"} }
                , { new VTreeNode {name = "Menú"} }
                , { new VTreeNode {name = "panel"} }
                , { new VTreeNode {name = "Panel"} }
            };

        public static readonly List<Either<STreeNode, CTreeNode>> testPath =
            new List<Either<STreeNode, CTreeNode>> {
                new STreeNode { name = "Menú Lectura de pantalla completa"
                              , nextMove = Move.Child }
                , new STreeNode { name = "Pestaña Archivo"
                              , nextMove = Move.Sibling }
                , new STreeNode { name = "Herramientas"
                              , nextMove = Move.Sibling }
                , new CTreeNode { sNode = new STreeNode { name = "Vista", nextMove = Move.Child }
                                , action = NodeAction.Expand
                                , path = logPath
                                , postActionEvents =
                                    new Tuple<AutomationEvent, StructureChange>(
                                        AutomationElementIdentifiers.MenuOpenedEvent
                                        , new StructureChange { changeType = StructureChangeType.ChildAdded
                                                              , scope = TreeScope.Children }
                                    )}
                , new STreeNode { name = ""
                                , nextMove = Move.Child}
                , new STreeNode { name = "Menú Grupo"
                                , nextMove = Move.Child}
                , new STreeNode { name = ""
                                , nextMove = Move.Sibling }
                , new STreeNode { name = ""
                                , nextMove = Move.Sibling }
                , new STreeNode { name = ""
                                , nextMove = Move.Child }
            };

        static void Main(string[] args)
        {
            TF.manualTreeTrasversing(testPath);
        }
    }
}
