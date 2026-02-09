using FuX.Core.extend;
using FuX.Log;
using FuX.Model.@enum;
using FuX.Model.Specenum;
using FuX.Unility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FuX.Core.virtualAddress
{
    public class VirtualAddress : CoreUnify<VirtualAddress, VirtualAddressData>, IDisposable
    {
        private CancellationTokenSource token;

        private Random random = new Random();

        private object? Value { get; set; }

        public VirtualAddress(VirtualAddressData basics)
            : base(basics)
        {
            try
            {
                if (token == null)
                {
                    token = new CancellationTokenSource();
                }
                string pattern = "\\{([^}]*)\\}";
                Match match = null;
                switch (basics.AddressType)
                {
                    case AddressType.VirtualDynamic_Random:
                        match = Regex.Match(basics.AddressName, pattern);
                        if (match.Success)
                        {
                            动态随机值(int.Parse(match.Groups[1].Value));
                        }
                        else
                        {
                            动态随机值();
                        }
                        break;
                    case AddressType.VirtualDynamic_RandomScope:
                        match = Regex.Match(basics.AddressName, pattern);
                        if (match.Success)
                        {
                            string[] array3 = match.Groups[1].Value.Split(',');
                            string[] array4 = array3[1].Split('^');
                            int interval2 = int.Parse(array3[0]);
                            string min2 = array4[0];
                            string max2 = array4[1];
                            动态随机范围值(min2, max2, interval2);
                        }
                        else
                        {
                            动态随机范围值("", "");
                        }
                        break;
                    case AddressType.VirtualDynamic_Order:
                        match = Regex.Match(basics.AddressName, pattern);
                        if (match.Success)
                        {
                            string[] array5 = match.Groups[1].Value.Split(',');
                            int interval3 = int.Parse(array5[0]);
                            float zzbl2 = float.Parse(array5[1]);
                            动态顺序值(zzbl2, interval3);
                        }
                        else
                        {
                            动态顺序值(0f);
                        }
                        break;
                    case AddressType.VirtualDynamic_OrderScope:
                        match = Regex.Match(basics.AddressName, pattern);
                        if (match.Success)
                        {
                            string[] array = match.Groups[1].Value.Split(',');
                            int interval = int.Parse(array[0]);
                            float zzbl = float.Parse(array[1]);
                            string[] array2 = array[2].Split('^');
                            string min = array2[0];
                            string max = array2[1];
                            动态顺序范围值(min, max, zzbl, interval);
                        }
                        else
                        {
                            动态顺序范围值("", "", 0f);
                        }
                        break;
                    default:
                        LogHelper.Error("虚拟地址 - [ " + basics.AddressName + " ]不支持虚拟地址操作", "VirtualAddress.log");
                        break;
                    case AddressType.VirtualStatic:
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("虚拟地址 - [ " + basics.AddressName + " ]初始化异常：" + ex.Message, "VirtualAddress.log", ex);
            }
        }

        public VirtualAddress()
        {
        }

        public object? Read()
        {
            return Value;
        }

        public bool Write(object value)
        {
            Value = value;
            return true;
        }

        private async Task 动态随机值(int interval = 1000)
        {
            await Task.Run(async delegate
            {
                try
                {
                    while (!token.IsCancellationRequested)
                    {
                        switch (base.basics.DataType)
                        {
                            case DataType.Bool:
                                if (Value == null)
                                {
                                    Value = false;
                                }
                                else
                                {
                                    Value = !(bool)Value;
                                }
                                break;
                            case DataType.String:
                            case DataType.Char:
                                Value = 随机String类型(random.Next(1, 100));
                                break;
                            case DataType.Double:
                                Value = 随机Double类型();
                                break;
                            case DataType.Float:
                            case DataType.Single:
                                Value = 随机Float类型();
                                break;
                            case DataType.Int:
                            case DataType.Int32:
                                Value = 随机Int32类型();
                                break;
                            case DataType.Long:
                            case DataType.Int64:
                                Value = 随机Int64类型();
                                break;
                            case DataType.Short:
                            case DataType.Int16:
                                Value = 随机Int16类型();
                                break;
                            case DataType.Ulong:
                            case DataType.UInt64:
                                Value = 随机UInt64类型(0uL);
                                break;
                            case DataType.Uint:
                            case DataType.UInt32:
                                Value = 随机UInt32类型();
                                break;
                            case DataType.Ushort:
                            case DataType.UInt16:
                                Value = 随机UInt16类型(0);
                                break;
                            case DataType.DateTime:
                            case DataType.Date:
                                Value = 随机日期时间(DateTime.Parse("2000-01-01 00:00:00.000"), DateTime.Now);
                                break;
                            case DataType.Time:
                                Value = 随机时间(DateTime.Parse("2000-01-01 00:00:00.000"), DateTime.Now);
                                break;
                        }
                        await Task.Delay(interval, token.Token);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error("虚拟地址 - [ " + base.basics.AddressName + " ]动态随机值异常：" + ex.Message, "VirtualAddress.log", ex);
                }
            }, token.Token);
        }

        private async Task 动态随机范围值(string min, string max, int interval = 1000)
        {
            string min2 = min;
            string max2 = max;
            await Task.Run(async delegate
            {
                try
                {
                    while (!token.IsCancellationRequested)
                    {
                        switch (base.basics.DataType)
                        {
                            case DataType.Bool:
                                if (Value == null)
                                {
                                    Value = false;
                                }
                                else
                                {
                                    Value = !(bool)Value;
                                }
                                break;
                            case DataType.String:
                            case DataType.Char:
                                Value = 随机String类型(random.Next(1, 100));
                                break;
                            case DataType.Double:
                                Value = 随机Double类型(double.Parse(min2), double.Parse(max2));
                                break;
                            case DataType.Float:
                            case DataType.Single:
                                Value = 随机Float类型(float.Parse(min2), float.Parse(max2));
                                break;
                            case DataType.Int:
                            case DataType.Int32:
                                Value = 随机Int32类型(int.Parse(min2), int.Parse(max2));
                                break;
                            case DataType.Long:
                            case DataType.Int64:
                                Value = 随机Int64类型(long.Parse(min2), long.Parse(max2));
                                break;
                            case DataType.Short:
                            case DataType.Int16:
                                Value = 随机Int16类型(short.Parse(min2), short.Parse(max2));
                                break;
                            case DataType.Ulong:
                            case DataType.UInt64:
                                Value = 随机UInt64类型(ulong.Parse(min2), ulong.Parse(max2));
                                break;
                            case DataType.Uint:
                            case DataType.UInt32:
                                Value = 随机UInt32类型(uint.Parse(min2), uint.Parse(max2));
                                break;
                            case DataType.Ushort:
                            case DataType.UInt16:
                                Value = 随机UInt16类型(ushort.Parse(min2), ushort.Parse(max2));
                                break;
                            case DataType.DateTime:
                            case DataType.Date:
                                Value = 随机日期时间(DateTime.Parse(min2), DateTime.Parse(max2));
                                break;
                            case DataType.Time:
                                Value = 随机时间(DateTime.Parse(min2), DateTime.Parse(max2));
                                break;
                        }
                        await Task.Delay(interval, token.Token);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error("虚拟地址 - [ " + base.basics.AddressName + " ]动态随机范围值异常：" + ex.Message, "VirtualAddress.log", ex);
                }
            }, token.Token);
        }

        private async Task 动态顺序值(float zzbl, int interval = 1000)
        {
            await Task.Run(async delegate
            {
                try
                {
                    while (!token.IsCancellationRequested)
                    {
                        switch (base.basics.DataType)
                        {
                            case DataType.Bool:
                                if (Value == null)
                                {
                                    Value = false;
                                }
                                else
                                {
                                    Value = !(bool)Value;
                                }
                                break;
                            case DataType.String:
                            case DataType.Char:
                                Value = 随机String类型(random.Next(1, 100));
                                break;
                            case DataType.Double:
                                try
                                {
                                    if (Value == null)
                                    {
                                        Value = double.Parse("0");
                                    }
                                    else
                                    {
                                        Value = Value.GetSource<double>() + (double)zzbl;
                                    }
                                }
                                catch
                                {
                                    Value = double.Parse("0");
                                }
                                break;
                            case DataType.Float:
                            case DataType.Single:
                                try
                                {
                                    if (Value == null)
                                    {
                                        Value = float.Parse("0");
                                    }
                                    else
                                    {
                                        Value = Value.GetSource<float>() + zzbl;
                                    }
                                }
                                catch
                                {
                                    Value = float.Parse("0");
                                }
                                break;
                            case DataType.Int:
                            case DataType.Int32:
                                try
                                {
                                    if (Value == null)
                                    {
                                        Value = int.Parse("0");
                                    }
                                    else
                                    {
                                        Value = (int)((float)Value.GetSource<int>() + zzbl);
                                    }
                                }
                                catch
                                {
                                    Value = int.Parse("0");
                                }
                                break;
                            case DataType.Long:
                            case DataType.Int64:
                                try
                                {
                                    if (Value == null)
                                    {
                                        Value = long.Parse("0");
                                    }
                                    else
                                    {
                                        Value = (long)((float)Value.GetSource<long>() + zzbl);
                                    }
                                }
                                catch
                                {
                                    Value = long.Parse("0");
                                }
                                break;
                            case DataType.Short:
                            case DataType.Int16:
                                try
                                {
                                    if (Value == null)
                                    {
                                        Value = short.Parse("0");
                                    }
                                    else
                                    {
                                        Value = (short)((float)Value.GetSource<short>() + zzbl);
                                    }
                                }
                                catch
                                {
                                    Value = short.Parse("0");
                                }
                                break;
                            case DataType.Ulong:
                            case DataType.UInt64:
                                try
                                {
                                    if (Value == null)
                                    {
                                        Value = ulong.Parse("0");
                                    }
                                    else
                                    {
                                        Value = (ulong)((float)Value.GetSource<ulong>() + zzbl);
                                    }
                                }
                                catch
                                {
                                    Value = ulong.Parse("0");
                                }
                                break;
                            case DataType.Uint:
                            case DataType.UInt32:
                                try
                                {
                                    if (Value == null)
                                    {
                                        Value = uint.Parse("0");
                                    }
                                    else
                                    {
                                        Value = (uint)((float)Value.GetSource<uint>() + zzbl);
                                    }
                                }
                                catch
                                {
                                    Value = uint.Parse("0");
                                }
                                break;
                            case DataType.Ushort:
                            case DataType.UInt16:
                                try
                                {
                                    if (Value == null)
                                    {
                                        Value = ushort.Parse("0");
                                    }
                                    else
                                    {
                                        Value = (ushort)((float)(int)Value.GetSource<ushort>() + zzbl);
                                    }
                                }
                                catch
                                {
                                    Value = ushort.Parse("0");
                                }
                                break;
                            case DataType.DateTime:
                            case DataType.Date:
                                try
                                {
                                    if (Value == null)
                                    {
                                        Value = DateTime.Parse("2000-01-01 00:00:00.000");
                                    }
                                    else
                                    {
                                        Value = Value.GetSource<DateTime>().AddSeconds(zzbl);
                                        Value = Value.GetSource<DateTime>().AddDays(zzbl);
                                    }
                                }
                                catch
                                {
                                    Value = DateTime.Parse("2000-01-01 00:00:00.000");
                                }
                                break;
                            case DataType.Time:
                                try
                                {
                                    if (Value == null)
                                    {
                                        Value = DateTime.Parse("2000-01-01 00:00:00.000");
                                    }
                                    else
                                    {
                                        Value = Value.GetSource<DateTime>().AddSeconds(zzbl);
                                    }
                                }
                                catch
                                {
                                    Value = DateTime.Parse("2000-01-01 00:00:00.000");
                                }
                                break;
                        }
                        await Task.Delay(interval, token.Token);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error("虚拟地址 - [ " + base.basics.AddressName + " ]动态顺序值异常：" + ex.Message, "VirtualAddress.log", ex);
                }
            }, token.Token);
        }

        private async Task 动态顺序范围值(string min, string max, float zzbl, int interval = 1000)
        {
            string min2 = min;
            string max2 = max;
            await Task.Run(async delegate
            {
                try
                {
                    while (!token.IsCancellationRequested)
                    {
                        switch (base.basics.DataType)
                        {
                            case DataType.Bool:
                                if (Value == null)
                                {
                                    Value = false;
                                }
                                else
                                {
                                    Value = !(bool)Value;
                                }
                                break;
                            case DataType.String:
                            case DataType.Char:
                                Value = 随机String类型(random.Next(1, 100));
                                break;
                            case DataType.Double:
                                try
                                {
                                    if (Value == null)
                                    {
                                        Value = double.Parse(min2);
                                    }
                                    else if (Value.GetSource<double>() >= double.Parse(max2))
                                    {
                                        Value = double.Parse(min2);
                                    }
                                    else
                                    {
                                        Value = Value.GetSource<double>() + double.Parse(zzbl.ToString());
                                    }
                                }
                                catch
                                {
                                    Value = double.Parse(min2);
                                }
                                break;
                            case DataType.Float:
                            case DataType.Single:
                                try
                                {
                                    if (Value == null)
                                    {
                                        Value = float.Parse(min2);
                                    }
                                    else if (Value.GetSource<float>() >= float.Parse(max2))
                                    {
                                        Value = float.Parse(min2);
                                    }
                                    else
                                    {
                                        Value = Value.GetSource<float>() + float.Parse(zzbl.ToString());
                                    }
                                }
                                catch
                                {
                                    Value = float.Parse(min2);
                                }
                                break;
                            case DataType.Int:
                            case DataType.Int32:
                                try
                                {
                                    if (Value == null)
                                    {
                                        Value = int.Parse(min2);
                                    }
                                    else if (Value.GetSource<int>() >= int.Parse(max2))
                                    {
                                        Value = int.Parse(min2);
                                    }
                                    else
                                    {
                                        Value = Value.GetSource<int>() + int.Parse(zzbl.ToString());
                                    }
                                }
                                catch
                                {
                                    Value = int.Parse(min2);
                                }
                                break;
                            case DataType.Long:
                            case DataType.Int64:
                                try
                                {
                                    if (Value == null)
                                    {
                                        Value = long.Parse(min2);
                                    }
                                    else if (Value.GetSource<long>() >= long.Parse(max2))
                                    {
                                        Value = long.Parse(min2);
                                    }
                                    else
                                    {
                                        Value = Value.GetSource<long>() + long.Parse(zzbl.ToString());
                                    }
                                }
                                catch
                                {
                                    Value = long.Parse(min2);
                                }
                                break;
                            case DataType.Short:
                            case DataType.Int16:
                                try
                                {
                                    if (Value == null)
                                    {
                                        Value = short.Parse(min2);
                                    }
                                    else if (Value.GetSource<short>() >= short.Parse(max2))
                                    {
                                        Value = short.Parse(min2);
                                    }
                                    else
                                    {
                                        Value = Value.GetSource<short>() + short.Parse(zzbl.ToString());
                                    }
                                }
                                catch
                                {
                                    Value = short.Parse(min2);
                                }
                                break;
                            case DataType.Ulong:
                            case DataType.UInt64:
                                try
                                {
                                    if (Value == null)
                                    {
                                        Value = ulong.Parse(min2);
                                    }
                                    else if (Value.GetSource<ulong>() >= ulong.Parse(max2))
                                    {
                                        Value = ulong.Parse(min2);
                                    }
                                    else
                                    {
                                        Value = Value.GetSource<ulong>() + ulong.Parse(zzbl.ToString());
                                    }
                                }
                                catch
                                {
                                    Value = ulong.Parse(min2);
                                }
                                break;
                            case DataType.Uint:
                            case DataType.UInt32:
                                try
                                {
                                    if (Value == null)
                                    {
                                        Value = uint.Parse(min2);
                                    }
                                    else if (Value.GetSource<uint>() >= uint.Parse(max2))
                                    {
                                        Value = uint.Parse(min2);
                                    }
                                    else
                                    {
                                        Value = Value.GetSource<uint>() + uint.Parse(zzbl.ToString());
                                    }
                                }
                                catch
                                {
                                    Value = uint.Parse(min2);
                                }
                                break;
                            case DataType.Ushort:
                            case DataType.UInt16:
                                try
                                {
                                    if (Value == null)
                                    {
                                        Value = ushort.Parse(min2);
                                    }
                                    else if (Value.GetSource<ushort>() >= ushort.Parse(max2))
                                    {
                                        Value = ushort.Parse(min2);
                                    }
                                    else
                                    {
                                        Value = Value.GetSource<ushort>() + ushort.Parse(zzbl.ToString());
                                    }
                                }
                                catch
                                {
                                    Value = ushort.Parse(min2);
                                }
                                break;
                            case DataType.DateTime:
                            case DataType.Date:
                                try
                                {
                                    if (Value == null)
                                    {
                                        Value = DateTime.Parse(min2);
                                    }
                                    else if (Value.GetSource<DateTime>() >= DateTime.Parse(max2))
                                    {
                                        Value = DateTime.Parse(min2);
                                    }
                                    else
                                    {
                                        Value = Value.GetSource<DateTime>().AddSeconds(zzbl);
                                        Value = Value.GetSource<DateTime>().AddDays(zzbl);
                                    }
                                }
                                catch
                                {
                                    Value = DateTime.Parse(min2);
                                }
                                break;
                            case DataType.Time:
                                try
                                {
                                    if (Value == null)
                                    {
                                        Value = DateTime.Parse(min2);
                                    }
                                    else if (Value.GetSource<DateTime>() >= DateTime.Parse(max2))
                                    {
                                        Value = DateTime.Parse(min2);
                                    }
                                    else
                                    {
                                        Value = Value.GetSource<DateTime>().AddSeconds(zzbl);
                                    }
                                }
                                catch
                                {
                                    Value = DateTime.Parse(min2);
                                }
                                break;
                        }
                        await Task.Delay(interval, token.Token);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error("虚拟地址 - [ " + base.basics.AddressName + " ]动态顺序范围值异常：" + ex.Message, "VirtualAddress.log", ex);
                }
            }, token.Token);
        }

        private DateTime 随机时间(DateTime minTime, DateTime maxTime)
        {
            double totalSeconds = (maxTime - minTime).TotalSeconds;
            TimeSpan value = TimeSpan.FromSeconds(random.NextDouble() * totalSeconds);
            return minTime.Add(value);
        }

        private DateTime 随机日期时间(DateTime minTime, DateTime maxTime)
        {
            long ticks = minTime.Ticks;
            long num = maxTime.Ticks - ticks;
            return new DateTime(ticks + (long)(random.NextDouble() * (double)num));
        }

        private short 随机Int16类型(short minValue = short.MinValue, short maxValue = short.MaxValue)
        {
            return (short)random.NextInt64(minValue, maxValue);
        }

        private ushort 随机UInt16类型(ushort minValue = 0, ushort maxValue = ushort.MaxValue)
        {
            return (ushort)random.NextInt64(minValue, maxValue);
        }

        private int 随机Int32类型(int minValue = int.MinValue, int maxValue = int.MaxValue)
        {
            return (int)random.NextInt64(minValue, maxValue);
        }

        private uint 随机UInt32类型(uint minValue = 0u, uint maxValue = uint.MaxValue)
        {
            return (uint)random.NextInt64(minValue, maxValue);
        }

        private long 随机Int64类型(long minValue = long.MinValue, long maxValue = long.MaxValue)
        {
            return random.NextInt64(minValue, maxValue);
        }

        private ulong 随机UInt64类型(ulong minValue = 0uL, ulong maxValue = 9223372036854775807uL)
        {
            return (ulong)random.NextInt64((long)minValue, (long)maxValue);
        }

        private double 随机Double类型(double minValue = -999999.999999, double maxValue = 999999.999999)
        {
            double num = maxValue - minValue;
            double num2 = random.NextDouble();
            double value = minValue + num2 * num;
            value = Math.Round(value, 6);
            if (double.IsInfinity(value))
            {
                value = 随机Double类型(minValue, maxValue);
            }
            return value;
        }

        private float 随机Float类型(float minValue = -1000000f, float maxValue = 1000000f)
        {
            double num = maxValue - minValue;
            double num2 = random.NextDouble();
            float num3 = (float)Math.Round((decimal)((double)minValue + num2 * num), 6);
            if (float.IsInfinity(num3))
            {
                num3 = 随机Float类型(minValue, maxValue);
            }
            return num3;
        }

        private string 随机String类型(int length, string validCharacters = "abcdefghijklmnopqrstuvwxyz0123456789")
        {
            StringBuilder stringBuilder = new StringBuilder();
            while (stringBuilder.Length < length)
            {
                int index = random.Next(validCharacters.Length);
                char value = validCharacters[index];
                stringBuilder.Append(value);
            }
            return stringBuilder.ToString();
        }

        public override void Dispose()
        {
            if (token != null)
            {
                token.Cancel();
                token = null;
            }
            base.Dispose();
        }

        public override async Task DisposeAsync()
        {
            Dispose();
            await base.DisposeAsync();
        }
    }
}
