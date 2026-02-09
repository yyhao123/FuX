using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    /// <summary>
    /// 传感器信息
    /// </summary>
    public class DYDataInfo
    {
        /// <summary>
        /// CCD1
        /// </summary>
        public int CCD1 { get; set; }

        /// <summary>
        /// CCD2
        /// </summary>
        public int CCD2 { get; set; }

        /// <summary>
        /// 编码器值
        /// </summary>
        public int address { get; set; }

        /// <summary>
        /// 波长值
        /// </summary>
        public int wel { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public double timenum { get; set; }

        /// <summary>
        /// x转换值
        /// </summary>
        public double xnum { get; set; }

        /// <summary>
        /// y转换值
        /// </summary>
        public double ynum { get; set; }
    }
}
