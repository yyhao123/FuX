using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    /// <summary>
    /// 响应结果模型 <br/> 此模型正对返回一个字节的情况判断使用
    /// </summary>
    public class ResponseResultModel
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ResponseResultModel()
        { }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="status">状态</param>
        /// <param name="message">提示信息</param>
        public ResponseResultModel(byte data, bool status, string message)
        {
            this.Message = message;
            this.Status = status;
            this.Data = data;
        }

        /// <summary>
        /// 字节数据
        /// </summary>
        public byte Data { get; set; }

        /// <summary>
        /// 提示消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 代表成功还是失败
        /// </summary>
        public bool Status { get; set; }
    }
}
