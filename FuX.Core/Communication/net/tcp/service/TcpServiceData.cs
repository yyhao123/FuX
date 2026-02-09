using FuX.Unility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FuX.Core.Communication.net.tcp.service
{
    public class TcpServiceData
    {
        public class Basics
        {
            [Category("基础数据")]
            [Description("唯一标识符")]
            public string? SN { get; set; } = Guid.NewGuid().ToUpperNString();


            [Description("Ip地址")]
            public string? IpAddress { get; set; } = "127.0.0.1";


            [Description("端口")]
            public int Port { get; set; } = 6688;


            [Description("最大连接数")]
            public int MaxNumber { get; set; } = 1000;


            [Description("最大块大小")]
            public int MaxChunkSize { get; set; } = 261120;


            [Description("重试发送次数")]
            public int RetrySendCount { get; set; } = 5;


            [Description("数据缓冲区大小")]
            public int BufferSize { get; set; } = 1048576;

        }

        public class ClientMessage
        {
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public Steps Step { get; set; }

            public string IpPort { get; set; }

            public byte[]? Bytes { get; set; }
        }

        public enum Steps
        {
            客户端连接,
            客户端断开,
            消息接收
        }
    }
}
