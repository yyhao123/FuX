using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Communication.constant
{
    public partial class _userCfg
    {

        /// <summary>
        /// 采集类型
        /// </summary>
        public const string CollectTypes = "CollectTypes";


        /// <summary>
        /// Y轴单位，数据类型
        /// </summary>
        public const string SpectrumYAxisUnit = "SpectrumYAxisUnit";


        /// <summary>
        /// 增益档位参数
        /// </summary>
        public const string Gain = "Gain";

        /// <summary>
        /// 精准度参数
        /// </summary>
        public const string Accuracy = "Accuracy";

        /// <summary>
        /// 滤波权重管理
        /// </summary>
        public const string FilterWeights = "FilterWeights";

        /// <summary>
        /// 角度换算值
        /// </summary>
        public const string AngleVal = "AngleVal";

        /// <summary>
        /// 设备温度系数
        /// </summary>
        public const string DevTempCof = "DevTempCof";

        /// <summary>
        /// CCD温度系数
        /// </summary>
        public const string CCDTempCof = "CCDTempCof";


        ////////////上面的需要新增

        /// <summary>
        /// xy放大倍数【double类型】
        /// </summary>
        public const string XYEnlarge = "XYEnlarge";

        /// <summary>
        /// 积分时间【int型】
        /// </summary>
        public const string interval = "interval";


        /// <summary>
        /// 显示光谱类型【枚举类型】
        /// </summary>
        public const string SpectrumDataType = "SpectrumDataType";

        /// <summary>
        /// 激光系数（波数和波长转换用）【字符串】
        /// </summary>
        public const string LaserCoefficient = "LaserCoefficient";

        /// <summary>
        /// 采集模式【枚举类型】
        /// </summary>
        public const string AcquireMethod = "AcquireMethod";

        /// <summary>
        /// 是否显示历史记录【int型】
        /// </summary>
        public const string BottomHistoryAreaVisible = "BottomHistoryAreaVisible";

        /// <summary>
        /// 是否显示类型【int型】
        /// </summary>
        public const string LeftLenAreaVisible = "LeftLenAreaVisible";

        /// <summary>
        /// 右侧光谱控制【int型】
        /// </summary>
        public const string RightControlAreaVisible = "RightControlAreaVisible";

        /// <summary>
        /// 右侧谱图列表【int型】
        /// </summary>
        public const string RightSpectrumListAreaVisible = "RightSpectrumListAreaVisible";


        public const string RightControlParmVisible = "RightControlParmVisible";

        /// <summary>
        /// 谱图轴单位 0：拉曼位移 1：像素【int型】 2：波长 3：时间
        /// </summary>
        public const string SpectrumXAxisUnit = "SpectrumXAxisUnit";

        /// <summary>
        /// 显示曲线数目【int型】
        /// </summary>
        public const string CurveCount = "CurveCount";

        /// <summary>
        /// 最大线条数量【int型】
        /// </summary>
        public const string MaxCurveCount = "MaxCurveCount";

        /// <summary>
        /// 积分时间【int型】
        /// </summary>
        public const string IntegrationTime = "IntegrationTime";

        /// <summary>
        /// 积分时间单位【字符串】
        /// </summary>
        public const string IntegrationTimeUnit = "IntegrationTimeUnit";

        /// <summary>
        /// 平均次数【int型】
        /// </summary>
        public const string Average = "Average";

        /// <summary>
        /// 制冷温度【int型】
        /// </summary>
        public const string CoolTemperature = "CoolTemperature";

        /// <summary>
        /// 积分时间【int型】
        /// </summary>
        public const string Interval = "Interval";

        /// <summary>
        /// 激光功率【int型】
        /// </summary>
        public const string LaserPower = "LaserPower";

        /// <summary>
        /// 最大功率【int型】
        /// </summary>
        public const string MaxLaserPower = "MaxLaserPower";

        /// <summary>
        /// 强度【int型】
        /// </summary>
        public const string PeakIntensityThreshold = "PeakIntensityThreshold";

        /// <summary>
        /// 信噪比【int型】
        /// </summary>
        public const string PeakSNRThreshold = "PeakSNRThreshold";

        /// <summary>
        /// 移动步进【int型】
        /// </summary>
        public const string MotorStep = "MotorStep";

        /// <summary>
        /// 伪色图开始位置【double类型】
        /// </summary>
        public const string PseudoColorRamanShiftFrom = "PseudoColorRamanShiftFrom";

        /// <summary>
        /// 伪色图开始结束【double类型】
        /// </summary>
        public const string PseudoColorRamanShiftTo = "PseudoColorRamanShiftTo";

        /// <summary>
        /// 左右两张图片重叠百分比【int型】
        /// </summary>
        public const string StitchOverlapPercentLeftAndRight = "StitchOverlapPercentLeftAndRight";

        /// <summary>
        /// 前后两张图片重叠百分比【int型】
        /// </summary>
        public const string StitchOverlapPercentBackwardAndForward = "StitchOverlapPercentBackwardAndForward";

        /// <summary>
        /// 匹配区域left-right mm【double类型】
        /// </summary>
        public const string MappingLeftToRight = "MappingLeftToRight";

        /// <summary>
        /// 匹配区域TOP-BUTTOM mm【double类型】
        /// </summary>
        public const string MappingTopToButton = "MappingTopToButton";

        /// <summary>
        /// 匹配区域left-right mm【double类型】
        /// </summary>
        public const string MappingTopToButtonD = "MappingTopToButtonD";

        /// <summary>
        /// 匹配区域backward-forward mm【double类型】
        /// </summary>
        public const string MappingBackwardToForward = "MappingBackwardToForward";

        /// <summary>
        /// Mapping前后【字符串】
        /// </summary>
        public const string MappingAcquireStepBF = "MappingAcquireStepBF";

        /// <summary>
        /// Mapping左右【字符串】
        /// </summary>
        public const string MappingAcquireStepLR = "MappingAcquireStepLR";

        /// <summary>
        /// 伪彩色计算方式【枚举类型】
        /// </summary>
        public const string PseudoColorEvaluateType = "PseudoColorEvaluateType";

        /// <summary>
        /// 相机自动曝光设置【bool类型】
        /// </summary>
        public const string CameraAutoExposure = "CameraAutoExposure";

        /// <summary>
        /// 相机曝光时长 单位毫秒【int类型】
        /// </summary>
        public const string CameraExposureTime = "CameraExposureTime";

        /// <summary>
        /// 相机曝光增益 单位毫秒【int类型】
        /// </summary>
        public const string CameraExposureGain = "CameraExposureGain";

        /// <summary>
        /// 打开路径的保存地址【字符串】
        /// </summary>
        public const string OpenFolderAfterSave = "OpenFolderAfterSave";

        /// <summary>
        /// 光谱导出路径地址【字符串】
        /// </summary>
        public const string SpectrumExportFolder = "SpectrumExportFolder";

        /// <summary>
        /// 导出文件类型【枚举类型】
        /// </summary>
        public const string SpectrumExportFileType = "SpectrumExportFileType";

        /// <summary>
        /// 算法处理类型【枚举类型】
        /// </summary>
        public const string SmoothType = "SmoothType";

        /// <summary>
        /// 算法参数存储【int型】
        /// </summary>
        public const string BoxcarWinSize = "BoxcarWinSize";

        /// <summary>
        /// 算法参数存储【bool类型】
        /// </summary>
        public const string SmoothSavitzkyGolayAuto = "SmoothSavitzkyGolayAuto";

        /// <summary>
        /// 算法参数存储【int型】
        /// </summary>
        public const string SmoothSavitzkyGolayIterator = "SmoothSavitzkyGolayIterator";

        /// <summary>
        /// 激光常亮功率mW，只在初始化控件值时有用到【int型】
        /// </summary>
        public const string LaserLightingPower = "LaserLightingPower";

        /// <summary>
        /// 前缀【字符串】
        /// </summary>
        public const string SaveFilePrefix = "SaveFilePrefix";

        /// <summary>
        /// 第一个间隔符【字符串】
        /// </summary>
        public const string SaveFileDelimiterFirst = "SaveFileDelimiterFirst";

        /// <summary>
        /// 第二个间隔符【字符串】
        /// </summary>
        public const string SaveFileDelimiterSecond = "SaveFileDelimiterSecond";

        /// <summary>
        /// 是否显示第一个间隔符【bool类型】
        /// </summary>
        public const string SaveFileShowDelimiterFirst = "SaveFileShowDelimiterFirst";

        /// <summary>
        /// 是否显示第二个间隔符【bool类型】
        /// </summary>
        public const string SaveFileShowDelimiterSecond = "SaveFileShowDelimiterSecond";

        /// <summary>
        /// 是否显示日期【bool类型】
        /// </summary>
        public const string SaveFileShowDate = "SaveFileShowDate";

        /// <summary>
        /// 日期【字符串】
        /// </summary>
        public const string SaveFileDate = "SaveFileDate";

        /// <summary>
        /// 流水号【int型】
        /// </summary>
        public const string SaveFileSequence = "SaveFileSequence";

        /// <summary>
        /// 流水号宽度【int型】
        /// </summary>
        public const string SaveFileSequenceWidth = "SaveFileSequenceWidth";

        /// <summary>
        /// 倍镜【字符串】
        /// </summary>
        public const string MagnificationList = "MagnificationList";

        /// <summary>
        /// 默认选中的波长【字符串】
        /// </summary>
        public const string CurSelectWelNum = "CurSelectWelNum";

        /// <summary>
        /// 默认选中的光栅【字符串】
        /// </summary>
        public const string CurSelectGsNum = "CurSelectGsNum";

        /// <summary>
        /// 默认中心波数【字符串】
        /// </summary>
        public const string CenterWave = "CenterWave";

        /// <summary>
        /// 激光对应关系【字符串】
        /// </summary>
        public const string laserInfo = "laserInfo";

        /// <summary>
        /// 相机关系【字符串】
        /// </summary>
        public const string CCDInfo = "CCDInfo";

        /// <summary>
        /// LED灯亮度【int型】
        /// </summary>
        public const string LedNum = "LedNum";

        /// <summary>
        /// 选择的激光波长【int型】
        /// </summary>
        public const string LaserNum = "LaserNum";

        /// <summary>
        /// 激光等待时间 ms【int型】
        /// </summary>
        public const string LaserDetime = "LaserDetime";

        /// <summary>
        /// SP是否启动【bool类型】
        /// </summary>
        public const string SpIsStartUp = "SpIsStartUp";

        /// <summary>
        /// 是否运行反射镜1【bool类型】
        /// </summary>
        public const string ReflectionMirror1 = "ReflectionMirror1";

        /// <summary>
        /// 是否运行反射镜2【bool类型】
        /// </summary>
        public const string ReflectionMirror2 = "ReflectionMirror2";

        /// <summary>
        /// 默认波特率 115200   921600【int型】
        /// </summary>
        public const string BaudRate = "BaudRate";

        /// <summary>
        /// 登录账号【字符串】
        /// </summary>
        public const string LoginUser = "LoginUser";

        /// <summary>
        /// 登录密码【字符串】
        /// </summary>
        public const string LoginPwd = "LoginPwd";

        /// <summary>
        /// 是否记住密码【bool类型】
        /// </summary>
        public const string IsSavePwd = "IsSavePwd";

        /// <summary>
        /// 权限代码【字符串】
        /// </summary>
        public const string Purviewid = "Purviewid";

        /// <summary>
        /// 项目名称【字符串】
        /// </summary>
        public const string ProjectName = "ProjectName";

        /// <summary>
        /// 版本编号【字符串】
        /// </summary>
        public const string ProjectVersion = "ProjectVersion";

        /// <summary>
        /// 默认链接的设备，填写情况将自动连接(如多个用,隔开 ATP7810-50,ATM2000) 【字符串】
        /// </summary>
        public const string DefaultDev = "DefaultDev";

        /// <summary>
        /// 主信息设备（多设备时填写，如不填默认读到的第一个）【字符串】
        /// </summary>
        public const string MasterDev = "MasterDev";

        /// <summary>
        /// 编码器最大刻度值【int型】
        /// </summary>
        public const string MaxEncoderNum = "MaxEncoderNum";

        /// <summary>
        /// 补丁版本号【字符串】
        /// </summary>
        public const string ProjectVersionNo = "ProjectVersionNo";

        /// <summary>
        /// 项目版本代码，用于系统更新，在网站中获取【字符串】
        /// </summary>
        public const string ProjectVersionId = "ProjectVersionId";

        /// <summary>
        /// 系统更新服务器地址【字符串】
        /// </summary>
        public const string ProjectVersionIp = "ProjectVersionIp";

        /// <summary>
        /// 语言对象【数据对象】
        /// </summary>
        public const string LanguageInfo = "LanguageInfo";

        /// <summary>
        /// 设备类型【枚举类型】
        /// </summary>
        public const string SubDeviceType = "SubDeviceType";

        /// <summary>
        /// Z放大倍数【double类型】
        /// </summary>
        public const string ZEnlarge = "ZEnlarge";

        /// <summary>
        /// 可见光相机像素【int型】
        /// </summary>
        public const string Low_320X240Index = "Low_320X240Index";

        /// <summary>
        /// 显示曲线数目【int型】
        /// </summary>
        public const string MCurveCount = "MCurveCount";

        /// <summary>
        /// 选择的镜头放大倍数【int型】
        /// </summary>
        public const string MicroLenMagnification = "MicroLenMagnification";

        /// <summary>
        /// 是否线条调试模式【int型】
        /// </summary>
        public const string PointLineTest = "PointLineTest";

        /// <summary>
        /// 暗背景强度数据存放路径
        /// </summary>
        public const string SpectrumDataDarkPath = "SpectrumDataDarkPath";

        /// <summary>
        /// 原始数据强度数据存放路径
        /// </summary>
        public const string SpectrumDataRawPath = "SpectrumDataRawPath";

        //public  string SpectrumDataDarkPath { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"data\\SpectrumDataDark\\");

        //public string SpectrumDataRawPath { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"data\\SpectrumDataRaw\\");

        public const string CCDInfosShow = "CCDInfosShow";

        /// <summary>
        /// 判断是否单光栅 【bool型】
        /// </summary>
        public const string IsCheckGs = "IsCheckGs";

        /// <summary>
        /// 激发波长 【int型】
        /// </summary>
        public const string ExWel = "ExWel";

        /// <summary>
        /// 采集参数 【string型】
        /// </summary>
        public const string PramInfo = "PramInfo";

        /// <summary>
        /// 是否验证参数 【bool型】
        /// </summary>
        public const string PramCheck = "PramCheck";

        /// <summary>
        /// 峰值数 【int型】
        /// </summary>
        public const string CalPeakCount = "CalPeakCount";

        /// <summary>
        /// 校正像素数据数组 【List<int>型】
        /// </summary>
        public const string CalPixs = "CalPixs";

        /// <summary>
        /// 校正波数数据数组 【List<double>型】
        /// </summary>
        public const string CalWaveNums = "CalWaveNums";

        /// <summary>
        /// 默认CCD 【string】
        /// </summary>
        public const string DefaultCCD = "DefaultCCD";

        /// <summary>
        /// 存储配置信息
        /// </summary>
        public const string DevInfos = "DevInfos";

        /// <summary>
        /// 产品信息
        /// </summary>
        public const string ProductInfo = "ProductInfo";

        /// <summary>
        /// 验证密码【字符串】
        /// </summary>
        public const string OPERATION_PROTECT_PWD = "OPERATION_PROTECT_PWD";

        /// <summary>
        /// 是否为测试模式【bool】
        /// </summary>
        public const string IsTest = "IsTest";

        /// <summary>
        /// 电机操作模块
        /// </summary>
        public const string MotorInfos = "MotorInfos";

        /// <summary>
        /// 光栅对应关系【字符串】
        /// </summary>
        public const string gsInfo = "gsInfo";

        /// <summary>
        /// ICCD参数【字符串】
        /// </summary>
        public const string ICCDParm = "ICCDParm";

        /// <summary>
        /// 采集频率
        /// </summary>
        public const string GatherRate = "GatherRate";

        /// <summary>
        /// 是否启用增强器【bool】
        /// </summary>
        public const string EnableICCD = "EnableICCD";

        /// <summary>
        /// 默认为登录界面【bool类型】
        /// </summary>
        public const string IsLogin = "IsLogin";

        /// <summary>
        /// 登录用户记录【字符串】
        /// </summary>
        public const string LoginUserList = "LoginUserList";

        /// <summary>
        /// 强度系数【字符串】
        /// </summary>
        public const string IntensityCof = "IntensityCof";

        /// <summary>
        /// 光谱参数Json（字符串）
        /// </summary>
        public const string SpecParaJson = "SpecParaJson";

        /// <summary>
        /// 压缩加密key
        /// </summary>
        public const string TCompress = "TCompress";

    }
}
