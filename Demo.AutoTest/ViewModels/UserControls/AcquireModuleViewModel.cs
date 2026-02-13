using CommunityToolkit.Mvvm.Input;
using Demo.AutoTest.Core.handler;
using Demo.AutoTest.data;
using Demo.AutoTest.services;
using Demo.AutoTest.Views.Modules;
using Demo.AutoTest.Views.UserControls;
using Demo.AutoTest.Views.UserControls.Bars;
using Demo.AutoTest.ViewModels.Modules;
using Demo.AutoTest.ViewModels.UserControls.Bars;
using Demo.AutoTest.ViewModels.Windows;
using Demo.Communication.constant;
using Demo.Core.extend;
using Demo.Driver.service;
using Demo.Model.data;
using Demo.Model.entities;
using Demo.Model.@enum;
using Demo.Windows.Controls.data;
using Demo.Windows.Controls.handler;
using Demo.Windows.Core.handler;
using Demo.Windows.Core.mvvm;
using Dm.util;
using FuX.Core.db;
using FuX.Core.services;
using FuX.Model.data;
using FuX.Unility;
using Newtonsoft.Json;
using ScottPlot.WPF;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls;
using MessageBox = Demo.Windows.Controls.message.MessageBox;

namespace Demo.AutoTest.ViewModels.UserControls
{
    public class AcquireModuleViewModel : NotifyObject
    {
        private AcquireModuleState _model = new AcquireModuleState();   
        private IAcquireModuleService _acquireModuleService;
        private RightSpectrumListViewModel _rightSpectrumListViewModel;
        private TopLeftBarViewModel _topLeftViewModel;
        private BottomHistoryAreaViewModel _bottomHistoryAreaViewModel;
        private IConnectedDeviceService _connectedDeviceService;
        private DBOperate dbOperate;
        private ISpectrumService _spectrumService;
        private IDeviceService _deviceService;
        private ILocalize _localize;
        /// <summary>
        /// 需要采集的光栅
        /// </summary>
        private List<GSColInfo> ColList = new List<GSColInfo>();
        private int maxCountCol = 0;//最大采集次数
        private int curCountCol = 0;//当前采集次数

        public AcquireModuleViewModel()
        {
           
            MiddleGraphArea = InjectionWpf.GetService<MiddleGraphArea>();
            RightParamView = InjectionWpf.GetService<RightParamView>();
            _topLeftBarView = InjectionWpf.GetService<TopLeftBarView>();
            RightSpectrumListView = InjectionWpf.GetService<RightSpectrumListView>();
            BottomHistoryAreaView= InjectionWpf.GetService<BottomHistoryAreaView>();
            _acquireModuleService = InjectionWpf.GetService<IAcquireModuleService>();
            _rightSpectrumListViewModel = InjectionWpf.GetService< RightSpectrumListViewModel>();       
            _topLeftViewModel = InjectionWpf.GetService<TopLeftBarViewModel>();
            _bottomHistoryAreaViewModel = InjectionWpf.GetService<BottomHistoryAreaViewModel>();         
            _bottomHistoryAreaViewModel.OnDataEvent += gridViewHistory_DoubleClick;
            _spectrumService = InjectionWpf.GetService<ISpectrumService>();         
            _localize = InjectionWpf.GetService<ILocalize>();
            _connectedDeviceService = InjectionWpf.GetService<IConnectedDeviceService>();
            _deviceService = _connectedDeviceService.DefaultService;
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

            _topLeftViewModel. chartOperate.Create(new() { SN = "Cpu", Title = "处理器", TitleEN = "Cpu" });
            //InitializeSpectrumTreeList();
            //InitializeHistoryGridView();
        }

        private async void  gridViewHistory_DoubleClick(object sender, EventDataResult e)
        {
            var spec = e.GetSource<HistorySpectrumBrowseStructuralBody>();
            var spectrumDto = await _spectrumService.GetAsync(spec.SpectrumId);
        }


