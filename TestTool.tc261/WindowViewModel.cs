using CommunityToolkit.Mvvm.Input;
using Demo.Driver.ccd.tc261;
using Demo.Windows.Controls.chart;
using Demo.Windows.Core.mvvm;
using FuX.Model.data;
using FuX.Unility;
using ScottPlot.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using FuX.Core.handler;
using System.Windows;
using Demo.Driver.ccd.ATP2000SH;
using Demo.Model.data;
using Demo.Driver.atp;
using System.IO.Ports;
using System.Collections.ObjectModel;
using Demo.Windows.Controls.filterDataGrid;
using Demo.Windows.Core.handler;
using Demo.AutoTest.view.userControls;

namespace TestTool.tc261
{
    class WindowViewModel : NotifyObject
    {
        private FilterDataGridOperate filterDataGridOperate;
        public WindowViewModel()
        {
            chartOperate = ChartOperate.Instance(new ChartData.Basics
            {
                XTitle = "波数",
                XTitleEN = "Wave Number",
                YTitle = "强度",
                YTitleEN = "Strength",
                ChartControl = ChartControl,
                LineAdjust = true,
                LegendRight = true,
                HideGrid = true
            });
            chartOperate.On();
            Employes = new ObservableCollection<Employe>();
            Employes.Add(new Employe("33", "33", 3, 2, null));
            Employes.Add(new Employe("22", "33", 3, 3, null));
            DisplayEmployes = new ObservableCollection<Employe>(Employes);
            filterDataGridControl.ItemsSource = DisplayEmployes;

            filterDataGridOperate = FilterDataGridOperate.Instance(new FilterDataGridData.Basics
            {

                FilterDataGrid = filterDataGridControl,


            });
            filterDataGridOperate.On();

            basics.SerialData = new FuX.Core.Communication.serial.SerialData.Basics();
            basics.SerialData.PortName = "COM8";

            _ATPOperate = ATPOperate.Instance(basics);

         //   SimControl = InjectionWpf.UserControl<SimView,SimViewModel>();

            //RibbonTest = InjectionWpf.UserControl<TestView, TestViewModel>();



        }

        public LanguageModel LanguageOperate { get; set; } = new("TestTool.tc261", "Language", "TestTool.tc261.dll");
        /// <summary>
        /// 图表操作
        /// </summary>
        ChartOperate chartOperate;
        /// <summary>
        /// TC261
        /// </summary>
        TC261Operate operate = TC261Operate.Instance(new());

        ATP2000Operate ATP2000Operate = ATP2000Operate.Instance(new());

        ATPData.Basics basics = new ATPData.Basics();
        
        ATPOperate _ATPOperate;
        private ObservableCollection<Employe> Employes;

