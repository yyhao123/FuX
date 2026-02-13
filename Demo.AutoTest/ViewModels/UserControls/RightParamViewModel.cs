using CommunityToolkit.Mvvm.Input;
using Demo.AutoTest;
using Demo.AutoTest.data;
using Demo.AutoTest.@enum;
using Demo.AutoTest.Views.UserControls;
using Demo.AutoTest.ViewModels.UserControls;
using Demo.Communication.constant;
using Demo.Driver.service;
using Demo.Model.data;
using Demo.Model.entities;
using Demo.Model.@enum;
using Demo.Windows.Controls.filterDataGrid;
using Demo.Windows.Controls.property.wpf;
using Demo.Windows.Core.handler;
using Demo.Windows.Core.mvvm;
using Dm.util;
using FuX.Core.handler;
using FuX.Core.services;
using FuX.Unility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using Range = Demo.Model.data.Range;



namespace Demo.AutoTest.ViewModels.UserControls
{
    public class RightParamViewModel : NotifyObject
    {     
        public CustomPropertyGridControlFactory CustomPropertyGridControlFactory { get; set; }

        public CustomPropertyGridOperator CustomPropertyGridOperator { get; set; }

        public List<GratingBindWel> gratingBindWels = new List<GratingBindWel>();

        private IConnectedDeviceService _connectedDeviceService;

        public IDeviceService _deviceService;

        private ILocalize _localize;

        public RightParamViewModel()
        {
            this.CustomPropertyGridControlFactory = new CustomPropertyGridControlFactory();
            this.CustomPropertyGridOperator = new CustomPropertyGridOperator();
            SpectrumEt = new spectrumEt() { RangeType=RangeTypeEm.All, Range1 = new Range {  Maximum=100,Minimum=0} };
            _connectedDeviceService = InjectionWpf.GetService<IConnectedDeviceService>();
            _deviceService = _connectedDeviceService.DefaultService;
            _localize = InjectionWpf.GetService<ILocalize>();
            

        }

     
        /// <summary>
        /// 采集参数
        /// </summary>
        public spectrumEt SpectrumEt
        {
            get
            {
                return GetProperty(() => SpectrumEt);
            }
            set
            {
                SetProperty(() => SpectrumEt, value);
            }
        }

      



        /// <summary>
        /// 双击
        /// </summary>
        public IAsyncRelayCommand SetParmCommand => new AsyncRelayCommand(SetParmAsync);

        public async Task SetParmAsync()
        {
           //判断设备状态
            var checkState = _deviceService.IsDeviceIdle();
            if (!checkState.Item1)
            {
                await Demo.Windows.Controls.message.MessageBox.Show(checkState.Item2, "全局异常捕获", Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Exclamation);      
                return;
            }
            try
            {
                //切换设备状态
                if (!_deviceService.ChangeDeviceState(DeviceState.Running, _deviceService.Device.DeviceStates))
                {
                    var info = _localize.GetString(LocalizeConstant.DEVICE_STATUS, (int)_deviceService.Device.DeviceStates);
                    await Demo.Windows.Controls.message.MessageBox.Show(info, "全局异常捕获", Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Exclamation);                 
                    return;
                }
                
                _deviceService.Device.CCDInfo = _deviceService.DevInfos.CCDInfos.Where(c => c.point == ((int)SpectrumEt.CCDName).toString())?.First();
                //获取参数
                await GetDataPram();
                var pram = JsonConvert.DeserializeObject<spectrumEt>(_localize.GetCfg<string>(_userCfg.SpecParaJson));
                if (pram == null) return;
                if(SpectrumEt.CCDType == CCDType.Ponit)
                {//单元相机
                   await _deviceService.SetGear(pram.GainSelectedItem.val);
                   await _deviceService.SetRate(pram.GatherRateSelectedItem.val);

                }
                await Demo.Windows.Controls.message.MessageBox.Show("操作成功", "全局异常捕获", Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Exclamation);
            }
            catch (Exception ex)
            {
                await Demo.Windows.Controls.message.MessageBox.Show(ex.Message, "全局异常捕获", Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Exclamation);
            }
            finally
            {
                _deviceService.ChangeDeviceState(DeviceState.Idle, DeviceState.Running);
            }
        }

        private async Task<bool> GetDataPram()
        {
            if(SpectrumEt is null)
            {
                await Demo.Windows.Controls.message.MessageBox.Show("参数为空", "全局异常捕获", Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Exclamation);
                return false;
            }
           
            var jsonStr = JsonConvert.SerializeObject(SpectrumEt);
            _localize.SaveCfg(_userCfg.SpecParaJson, jsonStr);
            return true;

        }
    }

   

   

   

   
}

