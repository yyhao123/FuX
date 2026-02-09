using CommunityToolkit.Mvvm.Input;
using Demo.Windows.Core.mvvm;
using Fluent.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TestTool.tc261
{
    public class GalleryDataItemViewModel:BindNotify
    {
        /// <summary>
        /// Creates new item
        /// </summary>
        /// <param name="icon">Icon</param>
        /// <param name="iconLarge">Large Icon</param>
        /// <param name="text">Text</param>
        /// <param name="group">Group</param>
        /// <returns>Item</returns>
        public static GalleryDataItemViewModel Create(string icon, string iconLarge, string text, string group)
        {
            var dataItem = new GalleryDataItemViewModel(icon, iconLarge, text, group);

            return dataItem;
        }

        private GalleryDataItemViewModel(string icon, string iconLarge, string text, string group)
        {
            this.Icon = (ImageSource?)StaticConverters.ObjectToImageConverter.Convert(icon, typeof(BitmapImage), null, null);
            this.IconLarge = (ImageSource?)StaticConverters.ObjectToImageConverter.Convert(iconLarge, typeof(BitmapImage), null, null);
            this.Text = text;
            this.Group = group;

            this.Command = new RelayCommand(() => Trace.WriteLine("Command executed"));
        }

        /// <summary>
        /// Gets or sets icon
        /// </summary>
        public ImageSource? Icon { get; }

        /// <summary>
        /// Gets or sets large icon
        /// </summary>
        public ImageSource? IconLarge { get; }

        /// <summary>
        /// Gets or sets text
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets or sets group name
        /// </summary>
        public string Group { get; }

        public ICommand Command { get; }
    }
}
