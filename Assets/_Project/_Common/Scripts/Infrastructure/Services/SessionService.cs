using System;
using _Project._Common.Scripts.Contracts.Interfaces;
using R3;

namespace _Project._Common.Scripts.Infrastructure.Services
{
    public class SessionService<T> : ISessionService<T>, IDisposable
    {
        private ReactiveProperty<T> _session = new ReactiveProperty<T>();
        public ReadOnlyReactiveProperty<T> Session => _session;
        
        public void Registration(T session)
        {
            _session.Value = session;
        }
        
        public void Dispose()
        {
            _session.Dispose();
        }
    }
}