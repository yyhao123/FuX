using FuX.Core.@abstract;
using FuX.Model.data;
using FuX.Model.@interface;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FuX.Core.Communication.serial
{
    //
    // 摘要:
    //     串口通信
    public class SerialOperate : CommunicationAbstract<SerialOperate, SerialData.Basics>, ICommunication, IOn, IOff, ISend, ISendWait, IGetObject, IGetStatus, IEvent, ICreateInstance, ILog, IGetParam, ILanguage, IDisposable
    {
        //
        // 摘要:
        //     通信库
        private SerialPort? Communication;

        private static bool _IsStop = true;

        public bool IsStop
        {
            get { return _IsStop; }
            set { _IsStop = value; }
        }

        //
        // 摘要:
        //     有参构造函数
        //
        // 参数:
        //   basics:
        //     基础数据
        public SerialOperate(SerialData.Basics basics)
            : base(basics)
        {
        }

        //
        // 摘要:
        //     获取当前在线的串口号
        public static List<string> GetPortArray()
        {
            return SerialPort.GetPortNames().ToList();
        }

        //
        // 摘要:
        //     私有停止监控
        private void StopMonitor()
        {
            if (Communication != null)
            {
                Communication.DataReceived -= Communication_DataReceived;
            }
        }

        //
        // 摘要:
        //     私有启动监控
        private void StartMonitor()
        {
            if (Communication != null)
            {
                Communication.DataReceived -= Communication_DataReceived;
                Communication.DataReceived += Communication_DataReceived;
            }
        }

        public override OperateResult On()
        {
            BegOperate("On");
            try
            {
                if (GetStatus().GetDetails(out string message))
                {
                    return EndOperate(status: false, message, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\serial\\SerialOperate.cs", "On", 66);
                }

                Communication = new SerialPort();
                Communication.PortName = base.basics.PortName;
                Communication.BaudRate = base.basics.BaudRate;
                Communication.Parity = base.basics.ParityBit;
                Communication.DataBits = base.basics.DataBit;
                Communication.StopBits = base.basics.StopBit;
                Communication.WriteTimeout = base.basics.WriteTimeout;
                Communication.ReadTimeout = base.basics.ReadTimeout;
                Communication.ReceivedBytesThreshold = base.basics.ReceivedBytesThreshold;
                Communication.Open();
                StartMonitor();
                return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "D:\\Shunnet\\Demo\\Demo.Core\\communication\\serial\\SerialOperate.cs", "On", 83);
            }
            catch (Exception ex)
            {
                Off(hardClose: true);
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "D:\\Shunnet\\Demo\\Demo.Core\\communication\\serial\\SerialOperate.cs", "On", 88);
            }
        }

        //
        // 摘要:
        //     接收串口数据
        //
        // 参数:
        //   sender:
        //     源
        //
        //   e:
        //     事件
        private void Communication_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] array = new byte[Communication.BytesToRead];
                if (Communication.BaseStream.Read(array, 0, array.Length) > 0)
                {
                    OnDataEventHandler(this, new EventDataResult(status: true, "[" + base.TAG + "]" + GetLanguageValue("监控数据"), array));
                }
            }
            catch (Exception ex)
            {
                OnInfoEventHandler(this, new EventInfoResult(status: false, "[" + base.TAG + "]" + string.Format(GetLanguageValue("监控异常"), ex.Message)));
                Off(hardClose: true);
            }
        }

        public override OperateResult Off(bool hardClose = false)
        {
            BegOperate("Off");
            try
            {
                if (!hardClose && !GetStatus().GetDetails(out string message))
                {
                    return EndOperate(status: false, message, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\serial\\SerialOperate.cs", "Off", 125);
                }

                StopMonitor();
                Communication?.Close();
                Communication?.Dispose();
                Communication = null;
                return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\serial\\SerialOperate.cs", "Off", 133);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\serial\\SerialOperate.cs", "Off", 137);
            }
        }

        public override OperateResult Send(byte[] data)
        {
            BegOperate("Send");
            try
            {
                if (!GetStatus().GetDetails(out string message))
                {
                    return EndOperate(status: false, message, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\serial\\SerialOperate.cs", "Send", 148);
                }

                if (data.Length > base.basics.MaxChunkSize)
                {
                    int maxChunkSize = base.basics.MaxChunkSize;
                    int i;
                    int num;
                    for (i = 0; i < data.Length; i += num)
                    {
                        num = Math.Min(data.Length - i, maxChunkSize);
                        byte[] array = new byte[num];
                        Array.Copy(data, i, array, 0, num);
                        Communication.Write(array, 0, array.Length);
                    }

                    if (i != data.Length)
                    {
                        return EndOperate(status: false, GetLanguageValue("存在数据块发送失败"), null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\serial\\SerialOperate.cs", "Send", 168);
                    }
                }
                else
                {
                    Communication.Write(data, 0, data.Length);
                }

                return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\serial\\SerialOperate.cs", "Send", 175);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\serial\\SerialOperate.cs", "Send", 179);
            }
        }

        public override OperateResult SendWait(byte[] data, CancellationToken token)
        {
            BegOperate("SendWait");
            try
            {
                if (!GetStatus().GetDetails(out string message))
                {
                    return EndOperate(status: false, message, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\serial\\SerialOperate.cs", "SendWait", 190);
                }

                StopMonitor();
                if (Send(data).Status)
                {
                    var receive = new List<byte>();
                    byte[] buffer = new byte[base.basics.BufferSize];
                    var count = ((base.basics.ReadTimeout + 30) / 29) + 1;
                    var index = 0;
                    while (true)
                    {
                        Task.Delay(29).Wait();
                        var byteToRead = Communication.BytesToRead;
                        var readBuffer = new byte[byteToRead];

                        var readCount = Communication.Read(readBuffer, 0, byteToRead);
                        for (var i = 0; i < readCount; i++)
                        {
                            receive.Add(readBuffer[i]);
                        }
                        index++;
                        if (index > count)
                        {
                            return EndOperate(status: false, GetLanguageValue("接收数据超时"), null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\serial\\SerialOperate.cs", "SendWait", 223);

                        }
                        // check package length and break
                        if (receive.Count > 4
                            && receive[0] == 0xAA
                            && receive[1] == 0x55)
                        {
                            var length = (receive[2] << 8) + receive[3];
                            if (receive.Count == length + 2)
                            {
                                
                                break;
                            }
                            
                        }
                    }
                    {
                        // byte[] array = buffer.Take(length).ToArray();
                        byte[] array = receive.ToArray(); ;

                        if (array.Length != 0)
                        {
                            return EndOperate(status: true, null, array, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\serial\\SerialOperate.cs", "SendWait", 214);
                        }

                        return EndOperate(status: false, GetLanguageValue("未接收到数据"), null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\serial\\SerialOperate.cs", "SendWait", 218);
                    }

                }

                return EndOperate(status: false, GetLanguageValue("发送等待结果失败"), null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\serial\\SerialOperate.cs", "SendWait", 226);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\serial\\SerialOperate.cs", "SendWait", 230);
            }
            finally
            {
                if (GetStatus().Status)
                {
                    StartMonitor();
                }
            }
        }

        public override OperateResult GetBaseObject()
        {
            BegOperate("GetBaseObject");
            if (!GetStatus().GetDetails(out string message))
            {
                return EndOperate(status: false, message, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\serial\\SerialOperate.cs", "GetBaseObject", 247);
            }

            return EndOperate(status: true, null, Communication, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\serial\\SerialOperate.cs", "GetBaseObject", 249);
        }

        public override OperateResult GetStatus()
        {
            BegOperate("GetStatus");
            if (Communication != null && Communication.IsOpen)
            {
                return EndOperate(status: true, GetLanguageValue("已连接"), null, null, logOutput: false, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\serial\\SerialOperate.cs", "GetStatus", 257);
            }

            return EndOperate(status: false, GetLanguageValue("未连接"), null, null, logOutput: false, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\serial\\SerialOperate.cs", "GetStatus", 261);
        }

        public Task<OperateResult> SendDataRevData(byte[] data, int timeout = 100000)
        {
            BegOperate("SendDataRevData");
            var tsk = Task.Run(() =>
            {
                try
                {
                   

                    Communication.Write(data, 0, data.Length);

                    // loop to wait data
                    var revData = ReadData(timeout);
                    return EndOperate(status: true, null, revData, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\serial\\SerialOperate.cs", "SendWait", 214);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });

            return tsk;

        }

        /// 读取串口数据
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public byte[] ReadData(int timeout = 3000)
        {
            // plus 1 used to transfer data
            var count = ((timeout + 30) / 29) + 1;
            var index = 0;
           
            var receive = new List<byte>();
            
            while (true)
            {
                Task.Delay(29).Wait();

               // if (!port.IsOpen)
                    // throw new AlterException(_localize.GetString(LocalizeConstantCom.DEVICE_DISCONNECTED));

                    var byteToRead = Communication.BytesToRead;

                var readBuffer = new byte[byteToRead];
                var readCount = Communication.Read(readBuffer, 0, byteToRead);
                for (var i = 0; i < readCount; i++)
                {
                    receive.Add(readBuffer[i]);
                }
               if( receive.Count() > 0)
                    {
                    break;
                    }
                index++;
                if (index > count) break;

              
            }

            return receive.ToArray();
        }

       

        private void DataToRevDataNotChek(List<byte> receive, int leng, ref List<RevData> revDatas, ref List<byte> lastlist)
        {
            while (receive.Count >= leng)
            {
                List<byte> firstPart = receive.GetRange(0, leng);
                revDatas.Add(new RevData
                {
                    bCommand = firstPart[4],
                    Datas = firstPart.ToArray(),
                    lDatas = firstPart.Take(leng).ToArray()
                });

                receive = receive.GetRange(leng, receive.Count - leng); // 更新 receive 为剩余的数据部分
            }

            lastlist = receive;
        }
    }
}