        /// <summary>
        /// 控件
        /// </summary>
        public MiddleGraphArea MiddleGraphArea
        {
            get
            {
                return GetProperty(() => MiddleGraphArea);
            }
            set
            {
                SetProperty(() => MiddleGraphArea, value);
            }
        }

        public RightParamView RightParamView
        {
            get
            {
                return GetProperty(() => RightParamView);
            }
            set
            {
                SetProperty(() => RightParamView, value);
            }
        }

        private TopLeftBarView _topLeftBarView;

        public BottomHistoryAreaView BottomHistoryAreaView
        {
            get
            {
                return GetProperty(() => BottomHistoryAreaView);
            }
            set
            {
                SetProperty(() => BottomHistoryAreaView, value);
            }
        }

        public RightSpectrumListView RightSpectrumListView
        {
            get
            {
                return GetProperty(() => RightSpectrumListView);
            }
            set
            {
                SetProperty(() => RightSpectrumListView, value);
            }
        }

        public AcquireModuleState Model
        {
            get
            {
                return GetProperty(() => Model);
            }
            set
            {
                SetProperty(() => Model, value);
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
                new ItemsControlModel("采集",System.Windows.Application.Current.TryFindResource("光谱形状"),App.LanguageOperate,tes),
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

            InitializeSpectrumTreeList();
            InitializeHistoryGridView();

            return models;
        }

        private void InitializeSpectrumTreeList()
        {
            _model.SpectrumTreeNodes.Clear();
            _model.ZedTypeBox = ZedTypeBox.D;
            _acquireModuleService.AddRootSpectrumTreeNode(_model);
            _rightSpectrumListViewModel.AcquireModuleState = _model;
           
            _rightSpectrumListViewModel.RefreshDataSource();



        }


        private async Task InitializeHistoryGridView()
        {
            _bottomHistoryAreaViewModel.AcquireModuleState = _model;
            dbOperate = DBOperate.Instance();
            _model.SpectrumHistory.Clear();

           Task.Run(() =>
            {          
                var history =  dbOperate.Query<Spectrum>(c => 1 == 1).GetSource<List<Spectrum>>().Select() ;

                history?.ToList().ForEach(x => _model.SpectrumHistory.Add(x));
            }).Wait();

            _bottomHistoryAreaViewModel.RefreshDataSource();
        }

        /// <summary>
        /// 单次采集
        /// </summary>
        public IAsyncRelayCommand SingleAcquireCommand => new AsyncRelayCommand(SingleAcquireAsync);

        private async Task SingleAcquireAsync()
        {         
            _deviceService.CollStopStatic = false;
            await Task.Run(async () =>
            {
                await GetRangeData();
            });
        }

        #region 按钮事件

        private void tes(object sender , EventArgs e)
        {

           // _deviceService.AcquireDataNotCheck();
        }


        //public async Task<SpectrumDto> SingleAcquireDy(bool isJx = false)
        //{
        //    await Task.Run(async () =>
        //    {
        //        await GetRangeData();
        //    });
        //}

        /// <summary>
        /// 获取范围数据
        /// </summary>
        public async Task GetRangeData()
        {
            string paramInfo = _localize.GetCfg<string>(_userCfg.SpecParaJson);
            spectrumEt spectrum = JsonConvert.DeserializeObject<spectrumEt>(paramInfo);

            var d = InjectionWpf.GetService<IConnectedDeviceService>();
            var c = _connectedDeviceService.DefaultService;
            //发送指令
            var min = spectrum.Range1.Minimum;

            var max = spectrum.Range1.Maximum;

            if (spectrum.RangeType == 0)
            {
                    ///all光栅全谱图
                    var minobj = _deviceService.DevInfos.GratingBindWel.FirstOrDefault(t => t.point == _deviceService.DevInfos.GratingBindWel.FirstOrDefault()?.point);
                    var maxobj = _deviceService.DevInfos.GratingBindWel.FirstOrDefault(t => t.point == _deviceService.DevInfos.GratingBindWel.LastOrDefault()?.point);
                    min = minobj.shownums;
                    max = maxobj.shownume;

                    foreach (var obj in _deviceService.DevInfos.GratingBindWel)
                    {
                        var mincheck = obj.shownums;
                        var maxcheck = obj.shownume;

                        if (minobj.point == obj.point)
                        {
                            var minnum = min > mincheck ? min : mincheck;
                            DataToGSColInfo(obj, minnum, maxcheck);
                        }
                        else if (maxobj.point == obj.point)
                        {
                            var maxnum = maxcheck > max ? max : maxcheck;
                            DataToGSColInfo(obj, mincheck, maxnum);
                        }
                        else if (obj.point > minobj.point && obj.point < maxobj.point)
                        {
                            DataToGSColInfo(obj, mincheck, maxcheck);
                        }
                    }
               
            }
            else
            {
                var minobj = _deviceService.DevInfos.GratingBindWel.Where(t => min >= t.shownums && min <= t.shownume).OrderBy(t => t.startdis).LastOrDefault();
                var maxobj = _deviceService.DevInfos.GratingBindWel.Where(t => max >= t.shownums && max <= t.shownume).OrderBy(t => t.startdis).LastOrDefault();

                foreach (var obj in _deviceService.DevInfos.GratingBindWel)
                {

                    var mincheck = obj.shownums;
                    var maxcheck = obj.shownume;

                    var minnum = min > mincheck ? min : mincheck;
                    var maxnum = maxcheck > max ? max : maxcheck;

                    //DataToGSColInfo(obj, min, max);
                    if (minobj.point == obj.point)
                    {
                        DataToGSColInfo(obj, minnum, maxnum);
                    }
                    else if (maxobj.point == obj.point)
                    {
                        DataToGSColInfo(obj, minnum, maxnum);
                    }
                    else if (obj.point > minobj.point && obj.point < maxobj.point)
                    {
                        DataToGSColInfo(obj, mincheck, maxcheck);
                    }
                }
            }
            ColList = ColList.OrderBy(t => t.min).ToList();
            maxCountCol = ColList.Count;
            foreach (var rangenum in ColList)
            {
                var minnum = rangenum.min;
                var maxnum = rangenum.max;
               
                minnum = minnum + rangenum.c1r1 + rangenum.c2r1;
                maxnum = maxnum + rangenum.c1r1 + rangenum.c2r1;
                  
                    
                

                var minaddress = BitConverter.GetBytes(minnum);//采集起点

                var maxaddress = BitConverter.GetBytes(maxnum);//采集终点

                var type = 0x00; //_deviceService.PramInfo.RangeType == 1 ? 0x00 : 0x01;

                //获取数据
                var ret =(int) await _deviceService.Acquire(new List<byte>(){
                    minaddress[3], minaddress[2], minaddress[1], minaddress[0], maxaddress[3], maxaddress[2], maxaddress[1], maxaddress[0],(byte)type}.ToArray());
               
                if (ret != 0)
                {
                    if (ret == 1)
                        await MessageBox.Show(_localize.GetString(LocalizeConstant.COLLECT_STATUS_1), "全局异常捕获", Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Exclamation);
                    if (ret == 2)
                        await MessageBox.Show(_localize.GetString(LocalizeConstant.COLLECT_STATUS_2), "全局异常捕获", Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Exclamation);
                    if (ret == 3)
                        await MessageBox.Show(_localize.GetString(LocalizeConstant.COLLECT_STATUS_3), "全局异常捕获", Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Exclamation);
                    
                    return;
                }
                await GetCollData();
            }
        }

        /// <summary>
        /// 生成采集数据集合
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        private async void DataToGSColInfo(GratingBindWel obj, double min, double max)
        {
            //先判断最小波长在哪一段
            //通过该段的系数计算出最小值
            //再判断最大波长在哪一段
            //通过该断系数计算出最大值
            var nums = (int)Math.Ceiling(min);

            var nume = (int)Math.Ceiling(max);

            if (!obj.modelFile && obj.cof.Count > 0 && obj.cof.Count > 0)
            {
                nums = (int)CommonFuncHandler.SetYGetXVal(obj.cof.ToArray(), min);
                if (nums < 0) nums = 0;

                nume = (int)CommonFuncHandler.SetYGetXVal(obj.cof.ToArray(), max);
            }
            

            if (_localize.GetCfg<int>(_userCfg.SpectrumXAxisUnit) == 2)
            {
                ///CCD范围
                var objlist = _deviceService.Device.CCDInfo?.CCDRangeInfos;
                if (objlist != null)
                {
                    //满足范围的数组
                    var objin = objlist.Where(t => (min >= t.s && t.e >= min) || (max >= t.s && t.e >= max)).ToList();
                    var num = 0;
                    var objc1 = objin.FirstOrDefault(t => t.k == 1);
                    if (objc1 != null)
                    {
                        var minnum = (int)Math.Ceiling(CommonFuncHandler.SetYGetXVal(obj.cof.ToArray(), objc1.s));
                        minnum = nums > minnum ? nums : minnum;
                        var maxnum = (int)Math.Ceiling(CommonFuncHandler.SetYGetXVal(obj.cof.ToArray(), objc1.e));
                        maxnum = maxnum > nume ? nume : maxnum;
                        ColList.Add(new GSColInfo() { point = obj.point, min = minnum, max = maxnum, c1r1 = obj.c1r1num, c1r2 = obj.c1r2num, c2r1 = 0, c2r2 = 0 });
                        num = num + 1;
                    }
                    var objc2 = objin.FirstOrDefault(t => t.k == 2);
                    if (objc2 != null)
                    {
                        var minnum = (int)Math.Ceiling(CommonFuncHandler.SetYGetXVal(obj.cof.ToArray(), objc2.s));
                        minnum = nums > minnum ? nums : minnum;
                        var maxnum = (int)Math.Ceiling(CommonFuncHandler.SetYGetXVal(obj.cof.ToArray(), objc2.e));
                        maxnum = maxnum > nume ? nume : maxnum;
                        ColList.Add(new GSColInfo() { point = obj.point, min = minnum, max = maxnum, c1r1 = 0, c1r2 = 0, c2r1 = obj.c2r1num, c2r2 = obj.c2r2num });
                    }
                }
               

            }
            else
            {
                ColList.Add(new GSColInfo() { point = obj.point, min = nums, max = nume, c1r1 = obj.c1r1num, c1r2 = obj.c1r2num, c2r1 = obj.c2r1num, c2r2 = obj.c2r2num });
            }

        }

        List<DYDataInfo> dYDataInfos = new List<DYDataInfo>();
        /// <summary>
        /// 获取数据
        /// </summary>
        private async Task GetCollData()
        {
            string paramInfo = _localize.GetCfg<string>(_userCfg.SpecParaJson);
            spectrumEt spectrum = JsonConvert.DeserializeObject<spectrumEt>(paramInfo);
            if (curCountCol == 0)
            {
                dYDataInfos = new List<DYDataInfo>();
            }
            List<byte> llist = new List<byte>();        
            var dataBuff = new List<DYDataInfo>();
            int index = 0;

            var sn = Guid.NewGuid().toString();
            _topLeftViewModel.chartOperate.Create(new() { SN = sn, Title = sn, TitleEN = "test" });
            //获取数据
            while (!_deviceService.CollStopStatic)
            {

             
                var data = _deviceService.AcquireDataNotCheck(new List<int>() { 2, 2, 3 }, ref llist);

                if (data.Count > 0)
                {
                    //InitSingleAcquireProgress(true, _localize.GetString(LocalizeConstant.COLLECTION_DATAING));
                   // barButtonStopMapping.Enabled = true;
                }
                foreach (var list in data)
                {
                    if (_deviceService.CollStopStatic) return;
                    var ccd1 = (int)list[0];   //探测器1

                    var ccd2 = (int)list[1];   //探测器2
                    var address = (int)list[2];   //编码器
                    var retstr = list[3];   //编码器
                    ////采集完成
                    if (ccd1 + ccd2 + address <= 0)
                    {
                        curCountCol++;

                        if (curCountCol == maxCountCol)
                        {

                           
                            //数据存储
                            //await CrateATP2000Data(dYDataInfos);
                        }
                        return;
                    }
                    double xnum = 0;
                    double ynum = 0;
                    if (spectrum.CollectTypes == CollectType.Range)
                    {
                        //显示每条数据

                        xnum = address;//x数据

                        var obj = _deviceService.DevInfos.GratingBindWel ;
                        var cofobj = _deviceService.DevInfos.GratingBindWel.FirstOrDefault();

                        
                       
                           
                        cofobj = _deviceService.DevInfos.GratingBindWel.Where(x => xnum >= x.startdis)
                                    ?.OrderByDescending(x => x.startdis)
                            ?.FirstOrDefault();
                        if (cofobj == null)
                                continue;
                       

                        var bcdata = ColList[curCountCol];

                       

                        Console.WriteLine(JsonConvert.SerializeObject(cofobj) + retstr + "-" + xnum);
                        if (cofobj.cof.Count() > 0 || cofobj.modelFile && (_localize.GetCfg<int>(_userCfg.SpectrumXAxisUnit) == 2 || _localize.GetCfg<int>(_userCfg.SpectrumXAxisUnit) == 3))
                        {
                            if (xnum > 260000)
                            {
                                
                            }
                            
                         xnum = CommonFuncHandler
                                    .SetXGetYVal(cofobj.cof.ToArray(), xnum);
                            

                        }

                    }
                    else
                    {
                        //xnum = _deviceService.PramInfo.PonitBC;
                    }






                    if (_localize.GetCfg<int>(_userCfg.PointLineTest) == 0)
                    {
                        var num = 0;
                        if (_deviceService.Device.CCDInfo == null)
                        {
                            num = 1;
                        }
                        else
                            num = _deviceService.Device.CCDInfo.CCDRangeInfos.Where(obj => obj.s <= xnum && obj.e >= xnum).FirstOrDefault()?.k ?? 0;

                        double cof = 1;
                        

                        if (num == 1)
                        {
                            ynum = ccd1;


                            ynum = Math.Round(ynum * cof, 1);
                        }
                        else if (num == 2)
                        {
                            //这里这样写的原因是有可能两个光栅只装一个探测器，和下位机的约定是每次都会发两个探测器的数据，只装一个则第二个发零
                            ynum = ccd2 == 0 ? ccd1 : ccd2;
                            ynum = Math.Round(ynum * cof, 1);
                        }
                        else
                        {
                            continue;
                        }
                    }

                 


                   

                   
                    //选用探测器的数据
                    var savedata = new DYDataInfo() { CCD1 = (int)ccd1, CCD2 = (int)ccd2, address = address, timenum = xnum, xnum = xnum, ynum = ynum };
                    Console.WriteLine(JsonConvert.SerializeObject(savedata) + "-" + retstr);
                    dYDataInfos.Add(savedata);
                    dataBuff.Add(savedata);

                   

                    //显示的数据扣除暗底
                   // if (_deviceService.OperateType == Model.Devices.OperateType.Data || _deviceService.OperateType == Model.Devices.OperateType.White)
                    {
                        var tempdata = _deviceService.Device.SpectrumDataDark;
                        ynum = ynum - (tempdata == null || tempdata.Intensity == null || tempdata.Intensity.Length <= 0 ? 0 : tempdata.Intensity[dYDataInfos.Count - 1]);
                       
                    }

                   
                    {
                        bool isRefresh = true;
                        if (dataBuff.Count() >= 15)
                        {
                            isRefresh = true;
                            dataBuff.Clear();
                        }
                        else
                        {
                            isRefresh = false;
                        }


                         _topLeftViewModel.chartOperate.Update(sn, xnum, ynum);
                      

                        // GetAcquireModule().ShowSpectrumData(xnum, ynum, _deviceService.ShowLineNum, _localize.GetCfg<CollectType>(_userCfg.CollectTypes) == CollectType.Range ? 0 : 2048, isRefresh);
                    }

                }
            }
        }
       
        #endregion



    }
}
