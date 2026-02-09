using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.@enum
{
    public enum FuncUse
    {
        [Description("禁用")]
        Disable,
        [Description("激活")]
        Activation
    }
}
