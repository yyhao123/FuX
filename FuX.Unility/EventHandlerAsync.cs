using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Unility
{
    public delegate Task EventHandlerAsync<in TEvent>(object? sender, TEvent e) where TEvent : EventArgsAsync;
}
