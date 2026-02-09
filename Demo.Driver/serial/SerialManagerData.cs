using FuX.Core.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Demo.Driver.serial
{
    public class SerialManagerData
    {
        //
        // 摘要:
        //     串口通信基础数据
        public class Basics : BasicsData
        {
            //
            // 摘要:
            //     串口号
            [Category("基础数据")]
            [Description("串口号")]
            public string? PortName { get; set; } = "COM4";


            //
            // 摘要:
            //     波特率
            [Description("波特率")]
            [DisplayName("hhhhh")]
            public int BaudRate { get; set; } = 115200;


            //
            // 摘要:
            //     校验位
            [Description("校验位")]
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public Parity ParityBit { get; set; } = Parity.None;


            //
            // 摘要:
            //     数据位
            [Description("数据位")]
            public int DataBit { get; set; } = 8;


            //
            // 摘要:
            //     停止位
            [Description("停止位")]
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public StopBits StopBit { get; set; } = StopBits.One;


            //
            // 摘要:
            //     写入超时时间
            [Description("写入超时时间")]
            public int WriteTimeout { get; set; } = 1000;


            //
            // 摘要:
            //     读取超时时间
            [Description("读取超时时间")]
            public int ReadTimeout { get; set; } = 5000;


            //
            // 摘要:
            //     接收缓冲区中数据的字节数阈值
            [Description("接收缓冲区中数据的字节数阈值")]
            public int ReceivedBytesThreshold { get; set; } = 1;

        }
    }
}
