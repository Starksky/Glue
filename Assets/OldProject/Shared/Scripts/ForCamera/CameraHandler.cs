using Project.Core.Services;
using Project.Shared.Scripts.Extensions;
using R3;
using SaintsField;
using UnityEngine;
using Zenject;

namespace Project.Shared.Scripts.ForCamera
{
    [RequireComponent(typeof(Camera))]
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField, ReadOnly, GetComponent(typeof(Camera))] private Camera camera;
        [SerializeField] private float speed;
        [SerializeField] private bool isDebugBounds;

        [Inject] private GameService _gameService;

        private CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private Transform _target;
        private Grid _mapGrid;
        private Vector3? _freePosition;
        private Bounds _mapBounds;
        private Bounds _cameraBounds;

        public Camera Camera => camera;
        
        private void Awake()
        {
            _gameService.PlayerTransform.Subscribe(_ =>
            {
                _target = _;
                ToTargetImmediate();
            }).AddTo(_compositeDisposable);
            _gameService.CurrentMap.Subscribe(m =>
            {
                _mapGrid = m.Grid;
                _mapBounds = _mapGrid ? _mapGrid.GetBounds() : default;
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
        public void SetFreePosition(Vector3? position)
        {
            _freePosition = position;
        }

        public void OnDrawGizmos()
        {
            if (!isDebugBounds)
                return;
            
            _mapBounds = _mapBounds = _mapGrid ? _mapGrid.GetBounds() : default;
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