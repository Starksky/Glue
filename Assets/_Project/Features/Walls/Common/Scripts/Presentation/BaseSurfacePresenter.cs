using _Project.Scripts.Contracts.Interfaces;
using UnityEngine;

namespace _Project.Features.Walls.Common.Scripts.Presentation
{
    public abstract class BaseSurfacePresenter : ISurfacePresenter
    {
        protected Vector2 Velocity { get; private set; }
        
        private Vector2 _lastPosition;
        
        public void FixedUpdateProcess(Vector2 position)
        {
            Velocity = (position - _lastPosition) / Time.fixedDeltaTime;
            _lastPosition = position;
        }

        public virtual bool BeginContact(IPhysicBody2D physicBody2D, Collider2D colliderSurface) => true;
        public virtual bool StayContact(IPhysicBody2D physicBody2D, Collider2D colliderSurface) => true;
        public virtual void EndContact(IPhysicBody2D physicBody2D) {}
    }
}