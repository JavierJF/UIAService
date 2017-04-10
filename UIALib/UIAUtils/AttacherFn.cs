using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using UIALib.Types;
using LanguageExt;
using static LanguageExt.Prelude;

namespace UIALib.UIAUtils
{
    public class AttacherFn
    {
        public static Option<string> listenToProperty(string property
                                           , AutomationElement elem
                                           , AutomationEventHandler handler)
        {
            if (property == "Invoke")
            {
                Automation.AddAutomationEventHandler(InvokePattern.InvokedEvent,
                     elem, TreeScope.Element, handler);
                return None;
            }
            else if (property == "Menu-Expansion")
            {
                return None;
            }
            else if (property == "Toogle-Componnent")
            {
                return None;
            }
            else
            {
                return "Fail";
            }
        }
    }
}
