using Demo.Windows.Core.@enum;
using Demo.Windows.Core.handler;
using FuX.Core.extend;
using FuX.Model.data;
using ScottPlot.Panels;
using ScottPlot;
using ScottPlot.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using System.Windows;

namespace Demo.Windows.Controls.filterDataGrid
{
    public class FilterDataGridOperate:CoreUnify<FilterDataGridOperate,FilterDataGridData.Basics>,IDisposable
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public FilterDataGridOperate() : base() { }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="basics">基础数据</param>
        public FilterDataGridOperate(FilterDataGridData.Basics basics) : base(basics) { }

        //private void SkinHandler_OnSkinEvent(object? sender, Core.data.EventSkinResult e)
        //{
        //    Style(e.Skin ??= SkinType.Dark, wpfPlot);
        //}

        #region 接口重写参数
        /// <inheritdoc/>
        protected override string CD => "支持表格的部分操作";

        /// <inheritdoc/>
        protected override string CN => "FilterDataGrid";

        /// <inheritdoc/>
        public override LanguageModel LanguageOperate { get; set; } = new("Demo.Windows.Controls", "Language", "Demo.Windows.Controls.dll");

        /// <inheritdoc/>
        public override void Dispose()
        {
           // Off();
            base.Dispose();
        }
        /// <inheritdoc/>
        public override async Task DisposeAsync()
        {
           // Off();
            await base.DisposeAsync();
        }
        #endregion

        /// <summary>
        /// 表格控件
        /// </summary>
        private FilterDataGrid filterDataGrid;

        /// <summary>
        /// 重置
        /// </summary>
        private void Reset()
        {
            //对象赋值
            filterDataGrid ??= basics.FilterDataGrid;
          
            switch (GetLanguage())
            {
                case FuX.Model.@enum.LanguageType.zh:
                   
                    break;
                case FuX.Model.@enum.LanguageType.en:
                    
                    break;
            }
           

            OnLanguageEventAsync -= ChartOperate_OnLanguageEventAsync;
            OnLanguageEventAsync += ChartOperate_OnLanguageEventAsync;

            ////这是默认值
            //DefaultMenu(wpfPlot);
            //SkinHandler.OnSkinEvent -= SkinHandler_OnSkinEvent;
            //SkinHandler.OnSkinEvent += SkinHandler_OnSkinEvent;
            //刷新
            
        }

        /// <summary>
        /// 打开
        /// </summary>
        /// <returns>统一出参</returns>
        public OperateResult On()
        {
            BegOperate();
            try
            {
                if (GetStatus().GetDetails(out string? message))
                {
                    return EndOperate(false, message);
                }
                Reset();
                return EndOperate(true);
            }
            catch (Exception ex)
            {
                return EndOperate(false, ex.Message, exception: ex);
            }
        }

        /// <summary>
        /// 获取状态
        /// </summary>
        /// <returns></returns>
        public OperateResult GetStatus()
        {
            BegOperate();
            try
            {
                if (filterDataGrid != null)
                {
                    return EndOperate(true);
                }
                return EndOperate(false);
            }
            catch (Exception ex)
            {
                return EndOperate(false, ex.Message, exception: ex);
            }
        }

        /// <summary>
        /// 如果语言切换则会进来
        /// </summary>
        private Task ChartOperate_OnLanguageEventAsync(object? sender, EventLanguageResult e)
        {
            {
                var paletteHelper = new PaletteHelper();
                Theme theme = paletteHelper.GetTheme();
                //if (theme.GetBaseTheme() == BaseTheme.Dark)
                //{
                //    Application.Current.Resources.Remove("DataGridLight.xaml");
                //    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
                //    {
                //        Source = new Uri("pack://application:,,,/Demo.Windows.Controls;component/filterDataGrid/themes/DataGridDark.xaml", UriKind.RelativeOrAbsolute)
                //    });

                //}
                //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
                //{
                //    Source = new Uri("pack://application:,,,/Demo.Windows.Controls;component/filterDataGrid/themes/Generic.xaml", UriKind.RelativeOrAbsolute)
                //});
            }
            return Task.CompletedTask;
        }
    }
}
