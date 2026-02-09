using FuX.Model.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.@interface
{
    //
    // 摘要:
    //     获取状态
    public interface IGetStatus
    {
        //
        // 摘要:
        //     获取状态
        //
        // 返回结果:
        //     统一结果
        OperateResult GetStatus();

        //
        // 摘要:
        //     获取状态
        //
        // 参数:
        //   token:
        //     传播应取消操作的通知
        //
        // 返回结果:
        //     统一结果
        Task<OperateResult> GetStatusAsync(CancellationToken token = default(CancellationToken));
    }
}
