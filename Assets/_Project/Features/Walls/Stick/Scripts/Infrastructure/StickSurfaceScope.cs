using _Project._Common.Scripts.Contracts.Interfaces;
using _Project.Features.Walls.Stick.Scripts.Contracts;
using _Project.Features.Walls.Stick.Scripts.Infrastructure.Repositories;
using _Project.Features.Walls.Stick.Scripts.Presentation;
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