
using Demo.Driver.atp;
using FuX.Core.db;
using FuX.Core.extend;
using FuX.Model.entities;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestTool.db
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DBOperate dbOperate;
        ATPOperate atpOperate;
        public MainWindow()
        {
            InitializeComponent();
            dbOperate=CoreUnify<DBOperate, DBData>.Instance();
            atpOperate = CoreUnify<ATPOperate,ATPData.Basics>.Instance();

           var  dbOperate2 = CoreUnify<DBOperate, DBData>.Instance();

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
           await dbOperate.OnAsync();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await dbOperate.CreateAsync<ConfigInfo>();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
          
            var ret = dbOperate.Query<ConfigInfo>(c => 1 == 1);
            ret = dbOperate.Query<c_Command>(c => c.cmdNum == "0xD0");
            // var d = ret.GetSource<List<ConfigInfo>>();
            // var ret = dbOperate.Query<c_Command>(c => 1== 1);
            // var d = ret.GetSource<List<ConfigInfo>>();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            byte d = 0xd0;
            var d1 = atpOperate.ComSerialPortAsk(d, "dd", null);
        }

        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
           await atpOperate.OnAsync();
        }
    }
}