        /// <summary>
        /// 信息框事件
        /// </summary>
        public IAsyncRelayCommand TextChanged { get => new AsyncRelayCommand<TextChangedEventArgs>(TextChangedAsync); }
        /// <summary>
        /// 信息框事件
        /// 让滚动条一直处在最下方
        /// </summary>
        public Task TextChangedAsync(TextChangedEventArgs? e)
        {
            TextBox textBox = e.Source.GetSource<TextBox>();
            textBox.SelectionStart = textBox.Text.Length;
            textBox.SelectionLength = 0;
            textBox.ScrollToEnd();
            return Task.CompletedTask;
        }
        /// <summary>
        /// 日志信息
        /// </summary>
        public string Log
        {
            get => GetProperty(() => Log);
            set => SetProperty(() => Log, value);
        }
        /// <summary>
        /// 日志显示
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public Task LogShow(string? msg, bool isDateTime = true)
        {
            if (msg.IsNullOrWhiteSpace())
                return Task.CompletedTask;
            return Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (Log?.Length > 10000)
                {
                    Log = string.Empty;
                }
                if (isDateTime)
                {
                    Log += $" {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} : {msg}\r\n";
                }
                else
                {
                    Log += $"{msg}\r\n";
                }
                return Task.CompletedTask;
            }).Result;
        }


        /// <summary>
        /// 控件
        /// </summary>
        public WpfPlot ChartControl
        {
            get => chartControl;
            set => SetProperty(ref chartControl, value);
        }
        private WpfPlot chartControl = new WpfPlot();

        public SimView SimControl
        {
            get => simControl;
            set => SetProperty(ref simControl, value);
        }

        public TestView RibbonTest
        {
            get => ribbonTest;
            set => SetProperty(ref ribbonTest, value);
        }


        /// <summary>
        /// 控件
        /// </summary>
        public FilterDataGrid FilterDataGridControl
        {
            get => filterDataGridControl;
            set => SetProperty(ref filterDataGridControl, value);
        }
        private FilterDataGrid filterDataGridControl = new FilterDataGrid();

        /// <summary>
        /// 打开
        /// </summary>
        public IAsyncRelayCommand Open => new AsyncRelayCommand(OpenAsync);
        private async Task OpenAsync()
        {
            //var result = await ATP2000Operate.OnAsync();
            var result = await _ATPOperate.OnAsync();
            await LogShow(result.Message);
            //ATP2000Operate.Init();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public IAsyncRelayCommand Close => new AsyncRelayCommand(CloseAsync);
        private async Task CloseAsync()
        {
            var result = await operate.OffAsync();
            await LogShow(result.Message);
        }

        /// <summary>
        /// 采集
        /// </summary>
        public IAsyncRelayCommand Gather => new AsyncRelayCommand(GatherAsync);
        private async Task GatherAsync()
        {
            var result = await ATP2000Operate.GatherAsync(1000);
            await LogShow(result.Message);
            if (result.GetDetails(out SpectrumDto? data))
            {
              //  ChartControl.Create(data.Wavelength,data.Intensity, DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                ChartControl.Adjust();
            }
        }

        /// <summary>
        /// 采集
        /// </summary>
        public IAsyncRelayCommand GetTemp => new AsyncRelayCommand(GetTempAsync);
        private async Task GetTempAsync()
        {
            var result = await operate.GetTemperatureAsync();
            await LogShow(result.Message);
            if (result.GetDetails(out float? data))
            {
                await Demo.Windows.Controls.message.MessageBox.Show(data.ToString(), LanguageOperate.GetLanguageValue("提示"), Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Information);
            }
        }


        /// <summary>
        /// 测试次数
        /// </summary>
        public int Count
        {
            get => count;
            set => SetProperty(ref count, value);
        }
        private int count = 99999;
        /// <summary>
        /// 间隔
        /// </summary>
        public int Interval
        {
            get => interval;
            set => SetProperty(ref interval, value);
        }
        private int interval = 100;

        /// <summary>
        /// 取消通知令牌
        /// </summary>
        private CancellationTokenSource? tokenSource;
        /// <summary>
        /// 开始
        /// </summary>
        public IAsyncRelayCommand Start => new AsyncRelayCommand(StartAsync);
        private async Task StartAsync()
        {
            if (tokenSource == null)
            {
                tokenSource = new CancellationTokenSource();
            }
            await PersistentTest(tokenSource.Token);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public IAsyncRelayCommand Stop => new AsyncRelayCommand(StopAsync);

        public ObservableCollection<Employe> DisplayEmployes { get; private set; }

        private async Task StopAsync()
        {
            if (tokenSource != null)
            {
                tokenSource.Cancel();
                tokenSource = null;
            }
            await LogShow(LanguageOperate.GetLanguageValue("测试已结束"));
        }
        /// <summary>
        /// 下标
        /// </summary>
        private int index = 0;
        private SimView simControl;
        private TestView ribbonTest;

        /// <summary>
        /// 持久化测试
        /// </summary>
        /// <returns></returns>
        public async Task PersistentTest(CancellationToken token)
        {
            try
            {
                await LogShow(LanguageOperate.GetLanguageValue("图表线条累计超十条则重置"));
                while (Count > 0 && !token.IsCancellationRequested)
                {
                    if (index >= 10)
                    {
                        ChartControl.RemoveAll();
                        index = 0;
                    }
                    index++;
                    Count--;



                    var result = await operate.GatherAsync(1000);
                    await LogShow(result.Message);

                    if (!result.Status)
                    {
                        return;
                    }

                    if (result.GetDetails(out List<ushort>? data))
                    {
                        ChartControl.Create(data, DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                        ChartControl.Adjust();
                    }
                    await Task.Delay(Interval);
                }
                await LogShow(LanguageOperate.GetLanguageValue("测试已完成"));
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception ex)
            {
                await LogShow($"{LanguageOperate.GetLanguageValue("测试异常")}:{ex.Message}");
            }
        }
    }
}
