using Demo.Unility;
using FuX.Model.attribute;
using FuX.Unility;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Demo.Driver.ccd.tc261
{
    /// <summary>
    /// 数据
    /// </summary>
    public class TC261Data
    {
        /// <summary>
        /// 基础数据
        /// </summary>
        public class Basics
        {
            /// <summary>
            /// 唯一标识符
            /// </summary>
            [Category("数据")]
            [Description("唯一标识符")]
            public string? SN { get; set; } = Guid.NewGuid().ToUpperNString();

            /// <summary>
            /// 设备序列号_UART 模式（串口通信）
            /// </summary>
            [Description("设备序列号")]
            public string SerialNumber_UART { get; set; } = "FT8ZF5THA";

            /// <summary>
            /// 设备序列号_FIFO 模式（并行通信）
            /// </summary>
            [Description("设备序列号")]
            public string SerialNumber_FIFO { get; set; } = "FT8ZF5THB";

            /// <summary>
            /// 波特率
            /// </summary>
            [Description("波特率")]
            public uint BaudRate { get; set; } = 115200;

            /// <summary>
            /// 读取超时时间
            /// </summary>
            [Description("读取超时时间")]
            public uint ReadTimeout { get; set; } = 50;

            /// <summary>
            /// 写入超时时间
            /// </summary>
            [Description("写入超时时间")]
            public uint WriteTimeout { get; set; } = 10;

            /// <summary>
            /// 数据缓冲区大小
            /// </summary>
            [Description("数据缓冲区大小")]
            public int BufferSize { get; set; } = 100;

            /// <summary>
            /// 设置点温度<br/>
            /// 如果设置的不是这个温度值，则自动设置
            /// </summary>
            [Description("设置点温度")]
            public long SetPointTemperature { get; set; } = -70;

            /// <summary>
            /// 采集阈值百分比
            /// </summary>
            [Description("采集阈值百分比")]
            public float GatherThresholdPercentage { get; set; } = 90;

            /// <summary>
            /// 倍数类型
            /// </summary>
            [Description("倍数类型")]
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public BinningType BinningType { get; set; } = BinningType.FVB;

            /// <summary>
            /// 增益类型
            /// </summary>
            [Description("增益类型")]
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public GainType GainType { get; set; } = GainType.LG;

            /// <summary>
            /// 验证
            /// </summary>
            [Description("验证")]
            public bool Verify { get; set; } = true;

            /// <summary>
            /// 发送等待间隔
            /// </summary>
            [Description("发送等待间隔")]
            public int SendWaitInterval { get; set; } = 10000;
        }

        /// <summary>
        /// 增益类型
        /// </summary>
        public enum GainType
        {
            /// <summary>
            /// 高增益<br/>
            /// 适合弱信号短时间曝光情况
            /// </summary>
            [Description("高增益")]
            HG,
            /// <summary>
            /// 低增益<br/>
            /// 适合弱信号长时间曝光情况（暗电流噪声会比较小）
            /// </summary>
            [Description("低增益")]
            LG
        }

        /// <summary>
        /// TC261响应状态
        /// </summary>
        public enum TC261Status : byte
        {
            /// <summary>
            /// 成功
            /// </summary>
            [Multilingual("成功", "Success")]
            ETX = 0x50,
            /// <summary>
            /// 超时
            /// </summary>
            [Multilingual("超时", "TimeOut")]
            ETX_SER_TIMEOUT = 0x51,
            /// <summary>
            /// 校验错误
            /// </summary>
            [Multilingual("校验错误", "VerificationError")]
            ETX_CK_SUM_ERR = 0x52,
            /// <summary>
            /// 处理失败
            /// </summary>
            [Multilingual("处理失败", "HandleFailed")]
            ETX_I2C_ERR = 0x53,
            /// <summary>
            /// 无效数据
            /// </summary>
            [Multilingual("无效数据", "InvalidData")]
            ETX_UNKNOWN_CMD = 0x54,
            /// <summary>
            /// 繁忙
            /// </summary>
            [Multilingual("繁忙", "Busy")]
            ETX_DONE_LOW = 0x55
        }

        /// <summary>
        /// TC261命令
        /// </summary>
        public class TC261Command
        {
            /// <summary>
            /// 校验状态
            /// </summary>
            private static bool _verifyStatus = false;
            /// <summary>
            /// 是否需要校验
            /// </summary>
            /// <param name="status"></param>
            public static void VerifyStatus(bool status)
            {
                _verifyStatus = status;
            }
            /// <summary>
            /// 校验
            /// </summary>
            /// <param name="datas">数据</param>
            /// <returns>返回已经校验好的结果</returns>
            private static byte[] Verify(byte[] datas)
            {
                if (!_verifyStatus)
                    return datas;

                byte checksum = 0;
                foreach (byte b in datas)
                {
                    checksum ^= b;
                }
                byte[] result = new byte[datas.Length + 1];
                Array.Copy(datas, result, datas.Length);
                result[result.Length - 1] = checksum;
                return result;
            }

            /// <summary>
            /// 重置
            /// </summary>
            /// <returns>打包后的字节指令列表</returns>
            public static byte[] Micro_RESET()
            {
                return [0x55, 0x99, 0x66, 0x11, 0x50, 0xEB];
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
            public static byte[] Set_System_State(SystemStateModel model)
            {
                byte result = 0x00;
                result = result.SetBit(0, model.enable_comms_to_FPGAEPROM);
                result = result.SetBit(1, model.Hold_FPGA_in_RESET);
                result = result.SetBit(2, model.if_FPGA_booted_ok);
                result = result.SetBit(4, model.enable_command_ACK);
                result = result.SetBit(6, model.check_sum_mode_enabled);
                return Verify([0x4f, result, 0x50]);
                //return Verify([0x4f, 0x52, 0x50]);
            }

            /// <summary>
            /// 设置 FPGA 控制寄存器<br/>
            /// YY Bit 7 = 0 to enable high pre amp gain i.e. ~TBD e/ADU(Default= 0)<br/>
            /// YY Bit 6,5,4,3,2 = reserved(Default= 0)<br/>
            /// YY Bit 1 = 1 to reset the temperature trip flag, self-clearing bit(Default= 0)<br/>
            /// YY Bit 0 = 1 to enable TEC(Default= 0)<br/>
            /// </summary>
            /// <param name="model">FPGA模型</param>
            /// <returns>打包后的字节指令列表</returns>
            public static byte[] Set_FPGA_CTRL_reg(FPGAModel model)
            {
                byte result = 0x00;
                result = result.SetBit(0, model.enableTEC);
                result = result.SetBit(1, model.resetTempTripFlag);
                result = result.SetBit(7, model.enableHighPreAmpGain);
                return Verify([0x53, 0xE0, 0x02, 0x00, result, 0x50]);
            }

            /// <summary>
            /// 设置帧率
            /// </summary>
            /// <param name="value">帧率</param>
            /// <param name="cycleCountType">周期计数类型</param>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Set_Frame_rate(ulong value, CycleCountType cycleCountType = CycleCountType.Clks)
            {
                //进行数据转换
                MultilingualAttribute attribute = cycleCountType.GetAttribute<MultilingualAttribute>();
                ulong unitValue = (ulong)attribute.En.ToLong();

                // 计算40bit数据 (5字节)
                ulong data = (ulong)(unitValue * value);

                // 命令数组
                var cmds = new byte[] { 0xDC, 0xDD, 0xDE, 0xDF, 0xE0 };

                //进行数据拼接
                List<byte[]> result = new List<byte[]>();
                for (int i = 0; i < 5; i++)
                {
                    byte b = (byte)(data >> (8 * (4 - i)) & 0xFF);
                    var cmd = new byte[] { 0x53, 0xE0, 0x02, cmds[i], b, 0x50 };
                    result.Add(Verify(cmd));
                }
                return result;
            }

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
            public static List<byte[]> Set_Exposure(ulong value, CycleCountType cycleCountType = CycleCountType.Clks)
            {
                //进行数据转换
                MultilingualAttribute attribute = cycleCountType.GetAttribute<MultilingualAttribute>();
                ulong unitValue = (ulong)attribute.En.ToLong();

                // 计算40bit数据 (5字节)
                ulong data = (ulong)(unitValue * value);

                // 命令数组
                var cmds = new byte[] { 0xED, 0xEE, 0xEF, 0xF0, 0xF1 };

                //进行数据拼接
                List<byte[]> result = new List<byte[]>();
                for (int i = 0; i < 5; i++)
                {
                    byte b = (byte)(data >> (8 * (4 - i)) & 0xFF);
                    var cmd = new byte[] { 0x53, 0xE0, 0x02, cmds[i], b, 0x50 };
                    result.Add(Verify(cmd));
                }
                return result;
            }

            /// <summary>
            /// 设置捕获触发模式<br/>
            /// 文档是0xD4，但实际测试是0xD3<br/>
            /// YY Bit 7 = 1 to enable rising edge, = 0 falling edge Ext trigger(Default= 1)
            /// YY Bit 6 = 1 to enable External trigger (Default= 0)
            /// YY Bit 3 = 1 to Abort current exposure, self clearing bit(Default= 0)
            /// YY Bit 2 = 1 to enable Internal Trigger (Default = 0)
            /// YY Bit 1 = 1 to enable Fixed Frame Rate mode(Default = 0)
            /// YY Bit 0 = 1 for snapshot, self-clearing bit(Default= 0)
            /// </summary>
            /// <param name="model">捕捉触发模型</param>
            /// <returns>打包后的字节指令列表</returns>
            public static byte[] Set_Capture_Trigger_Mode(CaptureTriggerModel model)
            {
                byte result = 0x00;
                result = result.SetBit(0, model.self_clearing_snapshot);
                result = result.SetBit(1, model.enable_Fixed_Frame_Rate_mode);
                result = result.SetBit(2, model.enable_Internal_Trigger);
                result = result.SetBit(3, model.self_clearing_Abort_current_exposure);
                result = result.SetBit(6, model.enable_External_trigger);
                result = result.SetBit(7, model.types);
                return Verify([0x53, 0xE0, 0x02, 0xD4, result, 0x50]);
            }

            /// <summary>
            /// 设置TEC设置点<br/>
            /// 12 bit DAC value, LSB = LL byte, Lower nibble of MM = MSBs<br/>
            /// Reg 0x03, bits 3..0 = set point bits 11..8 <br/>
            /// Reg 0x04, bits 7..0 = set point bits 7..0  <br/>
            /// 12 bit value to be converted to temperature(see Get manufacturers Data section 3.14)
            /// </summary>
            /// <param name="value">值</param>
            /// <param name="slope">斜率</param>
            /// <param name="offset">偏移量</param>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Set_TEC_Set_Point(long value, float slope, float offset)
            {
                int data = Convert.ToInt32((value - offset) / slope);
                //进行数据拼接
                List<byte[]> result = new List<byte[]>();
                byte msbNibble = (byte)((data >> 8) & 0x0F);
                byte lsbByte = (byte)(data & 0xFF);
                result.Add(Verify([0x53, 0xE0, 0x02, 0x03, msbNibble, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x02, 0x04, lsbByte, 0x50]));
                return result;
            }

            /// <summary>
            /// 设置 ROI X 尺寸（12-bit）<br/>
            /// 根据协议，将高 4 位写入 0xB4（仅 bits 3..0），低 8 位写入 0xB5<br/>
            /// 必须先写 LSB（0xB5）才会触发寄存器更新<br/>
            /// </summary>
            /// <param name="size">12-bit ROI 尺寸值（范围 0~4095）</param>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Set_ROI_X_Size(ushort size)
            {
                List<byte[]> result = new List<byte[]>();
                byte highBits = (byte)((size >> 8) & 0x0F);
                byte lowBits = (byte)(size & 0xFF);
                result.Add(Verify([0x53, 0xE0, 0x02, 0xB4, highBits, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x02, 0xB5, lowBits, 0x50]));
                return result;
            }

            /// <summary>
            /// 设置 ROI X Offset（偏移量），16-bit 值，数据更新在写入 LSB 时触发。
            /// 高位写入寄存器 0xB6，低位写入寄存器 0xB7。
            /// </summary>
            /// <param name="offset">16-bit ROI X 偏移量（范围 0~65535）</param>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Set_ROI_X_Offset(ushort offset)
            {
                List<byte[]> result = new List<byte[]>();
                byte highBits = (byte)((offset >> 8) & 0xFF);
                byte lowBits = (byte)(offset & 0xFF);
                result.Add(Verify([0x53, 0xE0, 0x02, 0xB6, highBits, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x02, 0xB7, lowBits, 0x50]));
                return result;
            }

            /// <summary>
            /// 设置 ROI Y 尺寸（12-bit）
            /// 数据写入 0xB8（高 4 位）与 0xB9（低 8 位），写入顺序必须是先 LSB（0xB9）
            /// </summary>
            /// <param name="size">ROI Y 尺寸（0~4095）</param>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Set_ROI_Y_Size(ushort size)
            {
                List<byte[]> result = new List<byte[]>();
                byte highBits = (byte)((size >> 8) & 0x0F);
                byte lowBits = (byte)(size & 0xFF);
                result.Add(Verify([0x53, 0xE0, 0x02, 0xB8, highBits, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x02, 0xB9, lowBits, 0x50]));
                return result;
            }

            /// <summary>
            /// 设置 ROI Y Offset（12-bit）
            /// 高位写入 0xBA（bits 3..0 = offset bits 11..8），低位写入 0xBB（offset bits 7..0）
            /// 数据更新在写入 LSB（0xBB）时触发。
            /// </summary>
            /// <param name="value">12-bit 偏移量值（0~4095）</param>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Set_ROI_Y_Offset(int value)
            {
                List<byte[]> result = new List<byte[]>();
                byte highBits = (byte)((value >> 8) & 0x0F);
                byte lowBits = (byte)(value & 0xFF);
                result.Add(Verify([0x53, 0xE0, 0x02, 0xBA, highBits, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x02, 0xBB, lowBits, 0x50]));
                return result;
            }

            /// <summary>
            /// 设置 X轴 倍数
            /// </summary>
            /// <param name="binning">倍数</param>
            /// <returns>打包后的字节指令列表</returns>
            public static byte[] Set_X_Binning(BinningType binning)
            {
                return Verify([0x53, 0xE0, 0x02, 0xA1, (byte)binning, 0x50]);
            }

            /// <summary>
            /// 设置 Y轴 倍数
            /// </summary>
            /// <param name="binning">倍数</param>
            /// <returns>打包后的字节指令列表</returns>
            public static byte[] Set_Y_Binning(BinningType binning)
            {
                return Verify([0x53, 0xE0, 0x02, 0xA2, (byte)binning, 0x50]);
            }

            /// <summary>
            /// 设置读出模式
            /// </summary>
            /// <param name="readout">读出类型</param>
            /// <returns>打包后的字节指令列表</returns>
            public static byte[] Set_Readout_Mode(ReadoutType readout)
            {
                return Verify([0x53, 0xE0, 0x02, 0xf7, (byte)readout, 0x50]);
            }

            /// <summary>
            /// 生成设置图像采集帧数的指令（两帧）
            /// </summary>
            /// <param name="value">要采集的帧数（0~65535）</param>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Set_Number_of_Frames_to_Capture(ushort value)
            {
                List<byte[]> result = new List<byte[]>();
                byte mm = (byte)((value >> 8) & 0xFF); // 高8位
                byte ll = (byte)(value & 0xFF);        // 低8位
                result.Add(Verify([0x53, 0xE0, 0x02, 0xF9, mm, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x02, 0xF8, ll, 0x50]));
                return result;
            }

            /// <summary>
            /// 生成设置读出帧数的指令（两帧）
            /// </summary>
            /// <param name="value"></param>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Set_Number_of_Frames_to_Readout(ushort value)
            {
                List<byte[]> result = new List<byte[]>();
                byte mm = (byte)((value >> 8) & 0xFF); // 高8位
                byte ll = (byte)(value & 0xFF);        // 低8位
                result.Add(Verify([0x53, 0xE0, 0x02, 0xFB, mm, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x02, 0xFA, ll, 0x50]));
                return result;
            }

            /// <summary>
            /// 设置读取触发模式的命令
            /// </summary>
            /// <param name="model">读出触发模型</param>
            /// <returns>打包后的字节指令列表</returns>
            public static byte[] Set_Readout_Trigger_Mode(ReadoutTriggerModel model)
            {
                byte result = 0x00;
                result = result.SetBit(0, model.ReadoutSnapshotImage);
                result = result.SetBit(1, model.ReadoutSequence);
                return Verify([0x53, 0xE0, 0x02, 0xF2, result, 0x50]);
            }

            /// <summary>
            /// 设置内存控制器模式
            /// </summary>
            /// <param name="reset">是否重置内存控制器</param>
            /// <returns>打包后的字节指令列表</returns>
            public static byte[] Set_Memory_Controller_Mode(bool reset)
            {
                byte result = 0x00;
                result = result.SetBit(3, reset);
                return Verify([0x53, 0xE0, 0x02, 0xD2, result, 0x50]);
            }

            /// <summary>
            /// 获取系统状态
            /// </summary>
            /// <returns>打包后的字节指令列表</returns>
            public static byte[] Get_System_Status()
            {
                return Verify([0x49, 0x50]);
            }

            /// <summary>
            /// 获取FPGA状态
            /// </summary>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Get_FPGA_Status()
            {
                List<byte[]> result = new List<byte[]>();
                result.Add(Verify([0x53, 0xE0, 0x01, 0x00, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                return result;
            }

            /// <summary>
            /// 获取曝光时间
            /// </summary>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Get_Exposure()
            {
                List<byte[]> result = new List<byte[]>();
                result.Add(Verify([0x53, 0xE0, 0x01, 0xED, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x01, 0xEE, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x01, 0xEF, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x01, 0xF0, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x01, 0xF1, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                return result;
            }

            /// <summary>
            /// 获取固定帧率
            /// </summary>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Get_Fixed_Frame_Rate()
            {
                List<byte[]> result = new List<byte[]>();
                result.Add(Verify([0x53, 0xE0, 0x01, 0xDC, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x01, 0xDD, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x01, 0xDE, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x01, 0xDF, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x01, 0XE0, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                return result;
            }

            /// <summary>
            /// 获取捕获触发模式<br/>
            /// 文档是0xD4，但实际测试是0xD3
            /// </summary>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Get_Capture_Trigger_Mode()
            {
                List<byte[]> result = new List<byte[]>();
                result.Add(Verify([0x53, 0xE0, 0x01, 0xD4, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                return result;
            }

            /// <summary>
            /// 获取TEC设置点
            /// </summary>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Get_TEC_Set_Point()
            {
                List<byte[]> result = new List<byte[]>();
                result.Add(Verify([0x53, 0xE0, 0x01, 0x03, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x01, 0x04, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                return result;
            }

            /// <summary>
            /// 获取ROI X轴大小
            /// </summary>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Get_ROI_X_Size()
            {
                List<byte[]> result = new List<byte[]>();
                result.Add(Verify([0x53, 0xE0, 0x01, 0xB4, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x01, 0xB5, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                return result;
            }

            /// <summary>
            /// 获取ROI X轴 偏移
            /// </summary>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Get_ROI_X_Offset()
            {
                List<byte[]> result = new List<byte[]>();
                result.Add(Verify([0x53, 0xE0, 0x01, 0xB6, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x01, 0xB7, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                return result;
            }

            /// <summary>
            /// 获取ROI Y轴大小
            /// </summary>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Get_ROI_Y_Size()
            {
                List<byte[]> result = new List<byte[]>();
                result.Add(Verify([0x53, 0xE0, 0x01, 0xB8, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x01, 0xB9, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                return result;
            }

            /// <summary>
            /// 获取ROI Y轴 偏移
            /// </summary>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Get_ROI_Y_Offset()
            {
                List<byte[]> result = new List<byte[]>();
                result.Add(Verify([0x53, 0xE0, 0x01, 0xBA, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x01, 0xBB, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                return result;
            }

            /// <summary>
            /// 获取X轴倍数
            /// </summary>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Get_X_Binning()
            {
                List<byte[]> result = new List<byte[]>();
                result.Add(Verify([0x53, 0xE0, 0x01, 0xA1, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                return result;
            }

            /// <summary>
            /// 获取Y轴倍数
            /// </summary>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Get_Y_Binning()
            {
                List<byte[]> result = new List<byte[]>();
                result.Add(Verify([0x53, 0xE0, 0x01, 0xA2, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                return result;
            }

            /// <summary>
            /// 获取读出模式
            /// </summary>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Get_Readout_Mode()
            {
                List<byte[]> result = new List<byte[]>();
                result.Add(Verify([0x53, 0xE0, 0x01, 0xF7, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                return result;
            }

            /// <summary>
            /// 获取要捕获的帧数
            /// </summary>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Get_Number_of_Frames_to_Capture()
            {
                List<byte[]> result = new List<byte[]>();
                result.Add(Verify([0x53, 0xE0, 0x01, 0xF9, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x01, 0xF8, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                return result;
            }

            /// <summary>
            /// 获取要读出的帧数
            /// </summary>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Get_Number_of_Frames_to_Readout()
            {
                List<byte[]> result = new List<byte[]>();
                result.Add(Verify([0x53, 0xE0, 0x01, 0xFB, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x01, 0xFA, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                return result;
            }


            /// <summary>
            /// 获取内存控制器写入地址
            /// </summary>
            /// <returns></returns>
            public static List<byte[]> Get_Memory_Controller_Write_Address()
            {
                List<byte[]> result = new List<byte[]>();
                result.Add(Verify([0x53, 0xE0, 0x01, 0x75, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x01, 0x76, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x01, 0x77, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x01, 0x78, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                return result;

            }

            /// <summary>
            /// 获取内存控制器读取地址
            /// </summary>
            /// <returns></returns>
            public static List<byte[]> Get_Memory_Controller_Read_Address()
            {
                List<byte[]> result = new List<byte[]>();
                result.Add(Verify([0x53, 0xE0, 0x01, 0x79, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x01, 0x7A, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x01, 0x7B, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x01, 0x7C, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                return result;

            }

            /// <summary>
            /// 获取读出触发模式
            /// </summary>
            /// <returns></returns>
            public static List<byte[]> Get_Readout_Trigger_Mode()
            {
                List<byte[]> result = new List<byte[]>();
                result.Add(Verify([0x53, 0xE0, 0x01, 0xF2, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                return result;
            }

            /// <summary>
            /// 获取PCB温度
            /// </summary>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Get_PCB_Temperature()
            {
                List<byte[]> result = new List<byte[]>();
                result.Add(Verify([0x53, 0xE0, 0x02, 0x70, 0x00, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x02, 0x71, 0x00, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                return result;
            }

            /// <summary>
            /// 获取CCD温度
            /// </summary>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Get_CCD_Silicon_Temperature()
            {
                List<byte[]> result = new List<byte[]>();
                result.Add(Verify([0x53, 0xE0, 0x02, 0x6E, 0x00, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x02, 0x6F, 0x00, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                return result;
            }

            /// <summary>
            /// 获取主板版本
            /// </summary>
            /// <returns>打包后的字节指令列表</returns>
            public static byte[] Get_Micro_Version()
            {
                return Verify([0x56, 0x50]);
            }

            /// <summary>
            /// 获取FPGA版本
            /// </summary>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Get_FPGA_Version()
            {
                List<byte[]> result = new List<byte[]>();
                result.Add(Verify([0x53, 0xE0, 0x01, 0x7E, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                result.Add(Verify([0x53, 0xE0, 0x01, 0x7F, 0x50]));
                result.Add(Verify([0x53, 0xE1, 0x01, 0x50]));
                return result;
            }

            /// <summary>
            /// 获取装置序列号
            /// </summary>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Get_Unit_Serial_Number()
            {
                List<byte[]> result = new List<byte[]>();
                result.Add(Verify([0x53, 0xAE, 0x05, 0x01, 0x00, 0x00, 0x02, 0x00, 0x50]));
                result.Add(Verify([0x53, 0xAF, 0x02, 0x50]));
                return result;
            }

            /// <summary>
            /// 获取生产日期
            /// </summary>
            /// <returns>打包后的字节指令列表</returns>
            public static List<byte[]> Get_Manufacturers_Data()
            {
                List<byte[]> result = new List<byte[]>();
                result.Add(Verify([0x53, 0xAE, 0x05, 0x01, 0x00, 0x00, 0x02, 0x00, 0x50]));
                result.Add(Verify([0x53, 0xAF, 0x12, 0x50]));
                return result;
            }
        }

        /// <summary>
        /// 系统状态模型
        /// </summary>
        public class SystemStateModel
        {
            /// <summary>
            /// 构造函数
            /// </summary>
            public SystemStateModel()
            {

            }
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="enable_comms_to_FPGAEPROM">启用与 FPGA EPROM 通讯<br/>
            /// Bit 0 = 1 to enable comms to FPGA EPROM </param>
            /// <param name="hold_FPGA_in_RESET">保持 FPGA 在 RESET 状态<br/>
            /// Bit 1 = 0 to Hold FPGA in RESET </param>
            /// <param name="if_FPGA_booted_ok">FPGA 启动成功<br/>
            /// Bit 2 = 1 if FPGA booted ok</param>
            /// <param name="enable_command_ACK">启用命令 ACK<br/>
            /// Bit 4 = 1 to enable command ACK </param>
            /// <param name="check_sum_mode_enabled">校验模式启用<br/>
            /// Bit 6 = 1 check sum mode enabled </param>
            public SystemStateModel(bool enable_comms_to_FPGAEPROM, bool hold_FPGA_in_RESET, bool if_FPGA_booted_ok, bool enable_command_ACK, bool check_sum_mode_enabled)
            {
                this.enable_comms_to_FPGAEPROM = enable_comms_to_FPGAEPROM;
                Hold_FPGA_in_RESET = hold_FPGA_in_RESET;
                this.if_FPGA_booted_ok = if_FPGA_booted_ok;
                this.enable_command_ACK = enable_command_ACK;
                this.check_sum_mode_enabled = check_sum_mode_enabled;
            }

            /// <summary>
            /// 启用与 FPGA EPROM 通讯<br/>
            /// Bit 0 = 1 to enable comms to FPGA EPROM 
            /// </summary>
            public bool enable_comms_to_FPGAEPROM { get; set; } = true;
            /// <summary>
            /// 保持 FPGA 在 RESET 状态<br/>
            /// Bit 1 = 0 to Hold FPGA in RESET 
            /// </summary>
            public bool Hold_FPGA_in_RESET { get; set; } = true;
            /// <summary>
            /// FPGA 启动成功<br/>
            /// Bit 2 = 1 if FPGA booted ok
            /// </summary>
            public bool if_FPGA_booted_ok { get; set; } = true;
            /// <summary>
            /// 启用命令 ACK<br/>
            /// Bit 4 = 1 to enable command ACK 
            /// </summary>
            public bool enable_command_ACK { get; set; } = true;
            /// <summary>
            /// 校验模式启用<br/>
            /// Bit 6 = 1 check sum mode enabled 
            /// </summary>
            public bool check_sum_mode_enabled { get; set; } = true;
        }

        /// <summary>
        /// 版本模型
        /// </summary>
        public class VersionModel
        {
            public VersionModel(int major, int minor)
            {
                Major = major;
                Minor = minor;
            }
            /// <summary>
            /// 主要版本
            /// </summary>
            public int Major { get; set; }
            /// <summary>
            /// 次要版本
            /// </summary>
            public int Minor { get; set; }
        }

        /// <summary>
        /// 设备信息
        /// </summary>
        public class DeviceModel
        {
            /// <summary>
            /// 设备信息构造函数
            /// </summary>
            public DeviceModel()
            { }
            /// <summary>
            /// 设备信息构造函数
            /// </summary>
            /// <param name="ePROM">EPROM 读取的数据模型，包含设备序列号、生产日期、版本代码及校准参数</param>
            /// <param name="systemState">系统状态</param>
            /// <param name="microVersion">Micro 版本</param>
            /// <param name="fPGAVersion">FPGA版本</param>
            /// <param name="pCBTemp">PCB温度</param>
            /// <param name="rOICTemp">ROIC温度</param>
            public DeviceModel(EPROMModel ePROM, SystemStateModel systemState, VersionModel microVersion, VersionModel fPGAVersion, float pCBTemp, float rOICTemp)
            {
                EPROM = ePROM;
                SystemState = systemState;
                MicroVersion = microVersion;
                FPGAVersion = fPGAVersion;
                PCBTemp = pCBTemp;
                ROICTemp = rOICTemp;
            }

            /// <summary>
            /// EPROM 读取的数据模型，包含设备序列号、生产日期、版本代码及校准参数
            /// </summary>
            public EPROMModel EPROM { get; set; }

            /// <summary>
            /// 系统状态
            /// </summary>
            public SystemStateModel SystemState { get; set; }

            /// <summary>
            /// Micro 版本
            /// </summary>
            public VersionModel MicroVersion { get; set; }

            /// <summary>
            /// FPGA版本
            /// </summary>
            public VersionModel FPGAVersion { get; set; }

            /// <summary>
            /// PCB温度
            /// </summary>
            public float PCBTemp { get; set; }

            /// <summary>
            /// ROIC温度
            /// </summary>
            public float ROICTemp { get; set; }
        }

        /// <summary>
        /// EPROM 读取的数据模型，包含设备序列号、生产日期、版本代码及校准参数。
        /// </summary>
        public class EPROMModel
        {
            /// <summary>
            /// 设备序列号，2 字节无符号整数。
            /// EEPROM 中序列号占 2 字节，低字节在前（LSB），高字节在后（MSB）。
            /// 用于唯一标识设备。
            /// </summary>
            public ushort SerialNumber { get; set; }

            /// <summary>
            /// 生产日期，格式为年/月/日。
            /// EEPROM 中的日期由 3 字节表示，分别对应日、月、年（两位数，需加上2000年偏移）。
            /// 用于标识设备的制造时间。
            /// </summary>
            public DateTime BuildDate { get; set; }

            /// <summary>
            /// 版本代码，5 字节 ASCII 字符串。
            /// EEPROM 中存储的设备固件或硬件版本信息，5 个字符组成。
            /// </summary>
            public string BuildCode { get; set; }

            /// <summary>
            /// ADC 校准值，0°C 点对应的数值，2 字节无符号整数。
            /// 用于将 ADC 原始读数转换为实际温度时的校准参考点。
            /// EEPROM 数据以低字节在前（LSB）格式存储。
            /// </summary>
            public ushort ADCCal0C { get; set; }

            /// <summary>
            /// ADC 校准值，+40°C 点对应的数值，2 字节无符号整数。
            /// 另一个温度校准点，用于温度线性插值或计算。
            /// 同样为低字节优先存储格式。
            /// </summary>
            public ushort ADCCal40C { get; set; }

            /// <summary>
            /// DAC 校准值，0°C 点对应的数值，2 字节无符号整数。
            /// 用于数字模拟转换器的温度校准参考。
            /// EEPROM 中为低字节在前格式。
            /// </summary>
            public ushort DACCal0C { get; set; }

            /// <summary>
            /// DAC 校准值，+40°C 点对应的数值，2 字节无符号整数。
            /// 另一个温度校准点，用于 DAC 输出的温度相关调整。
            /// </summary>
            public ushort DACCal40C { get; set; }
        }

        /// <summary>
        /// 读出触发模型
        /// </summary>
        public class ReadoutTriggerModel
        {
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="readoutSequence">通过USB读取图像序列</param>
            /// <param name="readoutSnapshotImage">通过USB读取快照图像，自清除位（默认值=0）</param>
            public ReadoutTriggerModel(bool readoutSequence, bool readoutSnapshotImage)
            {
                ReadoutSequence = readoutSequence;
                ReadoutSnapshotImage = readoutSnapshotImage;
            }
            /// <summary>
            /// 设置通过USB读取图像序列
            /// </summary>
            /// <returns></returns>
            public static ReadoutTriggerModel SetReadoutSequence()
            {
                return new ReadoutTriggerModel(true, false);
            }
            /// <summary>
            /// 设置通过USB读取快照图像
            /// </summary>
            /// <returns></returns>
            public static ReadoutTriggerModel SetReadoutSnapshotImage()
            {
                return new ReadoutTriggerModel(false, true);
            }

            /// <summary>
            /// 通过USB读取图像序列
            /// </summary>
            public bool ReadoutSequence { get; set; } = true;

            /// <summary>
            /// 通过USB读取快照图像，自清除位（默认值=0）
            /// </summary>
            public bool ReadoutSnapshotImage { get; set; } = false;
        }

        /// <summary>
        /// 捕获触发模型
        /// </summary>
        public class CaptureTriggerModel
        {
            /// <summary>
            /// 捕获触发模型 构造函数
            /// </summary>
            public CaptureTriggerModel()
            { }
            /// <summary>
            /// 捕获触发模型 构造函数
            /// </summary>
            /// <param name="self_clearing_snapshot">自动清除快照,默认值false</param>
            /// <param name="enable_Fixed_Frame_Rate_mode">启用固定帧率模式,默认值false</param>
            /// <param name="enable_Internal_Trigger">启用内部触发器,默认值false</param>
            /// <param name="self_clearing_Abort_current_exposure">自动清除中止当前暴露,默认值false</param>
            /// <param name="enable_External_trigger">启用外部触发器,默认值false</param>
            /// <param name="types">
            /// true:上升沿<br/>
            /// false:下降沿<br/>
            /// 默认值 true
            /// </param>
            public CaptureTriggerModel(bool self_clearing_snapshot, bool enable_Fixed_Frame_Rate_mode, bool enable_Internal_Trigger, bool self_clearing_Abort_current_exposure, bool enable_External_trigger, bool types = true)
            {
                this.self_clearing_snapshot = self_clearing_snapshot;
                this.enable_Fixed_Frame_Rate_mode = enable_Fixed_Frame_Rate_mode;
                this.enable_Internal_Trigger = enable_Internal_Trigger;
                this.self_clearing_Abort_current_exposure = self_clearing_Abort_current_exposure;
                this.enable_External_trigger = enable_External_trigger;
                this.types = types;
            }
            /// <summary>
            /// 自动清除快照,默认值false
            /// </summary>
            public bool self_clearing_snapshot { get; set; }
            /// <summary>
            /// 启用固定帧率模式,默认值false
            /// </summary>
            public bool enable_Fixed_Frame_Rate_mode { get; set; }
            /// <summary>
            /// 启用内部触发器,默认值false
            /// </summary>
            public bool enable_Internal_Trigger { get; set; } = true;
            /// <summary>
            /// 自动清除中止当前暴露,默认值false
            /// </summary>
            public bool self_clearing_Abort_current_exposure { get; set; }
            /// <summary>
            /// 启用外部触发器,默认值false
            /// </summary>
            public bool enable_External_trigger { get; set; } = false;
            /// <summary>
            /// true:上升沿<br/>
            /// false:下降沿<br/>
            /// 默认值 true
            /// </summary>
            public bool types { get; set; } = true;
        }

        /// <summary>
        /// FPGA模型
        /// </summary>
        public class FPGAModel
        {
            public FPGAModel()
            { }
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="enableHighPreAmpGain">是否启用高前置放大增益（true = 启用，对应 Bit7 = 0）</param>
            /// <param name="resetTempTripFlag">是否复位温度过热保护（true = Bit1 = 1，自清零）</param>
            /// <param name="enableTEC">是否启用 TEC 制冷器（true = Bit0 = 1）</param>
            public FPGAModel(bool enableHighPreAmpGain, bool resetTempTripFlag, bool enableTEC)
            {
                this.enableHighPreAmpGain = enableHighPreAmpGain;
                this.resetTempTripFlag = resetTempTripFlag;
                this.enableTEC = enableTEC;
            }
            /// <summary>
            /// 是否启用高前置放大增益（true = 启用，对应 Bit7 = 0）
            /// </summary>
            public bool enableHighPreAmpGain { get; set; }
            /// <summary>
            /// 是否复位温度过热保护（true = Bit1 = 1，自清零）
            /// </summary>
            public bool resetTempTripFlag { get; set; }
            /// <summary>
            /// 是否启用 TEC 制冷器（true = Bit0 = 1）
            /// </summary>
            public bool enableTEC { get; set; } = true;
        }

        /// <summary>
        /// TC261周期计数类型
        /// </summary>
        public enum CycleCountType
        {
            /// <summary>
            /// 时钟周期数（40 MHz），即 1 个周期为 25 纳秒，0x9C40 = 40000
            /// </summary>
            [Multilingual("时钟周期", "40000")]
            Clks,
            /// <summary>
            /// 毫秒时钟周期数，1ms = 40,000,000 * 25ns = 0x2625A00L = 40,000,000
            /// </summary>
            [Multilingual("时钟周期毫秒", "40000000")]
            Msec,
            /// <summary>
            /// 无
            /// </summary>
            None
        }

        /// <summary>
        /// 倍数类型
        /// </summary>
        public enum BinningType : byte
        {
            /// <summary>
            /// 1倍
            /// </summary>
            X1 = 0x00,
            /// <summary>
            /// 2倍
            /// </summary>
            X2 = 0x01,
            /// <summary>
            /// 32倍
            /// </summary>
            X32 = 0x1f,
            /// <summary>
            /// 64倍
            /// </summary>
            X64 = 0x3f,
            /// <summary>
            /// FVB
            /// </summary>
            FVB = 0x80
        }

        /// <summary>
        /// 读出类型
        /// </summary>
        public enum ReadoutType : byte
        {
            /// <summary>
            /// 正常模式
            /// </summary>
            Normal = 0x01,
            /// <summary>
            /// 测试模式
            /// </summary>
            Test = 0x04
        }
    }
}
