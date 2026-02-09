using FuX.Model.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.@interface
{
    public interface IWA
    {
        OperateResult WAOn(WAModel wAModel);

        Task<OperateResult> WAOnAsync(WAModel wAModel, CancellationToken token = default(CancellationToken));

        OperateResult WAOff();

        Task<OperateResult> WAOffAsync(CancellationToken token = default(CancellationToken));

        OperateResult WAStatus();

        Task<OperateResult> WAStatusAsync(CancellationToken token = default(CancellationToken));

        OperateResult WARequestExample();

        Task<OperateResult> WARequestExampleAsync(CancellationToken token = default(CancellationToken));
    }

}
