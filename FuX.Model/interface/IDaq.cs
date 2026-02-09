using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FuX.Model.@interface
{
    public interface IDaq : IOn, IOff, IRead, IWrite, ISubscribe, IEvent, IGetStatus, IGetParam, ICreateInstance, ILog, IWA, IGetObject, ILanguage, IDisposable
    {
    }
}
