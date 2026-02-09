using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.@interface
{
    public interface ICommunicationService : IOn, IOff, ISendService, IGetObject, IGetStatus, IRemove, IEvent, ICreateInstance, ILog, ILanguage, IDisposable
    {
    }
}
