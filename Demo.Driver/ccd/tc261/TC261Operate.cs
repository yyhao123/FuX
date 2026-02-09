using FTD2XX_NET;
using Demo.Core.@abstract;
using Demo.Model.@interface;
using Demo.Unility;
using FuX.Core.handler;
using FuX.Log;
using FuX.Model.attribute;
using FuX.Model.data;
using FuX.Unility;
using System.Runtime.CompilerServices;
using static FTD2XX_NET.FTDI;
using static Demo.Driver.ccd.tc261.TC261Data;

namespace Demo.Driver.ccd.tc261
{
    /// <summary>
    /// TC261操作类
    /// </summary>
    public class TC261Operate : CCDAbstract<TC261Operate, Basics>, ICCD
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public TC261Operate() : base()
        {
            SetLanguage(FuX.Model.@enum.LanguageType.en);
        }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="basics">基础数据</param>
        public TC261Operate(Basics basics) : base(basics) { }

        /// <inheritdoc/>
        protected override string CD => "支持TC261所有命令操作";

        /// <inheritdoc/>
        protected override string CN => "TC261";

        /// <inheritdoc/>
        public override LanguageModel LanguageOperate { get; set; } = new("Demo.Language", "Language", "Demo.Language.dll");

        #region 私有属性
        /// <summary>
        /// UART 模式（串口通信）
        /// </summary>
        FTDI Communication_UART = new FTDI();
        /// <summary>
        /// FIFO 模式（并行通信）
        /// </summary>
        FTDI Communication_FIFO = new FTDI();

        /// <summary>
        /// ADC斜率
        /// </summary>
        float slope_ADC = 0;
        /// <summary>
        /// ADC偏移
        /// </summary>
        float offset_ADC = 0;

        /// <summary>
        /// DAC斜率
        /// </summary>
        float slope_DAC = 0;

        /// <summary>
        /// DAC偏移
        /// </summary>
        float offset_DAC = 0;

        /// <summary>
        /// 设备信息
        /// </summary>
        DeviceModel deviceModel;

        /// <summary>
        /// X轴像素点
        /// </summary>
        int XPixelPoint = 0;

        /// <summary>
        /// Y轴像素点
        /// </summary>
        int YPixelPoint = 0;
        #endregion

        #region 私有函数
        /// <summary>
        /// 计算像素点
        /// </summary>
        /// <param name="x">x轴像素点</param>
        /// <param name="y">y轴像素点</param>
        /// <param name="msg">错误信息</param>
        /// <returns>成功失败</returns>
        private bool PixelPoints(out int x, out int y, out string? msg)
        {
            x = XPixelPoint;
            y = YPixelPoint;
            msg = null;
            if (XPixelPoint == 0 || YPixelPoint == 0)
            {
                if (!Get_ROI_Y_Size().GetDetails(out string? message, out object? obj))
                {
                    msg = message;
                    return false;
                }
                ushort ySize = obj.GetSource<ushort>();
                if (!Get_ROI_X_Size().GetDetails(out message, out obj))
                {
                    msg = message;
                    return false;
                }
                ushort xSize = obj.GetSource<ushort>();
                if (!Get_X_Binning().GetDetails(out message, out obj))
                {
                    msg = message;
                    return false;
                }
                BinningType xBinning = obj.GetSource<BinningType>();
                if (!Get_Y_Binning().GetDetails(out message, out obj))
                {
                    msg = message;
                    return false;
                }
                BinningType yBinning = obj.GetSource<BinningType>();

                x = XPixelPoint = xSize / ((byte)xBinning + 1);
                y = YPixelPoint = ySize / ((byte)yBinning + 1);
            }
            return true;
        }

        /// <summary>
        /// 单次通用处理
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="communication">通信方式</param>
        /// <param name="methodName">方法名</param>
        /// <returns>统一结果</returns>
        private OperateResult SingleGeneralHandler(byte[] command, FTDI? communication = null, [CallerMemberName] string methodName = "")
        {
            BegOperate(methodName: methodName);
            try
            {
                if (!GetStatus().GetDetails(out string? message))
                {
                    return EndOperate(false, message, methodName: methodName);
                }

                if (communication == null)
                {
                    communication = Communication_UART;
                }
                LogHelper.Verbose($"{methodName} Write：{command.HexToStr()}", $"{TAG}/Bytes.log");

                uint length = 0;
                FT_STATUS status = communication.Write(command, command.Length, ref length);
                if (status != FT_STATUS.FT_OK)
                {
                    return EndOperate(false, status.ToString(), methodName: methodName);
                }

                byte[] buffer = new byte[basics.BufferSize];
                status = communication.SetTimeouts((uint)basics.ReadTimeout, (uint)basics.WriteTimeout);
                if (status != FT_STATUS.FT_OK)
                {
                    return EndOperate(false, status.ToString(), methodName: methodName);
                }
                status = communication.Read(buffer, (uint)buffer.Length, ref length);
                if (status != FT_STATUS.FT_OK)
                {
                    return EndOperate(false, status.ToString(), methodName: methodName);
                }

                byte[] datas = buffer.Take((int)length).ToArray();

                LogHelper.Verbose($"{methodName} Read：{datas.HexToStr()}", $"{TAG}/Bytes.log");

                if (datas.Length <= 0)
                {
                    if (NoReturnCommands.Select(c => c == methodName).First())
                    {
                        return EndOperate(true, methodName: methodName);
                    }
                    else
                    {
                        return EndOperate(false, "无数据返回", methodName: methodName);
                    }
                }

                //统一解析处
                TC261Status tcs = (TC261Status)Enum.ToObject(typeof(TC261Status), datas[0]);
                MultilingualAttribute? attribute = tcs.GetAttribute<MultilingualAttribute>();
                if (attribute != null)
                {
                    switch (GetLanguage())
                    {
                        case FuX.Model.@enum.LanguageType.zh:
                            message = attribute.Zh;
                            break;
                        case FuX.Model.@enum.LanguageType.en:
                            message = attribute.En;
                            break;
                    }
                    switch (tcs)
                    {
                        case TC261Status.ETX:
                            return EndOperate(true, message, methodName: methodName);
                        default:
                            return EndOperate(false, message, methodName: methodName);
                    }
                }

                return ResultHandler(datas, methodName: methodName);
            }
            catch (Exception ex)
            {
                return EndOperate(false, ex.Message, exception: ex, methodName: methodName);
            }
        }

