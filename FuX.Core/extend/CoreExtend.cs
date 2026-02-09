using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FuX.Model.data;
using FuX.Model.@enum;
using FuX.Model.Specenum;

namespace FuX.Core.extend
{
    //
    // 摘要:
    //     核心扩展
    public static class CoreExtend
    {
        private static readonly Regex BracketRegex = new Regex("\\[\\s*[^\\[\\]]+\\s*\\]\\s*\\((?>[^()]+|\\((?<Depth>)|\\)(?<-Depth>))*(?(Depth)(?!))\\)", RegexOptions.Compiled);

        private static readonly Regex ParenthesisRegex = new Regex("\\((?>[^()]+|\\((?<Depth>)|\\)(?<-Depth>))*(?(Depth)(?!))\\)", RegexOptions.Compiled);

        //
        // 摘要:
        //     检查地址是否存在无效数据
        //
        // 参数:
        //   address:
        //     地址集合
        //
        // 返回结果:
        //     true:一切正常
        //     false:存在无效数据
        public static bool CheckAddress(this Address address)
        {
            if (address.AddressArray != null && address.AddressArray.Count > 0 && address.AddressArray.Where((AddressDetails c) => string.IsNullOrWhiteSpace(c.AddressName)).Count() == 0)
            {
                return true;
            }

            return false;
        }

        //
        // 摘要:
        //     获取精简返回值集合
        //
        // 参数:
        //   param:
        //     键值
        //
        // 返回结果:
        //     精简地址集合
        public static IEnumerable<AddressValueSimplify>? GetSimplifyArray(this ConcurrentDictionary<string, AddressValue> param)
        {
            if (param.Count == 0)
            {
                return null;
            }

            return param.Values.Select((AddressValue c) => c.GetSimplify());
        }

        //
        // 摘要:
        //     获取精简 Protobuf 数据对象
        //
        // 参数:
        //   param:
        //     键值
        //
        // 返回结果:
        //     精简 Protobuf 数据对象
        public static AddressValueSimplifyProtobuf? GetSimplifyProtobuf(this ConcurrentDictionary<string, AddressValue> param)
        {
            if (param.Count == 0)
            {
                return null;
            }

            return new AddressValueSimplifyProtobuf(param.Values.Select((AddressValue c) => c.GetSimplify()).ToList());
        }

        //
        // 摘要:
        //     查询精简数据集合；
        //     从传入的字典中检索指定的地址信息
        //
        // 参数:
        //   param:
        //     参数
        //
        //   addressNames:
        //     地址名称集合
        //
        // 返回结果:
        //     指定名称的数据对象
        public static IEnumerable<AddressValueSimplify>? SelectSimplifyArray(this ConcurrentDictionary<string, AddressValue> param, List<string> addressNames)
        {
            ConcurrentDictionary<string, AddressValue> param2 = param;
            if (param2.Count == 0)
            {
                return null;
            }

            return addressNames.Select((string c) => param2[c].GetSimplify());
        }

        //
        // 摘要:
        //     查询精简 Protobuf 数据；
        //     从传入的字典中检索指定的地址信息
        //
        // 参数:
        //   param:
        //     参数
        //
        //   addressNames:
        //     地址名称集合
        //
        // 返回结果:
        //     指定名称的数据对象
        public static AddressValueSimplifyProtobuf? SelectSimplifyProtobuf(this ConcurrentDictionary<string, AddressValue> param, List<string> addressNames)
        {
            ConcurrentDictionary<string, AddressValue> param2 = param;
            if (param2.Count == 0)
            {
                return null;
            }

            return new AddressValueSimplifyProtobuf(addressNames.Select((string c) => param2[c].GetSimplify()));
        }

        //
        // 摘要:
        //     获取返回值集合
        //
        // 参数:
        //   param:
        //     键值
        //
        // 返回结果:
        //     精简地址集合
        public static IEnumerable<AddressValue>? GetArray(this ConcurrentDictionary<string, AddressValue> param)
        {
            if (param.Count == 0)
            {
                return null;
            }

            return param.Values.ToArray();
        }

        //
        // 摘要:
        //     查询数据集合；
        //     从传入的字典中检索指定的地址信息
        //
        // 参数:
        //   param:
        //     参数
        //
        //   addressNames:
        //     地址名称集合
        //
        // 返回结果:
        //     精简地址集合
        public static IEnumerable<AddressValue>? SelectArray(this ConcurrentDictionary<string, AddressValue> param, List<string> addressNames)
        {
            ConcurrentDictionary<string, AddressValue> param2 = param;
            if (param2.Count == 0)
            {
                return null;
            }

            return addressNames.Select((string c) => param2[c]);
        }

        //
        // 摘要:
        //     写入获取字节；
        //     支持类型:Int16/UInt16/Int32/UInt32/Int64/UInt64/Double/Single
        //
        // 参数:
        //   value:
        //     值
        //
        // 返回结果:
        //     字节数据
        public static byte[] WriteGetBytes(this object value)
        {
            if (value is short || value is short)
            {
                return BitConverter.GetBytes((short)value);
            }

            if (value is ushort || value is ushort)
            {
                return BitConverter.GetBytes((ushort)value);
            }

            if (value is int || value is int)
            {
                return BitConverter.GetBytes((int)value);
            }

            if (value is uint || value is uint)
            {
                return BitConverter.GetBytes((uint)value);
            }

            if (value is long || value is long)
            {
                return BitConverter.GetBytes((long)value);
            }

            if (value is ulong || value is ulong)
            {
                return BitConverter.GetBytes((ulong)value);
            }

            if (value is double || value is double)
            {
                return BitConverter.GetBytes((double)value);
            }

            if (value is float || value is float)
            {
                return BitConverter.GetBytes((float)value);
            }

            return null;
        }

