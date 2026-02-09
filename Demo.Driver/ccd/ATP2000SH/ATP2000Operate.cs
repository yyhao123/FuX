using Demo.Core.@abstract;
using Demo.Model.@interface;
using FuX.Model.data;
using OptoskyConnector.Params;
using OptoskyConnector;
using FuX.Core.handler;
using static Demo.Driver.ccd.ATP2000SH.ATP2000Data;
using OptoskyConnector.Service;
using FuX.Unility;
using Demo.Model.data;

namespace Demo.Driver.ccd.ATP2000SH
{
    public class ATP2000Operate : CCDAbstract<ATP2000Operate, Basics>,ICCD
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ATP2000Operate() : base()
        {
            SetLanguage(FuX.Model.@enum.LanguageType.en);
        }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="basics">基础数据</param>
        public ATP2000Operate(Basics basics) : base(basics) { }

        /// <inheritdoc/>
        protected override string CD => "支持ATP2000SH所有命令操作";

        /// <inheritdoc/>
        protected override string CN => "ATP2000SH";

        /// <inheritdoc/>
        public override LanguageModel LanguageOperate { get; set; } = new("Demo.Language", "Language", "Demo.Language.dll");
        #region 私有属性
        /// <summary>
        /// 当前连接器实例
        /// </summary>
        Connector ConnectedDevices { get; set; }
         UdpWrapper Wrapper { get; set; }

        /// <summary>
        /// X轴像素点
        /// </summary>
        int XPixelPoint = 0;
        #endregion

        #region 私有方法



        
        #endregion

        #region 通用接口函数
        public override OperateResult Gather(int value)
        {
            BegOperate();
            try
            {         
                if (!GetWavelength().GetDetails(out string? message, out object? obj))
                {
                    return EndOperate(false, message);
                }
                float[] waveLength = obj.GetSource<float[]>();
                if(!GetSptmData(value,XPixelPoint,res => { }, res => { }).GetDetails(out message,out obj))
                {
                    return EndOperate(false, message);
                }
                int[] intensity = obj.GetSource<int[]>();

                SpectrumDto spectrum = new SpectrumDto();
               // spectrum.Wavelength = waveLength;
                //spectrum.Intensity = intensity;
                return EndOperate(true, resultData: spectrum);

            }
            catch (Exception ex)
            {
                return EndOperate(false,ex.Message);
            }
        }

        public override OperateResult GetStatus()
        {
            BegOperate();
            try
            {
                if(Wrapper!=null)
                {
                    return EndOperate(true, LanguageOperate.GetLanguageValue("已连接"), logOutput: false);
                }
                else
                {
                    return EndOperate(true, LanguageOperate.GetLanguageValue("未连接"), logOutput: false);

                }
            }
            catch (Exception ex)
            {
                return EndOperate(false,ex.Message);
            }
        }

        public override OperateResult GetTemperature()
        {
            throw new NotImplementedException();
        }

        public override OperateResult Off(bool hardClose = false)
        {
            BegOperate();
            if (!hardClose)
            {
                if (!GetStatus().GetDetails(out string? message))
                {
                    return EndOperate(false, message);
                }
            }
            // 关闭设备
            ConnectedDevices.Dispose();       
            return EndOperate(true);

        }

        public override OperateResult On()
        {
            BegOperate();
            try
            {
                if (GetStatus().GetDetails(out string? message))
                {
                    return EndOperate(false, message);
                }

                //打开监听端口
                bool connected = false;
                ConnectedDevices = new UdpConnector();
                ConnectedDevices.Conn(new UdpParams
                {
                    IP = basics.SerialIP,
                    Port = basics.SerialPort,

                    Result = (res, d, errMsg) =>
                    {

                        if (res == -1)
                        {
                            connected = false;
                        }
                        else if (res == 1)
                        {
                            connected = true;
                            //加载UDP信息
                            Wrapper = new OptoskyConnector.Service.UdpWrapper(d);

                        }
                    }
                });
                return EndOperate(connected);
            }
            catch (Exception ex)
            {
                Off(true);
                return EndOperate(false, ex.Message, exception: ex);
            }

        }

        public override OperateResult SetTemperature(long value)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 设备命令函数

        /// <summary>
        /// 初始化设备
        /// </summary>
        /// <returns></returns>
        public OperateResult Init()
        {
            BegOperate();
            //获取pn
            if(!getProductPN().GetDetails(out string? message,out object? obj))
            {
                return EndOperate(false,message);
            }
            string productPn = obj.GetSource<string>();

           
            if(!GetPN(productPn).GetDetails(out message,out obj))
            {
                return EndOperate(false, message);
            }

            string pn = obj.GetSource<string>();

            //获取像素
            if (!GetCCDSize(pn).GetDetails(out message, out obj))
            {
                return EndOperate(false, message);
            }

            XPixelPoint = obj.GetSource<int>();

            return EndOperate(true);

        }

        /// <summary>
        /// 获取产品pn
        /// </summary>
        /// <param name="pn"></param>
        /// <returns></returns>
        public OperateResult GetPN(string pn)
        {
            BegOperate();
            string result = pn;
            if (string.IsNullOrEmpty(result))
                return EndOperate(false);
            int strLength = result.IndexOf("#");
            if (strLength > 0)
                result = result.Substring(0, strLength);
            return EndOperate(true,resultData:result);
        }

        /// <summary>
        /// 获取产品型号
        /// </summary>
        /// <returns></returns>
        public OperateResult getProductPN()
        {
            BegOperate();
            try
            {
                string ret = Wrapper.getProductPN();
                return EndOperate(true, resultData: ret);
            }
            catch (Exception ex) 
            {
                return EndOperate(false, ex.Message);
            }
           

        }

        /// <summary>
        /// 获取像素长度
        /// </summary>
        /// <param name="pn"></param>
        /// <returns></returns>
        public OperateResult GetCCDSize(string pn)
        {
            BegOperate();
            try
            {
                int ret = Wrapper.GetCCDSize(pn);
                return EndOperate(true, resultData: ret);
            }
            catch (Exception ex)
            {
                return EndOperate(false, ex.Message);
            }
        }

        /// <summary>
        /// 获取X轴波长数据
        /// </summary>
        /// <returns></returns>
        public OperateResult GetWavelength()
        {
            BegOperate();
            try
            {
                float[] ret = Wrapper.getWavelength(); 
                return EndOperate(true, resultData: ret);
            }
            catch (Exception ex)
            {
                return EndOperate(false, ex.Message);
            }
        }

        /// <summary>
        /// 获取光谱强度数据
        /// </summary>
        /// <returns></returns>
        public OperateResult GetSptmData(int integrationtime, int pixelVaule, Action<string> errMsg, Action<Exception> err, OddOrEven oddOrEven = OddOrEven.None, double? dominantFrequency = null)
        {
            BegOperate();
            try
            {
                int[] ret = Wrapper.GetSptmData(integrationtime, pixelVaule, errMsg, err, oddOrEven, dominantFrequency);
                return EndOperate(true, resultData: ret);
            }
            catch (Exception ex)
            {
                return EndOperate(false, ex.Message);
            }
        }

        #endregion
    }

}
