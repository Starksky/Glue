using _Project._Common.Scripts.Contracts.Interfaces;
using _Project.Features.Surfaces.Common.Scripts.Contracts;
using UnityEngine;

namespace _Project.Features.Surfaces.Common.Scripts.Presentation
{
    public abstract class BaseSurfacePresenter : ISurfacePresenter
    {
        private readonly IBaseSurfaceConfig _config;
        protected Vector2 Velocity { get; private set; }
        
        private Vector2 _lastPosition;

        public BaseSurfacePresenter(IBaseSurfaceConfig config)
        {
            _config = config;
        }
        
        public void FixedUpdateProcess(Vector2 position)
        {
            Velocity = (position - _lastPosition) / Time.fixedDeltaTime;
            _lastPosition = position;
        }

        public virtual bool BeginContact(IPhysicBody2D physicBody2D, Collider2D colliderSurface)
        {
            physicBody2D.Snapshot();
            physicBody2D.LinearDamping = _config.LinearDumping;
            physicBody2D.AngularDamping = _config.AngularDumping;
            return true;
        }
        public virtual bool StayContact(IPhysicBody2D physicBody2D, Collider2D colliderSurface) => true;
        public virtual void EndContact(IPhysicBody2D physicBody2D)
        {
            physicBody2D.RestoreToSnapshot();
        }
    }
}