using System;
using System.Collections.Generic;
using VContainer;

namespace _Project._Common.Scripts.Infrastructure.ZeroMessenger
{
    public static class ZeroMessengerServiceExtensions
    {
        public static RegistrationBuilder RegisterZeroMessengerService(this IContainerBuilder builder, Lifetime lifetime)
            => builder.Register<ZeroMessengerService>(lifetime)
                .As<IZeroMessengerService>();
    }

    public class ZeroMessengerService : IDisposable, IZeroMessengerService
    {
        private readonly Dictionary<(object, Type), ZeroMessengerDispatcher> _dispatchers = new ();
        private readonly ZeroMessengerDispatcher _globalDispatcher = new ();

        public IDisposable Subscribe<T>(Action<T> handler)
            => _globalDispatcher.Subscribe(handler);
        
        public void Publish<T>(T message)
            => _globalDispatcher.Publish(message);
        
        public IDisposable Subscribe<TKey, TValue>(TKey key, Action<TValue> handler)
        {
            var dispatcherKey = (key, typeof(TKey));
            if (_dispatchers.TryGetValue(dispatcherKey, out var dispatcher))
                return dispatcher.Subscribe(handler);
            
            dispatcher = new ZeroMessengerDispatcher();
            _dispatchers[dispatcherKey] = dispatcher;

            return dispatcher.Subscribe(handler);
        }
        
        public void Publish<TKey, TValue>(TKey key, TValue message)
        {
            var dispatcherKey = (key, typeof(TKey));
            if (_dispatchers.TryGetValue(dispatcherKey, out var dispatcher))
                dispatcher.Publish(message);
        }
        
        public void Dispose()
        {
            foreach (var pair in _dispatchers)
                pair.Value.Dispose();
            _dispatchers.Clear();
            _globalDispatcher.Dispose();
        }
    }
}