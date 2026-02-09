using FuX.Model.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.@interface
{
    public interface IRead
    {
        OperateResult Read(Address address);

        Task<OperateResult> ReadAsync(Address address, CancellationToken token = default(CancellationToken));
    }

}