        //
        // 摘要:
        //     写入获取字节；
        //     目前支持:String
        //
        // 参数:
        //   value:
        //     值
        //
        //   encoding:
        //     编码格式
        //
        // 返回结果:
        //     字节数据
        public static byte[] WriteGetBytes(this string value, Encoding encoding)
        {
            return encoding.GetBytes(value);
        }

        //
        // 摘要:
        //     读取获取对应类型的长度；
        //     支持类型:Int16/UInt16/Int32/UInt32/Int64/UInt64/Double/Single
        //
        // 参数:
        //   dataType:
        //     数据类型
        //
        // 返回结果:
        //     返回对应类型的数据长度
        public static ushort ReadGetLength(this DataType dataType)
        {
            switch (dataType)
            {
                case DataType.Double:
                    return 8;
                case DataType.Float:
                case DataType.Single:
                    return 4;
                case DataType.Int:
                case DataType.Int32:
                    return 4;
                case DataType.Uint:
                case DataType.UInt32:
                    return 4;
                case DataType.Long:
                case DataType.Int64:
                    return 8;
                case DataType.Ulong:
                case DataType.UInt64:
                    return 8;
                case DataType.Short:
                case DataType.Int16:
                    return 2;
                case DataType.Ushort:
                case DataType.UInt16:
                    return 2;
                default:
                    return 0;
            }
        }

        //
        // 摘要:
        //     获取对应的编码格式
        //
        // 参数:
        //   encodingType_AllowEmpty:
        //     编码格式_允许为空
        //
        // 返回结果:
        //     System.Text.Encoding 对应格式的编码类型
        public static Encoding GetEncoding(this EncodingType? encodingType_AllowEmpty)
        {
            if (!encodingType_AllowEmpty.HasValue)
            {
                return Encoding.Default;
            }

            return Encoding.GetEncoding((int)encodingType_AllowEmpty.Value);
        }

        //
        // 摘要:
        //     获取对应的编码格式
        //
        // 参数:
        //   encodingType:
        //     编码格式
        //
        // 返回结果:
        //     System.Text.Encoding 对应格式的编码类型
        public static Encoding GetEncoding(this EncodingType encodingType)
        {
            return ((EncodingType?)encodingType).GetEncoding();
        }

        //
        // 摘要:
        //     获取默认编码的写入
        //
        // 参数:
        //   sources:
        //     写入的数据点，与数据
        //
        //   encodingType:
        //     编码格式
        //
        // 返回结果:
        //     带编码的返回,写入的数据点，数据，编码格式
        public static ConcurrentDictionary<string, (object value, EncodingType? encodingType)> GetDefaultEncodingWrite(this ConcurrentDictionary<string, object> sources, EncodingType? encodingType = EncodingType.ANSI)
        {
            ConcurrentDictionary<string, (object value, EncodingType? encodingType)> targetDict = new ConcurrentDictionary<string, (object, EncodingType?)>();
            Parallel.ForEach<KeyValuePair<string, object>>(sources, delegate (KeyValuePair<string, object> kvp)
            {
                targetDict.TryAdd(kvp.Key, (kvp.Value, encodingType.GetValueOrDefault()));
            });
            return targetDict;
        }

        //
        // 摘要:
        //     消息标准化处理
        //
        // 参数:
        //   input:
        //     输入的数据
        //
        // 返回结果:
        //     标准化后的数据
        public static string MessageStandard(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            int num = input.LastIndexOf(" : ", StringComparison.Ordinal);
            string input2 = ((num > 0) ? input.Substring(0, num) : input);
            string value = ((num > 0) ? input.Substring(num + 3).Trim() : "");
            StringBuilder stringBuilder = new StringBuilder();
            StringBuilder stringBuilder2 = new StringBuilder();
            string text = null;
            MatchCollection matchCollection = BracketRegex.Matches(input2);
            foreach (Match item in matchCollection)
            {
                string value2 = item.Value;
                int num2 = value2.IndexOf(']');
                if (num2 >= 0)
                {
                    if (text == null)
                    {
                        text = value2.Substring(0, num2 + 1);
                    }

                    stringBuilder2.Append(value2.AsSpan(num2 + 1));
                }
            }

            foreach (Match item2 in ParenthesisRegex.Matches(input2))
            {
                bool flag = false;
                foreach (Match item3 in matchCollection)
                {
                    if (item2.Index >= item3.Index && item2.Index < item3.Index + item3.Length)
                    {
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                {
                    stringBuilder2.Append(item2.Value);
                }
            }

            if (text == null)
            {
                return input;
            }

            stringBuilder.Append(text);
            stringBuilder.Append(stringBuilder2);
            if (!string.IsNullOrEmpty(value))
            {
                stringBuilder.Append(" : ").Append(value);
            }

            return stringBuilder.ToString();
        }
    }
}
