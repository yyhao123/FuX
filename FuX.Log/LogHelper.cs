using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Log
{
    //
    // 摘要:
    //     日志帮助类
    public class LogHelper
    {
        //
        // 摘要:
        //     日志底层
        private static LogCore logCore = new LogCore();

        //
        // 摘要:
        //     设置参数
        //
        // 参数:
        //   logModel:
        //     日志参数
        public static void Set(LogModel logModel)
        {
            logCore.Set(logModel);
        }

        //
        // 摘要:
        //     获取参数
        //
        // 返回结果:
        //     日志数据
        public static LogModel Get()
        {
            return logCore.Get();
        }

        //
        // 摘要:
        //     详细信息
        //
        // 参数:
        //   info:
        //     信息
        //
        //   filename:
        //     文件名
        //
        //   exception:
        //     异常对象
        //
        //   consoleShow:
        //     控制台显示
        public static void Verbose(string info, string? filename = null, Exception? exception = null, bool consoleShow = true)
        {
            logCore.Records(info, LogEventLevel.Verbose, filename, exception, consoleShow);
        }

        //
        // 摘要:
        //     异步详细信息
        //
        // 参数:
        //   info:
        //     信息
        //
        //   filename:
        //     文件名
        //
        //   exception:
        //     异常对象
        //
        //   consoleShow:
        //     控制台显示
        //
        //   token:
        //     传播应取消操作的通知
        public static async Task VerboseAsync(string info, string? filename = null, Exception? exception = null, bool consoleShow = true, CancellationToken token = default(CancellationToken))
        {
            string info2 = info;
            string filename2 = filename;
            Exception exception2 = exception;
            await Task.Run(delegate
            {
                Verbose(info2, filename2, exception2, consoleShow);
            }, token);
        }

        //
        // 摘要:
        //     调试
        //
        // 参数:
        //   info:
        //     信息
        //
        //   filename:
        //     文件名
        //
        //   exception:
        //     异常对象
        //
        //   consoleShow:
        //     控制台显示
        public static void Debug(string info, string? filename = null, Exception? exception = null, bool consoleShow = true)
        {
            logCore.Records(info, LogEventLevel.Debug, filename, exception, consoleShow);
        }

        //
        // 摘要:
        //     异步调试
        //
        // 参数:
        //   info:
        //     信息
        //
        //   filename:
        //     文件名
        //
        //   exception:
        //     异常对象
        //
        //   consoleShow:
        //     控制台显示
        //
        //   token:
        //     传播应取消操作的通知
        public static async Task DebugAsync(string info, string? filename = null, Exception? exception = null, bool consoleShow = true, CancellationToken token = default(CancellationToken))
        {
            string info2 = info;
            string filename2 = filename;
            Exception exception2 = exception;
            await Task.Run(delegate
            {
                Debug(info2, filename2, exception2, consoleShow);
            }, token);
        }

        //
        // 摘要:
        //     信息
        //
        // 参数:
        //   info:
        //     信息
        //
        //   filename:
        //     文件名
        //
        //   exception:
        //     异常对象
        //
        //   consoleShow:
        //     控制台显示
        public static void Info(string info, string? filename = null, Exception? exception = null, bool consoleShow = true)
        {
            logCore.Records(info, LogEventLevel.Information, filename, exception, consoleShow);
        }

        //
        // 摘要:
        //     异步信息
        //
        // 参数:
        //   info:
        //     信息
        //
        //   filename:
        //     文件名
        //
        //   exception:
        //     异常对象
        //
        //   consoleShow:
        //     控制台显示
        //
        //   token:
        //     传播应取消操作的通知
        public static async Task InfoAsync(string info, string? filename = null, Exception? exception = null, bool consoleShow = true, CancellationToken token = default(CancellationToken))
        {
            string info2 = info;
            string filename2 = filename;
            Exception exception2 = exception;
            await Task.Run(delegate
            {
                Info(info2, filename2, exception2, consoleShow);
            }, token);
        }

        //
        // 摘要:
        //     警告
        //
        // 参数:
        //   info:
        //     信息
        //
        //   filename:
        //     文件名
        //
        //   exception:
        //     异常对象
        //
        //   consoleShow:
        //     控制台显示
        public static void Warning(string info, string? filename = null, Exception? exception = null, bool consoleShow = true)
        {
            logCore.Records(info, LogEventLevel.Warning, filename, exception, consoleShow);
        }

        //
        // 摘要:
        //     异步警告
        //
        // 参数:
        //   info:
        //     信息
        //
        //   filename:
        //     文件名
        //
        //   exception:
        //     异常对象
        //
        //   consoleShow:
        //     控制台显示
        //
        //   token:
        //     传播应取消操作的通知
        public static async Task WarningAsync(string info, string? filename = null, Exception? exception = null, bool consoleShow = true, CancellationToken token = default(CancellationToken))
        {
            string info2 = info;
            string filename2 = filename;
            Exception exception2 = exception;
            await Task.Run(delegate
            {
                Warning(info2, filename2, exception2, consoleShow);
            }, token);
        }

        //
        // 摘要:
        //     异常或错误
        //
        // 参数:
        //   info:
        //     信息
        //
        //   filename:
        //     文件名
        //
        //   exception:
        //     异常对象
        //
        //   consoleShow:
        //     控制台显示
        public static void Error(string info, string? filename = null, Exception? exception = null, bool consoleShow = true)
        {
            logCore.Records(info, LogEventLevel.Error, filename, exception, consoleShow);
        }

        //
        // 摘要:
        //     异步异常或错误
        //
        // 参数:
        //   info:
        //     信息
        //
        //   filename:
        //     文件名
        //
        //   exception:
        //     异常对象
        //
        //   consoleShow:
        //     控制台显示
        //
        //   token:
        //     传播应取消操作的通知
        public static async Task ErrorAsync(string info, string? filename = null, Exception? exception = null, bool consoleShow = true, CancellationToken token = default(CancellationToken))
        {
            string info2 = info;
            string filename2 = filename;
            Exception exception2 = exception;
            await Task.Run(delegate
            {
                Error(info2, filename2, exception2, consoleShow);
            }, token);
        }

        //
        // 摘要:
        //     致命错误或异常
        //
        // 参数:
        //   info:
        //     信息
        //
        //   filename:
        //     文件名
        //
        //   exception:
        //     异常对象
        //
        //   consoleShow:
        //     控制台显示
        public static void Fatal(string info, string? filename = null, Exception? exception = null, bool consoleShow = true)
        {
            logCore.Records(info, LogEventLevel.Fatal, filename, exception, consoleShow);
        }

        //
        // 摘要:
        //     异步致命错误或异常
        //
        // 参数:
        //   info:
        //     信息
        //
        //   filename:
        //     文件名
        //
        //   exception:
        //     异常对象
        //
        //   consoleShow:
        //     控制台显示
        //
        //   token:
        //     传播应取消操作的通知
        public static async Task FatalAsync(string info, string? filename = null, Exception? exception = null, bool consoleShow = true, CancellationToken token = default(CancellationToken))
        {
            string info2 = info;
            string filename2 = filename;
            Exception exception2 = exception;
            await Task.Run(delegate
            {
                Fatal(info2, filename2, exception2, consoleShow);
            }, token);
        }
    }
}
