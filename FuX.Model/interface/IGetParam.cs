using System.Threading;
using System.Threading.Tasks;
using FuX.Model.data;

namespace FuX.Model.@interface
{
    //
    // 摘要:
    //     获取参数接口
    public interface IGetParam
    {
        //
        // 摘要:
        //     获取基础数据
        //     此返回的是:操作类实例化传入的基础数据
        //
        // 返回结果:
        //     统一结果
        OperateResult GetBasicsData();

        //
        // 摘要:
        //     获取基础数据
        //     此返回的是:操作类实例化传入的基础数据
        //
        // 参数:
        //   token:
        //     传播应取消操作的通知
        //
        // 返回结果:
        //     统一结果
        Task<OperateResult> GetBasicsDataAsync(CancellationToken token = default(CancellationToken));

        //
        // 摘要:
        //     获取参数
        //
        // 参数:
        //   getBasicsParam:
        //     获取基础数据
        //     true : 基础数据的'新'对象 false : 基础数据组织后的'新'对象
        //
        // 返回结果:
        //     统一结果
        OperateResult GetParam(bool getBasicsParam = false);

        //
        // 摘要:
        //     获取参数
        //
        // 参数:
        //   getBasicsParam:
        //     获取基础数据
        //     true : 基础数据的新对象 false : 基础数据组织后的新对象
        //
        //   token:
        //     传播应取消操作的通知
        //
        // 返回结果:
        //     统一结果
        Task<OperateResult> GetParamAsync(bool getBasicsParam = false, CancellationToken token = default(CancellationToken));

        //
        // 摘要:
        //     获取自动分配标识的属性值所对应参数集合；
        //     基础数据中必须包含 AutoAllocatingTagAttribute 特性；
        //     如果不包含，说明此库参数基本都需要填写
        //
        // 返回结果:
        //     统一结果
        OperateResult GetAutoAllocatingParam();

        //
        // 摘要:
        //     获取自动分配标识的属性值所对应参数集合；
        //     基础数据中必须包含 AutoAllocatingTagAttribute 特性；
        //     如果不包含，说明此库参数基本都需要填写
        //
        // 参数:
        //   token:
        //     传播应取消操作的通知
        //
        // 返回结果:
        //     统一结果
        Task<OperateResult> GetAutoAllocatingParamAsync(CancellationToken token = default(CancellationToken));

        //
        // 摘要:
        //     是否存在自动分配参数；
        //     判断当前基础数据中是否存在 AutoAllocatingTagAttribute 特性；
        //
        // 返回结果:
        //     统一结果
        OperateResult ExistsAutoAllocatingParam();

        //
        // 摘要:
        //     是否存在自动分配参数；
        //     判断当前基础数据中是否存在 AutoAllocatingTagAttribute 特性；
        //
        // 参数:
        //   token:
        //     传播应取消操作的通知
        //
        // 返回结果:
        //     统一结果
        Task<OperateResult> ExistsAutoAllocatingParamAsync(CancellationToken token = default(CancellationToken));
    }
}
