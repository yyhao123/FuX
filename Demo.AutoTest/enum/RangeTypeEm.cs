//using Demo.Windows.Controls.property.core.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.AutoTest.@enum
{
    /// <summary>
    /// 范围采集模式
    /// </summary>
    public enum RangeTypeEm
    {
        /// <summary>
        /// 全谱
        /// </summary>
        [Description("全谱")]   
        All = 0,
        /// <summary>
        /// 指定范围
        /// </summary>
        [Description("范围")]     
        Range = 1

    }
}
