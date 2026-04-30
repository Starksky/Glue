using System;
using System.Collections.Generic;
using ZeroMessenger;

namespace Project.Shared.Scripts.ForMessages
{
    public class MessageDispatcher : IDisposable
    {
        private readonly Dictionary<Type, IDisposable> _subscriptions = new();
        private readonly Dictionary<Type, object> _brokers = new();
    
        public IDisposable Subscribe<T>(Action<T> handler)
        {
            if (!_brokers.ContainsKey(typeof(T)))
                _brokers[typeof(T)] = new MessageBroker<T>();

            var broker = (MessageBroker<T>)_brokers[typeof(T)];
            var subscription = broker.Subscribe(handler);
            _subscriptions[typeof(T)] = subscription;
            return subscription;
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
            foreach (var sub in _subscriptions.Values)
                sub.Dispose();
            foreach (var broker in _brokers.Values)
                (broker as IDisposable)?.Dispose();
        }
    }
}