using CommunityToolkit.Mvvm.Input;

using Demo.AutoTest.view.userControls;

using Demo.Communication.constant;
using Demo.Driver.service;
using Demo.Model.data;
using Demo.Model.@enum;
using Demo.Windows.Controls.handler;
using Demo.Windows.Core.@enum;
using Demo.Windows.Core.handler;
using Demo.Windows.Core.mvvm;
using FuX.Core.handler;
using Dm;
using FuX.Core.services;
using FuX.Model.data;
using FuX.Model.@enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls;
using LanguageHandler = FuX.Core.handler.LanguageHandler;
using TestTool.tc261;

namespace Demo.AutoTest.viewModel.windows
{
    /// <summary>
    /// 主要窗口视图模型
    /// </summary>
    public class MainViewModel:NotifyObject
    {
       

        private IDeviceService _deviceService;

        private ILocalize _localize;

        private IConnectedDeviceService _connectedDeviceService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lang">全局语言</param>
        public MainViewModel()
        {
          
            LanguageHandler.OnLanguageEvent += LanguageHandler_OnLanguageEvent;
            SkinHandler.OnSkinEventAsync += SkinHandler_OnSkinEventAsync;
           

            //NavigationView 控件多语言操作步骤
            MenuItemsSource = MenuItemsOperate(App.LanguageOperate);   //给菜单项赋值
            FooterMenuItemsSource = FooterMenuItemsOperate(App.LanguageOperate);  //给底部菜单项赋值
        }

      

        /// <summary>
        /// 语言切换触发
        /// </summary>
        private void LanguageHandler_OnLanguageEvent(object? sender, FuX.Model.data.EventLanguageResult e)
        {

            SystemTitle = InitSystemTitle
                ();


            if (e.GetDetails(out LanguageType? language))
            {
                switch (language ??= LanguageType.zh)
                {
                    case LanguageType.zh:
                    case LanguageType.en:

                        break;
                }
            }
           
        }

        /// <summary>
        /// 皮肤切换触发
        /// </summary>
        private Task SkinHandler_OnSkinEventAsync(object? sender, Windows.Core.data.EventSkinResult e)
        {
            if (e.GetDetails(out SkinType? skin))
            {
                switch (skin ??= SkinType.Dark)
                {
                    case SkinType.Dark:
                    case SkinType.Light:
                        break;
                }
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// 系统标题
        /// </summary>
        public string SystemTitle
        {
            get => systemTitle;
            set => SetProperty(ref systemTitle, value);
        }
        private string systemTitle = $"Optosky - 拉曼自动测试软件";
        public string InitSystemTitle()
        {
            return $"Optosky - {App.LanguageOperate.GetLanguageValue("拉曼自动测试软件")}";
        }

        #region 菜单项

        /// <summary>
        /// 菜单项数据源
        /// </summary>
        public ICollection<object> MenuItemsSource
        {
            get => GetProperty(() => MenuItemsSource);
            set => SetProperty(() => MenuItemsSource, value);
        }
        /// <summary>
        /// 底部菜单项数据源
        /// </summary>
        public ICollection<object> FooterMenuItemsSource
        {
            get => GetProperty(() => FooterMenuItemsSource);
            set => SetProperty(() => FooterMenuItemsSource, value);
        }

        /// <summary>
        /// 菜单项操作
        /// </summary>
        /// <returns>返回新的菜单项</returns>
        public ObservableCollection<object> MenuItemsOperate(LanguageModel model)
            => new ObservableCollection<object>
            {
               WpfUiHandler.CreationControl("Test",SymbolRegular.Organization24,typeof(TestView),true,model),
            
              

            };

        /// <summary>
        /// 底部菜单项操作
        /// </summary>
        /// <returns>返回新的菜单项</returns>
        public ObservableCollection<object> FooterMenuItemsOperate(LanguageModel model)
            => new ObservableCollection<object>
            {
              // WpfUiHandler.CreationControl("Help", SymbolRegular.ChatHelp24, typeof(HelpView),true,model)
            };

        public IAsyncRelayCommand LoadDataCommand => new AsyncRelayCommand(LoadDataAsync);

        public async Task LoadDataAsync()
        {
            await InitDevice();
        }

        /// <summary>
        /// 初始化设备串口
        /// </summary>
        /// <returns></returns>
        private async Task InitDevice()
        {
            try
            {
                var list = await _deviceService.RetrieveATRCom();
                var defdev = _localize.GetCfg<List<DevInfoCom>>(UserCfgInfoConstantCom.MasterDev) ?? new List<DevInfoCom>();
                if (defdev.Count() > 0)
                {
                    var strlist = defdev.Where(t => t.IsAutoCon).Select(t => t.DevName);
                    var obj = list.Where(t => strlist.Contains(t.DevName)).ToList();
                    if (obj.Count() > 0)
                    {
                        _deviceService.Open(obj.Select(t => t.COM).ToList());
                        _deviceService.Device.SN = obj[0].SN;
                        _deviceService.Device.DevName = obj[0].DevName;
                        _deviceService.Device.DeviceStates = DeviceState.Idle;
                       //  barStaticItemDev.Caption = _localize.GetFormatString(LocalizeConstantUV.DeviceONLINE, _deviceService.Device.DevName);
                       // var resstr = await _deviceService.CloseCollection();//停止采集  避免客户异常关闭设置的情况前还在采集
                    }
                }
               // await _connectedDeviceService.DefaultService.GetDevInfoAsync();
            }
            catch (Exception ex)
            {
              
            }
        }
        #endregion
    }
}
