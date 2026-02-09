using Demo.Model.entities;
using Demo.Model.@enum;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    public class Device
    {

        /// <summary>
        /// Device Serial Number
        /// </summary>
        public string SN { get; set; } = "";

        /// <summary>
        /// Device Name
        /// </summary>
        public string DevName { get; set; } = "";

        /// <summary>
        /// device state
        /// </summary>
        public DeviceState DeviceStates { get; set; } = DeviceState.Idle;


        public Dictionary<string, List<double>> WelMappingPoint = new Dictionary<string, List<double>>();


        public static List<DevListShow> DevComList = new List<DevListShow>();


        public static List<SerialPort> openSerialPorts = new List<SerialPort>();

        /// <summary>
        /// 拉曼位移
        /// </summary>
        public DeviceRamanShift DeviceRamanShift { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public Spectrum Spectrum { get; set; }

        /// <summary>
        /// 快速采集暗底数据
        /// </summary>
        public SpectrumDataDark SpectrumDataDark { get; set; }

        /// <summary>
        /// 标准表值
        /// </summary>
        public SpectrumDataRaw SpectrumDataRaw { get; set; }

        /// <summary>
        /// 样品表值
        /// </summary>
        public SpectrumDataWhiteBoard SpectrumDataSample { get; set; }

        /// <summary>
        /// 选择的CCD信息
        /// </summary>
        public CCDInfo CCDInfo { get; set; }


    }
}
