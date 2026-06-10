using _Project.Scripts.Contracts.Interfaces;
using _Project.Scripts.Infrastructure.PoolObject;
using _Project.Scripts.Infrastructure.Services;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.Scopes
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

            builder.RegisterGameObjectPool(this);
        }
    }
}