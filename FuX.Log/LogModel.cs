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
    //     日志数据
    public class LogModel
    {
        //
        // 摘要:
        //     日志的文件名
        public string FileName { get; set; } = "details.log";


        //
        // 摘要:
        //     日志的文件夹名
        public string FolderName { get; set; } = "logs";


        //
        // 摘要:
        //     日期的时间格式
        public string InfoTimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss.fff";


        //
        // 摘要:
        //     日志输出的路径
        public string FileLocation { get; set; } = AppDomain.CurrentDomain.BaseDirectory;


        //
        // 摘要:
        //     删除多少天前的日志
        public int HistoryTime { get; set; } = 7;


        //
        // 摘要:
        //     控制台输出；
        //     null：以传入的参数来进行操作；
        //     true：控制台输出；
        //     false：控制台不输出
        public bool? ConsoleOut { get; set; }

        //
        // 摘要:
        //     日志输出
        //     false 则不记录日志
        public bool Out { get; set; } = true;


        //
        // 摘要:
        //     通知；
        //     外部传入的 Action 方法；
        //     用于给外部通知日志信息；
        //     此方法常用于日志入库
        public Action<string, LogEventLevel, string?, Exception?>? Notice { get; set; }

        //
        // 摘要:
        //     异步通知；
        //     外部传入的 Func 方法；
        //     用于给外部通知日志信息；
        //     此方法常用于日志入库
        public Func<string, LogEventLevel, string?, Exception?, Task>? NoticeAsync { get; set; }
    }
}
