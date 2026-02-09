using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.@enum
{
    public enum ConnectType
    {
        [Description("USB")]
        USB,
        [Description("COM")]
        COM
    }
}
