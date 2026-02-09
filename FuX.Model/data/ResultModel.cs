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
    //     结果模型
    public class ResultModel : EventArgsAsync
    {
        //
        // 摘要:
        //     描述信息
        public string? Message { get; set; }

        //
        // 摘要:
        //     状态
        public bool Status { get; set; }

        //
        // 摘要:
        //     响应时间
        public DateTime Time { get; set; } = DateTime.Now;


        //
        // 摘要:
        //     结果数据
        public object? ResultData { get; set; }

        //
        // 摘要:
        //     结果；
        //     无参构造函数
        public ResultModel()
        {
        }

        //
        // 摘要:
        //     结果；
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
        public ResultModel(bool status, string? message, object? resultData = null)
        {
            Status = status;
            Message = message;
            ResultData = resultData;
        }

        //
        // 摘要:
        //     获取结果数据源
        //
        // 类型参数:
        //   T:
        //     数据类型
        //
        // 返回结果:
        //     指定类型的数据
        public T? GetSource<T>()
        {
            if (ResultData != null)
            {
                return ResultData.GetSource<T>();
            }

            return default(T);
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
        //   resultData:
        //     抛出结果数据
        //
        // 返回结果:
        //     获取状态
        public bool GetDetails(out object? resultData)
        {
            resultData = ResultData;
            return Status;
        }

        //
        // 摘要:
        //     获取详情
        //
        // 参数:
        //   resultData:
        //     抛出结果数据源
        //
        // 类型参数:
        //   T:
        //     数据类型
        //
        // 返回结果:
        //     获取状态
        public bool GetDetails<T>(out T? resultData)
        {
            if (ResultData != null)
            {
                resultData = ResultData.GetSource<T>();
            }
            else
            {
                resultData = default(T);
            }

            return Status;
        }

        //
        // 摘要:
        //     获取详情
        //
        // 参数:
        //   resultData:
        //     抛出结果数据
        //
        //   message:
        //     抛出描述信息
        //
        // 返回结果:
        //     获取状态
        public bool GetDetails(out object? resultData, out string? message)
        {
            message = Message;
            resultData = ResultData;
            return Status;
        }

        //
        // 摘要:
        //     获取详情
        //
        // 参数:
        //   resultData:
        //     抛出结果数据源
        //
        //   message:
        //     抛出描述信息
        //
        // 类型参数:
        //   T:
        //     数据类型
        //
        // 返回结果:
        //     获取状态
        public bool GetDetails<T>(out T? resultData, out string? message)
        {
            if (ResultData != null)
            {
                resultData = ResultData.GetSource<T>();
            }
            else
            {
                resultData = default(T);
            }

            message = Message;
            return Status;
        }

        //
        // 摘要:
        //     获取详情
        //
        // 参数:
        //   resultData:
        //     抛出结果数据
        //
        //   message:
        //     抛出描述信息
        //
        // 返回结果:
        //     获取状态
        public bool GetDetails(out string? message, out object? resultData)
        {
            message = Message;
            resultData = ResultData;
            return Status;
        }

        //
        // 摘要:
        //     获取详情
        //
        // 参数:
        //   resultData:
        //     抛出结果数据源
        //
        //   message:
        //     抛出描述信息
        //
        // 类型参数:
        //   T:
        //     数据类型
        //
        // 返回结果:
        //     获取状态
        public bool GetDetails<T>(out string? message, out T? resultData)
        {
            if (ResultData != null)
            {
                resultData = ResultData.GetSource<T>();
            }
            else
            {
                resultData = default(T);
            }

            message = Message;
            return Status;
        }

        //
        // 摘要:
        //     重写ToString；
        //     响应 json 字符串
        public override string ToString()
        {
            return this.ToJson(formatting: true) ?? string.Empty;
        }
    }
}
