using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.@enum
{
    public enum DeviceState
    {
        Idle = 0,

        /// <summary>
        /// 设备正在工作
        /// </summary>
        Running = 1,

        /// <summary>
        /// 设备初始化
        /// </summary>
        Initializing = 3,

        /// <summary>
        /// 设备初始化失败
        /// </summary>
        InitializeFail = 4,

        /// <summary>
        /// 自动对焦
        /// </summary>
        AutoFocus = 5,

        /// <summary>
        /// 已连接（usb已连接，未初始化）
        /// </summary>
        PlugIn = 98,

        /// <summary>
        /// 未连接（usb已断开）
        /// </summary>
        PlugOff = 99
    }
}
