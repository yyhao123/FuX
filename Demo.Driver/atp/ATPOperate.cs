using Demo.Core.@abstract;
using FuX.Core.Communication.serial;
using FuX.Model.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Demo.Core.extend;
using Demo.Model.@enum;
using Demo.Model.data;
using FuX.Unility;
using FuX.Core.handler;
using FuX.Core.extend;
using FuX.Model.entities;
using FuX.Model.@enum;
using Demo.Core.handler;
using FuX.Core.db;
using FuX.Model.Specenum;

namespace Demo.Driver.atp
{
    public class ATPOperate : ATPAbstract<ATPOperate, ATPData.Basics>
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ATPOperate() : base() { }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="basics">基础数据</param>
        public ATPOperate(ATPData.Basics basics) : base(basics) { }

        /// <inheritdoc/>
        protected override string CD => "支持ATP系列所有命令操作";

        /// <inheritdoc/>
        protected override string CN => "ATP系列";

        /// <inheritdoc/>
        public override LanguageModel LanguageOperate { get; set; } = LanguageOperateStatic;
        private static LanguageModel LanguageOperateStatic { get; set; } = new("Demo.Language", "Language", "Demo.Language.dll");

        private DBOperate dbOperate = CoreUnify<DBOperate, DBData>.Instance();

        #region 私有属性
        /// <summary>
        /// 串口通信，负责数据的收发
        /// </summary>
        private SerialOperate? serialOperate;
        #endregion
        public override OperateResult ComSerialPortAsk(byte cmd, string devname, byte[] bytes = null)
        {
            BegOperate();
            try
            {
                //状态验证
                if (!GetStatus().GetDetails(out string? message))
                {
                    return EndOperate(false, message);
                }
                // var temp = "0x" + cmd.ToString("X2");
                string str = "0x" + cmd.ToString("X2");
                var ret = dbOperate.Query<c_Command>(c => c.cmdNum == "0xd0");
                //获取指令
                 ret = dbOperate.Query<c_Command>(c => c.cmdNum.ToLower() == str.ToLower());
                var com = dbOperate.Query<c_Command>(c => c.cmdNum.ToLower() == str.ToLower()).GetSource<List<c_Command>>().First();
                if (com==null)
                    return EndOperate(false, "未找到指令");

                //发送
                OperateResult result = serialOperate.SingleHandler(cmd, bytes);

                if (result.GetDetails(out object? resultData, out message))
                {
                    PackageModel? package = resultData?.GetSource<PackageModel>();
                    if (package != null)
                    {
                        
                        //解析
                        switch (com.retType)
                        {
                            case CmdRetDataType.Bool:
                                package.pDatas= RevPackParserHandler.ParseBoolean(package);
                                break;
                            case CmdRetDataType.Float:
                                package.pDatas = RevPackParserHandler.ParseFloat(package);
                                break;
                            case CmdRetDataType.CodingStr:
                                package.pDatas = RevPackParserHandler.ByteToChar(package);
                                break;
                            case CmdRetDataType.Data:
                                package.pDatas = RevPackParserHandler.ParseByte(package);
                                break;
                            case CmdRetDataType.FloatList:
                                package.pDatas = RevPackParserHandler.ParseFloatList(package);
                                break;
                            case CmdRetDataType.IntList:
                                package.pDatas = RevPackParserHandler.ParseToList(package);
                                break;
                            case CmdRetDataType.String:
                                package.pDatas = RevPackParserHandler.Get16XToStr(package).Replace("\0", "");
                                break;
                            case CmdRetDataType.Int:
                                package.pDatas = RevPackParserHandler.ParseInt(package);
                                break;
                            case CmdRetDataType.Motor:
                               // package.pDatas = RevPackParserHandler.ParseMotor(package);
                                break;
                            case CmdRetDataType.ATPCCD:
                                package.pDatas = RevPackParserHandler.ParseCCDData(package);
                                break;
                            default:
                                package.pDatas = null;
                                break;
                        }
                      
                        return EndOperate(true, resultData: package);
                    }
                }
                return EndOperate(false, message);
            }
            catch (Exception ex)
            {
                return EndOperate(false, ex.Message, exception: ex);
            }
        }

        public override OperateResult ComSerialPortAsk(byte cmd, byte[] bytes = null)
        {
            throw new NotImplementedException();
        }

        public override OperateResult ComSerialPortAsk(byte cmd, string devname, object bytes = null, string tip = "")
        {
            throw new NotImplementedException();
        }

        public override OperateResult ComSerialPortAsk(byte cmd, object bytes = null, string tip = "")
        {
            throw new NotImplementedException();
        }

        public override OperateResult ComSerialPortAsk(string cmd, byte[] bytes = null)
        {
            throw new NotImplementedException();
        }

        public override OperateResult ComSerialPortAsk(string cmd, byte[] bytes = null, string tip = "")
        {
            throw new NotImplementedException();
        }

        public override OperateResult ComSerialPortAsk(string cmd, string devname, byte[] bytes = null)
        {
            throw new NotImplementedException();
        }

        public override OperateResult ComSerialPortAsk(string cmd, string devname, object bytes = null, string tip = "")
        {
            throw new NotImplementedException();
        }

        public override OperateResult GetStatus()
        {
            BegOperate();
            if (serialOperate != null)
            {
                if (serialOperate.GetStatus().Status)
                {
                    return EndOperate(true, LanguageOperate.GetLanguageValue("已连接"), logOutput: false);
                }
            }
            return EndOperate(false, LanguageOperate.GetLanguageValue("未连接"), logOutput: false);
        }

        public override OperateResult Off(bool hardClose = false)
        {
            BegOperate();
            try
            {
                if (!hardClose)
                {
                    if (!GetStatus().GetDetails(out string? message))
                    {
                        return EndOperate(false, message);
                    }
                }
                OperateResult[] results = Task.WhenAll(serialOperate?.OffAsync()).Result;
                if (results.Where(c => !c.Status).FirstOrDefault() == null)
                {
                   
                    serialOperate.OnDataEvent -= OnDataEventHandler;
                    serialOperate.OnInfoEvent -= OnInfoEventHandler;
                    //释放对象                 
                    serialOperate.Dispose();
                    //清空
                    serialOperate = null;
                    return EndOperate(true);
                }
                return EndOperate(false);
            }
            catch (Exception ex)
            {
                return EndOperate(false, ex.Message, exception: ex);
            }
        }

        public override  OperateResult On()
        {
            BegOperate();
            try
            {
                if (GetStatus().GetDetails(out string? message))
                {
                    return EndOperate(false, message);
                }
                
                serialOperate = SerialOperate.Instance(basics.SerialData);
                //事件注册
                serialOperate.OnDataEvent -= OnDataEventHandler;
                serialOperate.OnInfoEvent -= OnInfoEventHandler;
               
                //打开连接，同步执行
                var ret = serialOperate.On();
                OperateResult[] results = new OperateResult[] {ret};


                if (results.Where(c => !c.Status).FirstOrDefault() == null)
                {
                    return EndOperate(true);
                }
                Off(true);
                return EndOperate(false);
            }
            catch (Exception ex)
            {
                Off(true);
                return EndOperate(false, ex.Message, exception: ex);
            }
        }
    }
}
