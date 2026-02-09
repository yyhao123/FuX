using FuX.Model.data;


namespace FuX.Model.@interface
{
    //
    // 摘要:
    //     创建实例接口（单例模式）； 特殊情况下使用
    public interface ICreateInstance
    {
        //
        // 摘要:
        //     创建一个实例
        //
        // 参数:
        //   param:
        //     基础数据
        //
        // 类型参数:
        //   T:
        //     泛型基础数据
        //
        // 返回结果:
        //     统一返回包含自身单例对象
        OperateResult CreateInstance<T>(T param);

        //
        // 摘要:
        //     创建一个实例
        //
        // 参数:
        //   param:
        //     基础数据
        //
        //   token:
        //     传播应取消操作的通知
        //
        // 类型参数:
        //   T:
        //     泛型基础数据
        //
        // 返回结果:
        //     统一返回包含自身单例对象
        Task<OperateResult> CreateInstanceAsync<T>(T param, CancellationToken token = default(CancellationToken));

        //
        // 摘要:
        //     创建一个实例
        //
        // 参数:
        //   json:
        //     基础数据反序列化为JSON的字符串
        //
        // 返回结果:
        //     统一返回包含自身单例对象
        OperateResult CreateInstance(string json);

        //
        // 摘要:
        //     创建一个实例
        //
        // 参数:
        //   json:
        //     基础数据反序列化为JSON的字符串
        //
        //   token:
        //     传播应取消操作的通知
        //
        // 返回结果:
        //     统一返回包含自身单例对象
        Task<OperateResult> CreateInstanceAsync(string json, CancellationToken token = default(CancellationToken));
    }
}
