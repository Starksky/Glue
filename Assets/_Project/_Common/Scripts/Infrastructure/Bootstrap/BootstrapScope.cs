using _Project._Common.Scripts.Infrastructure.Adapters;
using _Project._Common.Scripts.Infrastructure.Services;
using _Project.Scripts.Contracts.Interfaces;
using SaintsField;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project._Common.Scripts.Infrastructure.Bootstrap
{
    public class BootstrapScope : LifetimeScope
    {
        [SerializeField, Scene] private string sceneName;
        [SerializeField] private UnityLoaderScreen loaderScreenPrefab;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab(loaderScreenPrefab, Lifetime.Singleton)
                .As<ILoaderScreen>();
            builder.Register<SceneLoaderService>(Lifetime.Singleton)
                .As<ISceneLoaderService>();
            
            builder.RegisterEntryPoint<BootstrapEntryPoint>().WithParameter(sceneName);
        }
    }
}