using _Project.Scripts.Contracts.Interfaces;
using _Project.Scripts.View.Map;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.Scopes
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