using FuX.Core.extend;
using FuX.Model.data;
using FuX.Model.@interface;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Core.@abstract;
//
// 摘要:
//     底层通信抽象类；
//     外部必须实现抽象方法；
//     实现接口的异步方法；
//     继承核心抽象类
//
// 类型参数:
//   O:
//     操作类
//
//   D:
//     基础数据类，构造参数类
public abstract class CommunicationAbstract<O, D> : CoreUnify<O, D>, ICommunication, IOn, IOff, ISend, ISendWait, IGetObject, IGetStatus, IEvent, ICreateInstance, ILog, IGetParam, ILanguage, IDisposable where O : class where D : class
{
    //
    // 摘要:
    //     无惨构造函数
    protected CommunicationAbstract()
    {
    }

    //
    // 摘要:
    //     有参构造函数
    //
    // 参数:
    //   param:
    //     参数
    protected CommunicationAbstract(D param)
        : base(param)
    {
    }

    public override void Dispose()
    {
        Off(hardClose: true);
        base.Dispose();
    }

    public override async Task DisposeAsync()
    {
        await OffAsync(hardClose: true);
        await base.DisposeAsync();
    }

    public abstract OperateResult GetBaseObject();

    public abstract OperateResult GetStatus();

    public abstract OperateResult Off(bool hardClose = false);

    public abstract OperateResult On();

    public abstract OperateResult Send(byte[] data);

    public abstract OperateResult SendWait(byte[] data, CancellationToken token);

    

    public async Task<OperateResult> GetBaseObjectAsync(CancellationToken token = default(CancellationToken))
    {
        return await Task.Run(() => GetBaseObject(), token);
    }

    public async Task<OperateResult> GetStatusAsync(CancellationToken token = default(CancellationToken))
    {
        return await Task.Run(() => GetStatus(), token);
    }

    public async Task<OperateResult> OffAsync(bool hardClose = false, CancellationToken token = default(CancellationToken))
    {
        return await Task.Run(() => Off(hardClose), token);
    }

    public async Task<OperateResult> OnAsync(CancellationToken token = default(CancellationToken))
    {
        return await Task.Run(() => On(), token);
    }

    public async Task<OperateResult> SendAsync(byte[] data, CancellationToken token = default(CancellationToken))
    {
        byte[] data2 = data;
        return await Task.Run(() => Send(data2), token);
    }

    public async Task<OperateResult> SendWaitAsync(byte[] data, CancellationToken token)
    {
        byte[] data2 = data;
        return await Task.Run(() => SendWait(data2, token), token);
    }
}
