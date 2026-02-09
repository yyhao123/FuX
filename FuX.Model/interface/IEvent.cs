using FuX.Model.data;
using FuX.Unility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.@interface
{
    //
    // 摘要:
    //     事件接口
    public interface IEvent
    {
        //
        // 摘要:
        //     数据传递事件
        event EventHandler<EventDataResult> OnDataEvent;

        //
        // 摘要:
        //     数据传递事件异步
        event EventHandlerAsync<EventDataResult> OnDataEventAsync;

        //
        // 摘要:
        //     信息传递事件
        event EventHandler<EventInfoResult> OnInfoEvent;

        //
        // 摘要:
        //     信息传递事件异步
        event EventHandlerAsync<EventInfoResult> OnInfoEventAsync;

        //
        // 摘要:
        //     语言传递事件
        event EventHandler<EventLanguageResult> OnLanguageEvent;

        //
        // 摘要:
        //     语言传递事件异步
        event EventHandlerAsync<EventLanguageResult> OnLanguageEventAsync;
    }
}
