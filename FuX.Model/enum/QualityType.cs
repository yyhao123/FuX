using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.@enum
{
    public enum QualityType
    {
        [Description("尚未经过处理")]
        None = -1,
        [Description("异常")]
        Exception,
        [Description("正常")]
        Normal,
        [Description("数据类型错误")]
        DataTypeError,
        [Description("数据经过解析，并且解析成功，无法得知数据的正确性")]
        Unknown,
        [Description("解析错误")]
        ParseError
    }

}
