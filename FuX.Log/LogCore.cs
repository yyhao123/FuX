using Serilog.Configuration;
using Serilog.Events;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Serilog.Sinks.SystemConsole.Themes;

namespace FuX.Log
{
    //
    // 摘要:
    //     日志核心类
    public class LogCore : IDisposable
    {
        //
        // 摘要:
        //     日志数据
        private LogModel logModel;

        //
        // 摘要:
        //     日志容器
        private ConcurrentDictionary<(string file, bool consoleShow), ILogger> logIoc = new ConcurrentDictionary<(string, bool), ILogger>();

        //
        // 摘要:
        //     无参构造函数
        public LogCore()
        {
            if (logModel == null)
            {
                logModel = new LogModel();
            }

            HistoryLogDelete();
        }

        //
        // 摘要:
        //     记录
        //
        // 参数:
        //   info:
        //     信息
        //
        //   type:
        //     类型
        //
        //   filename:
        //     文件名称
        //
        //   exception:
        //     异常对象
        //
        //   consoleOut:
        //     控制台显示
        public void Records(string info, LogEventLevel type, string? filename = null, Exception? exception = null, bool consoleOut = true)
        {
            if (!logModel.Out)
            {
                return;
            }

            if (logModel.ConsoleOut.HasValue)
            {
                consoleOut = logModel.ConsoleOut.Value;
            }

            if (string.IsNullOrWhiteSpace(filename))
            {
                filename = logModel.FileName;
            }

            if (!logIoc.ContainsKey((filename, consoleOut)))
            {
                string empty = string.Empty;
                string empty2 = string.Empty;
                string[] array = filename.Split('.');
                if (array.Length == 2)
                {
                    empty = array[0];
                    empty2 = "." + array[1];
                }
                else
                {
                    string text = string.Empty;
                    for (int i = 0; i < array.Length - 1; i++)
                    {
                        text = ((i != array.Length - 2) ? (text + array[i] + ".") : (text + array[i]));
                    }

                    empty = text;
                    empty2 = "." + array[^1];
                }

                if (!empty.Contains(".") && empty.Any((char c) => char.IsUpper(c)))
                {
                    empty = Regex.Replace(empty, "(\\B[A-Z])", ".$1");
                }

                string outputTemplate = "{Timestamp:" + logModel.InfoTimeFormat + "} | {Level:u3} | {Message:lj}{NewLine}{Exception}";
                string value = $"{logModel.FileLocation}{logModel.FolderName}/{DateTime.Now.ToString("yyyy-MM-dd")}/";
                string path = $"{value}/{empty.ToLower()}/{empty2.ToLower()}";
                RollingInterval rollingInterval = RollingInterval.Hour;
                bool flag = true;
                bool flag2 = true;
                ConsoleTheme colored = SystemConsoleTheme.Colored;
                if (consoleOut)
                {
                    LoggerSinkConfiguration writeTo = new LoggerConfiguration().MinimumLevel.Verbose().WriteTo;
                    bool applyThemeToRedirectedOutput = flag2;
                    ConsoleTheme theme = colored;
                    LoggerSinkConfiguration writeTo2 = writeTo.Console(LogEventLevel.Verbose, outputTemplate, null, null, null, theme, applyThemeToRedirectedOutput).WriteTo;
                    RollingInterval rollingInterval2 = rollingInterval;
                    bool shared = flag;
                    Serilog.Log.Logger = writeTo2.File(path, LogEventLevel.Verbose, outputTemplate, null, 1073741824L, null, buffered: false, shared, null, rollingInterval2, rollOnFileSizeLimit: false, 31).CreateLogger();
                }
                else
                {
                    LoggerSinkConfiguration writeTo3 = new LoggerConfiguration().MinimumLevel.Verbose().WriteTo;
                    RollingInterval rollingInterval2 = rollingInterval;
                    bool shared = flag;
                    Serilog.Log.Logger = writeTo3.File(path, LogEventLevel.Verbose, outputTemplate, null, 1073741824L, null, buffered: false, shared, null, rollingInterval2, rollOnFileSizeLimit: false, 31).CreateLogger();
                }

                logIoc.AddOrUpdate((filename, consoleOut), Serilog.Log.Logger, ((string file, bool consoleShow) k, ILogger v) => Serilog.Log.Logger);
            }

            switch (type)
            {
                case LogEventLevel.Verbose:
                    if (exception != null)
                    {
                        logIoc[(filename, consoleOut)].Verbose(exception, info);
                    }
                    else
                    {
                        logIoc[(filename, consoleOut)].Verbose(info);
                    }

                    break;
                case LogEventLevel.Debug:
                    if (exception != null)
                    {
                        logIoc[(filename, consoleOut)].Debug(exception, info);
                    }
                    else
                    {
                        logIoc[(filename, consoleOut)].Debug(info);
                    }

                    break;
                case LogEventLevel.Information:
                    if (exception != null)
                    {
                        logIoc[(filename, consoleOut)].Information(exception, info);
                    }
                    else
                    {
                        logIoc[(filename, consoleOut)].Information(info);
                    }

                    break;
                case LogEventLevel.Warning:
                    if (exception != null)
                    {
                        logIoc[(filename, consoleOut)].Warning(exception, info);
                    }
                    else
                    {
                        logIoc[(filename, consoleOut)].Warning(info);
                    }

                    break;
                case LogEventLevel.Error:
                    if (exception != null)
                    {
                        logIoc[(filename, consoleOut)].Error(exception, info);
                    }
                    else
                    {
                        logIoc[(filename, consoleOut)].Error(info);
                    }

                    break;
                case LogEventLevel.Fatal:
                    if (exception != null)
                    {
                        logIoc[(filename, consoleOut)].Fatal(exception, info);
                    }
                    else
                    {
                        logIoc[(filename, consoleOut)].Fatal(info);
                    }

                    break;
            }

            logModel.Notice?.Invoke(info, type, filename, exception);
            logModel.NoticeAsync?.Invoke(info, type, filename, exception);
        }

