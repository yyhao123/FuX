using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.@enum
{
     /// <summary>
     /// 高光谱数据显示类型
     /// </summary>
    public enum DisplayDataType
{
    /// <summary>
    /// 光谱
    /// </summary>
    Spectrum = 0,
    /// <summary>
    /// 反射率
    /// </summary>
    Reflectivity = 1,
    /// <summary>
    /// 辐亮度
    /// </summary>
    Radiance = 2,
    /// <summary>
    /// 透射率
    /// </summary>
    Transmittance = 3,
    /// <summary>
    /// 吸光度
    /// </summary>
    Absorbance = 4
}
}
