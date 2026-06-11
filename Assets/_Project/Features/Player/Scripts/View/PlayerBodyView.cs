using _Project.Scripts.Contracts.Interfaces;
using UnityEngine;
using VContainer;

namespace _Project.Features.Player.Scripts.View
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerBodyView : MonoBehaviour, IPlayerBodyView
    {
        private IPhysicBody2D _physicBody2D;
        private ISurfaceView _currentSurfaceView;
        private Collider2D _currentSurface;
 
        public Transform Transform => transform;
        public Vector2? ContactClosestPoint => _currentSurface?.ClosestPoint(_physicBody2D.Position);
        public bool HasContactWithSurface => _currentSurface;

        [Inject]
        public void Construct(IPhysicBody2D physicBody2D)
        {
            _physicBody2D = physicBody2D;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.isTrigger)
                return;
            
            if (!other.TryGetComponent<ISurfaceView>(out var surfaceView))
                return;

            _currentSurfaceView?.EndContact(_physicBody2D);
            
            if (!surfaceView.BeginContact(_physicBody2D))
            {
                _currentSurfaceView = null;
                _currentSurface = null;
                return;
            }
            
            _currentSurface = other;
            _currentSurfaceView = surfaceView;
        }
        
        private void FixedUpdate() 
        {
            if (_currentSurfaceView == null)
                return;

            if (!_currentSurfaceView.StayContact(_physicBody2D))
            {
                _currentSurfaceView = null;
                _currentSurface = null;
            }
        }
    }
}