        //无返回数据
        string[] NoReturnCommands = ["Set_Exposure", "Set_Number_of_Frames_to_Capture", "Set_Number_of_Frames_to_Readout", "Set_ROI_X_Size", "Set_ROI_Y_Size"];

        /// <summary>
        /// 多次通用处理
        /// </summary>
        /// <param name="commands">命令集合</param>
        /// <param name="communication">通信方式</param>
        /// <param name="methodName">方法名</param>
        /// <param name="cycleCountType">周期计数类型</param>
        /// <returns>统一结果</returns>
        private OperateResult SingleGeneralHandler(List<byte[]> commands, FTDI? communication = null, [CallerMemberName] string methodName = "", CycleCountType cycleCountType = CycleCountType.Clks)
        {
            BegOperate(methodName: methodName);
            try
            {
                if (!GetStatus().GetDetails(out string? message))
                {
                    return EndOperate(false, message, methodName: methodName);
                }

                if (communication == null)
                {
                    communication = Communication_UART;
                }

                //数据缓冲区
                List<byte> dataBuffer = new List<byte>();

                for (int i = 0; i < commands.Count; i++)
                {
                    LogHelper.Verbose($"{methodName} Write：{commands[i].HexToStr()}", $"{TAG}/Bytes.log");

                    uint length = 0;
                    FT_STATUS status = communication.Write(commands[i], commands[i].Length, ref length);
                    if (status != FT_STATUS.FT_OK)
                    {
                        return EndOperate(false, status.ToString(), methodName: methodName);
                    }

                    byte[] buffer = new byte[basics.BufferSize];
                    status = communication.SetTimeouts((uint)basics.ReadTimeout, (uint)basics.WriteTimeout);
                    if (status != FT_STATUS.FT_OK)
                    {
                        return EndOperate(false, status.ToString(), methodName: methodName);
                    }
                    status = communication.Read(buffer, (uint)buffer.Length, ref length);
                    if (status != FT_STATUS.FT_OK)
                    {
                        return EndOperate(false, status.ToString(), methodName: methodName);
                    }

                    byte[] datas = buffer.Take((int)length).ToArray();

                    LogHelper.Verbose($"{methodName} Read：{datas.HexToStr()}", $"{TAG}/Bytes.log");

                    byte[] newDatas = datas.RemoveSequence(new byte[] { commands[i][^2], commands[i][^1] });
                    if (datas.Length > 0 && commands[i][1] == 0xE1 || commands[i][1] == 0xAF)
                    {
                        dataBuffer.AddRange(newDatas);
                    }
                }
                if (dataBuffer.Count <= 0)
                {
                    if (!NoReturnCommands.Where(c => c == methodName).FirstOrDefault().IsNullOrWhiteSpace())
                    {
                        return EndOperate(true, methodName: methodName);
                    }
                    else
                    {
                        return EndOperate(false, "无数据返回", methodName: methodName);
                    }
                }

                //统一解析处
                TC261Status tcs = (TC261Status)Enum.ToObject(typeof(TC261Status), dataBuffer[0]);
                MultilingualAttribute? attribute = tcs.GetAttribute<MultilingualAttribute>();
                if (attribute != null)
                {
                    switch (GetLanguage())
                    {
                        case FuX.Model.@enum.LanguageType.zh:
                            message = attribute.Zh;
                            break;
                        case FuX.Model.@enum.LanguageType.en:
                            message = attribute.En;
                            break;
                    }
                    switch (tcs)
                    {
                        case TC261Status.ETX:
                            return EndOperate(true, message, methodName: methodName);
                        default:
                            return EndOperate(false, message, methodName: methodName);
                    }
                }
                return ResultHandler(dataBuffer.ToArray(), methodName: methodName);
            }
            catch (Exception ex)
            {
                return EndOperate(false, ex.Message, exception: ex, methodName: methodName);
            }
        }

