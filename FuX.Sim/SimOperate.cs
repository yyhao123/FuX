
using Dm.filter.rw;
using FuX.Core.@abstract;
using FuX.Core.extend;
using FuX.Core.handler;
using FuX.Core.subscription;
using FuX.Core.virtualAddress;
using FuX.Model.data;
using FuX.Model.@enum;
using FuX.Model.@interface;
using FuX.Unility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Sim
{
    public class SimOperate : DaqAbstract<SimOperate, SimData.Basics>, IDaq, IOn, IOff, IRead, IWrite, ISubscribe, IEvent, IGetStatus, IGetParam, ICreateInstance, ILog, IWA, IGetObject, ILanguage, IDisposable
    {
        private VirtualAddressManage? VAM;

        private SubscribeOperate? subscribeOperate;

        protected override string CN => "采集模拟";

        protected override string CD => "地址类型中内置五种模式，虚拟静态地址、虚拟动态随机变化地址、虚拟动态随机范围变化地址、虚拟动态顺序变化地址、虚拟动态顺序范围变化地址";

        protected override List<ParamModel.propertie> AP => new List<ParamModel.propertie>
    {
        new ParamModel.propertie
        {
            PropertyName = "ServiceName",
            Description = "命名空间",
            Show = false,
            Use = false,
            Default = GetType().FullName,
            DataCate = null
        }
    };

        public SimOperate()
        {
        }

        public SimOperate(SimData.Basics basics)
            : base(basics)
        {
        }

        public override OperateResult On()
        {
            BegOperate("On");
            try
            {
                if (GetStatus().GetDetails(out string message))
                {
                    return EndOperate(status: false, message, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "On", 67);
                }
                if (VAM == null)
                {
                    VAM = new VirtualAddressManage();
                }
                return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "On", 75);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "On", 79);
            }
        }

        public override OperateResult Off(bool hardClose = false)
        {
            BegOperate("Off");
            try
            {
                if (!hardClose && !GetStatus().GetDetails(out string message))
                {
                    return EndOperate(status: false, message, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "Off", 92);
                }
                if (subscribeOperate != null)
                {
                    OperateResult operateResult = subscribeOperate.Off();
                    if (!operateResult.Status)
                    {
                        return EndOperate(status: false, operateResult.Message, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "Off", 100);
                    }
                    subscribeOperate = null;
                }
                VAM?.Dispose();
                VAM = null;
                return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "Off", 106);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "Off", 110);
            }
        }

        public override OperateResult Read(Address address)
        {
            BegOperate("Read");
            try
            {
                if (!GetStatus().GetDetails(out string message))
                {
                    return EndOperate(status: false, message, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "Read", 121);
                }
                if (!address.CheckAddress())
                {
                    return EndOperate(status: false, "存在无效点位数据，操作失败".GetLanguageValue(), null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "Read", 126);
                }
                ConcurrentDictionary<string, AddressValue> concurrentDictionary = new ConcurrentDictionary<string, AddressValue>();
                foreach (AddressDetails item in address.AddressArray)
                {
                    if (!item.IsEnable)
                    {
                        continue;
                    }
                    bool IsVA = false;
                    VAM.InitVirtualAddress(item, out IsVA);
                    object obj = null;
                    if (IsVA)
                    {
                        obj = VAM.Read(item);
                        AddressValue addressValue = item.ExecuteDispose(obj, string.IsNullOrEmpty(obj?.ToString()) ? "失败".GetLanguageValue() : "成功".GetLanguageValue());
                        concurrentDictionary.AddOrUpdate(item.AddressName, addressValue, (string k, AddressValue v) => addressValue);
                    }
                }
                if (concurrentDictionary.Count > 0)
                {
                    return EndOperate(status: true, null, concurrentDictionary, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "Read", 155);
                }
                return EndOperate(status: false, "读取失败".GetLanguageValue(), null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "Read", 159);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "Read", 164);
            }
        }

        public override OperateResult Write(ConcurrentDictionary<string, (object value, EncodingType? encodingType)> values)
        {
            BegOperate("Write");
            try
            {
                if (!GetStatus().GetDetails(out string message))
                {
                    return EndOperate(status: false, message, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "Write", 175);
                }
                List<string> list = new List<string>();
                foreach (KeyValuePair<string, (object, EncodingType?)> value in values)
                {
                    if (VAM.IsVirtualAddress(value.Key) && !VAM.Write(value.Key, value.Value.Item1))
                    {
                        list.Add((value.Key + "，写入失败").GetLanguageValue());
                    }
                }
                if (list.Count > 0)
                {
                    return EndOperate(status: false, list.ToJson(), null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "Write", 195);
                }
                return EndOperate(status: true, "写入成功".GetLanguageValue(), null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "Write", 197);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "Write", 201);
            }
        }

        public override OperateResult Subscribe(Address address)
        {
            BegOperate("Subscribe");
            try
            {
                if (!GetStatus().GetDetails(out string message))
                {
                    return EndOperate(status: false, message, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "Subscribe", 212);
                }
                if (!address.CheckAddress())
                {
                    return EndOperate(status: false, "存在无效点位数据，操作失败".GetLanguageValue(), null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "Subscribe", 217);
                }
                if (subscribeOperate == null)
                {
                    subscribeOperate = CoreUnify<SubscribeOperate, SubscribeData.Basics>.Instance(new SubscribeData.Basics
                    {
                        Address = address,
                        ChangeOut = base.basics.ChangeOut,
                        Function = Read,
                        AllOut = base.basics.AllOut,
                        HandleInterval = base.basics.HandleInterval,
                        SN = base.basics.SN,
                        TaskNumber = base.basics.TaskNumber
                    });
                    subscribeOperate.OnDataEvent += base.OnDataEventHandler;
                    subscribeOperate.OnInfoEvent += base.OnInfoEventHandler;
                    return EndOperate(subscribeOperate.On(), logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "Subscribe", 234);
                }
                return EndOperate(subscribeOperate.Subscribe(address), logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "Subscribe", 238);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "Subscribe", 243);
            }
        }

        public override OperateResult UnSubscribe(Address address)
        {
            BegOperate("UnSubscribe");
            try
            {
                if (!GetStatus().GetDetails(out string message))
                {
                    return EndOperate(status: false, message, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "UnSubscribe", 254);
                }
                if (subscribeOperate != null)
                {
                    return EndOperate(subscribeOperate.UnSubscribe(address), logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "UnSubscribe", 259);
                }
                return EndOperate(status: false, "当前尚未订阅".GetLanguageValue(), null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "UnSubscribe", 263);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "UnSubscribe", 268);
            }
        }

        public override OperateResult GetStatus()
        {
            BegOperate("GetStatus");
            try
            {
                if (VAM == null)
                {
                    return EndOperate(status: false, "未连接".GetLanguageValue(), null, null, logOutput: false, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "GetStatus", 279);
                }
                return EndOperate(status: true, "已连接".GetLanguageValue(), null, null, logOutput: false, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "GetStatus", 281);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", "GetStatus", 285);
            }
        }

        public override OperateResult GetBaseObject()
        {
            return EndOperate(status: false, "无底层公共对象", null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Sim\\SimOperate.cs", BegOperate("GetBaseObject"), 292);
        }
    }


}
