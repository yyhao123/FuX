using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    public class SpectrumPeak
    {
        public string SpectrumId { get; set; }

        public string SpectrumDataRawId { get; set; }

        /// <summary>
        /// 峰Y值
        /// </summary>
        public double YValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double XValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int XIndex { get; set; }

        /// <summary>
        /// 是否显示峰
        /// </summary>
        public bool IsShow { get; set; } = true;

        public SpectrumNode Node { get; set; }
    }
}
