using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    public class DevListShow
    {

        /// <summary>
        /// 设备型号
        /// </summary>
        public string DevName { get; set; }

        /// <summary>
        /// 设备名
        /// </summary>
        public string DevName1 { get; set; }

        /// <summary>
        /// 设备型号
        /// </summary>
        public string DevName2 { get; set; }

        /// <summary>
        /// SN号
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 出厂日期
        /// </summary>
        public string MFDate { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string QV { get; set; }

        /// <summary>
        /// COM口
        /// </summary>
        public string COM { get; set; }

        /// <summary>
        /// 是否链接
        /// </summary>
        public bool IsCon { get; set; } = false;

        /// <summary>
        /// 是否主设备
        /// </summary>
        public bool IsMaster { get; set; } = false;
    }
}
