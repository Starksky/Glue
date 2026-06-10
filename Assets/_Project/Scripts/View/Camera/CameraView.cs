using _Project.Scripts.Contracts.Interfaces;
using _Project.Scripts.Utils;
using R3;
using SaintsField;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.View.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraView : MonoBehaviour, ICameraView
    {
        [SerializeField, ReadOnly, GetComponent(typeof(UnityEngine.Camera))] 
        private UnityEngine.Camera camera;
        [SerializeField] private float speed;
        [SerializeField] private bool isDebugBounds;
        
        private ISessionService<IPlayerBodyView> _playerSessionsService;
        private ISessionService<IMapView> _mapSessionService;
        
        private CompositeDisposable _compositeDisposable = new CompositeDisposable();
        
        private Transform _target;
        private Vector3? _freePosition;
        private Bounds _mapBounds;
        private Bounds _cameraBounds;

        [Inject]
        public void Construct(ISessionService<IPlayerBodyView> playerSessionService, 
            ISessionService<IMapView> mapSessionService)
        {
            _playerSessionsService = playerSessionService;
            _mapSessionService =  mapSessionService;
        }
        
        private void Awake()
        {
            _playerSessionsService.Session.Subscribe(session =>
            {
                _target = session.Transform;
                ToTargetImmediate();
            }).AddTo(_compositeDisposable);
            
            _mapSessionService.Session.Subscribe(session =>
            {
                _mapBounds = session?.Bounds ?? default;
                var position = _mapBounds.center;
                position.z = transform.position.z;
                _mapBounds.center = position;
            }).AddTo(_compositeDisposable);

            _cameraBounds = camera.GetBounds();
        }
        private void OnDestroy()
        {
            _compositeDisposable.Dispose();
        }
        
        public void ToTarget() => _freePosition = null;
        public void ToTargetImmediate() 
        {
            _freePosition = null;
            
            var tr = transform;
            var position = tr.position;
            var targetPosition = _target.position;
            targetPosition.z = position.z;
            
            _cameraBounds = camera.GetBounds();
            _cameraBounds.center = targetPosition;

            tr.position = ClosestBounds();
        }
        
        public void SetFreeScreenPosition(Vector3? screenPosition)
        {
            if (screenPosition != null)
                _freePosition = camera.ScreenToWorldPoint(screenPosition.Value);
            else
                _freePosition = null;
        }
        
        public Vector2 ScreenToWorldPoint(Vector2 screenPos)
            => camera.ScreenToWorldPoint(screenPos);

        public void OnDrawGizmos()
        {
            if (!isDebugBounds)
                return;
            
            _cameraBounds = camera.GetBounds();
            _cameraBounds.center = transform.position;
            _mapBounds.DebugDrawBounds(Color.green);
            _cameraBounds.DebugDrawBounds(Color.blue);
        }

        private void FixedUpdate()
        {
            if (!_target && _freePosition == null)
                return;
            
            var position = transform.position;
            var targetPosition = _freePosition ?? _target.position;
            targetPosition.z = position.z;
            
            if ((position - targetPosition).magnitude < 0.1f)
                return;
            
            _cameraBounds = camera.GetBounds();
            position = Vector3.Lerp(position, targetPosition, Time.deltaTime * speed);
            _cameraBounds.center = position;
            
            transform.position = ClosestBounds();
        }

        private Vector3 ClosestBounds()
        {
            var position = _mapBounds.ClosestBoundsInBounds(_cameraBounds);
            
            if (_mapBounds.size.x < _cameraBounds.size.x)
                position.x = _mapBounds.center.x;
            if (_mapBounds.size.y < _cameraBounds.size.y)
                position.y = _mapBounds.center.y;

            return position;
        }
    }
}