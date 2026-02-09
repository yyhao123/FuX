using Demo.AutoTest.Core.handler;
using Demo.Windows.Controls.chart;
using Demo.Windows.Core.handler;
using Demo.Windows.Core.mvvm;
using FuX.Core.handler;
using ScottPlot.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.AutoTest.viewModel.userControls.bars
{
    public class TopLeftViewModel:NotifyObject
    {
       
        public TopLeftViewModel()
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
                Windows.Controls.message.MessageBox.Show(message, App.LanguageOperate.GetLanguageValue("提示"), Windows.Controls.@enum.MessageBoxButton.OK, Windows.Controls.@enum.MessageBoxImage.Error);
            }
            // chartOperate.ShowSampleData();
           
        }

        public string id = Guid.NewGuid().ToString();

        /// <summary>
        /// 图表操作
        /// </summary>
        public ChartOperate chartOperate = ChartOperate.Instance();
        /// <summary>
        /// 控件
        /// </summary>
        public WpfPlot ChartControl
        {
            get => chartControl;
            set => SetProperty(ref chartControl, value);
        }
        private WpfPlot chartControl = new WpfPlot();
    }
}
