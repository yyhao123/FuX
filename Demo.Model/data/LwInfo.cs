using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    /// <summary>
    /// 激光基础信息
    /// </summary>
    public class LwInfo
    {
        /// <summary>
        /// 激光名称
        /// </summary>
        public int LwName { get; set; }

        /// <summary>
        /// 串口
        /// </summary>
        public string point { get; set; }

        /// <summary>
        /// 最大功率
        /// </summary>
        public int MaximumLaserPower { get; set; }

        /// <summary>
        /// 调用方式
        /// </summary>
       // public LaserType LaserType { get; set; }

        /// <summary>
        /// ccd像素
        /// </summary>
        public int CCDSize { get; set; }

        /// <summary>
        /// 激光指令
        /// </summary>
        public byte LaserCmd { get; set; }

        /// <summary>
        /// 传感器信息
        /// </summary>
        public string cddName { get; set; }

        /// <summary>
        /// 传感器信息
        /// </summary>
        public string CCDType { get; set; }

        /// <summary>
        /// 采集方式(true正，false反)
        /// </summary>
        public bool Isjust { get; set; }

        /// <summary>
        /// 入光口
        /// </summary>
        public string inno { get; set; }

        /// <summary>
        /// 入光口
        /// </summary>
        public byte inbyte { get; set; }

        /// <summary>
        /// 出光口
        /// </summary>
        public byte outbyte { get; set; }

        /// <summary>
        /// 入光口名
        /// </summary>
        public string inname { get; set; }

        /// <summary>
        /// 出光口名
        /// </summary>
        public string outname { get; set; }

    }
}
