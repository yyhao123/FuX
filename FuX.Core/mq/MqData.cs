using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Core.mq
{
    public class MqData
    {
        public class Basics
        {
            public string? LibFolder { get; set; } = AppDomain.CurrentDomain.BaseDirectory + "lib\\mq";


            public string? LibConfigFolder { get; set; } = AppDomain.CurrentDomain.BaseDirectory + "config\\mq";


            public string? LibConfigSNKey { get; set; } = "SN";


            public string? DllWatcherFormat { get; set; } = "FuX.*.dll";


            public string? ConfigWatcherFormat { get; set; } = "*.Mq.Config.json";


            public string? ConfigFileNameFormat { get; set; } = "{0}.*.Mq.Config.json";


            public string? ConfigReplaceFormat { get; set; } = ".Mq.Config.json";


            public string? InterfaceFullName { get; set; } = "FuX.Model.interface.IMq";


            public bool AutoOn { get; set; } = true;


            public int TaskNumber { get; set; } = 5;

        }
    }
}
