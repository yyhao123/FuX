using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Driver.serial
{
    public class SerialInfo
    {
        public string Name { get; set; }

        public bool IsOpen { get; set; }

        public int BaudRate { get; set; }

        public int DataBits { get; set; }

        public Parity Parity { get; set; }

        public StopBits StopBits { get; set; }

        public int ReadTimeOut { get; set; }

        public int ReceivedBytesThreshold { get; set; }

        public void UpdatePort(SerialPort port)
        {
            port.BaudRate = BaudRate;
            port.DataBits = DataBits;
            port.Parity = Parity;
            port.PortName = Name;
            port.StopBits = StopBits;
            port.ReadTimeout = ReadTimeOut;
            port.ReceivedBytesThreshold = ReceivedBytesThreshold;
        }
    }
}
