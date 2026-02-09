using FuX.Model.data;


namespace FuX.Model.@interface
{
    //
    // 摘要:
    //     关闭接口
    public interface IOff
    {
        //
        // 摘要:
        //     关闭
        //
        // 参数:
        //   hardClose:
        //     强制关闭，不验证状态
        //
        // 返回结果:
        //     统一结果
        OperateResult Off(bool hardClose = false);

        //
        // 摘要:
        //     关闭
        //
        // 参数:
        //   hardClose:
        //     强制关闭，不验证状态
        //
        //   token:
        //     传播应取消操作的通知
        //
        // 返回结果:
        //     统一结果
        Task<OperateResult> OffAsync(bool hardClose = false, CancellationToken token = default(CancellationToken));
    }
}
