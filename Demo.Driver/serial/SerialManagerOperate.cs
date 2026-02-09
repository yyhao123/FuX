using Demo.Core.handler;
using Demo.Model.data;
using FuX.Core.extend;
using FuX.Model.data;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Driver.serial
{
    public class SerialManagerOperate:CoreUnify<SerialManagerOperate, SerialManagerData.Basics>
    {
        private short m_nVendorID;

        private short m_nProductID;

        private static List<SerialInfo> m_oSerialInfos = new List<SerialInfo>();

        private static List<SerialPort> m_oSerialPorts = new List<SerialPort>();

        private static SerialInfo m_oSerialInfo = new SerialInfo();

        public static SerialPort m_oSerialPort = new SerialPort();

        private Queue<byte> _queue = new Queue<byte>();

        

        

        private static bool _IsStop = true;

        private static string OpenCom = string.Empty;

        public bool IsStop
        {
            get { return _IsStop; }
            set { _IsStop = value; }
        }

        public SerialManagerOperate(SerialManagerData.Basics basics):base(basics)
        {

        }

        public OperateResult InitSerialInfo(string Com)
        {
            BegOperate("InitSerialInfo");
           
            m_oSerialInfo = new SerialInfo();
            m_oSerialInfo.IsOpen = true;
            m_oSerialInfo.BaudRate = basics.BaudRate;
            m_oSerialInfo.DataBits = 8;
            m_oSerialInfo.Parity = System.IO.Ports.Parity.None;
            m_oSerialInfo.StopBits = System.IO.Ports.StopBits.One;
            m_oSerialInfo.ReadTimeOut = 5000;
            m_oSerialInfo.ReceivedBytesThreshold = 1;
            m_oSerialInfo.Name = Com;

            m_oSerialPort = new SerialPort();
            m_oSerialInfo.UpdatePort(m_oSerialPort);
            m_oSerialInfos.Add(m_oSerialInfo);
            m_oSerialPorts.Add(m_oSerialPort);
            try
            {
                m_oSerialPort.Open();
                return EndOperate(true);
            }
            catch
            {    
                return EndOperate(false, "设备未连接");
            }
        }


        public void Init(short vendorID, short productID)
        {
            m_nVendorID = vendorID;
            m_nProductID = productID;
        }

        /// <summary>
        /// 查询是否打开
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        public bool IsOpen(string com)
        {
            var objinfo = m_oSerialInfos.FirstOrDefault(t => t.Name == com);

            return objinfo.IsOpen;
        }


        /// <summary>
        /// 打开指定串口
        /// </summary>
        /// <param name="comName"></param>
        public OperateResult Open(string comName)
        {
            BegOperate("Open");
            OpenCom = comName;
            var objinfo = m_oSerialInfos.FirstOrDefault(t => t.Name == comName);
            if (objinfo == null)
            {
                var ret =InitSerialInfo(comName);
                if (!ret.Status)
                    return EndOperate(false, "设备连接断开");
                else
                    return EndOperate(true);
            }
            else
            {
                var port = m_oSerialPorts.FirstOrDefault(t => t.PortName == comName);
                if (port.IsOpen)
                {
                    port.Close();
                    objinfo.IsOpen = false;
                }
                try
                {
                    port.Open();                 
                }
                catch
                {       
                    return EndOperate(false, "设备连接断开");
                }
                objinfo.IsOpen = true;
                _queue.Clear();
                return EndOperate(true);
            }
        }


        /// <summary>
        /// 关闭所有串口
        /// </summary>
        public void CloseAll()
        {
            foreach (var obj in m_oSerialInfos)
            {
                var port = m_oSerialPorts.FirstOrDefault(t => t.PortName == obj.Name);
                if (port != null)
                {
                    port.Close();
                    obj.IsOpen = false;
                }
                //__SerialClose();
            }
        }

        /// <summary>
        /// 关闭指定串口
        /// </summary>
        /// <param name="comName"></param>
        public void Close(string comName = "")
        {
            var obj = m_oSerialInfos.FirstOrDefault(t => t.Name == comName);
            if (obj != null)
            {
                var port = m_oSerialPorts.FirstOrDefault(t => t.PortName == obj.Name);
                if (port != null)
                {
                    port.Close();
                    obj.IsOpen = false;
                }
            }
        }


        /// <summary>
        /// 发送数据返回结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        public Task<PackageModel> SendDataRevData(byte[] data, int timeout = 100000)
        {
            var tsk = Task.Run(() =>
            {
                try
                {
                    var port = m_oSerialPorts.FirstOrDefault(t => t.PortName == OpenCom);

                    if (timeout < 0) throw new Exception($"Invalid timeout!");

                    if (port == null || !port.IsOpen)
                        throw new Exception("设备链接断开");

                    port.Write(data, 0, data.Length);

                    // loop to wait data
                    var revData = ReadData(timeout);
                    return revData;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });

            return tsk;

        }


        /// <summary>
        /// 发送数据返回结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        public Task<object> SendData(byte[] data, int timeout = 100000)
        {
            var tsk = Task.Run(() =>
            {
                try
                {
                    var port = m_oSerialPorts.FirstOrDefault(t => t.PortName == OpenCom);
                    if (timeout < 0) throw new Exception($"Invalid timeout!");

                    if (!port.IsOpen)
                        throw new Exception("设备连接断开");

                    port.Write(data, 0, data.Length);

                    // loop to wait data
                    var revData = ReadData(timeout);
                    var res = RevPackParserHandler.Parse(revData);
                    if (res == null)
                        return false;
                    return res;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });

            return tsk;
        }

        /// <summary>
        /// 读取串口数据
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public PackageModel ReadData(int timeout = 3000)
        {
            // plus 1 used to transfer data
            var count = ((timeout + 30) / 29) + 1;
            var index = 0;
            var RevData = new PackageModel();
            var receive = new List<byte>();
            var port = m_oSerialPorts.FirstOrDefault(t => t.PortName == OpenCom);
            while (true)
            {
                Task.Delay(29).Wait();

                if (!port.IsOpen)
                    throw new Exception("设备连接断开");

                var byteToRead = port.BytesToRead;

                var readBuffer = new byte[byteToRead];
                var readCount = port.Read(readBuffer, 0, byteToRead);
                for (var i = 0; i < readCount; i++)
                {
                    receive.Add(readBuffer[i]);
                }

                index++;
                if (index > count) break;

                // check package length and break
                if (receive.Count > 4
                    && receive[0] == FixedModel.HEAD_FIRST
                    && receive[1] == FixedModel.HEAD_SECOND)
                {
                    var length = (receive[2] << 8) + receive[3];
                    if (receive.Count == length + FixedModel.HEAD_LENGTH)
                    {
                        RevData.Command = receive[4];
                        RevData.lDatas = receive.Skip(5).Take(receive.Count - 6).ToArray();
                        break;
                    }
                    else if (receive.Count > length + FixedModel.HEAD_LENGTH)
                    {
                        //encounter 2f response invalid: aa 55 00 05  2f 00 34 aa  55 00 05 2f  00 34
                        //throw new Exception($"Invalid response length expect {length + ProtocolConstant.HEAD_LENGTH} received {receive.Count}");
                        var newLength = receive.Count - FixedModel.HEAD_LENGTH;
                        receive[3] = (byte)newLength;//hrb 补丁修复数据
                        break;
                    }
                }
            }

            return RevData;
        }

        /// <summary>
        /// 读取串口数据
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public List<PackageModel> ReadDataP(ref List<byte> llist, int timeout = 3000)
        {
            // plus 1 used to transfer data
            var count = ((timeout + 30) / 29) + 1;
            var index = 0;
            List<PackageModel> revDatas = new List<PackageModel>();

            var port = m_oSerialPorts.FirstOrDefault(t => t.PortName == OpenCom);
            while (!IsStop)
            {
                Task.Delay(100).Wait();

                if (IsStop)
                    throw new Exception("操作已取消");

                if (!port.IsOpen)
                    throw new Exception("设备连接断开");

                var byteToRead = port.BytesToRead;

                var readBuffer = new byte[byteToRead];
                var readCount = port.Read(readBuffer, 0, byteToRead);
                for (var i = 0; i < readCount; i++)
                {
                    llist.Add(readBuffer[i]);
                }

                index++;
                if (index > count) break;
                if (llist.Count > 4)
                {
                    var lastlist = new List<byte>();//不全的字节重复使用
                    DataToRevData(llist, ref revDatas, ref lastlist);
                    llist = lastlist;
                }
            }
            return revDatas;
        }

        /// <summary>
        /// 读取串口数据
        /// </summary>
        /// <param name="llist"></param>
        /// <param name="checkLen"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public List<RevData> ReadDataP(ref List<byte> llist, int checkLen, int timeout = 3000)
        {
            // plus 1 used to transfer data
            var count = ((timeout + 30) / 29) + 1;
            var index = 0;
            List<RevData> revDatas = new List<RevData>();

            var port = m_oSerialPorts.FirstOrDefault(t => t.PortName == OpenCom);
            while (!IsStop)
            {
                Task.Delay(100).Wait();

                if (IsStop)
                    throw new Exception("操作已取消");

                if (!port.IsOpen)
                    throw new Exception("设备连接已断开");

                var byteToRead = port.BytesToRead;

                var readBuffer = new byte[byteToRead];
                var readCount = port.Read(readBuffer, 0, byteToRead);
                for (var i = 0; i < readCount; i++)
                {
                    llist.Add(readBuffer[i]);
                }

                index++;
                if (index > count) break;

                if (llist.Count >= checkLen)
                {
                    var lastlist = new List<byte>();//不全的字节重复使用
                    DataToRevDataNotChek(llist, checkLen, ref revDatas, ref lastlist);
                    llist = lastlist;
                }

                //if (llist.Count % checkLen == 0)
                //{
                //    revDatas.Add(new RevData() { bCommand = 0x00, Datas = llist.ToArray(), lDatas = llist.ToArray() });
                //    llist = new List<byte>();
                //    return revDatas;
                //}
            }
            return revDatas;
        }


        /// <summary>
        /// 多包数据的转换
        /// </summary>
        /// <param name="receive"></param>
        /// <param name="revDatas"></param>
        private void DataToRevDataNotChek1(List<byte> receive, int leng, ref List<RevData> revDatas, ref List<byte> lastlist)
        {
            if (receive.Count >= leng)
            {
                List<byte> firstPart = receive.GetRange(0, leng);//需要处理的数据

                revDatas.Add(new RevData()
                {
                    bCommand = firstPart[4],
                    Datas = firstPart.ToArray(),
                    lDatas = firstPart.Take(leng).ToArray()
                });

                List<byte> secondPart = receive.GetRange(leng, receive.Count - leng);//如果有继续处理的数据
                if (secondPart.Count > 0)
                {
                    DataToRevDataNotChek(secondPart, leng, ref revDatas, ref lastlist);
                }
            }
            else
            {
                lastlist = receive;
            }
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

        /// <summary>
        /// 多包数据的转换
        /// </summary>
        /// <param name="receive"></param>
        /// <param name="revDatas"></param>
        private void DataToRevData(List<byte> receive, ref List<PackageModel> revDatas, ref List<byte> lastlist)
        {
            if (receive.Count > 4)
            {
                //满足时有一个长度
                if (receive[0] == FixedModel.HEAD_FIRST && receive[1] == FixedModel.HEAD_SECOND)
                {
                    var packlength = (receive[2] << 8) + receive[3] + FixedModel.HEAD_LENGTH;
                    if (receive.Count < packlength)
                    {
                        lastlist = receive;
                        return;
                    }
                    ;
                    List<byte> firstPart = receive.GetRange(0, packlength);//需要处理的数据

                    revDatas.Add(new PackageModel()
                    {
                        Command = firstPart[4],
                        Data = firstPart.ToArray(),
                        lDatas = firstPart.Skip(5).Take(packlength - 6).ToArray()
                    });

                    List<byte> secondPart = receive.GetRange(packlength, receive.Count - packlength);//如果有继续处理的数据
                    if (secondPart.Count > 0)
                    {
                        DataToRevData(secondPart, ref revDatas, ref lastlist);
                    }
                }
            }
            else
            {
                lastlist = receive;
            }
        }
    }
}
