using _Project._Common.Scripts.Contracts.Interfaces;
using _Project.Features.Surfaces.Common.Scripts.Contracts;
using _Project.Features.Surfaces.Common.Scripts.Infrastructure;
using _Project.Features.Surfaces.Stick.Scripts.Presentation;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Features.Surfaces.Stick.Scripts.Infrastructure
{
    public class StickSurfaceScope : LifetimeScope
    {
        [SerializeField] private StickableSurfaceConfigSo configSo;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(configSo).As<IStickableSurfaceConfig>();
            builder.Register<StickSurfacePresenter>(Lifetime.Scoped).As<ISurfacePresenter>();
        }
    }
}