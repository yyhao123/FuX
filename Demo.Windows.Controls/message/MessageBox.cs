
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using FuX.Unility;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using MessageBoxButton = Demo.Windows.Controls.@enum.MessageBoxButton;
using MessageBoxImage = Demo.Windows.Controls.@enum.MessageBoxImage;
using MaterialDesignThemes.Wpf;

namespace Demo.Windows.Controls.message
{
    /// <summary>
    /// 消息框
    /// </summary>
    public class MessageBox
    {
        /// <summary>
        /// 设置消息图标
        /// </summary>
        private static ImageSource SetIcon(MessageBoxImage iconType)
        {
#pragma warning disable CS8603 // 可能返回 null 引用。
            return iconType switch
            {
                MessageBoxImage.Exclamation => CreateIconBitmap(SystemIcons.Exclamation),
                MessageBoxImage.Application => CreateIconBitmap(SystemIcons.Application),
                MessageBoxImage.Asterisk => CreateIconBitmap(SystemIcons.Asterisk),
                MessageBoxImage.Error => CreateIconBitmap(SystemIcons.Error),
                MessageBoxImage.Hand => CreateIconBitmap(SystemIcons.Hand),
                MessageBoxImage.Information => CreateIconBitmap(SystemIcons.Information),
                MessageBoxImage.Question => CreateIconBitmap(SystemIcons.Question),
                MessageBoxImage.Shield => CreateIconBitmap(SystemIcons.Shield),
                MessageBoxImage.Warning => CreateIconBitmap(SystemIcons.Warning),
                MessageBoxImage.WinLogo => CreateIconBitmap(SystemIcons.WinLogo),
                _ => null  // 默认无图标
            };
#pragma warning restore CS8603 // 可能返回 null 引用。
        }

        /// <summary>
        /// 数据
        /// </summary>
        private static ConcurrentDictionary<Icon, BitmapSource> IconArry = new ConcurrentDictionary<Icon, BitmapSource>();

        /// <summary>
        /// 从系统图标创建位图源
        /// </summary>
        private static BitmapSource CreateIconBitmap(Icon ic)
        {
            BitmapSource bitmapSource = null;
            if (!IconArry.TryGetValue(ic, out BitmapSource? source))
            {
                using (Icon originalIcon = ic)
                {
                    using (Icon icon = (Icon)originalIcon.Clone())
                    {
                        bitmapSource = Imaging.CreateBitmapSourceFromHIcon(
                            icon.Handle,
                            Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());
                        IconArry.TryAdd(ic, bitmapSource);
                    }
                }
            }
            else
            {
                bitmapSource = source;
            }
            return bitmapSource;
        }

        /// <summary>
        /// 标志位
        /// </summary>
        private static string SignPosition = "DialogHost";



        /// <summary>
        /// 显示提示框
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="btn">按钮</param>
        /// <param name="img">图标</param>
        /// <returns>状态</returns>
        public static async Task<bool> Show(string content, string title, MessageBoxButton btn, MessageBoxImage img)
        {
            try
            {
                MessageModel messageModel = new MessageModel();
                messageModel.ContentIcon = SetIcon(img);
                messageModel.Content = content;
                messageModel.Title = title;
                object obj = null;
                switch (btn)
                {
                    case MessageBoxButton.OK:
                        OK oK = new OK();
                        oK.DataContext = messageModel;
                        obj = oK;
                        break;
                    case MessageBoxButton.OKCancel:
                        OKCancel oKCancel = new OKCancel();
                        oKCancel.DataContext = messageModel;
                        obj = oKCancel;
                        break;
                    case MessageBoxButton.Yes:
                        Yes yes = new Yes();
                        yes.DataContext = messageModel;
                        obj = yes;
                        break;
                    case MessageBoxButton.YesNo:
                        YesNo yesNo = new YesNo();
                        yesNo.DataContext = messageModel;
                        obj = yesNo;
                        break;
                }

                return (await DialogHost.Show(obj, SignPosition))?.GetSource<bool>() ?? false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 显示提示框
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="btn">按钮</param>
        /// <returns>状态</returns>
        public static async Task<bool> Show(string content, string title, MessageBoxButton btn) => await Show(content, title, btn, MessageBoxImage.Information);

        /// <summary>
        /// 显示提示框
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="img">图标</param>
        /// <returns>状态</returns>
        public static async Task<bool> Show(string content, string title, MessageBoxImage img) => await Show(content, title, MessageBoxButton.OK, img);

        /// <summary>
        /// 显示提示框
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <returns>状态</returns>
        public static async Task<bool> Show(string content, string title) => await Show(content, title, MessageBoxButton.OK, MessageBoxImage.Information);

        /// <summary>
        /// 显示提示框
        /// </summary>
        /// <param name="content">内容</param>
        /// <returns>状态</returns>
        public static async Task<bool> Show(string content) => await Show(content, string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
