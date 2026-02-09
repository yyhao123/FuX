using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.attribute
{
    //
    // 摘要:
    //     单位特性
    [AttributeUsage(AttributeTargets.All)]
    public class UnitAttribute : Attribute
    {
        //
        // 摘要:
        //     单位
        public string Unit { get; set; }

        //
        // 摘要:
        //     构造函数
        //
        // 参数:
        //   Unit:
        //     单位
        public UnitAttribute(string Unit)
        {
            this.Unit = Unit;
        }
    }
}
