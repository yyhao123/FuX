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
    //     发送等待
    public interface ISendWait
    {
        //
        // 摘要:
        //     发送等待结果，适用于单次命令应答模式
        //     会先关闭数据监控,在进行数据发送,函数内捕获数据,完成后在启动数据监控
        //     只要收到一次数据就会立马返回收到的数据
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
        OperateResult SendWait(byte[] data, CancellationToken token);

        //
        // 摘要:
        //     异步发送等待结果，适用于单次命令应答模式
        //     会先关闭数据监控,在进行数据发送,函数内捕获数据,完成后在启动数据监控
        //     只要收到一次数据就会立马返回收到的数据
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
        Task<OperateResult> SendWaitAsync(byte[] data, CancellationToken token);
    }
}
