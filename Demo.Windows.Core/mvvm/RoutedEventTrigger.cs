using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Demo.Windows.Core.mvvm
{
    public class RoutedEventTrigger : EventTriggerBase<DependencyObject>
    {
        private RoutedEventHandler _routedEventHandler;

        public RoutedEvent RoutedEventName { get; set; }

        protected override string GetEventName()
        {
            if (RoutedEventName == null)
            {
                throw new InvalidOperationException("RoutedEventTrigger 的 RoutedEvent 属性未设置。");
            }
            return RoutedEventName.Name;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            dynamic val = ResolveEventSource();
            if (val == null)
            {
                throw new InvalidOperationException("RoutedEventTrigger 只能附加到 FrameworkElement 或 FrameworkContentElement 或其行为（Behavior）上。");
            }
            if (RoutedEventName == null)
            {
                throw new InvalidOperationException("请设置 RoutedEventTrigger 的 RoutedEvent 属性。");
            }
            _routedEventHandler = OnRoutedEvent;
            val.AddHandler(RoutedEventName, _routedEventHandler);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            dynamic val = ResolveEventSource();
            if (val != null && _routedEventHandler != null)
            {
                val.RemoveHandler(RoutedEventName, _routedEventHandler);
            }
        }

        private void OnRoutedEvent(object sender, RoutedEventArgs args)
        {
            OnEvent(args);
        }

        private dynamic ResolveEventSource()
        {
            if (base.AssociatedObject is FrameworkElement result)
            {
                return result;
            }
            if (base.AssociatedObject is FrameworkContentElement result2)
            {
                return result2;
            }
            if (base.AssociatedObject is Behavior behavior)
            {
                if (((IAttachedObject)behavior).AssociatedObject is FrameworkElement result3)
                {
                    return result3;
                }
                if (((IAttachedObject)behavior).AssociatedObject is FrameworkContentElement result4)
                {
                    return result4;
                }
            }
            return null;
        }
    }

}
