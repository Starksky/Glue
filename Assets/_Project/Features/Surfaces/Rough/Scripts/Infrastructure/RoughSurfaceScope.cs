using _Project.Features.Surfaces.Common.Scripts.Contracts;
using _Project.Features.Surfaces.Rough.Scripts.Contracts;
using _Project.Features.Surfaces.Rough.Scripts.Presentation;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Features.Surfaces.Rough.Scripts.Infrastructure
{
    public class RoughSurfaceScope : LifetimeScope
    {
        [SerializeField] private RoughSurfaceConfigSo configSo;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(configSo).As<IRoughSurfaceConfig>();
            builder.Register<RoughSurfacePresenter>(Lifetime.Scoped).As<ISurfacePresenter>();
        }
    }
}