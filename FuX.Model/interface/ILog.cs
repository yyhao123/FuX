using FuX.Log;
using FuX.Model.data;
using Serilog.Events;


namespace FuX.Model.@interface
{
    //
    // 摘要:
    //     日志通用接口，用于关闭日志写入,关闭控制台输出
    public interface ILog
    {
        //
        // 摘要:
        //     日志操作参数设置；
        //     logOut为false时,consoleOut将不生效
        //
        // 参数:
        //   logOut:
        //     true:打开日志本地写入记录；
        //     false:关闭日志本地写入记录
        //
        //   consoleOut:
        //     true:控制台输出日志；
        //     false:控制不输出日志；
        //     null:根据底层代码传入的参数来做操作
        //
        //   noticeAsync:
        //     异步通知；
        //     外部传入的 Func 方法；
        //     用于给外部通知日志信息；
        //     此方法常用于日志入库
        //
        // 返回结果:
        //     操作结果
        OperateResult LogOperateSet(bool logOut = true, bool? consoleOut = null, Func<string, LogEventLevel, string?, Exception?, Task>? noticeAsync = null);

        //
        // 摘要:
        //     日志操作参数设置；
        //     logOut为false时,consoleOut将不生效
        //
        // 参数:
        //   logOut:
        //     true:打开日志本地写入记录；
        //     false:关闭日志本地写入记录
        //
        //   consoleOut:
        //     true:控制台输出日志；
        //     false:控制不输出日志；
        //     null:根据底层代码传入的参数来做操作
        //
        //   noticeAsync:
        //     异步通知；
        //     外部传入的 Func 方法；
        //     用于给外部通知日志信息；
        //     此方法常用于日志入库
        //
        //   token:
        //     传播应取消操作的通知
        //
        // 返回结果:
        //     操作结果
        Task<OperateResult> LogOperateSetAsync(bool logOut = true, bool? consoleOut = null, Func<string, LogEventLevel, string?, Exception?, Task>? noticeAsync = null, CancellationToken token = default(CancellationToken));

        //
        // 摘要:
        //     日志操作参数设置；
        //     logOut为false时,consoleOut将不生效
        //
        // 参数:
        //   logOut:
        //     true:打开日志本地写入记录；
        //     false:关闭日志本地写入记录
        //
        //   consoleOut:
        //     true:控制台输出日志；
        //     false:控制不输出日志；
        //     null:根据底层代码传入的参数来做操作
        //
        //   notice:
        //     通知；
        //     外部传入的 Action 方法；
        //     用于给外部通知日志信息；
        //     此方法常用于日志入库
        //
        // 返回结果:
        //     操作结果
        OperateResult LogOperateSet(bool logOut = true, bool? consoleOut = null, Action<string, LogEventLevel, string?, Exception?>? notice = null);

        //
        // 摘要:
        //     日志操作参数设置；
        //     logOut为false时,consoleOut将不生效
        //
        // 参数:
        //   logOut:
        //     true:打开日志本地写入记录；
        //     false:关闭日志本地写入记录
        //
        //   consoleOut:
        //     true:控制台输出日志；
        //     false:控制不输出日志；
        //     null:根据底层代码传入的参数来做操作
        //
        //   notice:
        //     通知；
        //     外部传入的 Action 方法；
        //     用于给外部通知日志信息；
        //     此方法常用于日志入库
        //
        //   token:
        //     传播应取消操作的通知
        //
        // 返回结果:
        //     操作结果
        Task<OperateResult> LogOperateSetAsync(bool logOut = true, bool? consoleOut = null, Action<string, LogEventLevel, string?, Exception?>? notice = null, CancellationToken token = default(CancellationToken));

        //
        // 摘要:
        //     日志操作参数设置；
        //     logOut为false时,consoleOut将不生效
        //
        // 参数:
        //   logOut:
        //     true:打开日志本地写入记录；
        //     false:关闭日志本地写入记录
        //
        //   consoleOut:
        //     true:控制台输出日志；
        //     false:控制不输出日志；
        //     null:根据底层代码传入的参数来做操作
        //
        // 返回结果:
        //     操作结果
        OperateResult LogOperateSet(bool logOut = true, bool? consoleOut = null);

        //
        // 摘要:
        //     日志操作参数设置；
        //     logOut为false时,consoleOut将不生效
        //
        // 参数:
        //   logOut:
        //     true:打开日志本地写入记录；
        //     false:关闭日志本地写入记录
        //
        //   consoleOut:
        //     true:控制台输出日志；
        //     false:控制不输出日志；
        //     null:根据底层代码传入的参数来做操作
        //
        //   token:
        //     传播应取消操作的通知
        //
        // 返回结果:
        //     操作结果
        Task<OperateResult> LogOperateSetAsync(bool logOut = true, bool? consoleOut = null, CancellationToken token = default(CancellationToken));

        //
        // 摘要:
        //     日志操作参数设置
        //
        // 参数:
        //   logModel:
        //     日志数据
        //
        // 返回结果:
        //     操作结果
        OperateResult LogOperateSet(LogModel logModel);

        //
        // 摘要:
        //     日志操作参数设置
        //
        // 参数:
        //   logModel:
        //     日志数据
        //
        //   token:
        //     传播应取消操作的通知
        //
        // 返回结果:
        //     操作结果
        Task<OperateResult> LogOperateSetAsync(LogModel logModel, CancellationToken token = default(CancellationToken));

        //
        // 摘要:
        //     日志操作参数获取
        //
        // 返回结果:
        //     操作结果
        OperateResult LogOperateGet();

        //
        // 摘要:
        //     日志操作参数获取
        //
        // 参数:
        //   token:
        //     传播应取消操作的通知
        //
        // 返回结果:
        //     操作结果
        Task<OperateResult> LogOperateGetAsync(CancellationToken token = default(CancellationToken));
    }
}
