using _Project.Features.Surfaces.Common.Scripts.Contracts;
using _Project.Features.Surfaces.Spring.Scripts.Contracts;
using _Project.Features.Surfaces.Spring.Scripts.Presentation;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Features.Surfaces.Spring.Scripts.Infrastructure
{
    public class SpringSurfaceScope : LifetimeScope
    {
        [SerializeField] private SpringSurfaceConfigSo configSo;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(configSo).As<ISpringSurfaceConfig>();
            builder.Register<SpringSurfacePresenter>(Lifetime.Scoped)
                .As<ISurfacePresenter>();
        }
    }
}