        /// <summary>
        /// 硬件返回的结果处理
        /// </summary>
        /// <param name="datas">数据</param>
        /// <param name="methodName">方法名</param>
        /// <param name="cycleCountType">周期计数类型</param>
        /// <returns>统一结果</returns>
        private OperateResult ResultHandler(byte[] datas, [CallerMemberName] string methodName = "", CycleCountType cycleCountType = CycleCountType.Clks)
        {
            BegOperate(methodName: methodName);
            try
            {
                if (!GetStatus().GetDetails(out string? message))
                {
                    throw CustomException.Create(message);
                }

                //获取命令结果值
                object GetResultValue = null;
                switch (methodName)
                {
                    default:
                        //统一解析处
                        TC261Status tcs = (TC261Status)Enum.ToObject(typeof(TC261Status), datas[0]);
                        MultilingualAttribute attribute = tcs.GetAttribute<MultilingualAttribute>();
                        switch (GetLanguage())
                        {
                            case FuX.Model.@enum.LanguageType.zh:
                                message = attribute.Zh;
                                break;
                            case FuX.Model.@enum.LanguageType.en:
                                message = attribute.En;
                                break;
                        }
                        switch (tcs)
                        {
                            case TC261Status.ETX:
                                return EndOperate(true, message, methodName: methodName);
                            default:
                                return EndOperate(false, message, methodName: methodName);
                        }
                    case "Get_System_Status":
                        GetResultValue = new SystemStateModel(
                            datas[0].GetBit(0),
                            datas[0].GetBit(1),
                            datas[0].GetBit(2),
                            datas[0].GetBit(4),
                            datas[0].GetBit(6));
                        break;
                    case "Get_FPGA_Status":
                        GetResultValue = new FPGAModel(datas[0].GetBit(7), datas[0].GetBit(1), datas[0].GetBit(0));
                        break;
                    case "Get_Exposure":
                    case "Get_Fixed_Frame_Rate":
                        GetResultValue = (((ulong)datas[0] << 32) |
                                         ((ulong)datas[1] << 24) |
                                         ((ulong)datas[2] << 16) |
                                         ((ulong)datas[3] << 8) |
                                         datas[4]) / ((ulong)cycleCountType.GetAttribute<MultilingualAttribute>().En.ToLong());
                        break;
                    case "Get_Capture_Trigger_Mode":
                        GetResultValue = new CaptureTriggerModel(
                            datas[0].GetBit(0),
                            datas[0].GetBit(1),
                            datas[0].GetBit(2),
                            datas[0].GetBit(3),
                            datas[0].GetBit(6),
                            datas[0].GetBit(7));
                        break;
                    case "Get_TEC_Set_Point":
                        GetResultValue = (((datas[0] << 8) + datas[1]) * slope_DAC) + offset_DAC;
                        break;
                    case "Get_ROI_X_Size":
                    case "Get_ROI_X_Offset":
                    case "Get_ROI_Y_Size":
                    case "Get_ROI_Y_Offset":
                        GetResultValue = (ushort)((((ushort)(datas[0] & 0x0F)) << 8) | datas[1]);
                        break;
                    case "Get_X_Binning":
                    case "Get_Y_Binning":
                        GetResultValue = (BinningType)datas[0];
                        break;
                    case "Get_Readout_Mode":
                        GetResultValue = (ReadoutType)datas[0];
                        break;
                    case "Get_Number_of_Frames_to_Capture":
                    case "Get_Number_of_Frames_to_Readout":
                        GetResultValue = (ushort)((datas[0] << 8) | datas[1]);
                        break;
                    case "Get_Memory_Controller_Write_Address":
                    case "Get_Memory_Controller_Read_Address":
                        uint address = ((uint)datas[0] << 24) |
                                       ((uint)datas[1] << 16) |
                                       ((uint)datas[2] << 8) |
                                       datas[3];
                        GetResultValue = address;
                        break;
                    case "Get_Readout_Trigger_Mode":
                        byte readoutTriggerMode = datas[0];
                        GetResultValue = new ReadoutTriggerModel(readoutTriggerMode.GetBit(1), readoutTriggerMode.GetBit(0));
                        break;
                    case "Get_CCD_Silicon_Temperature":
                        int svalue = (datas[0] << 8) | datas[1];
                        float temp = (svalue * slope_ADC) + offset_ADC;
                        GetResultValue = temp;
                        break;
                    case "Get_PCB_Temperature":
                        int raw = ((datas[0] & 0x0F) << 8) | datas[1];
                        if ((raw & 0x800) != 0)
                        {
                            raw -= 0x1000;
                        }
                        GetResultValue = raw / 16.0;
                        break;
                    case "Get_Micro_Version":
                    case "Get_FPGA_Version":
                        GetResultValue = new VersionModel(datas[0], datas[1]);
                        break;
                    case "Get_Unit_Serial_Number":
                        GetResultValue = (ushort)((datas[1] << 8) | datas[0]);
                        break;
                    case "Get_Manufacturers_Data":
                        EPROMModel result = new EPROMModel()
                        {
                            SerialNumber = (ushort)(datas[0] | (datas[1] << 8)),
                            BuildDate = new DateTime(year: 2000 + datas[4], month: datas[3], day: datas[2]),
                            BuildCode = System.Text.Encoding.ASCII.GetString(datas, 5, 5),
                            ADCCal0C = (ushort)(datas[10] | (datas[11] << 8)),
                            ADCCal40C = (ushort)(datas[12] | (datas[13] << 8)),
                            DACCal0C = (ushort)(datas[14] | (datas[15] << 8)),
                            DACCal40C = (ushort)(datas[16] | (datas[17] << 8))
                        };
                        slope_DAC = (0f - 40f) / (result.DACCal0C - result.DACCal40C);
                        offset_DAC = 40f - (slope_DAC * result.DACCal40C);

                        slope_ADC = (0f - 40f) / (result.ADCCal0C - result.ADCCal40C);
                        offset_ADC = 40f - (slope_ADC * result.ADCCal40C);

                        GetResultValue = result;
                        break;
                }
                return EndOperate(true, resultData: GetResultValue, methodName: methodName);
            }
            catch (Exception ex)
            {
                return EndOperate(false, ex.Message, exception: ex, methodName: methodName);
            }
        }
        #endregion

