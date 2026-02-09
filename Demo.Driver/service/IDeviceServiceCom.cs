using CyUSB;
using Demo.Communication.constant;
using Demo.Core.extend;
using Demo.Core.handler;
using Demo.Driver.atp;
using Demo.Driver.serial;
using Demo.Model.data;
using Demo.Model.@enum;
using Demo.Windows.Core.handler;
using FuX.Core.Communication.serial;
using FuX.Core.db;
using FuX.Core.extend;
using FuX.Core.services;
using FuX.Model.entities;
using FuX.Model.Specenum;
using FuX.Unility;
using Optosky.DeviceDriver.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Driver.service
{
    public partial interface IDeviceServiceCom
    {

        /// <summary>
        /// 串口信息
        /// </summary>
        DeviceCom DeviceCom { get; }

        /// <summary>
        /// 打开设备集合
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        Tuple<bool, string> Open(List<string> com);

        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <param name="com"></param>
        void Close(string com = "");

        /// <summary>
        /// 获取设备
        /// </summary>
        /// <returns></returns>
        Task<List<DevListShow>> RetrieveATRCom();

        /// <summary>
        /// 清空缓存池
        /// </summary>
        void ClearCachePool();

        /// <summary>
        /// 获取当前打开的设备信息
        /// </summary>
        /// <returns></returns>
        List<DevListShow> GetOpenDevInfo();

      


        /// <summary>
        /// 获取连续数据（带校验）
        /// </summary>
        /// <param name="retlen">转换的长度集合</param>
        /// <param name="llist">返回数据</param>
        /// <returns>List集合，前面为数据转换的长度，最后一位为原始数据用来查看</returns>
        List<List<object>> AcquireData(List<int> retlen, ref List<byte> llist);

        /// <summary>
        /// 获取连续数据（带校验）16进制
        /// </summary>
        /// <param name="llist">返回数据</param>
        /// <returns>List集合，前面为数据转换的长度，最后一位为原始数据用来查看</returns>
        string AcquireData16(ref List<byte> llist);

        /// <summary>
        /// 获取连续数据（不带校验）
        /// </summary>
        /// <param name="retlen">转换的长度集合</param>
        /// <param name="llist">返回数据</param>
        /// <param name="gettime">获取等待时长</param>
        /// <returns>List集合，前面为数据转换的长度，最后一位为原始数据用来查看</returns>
        List<List<object>> AcquireDataNotCheck(List<int> retlen, ref List<byte> llist, int gettime = 50);


        /// <summary>
        /// 读取 SN 号
        /// </summary>
        /// <returns></returns>
        Task<string> GetDevSN(byte code);

        /// <summary>
        /// 读取 版本 号
        /// </summary>
        /// <returns></returns>
        Task<string> GetDevQV(byte code);

        /// <summary>
        /// 是否禁用指令
        /// </summary>

        void SetQvDisableCmd(bool d);

    }

    public partial class DeviceServiceCom : IDeviceServiceCom
    {
        private ATPOperate _atpOperate;
        private DBOperate _dbOperate;
        private SerialOperate _serialOperate;
        private SerialManagerOperate _serialManager;
        private ILocalize _localize;

        public DeviceServiceCom()
        {
            _atpOperate = CoreUnify<ATPOperate, ATPData.Basics>.Instance();
            _dbOperate = DBOperate.Instance();
            _serialOperate = SerialOperate.Instance();
            _serialManager = SerialManagerOperate.Instance(new SerialManagerData.Basics());
            _localize = InjectionWpf.GetService<ILocalize>();

        }

        public DeviceCom DeviceCom { get; private set; } = new DeviceCom();

        /// <summary>
        /// 是否禁用指令
        /// </summary>

        public void SetQvDisableCmd(bool d)
        {
         //   _protocol.IsQvDisableCmd = d;
        }

        public async Task<object> ComSerialPortAsk(string cmd, byte[] bytes = null)
        {
            try
            {
                if (_localize.GetCfg<ConnectType>("ConnectType") == ConnectType.USB)
                {
                   // return ComUsbSendAsk(cmd, bytes);
                }

                List<DevListShow> open = GetOpenDevInfo();
                if (open.Count <= 0)
                {
                    throw new Exception(_localize.GetString("设备未连接"));
                }

                string devname = open[0].DevName;
                List<DevInfoCom> defdev = _localize.GetCfg<List<DevInfoCom>>("MasterDev") ?? new List<DevInfoCom>();
                if (defdev.Count((DevInfoCom t) => t.IsMaster) > 0)
                {
                    string mname = defdev.FirstOrDefault((DevInfoCom z) => z.IsMaster)?.DevName;
                    if (open.Count((DevListShow t) => t.DevName == mname) > 0)
                    {
                        devname = mname;
                    }
                }

                return await ComSerialPortAsk(cmd, devname, bytes);
            }
            catch (Exception ex2)
            {
                Exception ex = ex2;
                throw ex;
            }
        }

        public async Task<object> ComSerialPortAsk(string cmd, object bytes = null, string tip = "")
        {
            try
            {
                if (_localize.GetCfg<ConnectType>("ConnectType") == ConnectType.USB)
                {
                   // return ComUsbSendAsk(cmd, bytes, tip);
                }

                List<DevListShow> open = GetOpenDevInfo();
                if (open.Count <= 0)
                {
                    throw new Exception(_localize.GetString("设备未连接"));
                }

                string devname = open[0].DevName;
                List<DevInfoCom> defdev = _localize.GetCfg<List<DevInfoCom>>("MasterDev") ?? new List<DevInfoCom>();
                if (defdev.Count((DevInfoCom t) => t.IsMaster) > 0)
                {
                    string mname = defdev.FirstOrDefault((DevInfoCom z) => z.IsMaster)?.DevName;
                    if (open.Count((DevListShow t) => t.DevName == mname) > 0)
                    {
                        devname = mname;
                    }
                }

                return await ComSerialPortAsk(cmd, devname, bytes, tip);
            }
            catch (Exception ex2)
            {
                Exception ex = ex2;
                throw ex;
            }
        }

        public async Task<object> ComSerialPortAsk(string cmd, string devname, byte[] bytes = null)
        {
            try
            {
                if (_localize.GetCfg<ConnectType>("ConnectType") == ConnectType.USB)
                {
                   // return ComUsbSendAsk(cmd, bytes);
                }

                List<DevListShow> open = GetOpenDevInfo();
                if (open.Count <= 0)
                {
                    throw new Exception(_localize.GetString("设备未连接"));
                }

                DevListShow obj = GetOpenDevInfo().FirstOrDefault((DevListShow t) => t.DevName.Contains(devname));
                if (obj == null)
                {
                    List<DevInfoCom> list = _localize.GetCfg<List<DevInfoCom>>("MasterDev") ?? new List<DevInfoCom>();
                    List<DevInfoCom> temp = list.Where((DevInfoCom t) => (from x in GetOpenDevInfo()
                                                                          select x.DevName).ToList().Contains(t.DevName)).ToList();
                    string tempname = string.Empty;
                    try
                    {
                        if (ConBoard.Mcon == (ConBoard)Enum.Parse(typeof(ConBoard), devname))
                        {
                            tempname = temp.FirstOrDefault((DevInfoCom t) => t.IsMaster)?.DevName?.ToString();
                        }
                        else if (ConBoard.Dcon == (ConBoard)Enum.Parse(typeof(ConBoard), devname))
                        {
                            tempname = temp.FirstOrDefault((DevInfoCom t) => !t.IsMaster)?.DevName?.ToString();
                        }
                    }
                    catch (Exception)
                    {
                        tempname = devname;
                    }

                    if (string.IsNullOrEmpty(tempname))
                    {
                        throw new Exception(_localize.GetString("设备未连接"));
                    }

                    obj = GetOpenDevInfo().FirstOrDefault((DevListShow t) => t.DevName.Contains(tempname));
                    if (obj == null)
                    {
                        throw new Exception(_localize.GetString("设备未连接"));
                    }
                }
                
                c_Command com = _dbOperate.Query<c_Command>(c => c.cmdName == cmd && c.devName == devname).GetSource<List<c_Command>>()?.SingleOrDefault();
                if (com == null)
                {
                    throw new Exception(_localize.GetFormatString("指令未找到", cmd, devname));
                }

                byte bytecmd = Convert.ToByte(com.cmdNum, 16);
                
                byte[] data = bytecmd.GetPackageModel((bytes != null) ? bytes : null).GetBytes(out PackageModel model);
                if (com.IsTransmission)
                {
                    byte cmdt = CommonFuncHandler.StringToByte(com.TransmissionCmd);
                    data = cmdt.GetPackageModel(data).GetBytes(out PackageModel model1);
                }

                if (obj == null)
                {
                    throw new Exception(_localize.GetString("设备未连接"));
                }

                _serialManager.IsStop = true;
                _serialManager.Open(obj.COM);
                PackageModel res = await _serialManager.SendDataRevData(data, com.timeout).ConfigureAwait(continueOnCapturedContext: false);
                if (res == null || res.lDatas == null)
                {
                    throw new Exception(_localize.GetFormatString("请求超时", com.cmdNum, com.cmdName, devname));
                }

                if (com.IsTransmission)
                {
                    byte[] receive = res.lDatas;
                    if (receive.Length > 4 && receive[0] == 170 && receive[1] == 85)
                    {
                        int length = (receive[2] << 8) + receive[3];
                        if (receive.Length < length + 2)
                        {
                            throw new Exception("DATA_PACK_ERR");
                        }

                        res.Command = receive[4];
                        res.lDatas = receive.Skip(5).Take(length - 4).ToArray();
                    }
                }

                return com.retType switch
                {
                    CmdRetDataType.Bool => RevPackParserHandler.ParseBoolean(res),
                    CmdRetDataType.Float => RevPackParserHandler.ParseFloat(res),
                    CmdRetDataType.CodingStr => RevPackParserHandler.ByteToChar(res),
                    CmdRetDataType.Data => RevPackParserHandler.ParseByte(res),
                    CmdRetDataType.FloatList => RevPackParserHandler.ParseFloatList(res),
                    CmdRetDataType.IntList => RevPackParserHandler.ParseToList(res),
                    CmdRetDataType.String => RevPackParserHandler.Get16XToStr(res).Replace("\0", ""),
                    CmdRetDataType.Int => RevPackParserHandler.ParseInt(res),
                    CmdRetDataType.Motor => RevPackParserHandler.ParseMotor(res),
                    CmdRetDataType.ATPCCD => RevPackParserHandler.ParseCCDData(res),
                    _ => null,
                };
            }
            catch (Exception ex3)
            {
                Exception ex = ex3;
                throw ex;
            }
        }

        public async Task<object> ComSerialPortAsk(string cmd, string devname, object objdata = null, string tip = "")
        {
            try
            {
                if (_localize.GetCfg<ConnectType>("ConnectType") == ConnectType.USB)
                {
                   // return ComUsbSendAsk(cmd, objdata, tip);
                }

                DevListShow obj = GetOpenDevInfo().FirstOrDefault((DevListShow t) => t.DevName.Contains(devname));
                if (obj == null)
                {
                    List<DevInfoCom> list2 = _localize.GetCfg<List<DevInfoCom>>("MasterDev") ?? new List<DevInfoCom>();
                    List<DevInfoCom> temp = list2.Where((DevInfoCom t) => (from x in GetOpenDevInfo()
                                                                           select x.DevName).ToList().Contains(t.DevName)).ToList();
                    string tempname = string.Empty;
                    try
                    {
                        if (ConBoard.Mcon == (ConBoard)Enum.Parse(typeof(ConBoard), devname))
                        {
                            tempname = temp.FirstOrDefault((DevInfoCom t) => t.IsMaster)?.DevName?.ToString();
                        }
                        else if (ConBoard.Dcon == (ConBoard)Enum.Parse(typeof(ConBoard), devname))
                        {
                            tempname = temp.FirstOrDefault((DevInfoCom t) => !t.IsMaster)?.DevName?.ToString();
                        }
                    }
                    catch (Exception)
                    {
                        tempname = devname;
                    }

                    if (string.IsNullOrEmpty(tempname))
                    {
                        throw new Exception(_localize.GetString("设备未连接"));
                    }

                    obj = GetOpenDevInfo().FirstOrDefault((DevListShow t) => t.DevName.Contains(tempname));
                    if (obj == null)
                    {
                        throw new Exception(_localize.GetString("设备未连接"));
                    }
                }

                List<DevListShow> open = GetOpenDevInfo();
                if (open.Count <= 0)
                {
                    throw new Exception(_localize.GetString("设备未连接"));
                }

                c_Command com = _dbOperate.Query<c_Command>(c => c.cmdName == cmd && c.devName == devname).GetSource<List<c_Command>>()?.SingleOrDefault();
                if (com == null)
                {
                    throw new Exception(_localize.GetString("设备未连接"));
                }

                byte bytecmd = Convert.ToByte(com.cmdNum, 16);
                List<byte> bytelist = new List<byte>();
                if (objdata != null)
                {
                    string bytes = objdata.ToString();
                    switch (com.InType)
                    {
                        case CmdInDataType.CodingStr:
                            {
                                string[] list = bytes.Split(' ');
                                string[] array = list;
                                foreach (string str in array)
                                {
                                    if (!string.IsNullOrEmpty(str))
                                    {
                                        if (str.Length == 4)
                                        {
                                            bytelist.Add(byte.Parse(str, NumberStyles.HexNumber));
                                        }
                                        else
                                        {
                                            bytelist.Add(Convert.ToByte(str, 16));
                                        }
                                    }
                                }

                                break;
                            }
                        case CmdInDataType.Float:
                            {
                                byte[] ft = BitConverter.GetBytes(float.Parse(bytes));
                                for (int l = ft.Length - 1; l >= 0; l--)
                                {
                                    bytelist.Add(ft[l]);
                                }

                                break;
                            }
                        case CmdInDataType.Int:
                            {
                                byte[] intby = BitConverter.GetBytes(int.Parse(bytes));
                                for (int k = com.strnum - 1; k >= 0; k--)
                                {
                                    bytelist.Add(intby[k]);
                                }

                                break;
                            }
                        case CmdInDataType.ListInt:
                            foreach (int tdata2 in (List<int>)objdata)
                            {
                                byte[] intbyt2 = BitConverter.GetBytes(tdata2);
                                for (int j = com.strnum - 1; j >= 0; j--)
                                {
                                    bytelist.Add(intbyt2[j]);
                                }
                            }

                            break;
                        case CmdInDataType.ListFlt:
                            foreach (double tdata in (List<double>)objdata)
                            {
                                byte[] intbyt = BitConverter.GetBytes((float)tdata);
                                for (int i = intbyt.Length - 1; i >= 0; i--)
                                {
                                    bytelist.Add(intbyt[i]);
                                }
                            }

                            break;
                        case CmdInDataType.String:
                            bytelist.AddRange(CommonFuncHandler.StrToByteList(bytes));
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(tip))
                {
                    tip = "0X" + ("0000" + tip).RightSubstring(2);
                    bytelist.Insert(0, Convert.ToByte(tip, 16));
                }
                
                byte[] data = bytecmd.GetPackageModel(bytelist.ToArray()).GetBytes(out PackageModel model);
                if (com.IsTransmission)
                {
                    byte cmdt = CommonFuncHandler.StringToByte(com.TransmissionCmd);
                    data = cmdt.GetPackageModel(data).GetBytes(out PackageModel model1);
                }

                _serialManager.IsStop = true;
                _serialManager.Open(obj.COM);
                PackageModel res = await _serialManager.SendDataRevData(data, com.timeout).ConfigureAwait(continueOnCapturedContext: false);
                if (res == null || res.lDatas == null)
                {
                    throw new Exception(_localize.GetFormatString("请求超时", com.cmdNum, com.cmdName, devname));
                }

                if (com.IsTransmission)
                {
                    byte[] receive = res.lDatas;
                    if (receive.Length > 4 && receive[0] == 170 && receive[1] == 85)
                    {
                        int length = (receive[2] << 8) + receive[3];
                        if (receive.Length < length + 2)
                        {
                            throw new Exception("DATA_PACK_ERR");
                        }

                        res.Command = receive[4];
                        res.lDatas = receive.Skip(5).Take(length - 4).ToArray();
                    }
                }

                return com.retType switch
                {
                    CmdRetDataType.Bool => RevPackParserHandler.ParseBoolean(res),
                    CmdRetDataType.Float => RevPackParserHandler.ParseFloat(res),
                    CmdRetDataType.CodingStr => RevPackParserHandler.ByteToChar(res),
                    CmdRetDataType.Data => RevPackParserHandler.ParseByte(res),
                    CmdRetDataType.FloatList => RevPackParserHandler.ParseFloatList(res),
                    CmdRetDataType.IntList => RevPackParserHandler.ParseToList(res),
                    CmdRetDataType.String => RevPackParserHandler.Get16XToStr(res).Replace("\0", ""),
                    CmdRetDataType.Int => RevPackParserHandler.ParseInt(res),
                    CmdRetDataType.Motor => RevPackParserHandler.ParseMotor(res),
                    CmdRetDataType.ATPCCD => RevPackParserHandler.ParseCCDData(res),
                    _ => null,
                };
            }
            catch (Exception ex3)
            {
                Exception ex = ex3;
                throw ex;
            }
        }

        public async Task<object> ComSerialPortAsk(byte cmd, byte[] bytes = null)
        {
            try
            {
                if (_localize.GetCfg<ConnectType>("ConnectType") == ConnectType.USB)
                {
                   // return ComUsbSendAsk(cmd, bytes);
                }

                List<DevListShow> open = GetOpenDevInfo();
                if (open.Count <= 0)
                {
                    throw new Exception(_localize.GetString("设备未连接"));
                }

                string devname = open[0].DevName;
                List<DevInfoCom> defdev = _localize.GetCfg<List<DevInfoCom>>("MasterDev") ?? new List<DevInfoCom>();
                if (defdev.Count((DevInfoCom t) => t.IsMaster) > 0)
                {
                    string mname = defdev.FirstOrDefault((DevInfoCom z) => z.IsMaster)?.DevName;
                    if (open.Count((DevListShow t) => t.DevName == mname) > 0)
                    {
                        devname = mname;
                    }
                }

                return await ComSerialPortAsk(cmd, devname, bytes);
            }
            catch (Exception ex2)
            {
                Exception ex = ex2;
                throw ex;
            }
        }

        public async Task<object> ComSerialPortAsk(byte cmd, object bytes = null, string tip = "")
        {
            try
            {
                if (_localize.GetCfg<ConnectType>("ConnectType") == ConnectType.USB)
                {
                    //return ComUsbSendAsk(cmd, bytes, tip);
                }

                List<DevListShow> open = GetOpenDevInfo();
                if (open.Count <= 0)
                {
                    throw new Exception(_localize.GetString("设备未连接"));
                }

                string devname = open[0].DevName;
                List<DevInfoCom> defdev = _localize.GetCfg<List<DevInfoCom>>("MasterDev") ?? new List<DevInfoCom>();
                if (defdev.Count((DevInfoCom t) => t.IsMaster) > 0)
                {
                    string mname = defdev.FirstOrDefault((DevInfoCom z) => z.IsMaster)?.DevName;
                    if (open.Count((DevListShow t) => t.DevName == mname) > 0)
                    {
                        devname = mname;
                    }
                }

                return await ComSerialPortAsk(cmd, devname, bytes, tip);
            }
            catch (Exception ex2)
            {
                Exception ex = ex2;
                throw ex;
            }
        }

        public void ClearCachePool()
        {
            _serialManager.ReadData(1000);
        }

        public async Task<object> ComSerialPortAsk(byte cmd, string devname, byte[] bytes = null)
        {
            if (_localize.GetCfg<ConnectType>("ConnectType") == ConnectType.USB)
            {
                //return ComUsbSendAsk(cmd, bytes);
            }

            DevListShow obj = GetOpenDevInfo().FirstOrDefault((DevListShow t) => t.DevName.Contains(devname));
            if (obj == null)
            {
                List<DevInfoCom> list = _localize.GetCfg<List<DevInfoCom>>("MasterDev") ?? new List<DevInfoCom>();
                List<DevInfoCom> temp = list.Where((DevInfoCom t) => (from x in GetOpenDevInfo()
                                                                      select x.DevName).ToList().Contains(t.DevName)).ToList();
                string tempname = string.Empty;
                if (ConBoard.Mcon == (ConBoard)Enum.Parse(typeof(ConBoard), devname))
                {
                    tempname = temp.FirstOrDefault((DevInfoCom t) => t.IsMaster)?.DevName?.ToString();
                }
                else if (ConBoard.Dcon == (ConBoard)Enum.Parse(typeof(ConBoard), devname))
                {
                    tempname = temp.FirstOrDefault((DevInfoCom t) => !t.IsMaster)?.DevName?.ToString();
                }

                if (string.IsNullOrEmpty(tempname))
                {
                    throw new Exception(_localize.GetString("设备未连接"));
                }

                obj = GetOpenDevInfo().FirstOrDefault((DevListShow t) => t.DevName.Contains(tempname));
                if (obj == null)
                {
                    throw new Exception(_localize.GetString("设备未连接"));
                }
            }

            
            string cmd2 = "0x" + cmd.ToString("X2");
            string text = devname;
            text = (text != null) ? text.Split('-')[0] : null;
           
            c_Command com = _dbOperate.Query<c_Command>(c => c.cmdNum == cmd2 && c.devName == text).GetSource<List<c_Command>>()?.SingleOrDefault();
            if (com == null)
            {
                throw new Exception(_localize.GetFormatString("指令未找到", cmd, devname));
            }

            byte bytecmd = Convert.ToByte(com.cmdNum, 16);

            byte[] data = bytecmd.GetPackageModel((bytes != null) ? bytes : null).GetBytes(out PackageModel model);
            if (com.IsTransmission)
            {
                byte cmdt = CommonFuncHandler.StringToByte(com.TransmissionCmd);
                data = cmdt.GetPackageModel(data).GetBytes(out PackageModel model1);
            }

            if (obj == null)
            {
                throw new Exception(_localize.GetString("设备未连接"));
            }

            _serialManager.IsStop = true;
            _serialManager.Open(obj.COM);
            PackageModel res = await _serialManager.SendDataRevData(data, com.timeout).ConfigureAwait(false);
            if (res == null || res.lDatas == null)
            {
                throw new Exception(_localize.GetFormatString("请求超时", com.cmdNum, com.cmdName, devname));
            }

            if (com.IsTransmission)
            {
                byte[] receive = res.lDatas;
                if (receive.Length > 4 && receive[0] == 170 && receive[1] == 85)
                {
                    int length = (receive[2] << 8) + receive[3];
                    if (receive.Length < length + 2)
                    {
                        throw new Exception("DATA_PACK_ERR");
                    }

                    res.Command = receive[4];
                    res.lDatas = receive.Skip(5).Take(length - 4).ToArray();
                }
            }

            return com.retType switch
            {
                CmdRetDataType.Bool => RevPackParserHandler.ParseBoolean(res),
                CmdRetDataType.Float => RevPackParserHandler.ParseFloat(res),
                CmdRetDataType.CodingStr => RevPackParserHandler.ByteToChar(res),
                CmdRetDataType.Data => RevPackParserHandler.ParseByte(res),
                CmdRetDataType.FloatList => RevPackParserHandler.ParseFloatList(res),
                CmdRetDataType.IntList => RevPackParserHandler.ParseToList(res),
                CmdRetDataType.String => RevPackParserHandler.Get16XToStr(res).Replace("\0", ""),
                CmdRetDataType.Int => RevPackParserHandler.ParseInt(res),
                CmdRetDataType.Motor => RevPackParserHandler.ParseMotor(res),
                CmdRetDataType.ATPCCD => RevPackParserHandler.ParseCCDData(res),
                _ => null,
            };
        }

        public async Task<object> ComSerialPortAsk(byte cmd, string devname, object objdata = null, string tip = "")
        {
            if (_localize.GetCfg<ConnectType>("ConnectType") == ConnectType.USB)
            {
               // return ComUsbSendAsk(cmd, objdata, tip);
            }

            DevListShow obj = GetOpenDevInfo().FirstOrDefault((DevListShow t) => t.DevName.Contains(devname));
            if (obj == null)
            {
                List<DevInfoCom> list = _localize.GetCfg<List<DevInfoCom>>("MasterDev") ?? new List<DevInfoCom>();
                List<DevInfoCom> temp = list.Where((DevInfoCom t) => (from x in GetOpenDevInfo()
                                                                      select x.DevName).ToList().Contains(t.DevName)).ToList();
                string tempname = string.Empty;
                if (ConBoard.Mcon == (ConBoard)Enum.Parse(typeof(ConBoard), devname))
                {
                    tempname = temp.FirstOrDefault((DevInfoCom t) => t.IsMaster)?.DevName?.ToString();
                }
                else if (ConBoard.Dcon == (ConBoard)Enum.Parse(typeof(ConBoard), devname))
                {
                    tempname = temp.FirstOrDefault((DevInfoCom t) => !t.IsMaster)?.DevName?.ToString();
                }

                if (string.IsNullOrEmpty(tempname))
                {
                    throw new Exception(_localize.GetString("设备未连接")); ;
                }

                obj = GetOpenDevInfo().FirstOrDefault((DevListShow t) => t.DevName.Contains(tempname));
                if (obj == null)
                {
                    throw new Exception(_localize.GetString("设备未连接"));
                }
            }

           
            string cmd2 = "0x" + cmd.ToString("X2");
            string text = devname;
            text = (text != null) ? text.Split('-')[0] : null;
            c_Command com = _dbOperate.Query<c_Command>(c => c.cmdNum == cmd2 && c.devName == text).GetSource<List<c_Command>>()?.SingleOrDefault();
            if (com == null)
            {
                throw new Exception(_localize.GetFormatString("请求超时", cmd.ToString("X2")));
            }

            byte bytecmd = Convert.ToByte(com.cmdNum, 16);
            List<byte> bytelist = new List<byte>();
            if (objdata != null)
            {
                string bytes = objdata.ToString();
                switch (com.InType)
                {
                    case CmdInDataType.CodingStr:
                        {
                            string[] list2 = bytes.Split(' ');
                            string[] array = list2;
                            foreach (string str in array)
                            {
                                if (!string.IsNullOrEmpty(str))
                                {
                                    if (str.Length == 4)
                                    {
                                        bytelist.Add(byte.Parse(str, NumberStyles.HexNumber));
                                    }
                                    else
                                    {
                                        bytelist.Add(Convert.ToByte(str, 16));
                                    }
                                }
                            }

                            break;
                        }
                    case CmdInDataType.Float:
                        {
                            byte[] ft = BitConverter.GetBytes(float.Parse(bytes));
                            for (int l = ft.Length - 1; l >= 0; l--)
                            {
                                bytelist.Add(ft[l]);
                            }

                            break;
                        }
                    case CmdInDataType.Int:
                        {
                            byte[] intby = BitConverter.GetBytes(int.Parse(bytes));
                            for (int k = com.strnum - 1; k >= 0; k--)
                            {
                                bytelist.Add(intby[k]);
                            }

                            break;
                        }
                    case CmdInDataType.ListInt:
                        foreach (int tdata2 in (List<int>)objdata)
                        {
                            byte[] intbyt2 = BitConverter.GetBytes(tdata2);
                            for (int j = com.strnum - 1; j >= 0; j--)
                            {
                                bytelist.Add(intbyt2[j]);
                            }
                        }

                        break;
                    case CmdInDataType.ListFlt:
                        foreach (double tdata in (List<double>)objdata)
                        {
                            byte[] intbyt = BitConverter.GetBytes((float)tdata);
                            for (int i = intbyt.Length - 1; i >= 0; i--)
                            {
                                bytelist.Add(intbyt[i]);
                            }
                        }

                        break;
                    case CmdInDataType.String:
                        bytelist.AddRange(CommonFuncHandler.StrToByteList(bytes));
                        break;
                }
            }

            if (!string.IsNullOrEmpty(tip))
            {
                tip = "0X" + ("0000" + tip).RightSubstring(2);
                bytelist.Insert(0, Convert.ToByte(tip, 16));
            }

            byte[] data = bytecmd.GetPackageModel(bytelist.ToArray()).GetBytes(out PackageModel model);
            if (com.IsTransmission)
            {
                byte cmdt = CommonFuncHandler.StringToByte(com.TransmissionCmd);
                data = cmdt.GetPackageModel(data).GetBytes(out PackageModel model1);
            }

            _serialManager.IsStop = true;
            _serialManager.Open(obj.COM);
            PackageModel res = await _serialManager.SendDataRevData(data, com.timeout).ConfigureAwait(continueOnCapturedContext: false);
            if (res == null || res.lDatas == null)
            {
                throw new Exception(_localize.GetFormatString("请求超时", com.cmdNum, com.cmdName, devname));
            }

            if (com.IsTransmission)
            {
                byte[] receive = res.lDatas;
                if (receive.Length > 4 && receive[0] == 170 && receive[1] == 85)
                {
                    int length = (receive[2] << 8) + receive[3];
                    if (receive.Length < length + 2)
                    {
                        throw new Exception("DATA_PACK_ERR");
                    }

                    res.Command = receive[4];
                    res.lDatas = receive.Skip(5).Take(length - 4).ToArray();
                }
            }

            return com.retType switch
            {
                CmdRetDataType.Bool => RevPackParserHandler.ParseBoolean(res),
                CmdRetDataType.Float => RevPackParserHandler.ParseFloat(res),
                CmdRetDataType.CodingStr => RevPackParserHandler.ByteToChar(res),
                CmdRetDataType.Data => RevPackParserHandler.ParseByte(res),
                CmdRetDataType.FloatList => RevPackParserHandler.ParseFloatList(res),
                CmdRetDataType.IntList => RevPackParserHandler.ParseToList(res),
                CmdRetDataType.String => RevPackParserHandler.Get16XToStr(res).Replace("\0", ""),
                CmdRetDataType.Int => RevPackParserHandler.ParseInt(res),
                CmdRetDataType.Motor => RevPackParserHandler.ParseMotor(res),
                CmdRetDataType.ATPCCD => RevPackParserHandler.ParseCCDData(res),
                _ => null,
            };
        }


        /// <summary>
        /// 获取连续数据（带框架校验）
        /// </summary>
        /// <param name="retlen">转换的长度集合</param>
        /// <param name="llist">返回数据</param>
        /// <returns></returns>
        public List<List<object>> AcquireData(List<int> retlen, ref List<byte> llist)
        {
            List<List<object>> ret = new List<List<object>>();
            _serialManager.IsStop = false;
            //获取返回数据
            var res = _serialManager.ReadDataP(ref llist, 50);
            if (res.Count <= 0) return ret;

            foreach (var rdata in res)
            {
                List<object> temp = new List<object>();

                var index = 0;
                //长度
                foreach (var num in retlen)
                {
                    var intnum = RevPackParserHandler.ParseInt(new PackageModel { lDatas = rdata.lDatas.Skip(index).Take(num).ToArray() });
                    temp.Add(intnum);
                    index = index + num;
                }
                temp.Add(BitConverter.ToString(rdata.Data).Replace("-", " "));
                ret.Add(temp);
            }
            return ret;
        }

        /// <summary>
        /// 获取连续数据（带框架校验）16进制
        /// </summary>
        /// <param name="llist">返回数据</param>
        /// <returns></returns>
        public string AcquireData16(ref List<byte> llist)
        {
            string retstr = string.Empty;
            _serialManager.IsStop = false;
            //获取返回数据
            var res = _serialManager.ReadDataP(ref llist, 50);
            if (res.Count <= 0) return retstr;

            foreach (var rdata in res)
            {
                retstr += " " + string.Join(" ", RevPackParserHandler.ByteToChar(rdata));
            }
            return retstr.Substring(1);
        }


        /// <summary>
        /// 获取连续数据（不带校验）
        /// </summary>
        /// <param name="retlen">转换的长度集合</param>
        /// <param name="llist">返回数据</param>
        /// <returns>List集合，前面为数据转换的长度，最后一位为原始数据用来查看</returns>
        public List<List<object>> AcquireDataNotCheck(List<int> retlen, ref List<byte> llist, int gettime = 50)
        {
            List<List<object>> ret = new List<List<object>>();
            _serialManager.IsStop = false;
            //获取返回数据
            var res = _serialManager.ReadDataP(ref llist, retlen.Sum(), gettime);
            if (res.Count <= 0) return ret;

            foreach (var rdata in res)
            {
                List<object> temp = new List<object>();

                var index = 0;
                //长度
                foreach (var num in retlen)
                {
                    if (rdata.lDatas == null || rdata.lDatas.Count() <= 0) continue;
                    var intnum = RevPackParserHandler.ParseInt(new PackageModel { lDatas = rdata.lDatas.Skip(index).Take(num).ToArray() });
                    temp.Add(intnum);
                    index = index + num;
                }
                if (temp.Count > 0)
                {
                    temp.Add(BitConverter.ToString(rdata.Datas).Replace("-", " "));
                    ret.Add(temp);
                }
            }
            return ret;
        }

       

        #region 指定串口连接

        /// <summary>
        /// 打开指定串口
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        public Tuple<bool, string> Open(List<string> com)
        {
            //查看是否有打开的串口
            var coms = DeviceCom.DevComList.Where(t => t.IsCon == true).ToList();

            foreach (var objs in com)
            {
                //_serialManager.Close(coms?.COM);//关闭所有的串口

                var obj = DeviceCom.DevComList.FirstOrDefault(t => t.COM == objs);
                if (obj != null)
                {
                    if (obj.IsCon)
                        continue;
                    //return Tuple.Create(true, "已连接，无需重复操作");

                    var ret = OpenCom(objs);
                    if (!ret.Item1)
                        Close();
                    obj.IsCon = true;
                }
                else
                {
                    throw new Exception("设备连接断开");
                }
            }
            return Tuple.Create(true, "成功");
        }


        private Tuple<bool, string> OpenCom(string com)
        {

            _serialManager.Open(com);

            return Tuple.Create(true, string.Empty);
        }

        /// <summary>
        /// 获取所有设备的信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<DevListShow>> RetrieveATRCom()
        {

            //查看是否有打开的串口
            var comList = DeviceCom.DevComList.Where(t => t.IsCon == true).ToList();
            DeviceCom.DevComList.Clear();//清空串口数据
            _serialManager.CloseAll();//关闭所有的串口

            var coms = await GetDeviceComInfo().ConfigureAwait(true);
            var devlist = _localize.GetCfg<List<DevInfoCom>>(UserCfgInfoConstantCom.MasterDev) ?? new List<DevInfoCom>();
            
            
            
            
            coms.ToList().ForEach(t => DeviceCom.DevComList.Add(new DevListShow()
            {
                COM = t.COM,
                DevName = t.DevName?.Replace("\0", ""),
                IsMaster = devlist.FirstOrDefault(z => z.DevName == t.DevName?.Replace("\0", "")) == null ? false : devlist.FirstOrDefault(z => z.DevName == t.DevName?.Replace("\0", "")).IsMaster,
                DevName1 = t.DevName?.Replace("\0", "")?.Split('-').Length > 0 ? t.DevName?.Replace("\0", "")?.Split('-')[0] : string.Empty,
                DevName2 = t.DevName?.Replace("\0", "")?.Split('-').Length > 1 ? t.DevName?.Replace("\0", "")?.Split('-')[1] : string.Empty,
                IsCon = comList.Count(c => c.COM == t.COM) > 0 ? true : false,
                SN = t.SN?.Replace("\0", ""),
                QV = t.QV?.Replace("\0", ""),
                MFDate = t.MFDate?.Replace("\0", ""),
            }));

            //重新打开串口
            if (comList.Count > 0)
            {
                Open(comList.Select(t => t.COM).ToList());
            }

            return DeviceCom.DevComList.ToList();

        }

        /// <summary>
        /// 获取串口设备信息
        /// </summary>
        /// <returns></returns>
        public async Task<IList<ComInfo>> GetDeviceComInfo()
        {
            var res = new List<ComInfo>();

            var coms = COMUtilHandler.GetComInfos();

            foreach (var com in coms)
            {
                var deviceTypeRes = await GetSubDeviceType(com.COM);

                if (deviceTypeRes == null || string.IsNullOrEmpty(deviceTypeRes.COM)) continue;
                if (string.IsNullOrEmpty(deviceTypeRes.DevName)) continue;

                com.DevName = deviceTypeRes.DevName;
                com.SN = deviceTypeRes.SN;
                com.QV = deviceTypeRes.QV;
                com.MFDate = deviceTypeRes.MFDate;
                res.Add(com);
            }

            return res;
        }

        /// <summary>
        /// 指定串口获取设备信息
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        private async Task<DevListShow> GetSubDeviceType(string com)
        {

            DevListShow devListShow = new DevListShow();
            if (string.IsNullOrEmpty(com)) return null;

            var openRes = Open(com);

            if (!openRes.Item1) return null;

            try
            {
                var _list = _localize.GetCfg<List<DevInfoCom>>(UserCfgInfoConstantCom.MasterDev) ?? new List<DevInfoCom>();
                if (_list.Count <= 0)
                {
                    _list.Add(new DevInfoCom()
                    {
                        DevName = "Tepm"
                    });
                }

                foreach (var obj in _list)
                {
                    //获取设备信息
                    var res = (await GetDevInfo(obj.Byte_GetDevInfo)).Replace("\u0001", "");

                    if (string.IsNullOrEmpty(res))
                    {
                        continue;
                    }
                    devListShow.COM = com;
                    devListShow.DevName = res;
                    //获取SN码
                    res = await GetDevSN(obj.Byte_GetSn);
                    devListShow.SN = res;
                    try
                    {
                        res = await GetDevSN(obj.Byte_Getqv);
                        devListShow.QV = res;
                    }
                    catch { }
                }
                //获取出厂日期
                //res = await GetDateMF();
                //devListShow.MFDate = res;
                return devListShow;
            }
            finally
            {
                _serialManager.CloseAll();//关闭所有的串口
            }
        }

        /// <summary>
        /// 获取当前打开的设备信息
        /// </summary>
        /// <returns></returns>
        public List<DevListShow> GetOpenDevInfo()
        {
            return DeviceCom.DevComList.Where(t => t.IsCon == true).ToList();
        }



        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <param name="com"></param>
        public void Close(string com = "")
        {
            _serialManager.Close(com);//关闭所有的串口
            var temp = DeviceCom.DevComList.FirstOrDefault(t => t.COM == com);
            if (temp != null)
            {
                temp.IsCon = false;
            }
        }


        /// <summary>
        /// 打开链接
        /// </summary>
        /// <param name="com"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private Tuple<bool, bool> Open(string com)
        {
            try
            {
                var vid = _localize.GetCfg<string>(UserCfgInfoConstantCom.Serial_VID);
                var pid = _localize.GetCfg<string>(UserCfgInfoConstantCom.Serial_PID);

                _serialManager.Init(Convert.ToInt16(vid, 16), Convert.ToInt16(pid, 16));

                _serialManager.Open(com);
                DeviceCom.COM = com;

                return Tuple.Create(true, false);
            }
            catch
            {
                return Tuple.Create(false, false);
            }
        }
        #endregion

        #region 设备指令

        /// <summary>
        /// 读取设备信息
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetDevInfo(byte code)
        {
            try
            {
                byte[] data = code.GetPackageModel().GetBytes(out PackageModel model);              
                var res = await _serialManager.SendData(data, 200).ConfigureAwait(false);
                res = await _serialManager.SendData(data, 200).ConfigureAwait(false);

                if (res != null)
                {
                    return res.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 读取 SN 号
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetDevSN(byte code)
        {
            byte[] data = code.GetPackageModel().GetBytes(out PackageModel model);
            var res = await _serialManager.SendData(data, 200).ConfigureAwait(false);

            if (res != null)
            {
                return res.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 读取 版本 号
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetDevQV(byte code)
        {
            byte[] data = code.GetPackageModel().GetBytes(out PackageModel model);
            var res = await _serialManager.SendData(data, 200).ConfigureAwait(false);

            if (res != null)
            {
                return res.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

       

        #endregion

    }
}
