using Demo.AutoTest.Core.handler;
using Demo.AutoTest.Views.UserControls;
using Demo.Windows.Controls.handler;
using Demo.Windows.Core;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Demo.AutoTest.Views.Windows
{
    public partial class MainView : WindowBase
    {
        public MainView()
        {
            InitializeComponent();
            NavigationViewControls.SelectNavigationViewDefaultItem(this, typeof(AcquireModuleView), App.LanguageOperate,0);
        }
    }
}
