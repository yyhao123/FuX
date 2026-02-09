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
    //     发送
    public interface ISend
    {
        //
        // 摘要:
        //     发送
        //
        // 参数:
        //   data:
        //     字节数据
        //
        // 返回结果:
        //     统一出参
        OperateResult Send(byte[] data);

        //
        // 摘要:
        //     发送
        //
        // 参数:
        //   data:
        //     字节数据
        //
        //   token:
        //     传播应取消操作的通知
        //
        // 返回结果:
        //     统一出参
        Task<OperateResult> SendAsync(byte[] data, CancellationToken token = default(CancellationToken));
    }
}
