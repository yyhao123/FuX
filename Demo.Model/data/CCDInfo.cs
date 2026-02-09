using Demo.Model.@enum;
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
    public class 
        CCDInfo
    {
        /// <summary>
        /// CCD名称
        /// </summary>
        public string ccdName { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public string point { get; set; }

        /// <summary>
        /// ccd像素
        /// </summary>
        public int ccdpx { get; set; }

        /// <summary>
        /// 绑定的出光口 01,1 00,2
        /// </summary>
        public string outno { get; set; }

        /// <summary>
        /// 绑定的入光口 01,1 00,2
        /// </summary>
        public string inno { get; set; }

        /// <summary>
        /// 绑定的出光口 01,1 00,2
        /// </summary>
        public string outname { get; set; }



        /// <summary>
        /// 相机类型
        /// </summary>
        public CCDType CCDType { get; set; }

        /// <summary>
        /// 切换外置CCD位置多个CCD时
        /// </summary>
        public int CCDChageMin { get; set; }

        /// <summary>
        /// 切换外置CCD位置多个CCD时
        /// </summary>
        public int CCDChageMax { get; set; }

        public bool IsDefaultCCD { get; set; }

        public bool ICCDEnable { get; set; }

        /// <summary>
        /// CCD范围信息
        /// </summary>
        public List<CCDRangeInfo> CCDRangeInfos { get; set; } = new List<CCDRangeInfo>();

    }
}
