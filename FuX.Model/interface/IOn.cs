using FuX.Model.data;


namespace FuX.Model.@interface
{
    //
    // 摘要:
    //     打开接口
    public interface IOn
    {
        //
        // 摘要:
        //     打开
        //
        // 返回结果:
        //     统一结果
        OperateResult On();

        //
        // 摘要:
        //     异步打开
        //
        // 参数:
        //   token:
        //     传播应取消操作的通知
        //
        // 返回结果:
        //     统一结果
        Task<OperateResult> OnAsync(CancellationToken token = default(CancellationToken));
    }
}