        #region 通用接口函数
        /// <inheritdoc/>
        public override OperateResult On()
        {
            BegOperate();
            try
            {
                if (GetStatus().GetDetails(out string? message))
                {
                    return EndOperate(false, message);
                }

                //设置验证状态
                TC261Command.VerifyStatus(basics.Verify);

                //----------------
                FT_STATUS status = Communication_UART.OpenBySerialNumber(basics.SerialNumber_UART);
                if (status != FT_STATUS.FT_OK)
                {
                    return EndOperate(false, LanguageOperate.GetLanguageValue($"打开FTDI设备失败") + $"，{status}");
                }
                if (Communication_UART.IsOpen)
                {
                    //清除接收和发送缓冲区
                    if (Communication_UART.Purge(FTDI.FT_PURGE.FT_PURGE_RX | FTDI.FT_PURGE.FT_PURGE_TX) != FT_STATUS.FT_OK)
                    {
                        return EndOperate(false, LanguageOperate.GetLanguageValue($"清除缓冲区失败"));
                    }
                    //设置波特率
                    if (Communication_UART.SetBaudRate(basics.BaudRate) != FT_STATUS.FT_OK)
                    {
                        return EndOperate(false, LanguageOperate.GetLanguageValue($"设置波特率失败"));
                    }
                    //设置数据位
                    if (Communication_UART.SetDataCharacteristics(FT_DATA_BITS.FT_BITS_8, FT_STOP_BITS.FT_STOP_BITS_1, FT_PARITY.FT_PARITY_NONE) != FT_STATUS.FT_OK)
                    {
                        return EndOperate(false, LanguageOperate.GetLanguageValue($"设置数据位失败"));
                    }
                    //设置流量控制
                    if (Communication_UART.SetFlowControl(FT_FLOW_CONTROL.FT_FLOW_NONE, 0x11, 0x13) != FT_STATUS.FT_OK)
                    {
                        return EndOperate(false, LanguageOperate.GetLanguageValue($"设置流量控制失败"));
                    }
                }

                //-------------------
                status = Communication_FIFO.OpenBySerialNumber(basics.SerialNumber_FIFO);
                if (status != FT_STATUS.FT_OK)
                {
                    return EndOperate(false, LanguageOperate.GetLanguageValue($"打开FTDI设备失败") + $"，{status}");
                }
                if (Communication_FIFO.IsOpen)
                {
                    //清除接收和发送缓冲区
                    if (Communication_FIFO.Purge(FTDI.FT_PURGE.FT_PURGE_RX | FTDI.FT_PURGE.FT_PURGE_TX) != FT_STATUS.FT_OK)
                    {
                        return EndOperate(false, LanguageOperate.GetLanguageValue($"清除缓冲区失败"));
                    }
                    //设置流量控制
                    if (Communication_FIFO.SetFlowControl(FT_FLOW_CONTROL.FT_FLOW_NONE, 0x11, 0x13) != FT_STATUS.FT_OK)
                    {
                        return EndOperate(false, LanguageOperate.GetLanguageValue($"设置流量控制失败"));
                    }
                }

                if (!Init().GetDetails(out message))
                {
                    Off(true);
                    return EndOperate(false, message);
                }

                return EndOperate(true);
            }
            catch (Exception ex)
            {
                Off(true);
                return EndOperate(false, ex.Message, exception: ex);
            }
        }

        /// <inheritdoc/>
        public override OperateResult Off(bool hardClose = false)
        {
            BegOperate();
            try
            {
                if (!hardClose)
                {
                    if (!GetStatus().GetDetails(out string? message))
                    {
                        return EndOperate(false, message);
                    }
                }
                // 关闭设备
                Communication_UART?.Close();
                Communication_FIFO?.Close();
                return EndOperate(true);
            }
            catch (Exception ex)
            {
                return EndOperate(false, ex.Message, exception: ex);
            }
        }

