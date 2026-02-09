using FuX.Core.extend;
using FuX.Model.data;
using FuX.Unility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Core.subscribe.core
{
    public class SubscribeSource<T> : CoreUnify<SubscribeSource<T>, string>, IDisposable
    {
        public T Source { get; set; }

        public DateTime UpdateTime { get; set; } = DateTime.Now;


        public SubscribeSource(string basics)
            : base(basics)
        {
        }

        public OperateResult Set(T Data)
        {
            BegOperate("Set");
            UpdateTime = DateTime.Now;
            if (!Data.Comparer(Source, new string[1] { "Time" }).result)
            {
                Source = Data;
                OnDataEventHandler(this, new EventDataResult(status: true, "Data Update", this));
            }
            return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\subscribe\\core\\SubscribeSource.cs", "Set", 44);
        }

        public async Task<OperateResult> SetAsync(T Data)
        {
            T Data2 = Data;
            return await Task.Run(() => Set(Data2));
        }

        public OperateResult Get()
        {
            return EndOperate(status: true, "Get Succeed", this, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\subscribe\\core\\SubscribeSource.cs", BegOperate("Get"), 60);
        }

        public async Task<OperateResult> GetAsync()
        {
            return await Task.Run(() => Get());
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override async Task DisposeAsync()
        {
            await base.DisposeAsync();
        }
    }

}
