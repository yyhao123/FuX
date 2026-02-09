using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.attribute
{
    //
    // 摘要:
    //     自动分割属性特性；
    //     用于协议类型枚举中使用；
    //     快速检索出对应的属性列表；
    //     配合 AutoAllocatingTagAttribute 使用
    [AttributeUsage(AttributeTargets.All)]
    public class AutoAllocatingAttribute : Attribute
    {
        //
        // 摘要:
        //     属性名称集合
        public string[] PropertyNameArray { get; set; }

        //
        // 摘要:
        //     下标集合特性；
        //     用于协议类型枚举中使用；
        //     快速检索出对应的属性列表；
        //     配合 AutoAllocatingTagAttribute 使用
        //
        // 参数:
        //   propertyNameArray:
        //     属性名称
        public AutoAllocatingAttribute(string[] propertyNameArray)
        {
            PropertyNameArray = propertyNameArray;
        }
    }
}
