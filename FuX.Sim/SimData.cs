using FuX.Core.subscription;
using FuX.Unility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Sim
{
    public class SimData
    {
        public class Basics : SubscribeData.SCData
        {
            [Category("基础数据")]
            [Description("唯一标识符")]
            public string? SN { get; set; } = Guid.NewGuid().ToUpperNString();

        }
    }
}
