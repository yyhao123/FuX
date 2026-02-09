using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    public class DeviceCom
    {


        public static List<DevListShow> DevComList = new List<DevListShow>();


        public static List<SerialPort> openSerialPorts = new List<SerialPort>();

        /// <summary>
        /// 设备连接的COM
        /// </summary>
        public static string COM { get; set; }


    }
}
