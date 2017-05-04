using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Disposables;
using UIALib.Components;
using UIALib.Types;
using UIALib.Components.UIA;

namespace UIAUtils_Test
{
    class Program
    {
        //         static List<EWComp<object, object>> ewComps =
        //             new List<EWComp<object, object>>
        //                 {
        //                     new UIANode<string,UIEvent<object>>("Microsoft-Word"
        //                                , "Window-App"
        //                                , "Node-Creation"
        //                                , new Tree<string> {
        //                                     "Path"
        //                                , new List<Either<STreeNode,CTreeNode>>
        //                                    {
        //                                        new STreeNode
        //                                        {
        //                                            name = "MsoDockTop"
        //                                            , nextMove = Move.Child
        //                                        },
        //                                        new STreeNode
        //                                        {
        //                                            name = ""
        //                                            , nextMove = Move.Sibling
        //                                        },
        //                                        new STreeNode
        //                                        {
        //                                            name = ""
        //                                            , nextMove = Move.Child
        //                                        },
        //                                        new STreeNode
        //                                        {
        //                                            name = "Menú Lectura de pantalla completa"
        //                                            , nextMove = Move.Child
        //                                        },
        //                                        new STreeNode
        //                                        {
        //                                            name = ""
        //                                            , nextMove = Move.Child
        //                                        },
        //                                        new STreeNode
        //                                        {
        //                                            name = ""
        //                                            , nextMove = Move.Child
        //                                        },
        //                                        new STreeNode {
        //                                            name = "Menú Lectura de pantalla completa"
        //                                            , nextMove = Move.Child
        //                                        },
        //                                        new STreeNode {
        //                                            name = "Pestaña Archivo"
        //                                            , nextMove = Move.Sibling
        //                                        }, new STreeNode {
        //                                            name = "Herramientas"
        //                                            , nextMove = Move.Sibling
        //                                        }
        //                                    }
        //                                )
        //                 };
        //         static List<EComp> eComps =
        //             new List<EComp>
        //                 {
        //                     new AppWindowDetector()
        //                 };
        //         static List<WComp<object>> wComps =
        //             new List<WComp<object>>
        //                 {
        //                     new Logger()
        //                 };

        //Example code only

        static void Main(string[] args)
        {
            // foreach(var wcomp in wComps)
            // {
            //     foreach(var ecomp in eComps)
            //     {
            //         var res = wcomp.watchedComps.Find((s) => s == ecomp.name);
            //         if (res != null)
            //         {
            //             ecomp.Subscribe(wcomp);
            //         }
            //     }
            // }

            // EComp windowsDetector = AppWindowDetector.appWindowDetector();
            // WComp<object> windowsWatcher = new WindowsAppDetectorWatcher();
            // windowsDetector.Subscribe(windowsWatcher);

            var uiActions = UIAACompActionRecorderK.uiAComActionRecorder();
            var uiActionW = new UICompActionRecorderW();
            uiActions.Subscribe(uiActionW);

            var uMouse = UnderMouseUIA.underMouseUIA();
            var uMouseWatcher = new UnderMouseWatcher();
            uMouse.Subscribe(uMouseWatcher);
            uMouse.Subscribe(uiActions);

            var mStop = MouseStopped.mouseStopped();
            var mWatcher = new MouseWatcherC();
            mStop.Subscribe(mWatcher);

            mStop.Subscribe(uMouse);

            var mMouse = MouseMoved.mouseMoved();
            mMouse.Subscribe(mStop);
            Console.ReadLine();
        }
    }
}
