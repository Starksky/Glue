using System;
using System.Collections.Generic;
using ZeroMessenger;

namespace Project.Shared.Scripts.ForMessages
{
    public class MessageDispatcher : IDisposable
    {
        private readonly Dictionary<Type, object> _brokers = new();
    
        public IDisposable Subscribe<T>(Action<T> handler)
        {
            if (!_brokers.TryGetValue(typeof(T), out var brokerObj))
            {
                brokerObj = new MessageBroker<T>();
                _brokers[typeof(T)] = brokerObj;
            }

            var broker = (MessageBroker<T>)brokerObj;
            return broker.Subscribe(handler);
        }
    
        public void Publish<T>(T message)
        {
            if (_brokers.TryGetValue(typeof(T), out var brokerObj))
            {
                var broker = (MessageBroker<T>)brokerObj;
                broker.Publish(message);
            }
        }
    
        public void Dispose()
        {
            foreach (var broker in _brokers.Values)
                (broker as IDisposable)?.Dispose();
        }
    }
}