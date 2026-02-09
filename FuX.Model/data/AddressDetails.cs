using FuX.Model.@enum;
using FuX.Model.Specenum;
using FuX.Unility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FuX.Model.data
{
    //
    // 摘要:
    //     单个地址的详细信息
    public class AddressDetails
    {
        //
        // 摘要:
        //     可以理解成唯一标识符（可以存机台号、组名、车间、厂）；
        //     默认为随机guid
        public string? SN { get; set; } = Guid.NewGuid().ToUpperNString();


        //
        // 摘要:
        //     唯一；
        //     实际的点位地址名称；
        //     是扩展数据时，可为空；
        //     不是扩展数据时，此项为PLC的地址
        public string? AddressName { get; set; }

        //
        // 摘要:
        //     长度，默认值 1；
        //     AddressDataType 存在 Array 类型时，根据实际情况修改此参数；
        //     读取对应的 Array 类型长度
        public ushort Length { get; set; } = 1;


        //
        // 摘要:
        //     编码类型
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EncodingType EncodingType { get; set; }

        //
        // 摘要:
        //     地址别名
        public string? AddressAnotherName { get; set; }

        //
        // 摘要:
        //     地址属性名称（与定义的实体名称对应）；
        //     是扩展数据时，可为空；
        //     不是扩展数据时，此项为PLC的最后字段名
        public string? AddressPropertyName { get; set; }

        //
        // 摘要:
        //     描述
        public string? AddressDescribe { get; set; }

        //
        // 摘要:
        //     指定通信的扩展数据
        public object? AddressExtendParam { get; set; }

        //
        // 摘要:
        //     是否启用
        public bool IsEnable { get; set; } = true;


        //
        // 摘要:
        //     地址消息中间件参数
        //     通过指定的消息中间件把数据发送出去
        public AddressMq? AddressMqParam { get; set; }

        //
        // 摘要:
        //     数据解析参数
        public AddressParse? AddressParseParam { get; set; }

        //
        // 摘要:
        //     数据类型
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DataType AddressDataType { get; set; } = DataType.String;


        //
        // 摘要:
        //     地址类型，默认实际地址
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AddressType AddressType { get; set; }

        //
        // 摘要:
        //     有参构造函数
        //
        // 参数:
        //   sn:
        //     唯一标识
        //
        //   addressName:
        //     唯一；
        //     实际的点位地址名称；
        //     是扩展数据时，可为空；
        //     不是扩展数据时，此项为PLC的地址
        //
        //   addressAnotherName:
        //     地址别名
        //
        //   addressPropertyName:
        //     地址属性名
        //
        //   addressDescribe:
        //     地址描述
        //
        //   addressExtendParam:
        //     地址扩展参数
        //
        //   addressMqParam:
        //     地址消息中间件参数
        //
        //   addressParseParam:
        //     解析参数
        //
        //   addressDataType:
        //     数据类型
        //
        //   addressType:
        //     地址类型
        //
        //   isEnable:
        //     是否启用
        //
        //   length:
        //     数组长度，默认值 1；
        //     AddressDataType 存在 Array 类型时，根据实际情况修改此参数；
        //     读取对应的 Array 类型长度
        //
        //   encodingType:
        //     编码类型
        public AddressDetails(string sn, string addressName, string addressAnotherName, string addressPropertyName, string addressDescribe, object addressExtendParam, AddressMq addressMqParam, AddressParse addressParseParam, DataType addressDataType, AddressType addressType = AddressType.Reality, bool isEnable = true, ushort length = 1, EncodingType? encodingType = EncodingType.ANSI)
        {
            SN = sn;
            AddressName = addressName;
            Length = length;
            AddressAnotherName = addressAnotherName;
            AddressPropertyName = addressPropertyName;
            AddressDescribe = addressDescribe;
            AddressExtendParam = addressExtendParam;
            IsEnable = isEnable;
            AddressMqParam = addressMqParam;
            AddressParseParam = addressParseParam;
            AddressDataType = addressDataType;
            AddressType = addressType;
            EncodingType = encodingType.GetValueOrDefault();
        }

        //
        // 摘要:
        //     有参构造函数
        //
        // 参数:
        //   sn:
        //     唯一标识
        //
        //   addressName:
        //     唯一；
        //     实际的点位地址名称；
        //     是扩展数据时，可为空；
        //     不是扩展数据时，此项为PLC的地址
        //
        //   addressDataType:
        //     数据类型
        //
        //   length:
        //     数组长度，默认值 1；
        //     AddressDataType 存在 Array 类型时，根据实际情况修改此参数；
        //     读取对应的 Array 类型长度
        //
        //   encodingType:
        //     编码类型
        public AddressDetails(string sn, string addressName, DataType addressDataType, ushort length = 1, EncodingType? encodingType = EncodingType.ANSI)
        {
            SN = sn;
            AddressName = addressName;
            AddressDataType = addressDataType;
            Length = length;
            EncodingType = encodingType.GetValueOrDefault();
        }

        //
        // 摘要:
        //     有参构造函数
        //
        // 参数:
        //   addressName:
        //     唯一；
        //     实际的点位地址名称；
        //     是扩展数据时，可为空；
        //     不是扩展数据时，此项为PLC的地址
        //
        //   addressDataType:
        //     数据类型
        //
        //   length:
        //     数组长度，默认值 1；
        //     AddressDataType 存在 Array 类型时，根据实际情况修改此参数；
        //     读取对应的 Array 类型长度
        //
        //   encodingType:
        //     编码类型
        public AddressDetails(string addressName, DataType addressDataType, ushort length = 1, EncodingType? encodingType = EncodingType.ANSI)
        {
            AddressName = addressName;
            AddressDataType = addressDataType;
            Length = length;
            EncodingType = encodingType.GetValueOrDefault();
        }

        //
        // 摘要:
        //     无参构造函数
        public AddressDetails()
        {
        }

        //
        // 摘要:
        //     重写ToString；
        //     响应 json 字符串
        //
        // 返回结果:
        //     json 字符串
        public override string ToString()
        {
            return this.ToJson(formatting: true) ?? string.Empty;
        }

        //
        // 摘要:
        //     重写Equals
        //
        // 参数:
        //   o:
        //     对象
        //
        // 返回结果:
        //     是否一致
        public override bool Equals(object? o)
        {
            if (o == null)
            {
                return false;
            }

            return this.Comparer(o as AddressDetails).result;
        }
    }
}
