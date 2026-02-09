using FuX.Model.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.@interface
{
    public interface IRemove
    {
        OperateResult Remove(string[] address);

        Task<OperateResult> RemoveAsync(string[] address, CancellationToken token = default(CancellationToken));
    }
}
