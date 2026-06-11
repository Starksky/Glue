using _Project._Common.Scripts.Contracts.Interfaces;
using _Project.Features.Map.Scripts.View;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Features.Map.Scripts.Infrastructure
{
    public class MapScope : LifetimeScope
    {
        [SerializeField] private MapView mapView;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(mapView).As<IMapView>();
            builder.RegisterBuildCallback(container =>
            {
                var service = container.Resolve<ISessionService<IMapView>>();
                var resolved = container.Resolve<IMapView>();
                service.Registration(resolved);
            });
        }
    }
}