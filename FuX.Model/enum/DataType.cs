using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.Specenum
{
    //
    // 摘要:
    //     数据类型
    public enum DataType
    {
        //
        // 摘要:
        //     未定义
        [Description("未定义")]
        None,
        //
        // 摘要:
        //     布尔值；
        //     1字节8位
        [Description("布尔值")]
        Bool,
        //
        // 摘要:
        //     双精度浮点数；
        //     8字节64位
        [Description("双精度浮点数")]
        Double,
        //
        // 摘要:
        //     单精度浮点数；
        //     4字节32位
        [Description("单精度浮点数")]
        Float,
        //
        // 摘要:
        //     单精度浮点数；
        //     4字节32位
        [Description("单精度浮点数")]
        Single,
        //
        // 摘要:
        //     有符号16位整数；
        //     2字节16位
        [Description("有符号16位整数")]
        Short,
        //
        // 摘要:
        //     有符号16位整数； 2字节16位
        [Description("有符号16位整数")]
        Int16,
        //
        // 摘要:
        //     无符号16位整数；
        //     2字节16位
        [Description("无符号16位整数")]
        Ushort,
        //
        // 摘要:
        //     无符号16位整数；
        //     2字节16位
        [Description("无符号16位整数")]
        UInt16,
        //
        // 摘要:
        //     有符号32位整数；
        //     4字节32位
        [Description("有符号32位整数")]
        Int,
        //
        // 摘要:
        //     有符号32位整数；
        //     4字节32位
        [Description("有符号32位整数")]
        Int32,
        //
        // 摘要:
        //     无符号32位整数；
        //     4字节32位
        [Description("无符号32位整数")]
        Uint,
        //
        // 摘要:
        //     无符号32位整数；
        //     4字节32位
        [Description("无符号32位整数")]
        UInt32,
        //
        // 摘要:
        //     有符号64位整数；
        //     8字节64位
        [Description("有符号64位整数")]
        Long,
        //
        // 摘要:
        //     有符号64位整数；
        //     8字节64位
        [Description("有符号64位整数")]
        Int64,
        //
        // 摘要:
        //     无符号64位整数；
        //     8字节64位
        [Description("无符号64位整数")]
        Ulong,
        //
        // 摘要:
        //     无符号64位整数；
        //     8字节64位
        [Description("无符号64位整数")]
        UInt64,
        //
        // 摘要:
        //     日期和时间；
        //     8字节64位；
        //     不支持读取写入
        [Description("日期和时间")]
        DateTime,
        //
        // 摘要:
        //     日期；
        //     不支持读取写入
        [Description("日期")]
        Date,
        //
        // 摘要:
        //     时间；
        //     不支持读取写入
        [Description("时间")]
        Time,
        //
        // 摘要:
        //     字符串；
        //     2字节16位
        [Description("字符串")]
        String,
        //
        // 摘要:
        //     Unicode字符；
        //     2字节16位
        [Description("Unicode字符")]
        Char,
        //
        // 摘要:
        //     字节数组；
        //     当在 WebApi 写入时，请以 Byte[] 的 string 表达方式写入 0x00 0x01 0x02 0x03
        //     不支持虚拟地址
        [Description("字节数组")]
        ByteArray,
        //
        // 摘要:
        //     布尔数组值；
        //     [true,false,true,false]
        //     不支持虚拟地址
        [Description("布尔数组值")]
        BoolArray,
        //
        // 摘要:
        //     双精度浮点数组值；
        //     [1.1,2.1,3.1,4.1,5.1,6.1]
        //     不支持虚拟地址
        [Description("双精度浮点数组")]
        DoubleArray,
        //
        // 摘要:
        //     单精度浮点数组；
        //     [1.1,2.1,3.1,4.1,5.1,6.1]
        //     不支持虚拟地址
        [Description("单精度浮点数组")]
        FloatArray,
        //
        // 摘要:
        //     单精度浮点数组；
        //     [1.1,2.1,3.1,4.1,5.1,6.1]
        //     不支持虚拟地址
        [Description("单精度浮点数组")]
        SingleArray,
        //
        // 摘要:
        //     有符号16位整数组；
        //     [1,2,3,4,5,6]
        //     不支持虚拟地址
        [Description("有符号16位整数组")]
        ShortArray,
        //
        // 摘要:
        //     有符号16位整数组；
        //     [1,2,3,4,5,6]
        //     不支持虚拟地址
        [Description("有符号16位整数组")]
        Int16Array,
        //
        // 摘要:
        //     无符号16位整数组；
        //     [1,2,3,4,5,6]
        //     不支持虚拟地址
        [Description("无符号16位整数组")]
        UshortArray,
        //
        // 摘要:
        //     无符号16位整数组；
        //     [1,2,3,4,5,6]
        //     不支持虚拟地址
        [Description("无符号16位整数组")]
        UInt16Array,
        //
        // 摘要:
        //     有符号32位整数组；
        //     [1,2,3,4,5,6]
        //     不支持虚拟地址
        [Description("有符号32位整数组")]
        IntArray,
        //
        // 摘要:
        //     有符号32位整数组；
        //     [1,2,3,4,5,6]
        //     不支持虚拟地址
        [Description("有符号32位整数组")]
        Int32Array,
        //
        // 摘要:
        //     无符号32位整数组；
        //     [1,2,3,4,5,6]
        //     不支持虚拟地址
        [Description("无符号32位整数组")]
        UintArray,
        //
        // 摘要:
        //     无符号32位整数组；
        //     [1,2,3,4,5,6]
        //     不支持虚拟地址
        [Description("无符号32位整数组")]
        UInt32Array,
        //
        // 摘要:
        //     有符号64位整数组；
        //     [1,2,3,4,5,6]
        //     不支持虚拟地址
        [Description("有符号64位整数组")]
        LongArray,
        //
        // 摘要:
        //     有符号64位整数组；
        //     [1,2,3,4,5,6]
        //     不支持虚拟地址
        [Description("有符号64位整数组")]
        Int64Array,
        //
        // 摘要:
        //     无符号64位整数组；
        //     [1,2,3,4,5,6]
        //     不支持虚拟地址
        [Description("无符号64位整数组")]
        UlongArray,
        //
        // 摘要:
        //     无符号64位整数组；
        //     [1,2,3,4,5,6]
        //     不支持虚拟地址
        [Description("无符号64位整数组")]
        UInt64Array
    }
}
