using FuX.Core.Communication.serial;
using FuX.Unility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Driver.atp
{
    public class ATPData
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
            /// 串口数据
            /// </summary>
            [Description("串口数据")]
            public SerialData.Basics SerialData { get; set; }

            /// <summary>
            /// UDP通信数据
            /// </summary>
           // [Description("UDP通信数据")]
           // public UdpData.Basics UdpData { get; set; }
        }
    }
}
