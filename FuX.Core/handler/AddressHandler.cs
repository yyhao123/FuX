using FuX.Core.extend;
using FuX.Core.mq;
using FuX.Core.reflection;
using FuX.Core.script;
using FuX.Log;
using FuX.Model.data;
using FuX.Model.@enum;
using FuX.Model.Specenum;
using FuX.Unility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Core.handler
{

    public static class AddressHandler
    {
        private static readonly string ExecuteDisposeLog = "Core/AddressHandler/ExecuteDispose.log";

        private static readonly string ParseLog = "Core/AddressHandler/Parse.log";

        private static readonly string ProduceLog = "Core/AddressHandler/Produce.log";

        private static MqOperate mqOperate = CoreUnify<MqOperate, MqData.Basics>.Instance(new MqData.Basics());

        private static ScriptOperate scriptOperate = CoreUnify<ScriptOperate, ScriptData.Basics>.Instance(new ScriptData.Basics());

        private static ConcurrentDictionary<ReflectionData.Basics, ReflectionOperate> ReflectionIoc = new ConcurrentDictionary<ReflectionData.Basics, ReflectionOperate>();

        public static AddressValue? ExecuteDispose(this AddressDetails addressDetails, object? value, string? message)
        {
            try
            {
                object originalValue = null;
                object obj = null;
                QualityType qualityType = QualityType.None;
                if (value == null || string.IsNullOrWhiteSpace(value?.ToString()))
                {
                    qualityType = QualityType.Exception;
                }
                else
                {
                    qualityType = DataTypeConvert(addressDetails.AddressDataType, value.ToJson(), out object outValue, out string message2);
                    if (qualityType == QualityType.Normal)
                    {
                        originalValue = outValue;
                        obj = outValue;
                        Parse(addressDetails, obj, out (QualityType, object, string) result);
                        obj = result.Item2;
                        (qualityType, _, _) = result;
                        if (!string.IsNullOrWhiteSpace(result.Item3))
                        {
                            message = result.Item3;
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(message2))
                    {
                        message = message2;
                    }
                }
                AddressValue addressValue = new AddressValue().SET(addressDetails);
                addressValue.ResultValue = obj;
                addressValue.OriginalValue = originalValue;
                addressValue.Quality = qualityType;
                addressValue.Message = message;
                if (qualityType == QualityType.Normal || qualityType == QualityType.Unknown)
                {
                    Produce(addressDetails, obj);
                }
                return addressValue;
            }
            catch (Exception ex)
            {
                LogHelper.Error("执行数据处理异常".GetLanguageValue() + "：" + ex.Message, ExecuteDisposeLog);
            }
            return null;
        }

        public static AddressValue? ExecuteSpecialDispose(this AddressDetails addressDetails, string? value, string? message)
        {
            object outValue;
            string message2;
            QualityType qualityType = DataTypeConvert(addressDetails.AddressDataType, value, out outValue, out message2);
            if (qualityType == QualityType.Normal)
            {
                return addressDetails.ExecuteDispose(outValue, message);
            }
            AddressValue addressValue = new AddressValue().SET(addressDetails);
            addressValue.ResultValue = value;
            addressValue.OriginalValue = value;
            addressValue.Quality = qualityType;
            addressValue.Message = message2;
            return addressValue;
        }

        private static QualityType DataTypeConvert(DataType dataType, string value, out object? outValue, out string? message)
        {
            QualityType result = QualityType.Normal;
            outValue = null;
            message = string.Empty;
            try
            {
                switch (dataType)
                {
                    case DataType.Bool:
                        outValue = Convert.ToBoolean(value);
                        return result;
                    case DataType.String:
                        outValue = Convert.ToString(value);
                        return result;
                    case DataType.Char:
                        outValue = Convert.ToChar(value);
                        return result;
                    case DataType.Double:
                        outValue = Convert.ToDouble(value);
                        return result;
                    case DataType.Float:
                    case DataType.Single:
                        outValue = Convert.ToSingle(value);
                        return result;
                    case DataType.Short:
                    case DataType.Int16:
                        outValue = Convert.ToInt16(value);
                        return result;
                    case DataType.Ushort:
                    case DataType.UInt16:
                        outValue = Convert.ToUInt16(value);
                        return result;
                    case DataType.Int:
                    case DataType.Int32:
                        outValue = Convert.ToInt32(value);
                        return result;
                    case DataType.Uint:
                    case DataType.UInt32:
                        outValue = Convert.ToUInt32(value);
                        return result;
                    case DataType.Long:
                    case DataType.Int64:
                        outValue = Convert.ToInt64(value);
                        return result;
                    case DataType.Ulong:
                    case DataType.UInt64:
                        outValue = Convert.ToUInt64(value);
                        return result;
                    case DataType.DateTime:
                    case DataType.Date:
                    case DataType.Time:
                        outValue = Convert.ToDateTime(value);
                        return result;
                    case DataType.ByteArray:
                        outValue = value.ToJsonEntity<byte[]>();
                        return result;
                    case DataType.BoolArray:
                        outValue = value.ToJsonEntity<bool[]>();
                        return result;
                    case DataType.DoubleArray:
                        outValue = value.ToJsonEntity<double[]>();
                        return result;
                    case DataType.FloatArray:
                    case DataType.SingleArray:
                        outValue = value.ToJsonEntity<float[]>();
                        return result;
                    case DataType.ShortArray:
                    case DataType.Int16Array:
                        outValue = value.ToJsonEntity<short[]>();
                        return result;
                    case DataType.UshortArray:
                    case DataType.UInt16Array:
                        outValue = value.ToJsonEntity<ushort[]>();
                        return result;
                    case DataType.IntArray:
                    case DataType.Int32Array:
                        outValue = value.ToJsonEntity<int[]>();
                        return result;
                    case DataType.UintArray:
                    case DataType.UInt32Array:
                        outValue = value.ToJsonEntity<uint[]>();
                        return result;
                    case DataType.LongArray:
                    case DataType.Int64Array:
                        outValue = value.ToJsonEntity<long[]>();
                        return result;
                    case DataType.UlongArray:
                    case DataType.UInt64Array:
                        outValue = value.ToJsonEntity<ulong[]>();
                        return result;
                    case DataType.None:
                        outValue = value;
                        return result;
                    default:
                        return result;
                }
            }
            catch
            {
                result = QualityType.DataTypeError;
                message = $"[ {value} ][ {dataType.ToString()} ]{"类型错误".GetLanguageValue()}";
                return result;
            }
        }

        private static void Produce(AddressDetails addressDetails, object value)
        {
            if (addressDetails.AddressMqParam != null)
            {
                if (!string.IsNullOrWhiteSpace(addressDetails.AddressMqParam.ContentFormat) && addressDetails.AddressMqParam.ContentFormat.Contains("{0}"))
                {
                    value = string.Format(addressDetails.AddressMqParam.ContentFormat, value);
                }
                if (!mqOperate.Produce(addressDetails.AddressMqParam.Topic, value.ToString(), addressDetails.AddressMqParam.ISns).Status)
                {
                    LogHelper.Error("{" + $"\"Topic\":\"{addressDetails.AddressMqParam?.Topic}\",\"Content\":\"{value}\",\"ISns\":{addressDetails.AddressMqParam?.ISns?.ToJson()}" + "}", ProduceLog, null, consoleShow: false);
                }
            }
        }

        private static void Parse(AddressDetails addressDetails, object value, out (QualityType Q, object? V, string? M) result)
        {
            string text = string.Empty;
            object obj = null;
            if (addressDetails.AddressParseParam != null)
            {
                try
                {
                    if (addressDetails.AddressParseParam?.ReflectionParam != null)
                    {
                        AddressParse? addressParseParam = addressDetails.AddressParseParam;
                        ReflectionData.Basics basics = ((addressParseParam != null) ? addressParseParam.ReflectionParam[0] : null) as ReflectionData.Basics;
                        AddressParse? addressParseParam2 = addressDetails.AddressParseParam;
                        string text2 = ((addressParseParam2 != null) ? addressParseParam2.ReflectionParam[1].ToString() : null);
                        if (basics != null && text2 != null)
                        {
                            if (!ReflectionIoc.ContainsKey(basics))
                            {
                                ReflectionOperate operate = CoreUnify<ReflectionOperate, ReflectionData.Basics>.Instance(basics);
                                ReflectionIoc.AddOrUpdate(basics, operate, (ReflectionData.Basics k, ReflectionOperate v) => operate);
                                OperateResult operateResult = ReflectionIoc[basics].Init();
                                if (!operateResult.Status)
                                {
                                    ReflectionIoc.Remove(basics, out var _);
                                    text = "反射初始化失败：" + operateResult.Message;
                                    LogHelper.Error(text, ParseLog);
                                }
                            }
                            if (ReflectionIoc.ContainsKey(basics))
                            {
                                obj = ReflectionIoc[basics].ExecuteMethod(text2, new object[2] { addressDetails.AddressName, value })?.ToString();
                            }
                        }
                        else
                        {
                            text = "使用了“反射”解析，但参数错误";
                            LogHelper.Error(text, ParseLog);
                        }
                    }
                    if (addressDetails.AddressParseParam?.ScriptParam != null)
                    {
                        if (addressDetails.AddressParseParam?.ScriptParam is ScriptData.Basics basics2)
                        {
                            OperateResult operateResult2 = scriptOperate.Execute(basics2.ScriptType, basics2.ScriptCode, basics2.ScriptFunction, new object[2] { addressDetails.AddressName, value });
                            if (operateResult2.Status)
                            {
                                obj = operateResult2.ResultData?.ToString();
                            }
                            else
                            {
                                text = "脚本执行异常：" + operateResult2.Message;
                                LogHelper.Error(text, ParseLog);
                            }
                        }
                        else
                        {
                            text = "使用了“脚本”解析，但参数错误";
                            LogHelper.Error(text, ParseLog);
                        }
                    }
                    if (obj == null || string.IsNullOrWhiteSpace(obj.ToString()))
                    {
                        result = (QualityType.ParseError, null, text);
                    }
                    else
                    {
                        result = (QualityType.Unknown, obj, text);
                    }
                    return;
                }
                catch (Exception ex)
                {
                    text = "解析异常".GetLanguageValue() + "：" + ex.Message;
                    result = (QualityType.ParseError, null, text);
                    LogHelper.Error(text, ParseLog, ex, consoleShow: false);
                }
            }
            result = (QualityType.Normal, value, text);
        }
    }
}
