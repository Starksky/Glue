using _Project._Common.Scripts.Contracts.Interfaces;
using _Project._Common.Scripts.Utils;
using SaintsField;
using UnityEngine;
using VContainer;

namespace _Project.Features.Map.Scripts.View
{
    [RequireComponent(typeof(Grid))]
    public class MapView : MonoBehaviour, IMapView
    {
        [SerializeField, ReadOnly, GetComponent(typeof(Grid))]
        private Grid grid;
        
        private ISessionService<IMapView> _mapSessionService;
        
        public Bounds Bounds => grid.GetBounds();
        
        [Inject]
        public void Construct(ISessionService<IMapView> mapSessionService)
        {
            _mapSessionService = mapSessionService;
        }

        private void Awake()
        {
            _mapSessionService.Registration(this);
        }
        
    }
}