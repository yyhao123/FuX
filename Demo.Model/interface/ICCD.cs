using FuX.Model.data;
using FuX.Model.@interface;

namespace Demo.Model.@interface
{
    /// <summary>
    /// CCD相机接口
    /// </summary>
    public interface ICCD : IOn, IOff, IGetStatus, ILanguage, IDisposable
    {
        /// <summary>
        /// 采集
        /// </summary>
        /// <param name="value">积分时间毫秒</param>
        /// <returns>统一结果</returns>
        OperateResult Gather(int value);
        /// <summary>
        /// 异步采集
        /// </summary>
        /// <param name="value">积分时间毫秒</param>
        /// <param name="token">传播消息取消通知</param>
        /// <returns>统一结果</returns>
        Task<OperateResult> GatherAsync(int value, CancellationToken token = default);


        /// <summary>
        /// 设置温度
        /// </summary>
        /// <param name="value">温度</param>
        /// <returns>统一结果</returns>
        OperateResult SetTemperature(long value);

        /// <summary>
        /// 异步设置温度
        /// </summary>
        /// <param name="value">温度</param>
        /// <param name="token">传播消息取消通知</param>
        /// <returns>统一结果</returns>
        Task<OperateResult> SetTemperatureAsync(long value, CancellationToken token = default);


        /// <summary>
        /// 获取温度
        /// </summary>
        /// <returns>统一结果</returns>
        OperateResult GetTemperature();

        /// <summary>
        /// 异步获取温度
        /// </summary>
        /// <param name="token">传播消息取消通知</param>
        /// <returns>统一结果</returns>
        Task<OperateResult> GetTemperatureAsync(CancellationToken token = default);
    }
}
