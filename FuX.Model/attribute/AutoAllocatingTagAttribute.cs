using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.attribute
{
    //
    // 摘要:
    //     自动分配标识特性，标识此属性是用于自动分配属性使用；
    //     在当前类中只能出现一个这样的标识；
    //     用于协议类型枚举中使用；
    //     快速检索出对应的属性列表；
    //     配合 AutoAllocatingAttribute 使用
    [AttributeUsage(AttributeTargets.All)]
    public class AutoAllocatingTagAttribute : Attribute
    {
        //
        // 摘要:
        //     枚举的类型
        public Type EnumType { get; set; }

        //
        // 摘要:
        //     自动分配标识特性，标识此属性是用于自动分配属性使用；
        //     在当前类中只能出现一个这样的标识；
        //     用于协议类型枚举中使用；
        //     快速检索出对应的属性列表；
        //     配合 AutoAllocatingAttribute 使用
        //
        // 参数:
        //   enumType:
        //     枚举的类型
        public AutoAllocatingTagAttribute(Type enumType)
        {
            EnumType = enumType;
        }
    }
}
