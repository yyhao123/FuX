using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.@enum
{

    /// <summary>
    /// 探测器类型
    /// </summary>
    public enum CCDType
{
        /// <summary>
        /// 单元
        /// </summary>
        [Description("点")]
        Ponit = 0,

        /// <summary>
        /// 线阵
        /// </summary>
        [Description("线")]
        Line = 1,

        /// <summary>
        /// 面
        /// </summary>
        [Description("面")]
        Area = 2,
}
}
