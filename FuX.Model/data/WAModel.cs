using FuX.Unility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.data
{
    public class WAModel
    {
        [Description("Ip地址")]
        public string IpAddress { get; set; } = "127.0.0.1";


        [Description("端口")]
        public int Port { get; set; } = 6688;


        [Description("跨域")]
        public bool CrossDomain { get; set; }

        public WAModel()
        {
        }

        public WAModel(string ipAddress, int port, bool crossDomain = false)
        {
            IpAddress = ipAddress;
            Port = port;
            CrossDomain = crossDomain;
        }

        public override string ToString()
        {
            return this.ToJson(formatting: true) ?? string.Empty;
        }
    }
}
