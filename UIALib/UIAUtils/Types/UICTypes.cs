using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace UIALib.UIAUtils.Types {

    public enum UIControlType {
        Unknown,
        ExpandableMenu,
        SelectableMenu,
        Invokable,
        Toggleable,
        Value
    };

    public class UICTypes {
        /// <summary>
        /// Looks like standard procedure try to cast the pattern in order
        /// to obtain which one it's, 
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        public static UIControlType getType(AutomationElement elem) {
            object pattern;

            try {
                if (elem.TryGetCurrentPattern(SelectionItemPattern.Pattern, out pattern)) {
                    return UIControlType.SelectableMenu;
                } else if (elem.TryGetCurrentPattern(TogglePattern.Pattern, out pattern)) {
                    return UIControlType.Toggleable;
                } else if (elem.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out pattern)) {
                    return UIControlType.ExpandableMenu;
                } else if (elem.TryGetCurrentPattern(ValuePattern.Pattern, out pattern)) {
                    return UIControlType.Value;
                }
            } catch {
            }

            return UIControlType.Unknown;
        }
    }
}
