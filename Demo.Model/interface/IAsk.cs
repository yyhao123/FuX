using FuX.Model.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.@interface
{
    public interface IAsk
    {
        /// <summary>
        /// 通用串口请求
        /// </summary>
        /// <param name="cmd">指令</param>
        /// <param name="devname">设备名</param>
        /// <param name="bytes">数据，没有可不传</param>
        /// <returns></returns>
        OperateResult ComSerialPortAsk(byte cmd, string devname, byte[] bytes = null);

        /// <inheritdoc/>
        Task<OperateResult> ComSerialPortAskAsync(byte cmd, string devname, byte[] bytes = null, CancellationToken token = default);

        /// <summary>
        /// 通用串口请求(直接输入byte)
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <param name="bytes">请求数据</param>
        /// <returns></returns>
        OperateResult ComSerialPortAsk(byte cmd, byte[] bytes = null);

        Task<OperateResult> ComSerialPortAskAsync(byte cmd, byte[] bytes = null, CancellationToken token = default);

        /// <summary>
        /// 通用串口请求(输入数值)
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <param name="devname">设备名称</param>
        /// <param name="bytes">请求数据</param>
        /// <param name="tip">标注</param>
        /// <returns></returns>
        OperateResult ComSerialPortAsk(byte cmd, string devname, object bytes = null, string tip = "");

        Task<OperateResult> ComSerialPortAskAsync(byte cmd, string devname, object bytes = null, string tip = "", CancellationToken token = default);

        /// <summary>
        /// 通用串口请求(输入数值)
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="bytes">数据位有则填</param>
        /// <param name="tip">标识位有则填</param>
        /// <returns></returns>
        OperateResult ComSerialPortAsk(byte cmd, object bytes = null, string tip = "");

        Task<OperateResult> ComSerialPortAskAsync(byte cmd, object bytes = null, string tip = "", CancellationToken token = default);

        /// <summary>
        /// 通用串口请求(别名模式)
        /// </summary>
        /// <param name="cmd">别名</param>
        /// <param name="bytes">请求数据</param>
        /// <returns></returns>
        OperateResult ComSerialPortAsk(string cmd, byte[] bytes = null);

        Task<OperateResult> ComSerialPortAskAsync(string cmd, byte[] bytes = null,  CancellationToken token = default);

        /// <summary>
        /// 通用串口请求(别名模式)
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="bytes">数据位有则填</param>
        /// <param name="tip">标识位有则填</param>
        /// <returns></returns>
        OperateResult ComSerialPortAsk(string cmd, byte[] bytes = null, string tip = "");

        Task<OperateResult> ComSerialPortAskAsync(string cmd, byte[] bytes = null, string tip = "", CancellationToken token = default);

        /// <summary>
        /// 通用串口请求(别名模式)
        /// </summary>
        /// <param name="cmd">别名</param>
        /// <param name="devname">设备名称</param>
        /// <param name="bytes">请求数据</param>
        /// <returns></returns>
        OperateResult ComSerialPortAsk(string cmd, string devname, byte[] bytes = null);

        Task<OperateResult> ComSerialPortAskAsync(string cmd, string devname, byte[] bytes = null, CancellationToken token = default);

        /// <summary>
        /// 通用串口请求(别名模式)
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="devname">设备名称</param>
        /// <param name="bytes">数据位有则填</param>
        /// <param name="tip">标识位有则填</param>
        /// <returns></returns>
        OperateResult ComSerialPortAsk(string cmd, string devname, object bytes = null, string tip = "");

        Task<OperateResult> ComSerialPortAskAsync(string cmd, string devname, object bytes = null, string tip = "", CancellationToken token = default);

    }
}
