using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuX.Unility;

namespace FuX.Unility
{
    public struct EventingWrapperAsync<TEvent> where TEvent : EventArgsAsync
    {
        private Delegate[]? _handlers;

        private string? _context;

        private Func<Exception, string, CancellationToken, Task>? _onException;

        public bool IsEmpty => this._eventHandler == null;

        private event EventHandlerAsync<TEvent>? _eventHandler;

        public EventingWrapperAsync(string context, Func<Exception, string, CancellationToken, Task> onException)
        {
            this._eventHandler = null;
            _handlers = null;
            _context = context;
            _onException = onException;
        }

        public void AddHandler(EventHandlerAsync<TEvent>? handler)
        {
            _eventHandler += handler;
            _handlers = null;
        }

        public void RemoveHandler(EventHandlerAsync<TEvent>? handler)
        {
            _eventHandler -= handler;
            _handlers = null;
        }

        public Task InvokeAsync(object? sender, TEvent parameter)
        {
            Delegate[] array = _handlers;
            if (array == null)
            {
                array = this._eventHandler?.GetInvocationList();
                if (array == null)
                {
                    return Task.CompletedTask;
                }
                _handlers = array;
            }
            return InternalInvoke(array, sender, parameter);
        }

        private readonly async Task InternalInvoke(Delegate[] handlers, object? sender, TEvent @event)
        {
            for (int i = 0; i < handlers.Length; i++)
            {
                EventHandlerAsync<TEvent> eventHandlerAsync = (EventHandlerAsync<TEvent>)handlers[i];
                try
                {
                    await eventHandlerAsync(sender, @event).ConfigureAwait(continueOnCapturedContext: false);
                }
                catch (Exception arg)
                {
                    if (_onException == null)
                    {
                        throw;
                    }
                    await _onException(arg, _context, @event.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                }
            }
        }

        public void Takeover(in EventingWrapperAsync<TEvent> other)
        {
            this._eventHandler = other._eventHandler;
            _handlers = other._handlers;
            _context = other._context;
            _onException = other._onException;
        }
    }
}