        /// <inheritdoc/>
        public override OperateResult GetStatus()
        {
            BegOperate();
            if (Communication_UART != null && Communication_UART.IsOpen && Communication_FIFO != null && Communication_FIFO.IsOpen)
            {
                return EndOperate(true, LanguageOperate.GetLanguageValue("已连接"), logOutput: false);
            }
            else
            {
                return EndOperate(false, LanguageOperate.GetLanguageValue("未连接"), logOutput: false);
            }
        }

        /// <inheritdoc/>
        public override OperateResult Gather(int value)
        {
            BegOperate();
            try
            {
                if (!Get_CCD_Silicon_Temperature().GetDetails(out string? message, out object? temp))
                    return EndOperate(false, message);

                if ((((float)temp / basics.SetPointTemperature) * 100) < basics.GatherThresholdPercentage)
                    return EndOperate(false, $"采集失败，温度未达到采集的最佳温度，当前温度：{temp}");

                if (!Set_Capture_Trigger_Mode(new()).GetDetails(out message))
                    return EndOperate(false, message);

                if (!Set_Readout_Trigger_Mode(ReadoutTriggerModel.SetReadoutSequence()).GetDetails(out message))
                    return EndOperate(false, message);

                if (!Set_Exposure((ulong)value).GetDetails(out message))
                    return EndOperate(false, message);

                if (!PixelPoints(out int x_pp, out int y_pp, out message))
                    return EndOperate(false, message);

                double p = ((2E-06 * x_pp) * y_pp) * 1000.0;
                uint timeout = (uint)((value + p) + 2000.0);
                Communication_FIFO.SetTimeouts((uint)timeout, basics.WriteTimeout);

                //读取数据，读最大缓冲区
                byte[] dataBuffer = new byte[x_pp * y_pp];
                uint length = 0;
                Communication_FIFO.Read(dataBuffer, (uint)dataBuffer.Length, ref length);
                byte[] newBytes = dataBuffer.Take((int)length).ToArray();

                //处理数据
                List<ushort> datas = new List<ushort>();
                for (int j = 0; j < newBytes.Length - 1; j += 2)
                {
                    ushort data = (ushort)((newBytes[j] << 8) | newBytes[j + 1]); // 高位在前
                    datas.Add(data);
                }
                if (datas.Count <= 0)
                {
                    return EndOperate(false, "未采集到数据");
                }
                return EndOperate(true, resultData: datas);
            }
            catch (Exception ex)
            {
                return EndOperate(false, ex.Message, exception: ex);
            }
        }
        /// <inheritdoc/>
        public override OperateResult GetTemperature()
            => EndOperate(Get_CCD_Silicon_Temperature().GetDetails(out object? data), resultData: data, methodName: BegOperate());
        /// <inheritdoc/>
        public override OperateResult SetTemperature(long value)
            => EndOperate(Set_TEC_Set_Point(value).GetDetails(out object? data), resultData: data, methodName: BegOperate());

        #endregion

        #region 设备命令函数
        /// <summary>
        /// 重置设备
        /// </summary>
        /// <returns>统一结果</returns>
        public OperateResult Init()
        {
            BegOperate();

            if (!Micro_RESET().GetDetails(out string? message))
                return EndOperate(false, message);

            TimeHandler.DelayUs(3000);

            if (!Set_System_State(new()).GetDetails(out message))
                return EndOperate(false, message);

            if (!Get_Manufacturers_Data().GetDetails(out message))
                return EndOperate(false, message);

            if (!Get_TEC_Set_Point().GetDetails(out message, out object? value))
                return EndOperate(false, message);
            if (value?.ToInt() != basics.SetPointTemperature)
            {
                if (!Set_TEC_Set_Point(basics.SetPointTemperature).GetDetails(out message))
                    return EndOperate(false, message);
            }

            if (!Get_X_Binning().GetDetails(out message, out value))
                return EndOperate(false, message);
            if ((BinningType)value != BinningType.X1)
            {
                if (!Set_X_Binning(BinningType.X1).GetDetails(out message))
                    return EndOperate(false, message);
            }
            if (!Get_Y_Binning().GetDetails(out message, out value))
                return EndOperate(false, message);
            if ((BinningType)value != BinningType.FVB)
            {
                if (!Set_Y_Binning(BinningType.FVB).GetDetails(out message))
                    return EndOperate(false, message);
            }

            if (!Set_Number_of_Frames_to_Capture(1).GetDetails(out message))
                return EndOperate(false, message);

            if (!Set_Number_of_Frames_to_Readout(1).GetDetails(out message))
                return EndOperate(false, message);

            if (!Set_FPGA_CTRL_reg(new FPGAModel { enableHighPreAmpGain = basics.GainType == GainType.HG ? true : false }).GetDetails(out message))
                return EndOperate(false, message);

            Gather(1000);

            return EndOperate(true);
        }

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="communication">通信方式</param>
        /// <returns>统一结果</returns>
        public OperateResult GetDeviceInfo(FTDI communication)
        {
            BegOperate();
            deviceModel = new DeviceModel();
            if (!SingleGeneralHandler(TC261Command.Get_Manufacturers_Data(), communication, methodName: "Get_Manufacturers_Data").GetDetails(out string? message, out object? data))
            {
                return EndOperate(false, message, consoleOutput: false);
            }
            deviceModel.EPROM = data.GetSource<EPROMModel>();
            if (!SingleGeneralHandler(TC261Command.Get_System_Status(), communication, methodName: "Get_System_Status").GetDetails(out message, out data))
            {
                return EndOperate(false, message, consoleOutput: false);
            }
            deviceModel.SystemState = data.GetSource<SystemStateModel>();
            if (!SingleGeneralHandler(TC261Command.Get_Micro_Version(), communication, methodName: "Get_Micro_Version").GetDetails(out message, out data))
            {
                return EndOperate(false, message, consoleOutput: false);
            }
            deviceModel.MicroVersion = data.GetSource<VersionModel>();
            if (!SingleGeneralHandler(TC261Command.Get_FPGA_Version(), communication, methodName: "Get_FPGA_Version").GetDetails(out message, out data))
            {
                return EndOperate(false, message, consoleOutput: false);
            }
            deviceModel.FPGAVersion = data.GetSource<VersionModel>();
            if (!SingleGeneralHandler(TC261Command.Get_PCB_Temperature(), communication, methodName: "Get_PCB_Temperature").GetDetails(out message, out data))
            {
                return EndOperate(false, message, consoleOutput: false);
            }
            deviceModel.PCBTemp = data.GetSource<float>();
            if (!SingleGeneralHandler(TC261Command.Get_CCD_Silicon_Temperature(), communication, methodName: "Get_CCD_Silicon_Temperature").GetDetails(out message, out data))
            {
                return EndOperate(false, message, consoleOutput: false);
            }
            deviceModel.ROICTemp = data.GetSource<float>();
            return EndOperate(true, resultData: deviceModel);
        }

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <returns>统一结果</returns>
        public OperateResult GetDeviceInfo() => GetDeviceInfo(Communication_UART);

