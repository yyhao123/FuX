using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Demo.Windows.Controls.textBox
{
    /// <summary>
    /// TextBoxControl.xaml 的交互逻辑
    /// </summary>
    public partial class TextBoxControl : UserControl, IComponentConnector
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(TextBoxControl), new PropertyMetadata(null));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(object), typeof(TextBoxControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty HintProperty = DependencyProperty.Register("Hint", typeof(string), typeof(TextBoxControl), new PropertyMetadata(string.Empty));

        public ImageSource Icon
        {
            get
            {
                return (ImageSource)GetValue(IconProperty);
            }
            set
            {
                SetValue(IconProperty, value);
            }
        }

        public object Text
        {
            get
            {
                return GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public string Hint
        {
            get
            {
                return (string)GetValue(HintProperty);
            }
            set
            {
                SetValue(HintProperty, value);
            }
        }
        public TextBoxControl()
        {
            InitializeComponent();
        }
    }
}
