using FuX.Unility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Demo.Driver.ccd.ATP2000SH
{
    public class ATP2000Data
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
            /// 设备IP（UDP通信）
            /// </summary>
            [Description("设备IP")]
            public string SerialIP { get; set; } = "192.168.0.2";

            /// <summary>
            /// 端口
            /// </summary>
            [Description("设备端口")]
            public int SerialPort { get; set; } = 8080;

        }
    }
}
