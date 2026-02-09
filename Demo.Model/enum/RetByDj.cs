using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.@enum
{
    /// <summary>
    /// 电机状态
    /// </summary>
    public enum RetByDj : byte
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 0x00,
        /// <summary>
        /// 最小限位
        /// </summary>
        [Description("最小限位")]
        Limit_Min = 0x01,
        /// <summary>
        /// 最大限位
        /// </summary>
        [Description("最大限位")]
        Limit_Max = 0x02,
        /// <summary>
        /// 电机繁忙
        /// </summary>
        [Description("电机繁忙")]
        Busy = 0x03,
        /// <summary>
        /// 限位错误
        /// </summary>
        [Description("限位错误")]
        Limit_Err = 0x04,
        /// <summary>
        /// 数据长度错误
        /// </summary>
        [Description("数据长度错误")]
        Data_Err = 0x05
    }
}
