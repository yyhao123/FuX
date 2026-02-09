using Demo.Model.@interface;
using FuX.Core.extend;
using FuX.Model.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.@abstract
{
    /// <summary>
    /// ATP系列抽象类 底层通信抽象类；<br/> 
    /// 外部必须实现抽象方法；<br/> 
    /// 实现接口的异步方法 
    /// <typeparam name="O">操作类</typeparam>
    /// <typeparam name="D">基础数据类，构造参数类</typeparam>
    /// </summary>
    public abstract class ATPAbstract<O, D> : CoreUnify<O, D>,IATP
        where O : class
        where D : class
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ATPAbstract() : base() { }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="param"></param>
        public ATPAbstract(D param) : base(param) { }

        

        /// <inheritdoc/>
        public override void Dispose()
        {
            Off(true);
            base.Dispose();
        }

        /// <inheritdoc/>
        public override async Task DisposeAsync()
        {
            await OffAsync(true);
            await base.DisposeAsync();
        }

        #region 硬件命令

        public abstract OperateResult ComSerialPortAsk(byte cmd, string devname, byte[] bytes = null);
        public abstract OperateResult ComSerialPortAsk(byte cmd, byte[] bytes = null);
        public abstract OperateResult ComSerialPortAsk(byte cmd, string devname, object bytes = null, string tip = "");
        public abstract OperateResult ComSerialPortAsk(byte cmd, object bytes = null, string tip = "");
        public abstract OperateResult ComSerialPortAsk(string cmd, byte[] bytes = null);
        public abstract OperateResult ComSerialPortAsk(string cmd, byte[] bytes = null, string tip = "");
        public abstract OperateResult ComSerialPortAsk(string cmd, string devname, byte[] bytes = null);
        public abstract OperateResult ComSerialPortAsk(string cmd, string devname, object bytes = null, string tip = "");
        public async Task<OperateResult> ComSerialPortAskAsync(byte cmd, string devname, byte[] bytes = null, CancellationToken token = default)=> await Task.Run(()=>ComSerialPortAsk(cmd,devname,bytes),token);
        public async Task<OperateResult> ComSerialPortAskAsync(byte cmd, byte[] bytes = null, CancellationToken token = default) => await Task.Run(() => ComSerialPortAsk(cmd, bytes), token);
        public async Task<OperateResult> ComSerialPortAskAsync(byte cmd, string devname, object bytes = null, string tip = "", CancellationToken token = default)=> await Task.Run(() => ComSerialPortAsk(cmd, devname, bytes,tip), token);
        public async Task<OperateResult> ComSerialPortAskAsync(byte cmd, object bytes = null, string tip = "", CancellationToken token = default) => await Task.Run(() => ComSerialPortAsk(cmd, bytes,tip), token);
        public async Task<OperateResult> ComSerialPortAskAsync(string cmd, byte[] bytes = null, CancellationToken token = default) => await Task.Run(() => ComSerialPortAsk(cmd, bytes), token);
        public async Task<OperateResult> ComSerialPortAskAsync(string cmd, byte[] bytes = null, string tip = "", CancellationToken token = default) => await Task.Run(() => ComSerialPortAsk(cmd,  bytes,tip), token);
        public async Task<OperateResult> ComSerialPortAskAsync(string cmd, string devname, byte[] bytes = null, CancellationToken token = default)=> await Task.Run(() => ComSerialPortAsk(cmd, devname, bytes), token);
        public async Task<OperateResult> ComSerialPortAskAsync(string cmd, string devname, object bytes = null, string tip = "", CancellationToken token = default)=> await Task.Run(() => ComSerialPortAsk(cmd, devname, bytes,tip), token);

        #endregion

        #region 通信命令

        /// <inheritdoc/>
        public abstract OperateResult GetStatus();

        /// <inheritdoc/>
        public async Task<OperateResult> GetStatusAsync(CancellationToken token = default) => await Task.Run(() => GetStatus(), token);

        /// <inheritdoc/>
        public abstract OperateResult Off(bool hardClose = false);

        /// <inheritdoc/>
        public async Task<OperateResult> OffAsync(bool hardClose = false, CancellationToken token = default) => await Task.Run(() => Off(hardClose), token);

        /// <inheritdoc/>
        public abstract OperateResult On();

        /// <inheritdoc/>
        public async Task<OperateResult> OnAsync(CancellationToken token = default) => await Task.Run(() => On(), token);

        #endregion 通信命令
    }

}
