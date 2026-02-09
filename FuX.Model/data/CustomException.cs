using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.data
{
    //
    // 摘要:
    //     自定义异常类
    public class CustomException : Exception
    {
        //
        // 摘要:
        //     自定义异常构造函数
        //
        // 参数:
        //   message:
        //     异常信息
        //
        //   methodName:
        //     方法名称
        public CustomException(string message, [CallerMemberName] string methodName = "")
            : base(methodName + " exception: " + message)
        {
        }

        //
        // 摘要:
        //     自定义异常构造函数
        //
        // 参数:
        //   message:
        //     异常信息
        //
        //   innerException:
        //     底层异常对象
        //
        //   methodName:
        //     方法名称
        public CustomException(string message, Exception innerException, [CallerMemberName] string methodName = "")
            : base(methodName + " exception: " + message, innerException)
        {
        }

        //
        // 摘要:
        //     创建自定义异常
        //
        // 参数:
        //   message:
        //     异常信息
        //
        //   methodName:
        //     方法名称
        //
        // 返回结果:
        //     返回快速创建的对象
        public static CustomException Create(string message, [CallerMemberName] string methodName = "")
        {
            return new CustomException(message, methodName);
        }

        //
        // 摘要:
        //     异步创建自定义异常
        //
        // 参数:
        //   message:
        //     异常信息
        //
        //   token:
        //     传播消息取消通知
        //
        //   methodName:
        //     方法名称
        //
        // 返回结果:
        //     返回快速创建的对象
        public static Task<CustomException> CreateAsync(string message, CancellationToken token = default(CancellationToken), [CallerMemberName] string methodName = "")
        {
            string message2 = message;
            string methodName2 = methodName;
            return Task.Run(() => Create(message2, methodName2), token);
        }

        //
        // 摘要:
        //     创建自定义异常 异常信息 底层异常对象 方法名称
        //
        // 返回结果:
        //     返回自定义异常的对象
        public static CustomException Create(string message, Exception innerException, [CallerMemberName] string methodName = "")
        {
            return new CustomException(message, innerException, methodName);
        }

        //
        // 摘要:
        //     异步创建自定义异常 异常信息 底层异常对象 传播消息取消通知 方法名称
        //
        // 返回结果:
        //     返回自定义异常的对象
        public static Task<CustomException> CreateAsync(string message, Exception innerException, CancellationToken token = default(CancellationToken), [CallerMemberName] string methodName = "")
        {
            string message2 = message;
            Exception innerException2 = innerException;
            string methodName2 = methodName;
            return Task.Run(() => Create(message2, innerException2, methodName2), token);
        }
    }
}
