using System.Collections.Concurrent;
using System.Text;
using System.Threading.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChannelTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ConcurrentDictionary<CancellationTokenSource, Task>? TaskArray;
        private Channel<AddressValue>? DataQueue;


        private BoundedChannelOptions channelOptions = new BoundedChannelOptions(int.MaxValue)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = false,
            SingleWriter = false
        };
        public MainWindow()
        {
            InitializeComponent();
            On();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            AddressValue value = new AddressValue();
            CancellationToken token = new CancellationToken();
            value.SN = "111";
            await DataQueue.Writer.WriteAsync(value, token).ConfigureAwait(continueOnCapturedContext: false);
        }

        private void On()
        {

            if (DataQueue == null)
            {
                DataQueue = Channel.CreateBounded<AddressValue>(channelOptions);
            }
            if (TaskArray == null)
            {
                TaskArray = new ConcurrentDictionary<CancellationTokenSource, Task>();
                for (int i = 0; i < 5; i++)
                {
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                    TaskArray.TryAdd(cancellationTokenSource, TaskHandle(cancellationTokenSource.Token));
                }
            }

         }

        private async Task TaskHandle(CancellationToken token)
        {
            _ = 1;
            try
            {
                while (await DataQueue.Reader.WaitToReadAsync(token).ConfigureAwait(continueOnCapturedContext: false))
                
                {
                    AddressValue item;
                    while (DataQueue.Reader.TryRead(out item))
                    {
                        if (item != null && !token.IsCancellationRequested)
                        {
                            
                        }
                    }
                }

            }
            catch (TaskCanceledException)
            {
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception)
            {
            }
        }


    }

    public class AddressValue
    { 
       public string SN {  get; set; }
    }

}