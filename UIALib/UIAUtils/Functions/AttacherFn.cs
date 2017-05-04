using System;
using System.Windows.Automation;

namespace UIALib.Utils.Functions
{

    class EA
    {
        public static void structChanged(AutomationElement elem
                                        , StructureChangedEventHandler handler)
        {
            Automation.AddStructureChangedEventHandler(elem
                                                      , TreeScope.Element
                                                      , handler);
        }

        public static Action<AutomationElement> menuExpanded
            (AutomationElement elem , AutomationEventHandler handler)
        {
            Automation.AddAutomationEventHandler(AutomationElement.MenuOpenedEvent
                                                , elem
                                                , TreeScope.Element
                                                , handler);

            Action<AutomationElement> removeEvent =
                (auElem) => {
                    Automation.
                        RemoveAutomationEventHandler
                            (AutomationElement.MenuOpenedEvent
                            , elem
                            , handler);
                };

            return removeEvent;
        }

        public static Action<AutomationElement> itemInvoked
            (AutomationElement elem, AutomationEventHandler handler)
        {
            Automation.AddAutomationEventHandler(InvokePattern.InvokedEvent
                                                , elem
                                                , TreeScope.Element
                                                , handler);

            Action<AutomationElement> removeEvent =
                (auElem) => {
                    Automation.
                        RemoveAutomationEventHandler
                            (InvokePattern.InvokedEvent
                            , elem
                            , handler);
                };

            return removeEvent;
        }

        public static Action<AutomationElement> toggledItem 
            (AutomationElement elem , AutomationPropertyChangedEventHandler handler)
        {
            Automation.AddAutomationPropertyChangedEventHandler
                ( elem
                , TreeScope.Element
                , handler
                , TogglePattern.ToggleStateProperty);

            Action<AutomationElement> removeEvent =
                (auElem) => {
                    Automation.
                        RemoveAutomationPropertyChangedEventHandler
                            (elem
                            , handler);
                };

            return removeEvent;
        }

        public static Action<AutomationElement> selectedMenuItem
            (AutomationElement elem, AutomationEventHandler handler)
        {
            Automation.AddAutomationEventHandler(SelectionItemPattern.ElementSelectedEvent
                                                , elem
                                                , TreeScope.Element
                                                , handler);

            Action<AutomationElement> removeEvent =
                (auElem) => {
                    Automation.
                        RemoveAutomationEventHandler
                            (SelectionItemPattern.ElementSelectedEvent
                            , elem
                            , handler);
                };

            return removeEvent;
        }

        public static Action<AutomationElement> selectedValue
            (AutomationElement elem, AutomationPropertyChangedEventHandler handler)
        {
            Automation.
                AddAutomationPropertyChangedEventHandler
                    ( elem
                    , TreeScope.Element
                    , handler
                    , ValuePattern.ValueProperty);


            Action<AutomationElement> removeEvent =
                (auElem) => {
                    Automation.
                        RemoveAutomationPropertyChangedEventHandler
                            (elem
                            , handler);
                };

            return removeEvent;
        }

    }
}
