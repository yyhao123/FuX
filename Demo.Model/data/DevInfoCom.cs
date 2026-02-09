using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    /// <summary>
    /// 设备信息
    /// </summary>
    public class DevInfoCom
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DevName { get; set; }

        /// <summary>
        /// 是否主设备
        /// </summary>
        public bool IsMaster { get; set; } = false;

        /// <summary>
        /// 是否自动连接
        /// </summary>
        public bool IsAutoCon { get; set; } = true;

        /// <summary>
        /// 读取设备型号
        /// </summary>
        public byte Byte_GetDevInfo { get; set; } = 0xFF;

        /// <summary>
        /// 设置设备型号
        /// </summary>
        public byte Byte_SetDevInfo { get; set; } = 0xFE;

        /// <summary>
        /// 读取SN
        /// </summary>
        public byte Byte_GetSn { get; set; } = 0xFB;

        /// <summary>
        /// 读取版本
        /// </summary>
        public byte Byte_Getqv { get; set; } = 0xAF;

        /// <summary>
        /// 设置SN
        /// </summary>
        public byte Byte_SetSn { get; set; } = 0xFA;

        /// <summary>
        /// 读取出厂日期
        /// </summary>
        public byte Byte_GetInitDate { get; set; } = 0xFD;

        /// <summary>
        /// 设置出厂日期
        /// </summary>
        public byte Byte_SetInitDate { get; set; } = 0xFC;

    }
}
