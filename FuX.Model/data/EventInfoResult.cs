using FuX.Unility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.data
{
    //
    // 摘要:
    //     事件信息结果
    public class EventInfoResult : EventArgsAsync
    {
        //
        // 摘要:
        //     信息
        public string? Message { get; set; }

        //
        // 摘要:
        //     状态
        public bool Status { get; set; }

        //
        // 摘要:
        //     时间
        public DateTime Time { get; set; } = DateTime.Now;


        //
        // 摘要:
        //     事件信息结果；
        //     无参构造函数
        public EventInfoResult()
        {
        }

        //
        // 摘要:
        //     事件信息结果；
        //     有参构造函数
        //
        // 参数:
        //   result:
        //     事件信息结果
        public EventInfoResult(EventInfoResult result)
        {
            Message = result.Message;
            Status = result.Status;
            Time = result.Time;
        }

        //
        // 摘要:
        //     事件信息结果；
        //     有参构造函数
        //
        // 参数:
        //   status:
        //     状态
        //
        //   message:
        //     消息
        public EventInfoResult(bool status, string message)
        {
            Status = status;
            Message = message;
        }

        //
        // 摘要:
        //     获取详情
        //
        // 参数:
        //   message:
        //     抛出描述信息
        //
        // 返回结果:
        //     获取状态
        public bool GetDetails(out string? message)
        {
            message = Message;
            return Status;
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
        public bool GetDetails(out EventInfoResult result)
        {
            result = this;
            return Status;
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
        public static EventInfoResult CreateSuccessResult(string successMessage)
        {
            return new EventInfoResult(status: true, successMessage);
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
        public static EventInfoResult CreateFailureResult(string failureMessage)
        {
            return new EventInfoResult(status: false, failureMessage);
        }

        //
        // 摘要:
        //     重写ToString；
        //     响应 json 字符串
        //
        // 返回结果:
        //     json 字符串
        public override string ToString()
        {
            return this.ToJson(formatting: true) ?? string.Empty;
        }
    }
}
