using _Project._Common.Scripts.Contracts;
using _Project._Common.Scripts.Contracts.Interfaces;
using _Project._Common.Scripts.Infrastructure.PoolObject;
using _Project._Common.Scripts.Infrastructure.Services;
using _Project._Common.Scripts.Infrastructure.ZeroMessenger;
using VContainer;
using VContainer.Unity;

namespace _Project._Common.Scripts.Infrastructure.Scopes
{
    public class GameScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SessionService<ICameraView>>(Lifetime.Singleton)
                .As<ISessionService<ICameraView>>();
            builder.Register<SessionService<IPlayerBodyView>>(Lifetime.Singleton)
                .As<ISessionService<IPlayerBodyView>>();
            builder.Register<SessionService<IMapView>>(Lifetime.Singleton)
                .As<ISessionService<IMapView>>();

            builder.RegisterGameObjectPool(this, Lifetime.Singleton);
            builder.RegisterZeroMessengerService(Lifetime.Singleton);
        }
    }
}