        /// <summary>
        /// 重置
        /// </summary>
        /// <returns>统一结果</returns>
        public OperateResult Micro_RESET()
        {
            BegOperate();
            if (!GetStatus().GetDetails(out string? message))
            {
                return EndOperate(false, message);
            }
            byte[] bytes = TC261Command.Micro_RESET();
            uint length = 0;
            FT_STATUS status = Communication_UART.Write(bytes, bytes.Length, ref length);
            if (status == FT_STATUS.FT_OK)
            {
                return EndOperate(true);
            }
            return EndOperate(false, status.ToString());
        }

        /// <summary>
        /// 设置系统状态<br/>
        /// 8 bit value YY
        /// Bit 7 = Reserved
        /// Bit 6 = 1 check sum mode enabled
        /// Bit 5 = Reserved
        /// Bit 4 = 1 to enable command ACK
        /// Bit 3 = Reserved
        /// Bit 2 = 1 if FPGA booted ok
        /// Bit 1 = 0 to Hold FPGA in RESET
        /// Bit 0 = 1 to enable comms to FPGAEPROM
        /// </summary>
        /// <param name="model">系统状态</param>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Set_System_State(SystemStateModel model)
            => SingleGeneralHandler(TC261Command.Set_System_State(model));

        /// <summary>
        /// 设置FPGA CTRL reg<br/>
        /// YY Bit 7 = 0 to enable high pre amp gain i.e. ~TBD e/ADU(Default= 0)<br/>
        /// YY Bit 6,5,4,3,2 = reserved(Default= 0)<br/>
        /// YY Bit 1 = 1 to reset the temperature trip flag, self-clearing bit(Default= 0)<br/>
        /// YY Bit 0 = 1 to enable TEC(Default= 0)<br/>
        /// </summary>
        /// <param name="model">FPGA模型</param>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Set_FPGA_CTRL_reg(FPGAModel model)
            => SingleGeneralHandler(TC261Command.Set_FPGA_CTRL_reg(model));

        /// <summary>
        /// 设置帧率
        /// </summary>
        /// <param name="value">帧率</param>
        /// <param name="cycleCountType">周期计数类型</param>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Set_Frame_rate(ulong value, CycleCountType cycleCountType = CycleCountType.Clks)
            => SingleGeneralHandler(TC261Command.Set_Frame_rate(value, cycleCountType));

        /// <summary>
        /// 设置曝光时间<br/>
        /// 40 bit value, 5 separate commands, 1count = 1 × 40MHz period = 25nsecs<br/>
        /// Y1 = MSB of 5 byte word<br/>
        /// :::<br/>
        /// Y5 = LSB of 5 byte word;<br/>
        /// Frame rate updated on LSB write
        /// </summary>
        /// <param name="value">时间</param>
        /// <param name="cycleCountType">周期计数类型</param>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Set_Exposure(ulong value, CycleCountType cycleCountType = CycleCountType.Clks)
            => SingleGeneralHandler(TC261Command.Set_Exposure(value, cycleCountType));

