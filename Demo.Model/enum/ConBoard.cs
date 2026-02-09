using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.@enum
{
    public enum ConBoard
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,
        /// <summary>
        /// 主设备
        /// </summary>
        [Description("主设备")]
        Mcon = 1,
        /// <summary>
        /// 从设备
        /// </summary>
        [Description("从设备")]
        Dcon = 2
    }
}
