using R3;

namespace _Project._Common.Scripts.Contracts.Interfaces
{
    public interface ISessionService<T>
    {
        public ReadOnlyReactiveProperty<T> Session { get; }
        public void Registration(T session);
    }
}