        /// <summary>
        /// 设置捕获触发模式<br/>
        /// YY Bit 7 = 1 to enable rising edge, = 0 falling edge Ext trigger(Default= 1)
        /// YY Bit 6 = 1 to enable External trigger (Default= 0)
        /// YY Bit 3 = 1 to Abort current exposure, self clearing bit(Default= 0)
        /// YY Bit 2 = 1 to enable Internal Trigger (Default = 0)
        /// YY Bit 1 = 1 to enable Fixed Frame Rate mode(Default = 0)
        /// YY Bit 0 = 1 for snapshot, self-clearing bit(Default= 0)
        /// </summary>
        /// <param name="model">捕捉触发模型</param>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Set_Capture_Trigger_Mode(CaptureTriggerModel model)
            => SingleGeneralHandler(TC261Command.Set_Capture_Trigger_Mode(model));

        /// <summary>
        /// 设置TEC设置点<br/>
        /// 12 bit DAC value, LSB = LL byte, Lower nibble of MM = MSBs<br/>
        /// Reg 0x03, bits 3..0 = set point bits 11..8 <br/>
        /// Reg 0x04, bits 7..0 = set point bits 7..0  <br/>
        /// 12 bit value to be converted to temperature(see Get manufacturers Data section 3.14)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Set_TEC_Set_Point(long value)
            => SingleGeneralHandler(TC261Command.Set_TEC_Set_Point(value, slope_DAC, offset_DAC));

        /// <summary>
        /// 设置 ROI X 尺寸（12-bit）<br/>
        /// 根据协议，将高 4 位写入 0xB4（仅 bits 3..0），低 8 位写入 0xB5<br/>
        /// 必须先写 LSB（0xB5）才会触发寄存器更新<br/>
        /// </summary>
        /// <param name="size">12-bit ROI 尺寸值（范围 0~4095）</param>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Set_ROI_X_Size(ushort size)
            => SingleGeneralHandler(TC261Command.Set_ROI_X_Size(size));

        /// <summary>
        /// 设置 ROI X Offset（偏移量），16-bit 值，数据更新在写入 LSB 时触发。
        /// 高位写入寄存器 0xB6，低位写入寄存器 0xB7。
        /// </summary>
        /// <param name="offset">16-bit ROI X 偏移量（范围 0~65535）</param>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Set_ROI_X_Offset(ushort offset)
            => SingleGeneralHandler(TC261Command.Set_ROI_X_Offset(offset));


        /// <summary>
        /// 设置 ROI Y 尺寸（12-bit）
        /// 数据写入 0xB8（高 4 位）与 0xB9（低 8 位），写入顺序必须是先 LSB（0xB9）
        /// </summary>
        /// <param name="size">ROI Y 尺寸（0~4095）</param>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Set_ROI_Y_Size(ushort size)
            => SingleGeneralHandler(TC261Command.Set_ROI_Y_Size(size));

        /// <summary>
        /// 设置 ROI Y Offset（12-bit）
        /// 高位写入 0xBA（bits 3..0 = offset bits 11..8），低位写入 0xBB（offset bits 7..0）
        /// 数据更新在写入 LSB（0xBB）时触发。
        /// </summary>
        /// <param name="value">12-bit 偏移量值（0~4095）</param>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Set_ROI_Y_Offset(int value)
            => SingleGeneralHandler(TC261Command.Set_ROI_Y_Offset(value));

        /// <summary>
        /// 设置 X轴 倍数
        /// </summary>
        /// <param name="binning">倍数</param>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Set_X_Binning(BinningType binning)
            => SingleGeneralHandler(TC261Command.Set_X_Binning(binning));

        /// <summary>
        /// 设置 Y轴 倍数
        /// </summary>
        /// <param name="binning">倍数</param>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Set_Y_Binning(BinningType binning)
            => SingleGeneralHandler(TC261Command.Set_Y_Binning(binning));

        /// <summary>
        /// 设置读出模式
        /// </summary>
        /// <param name="readout">读出类型</param>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Set_Readout_Mode(ReadoutType readout)
            => SingleGeneralHandler(TC261Command.Set_Readout_Mode(readout));

        /// <summary>
        /// 生成设置图像采集帧数的指令（两帧）
        /// </summary>
        /// <param name="value">要采集的帧数（0~65535）</param>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Set_Number_of_Frames_to_Capture(ushort value)
            => SingleGeneralHandler(TC261Command.Set_Number_of_Frames_to_Capture(value));

        /// <summary>
        /// 生成设置读出帧数的指令（两帧）
        /// </summary>
        /// <param name="value"></param>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Set_Number_of_Frames_to_Readout(ushort value)
            => SingleGeneralHandler(TC261Command.Set_Number_of_Frames_to_Readout(value));

        /// <summary>
        /// 设置读取触发模式的命令
        /// </summary>
        /// <param name="model">读出触发模型</param>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Set_Readout_Trigger_Mode(ReadoutTriggerModel model)
            => SingleGeneralHandler(TC261Command.Set_Readout_Trigger_Mode(model));

        /// <summary>
        /// 设置内存控制器模式
        /// </summary>
        /// <param name="reset">是否重置内存控制器</param>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Set_Memory_Controller_Mode(bool reset)
            => SingleGeneralHandler(TC261Command.Set_Memory_Controller_Mode(reset));

        /// <summary>
        /// 获取系统状态
        /// </summary>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Get_System_Status()
            => SingleGeneralHandler(TC261Command.Get_System_Status());

