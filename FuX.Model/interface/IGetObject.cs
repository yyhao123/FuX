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
    //     获取对象
    public interface IGetObject
    {
        //
        // 摘要:
        //     获取底层通信对象
        //
        // 返回结果:
        //     统一结果
        OperateResult GetBaseObject();

        //
        // 摘要:
        //     获取底层通信对象异步
        //
        // 参数:
        //   token:
        //     传播应取消操作的通知
        //
        // 返回结果:
        //     统一结果
        Task<OperateResult> GetBaseObjectAsync(CancellationToken token = default(CancellationToken));
    }
}
