using CommunityToolkit.Mvvm.Input;
using Demo.AutoTest.Core.handler;
using Demo.Windows.Core.handler;
using FuX.Unility;
using Demo.Driver.ccd.tc261;

using Demo.Windows.Controls.chart;
using Demo.Windows.Controls.data;
using Demo.Windows.Controls.handler;
using FuX.Core.handler;
using Demo.Windows.Core.mvvm;
using ScottPlot.WPF;

using System.Collections.ObjectModel;
using FuX.Core.services;


namespace Demo.AutoTest.ViewModels.UserControls
{
    public class TestViewModel : NotifyObject
    {
        public TestViewModel()
        {
            

           
            //创建图表所需的基础数据
            chartOperate.InstanceBasics(new ChartData.Basics
            {
                XTitle = "波数",
                XTitleEN = "Wave Number",
                YTitle = "强度",
                YTitleEN = "Strength",
                ChartControl = ChartControl,
                LineAdjust = true,
                HideGrid = true
            });

            if (!chartOperate.On().GetDetails(out string? message))
            {
                Demo.Windows.Controls.message.MessageBox.Show(message, App.LanguageOperate.GetLanguageValue("提示"), Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Error);
            }

            //if (!operate.On().GetDetails(out message))
            //{
            //    Windows.Controls.message.MessageBox.Show(message, lang?.GetLanguageValue("提示"), Windows.Controls.@enum.MessageBoxButton.OK, Windows.Controls.@enum.MessageBoxImage.Error);
            //}

            //切换皮肤时
            SkinHandler.OnSkinEvent += (sender, e) =>
            {
                ItemsControlItemsSource = InitTabControlItemsSource();
            };
            //语言切换时
            FuX.Core.handler.LanguageHandler.OnLanguageEvent += (sender, e) =>
            {
                ItemsControlItemsSource = InitTabControlItemsSource();
            };

        }
      
        /// <summary>
        /// 图表操作
        /// </summary>
        ChartOperate chartOperate = ChartOperate.Instance();
        /// <summary>
        /// TC261
        /// </summary>
        TC261Operate operate = TC261Operate.Instance(new());

        /// <summary>
        /// 控件
        /// </summary>
        public WpfPlot ChartControl
        {
            get => chartControl;
            set => SetProperty(ref chartControl, value);
        }
        private WpfPlot chartControl = new WpfPlot();

        /// <summary>
        /// 采集
        /// </summary>
        public IAsyncRelayCommand Gather => new AsyncRelayCommand(GatherAsync);
        private async Task GatherAsync()
        {
            var result = await operate.GatherAsync(1000);
            if (!result.GetDetails(out string? message, out object? data))
            {
                await Demo.Windows.Controls.message.MessageBox.Show(message, App.LanguageOperate.GetLanguageValue("提示"), Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Error);
            }
            else
            {
                ChartControl.Create(data.GetSource<List<ushort>>(), Guid.NewGuid().ToNString());
                ChartControl.Adjust();
            }
        }

        /// <summary>
        /// 主界面菜单项集合
        /// </summary>
        public ObservableCollection<ItemsControlModel> ItemsControlItemsSource
        {
            get => GetProperty(() => ItemsControlItemsSource);
            set => SetProperty(() => ItemsControlItemsSource, value);
        }
        private ObservableCollection<ItemsControlModel> InitTabControlItemsSource()
        {
            ObservableCollection<ItemsControlModel> models = new(new()
            {
                new ItemsControlModel("光谱形状",System.Windows.Application.Current.TryFindResource("光谱形状"),App.LanguageOperate),
                new ItemsControlModel("光谱范围",System.Windows.Application.Current.TryFindResource("光谱范围"),App.LanguageOperate),
                new ItemsControlModel("暗电流",System.Windows.Application.Current.TryFindResource("暗电流"),App.LanguageOperate),
                new ItemsControlModel("暗噪声",System.Windows.Application.Current.TryFindResource("暗噪声"),App.LanguageOperate),
                new ItemsControlModel("光谱分辨率",System.Windows.Application.Current.TryFindResource("光谱分辨率"),App.LanguageOperate),
                new ItemsControlModel("位移准确度",System.Windows.Application.Current.TryFindResource("位移准确度"),App.LanguageOperate),
                new ItemsControlModel("位移重复性",System.Windows.Application.Current.TryFindResource("位移重复性"),App.LanguageOperate),
                new ItemsControlModel("位移分辨力",System.Windows.Application.Current.TryFindResource("位移分辨力"),App.LanguageOperate),
                new ItemsControlModel("强度重复性",System.Windows.Application.Current.TryFindResource("强度重复性"),App.LanguageOperate),
                new ItemsControlModel("灵敏度",System.Windows.Application.Current.TryFindResource("灵敏度"),App.LanguageOperate),
                new ItemsControlModel("信噪比",System.Windows.Application.Current.TryFindResource("信噪比"),App.LanguageOperate),
                new ItemsControlModel("二阶信噪比",System.Windows.Application.Current.TryFindResource("二阶信噪比"),App.LanguageOperate),
                new ItemsControlModel("三阶信噪比",System.Windows.Application.Current.TryFindResource("三阶信噪比"),App.LanguageOperate),
                new ItemsControlModel("长效稳定性",System.Windows.Application.Current.TryFindResource("长效稳定性"),App.LanguageOperate),
                new ItemsControlModel("TEC制冷能力",System.Windows.Application.Current.TryFindResource("TEC制冷能力"),App.LanguageOperate),
                new ItemsControlModel("TEC温度监控",System.Windows.Application.Current.TryFindResource("TEC温度监控"),App.LanguageOperate),
            });

            //获取选中的项
            if (ItemsControlItemsSource != null && ItemsControlItemsSource.Count > 0)
            {
                var item = ItemsControlItemsSource.GetCheckedItem();
                if (item != null)
                {
                    models.SetCheckedItem(item);
                }
            }

            return models;
        }

    }
}
