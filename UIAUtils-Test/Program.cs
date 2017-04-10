using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using UIALib.Components;
using UIALib.Types;
using UIALib;
using LanguageExt;

namespace UIAUtils_Tes
{
    class Program
    {
        static List<EWComp<object, object>> ewComps =
            new List<EWComp<object, object>>
                {
                    new UIANode("Microsoft-Word"
                               , "Window-App"
                               , new Dictionary<string, string>
                                    { {"Backend", "UI-Automation"} }
                               , new List<string>
                                    { "Node-Creation" }
                               , new List<Either<STreeNode,CTreeNode>>
                                   {
                                       new STreeNode
                                       {
                                           name = "MsoDockTop"
                                           , nextMove = Move.Child
                                       },
                                       new STreeNode
                                       {
                                           name = ""
                                           , nextMove = Move.Sibling
                                       },
                                       new STreeNode
                                       {
                                           name = ""
                                           , nextMove = Move.Child
                                       },
                                       new STreeNode
                                       {
                                           name = "Menú Lectura de pantalla completa"
                                           , nextMove = Move.Child
                                       },
                                       new STreeNode
                                       {
                                           name = ""
                                           , nextMove = Move.Child
                                       },
                                       new STreeNode
                                       {
                                           name = ""
                                           , nextMove = Move.Child
                                       },
                                       new STreeNode {
                                           name = "Menú Lectura de pantalla completa"
                                           , nextMove = Move.Child
                                       },
                                       new STreeNode {
                                           name = "Pestaña Archivo"
                                           , nextMove = Move.Sibling
                                       }, new STreeNode {
                                           name = "Herramientas"
                                           , nextMove = Move.Sibling
                                       }
                                   }
                               )
                };
        static List<EComp> eComps =
            new List<EComp>
                {
                    new AppWindowDetector()
                };
        static List<WComp<object>> wComps =
            new List<WComp<object>>
                {
                    new Logger()
                };

        static void Main(string[] args)
        {
            foreach(var wcomp in wComps)
            {
                foreach(var ecomp in eComps)
                {
                    var res = wcomp.watchedComps.Find((s) => s == ecomp.name);
                    if (res != null)
                    {
                        ecomp.Subscribe(wcomp);
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
