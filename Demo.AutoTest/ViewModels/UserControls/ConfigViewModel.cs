using Demo.AutoTest.Core.handler;
using Demo.AutoTest.Views.UserControls;
using Demo.AutoTest.Views.UserControls.Bars;
using Demo.AutoTest.ViewModels.UserControls.Bars;
using Demo.Windows.Controls.data;
using Demo.Windows.Controls.handler;
using Demo.Windows.Controls.pages;
using Demo.Windows.Core.handler;
using Demo.Windows.Core.mvvm;
using FuX.Core.handler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.AutoTest.ViewModels.UserControls
{
    public class ConfigViewModel : NotifyObject
    {
       
        private object middleGraphArea;

        public ConfigViewModel()
        {
            //获取语言实例
        
            middleGraphArea = InjectionWpf.UserControl<ConfigManagerView, ConfigManagerViewModel>(true);

            ItemsControlItemsSource = InitTabControlItemsSource();

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
        /// 控件
        /// </summary>
        public object MiddleGraphArea
        {
            get => middleGraphArea;
            set => SetProperty(ref middleGraphArea, value);
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
                new ItemsControlModel("指令管理",System.Windows.Application.Current.TryFindResource("指令管理"),App.LanguageOperate,OnCmdView),
                new ItemsControlModel("型号配置管理",System.Windows.Application.Current.TryFindResource("型号配置管理"),App.LanguageOperate,OnDevCmdInfoView),
                new ItemsControlModel("设备管理",System.Windows.Application.Current.TryFindResource("设备管理"),App.LanguageOperate),
                new ItemsControlModel("配置管理",System.Windows.Application.Current.TryFindResource("配置管理"),App.LanguageOperate,OnConfigView),
                new ItemsControlModel("语言管理",System.Windows.Application.Current.TryFindResource("语言管理"),App.LanguageOperate),
                new ItemsControlModel("定标拟合",System.Windows.Application.Current.TryFindResource("定标拟合"),App.LanguageOperate),
                new ItemsControlModel("脉冲补偿定标",System.Windows.Application.Current.TryFindResource("脉冲补偿定标"),App.LanguageOperate),
                new ItemsControlModel("定标调试",System.Windows.Application.Current.TryFindResource("定标调试"),App.LanguageOperate),
                new ItemsControlModel("链接设备",System.Windows.Application.Current.TryFindResource("链接设备"),App.LanguageOperate),
                new ItemsControlModel("重启软件",System.Windows.Application.Current.TryFindResource("重启软件"),App.LanguageOperate),

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

        private void OnDevCmdInfoView(object? sender, EventArgs e)
        {
            MiddleGraphArea = InjectionWpf.UserControl<DevCmdInfoView, DevCmdInfoViewModel>(true);
        }

        #region 按钮事件

        private void OnCmdView(object sender, EventArgs e)
        {
            MiddleGraphArea = InjectionWpf.UserControl<CMDManagerView, CMDManagerViewModel>(true);
        }

        private void OnConfigView(object sender, EventArgs e)
        {
            MiddleGraphArea = InjectionWpf.UserControl<ConfigManagerView, ConfigManagerViewModel>(true);

        }


        #endregion
    }
}
