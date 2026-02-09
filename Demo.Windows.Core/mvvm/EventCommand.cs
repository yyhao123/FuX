using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Demo.Windows.Core.mvvm
{
    /// <summary>
    /// 事件命令 需要到nuget 安装 Microsoft.Xaml.Behaviors.Wpf 界面引用 http://schemas.microsoft.com/xaml/behaviors
    /// </summary>
    public class EventCommand : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty CommandParateterProperty = DependencyProperty.Register("CommandParateter", typeof(object), typeof(EventCommand), new PropertyMetadata(null));

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(EventCommand), new PropertyMetadata(null));

        /// <summary>
        /// 事件要绑定的命令
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// 绑定命令的参数，保持为空就是事件的参数
        /// </summary>
        public object CommandParateter
        {
            get { return GetValue(CommandParateterProperty); }
            set { SetValue(CommandParateterProperty, value); }
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Invoke(object parameter)
        {
            if (CommandParateter != null)
                parameter = CommandParateter;
            var cmd = Command;
            if (cmd != null)
                cmd.Execute(parameter);
        }
    }
}
