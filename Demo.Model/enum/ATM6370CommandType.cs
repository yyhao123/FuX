using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.@enum
{
    /// <summary>
    /// ATM6370命令类型
    /// </summary>
    public enum ATM6370CommandType : byte
    {
        /// <summary>
        /// USB同步信号
        /// </summary>
        [Description("USB同步信号")]
        UsbSyncSignal = 0x0B,

        /// <summary>
        /// 紧急停止电机
        /// </summary>
        [Description("紧急停止电机")]
        EmergencyStopMotor = 0x0C,

        /// <summary>
        /// 复位电机
        /// </summary>
        [Description("复位电机")]
        ResetMotor = 0x0D,

        /// <summary>
        /// 获取所有电机状态
        /// </summary>
        [Description("获取所有电机状态")]
        GetAllMotorStatus = 0x0E,

        /// <summary>
        /// 重置电机偏移位置
        /// </summary>
        [Description("重置电机偏移位置")]
        ResetMotorOffset = 0x0F,

        /// <summary>
        /// 旋钮功能
        /// </summary>
        [Description("旋钮功能")]
        KnobFunction = 0x10,

        /// <summary>
        /// 获取当前编码器值
        /// </summary>
        [Description("获取当前编码器值")]
        GetCurrentEncoderValue = 0x11,

        /// <summary>
        /// 设置光栅相对位置脉冲
        /// </summary>
        [Description("设置光栅相对位置脉冲")]
        SetRasterRelativePulse = 0x20,

        /// <summary>
        /// 设置狭缝相对位置脉冲
        /// </summary>
        [Description("设置狭缝相对位置脉冲")]
        SetSlitRelativePulse = 0x21,

        /// <summary>
        /// 设置狭缝丝杆电机相对位置脉冲
        /// </summary>
        [Description("设置狭缝丝杆电机相对位置脉冲")]
        SetSlitScrewMotorRelativePulse = 0x22,

        /// <summary>
        /// 设置狭缝丝杆电机偏移位置
        /// </summary>
        [Description("设置狭缝丝杆电机偏移位置")]
        SetSlitScrewMotorOffsetPosition = 0x30,

        /// <summary>
        /// 设置狭缝丝杆电机定标位置
        /// </summary>
        [Description("设置狭缝丝杆电机定标位置")]
        SetSlitScrewMotorCalibrationPosition = 0x40,

        /// <summary>
        /// 设置狭缝丝杆电机位置
        /// </summary>
        [Description("设置狭缝丝杆电机位置")]
        SetSlitScrewMotorPosition = 0x50,

        /// <summary>
        /// 设置采样参数
        /// </summary>
        [Description("设置采样参数")]
        SetSamplingParam = 0x60,

        /// <summary>
        /// 设置采集范围
        /// </summary>
        [Description("设置采集范围")]
        SetCollectionRange = 0x61,

        /// <summary>
        /// 扫描速度
        /// </summary>
        [Description("扫描速度")]
        ScanSpeed = 0x62,

        /// <summary>
        /// 放大等级
        /// </summary>
        [Description("放大等级")]
        ZoomLevel = 0x63,

        /// <summary>
        /// 定标参数设定
        /// </summary>
        [Description("定标参数设定")]
        SetCalibrationParam = 0x64,

        /// <summary>
        /// DAC电压设置
        /// </summary>
        [Description("DAC电压设置")]
        DacVoltageSetting = 0x65,

        /// <summary>
        /// 对准调节
        /// </summary>
        [Description("对准调节")]
        AlignmentAdjustment = 0xd0,

        /// <summary>
        /// 波长校准
        /// </summary>
        [Description("波长校准")]
        WavelengthCalibration = 0x71,

        /// <summary>
        /// 单次扫描
        /// </summary>
        [Description("单次扫描")]
        SingleScan = 0x72,

        /// <summary>
        /// 触发扫描
        /// </summary>
        [Description("触发扫描")]
        TriggerScan = 0x73,

        /// <summary>
        /// 摆动扫描
        /// </summary>
        [Description("摆动扫描")]
        SwingScan = 0x74,

        /// <summary>
        /// 无刷狭缝原点记录
        /// </summary>
        [Description("无刷狭缝原点记录")]
        BrushlessSlitOriginRecord = 0x81,

        /// <summary>
        /// 光栅记录当前位置为零点
        /// </summary>
        [Description("光栅记录当前位置为零点")]
        RecordRasterCurrentPositionAsZero = 0x82,

        /// <summary>
        /// 无刷狭缝记录当前位置为零点
        /// </summary>
        [Description("无刷狭缝记录当前位置为零点")]
        RecordBrushlessSlitCurrentPositionAsZero = 0x83,

        /// <summary>
        /// 获取零点编码器值
        /// </summary>
        [Description("获取零点编码器值")]
        GetZeroPointEncoderValue = 0x84,

        /// <summary>
        /// 范围采集
        /// </summary>
        [Description("范围采集")]
        RangeCollection = 0x85,

        /// <summary>
        /// 定点采集
        /// </summary>
        [Description("定点采集")]
        PointCollection = 0x86,

        /// <summary>
        /// 停止采集
        /// </summary>
        [Description("停止采集")]
        StopCollection = 0x87,

        /// <summary>
        /// 光栅原点记录
        /// </summary>
        [Description("光栅原点记录")]
        RasterOriginRecord = 0x88,

        /// <summary>
        /// 设置到无刷电机原点
        /// </summary>
        [Description("设置到无刷电机原点")]
        SetToBrushlessMotorOrigin = 0x89,

        /// <summary>
        /// 获取系统配置
        /// </summary>
        [Description("获取系统配置")]
        GetSystemConfig = 0xD0,

        /// <summary>
        /// 写入系统配置
        /// </summary>
        [Description("写入系统配置")]
        WriteSystemConfig = 0xD1,

        /// <summary>
        /// 设置波长系数
        /// </summary>
        [Description("设置波长系数")]
        SetWavelengthCoefficient = 0xE0,

        /// <summary>
        /// 获取波长系数
        /// </summary>
        [Description("获取波长系数")]
        GetWavelengthCoefficient = 0xE1,

        /// <summary>
        /// 获取版本号
        /// </summary>
        [Description("获取版本号")]
        GetVersion = 0xF9,

        /// <summary>
        /// 写入SN
        /// </summary>
        [Description("写入SN")]
        WriteSn = 0xFA,

        /// <summary>
        /// 读取
        /// </summary>
        [Description("读取")]
        ReadSn = 0xFB,

        /// <summary>
        /// 写入生产日期
        /// </summary>
        [Description("写入生产日期")]
        WriteProductionDate = 0xFC,

        /// <summary>
        /// 读取生产日期
        /// </summary>
        [Description("读取生产日期")]
        ReadProductionDate = 0xFD,

        /// <summary>
        /// 写入产品型号
        /// </summary>
        [Description("写入产品型号")]
        WriteProductModel = 0xFE,

        /// <summary>
        /// 获取产品型号
        /// </summary>
        [Description("获取产品型号")]
        GetProductModel = 0xFF
    }
}
