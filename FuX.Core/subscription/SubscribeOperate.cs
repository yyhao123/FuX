using FuX.Core.extend;
using FuX.Core.subscribe.core;
using FuX.Model.data;
using FuX.Model.@interface;
using FuX.Unility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace FuX.Core.subscription
{
    public class SubscribeOperate : CoreUnify<SubscribeOperate, SubscribeData.Basics>, ISubscribe, IEvent, IOn, IOff, IGetStatus, IDisposable
    {
        private CancellationTokenSource? MonitorSwitch;

        private ConcurrentDictionary<CancellationTokenSource, Task>? TaskArray;
        private ConcurrentDictionary<CancellationTokenSource, Task>? TaskArray2;

        private Channel<AddressValue>? DataQueue;

        private bool GoOn = true;

        private ConcurrentDictionary<string, AddressValue> DataCachePool = new ConcurrentDictionary<string, AddressValue>();

        private List<ConcurrentDictionary<string, AddressValue>> DataCachePoolArray = new List<ConcurrentDictionary<string, AddressValue>>();

        private BoundedChannelOptions channelOptions = new BoundedChannelOptions(int.MaxValue)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = false,
            SingleWriter = false
        };

      

        private SubscribeService<AddressValue> subscribeService = CoreUnify<SubscribeService<AddressValue>, string>.Instance(Guid.NewGuid().ToUpperNString());

        private Channel<AddressValue>? DataQueuePack;

        private CancellationTokenSource? DataQueuePackToken;

        public SubscribeOperate(SubscribeData.Basics basics)
            : base(basics)
        {
        }

        private async Task Polling(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    try
                    {
                        if (!GoOn || base.basics.Address == null || base.basics.Address.AddressArray == null || base.basics.Address.AddressArray.Count <= 0)
                        {
                            goto end_IL_0035;
                        }
                        List<AddressDetails> addressArray = base.basics.Address.AddressArray;
                        IGrouping<string, AddressDetails>[] array = (from p in addressArray
                                                                     group p by p.AddressName into g
                                                                     where g.Count() > 1
                                                                     select g).ToArray();
                        if (array.Length != 0)
                        {
                            IGrouping<string, AddressDetails>[] array2 = array;
                            for (int i = 0; i < array2.Length; i++)
                            {
                                foreach (AddressDetails item in array2[i])
                                {
                                    addressArray.Remove(item);
                                }
                            }
                            List<string> obj = array.Select((IGrouping<string, AddressDetails> g) => $"[ {g.Key} ]点位名存在{g.Count()}个重复，请检查后重新订阅当前点位（此操作为保证当前点位数据正确性）").ToList();
                            OnInfoEventHandler(this, EventInfoResult.CreateFailureResult(obj.ToJson()));
                        }
                        OperateResult operateResult;
                        lock (base.basics.Address)
                        {
                            operateResult = base.basics.Function?.Invoke(base.basics.Address) ?? OperateResult.CreateFailureResult("未设置读取函数");
                        }
                        if (!operateResult.Status)
                        {
                            OnInfoEventHandler(this, EventInfoResult.CreateFailureResult("自定义订阅轮询异常：" + operateResult.Message));
                            goto end_IL_0035;
                        }
                        object resultData = operateResult.ResultData;
                        ConcurrentDictionary<string, AddressValue> resultDict = resultData as ConcurrentDictionary<string, AddressValue>;
                        if (resultDict == null)
                        {
                            if (!(resultData is List<ConcurrentDictionary<string, AddressValue>> list) || list.Count <= 0)
                            {
                                continue;
                            }
                            if (base.basics.ChangeOut)
                            {
                                if (!Comparison(DataCachePoolArray, list))
                                {
                                    OnDataEventHandler(this, new EventDataResult(status: true, "存在变化数据", list));
                                    DataCachePoolArray = list;
                                }
                            }
                            else
                            {
                                OnDataEventHandler(this, new EventDataResult(status: true, "实时数据", list));
                            }
                            continue;
                        }
                        if (resultDict.Count <= 0)
                        {
                            continue;
                        }
                        if (base.basics.ChangeOut)
                        {
                            if (Comparison(DataCachePool, resultDict))
                            {
                                continue;
                            }
                            if (base.basics.AllOut)
                            {
                                OnDataEventHandler(this, new EventDataResult(status: true, "存在变化数据", resultDict));
                            }
                            else
                            {
                                foreach (AddressValue value in resultDict.Values)
                                {
                                    await DataQueue.Writer.WriteAsync(value, token).ConfigureAwait(continueOnCapturedContext: false);
                                }
                            }
                            DataCachePool = resultDict;
                            continue;
                        }
                        OnDataEventHandler(this, new EventDataResult(status: true, "实时数据", resultDict));
                        goto end_IL_002c;
                    end_IL_0035:;
                    }
                    catch (Exception ex)
                    {
                        OnInfoEventHandler(this, EventInfoResult.CreateFailureResult("自定义订阅轮询异常：" + ex.Message));
                        goto end_IL_002c;
                    }
                end_IL_002c:;
                }
                finally
                {
                    await Task.Delay(base.basics.HandleInterval, token).ConfigureAwait(continueOnCapturedContext: false);
                }
            }
        }

        private async Task TaskPackHandle(CancellationToken token)
        {
            try
            {
                while (await DataQueuePack.Reader.WaitToReadAsync(token).ConfigureAwait(continueOnCapturedContext: false))
                {
                    ConcurrentDictionary<string, AddressValue> concurrentDictionary = new ConcurrentDictionary<string, AddressValue>();
                    while (true)
                    {
                        if (!DataQueuePack.Reader.TryRead(out AddressValue queueData))
                        {
                            break;
                        }
                        if (queueData != null && !token.IsCancellationRequested)
                        {
                            concurrentDictionary.AddOrUpdate(queueData.AddressName, queueData, (string k, AddressValue v) => queueData);
                        }
                    }
                    if (concurrentDictionary.Count > 0)
                    {
                        OnDataEventHandler(this, new EventDataResult(status: true, "变化数据", concurrentDictionary));
                    }
                }
            }
            catch (TaskCanceledException)
            {
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception)
            {
            }
        }

        private async Task SubscribeService_OnDataEventAsync(object? sender, EventDataResult e)
        {
            SubscribeSource<AddressValue> source = e.GetSource<SubscribeSource<AddressValue>>();
            if (source != null)
            {
                await DataQueuePack.Writer.WriteAsync(source.Source).ConfigureAwait(continueOnCapturedContext: false);
            }
        }

        private async Task TaskHandle(CancellationToken token)
        {
            _ = 1;
            try
            {
                while (await DataQueue.Reader.WaitToReadAsync(token).ConfigureAwait(continueOnCapturedContext: false))
                {
                    AddressValue item;
                    while (DataQueue.Reader.TryRead(out item))
                    {
                        if (item != null && !token.IsCancellationRequested)
                        {
                            await subscribeService.SetAsync(item.AddressName, item, token);
                        }
                    }
                }

            }
            catch (TaskCanceledException)
            {
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception)
            {
            }
        }

        private async Task TaskHandle2(CancellationToken token)
        {
            
            try
            {
                if (1 == 1)
                {
                    int d = 1;

                }

            }
            catch (TaskCanceledException)
            {
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception)
            {
            }
        }

        private bool Comparison(ConcurrentDictionary<string, AddressValue> Param1, ConcurrentDictionary<string, AddressValue> Param2)
        {
            bool result = false;
            if (Param1.Count == Param2.Count)
            {
                result = true;
                {
                    foreach (KeyValuePair<string, AddressValue> item in Param1)
                    {
                        if (Param2.TryGetValue(item.Key, out AddressValue value))
                        {
                            if (!value.Equals(item.Value))
                            {
                                return false;
                            }
                            continue;
                        }
                        return false;
                    }
                    return result;
                }
            }
            return result;
        }

        private bool Comparison(List<ConcurrentDictionary<string, AddressValue>> Param1, List<ConcurrentDictionary<string, AddressValue>> Param2)
        {
            foreach (ConcurrentDictionary<string, AddressValue> a in Param2)
            {
                if (!Param1.Exists((ConcurrentDictionary<string, AddressValue> b) => Comparison(a, b)))
                {
                    return false;
                }
            }
            return true;
        }

        public override void Dispose()
        {
            Off(hardClose: true);
            base.Dispose();
        }

        public override async Task DisposeAsync()
        {
            await OffAsync(hardClose: true);
            await base.DisposeAsync();
        }

        public OperateResult Subscribe(Address address)
        {
            Address address2 = address;
            string mName = BegOperate("Subscribe");
            try
            {
                GoOn = false;
                lock (base.basics.Address)
                {
                    base.basics.Address.AddressArray.RemoveAll(delegate (AddressDetails a)
                    {
                        AddressDetails a2 = a;
                        return base.basics.Address.AddressArray.Where(a => address2.AddressArray.Any(b => b.AddressName == a.AddressName)).ToArray().Any(b => b.AddressName == a2.AddressName);
                    });
                    return RemoveIdenticalItem(address2, mName);
                }
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\subscribe\\SubscribeOperate.cs", "Subscribe", 339);
            }
        }

        private OperateResult RemoveIdenticalItem(Address address, string mName)
        {
            List<string> list = new List<string>();
            IGrouping<string, AddressDetails>[] array = (from p in address.AddressArray
                                                         group p by p.AddressName into g
                                                         where g.Count() > 1
                                                         select g).ToArray();
            if (array.Any())
            {
                IGrouping<string, AddressDetails>[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    foreach (AddressDetails item in array2[i])
                    {
                        address.AddressArray.Remove(item);
                    }
                }
                array2 = array;
                foreach (IGrouping<string, AddressDetails> grouping in array2)
                {
                    list.Add($"[ {grouping.Key} ]点位名存在{grouping.Count()}个重复，请检查后重新订阅当前点位（此操作为保证当前点位数据正确性）");
                }
            }
            if (address.AddressArray.Count > 0)
            {
                base.basics.Address.AddressArray.AddRange(address.AddressArray);
                base.basics.Address.AddressArray = base.basics.Address.AddressArray.Distinct().ToList();
            }
            if (list.Count > 0)
            {
                GoOn = true;
                return EndOperate(status: false, "订阅存在失败信息：" + list.ToJson(), list, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\subscribe\\SubscribeOperate.cs", mName, 383);
            }
            GoOn = true;
            return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\subscribe\\SubscribeOperate.cs", mName, 387);
        }

        public OperateResult UnSubscribe(Address address)
        {
            Address address2 = address;
            BegOperate("UnSubscribe");
            try
            {
                GoOn = false;
                List<string> list = new List<string>();
                lock (base.basics.Address)
                {
                    base.basics.Address.AddressArray.RemoveAll(delegate (AddressDetails a)
                    {
                        AddressDetails a2 = a;
                        return address2.AddressArray.Any((AddressDetails b) => b.AddressName == a2.AddressName);
                    });
                }
                GoOn = true;
                if (list.Count > 0)
                {
                    return EndOperate(status: false, list.ToJson(), null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\subscribe\\SubscribeOperate.cs", "UnSubscribe", 407);
                }
                return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\subscribe\\SubscribeOperate.cs", "UnSubscribe", 409);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\subscribe\\SubscribeOperate.cs", "UnSubscribe", 413);
            }
        }

        public OperateResult On()
        {
            BegOperate("On");
            try
            {
                if (MonitorSwitch == null)
                {
                    MonitorSwitch = new CancellationTokenSource();
                    if (DataQueue == null)
                    {
                        DataQueue = Channel.CreateBounded<AddressValue>(channelOptions);
                    }
                    Polling(MonitorSwitch.Token);
                    if (DataQueuePack == null)
                    {
                        DataQueuePack = Channel.CreateBounded<AddressValue>(channelOptions);
                    }
                    if (DataQueuePackToken == null)
                    {
                        DataQueuePackToken = new CancellationTokenSource();
                        TaskPackHandle(DataQueuePackToken.Token);
                    }
                    if (TaskArray == null)
                    {
                        TaskArray = new ConcurrentDictionary<CancellationTokenSource, Task>();
                        for (int i = 0; i < base.basics.TaskNumber; i++)
                        {
                            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                            TaskArray.TryAdd(cancellationTokenSource, TaskHandle(cancellationTokenSource.Token));
                        }
                        TaskArray2 = new ConcurrentDictionary<CancellationTokenSource, Task>();
                        for (int i = 0; i < base.basics.TaskNumber; i++)
                        {
                            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                            TaskArray2.TryAdd(cancellationTokenSource, TaskHandle2(cancellationTokenSource.Token));
                        }
                    }
                    subscribeService.OnDataEventAsync -= SubscribeService_OnDataEventAsync;
                    subscribeService.OnDataEventAsync += SubscribeService_OnDataEventAsync;
                    return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\subscribe\\SubscribeOperate.cs", "On", 460);
                }
                return EndOperate(status: false, "已启动", null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\subscribe\\SubscribeOperate.cs", "On", 464);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\subscribe\\SubscribeOperate.cs", "On", 469);
            }
        }

        public OperateResult Off(bool hardClose = false)
        {
            BegOperate("Off");
            try
            {
                GoOn = false;
                if (MonitorSwitch != null)
                {
                    MonitorSwitch.Cancel();
                }
                if (DataQueuePackToken != null)
                {
                    DataQueuePackToken.Cancel();
                }
                if (TaskArray != null)
                {
                    foreach (KeyValuePair<CancellationTokenSource, Task> item2 in TaskArray)
                    {
                        item2.Key.Cancel();
                    }
                    TaskArray.Clear();
                    TaskArray = null;
                }
                AddressValue item;
                if (DataQueue != null)
                {
                    DataQueue.Writer.Complete();
                    while (DataQueue.Reader.TryRead(out item))
                    {
                    }
                    DataQueue = null;
                }
                if (DataQueuePack != null)
                {
                    DataQueuePack.Writer.Complete();
                    while (DataQueuePack.Reader.TryRead(out item))
                    {
                    }
                    DataQueuePack = null;
                }
                subscribeService.OnDataEventAsync -= SubscribeService_OnDataEventAsync;
                return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\subscribe\\SubscribeOperate.cs", "Off", 519);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\subscribe\\SubscribeOperate.cs", "Off", 523);
            }
        }
        
        public OperateResult GetStatus()
        {
            BegOperate("GetStatus");
            if (DataQueue == null && TaskArray == null)
            {
                return EndOperate(status: false, "未订阅", null, null, logOutput: false, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\subscribe\\SubscribeOperate.cs", "GetStatus", 532);
            }
            return EndOperate(status: true, "已订阅", null, null, logOutput: false, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\subscribe\\SubscribeOperate.cs", "GetStatus", 536);
        }

        public async Task<OperateResult> SubscribeAsync(Address address, CancellationToken token = default(CancellationToken))
        {
            Address address2 = address;
            return await Task.Run(() => Subscribe(address2));
        }

        public async Task<OperateResult> UnSubscribeAsync(Address address, CancellationToken token = default(CancellationToken))
        {
            Address address2 = address;
            return await Task.Run(() => UnSubscribe(address2));
        }

        public async Task<OperateResult> OnAsync(CancellationToken token = default(CancellationToken))
        {
            return await Task.Run(() => On(), token);
        }

        public async Task<OperateResult> OffAsync(bool hardClose = false, CancellationToken token = default(CancellationToken))
        {
            return await Task.Run(() => Off(hardClose), token);
        }

        public async Task<OperateResult> GetStatusAsync(CancellationToken token = default(CancellationToken))
        {
            return await Task.Run(() => GetStatus(), token);
        }
    }
}
