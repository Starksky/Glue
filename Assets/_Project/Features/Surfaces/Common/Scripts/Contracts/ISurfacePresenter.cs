using _Project._Common.Scripts.Contracts.Interfaces;
using UnityEngine;

namespace _Project.Features.Surfaces.Common.Scripts.Contracts
{
    public interface ISurfacePresenter
    {
        public void FixedUpdateProcess(Vector2 position);
        public bool BeginContact(IPhysicBody2D physicBody2D, Collider2D colliderSurface);
        public bool StayContact(IPhysicBody2D physicBody2D, Collider2D colliderSurface);
        public void EndContact(IPhysicBody2D physicBody2D);
    }
}