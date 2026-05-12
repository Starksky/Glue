using Project.Core.Player.Scripts;
using UnityEngine;

namespace Project.Core.Surfaces.Scripts
{
    public class BounceSurfaceHandler : BaseSurfaceHandler
    {
        protected override bool Stick(StickHandler handler, Collider2D surface)
        {
            handler.IsCompensationGravity = true;
            
            var rb = handler.Rigidbody2D;
            var velocity = handler.LastThrowVelocity;
            var contactPoint = surface.ClosestPoint(rb.position);
            var normal = (contactPoint - rb.position).normalized;
            rb.linearVelocity = Vector2.Reflect(velocity, normal);
            handler.LastThrowVelocity = rb.linearVelocity;
            
            return base.Stick(handler, surface);
        }
    }
}