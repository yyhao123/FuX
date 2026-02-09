using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.@enum
{
    //
    // 摘要:
    //     编码类型
    public enum EncodingType
    {
        //
        // 摘要:
        //     UTF-16
        [Description("UTF-16")]
        Unicode = 1200,
        //
        // 摘要:
        //     UTF-16
        [Description("UTF-16")]
        BigEndianUnicode = 1201,
        //
        // 摘要:
        //     UTF-32
        [Description("UTF-32")]
        UTF32 = 12000,
        //
        // 摘要:
        //     UTF-32
        [Description("UTF-32")]
        BigEndianUTF32 = 12001,
        //
        // 摘要:
        //     UTF-8
        [Description("UTF-8")]
        UTF8 = 65001,
        //
        // 摘要:
        //     ASCII
        [Description("ASCII")]
        ASCII = 20127,
        //
        // 摘要:
        //     GB2312
        [Description("GB2312")]
        GB2312 = 936,
        //
        // 摘要:
        //     ANSI
        [Description("ANSI")]
        ANSI = 0
    }
}
