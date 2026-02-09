using Demo.Model.data;
using Demo.Model.@enum;
using Demo.Windows.Core.handler;
using FuX.Model.data;
using FuX.Model.@interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.extend
{
    /// <summary>
    /// 核心扩展类
    /// </summary>
    public static class CoreExtend
    {
        /// <summary>
        /// 响应结果模型默认值
        /// </summary>
        private static List<ResponseResultModel> responseResultModels = new List<ResponseResultModel>
        {
            new ResponseResultModel(0x00, true, LanguageHandler.GetLanguageValue("成功")),
            new ResponseResultModel(0x01, false, LanguageHandler.GetLanguageValue("失败"))
        };

        /// <summary>
        /// 获得ATM6370组包模型
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="data">数据，可为空</param>
        /// <returns>组包模型</returns>
        public static PackageModel GetPackageModel(this byte command, byte[]? data = null)
        {
            return new((byte)command, data);
        }

        /// <summary>
        /// 获得ATM6370组包模型
        /// </summary>
        /// <param name="bytes">上报的字节数据</param>
        /// <returns>组包模型</returns>
        public static PackageModel GetPackageModel(this byte[] bytes)
        {
            return new PackageModel().SetBytes(bytes);
        }

        /// <summary>
        /// 响应判读是否成功，只适用于返回的数据为单个字节时使用 <br/> 为空则默认 0x00：成功 - 0x01：失败
        /// </summary>
        /// <param name="operateResult">操作结果</param>
        /// <param name="responses">响应结果模型，用于判断</param>
        /// <returns>统一结果</returns>
        public static OperateResult ResponseHandler(this OperateResult operateResult, List<ResponseResultModel>? responses = null)
        {
            //创建默认值
            if (responses == null)
            {
                responses = responseResultModels;
            }
            //判断包体是否为空
            PackageModel? package = operateResult.GetSource<PackageModel>();
            if (package != null)
            {
                ResponseResultModel result = responses.Where(c => c.Data == package.Data[0]).ToArray()[0];
                return new OperateResult(result.Status, result.Message, operateResult.RunTime + 1);
            }
            return operateResult;
        }

        /// <summary>
        /// 构建通用单次应答模式处理
        /// </summary>
        /// <param name="communication">通信库对象</param>
        /// <param name="command">命令</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static  OperateResult SingleHandler(this ICommunication communication, byte command, byte[]? data = null)
        {
            //构建出一条命令
            byte[] bytes = command.GetPackageModel(data).GetBytes(out PackageModel model);

            //通过串口发送等待结果
            OperateResult? result = communication.SendWait(bytes, CancellationToken.None);

            

            //获取底层上报的数据
            byte[]? reportBytes = result?.GetSource<byte[]>();

            //判断字节是否为空
            if (reportBytes != null)
            {
                //进行转换
                PackageModel package = reportBytes.GetPackageModel().SetBytes(reportBytes);

                //进行解析


                //返回状态判断
                return OperateResult.CreateSuccessResult(LanguageHandler.GetLanguageValue("命令执行成功，并成功返回数据"), package);
            }
            else
            {
                return OperateResult.CreateFailureResult(LanguageHandler.GetLanguageValue("命令执行失败") + $":{result?.Message}");
            }
        }
    }
}
