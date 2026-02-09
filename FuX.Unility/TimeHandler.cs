using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Unility
{
//
// 摘要:
//     时间处理
public class TimeHandler
    {
        //
        // 摘要:
        //     直接操作底层，性能消耗更小
        internal struct ValueStopwatch
        {
            private readonly long _startTimestamp;

            public bool IsActive => _startTimestamp != 0;

            private ValueStopwatch(long startTimestamp)
            {
                _startTimestamp = startTimestamp;
            }

            public static ValueStopwatch StartNew()
            {
                return new ValueStopwatch(Stopwatch.GetTimestamp());
            }

            public TimeSpan GetElapsedTime()
            {
                if (!IsActive)
                {
                    throw new InvalidOperationException("An uninitialized, or 'default', ValueStopwatch cannot be used to get elapsed time.");
                }

                long timestamp = Stopwatch.GetTimestamp();
                return Stopwatch.GetElapsedTime(_startTimestamp, timestamp);
            }
        }

        //
        // 摘要:
        //     锁
        private static readonly object Lock = new object();

        //
        // 摘要:
        //     自身对象集合
        private static readonly ConcurrentDictionary<string, TimeHandler> ThisObjList = new ConcurrentDictionary<string, TimeHandler>();

        //
        // 摘要:
        //     开关
        private ValueStopwatch stopwatch;

        private static Stopwatch stopTime = new Stopwatch();

        //
        // 摘要:
        //     标识符
        private string SN { get; set; }

        //
        // 摘要:
        //     有参构造函数
        //
        // 参数:
        //   SN:
        public TimeHandler(string SN)
        {
            this.SN = SN;
        }

        //
        // 摘要:
        //     单例模式
        public static TimeHandler Instance(string sn)
        {
            string sn2 = sn;
            TimeHandler value = ThisObjList.FirstOrDefault<KeyValuePair<string, TimeHandler>>((KeyValuePair<string, TimeHandler> c) => c.Value.SN.Comparer(sn2).result).Value;
            if (value == null)
            {
                lock (Lock)
                {
                    TimeHandler exp2 = new TimeHandler(sn2);
                    ThisObjList.AddOrUpdate(sn2, exp2, (string k, TimeHandler v) => exp2);
                    return exp2;
                }
            }

            return value;
        }

        //
        // 摘要:
        //     开始记录
        public void StartRecord()
        {
            stopwatch = ValueStopwatch.StartNew();
        }

        //
        // 摘要:
        //     停止记录
        //
        // 返回结果:
        //     时，分，秒，毫秒
        public (int hours, int minutes, int seconds, int milliseconds) StopRecord()
        {
            TimeSpan elapsedTime = stopwatch.GetElapsedTime();
            int item = Math.Round(elapsedTime.TotalHours).ToInt();
            int item2 = Math.Round(elapsedTime.TotalMinutes).ToInt();
            int item3 = Math.Round(elapsedTime.TotalSeconds).ToInt();
            int item4 = Math.Ceiling(elapsedTime.TotalMilliseconds).ToInt();
            return (hours: item, minutes: item2, seconds: item3, milliseconds: item4);
        }

        //
        // 摘要:
        //     微秒延时
        //
        // 参数:
        //   time:
        //     延时时间,单位:ms
        public static void DelayUs(double time)
        {
            stopTime.Start();
            while (stopTime.Elapsed.TotalMilliseconds < time)
            {
            }

            stopTime.Stop();
            stopTime.Reset();
        }
    }
}
