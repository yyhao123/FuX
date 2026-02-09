using FuX.Model.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.@interface
{
    public interface ISendService
    {
        OperateResult Send(byte[] data, string[]? address = null);

        Task<OperateResult> SendAsync(byte[] data, string[]? address = null, CancellationToken token = default(CancellationToken));

        OperateResult Send(byte[] data, string address);

        Task<OperateResult> SendAsync(byte[] data, string address, CancellationToken token = default(CancellationToken));
    }
}
