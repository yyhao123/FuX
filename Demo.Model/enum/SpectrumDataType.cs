using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.@enum
{
    public enum SpectrumDataType
    {
        /// <summary>
        /// 原始光谱数据
        /// </summary>
        Raw = 0,

        /// <summary>
        /// 暗底光谱数据
        /// </summary>
        Dark = 1,

        /// <summary>
        /// 扣除暗底光谱数据
        /// </summary>
        DarkSubtracted = 2,
        /// <summary>
        /// 吸光度
        /// </summary>
        AbsorbanceData = 3,
        /// <summary>
        /// 透射率
        /// </summary>
        TransmissivityData = 4,
        /// <summary>
        /// 反射率
        /// </summary>
        ReflectivityData = 5,
        /// <summary>
        /// 辐照度
        /// </summary>
        IrradianceData = 6,
        /// <summary>
        /// 白板
        /// </summary>
        White = 7
    }
}
