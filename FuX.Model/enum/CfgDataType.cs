using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.Specenum
{
    /// <summary>
    /// 配置存储类型
    /// </summary>
    public enum CfgDataType
    {

        /// <summary>
        /// 字符串
        /// </summary>
        [Description("字符串")]
        String = 0,

        /// <summary>
        /// int型
        /// </summary>
        [Description("int型")]
        Int = 1,

        /// <summary>
        /// double类型
        /// </summary>
        [Description("double类型")]
        Double = 2,

        /// <summary>
        /// int集合
        /// </summary>
        [Description("int集合")]
        IntList = 3,


        /// <summary>
        /// bool类型
        /// </summary>
        [Description("bool类型")]
        Bool = 4,

        /// <summary>
        /// 枚举类型
        /// </summary>
        [Description("枚举类型")]
        Enumerate = 5,

        /// <summary>
        /// double集合
        /// </summary>
        [Description("double集合")]
        DoubleList = 6,

        /// <summary>
        /// 数据对象
        /// </summary>
        [Description("数据对象")]
        DataObject = 7,


        /// <summary>
        /// 字符串集合
        /// </summary>
        [Description("字符串集合")]
        StrList = 8,

        /// <summary>
        /// 数据对象集合
        /// </summary>
        [Description("数据对象集合")]
        DataObjectList = 9,

    }

    /// <summary>
    /// 是否有效
    /// </summary>
    public enum Iseffective
    {
        /// <summary>
        /// 是
        /// </summary>
        [Description("是")]
        OK = 0,

        /// <summary>
        /// 否
        /// </summary>
        [Description("否")]
        NO = 1,
        /// <summary>
        /// 全部
        /// </summary>
        [Description("全部")]
        ALL = 2
    }

    /// <summary>
    /// 返回数据类型
    /// </summary>
    public enum CmdRetDataType
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = -1,

        /// <summary>
        /// 浮点型
        /// </summary>
        [Description("浮点型")]
        Float = 0,

        /// <summary>
        /// int型
        /// </summary>
        [Description("int型")]
        Int = 1,

        /// <summary>
        /// 浮点集合
        /// </summary>
        [Description("浮点集合")]
        FloatList = 2,

        /// <summary>
        /// int集合
        /// </summary>
        [Description("int集合")]
        IntList = 3,

        /// <summary>
        /// 编码字符
        /// </summary>
        [Description("编码字符")]
        CodingStr = 4,

        /// <summary>
        /// 字符串
        /// </summary>
        [Description("字符串")]
        String = 5,

        /// <summary>
        /// bool类型
        /// </summary>
        [Description("bool类型")]
        Bool = 6,

        /// <summary>
        /// 原始数据位
        /// </summary>
        [Description("原始数据位")]
        Data = 7,

        /// <summary>
        /// 电机移动
        /// </summary>
        [Description("电机移动")]
        Motor = 8,

        /// <summary>
        /// ATPCCD
        /// </summary>
        [Description("ATPCCD")]
        ATPCCD = 9,

    }

    /// <summary>
    /// 输入数据类型
    /// </summary>
    public enum CmdInDataType
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = -1,

        /// <summary>
        /// 浮点型
        /// </summary>
        [Description("浮点型")]
        Float = 0,

        /// <summary>
        /// int型
        /// </summary>
        [Description("int型")]
        Int = 1,

        /// <summary>
        /// 编码字符
        /// </summary>
        [Description("编码字符")]
        CodingStr = 2,

        /// <summary>
        /// 字符串
        /// </summary>
        [Description("字符串")]
        String = 3,

        /// <summary>
        /// Byte集合
        /// </summary>
        [Description("Byte集合")]
        ByteList = 4,

        /// <summary>
        /// Int数据集合
        /// </summary>
        [Description("Int数据集合")]
        ListInt = 5,

        /// <summary>
        /// 浮点数据集合
        /// </summary>
        [Description("浮点数据集合")]
        ListFlt = 6,
    }
}
