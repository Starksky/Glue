using _Project._Common.Scripts.Infrastructure.Repositories.Surfaces;
using _Project.Scripts.Contracts.Interfaces;
using _Project.Scripts.Presentation.Surfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Features.Walls.Stick.Scripts.Infrastructure
{
    public class StickSurfaceScope : LifetimeScope
    {
        [SerializeField] private StickSurfaceConfigSo configSo;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(configSo).As<IStickSurfaceConfig>();
            builder.Register<StickSurfacePresenter>(Lifetime.Scoped).As<ISurfacePresenter>();
        }
    }
}