using Demo.Model.@interface;
using FuX.Core.extend;
using FuX.Model.data;

namespace Demo.Core.@abstract
{
    /// <summary>
    /// CCD 抽象类 底层通信抽象类；<br/> 
    /// 外部必须实现抽象方法；<br/> 
    /// 实现接口的异步方法 
    /// <typeparam name="O">操作类</typeparam>
    /// <typeparam name="D">基础数据类，构造参数类</typeparam>
    /// </summary>
    public abstract class CCDAbstract<O, D> : CoreUnify<O, D>, ICCD
        where O : class
        where D : class
    {
        /// <summary>
        /// 无惨构造函数
        /// </summary>
        public CCDAbstract() : base() { }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="param">参数</param>
        public CCDAbstract(D param) : base(param) { }

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
        /// <inheritdoc/>
        public abstract OperateResult Gather(int value);
        /// <inheritdoc/>
        public async Task<OperateResult> GatherAsync(int value, CancellationToken token = default)
            => await Task.Run(() => Gather(value), token);
        /// <inheritdoc/>
        public abstract OperateResult GetTemperature();
        /// <inheritdoc/>
        public async Task<OperateResult> GetTemperatureAsync(CancellationToken token = default)
            => await Task.Run(() => GetTemperature(), token);
        /// <inheritdoc/>
        public abstract OperateResult SetTemperature(long value);
        /// <inheritdoc/>
        public async Task<OperateResult> SetTemperatureAsync(long value, CancellationToken token = default)
            => await Task.Run(() => SetTemperature(value), token);

        /// <inheritdoc/>
        public abstract OperateResult GetStatus();

        /// <inheritdoc/>
        public async Task<OperateResult> GetStatusAsync(CancellationToken token = default)
            => await Task.Run(() => GetStatus(), token);

        /// <inheritdoc/>
        public abstract OperateResult Off(bool hardClose = false);

        /// <inheritdoc/>
        public async Task<OperateResult> OffAsync(bool hardClose = false, CancellationToken token = default)
            => await Task.Run(() => Off(hardClose), token);

        /// <inheritdoc/>
        public abstract OperateResult On();

        /// <inheritdoc/>
        public async Task<OperateResult> OnAsync(CancellationToken token = default)
            => await Task.Run(() => On(), token);
    }
}
