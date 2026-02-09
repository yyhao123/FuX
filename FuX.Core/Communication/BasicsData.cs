using FuX.Unility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Core.Communication
{
    //
    // 摘要:
    //     基础数据
    public class BasicsData
    {
        //
        // 摘要:
        //     唯一标识符
        [Category("基础数据")]
        [Description("唯一标识符")]
        public string? SN { get; set; } = Guid.NewGuid().ToUpperNString();


        //
        // 摘要:
        //     发送等待间隔
        //     继承者可选重写
        [Description("发送等待间隔")]
        public virtual int SendWaitInterval { get; set; } = 10000;


        //
        // 摘要:
        //     最大块大小；
        //     如果数据超过限定值则自动分包发送
        //     继承者可选重写
        [Description("最大块大小")]
        public virtual int MaxChunkSize { get; set; } = 261120;


        //
        // 摘要:
        //     重试发送次数；
        //     只在分包发送中触发；
        //     在限定次数中还是失败则直接返回失败
        //     继承者可选重写
        [Description("重试发送次数")]
        public virtual int RetrySendCount { get; set; } = 5;


        //
        // 摘要:
        //     数据缓冲区大小
        //     继承者可选重写
        [Description("数据缓冲区大小")]
        public virtual int BufferSize { get; set; } = 1048576;

    }
}
