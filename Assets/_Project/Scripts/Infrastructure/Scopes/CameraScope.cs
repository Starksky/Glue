using _Project.Scripts.Contracts.Interfaces;
using _Project.Scripts.View.Camera;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.Scopes
{
    public class CameraScope : LifetimeScope
    {
        [SerializeField] private CameraView cameraView;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(cameraView).As<ICameraView>();
            
            builder.RegisterBuildCallback(container =>
            {
                var sessionService = container.Resolve<ISessionService<ICameraView>>();
                var resolvedCamera = container.Resolve<ICameraView>();
                sessionService.Registration(resolvedCamera);
            });
        }
    }
}