        /// <summary>
        /// 获取FPGA状态
        /// </summary>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Get_FPGA_Status()
            => SingleGeneralHandler(TC261Command.Get_FPGA_Status());

        /// <summary>
        /// 获取曝光时间
        /// </summary>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Get_Exposure(CycleCountType cycleCountType = CycleCountType.Clks)
            => SingleGeneralHandler(TC261Command.Get_Exposure(), cycleCountType: cycleCountType);

        /// <summary>
        /// 获取固定帧率
        /// </summary>
        /// <param name="cycleCountType">周期刷新率</param>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Get_Fixed_Frame_Rate(CycleCountType cycleCountType = CycleCountType.Clks)
            => SingleGeneralHandler(TC261Command.Get_Fixed_Frame_Rate(), cycleCountType: cycleCountType);

        /// <summary>
        /// 获取捕获触发模式
        /// </summary>
        /// <returns></returns>
        public OperateResult Get_Capture_Trigger_Mode()
            => SingleGeneralHandler(TC261Command.Get_Capture_Trigger_Mode());

        /// <summary>
        /// 获取TEC设置点
        /// </summary>
        /// <returns></returns>
        public OperateResult Get_TEC_Set_Point()
            => SingleGeneralHandler(TC261Command.Get_TEC_Set_Point());

        /// <summary>
        /// 获取ROI X轴大小
        /// </summary>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Get_ROI_X_Size()
            => SingleGeneralHandler(TC261Command.Get_ROI_X_Size());

        /// <summary>
        /// 获取ROI X轴 偏移
        /// </summary>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Get_ROI_X_Offset()
            => SingleGeneralHandler(TC261Command.Get_ROI_X_Offset());

        /// <summary>
        /// 获取ROI Y轴大小
        /// </summary>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Get_ROI_Y_Size()
            => SingleGeneralHandler(TC261Command.Get_ROI_Y_Size());

        /// <summary>
        /// 获取ROI Y轴 偏移
        /// </summary>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Get_ROI_Y_Offset()
            => SingleGeneralHandler(TC261Command.Get_ROI_Y_Offset());

        /// <summary>
        /// 获取X轴倍数
        /// </summary>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Get_X_Binning()
            => SingleGeneralHandler(TC261Command.Get_X_Binning());

        /// <summary>
        /// 获取Y轴倍数
        /// </summary>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Get_Y_Binning()
            => SingleGeneralHandler(TC261Command.Get_Y_Binning());

        /// <summary>
        /// 获取读出模式
        /// </summary>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Get_Readout_Mode()
            => SingleGeneralHandler(TC261Command.Get_Readout_Mode());

        /// <summary>
        /// 获取要捕获的帧数
        /// </summary>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Get_Number_of_Frames_to_Capture()
            => SingleGeneralHandler(TC261Command.Get_Number_of_Frames_to_Capture());

        /// <summary>
        /// 获取要读出的帧数
        /// </summary>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Get_Number_of_Frames_to_Readout()
            => SingleGeneralHandler(TC261Command.Get_Number_of_Frames_to_Readout());


        /// <summary>
        /// 获取内存控制器写入地址
        /// </summary>
        /// <returns></returns>
        public OperateResult Get_Memory_Controller_Write_Address()
            => SingleGeneralHandler(TC261Command.Get_Memory_Controller_Write_Address());

        /// <summary>
        /// 获取内存控制器读取地址
        /// </summary>
        /// <returns></returns>
        public OperateResult Get_Memory_Controller_Read_Address()
            => SingleGeneralHandler(TC261Command.Get_Memory_Controller_Read_Address());

        /// <summary>
        /// 获取读出触发模式
        /// </summary>
        /// <returns></returns>
        public OperateResult Get_Readout_Trigger_Mode()
            => SingleGeneralHandler(TC261Command.Get_Readout_Trigger_Mode());

        /// <summary>
        /// 获取PCB温度
        /// </summary>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Get_PCB_Temperature()
            => SingleGeneralHandler(TC261Command.Get_PCB_Temperature());

        /// <summary>
        /// 获取CCD温度
        /// </summary>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Get_CCD_Silicon_Temperature()
            => SingleGeneralHandler(TC261Command.Get_CCD_Silicon_Temperature());

        /// <summary>
        /// 获取主板版本
        /// </summary>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Get_Micro_Version()
            => SingleGeneralHandler(TC261Command.Get_Micro_Version());

        /// <summary>
        /// 获取FPGA版本
        /// </summary>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Get_FPGA_Version()
            => SingleGeneralHandler(TC261Command.Get_FPGA_Version());

        /// <summary>
        /// 获取装置序列号
        /// </summary>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Get_Unit_Serial_Number()
            => SingleGeneralHandler(TC261Command.Get_Unit_Serial_Number());

        /// <summary>
        /// 获取生产日期
        /// </summary>
        /// <returns>打包后的字节指令列表</returns>
        public OperateResult Get_Manufacturers_Data()
            => SingleGeneralHandler(TC261Data.TC261Command.Get_Manufacturers_Data());
        #endregion
    }
}
