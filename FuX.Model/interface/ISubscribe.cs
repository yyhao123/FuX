using FuX.Model.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.@interface
{
    public interface ISubscribe : IEvent
    {
        OperateResult Subscribe(Address address);

        Task<OperateResult> SubscribeAsync(Address address, CancellationToken token = default(CancellationToken));

        OperateResult UnSubscribe(Address address);

        Task<OperateResult> UnSubscribeAsync(Address address, CancellationToken token = default(CancellationToken));
    }

}
