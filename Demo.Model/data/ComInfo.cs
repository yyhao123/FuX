using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    public class ComInfo
    {
        public ushort VID { get; set; }

        public ushort PID { get; set; }

        public string COM { get; set; }

        public string Display { get; set; }

        public string DevName { get; set; }

        public string SN { get; set; }

        public string QV { get; set; }

        public string MFDate { get; set; }

        public int DeviceType { get; set; }
    }
}
