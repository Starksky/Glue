using System;
using System.Collections.Generic;
using Zenject;

namespace Project.Shared.Scripts.ForMessages
{
    public class MessageBrokersService : IInitializable, IDisposable
    {
        private readonly string _defaultChanel = "_default";
        private readonly Dictionary<string, MessageDispatcher> _messageDispatchers = new Dictionary<string, MessageDispatcher>();
        
        public void Initialize()
        {
            
        }

        public IDisposable Subscribe<T>(Action<T> handler) => Subscribe(_defaultChanel, handler);
        public void Publish<T>(T message) => Publish(_defaultChanel, message);
        
        public IDisposable Subscribe<T>(string chanel, Action<T> handler)
        {
            chanel = string.IsNullOrEmpty(chanel) ? _defaultChanel : chanel;
            if (!_messageDispatchers.TryGetValue(chanel, out var dispatcher))
            {
                dispatcher = new MessageDispatcher();
                _messageDispatchers[chanel] = dispatcher;
            }
            
            return dispatcher.Subscribe(handler);
        }
        
        public void Publish<T>(string chanel, T message)
        {
            chanel = string.IsNullOrEmpty(chanel) ? _defaultChanel : chanel;
            if (_messageDispatchers.TryGetValue(chanel, out var dispatcher))
                dispatcher.Publish(message);
        }
        
        public void Dispose()
        {
            foreach (var pair in _messageDispatchers)
                pair.Value.Dispose();
            _messageDispatchers.Clear();
        }
    }
}