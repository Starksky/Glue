using _Project.Scripts.Contracts.Interfaces;
using SaintsField;
using UnityEngine;
using VContainer;

namespace _Project.Features.Walls.Common.Scripts.View
{
    public class SurfaceView : MonoBehaviour, ISurfaceView
    {
        [SerializeField, ReadOnly, GetComponent(typeof(Rigidbody2D))] private Rigidbody2D rb2D;
        [SerializeField, ReadOnly] private Collider2D colliderSurface;
        [SerializeField] private bool showDebugHitSurface;
        
        private ISurfacePresenter _surfacePresenter;

        [Inject]
        public void Construct(ISurfacePresenter surfacePresenter)
        {
            _surfacePresenter = surfacePresenter;
        }
        
        private void OnValidate()
        {
            if (colliderSurface == null)
                colliderSurface = GetComponent<CompositeCollider2D>();
            if (colliderSurface == null)
                colliderSurface = GetComponent<Collider2D>();
        }
        
        private void DrawDebugHitSurface(IPhysicBody2D physicBody2D)
        {
            var position = physicBody2D.Position;
            var stickPoint = colliderSurface.ClosestPoint(position);
            Vector2 dir = stickPoint - position;
            Debug.DrawRay(position, dir, Color.magenta, 1f);
        }

        private void FixedUpdate()
        {
            _surfacePresenter?.FixedUpdateProcess(rb2D.position);
        }

        public bool BeginContact(IPhysicBody2D physicBody2D)
            => _surfacePresenter.BeginContact(physicBody2D, colliderSurface);

        public bool StayContact(IPhysicBody2D physicBody2D)
        {
            if (showDebugHitSurface)
                DrawDebugHitSurface(physicBody2D);
            
            bool isStay = _surfacePresenter.StayContact(physicBody2D, colliderSurface);
            
            if (!isStay)
                _surfacePresenter.EndContact(physicBody2D);

            return isStay;
        }
        
        public void EndContact(IPhysicBody2D physicBody2D)
            => _surfacePresenter.EndContact(physicBody2D);
    }
}