        //
        // 摘要:
        //     历史日志删除
        public void HistoryLogDelete()
        {
            try
            {
                string text = logModel.FileLocation + logModel.FolderName;
                if (!Directory.Exists(text))
                {
                    return;
                }

                string[] directories = Directory.GetDirectories(text);
                foreach (string text2 in directories)
                {
                    DateTime dateTime = Convert.ToDateTime(text2.Replace(text, string.Empty).Replace("\\", "").Replace("/", ""));
                    if ((Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) - dateTime).TotalDays > (double)logModel.HistoryTime)
                    {
                        DeleteFilesInFolder(text2);
                    }
                }
            }
            catch (Exception ex)
            {
                Records("Deleting historical logs is abnormal ：" + ex.Message, LogEventLevel.Error, null, ex);
            }
        }

        //
        // 摘要:
        //     递归删除
        //
        // 参数:
        //   folderPath:
        //     文件夹路径
        public void DeleteFilesInFolder(string folderPath)
        {
            string[] files = Directory.GetFiles(folderPath);
            foreach (string path in files)
            {
                try
                {
                    File.Delete(path);
                }
                catch
                {
                }
            }

            files = Directory.GetDirectories(folderPath);
            foreach (string folderPath2 in files)
            {
                DeleteFilesInFolder(folderPath2);
            }
        }

        //
        // 摘要:
        //     更新参数
        //
        // 参数:
        //   logModel:
        //     日志参数
        public void Set(LogModel logModel)
        {
            this.logModel = logModel;
        }

        //
        // 摘要:
        //     获取参数
        //
        // 返回结果:
        //     日志参数
        public LogModel Get()
        {
            return logModel;
        }

        //
        // 摘要:
        //     释放
        public void Dispose()
        {
            logIoc.Clear();
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
