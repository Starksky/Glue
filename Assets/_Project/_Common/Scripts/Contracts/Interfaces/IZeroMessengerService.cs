using System;

namespace _Project._Common.Scripts.Infrastructure.ZeroMessenger
{
    public interface IZeroMessengerService
    {
        IDisposable Subscribe<T>(Action<T> handler);
        IDisposable Subscribe<TKey, TValue>(TKey key, Action<TValue> handler);
        void Publish<T>(T message);
        void Publish<TKey, TValue>(TKey key, TValue message);
    }
}