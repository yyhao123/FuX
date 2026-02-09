using FuX.Model.nterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.@interface
{
    public interface IMq : IOn, IOff, IProducer, IConsumer, IEvent, IGetStatus, IGetParam, ICreateInstance, ILog, ILanguage, IDisposable
    {
    }

}
