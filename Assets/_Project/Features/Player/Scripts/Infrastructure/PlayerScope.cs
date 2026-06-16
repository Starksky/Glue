using _Project._Common.Scripts.Contracts.Interfaces;
using _Project._Common.Scripts.Infrastructure.Adapters;
using _Project._Common.Scripts.Infrastructure.PoolObject;
using _Project.Features.Player.Scripts.Application;
using _Project.Features.Player.Scripts.Contracts;
using _Project.Features.Player.Scripts.Domain;
using _Project.Features.Player.Scripts.Infrastructure.Repositories;
using _Project.Features.Player.Scripts.Presentation;
using _Project.Features.Player.Scripts.View;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Features.Player.Scripts.Infrastructure
{
    public class PlayerScope : LifetimeScope
    {
        [SerializeField] private UnityPhysicBody2D playerBody;
        [SerializeField] private PlayerBodyView playerBodyView;
        [SerializeField] private PlayerSlingshotView playerSlingshotView;
        [SerializeField] private PlayerSlingshotVisualView playerSlingshotVisualView;
        [SerializeField] private MonoPoolable playerPoolable;
        [SerializeField] private PlayerSlingshotConfigSo playerSlingshotConfigSo;
        [SerializeField] private PlayerSlingshotVisualConfigSo playerSlingshotVisualConfigSo;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(playerPoolable).As<MonoPoolable>();
            
            builder.RegisterComponent(playerBody).As<IPhysicBody2D>();
            builder.RegisterComponent(playerBodyView).As<IPlayerBodyView>();
            builder.RegisterComponent(playerSlingshotView).As<IPlayerSlingshotView>();
            builder.RegisterComponent(playerSlingshotVisualView).As<IPlayerSlingshotVisualView>();
            
            builder.RegisterInstance(playerSlingshotConfigSo).As<IPlayerSlingshotConfig>();
            builder.RegisterInstance(playerSlingshotVisualConfigSo).As<IPlayerSlingshotVisualConfig>();

            builder.Register<PlayerSlingshotModel>(Lifetime.Scoped)
                .As<IPlayerSlingshotModel>()
                .AsImplementedInterfaces();
            builder.Register<PlayerSlingshotService>(Lifetime.Scoped)
                .As<IPlayerSlingshotService>()
                .AsImplementedInterfaces();
            builder.Register<PlayerSlingshotPresenter>(Lifetime.Scoped)
                .AsImplementedInterfaces();
            
            builder.RegisterBuildCallback(container =>
            {
                var service = container.Resolve<ISessionService<IPlayerBodyView>>();
                var resolved = container.Resolve<IPlayerBodyView>();
                service.Registration(resolved);
            });
        }
    }
}