using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    /// <summary>
    /// 光谱图参数类
    /// </summary>
    public class dynamicsEt : BicolorNoEt
    {
        /// <summary>
        /// 波长范围
        /// </summary>
        [DescriptionAttribute("波长值")]
        public string WavelengthVal { get; set; }

        /// <summary>
        /// 监测时长
        /// </summary>
        [DescriptionAttribute("监测时长")]
        public string MonitorTime { get; set; }

        /// <summary>w
        /// 间隔时长
        /// </summary>
        [DescriptionAttribute("间隔时长")]
        public string IntervalTime { get; set; }

        /// <summary>
        /// 延迟时长
        /// </summary>
        [DescriptionAttribute("延迟时长")]
        public string DelayTime { get; set; }

        /// <summary>
        /// 是否滑动滤波
        /// </summary>
        [DescriptionAttribute("是否滑动滤波")]
        public bool Filtering { get; set; }

        /// <summary>
        /// 滤波权重
        /// </summary>
        [DescriptionAttribute("滤波权重")]
        public int FilteringWeights { get; set; }

        /// <summary>
        /// S/R转换(默认0标准 1 相反)
        /// </summary>
        [DescriptionAttribute("S/R转换")]
        public int IsSRConvert { get; set; } = 0;

    }
}
