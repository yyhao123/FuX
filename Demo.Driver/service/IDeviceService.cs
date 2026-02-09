using Demo.Communication.constant;
using Demo.Communication.potocols;
using Demo.Model.data;
using Demo.Model.entities;
using Demo.Model.@enum;
using Demo.Windows.Core.handler;
using FuX.Core.db;
using FuX.Core.services;
using FuX.Model.entities;
using FuX.Unility;
using Newtonsoft.Json;
using ScottPlot;
using ScottPlot.TickGenerators.TimeUnits;
using SharpPcap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Demo.Driver.service
{
    public partial interface IDeviceService : IDeviceServiceCom
    {

        public string id {  get; set; }
        /// <summary>
        /// 动力学设置的参数(背景参数)
        /// </summary>
        dynamicsEt BackDynamicsPramInfo { get; set; }
        SpectrumDto CreateSpectrumDto(Spectrum spectrum, SpectrumDataRaw spectrumRaw, SpectrumDataDark spectrumDark, SpectrumDataWhiteBoard spectrumDataWhiteBoard = null);

        Tuple<bool, string> SaveSpectrum(Spectrum spectrum, SpectrumDataRaw spectrumRaw, SpectrumDataDark spectrumDark, SpectrumDataWhiteBoard spectrumDataWhiteBoard = null);


        /// <summary>
        ///保存测光数据
        /// </summary>
        /// <param name="spectrum"></param>
        /// <param name="spectrumRaws"></param>
        /// <param name="spectrumDark"></param>
        /// <param name="spectrumDataWhiteBoards"></param>
        /// <returns></returns>
        Tuple<bool, string, AbsStandardSpectrumData, SpectrumDto> SaveAbsSpectrum(Spectrum spectrum,
            List<AbsStandardDetailData> standardDetailDatas, List<AbsSampleSpectrumData> sampleSpectrumDatas, SpectrumDataDark spectrumDark, bool isSave = false);

        /// <summary>
        /// 修改测光标准数据
        /// </summary>
        /// <param name="spectrum"></param>
        /// <param name="standardDetailDatas"></param>
        /// <param name="spectrumDark"></param>
        /// <param name="standardId"></param>
        /// <returns></returns>
        Tuple<bool, string, AbsStandardSpectrumData, SpectrumDto> UpdateAbsStandardSpectrum(Spectrum spectrum,
           List<AbsStandardDetailData> standardDetailDatas, SpectrumDataDark spectrumDark, string standardId);

        #region 设备指令
        Device Device { get; }

        /// <summary>
        /// 设备的配置信息
        /// </summary>
        //DevInfos DevInfos { get; set; }

        bool ChangeDeviceState(DeviceState state, DeviceState currentState = DeviceState.Idle);
        #endregion

        /// <summary>
        /// 光谱参数
        /// </summary>
        string SpecPramInfo { get; set; }

        /// <summary>
        /// 设备的配置信息
        /// </summary>
        DevInfos DevInfos { get; set; }



        /// <summary>
        /// 采集状态
        /// </summary>
        bool CollStopStatic { get; set; }

        /// <summary>
        /// 电机是否全都停止
        /// </summary>
        bool IsStopAllMotor { get; set; }


        /// <summary>
        /// 类型
        /// </summary>
        DataOperateType OperateType { get; set; }

        /// <summary>
        /// 采集类型
        /// </summary>
        DisplayDataType DisplayDataType { get; set; }

        /// <summary>
        /// 连续采集时显示的线条
        /// </summary>
        int ShowLineNum { get; set; }

        /// <summary>
        /// 自检状态
        /// </summary>
        /// <returns></returns>
        Task<string> DevSelfTest();

        /// <summary>
        /// 设置设备已自检
        /// </summary>
        /// <returns></returns>
        Task<bool> SetDevSelfTest();

        /// <summary>
        /// 获取下位机系数
        /// </summary>
        /// <returns></returns>
        Task<List<double>> GetCoeff();

        /// <summary>
        /// 多段拟合系数
        /// </summary>
        /// <returns></returns>

        Task<string> GetCoeffNew();

        /// <summary>
        ///停止采集
        /// </summary>
        /// <returns></returns>
        Task<bool> CloseCollection(bool isManualStop = false);



        /// <summary>
        /// 定点采集设置波长位置
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        Task<Tuple<bool, string>> SetFixedPointWelAddress(int num);

        /// <summary>
        /// 定点采集
        /// </summary>
        /// <returns></returns>
        Task<Tuple<bool, string>> FixedPointCollection();

        /// <summary>
        /// 设置档位
        /// </summary>
        /// <param name="Gear"></param>
        /// <returns></returns>
        Task<Tuple<bool, string>> SetGear(int Gear, int collInterval);

        /// <summary>
        /// 波长范围设置
        /// </summary>
        /// <param name="snum"></param>
        /// <param name="endnum"></param>
        /// <returns></returns>
        Task<bool> SetWelRangeAddress(int snum, int endnum);

        Task<string> GetDevSN();

        /// <summary>
        /// 设置比色皿
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        Task<bool> SetCuvetteNo(List<string> ls);

        /// <summary>
        /// 设置比色皿
        /// </summary>
        /// <param name="cuvetteNo"></param>
        /// <returns></returns>
        Task<bool> SetCuvetteNo(int cuvetteNo);



        /// <summary>
        /// 设置测光采集波长
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        Task<Tuple<bool, string>> SetAbsWelAddress(int num);

        /// <summary>
        /// 测光采集
        /// </summary>
        /// <returns></returns>
        Task<List<List<object>>> MeteringCollection();

        /// <summary>
        /// 写入波长系数
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        Task<bool> SetWaveLengthCoefficient(string hex);

        /// <summary>
        /// 清除氙灯闪烁次数
        /// </summary>
        /// <returns></returns>
        Task<bool> ResetUseNum();

        /// <summary>
        /// 获取氙灯闪烁次数
        /// </summary>
        /// <returns></returns>
        Task<int> GetXenonLampCout();

        /// <summary>
        /// 光谱采集(全谱)
        /// </summary>
        /// <returns></returns>
        Task<Tuple<bool, string>> FullSpectrum();

        /// <summary>
        /// 光谱采集(范围)
        /// </summary>
        /// <returns></returns>
        Task<Tuple<bool, string>> RangeSpectrum();

        /// <summary>
        /// 设置滤光片脉冲值
        /// </summary>
        /// <param name="value">脉冲值</param>
        /// <param name="CmdLGPZF">FF正转 00反转</param>
        /// <returns></returns>
        Task<bool> SetFilterPulseValue(int value, string CmdLGPZF);

        /// <summary>
        /// 记录滤光片电机位置
        /// </summary>
        /// <param name="value">位置值</param>
        /// <returns></returns>
        Task<bool> RecordFilterPosition(int value);

        /// <summary>
        /// 设置滤光片到达指定位置
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> SetFilterPosition(int value);

        /// <summary>
        /// 滤光片切换
        /// </summary>
        /// <param name="groupNo"></param>
        /// <param name="value"></param>
        /// <param name="filterNo"></param>
        /// <returns></returns>
        Task<bool> FilterChange(int groupNo, int value, int filterNo);

        /// <summary>
        /// 档位切换
        /// </summary>
        /// <param name="gropuNo">档位顺序</param>
        /// <param name="gearNo">档位选择</param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> GearShift(int gropuNo, int gearNo, int value);
        /// <summary>
        /// 设置比色皿脉冲值
        /// </summary>
        /// <param name="value">脉冲值</param>
        /// <param name="CmdCuvetteZF">>FF正转 00反转</param>
        /// <returns></returns>
        Task<bool> SetCuvetteMC(int value, string CmdCuvetteZF);

        /// <summary>
        /// 设置比色皿到达记录位置
        /// </summary>
        /// <param name="value">比色皿号</param>
        /// <returns></returns>
        Task<bool> SetCuvetteRecordPosition(int value);

        /// <summary>
        /// 记录比色皿位置
        /// </summary>
        /// <param name="value">比色皿位置</param>
        /// <returns></returns>
        Task<bool> RecordCuvettePosition(int value);

        /// <summary>
        /// 转动光栅
        /// </summary>
        /// <param name="Gs1">是否是光栅1</param>
        /// <param name="num">脉冲值</param>
        /// <param name="CmdGSZF">FF正转 00反转</param>
        /// <returns></returns>
        Task<bool> SetGSValue(bool Gs1, int num, string CmdGSZF);

        /// <summary>
        /// 设置光栅到达记录位置
        /// </summary>
        /// <param name="Gs1"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        Task<bool> SetToRecordGsPosition(bool Gs1, int num);

        /// <summary>
        /// 记录光栅记录位置
        /// </summary>
        /// <param name="Gs1"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        Task<bool> RecordGsPosition(bool Gs1, int num);

        /// <summary>
        /// 设置光源脉冲值
        /// </summary>
        /// <param name="value">脉冲值</param>
        /// <param name="CmdGYZF">FF正转 00反转</param>
        /// <returns></returns>
        Task<bool> SetGYPulseValue(int value, string CmdGYZF);

        /// <summary>
        ///  设置光源到达记录位置
        /// </summary>
        /// <param name="value">位置号</param>
        /// <returns></returns>
        Task<bool> SetGYToRecord(int value);

        /// <summary>
        /// 记录光源到达位置
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> RecordGYPosition(int value);

        /// <summary>
        /// 光源选择
        /// </summary>
        /// <param name="value">0关灯 1氘灯 2卤素灯</param>
        /// <returns></returns>
        Task<bool> SelectLightSource(int value);

        /// <summary>
        /// 设置狭缝脉冲值
        /// </summary>
        /// <param name="value">脉冲值</param>
        /// <param name="CmdXFZF">FF正转 00反转</param>
        /// <returns></returns>
        Task<bool> SetXFPulseValue(int value, string CmdXFZF);

        /// <summary>
        /// 记录狭缝位置
        /// </summary>
        /// <param name="no">位置号</param>
        /// <returns></returns>
        Task<bool> RecordXFPosition(int no);

        /// <summary>
        ///  设置狭缝到达位置
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        Task<Tuple<bool, string>> SetXFToRecord(int no, int no2);

        /// <summary>
        /// 设置滑动电机脉冲值
        /// </summary>
        /// <param name="value">脉冲值</param>
        /// <param name="CmdHDZF">FF正转 00反转</param>
        /// <returns></returns>
        Task<bool> SetHDPulseValue(int value, string CmdHDZF);

        Task<bool> SetHDToRecord(int value);

        Task<bool> RecordHDPosition(int value);

        /// <summary>
        /// 设置无刷电机目标速度
        /// </summary>
        /// <param name="value">速度值</param>
        /// <param name="address">从机地址</param>
        /// <returns></returns>
        Task<bool> SetMotorSpeed(int value, string address);

        /// <summary>
        /// 设置无刷电机达到位置
        /// </summary>
        /// <param name="value"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        Task<bool> SetMotorPosition(int value, string address);

        /// <summary>
        ///  设置无刷电机位置目标值  
        /// </summary>
        /// <param name="address">byte0 从机地址1-5 1-滤光片电机 2-狭缝1 3-狭缝2  4-斩波伦 5-反光镜  </param>
        /// <param name="value">0-360°转换成16进制放大一百倍发</param>
        /// <param name="no">1-8 位置编号</param>
        /// <returns></returns>
        Task<bool> SetMotorValue(int address, int value, int no);

        /// <summary>
        /// 保存无刷电机达到位置
        /// </summary>
        /// <param name="value"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        Task<bool> SaveMotorPosition(int value, string address);

        /// <summary>
        /// 获取CCD温度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<int> GetTemperature(int value);

        /// <summary>
        /// 动力学采集
        /// </summary>
        /// <returns></returns>
        Task<bool> DynamicCollect();

        /// <summary>
        /// 设置动力学光栅位置
        /// </summary>
        /// <param name="wels">波长或者编码器位置</param>
        /// <returns></returns>
        Task<bool> SetDynGSPosition(int wels);

        /// <summary>
        /// 设置间隔时间
        /// </summary>
        /// <param name="IntervalTime"></param>
        /// <returns></returns>
        Task<bool> SetDynIntervalTime(int IntervalTime);

        /// <summary>
        /// 设置动力学总时长
        /// </summary>
        /// <param name="MonitorTime"></param>
        /// <returns></returns>
        Task<bool> SetDynAllTime(int MonitorTime);

        /// <summary>
        /// 设置动力学延长时间
        /// </summary>
        /// <param name="DelayTime"></param>
        /// <returns></returns>
        Task<bool> SetDynDelayTime(int DelayTime);

        /// <summary>
        /// 获取光栅位置
        /// </summary>
        /// <returns></returns>
        Task<int[]> GetGSPosition();

        /// <summary>
        /// 获取光栅原点位置
        /// </summary>
        /// <returns></returns>
        Task<string> GetGSOriginPoint();

        /// <summary>
        /// 设置PMT电压
        /// </summary>
        /// <param name="value"></param>
        /// <param name="gy"></param>
        /// <returns></returns>
        Task<bool> SetVoltage(int value, string gy, int position);

        /// <summary>
        /// 设置设备配置文件
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        Task<bool> SetDevCfgInfo(string cfg);

        /// <summary>
        /// 设置光源切换位置
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        Task<bool> SetLight(int val);

        /// <summary>
        /// 获取设备基础信息
        /// </summary>
        /// <returns></returns>
        Task<bool> GetDevInfoAsync();

        /// <summary>
        /// 获取设备配置文件
        /// </summary>
        /// <returns></returns>
        Task<string> GetDevCfgInfo();

        /// <summary>
        /// 获取所有电机状态
        /// </summary>
        /// <returns></returns>
        Task<bool> GetAllMotorStatus();

        /// <summary>
        /// 设置附件
        /// </summary>
        /// <param name="multiPool"></param>
        /// <returns></returns>
        Task<bool> SetAnnex(int multiPool);


        /// <summary>
        /// 设置附件
        /// </summary>
        /// <param name="multiPool"></param>
        /// <returns></returns>
        Task<int> GetAnnex();

        Task<Tuple<bool, double>> GetXDataToYData(int xData, string data = "");


        Task<Tuple<bool, int>> GetYDataToXData(double yData, string coeffData = "");

        /// <summary>
        /// 获取角度值
        /// </summary>
        /// <param name="address">从机地址</param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<int> GetAngleValue(string address, int position);

        /// <summary>
        /// 反光镜定标
        /// </summary>
        /// <returns></returns>
        Task<bool> MirrorCalibration();

        /// <summary>
        /// 暗背景写入
        /// </summary>
        /// <returns></returns>
        Task<bool> DarkBackWriting();

        /// <summary>
        /// 清除系数
        /// </summary>
        /// <returns></returns>
        Task<bool> ClearCalibration();

        /// <summary>
        /// 清楚强度补偿
        /// </summary>
        /// <returns></returns>
        Task<bool> ClaarCompensation();

        /// <summary>
        /// 写入强度补偿
        /// </summary>
        /// <param name="compensation"></param>
        /// <returns></returns>
        Task<bool> WriteCompensation(string compensation, string colInterval);

        /// <summary>
        /// 读取强度补偿
        /// </summary>
        /// <returns></returns>
        Task<string> ReadCompensation(string colInterval);


        /// <summary>
        /// 写入透射比校零系数
        /// </summary>
        /// <param name="compensation"></param>
        /// <returns></returns>
        Task<bool> WriteStraylightZeroCalib(string calib);

        /// <summary>
        /// 读取透射比校零系数
        /// </summary>
        /// <param name="compensation"></param>
        /// <returns></returns>
        Task<string> ReadStraylightZeroCalib();

        #region ATP7810
        /// <summary>
        /// 设置增益
        /// </summary>
        /// <param name="gear"></param>
        /// <returns></returns>
        Task<bool> SetGear(string gear);

        /// <summary>
        /// 设置频率
        /// </summary>
        /// <returns></returns>
        Task<bool> SetRate(string rate);
        #endregion

        /// <summary>
        /// 电机状态
        /// </summary>
        /// <returns></returns>
        Tuple<bool, string> IsDeviceIdle();

        /// <summary>
        /// 获取光栅及波长关系
        /// </summary>
        /// <returns></returns>
        Task GetGratingBindWelAsync();

        /// <summary>
        /// 获取对应原点的系数
        /// </summary>
        /// <returns></returns>
        Task<List<double>> GetCofInfoAsync(int point);

        /// <summary>
        /// 采集
        /// </summary>
        /// <returns></returns>
        Task<int> Acquire(byte[] bytes);
    }

    public partial class DeviceService : DeviceServiceCom, IDeviceService
    {
        public string id {  get; set; }=Guid.NewGuid().ToString();
        protected ILocalize _localize;
        private DBOperate dbOperate;

        public DeviceService()
        {
            _localize = InjectionWpf.GetService<ILocalize>();
            dbOperate = DBOperate.Instance();

        }

        /// <summary>
        /// 设备的配置信息
        /// </summary>
        public DevInfos DevInfos { get; set; }

        public bool IsSiliconWaferCal { get; set; } = false;

        /// <summary>
        /// 设置的参数(光谱)
        /// </summary>
        public string SpecPramInfo { get; set; }

        /// <summary>
        /// 采集状态
        /// </summary>
        public bool CollStopStatic { get; set; } = true;

        /// <summary>
        /// 电机是否全都停止
        /// </summary>
        public bool IsStopAllMotor { get; set; } = false;

        /// <summary>
        /// 类型
        /// </summary>
        public DataOperateType OperateType { get; set; }

        public Device Device { get; private set; } = new Device();

        /// <summary>
        /// 采集类型
        /// </summary>
        public DisplayDataType DisplayDataType { get; set; } = DisplayDataType.Spectrum;

        /// <summary>
        /// 连续采集时显示的线条
        /// </summary>
        public int ShowLineNum { get; set; } = 0;
        public dynamicsEt BackDynamicsPramInfo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        /// <summary>
        /// device state changed event
        /// </summary>
        public event EventHandler DeviceStateChanged;

        public bool ChangeDeviceState(DeviceState state, DeviceState currentState = DeviceState.Idle)
        {
            if (currentState != Device.DeviceStates) return false;

            Device.DeviceStates = state;

            // trigger device state change event, change control enable
            DeviceStateChanged?.Invoke(this, null);

            return true;
        }



        public Tuple<bool, string> SaveSpectrum(Spectrum spectrum, SpectrumDataRaw spectrumRaw, SpectrumDataDark spectrumDark, SpectrumDataWhiteBoard spectrumDataWhiteBoard = null)
        {


            return Tuple.Create(true, string.Empty);
        }


        /// <summary>
        /// 保存测光临时数据
        /// </summary>
        /// <param name="spectrum"></param>
        /// <param name="standardDetailDatas"></param>
        /// <param name="sampleSpectrumDatas"></param>
        /// <param name="spectrumData"></param>
        /// <returns></returns>
        public Tuple<bool, string, AbsStandardSpectrumData, SpectrumDto> SaveAbsSpectrum(Spectrum spectrum,
            List<AbsStandardDetailData> standardDetailDatas, List<AbsSampleSpectrumData> sampleSpectrumDatas, SpectrumDataDark spectrumDark, bool isSave = false)
        {
            AbsStandardSpectrumData absTempData = null;
            SpectrumDto spectrumDto = null;
            bool IsStandard = (sampleSpectrumDatas == null || sampleSpectrumDatas.Count() == 0) ? true : false;
            // valid data

            return Tuple.Create(true, string.Empty, absTempData, spectrumDto);
        }

        /// <summary>
        /// 保存测光临时数据
        /// </summary>
        /// <param name="spectrum"></param>
        /// <param name="standardDetailDatas"></param>
        /// <param name="sampleSpectrumDatas"></param>
        /// <param name="spectrumData"></param>
        /// <returns></returns>
        public Tuple<bool, string, AbsStandardSpectrumData, SpectrumDto> UpdateAbsStandardSpectrum(Spectrum spectrum,
            List<AbsStandardDetailData> standardDetailDatas, SpectrumDataDark spectrumDark, string standardId)
        {
            AbsStandardSpectrumData absTempData = null;
            SpectrumDto spectrumDto = null;
            // valid data

            return Tuple.Create(true, string.Empty, absTempData, spectrumDto);
        }

        public SpectrumDto CreateSpectrumDto(Spectrum spectrum, SpectrumDataRaw spectrumRaw, SpectrumDataDark spectrumDark, SpectrumDataWhiteBoard spectrumDataWhiteBoard = null)
        {
            var model = new SpectrumDto(spectrum, spectrumRaw, spectrumDark, spectrumDataWhiteBoard);

            var data = new SpectrumDataDto(spectrumRaw, spectrumDark, spectrumDataWhiteBoard);

            model.Data = new List<SpectrumDataDto>() { data };

            return model;
        }

        public Task<string> DevSelfTest()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetDevSelfTest()
        {
            throw new NotImplementedException();
        }

        public Task<List<double>> GetCoeff()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetCoeffNew()
        {
            throw new NotImplementedException();
        }

        public Task<bool> CloseCollection(bool isManualStop = false)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<bool, string>> SetFixedPointWelAddress(int num)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<bool, string>> FixedPointCollection()
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<bool, string>> SetGear(int Gear, int collInterval)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetWelRangeAddress(int snum, int endnum)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetDevSN()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetCuvetteNo(List<string> ls)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetCuvetteNo(int cuvetteNo)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<bool, string>> SetAbsWelAddress(int num)
        {
            throw new NotImplementedException();
        }

        public Task<List<List<object>>> MeteringCollection()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetWaveLengthCoefficient(string hex)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ResetUseNum()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetXenonLampCout()
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<bool, string>> FullSpectrum()
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<bool, string>> RangeSpectrum()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetFilterPulseValue(int value, string CmdLGPZF)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RecordFilterPosition(int value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetFilterPosition(int value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> FilterChange(int groupNo, int value, int filterNo)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GearShift(int gropuNo, int gearNo, int value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetCuvetteMC(int value, string CmdCuvetteZF)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetCuvetteRecordPosition(int value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RecordCuvettePosition(int value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetGSValue(bool Gs1, int num, string CmdGSZF)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetToRecordGsPosition(bool Gs1, int num)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RecordGsPosition(bool Gs1, int num)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetGYPulseValue(int value, string CmdGYZF)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetGYToRecord(int value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RecordGYPosition(int value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SelectLightSource(int value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetXFPulseValue(int value, string CmdXFZF)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RecordXFPosition(int no)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<bool, string>> SetXFToRecord(int no, int no2)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetHDPulseValue(int value, string CmdHDZF)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetHDToRecord(int value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RecordHDPosition(int value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetMotorSpeed(int value, string address)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetMotorPosition(int value, string address)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetMotorValue(int address, int value, int no)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveMotorPosition(int value, string address)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTemperature(int value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DynamicCollect()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetDynGSPosition(int wels)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetDynIntervalTime(int IntervalTime)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetDynAllTime(int MonitorTime)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetDynDelayTime(int DelayTime)
        {
            throw new NotImplementedException();
        }

        public Task<int[]> GetGSPosition()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetGSOriginPoint()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetVoltage(int value, string gy, int position)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetDevCfgInfo(string cfg)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetLight(int val)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> GetDevInfoAsync()
        {
            try
            {
                var lists = GetOpenDevInfo();
                if (lists.Count <= 0)
                    DevInfos = _localize.GetCfg<DevInfos>(_userCfg.DevInfos);

                var ret = await GetDevCfgInfo();
                if (!string.IsNullOrEmpty(ret))
                {
                    var savekey = _localize.GetCfg<string>(_userCfg.TCompress);
                    var mdkey = ret.Substring(0, savekey.Length);
                    if (mdkey == savekey && !string.IsNullOrEmpty(savekey))
                    {//加密数据
                        var data = ret.Substring(savekey.Length);

                        DevInfos = JsonConvert.DeserializeObject<DevInfos>(ZipHandler.GetDatasetByString(data));
                    }
                    else
                    {//未加密数据
                        DevInfos = JsonConvert.DeserializeObject<DevInfos>(ret.ToString());
                    
                    }
                }
                if (DevInfos == null)
                {
                    throw new Exception(_localize.GetString(LocalizeConstant.NOCONNECTDEVICE));
                }
                DevInfos = DevInfos ?? new DevInfos();

                //更新光栅绑定信息
                await GetGratingBindWelAsync();
                return true;
            }
            catch (Exception ex)
            {
                DevInfos = DevInfos ?? _localize.GetCfg<DevInfos>(_userCfg.DevInfos) as DevInfos;

                throw ex;
            }



        }

        public async Task<string> GetDevCfgInfo()
        {
            try
            {
                var cmd = ProtocolCmds.GDEV_I;
                var com = dbOperate.Query<c_Command>(t => t.devName == ProtocolCmds.DevName && t.KeyName.ToLower() == cmd.ToLower()).GetSource<List<c_Command>>()?.FirstOrDefault();
               
                if (com == null)
                {
                    throw new Exception(_localize.GetString(LocalizeConstant.CMD_NOTFIND));
                }

                var ret = (string)await ComSerialPortAsk(Convert.ToByte(com.cmdNum, 16), ProtocolCmds.DevName, null);

                return ret?.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<bool> GetAllMotorStatus()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetAnnex(int multiPool)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetAnnex()
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<bool, double>> GetXDataToYData(int xData, string data = "")
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<bool, int>> GetYDataToXData(double yData, string coeffData = "")
        {
            throw new NotImplementedException();
        }

        public Task<int> GetAngleValue(string address, int position)
        {
            throw new NotImplementedException();
        }

        public Task<bool> MirrorCalibration()
        {
            throw new NotImplementedException();
        }

        public Task<bool> DarkBackWriting()
        {
            throw new NotImplementedException();
        }

        public Task<bool> ClearCalibration()
        {
            throw new NotImplementedException();
        }

        public Task<bool> ClaarCompensation()
        {
            throw new NotImplementedException();
        }

        public Task<bool> WriteCompensation(string compensation, string colInterval)
        {
            throw new NotImplementedException();
        }

        public Task<string> ReadCompensation(string colInterval)
        {
            throw new NotImplementedException();
        }

        public Task<bool> WriteStraylightZeroCalib(string calib)
        {
            throw new NotImplementedException();
        }

        public Task<string> ReadStraylightZeroCalib()
        {
            throw new NotImplementedException();
        }

        public Tuple<bool, string> IsDeviceIdle()
        {
            if (Device.DeviceStates == (int)DeviceState.Idle)
                return Tuple.Create(true, string.Empty);

            return Tuple.Create(false, _localize.GetString(LocalizeConstant.DEVICE_STATUS, (int)Device.DeviceStates));
        }

        public async Task<bool> SetGear(string gear)
        {
            try
            {
                var cmd = ProtocolCmds.Gear_Set;               
                var com = dbOperate.Query<c_Command>(t => t.devName == ProtocolCmds.DevName && t.KeyName.ToLower() == cmd.ToLower()).GetSource<List<c_Command>>()?.FirstOrDefault();
                var ret = (bool)await ComSerialPortAsk(Convert.ToByte(com.cmdNum, 16), ProtocolCmds.DevName, (object)gear);
                return ret;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SetRate(string rate)
        {
            try
            {
                var cmd = ProtocolCmds.Accuracy_Set;
                var com = dbOperate.Query<c_Command>(t => t.devName == ProtocolCmds.DevName && t.KeyName.ToLower() == cmd.ToLower()).GetSource<List<c_Command>>()?.FirstOrDefault();
                var ret = (bool)await ComSerialPortAsk(Convert.ToByte(com.cmdNum, 16), ProtocolCmds.DevName, (object)rate);
                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task GetGratingBindWelAsync()
        {
           try
            {
                var ret = DevInfos.GratingBindWel ?? new List<GratingBindWel>();
                foreach (var obj in ret)
                {
                    try
                    {
                        obj.cof = await GetCofInfoAsync(obj.point);
                    }
                    catch
                    {

                    }             
                    obj.startdis = ret.Sum(t => t.sumdis);
                   
                    //开始值 编码器
                    var MinNum = (int)await ComSerialPortAsk(ProtocolCmds.GS_GET_P, ProtocolCmds.DevName, (object)("0" + obj.point));
                    //终点值 编码器

                    var MaxNum = (int)await ComSerialPortAsk(ProtocolCmds.GS_GET_E,  ProtocolCmds.DevName, (object)("0" + obj.point));

                    if (MinNum == 0 && MaxNum == 0) continue;

                    if (MinNum > MaxNum)
                    {
                        obj.sumdis = _localize.GetCfg<int>(_userCfg.MaxEncoderNum) - MinNum + MaxNum;
                    }
                    else
                    {
                        obj.sumdis = Math.Abs(MinNum - MaxNum);
                    }
                    try
                    {
                        if (!obj.modelFile)
                        {
                            obj.sumdis = obj.cof.Count > 0 ? (int)Math.Ceiling(CommonFuncHandler.SetYGetXVal(obj.cof.ToArray(), obj.shownume)) : obj.shownume;
                            obj.startdis = obj.cof.Count > 0 ? (int)Math.Ceiling(CommonFuncHandler.SetYGetXVal(obj.cof.ToArray(), obj.shownums)) : obj.shownums;
                            obj.startwel = obj.startdis;
                            obj.startwel = obj.cof.Count > 0 ? (int)Math.Floor(CommonFuncHandler.SetXGetYVal(obj.cof.ToArray(), obj.startdis)) : obj.startwel;
                        }
                               
                    }
                    catch
                    {

                    }



                }

                DevInfos.GratingBindWel = ret.OrderBy(t => t.point).ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task GetWelAndCCDInfo()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        public async Task<List<double>> GetCofInfoAsync(int point)
        {
            try
            {
                //获取定标信息(对应的原点系数)
                var ret = (await ComSerialPortAsk(ProtocolCmds.GET_COF_PX, ProtocolCmds.DevName, (object)point))?.ToString();
                if (string.IsNullOrEmpty(ret))
                {
                    return new List<double>();
                }
                //系数
                List<double> list = new List<double>(ret.Split(',').ToList().Select(x => Convert.ToDouble(x)).ToList());
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Acquire(byte[] bytes)
        {
            try
            {
                var cmd = ProtocolCmds.RANGE_COL;
                var com = dbOperate.Query<c_Command>(t => t.devName == ProtocolCmds.DevName && t.KeyName.ToLower() == cmd.ToLower()).GetSource<List<c_Command>>()?.FirstOrDefault();
                var ret = (int)await ComSerialPortAsk(Convert.ToByte(com.cmdNum, 16), ProtocolCmds.DevName, bytes);
                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
