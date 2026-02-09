using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.data
{
    //
    // 摘要:
    //     操作结果
    public class OperateResult : ResultModel
    {
        //
        // 摘要:
        //     此函数运行时间 ( ms )
        public int RunTime { get; set; }

        //
        // 摘要:
        //     操作结果；
        //     无参构造函数
        public OperateResult()
        {
        }

        //
        // 摘要:
        //     操作结果；
        //     有参构造函数
        //
        // 参数:
        //   result:
        //     操作结果
        public OperateResult(OperateResult result)
        {
            base.Status = result.Status;
            base.Time = result.Time;
            RunTime = result.RunTime;
            base.ResultData = result.ResultData;
            base.Message = result.Message;
        }

        //
        // 摘要:
        //     操作结果；
        //     有参构造函数
        //
        // 参数:
        //   result:
        //     操作结果
        //
        //   rumTime:
        //     此函数运行时间
        public OperateResult(OperateResult result, int rumTime)
        {
            base.Status = result.Status;
            base.Time = result.Time;
            RunTime = rumTime;
            base.ResultData = result.ResultData;
            base.Message = result.Message;
        }

        //
        // 摘要:
        //     操作结果；
        //     有参构造函数
        //
        // 参数:
        //   status:
        //     状态
        //
        //   message:
        //     消息
        //
        //   runTime:
        //     此函数运行时间
        //
        //   resultData:
        //     结果数据
        public OperateResult(bool status, string message, int runTime, object? resultData = null)
            : base(status, message, resultData)
        {
            RunTime = runTime;
        }

        //
        // 摘要:
        //     快速创建一个成功的结果
        //
        // 参数:
        //   successMessage:
        //     成功消息
        //
        //   runTime:
        //     运行时间
        //
        // 返回结果:
        //     结果对象
        public static OperateResult CreateSuccessResult(string successMessage, int? runTime = null)
        {
            return new OperateResult
            {
                Status = true,
                Message = successMessage,
                RunTime = runTime.GetValueOrDefault(8)
            };
        }

        //
        // 摘要:
        //     快速创建一个成功的结果
        //
        // 参数:
        //   successMessage:
        //     成功消息
        //
        //   resultData:
        //     结果数据
        //
        //   runTime:
        //     运行时间
        //
        // 返回结果:
        //     结果对象
        public static OperateResult CreateSuccessResult<T>(string successMessage, T resultData, int? runTime = null)
        {
            return new OperateResult
            {
                Status = true,
                ResultData = resultData,
                Message = successMessage,
                RunTime = runTime.GetValueOrDefault(8)
            };
        }

        //
        // 摘要:
        //     快速创建一个失败的结果
        //
        // 参数:
        //   failureMessage:
        //     失败的消息
        //
        //   runTime:
        //     运行时间
        //
        // 返回结果:
        //     结果对象
        public static OperateResult CreateFailureResult(string failureMessage, int? runTime = null)
        {
            return new OperateResult
            {
                Status = false,
                Message = failureMessage,
                RunTime = runTime.GetValueOrDefault(8)
            };
        }

        //
        // 摘要:
        //     获取详情
        //
        // 参数:
        //   result:
        //     抛出结果对象
        //
        // 返回结果:
        //     获取状态
        public bool GetDetails(out OperateResult result)
        {
            result = this;
            return base.Status;
        }
    }
}
