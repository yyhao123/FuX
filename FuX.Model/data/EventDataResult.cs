using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.data
{
    //
    // 摘要:
    //     事件数据结果
    public class EventDataResult : ResultModel
    {
        //
        // 摘要:
        //     事件数据结果；
        //     无参构造函数
        public EventDataResult()
        {
        }

        //
        // 摘要:
        //     事件数据结果；
        //     有参构造函数
        //
        // 参数:
        //   result:
        //     事件数据结果
        public EventDataResult(EventDataResult result)
            : base(result.Status, result.Message, result.ResultData)
        {
        }

        //
        // 摘要:
        //     事件数据结果；
        //     有参构造函数
        //
        // 参数:
        //   status:
        //     状态
        //
        //   message:
        //     消息
        //
        //   resultData:
        //     结果数据
        public EventDataResult(bool status, string message, object? resultData = null)
            : base(status, message, resultData)
        {
        }

        //
        // 摘要:
        //     快速创建一个成功的结果
        //
        // 参数:
        //   successMessage:
        //     成功消息
        //
        // 返回结果:
        //     结果对象
        public static EventDataResult CreateSuccessResult(string successMessage)
        {
            return new EventDataResult(status: true, successMessage);
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
        // 返回结果:
        //     结果对象
        public static EventDataResult CreateSuccessResult<T>(string successMessage, T resultData)
        {
            return new EventDataResult(status: true, successMessage, resultData);
        }

        //
        // 摘要:
        //     快速创建一个失败的结果
        //
        // 参数:
        //   failureMessage:
        //     失败的消息
        //
        // 返回结果:
        //     结果对象
        public static EventDataResult CreateFailureResult(string failureMessage)
        {
            return new EventDataResult(status: false, failureMessage);
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
        public bool GetDetails(out EventDataResult result)
        {
            result = this;
            return base.Status;
        }
    }
}
