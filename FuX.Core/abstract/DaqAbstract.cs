using FuX.Core.Communication.net.http.service;
using FuX.Core.extend;
using FuX.Model.data;
using FuX.Model.@enum;
using FuX.Model.@interface;
using FuX.Model.Specenum;
using FuX.Unility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Core.@abstract
{
    public abstract class DaqAbstract<O, D> : CoreUnify<O, D>, IDaq, IOn, IOff, IRead, IWrite, ISubscribe, IEvent, IGetStatus, IGetParam, ICreateInstance, ILog, IWA, IGetObject, ILanguage, IDisposable where O : class where D : class
    {
        private HttpServiceOperate? WebApi;

        private readonly string on = "/api/on";

        private readonly string off = "/api/off";

        private readonly string read = "/api/read";

        private readonly string write = "/api/write";

        private readonly string getstatus = "/api/getstatus";

        private readonly string switchlanguage = "/api/switchlanguage";

        protected DaqAbstract()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        protected DaqAbstract(D param)
            : base(param)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        private void WebApi_OnDataEvent(object? sender, EventDataResult e)
        {
            HttpServiceData.WaitHandler source = e.GetSource<HttpServiceData.WaitHandler>();
            try
            {
                if (source == null)
                {
                    return;
                }
                string absolutePath = source.Request.Url.AbsolutePath;
                if (absolutePath.Equals(on))
                {
                    WebApi?.Write(source.Response, (object)On());
                }
                if (absolutePath.Equals(off))
                {
                    WebApi?.Write(source.Response, (object)Off());
                }
                if (absolutePath.Equals(read))
                {
                    Address address = source.BodyData?.ToJsonEntity<Address>();
                    if (address == null)
                    {
                        WebApi?.Write(source.Response, (object)OperateResult.CreateFailureResult("[ " + absolutePath + " ] 接口请求参数错误"));
                        return;
                    }
                    WebApi?.Write(source.Response, (object)Read(address));
                }
                if (absolutePath.Equals(write))
                {
                    ConcurrentDictionary<string, WriteModel> concurrentDictionary = source.BodyData?.ToJsonEntity<ConcurrentDictionary<string, WriteModel>>();
                    if (concurrentDictionary == null)
                    {
                        WebApi?.Write(source.Response, (object)OperateResult.CreateFailureResult("[ " + absolutePath + " ] 接口请求参数错误"));
                        return;
                    }
                    WebApi?.Write(source.Response, (object)Write(concurrentDictionary));
                }
                if (absolutePath.Equals(getstatus))
                {
                    WebApi?.Write(source.Response, (object)GetStatus());
                }
                if (absolutePath.Equals(switchlanguage))
                {
                    LanguageType language = ((GetLanguage() == LanguageType.zh) ? LanguageType.en : LanguageType.zh);
                    SetLanguage(language);
                }
            }
            catch (Exception ex)
            {
                WebApi?.Write(source.Response, (object)OperateResult.CreateFailureResult("接口响应异常：" + ex.Message));
            }
        }

        public OperateResult WAOn(WAModel wAModel)
        {
            if (WebApi == null)
            {
                WebApi = new HttpServiceOperate();
            }
            HttpServiceData.Basics basics = new HttpServiceData.Basics();
            basics.SET(wAModel);
            basics.AbsolutePaths = new List<string> { on, off, read, write, getstatus, switchlanguage };
            WebApi = CoreUnify<HttpServiceOperate, HttpServiceData.Basics>.Instance(basics);
            WebApi.OnDataEvent -= WebApi_OnDataEvent;
            WebApi.OnDataEvent += WebApi_OnDataEvent;
            WebApi.OnInfoEvent -= base.OnInfoEventHandler;
            WebApi.OnInfoEvent += base.OnInfoEventHandler;
            return WebApi.On();
        }

        public OperateResult WAOff()
        {
            BegOperate("WAOff");
            if (WebApi == null)
            {
                return EndOperate(status: false, "WA尚未启动", null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\abstract\\DaqAbstract.cs", "WAOff", 163);
            }
            WebApi.Dispose();
            WebApi = null;
            return EndOperate(status: false, "WA停止成功", null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\abstract\\DaqAbstract.cs", "WAOff", 167);
        }

        public OperateResult WAStatus()
        {
            if (WebApi == null)
            {
                return EndOperate(status: false, "WA尚未启动", null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\abstract\\DaqAbstract.cs", BegOperate("WAStatus"), 174);
            }
            return WebApi.GetStatus();
        }

        public OperateResult WARequestExample()
        {
            return EndOperate(status: true, null, $"\r\n{on} [ 打开 ]，无参请求\r\n\r\n{off} [ 关闭 ]，无参请求\r\n\r\n{getstatus} [ 获取状态 ]，无参请求\r\n\r\n{switchlanguage} [ 切换语言 ]，无参请求\r\n\r\n{read} [ 读取 ]，带参请求：Demo.Model.data.Address\r\n{{\r\n    \"SN\": \"8c71f4a7-04eb-4f9c-88d9-849c2f0c3a00\",\r\n    \"AddressArray\": [\r\n        {{\r\n            \"SN\": \"TestAddress\",\r\n            \"AddressName\": \"M100\",\r\n            \"AddressDataType\": \"Float\"\r\n        }}\r\n    ],\r\n    \"CreationTime\": \"2024-06-05T13:01:24.6245462+08:00\"\r\n}}\r\n\r\n{write} [ 写入 ]，带参请求：ConcurrentDictionary<string, Demo.Model.data.WriteModel>\r\n{{\r\n  \"M100\": {{\r\n    \"Value\": 99.1,\r\n    \"AddressDataType\": \"Float\"\r\n  }}\r\n}}", null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\abstract\\DaqAbstract.cs", BegOperate("WARequestExample"), 181);
        }

        public async Task<OperateResult> WAOnAsync(WAModel wAModel, CancellationToken token = default(CancellationToken))
        {
            WAModel wAModel2 = wAModel;
            return await Task.Run(() => WAOn(wAModel2), token);
        }

        public async Task<OperateResult> WAOffAsync(CancellationToken token = default(CancellationToken))
        {
            return await Task.Run(() => WAOff(), token);
        }

        public async Task<OperateResult> WAStatusAsync(CancellationToken token = default(CancellationToken))
        {
            return await Task.Run(() => WAStatus(), token);
        }

        public async Task<OperateResult> WARequestExampleAsync(CancellationToken token = default(CancellationToken))
        {
            return await Task.Run(() => WARequestExample(), token);
        }

        public abstract OperateResult GetBaseObject();

        public abstract OperateResult GetStatus();

        public abstract OperateResult Off(bool hardClose = false);

        public abstract OperateResult On();

        public abstract OperateResult Read(Address address);

        public abstract OperateResult Subscribe(Address address);

        public abstract OperateResult UnSubscribe(Address address);

        public abstract OperateResult Write(ConcurrentDictionary<string, (object value, EncodingType? encodingType)> values);

        public OperateResult Write(ConcurrentDictionary<string, object> values)
        {
            return Write(values.GetDefaultEncodingWrite(EncodingType.ASCII));
        }

        public OperateResult Write(ConcurrentDictionary<string, WriteModel> values)
        {
            if (values == null || values.Count <= 0)
            {
                return OperateResult.CreateFailureResult("数据不能为空");
            }
            ConcurrentDictionary<string, (object, EncodingType?)> concurrentDictionary = new ConcurrentDictionary<string, (object, EncodingType?)>();
            foreach (KeyValuePair<string, WriteModel> value in values)
            {
                try
                {
                    switch (value.Value.AddressDataType)
                    {
                        case DataType.Bool:
                            concurrentDictionary.TryAdd(value.Key, (Convert.ToBoolean(value.Value.Value.ToString()), value.Value.EncodingType));
                            break;
                        case DataType.BoolArray:
                            if (value.Value.Value is string)
                            {
                                concurrentDictionary.TryAdd(value.Key, (value.Value.Value.ToString().ToJsonEntity<bool[]>(), value.Value.EncodingType));
                            }
                            else
                            {
                                concurrentDictionary.TryAdd(value.Key, (value.Value.Value.GetSource<bool[]>(), value.Value.EncodingType));
                            }
                            break;
                        case DataType.String:
                            concurrentDictionary.TryAdd(value.Key, (value.Value.Value.ToString(), value.Value.EncodingType));
                            break;
                        case DataType.Char:
                            concurrentDictionary.TryAdd(value.Key, (Convert.ToChar(value.Value.Value.ToString()), value.Value.EncodingType));
                            break;
                        case DataType.Double:
                            concurrentDictionary.TryAdd(value.Key, (Convert.ToDouble(value.Value.Value.ToString()), value.Value.EncodingType));
                            break;
                        case DataType.DoubleArray:
                            if (value.Value.Value is string)
                            {
                                concurrentDictionary.TryAdd(value.Key, (value.Value.Value.ToString().ToJsonEntity<double[]>(), value.Value.EncodingType));
                            }
                            else
                            {
                                concurrentDictionary.TryAdd(value.Key, (value.Value.Value.GetSource<double[]>(), value.Value.EncodingType));
                            }
                            break;
                        case DataType.Float:
                        case DataType.Single:
                            concurrentDictionary.TryAdd(value.Key, (Convert.ToSingle(value.Value.Value.ToString()), value.Value.EncodingType));
                            break;
                        case DataType.FloatArray:
                        case DataType.SingleArray:
                            if (value.Value.Value is string)
                            {
                                concurrentDictionary.TryAdd(value.Key, (value.Value.Value.ToString().ToJsonEntity<float[]>(), value.Value.EncodingType));
                            }
                            else
                            {
                                concurrentDictionary.TryAdd(value.Key, (value.Value.Value.GetSource<float[]>(), value.Value.EncodingType));
                            }
                            break;
                        case DataType.Int:
                        case DataType.Int32:
                            concurrentDictionary.TryAdd(value.Key, (Convert.ToInt32(value.Value.Value.ToString()), value.Value.EncodingType));
                            break;
                        case DataType.IntArray:
                        case DataType.Int32Array:
                            if (value.Value.Value is string)
                            {
                                concurrentDictionary.TryAdd(value.Key, (value.Value.Value.ToString().ToJsonEntity<int[]>(), value.Value.EncodingType));
                            }
                            else
                            {
                                concurrentDictionary.TryAdd(value.Key, (value.Value.Value.GetSource<int[]>(), value.Value.EncodingType));
                            }
                            break;
                        case DataType.Uint:
                        case DataType.UInt32:
                            concurrentDictionary.TryAdd(value.Key, (Convert.ToUInt32(value.Value.Value.ToString()), value.Value.EncodingType));
                            break;
                        case DataType.UintArray:
                        case DataType.UInt32Array:
                            if (value.Value.Value is string)
                            {
                                concurrentDictionary.TryAdd(value.Key, (value.Value.Value.ToString().ToJsonEntity<uint[]>(), value.Value.EncodingType));
                            }
                            else
                            {
                                concurrentDictionary.TryAdd(value.Key, (value.Value.Value.GetSource<uint[]>(), value.Value.EncodingType));
                            }
                            break;
                        case DataType.Long:
                        case DataType.Int64:
                            concurrentDictionary.TryAdd(value.Key, (Convert.ToInt64(value.Value.Value.ToString()), value.Value.EncodingType));
                            break;
                        case DataType.LongArray:
                        case DataType.Int64Array:
                            if (value.Value.Value is string)
                            {
                                concurrentDictionary.TryAdd(value.Key, (value.Value.Value.ToString().ToJsonEntity<long[]>(), value.Value.EncodingType));
                            }
                            else
                            {
                                concurrentDictionary.TryAdd(value.Key, (value.Value.Value.GetSource<long[]>(), value.Value.EncodingType));
                            }
                            break;
                        case DataType.Ulong:
                        case DataType.UInt64:
                            concurrentDictionary.TryAdd(value.Key, (Convert.ToUInt64(value.Value.Value.ToString()), value.Value.EncodingType));
                            break;
                        case DataType.UlongArray:
                        case DataType.UInt64Array:
                            if (value.Value.Value is string)
                            {
                                concurrentDictionary.TryAdd(value.Key, (value.Value.Value.ToString().ToJsonEntity<ulong[]>(), value.Value.EncodingType));
                            }
                            else
                            {
                                concurrentDictionary.TryAdd(value.Key, (value.Value.Value.GetSource<ulong[]>(), value.Value.EncodingType));
                            }
                            break;
                        case DataType.Short:
                        case DataType.Int16:
                            concurrentDictionary.TryAdd(value.Key, (Convert.ToInt16(value.Value.Value.ToString()), value.Value.EncodingType));
                            break;
                        case DataType.ShortArray:
                        case DataType.Int16Array:
                            if (value.Value.Value is string)
                            {
                                concurrentDictionary.TryAdd(value.Key, (value.Value.Value.ToString().ToJsonEntity<short[]>(), value.Value.EncodingType));
                            }
                            else
                            {
                                concurrentDictionary.TryAdd(value.Key, (value.Value.Value.GetSource<short[]>(), value.Value.EncodingType));
                            }
                            break;
                        case DataType.Ushort:
                        case DataType.UInt16:
                            concurrentDictionary.TryAdd(value.Key, (Convert.ToUInt16(value.Value.Value.ToString()), value.Value.EncodingType));
                            break;
                        case DataType.UshortArray:
                        case DataType.UInt16Array:
                            if (value.Value.Value is string)
                            {
                                concurrentDictionary.TryAdd(value.Key, (value.Value.Value.ToString().ToJsonEntity<ushort[]>(), value.Value.EncodingType));
                            }
                            else
                            {
                                concurrentDictionary.TryAdd(value.Key, (value.Value.Value.GetSource<ushort[]>(), value.Value.EncodingType));
                            }
                            break;
                        case DataType.ByteArray:
                            if (value.Value.Value is string)
                            {
                                concurrentDictionary.TryAdd(value.Key, (ByteHandler.HexStringToByteArray(value.Value.Value.ToString()), value.Value.EncodingType));
                            }
                            else
                            {
                                concurrentDictionary.TryAdd(value.Key, (value.Value.Value.GetSource<byte[]>(), value.Value.EncodingType));
                            }
                            break;
                        default:
                            return OperateResult.CreateFailureResult($"{value.Key} 不支持 {value.Value.AddressDataType} 类型转换");
                    }
                }
                catch (Exception ex)
                {
                    return OperateResult.CreateFailureResult(value.Key + " 地址数据类型转换异常:" + ex.Message);
                }
            }
            return Write(concurrentDictionary);
        }

        public async Task<OperateResult> GetBaseObjectAsync(CancellationToken token = default(CancellationToken))
        {
            return await Task.Run(() => GetBaseObject(), token);
        }

        public async Task<OperateResult> GetStatusAsync(CancellationToken token = default(CancellationToken))
        {
            return await Task.Run(() => GetStatus(), token);
        }

        public async Task<OperateResult> OffAsync(bool hardClose = false, CancellationToken token = default(CancellationToken))
        {
            return await Task.Run(() => Off(hardClose), token);
        }

        public async Task<OperateResult> OnAsync(CancellationToken token = default(CancellationToken))
        {
            return await Task.Run(() => On(), token);
        }

        public async Task<OperateResult> ReadAsync(Address address, CancellationToken token = default(CancellationToken))
        {
            Address address2 = address;
            return await Task.Run(() => Read(address2), token);
        }

        public async Task<OperateResult> SubscribeAsync(Address address, CancellationToken token = default(CancellationToken))
        {
            Address address2 = address;
            return await Task.Run(() => Subscribe(address2), token);
        }

        public async Task<OperateResult> UnSubscribeAsync(Address address, CancellationToken token = default(CancellationToken))
        {
            Address address2 = address;
            return await Task.Run(() => UnSubscribe(address2), token);
        }

        public async Task<OperateResult> WriteAsync(ConcurrentDictionary<string, object> values, CancellationToken token = default(CancellationToken))
        {
            ConcurrentDictionary<string, object> values2 = values;
            return await Task.Run(() => Write(values2), token);
        }

        public async Task<OperateResult> WriteAsync(ConcurrentDictionary<string, WriteModel> values, CancellationToken token = default(CancellationToken))
        {
            ConcurrentDictionary<string, WriteModel> values2 = values;
            return await Task.Run(() => Write(values2), token);
        }

        public async Task<OperateResult> WriteAsync(ConcurrentDictionary<string, (object value, EncodingType? encodingType)> values, CancellationToken token = default(CancellationToken))
        {
            ConcurrentDictionary<string, (object value, EncodingType? encodingType)> values2 = values;
            return await Task.Run(() => Write(values2), token);
        }

        public override void Dispose()
        {
            WAOff();
            Off(hardClose: true);
            base.Dispose();
        }

        public override async Task DisposeAsync()
        {
            await WAOffAsync();
            await OffAsync(hardClose: true);
            await base.DisposeAsync();
        }
    }

}
