using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.@enum
{
    /// <summary>
    /// 采集类型
    /// </summary>
    public enum AcquireType
    {
        /// <summary>
        /// 单次采集
        /// </summary>
        Single = 0,

        /// <summary>
        /// Mapping采集
        /// </summary>
        Mapping = 1,

        Unknown = 99
    }
}
