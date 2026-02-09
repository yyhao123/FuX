using FuX.Model.@interface;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.@interface
{
    //
    // 摘要:
    //     底层通信接口
    public interface ICommunication : IOn, IOff, ISend, ISendWait, IGetObject, IGetStatus, IEvent, ICreateInstance, ILog, IGetParam, ILanguage, IDisposable
    {
    }
}
