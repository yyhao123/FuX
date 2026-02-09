using FuX.Model.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.@interface
{
    public interface IConsumer : IEvent
    {
        OperateResult Consume(string topic);

        Task<OperateResult> ConsumeAsync(string topic, CancellationToken token = default(CancellationToken));

        OperateResult UnConsume(string topic);

        Task<OperateResult> UnConsumeAsync(string topic, CancellationToken token = default(CancellationToken));
    }

}
