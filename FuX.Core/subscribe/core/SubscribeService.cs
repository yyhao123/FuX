using FuX.Core.extend;
using FuX.Model.data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Core.subscribe.core
{
    public class SubscribeService<T> : CoreUnify<SubscribeService<T>, string>, IDisposable
    {
        private ConcurrentDictionary<string, SubscribeSource<T>> Source = new ConcurrentDictionary<string, SubscribeSource<T>>();

        public SubscribeService(string basics)
            : base(basics)
        {
        }

        public override void Dispose()
        {
            foreach (KeyValuePair<string, SubscribeSource<T>> item in Source)
            {
                item.Value.Dispose();
            }
            Source.Clear();
            base.Dispose();
        }

        public override async Task DisposeAsync()
        {
            foreach (KeyValuePair<string, SubscribeSource<T>> item in Source)
            {
                await item.Value.DisposeAsync();
            }
            Source.Clear();
            await base.DisposeAsync();
        }

        public OperateResult Get(string SN)
        {
            if (CoreUnify<SubscribeService<T>, string>.objList == null)
            {
                throw new Exception("please use singleton mode");
            }
            if (Source.ContainsKey(SN))
            {
                return Source[SN].Get();
            }
            return EndOperate(status: false, "(" + SN + ") Instance does not exist", null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\subscribe\\core\\SubscribeService.cs", BegOperate("Get"), 61);
        }

        public async Task<OperateResult> GetAsync(string SN, CancellationToken token = default(CancellationToken))
        {
            string SN2 = SN;
            return await Task.Run(() => Get(SN2), token);
        }

        public OperateResult Set(string SN, T Data)
        {
            if (CoreUnify<SubscribeService<T>, string>.objList == null)
            {
                throw new Exception("please use singleton mode");
            }
            BegOperate("Set");
            if (!Source.ContainsKey(SN))
            {
                SubscribeSource<T> core = new SubscribeSource<T>(SN);
                core.OnDataEvent += base.OnDataEventHandler;
                Source.AddOrUpdate(SN, core, (string k, SubscribeSource<T> v) => core);
            }
            Source[SN].Set(Data);
            return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\subscribe\\core\\SubscribeService.cs", "Set", 92);
        }

        public async Task<OperateResult> SetAsync(string SN, T Data, CancellationToken token = default(CancellationToken))
        {
            string SN2 = SN;
            T Data2 = Data;
            return await Task.Run(() => Set(SN2, Data2), token);
        }
    }

}
