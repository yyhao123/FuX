using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.@enum
{
    /// <summary>
    /// 数据处理方法
    /// </summary>
    public enum PretreatMethod
    {
        /// <summary>
        /// 未做处理
        /// </summary>
        None = 0,

        /// <summary>
        /// 基线校正
        /// </summary>
        BaselineCorrect = 1,

        /// <summary>
        /// 一阶导数
        /// </summary>
        FirstDerivative = 2,

        /// <summary>
        /// 归一
        /// </summary>
        Normalize = 3,

        /// <summary>
        /// 常数运算
        /// </summary>
        ConstantArithmetic = 4,

        /// <summary>
        /// 谱图运算
        /// </summary>
        SpectrumArithmetic = 5,

        /// <summary>
        /// 平滑
        /// </summary>
        Smooth = 6,

        /// <summary>
        /// 二阶导数
        /// </summary>
        SecondDerivative = 7,
        /// <summary>
        /// 去峰
        /// </summary>
        RemovePeak = 8
    }
}
