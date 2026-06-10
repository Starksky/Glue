using Project.Core.Player.Scripts;
using UnityEngine;

namespace Project.Core.Surfaces.Scripts
{
    public class SimpleBounceSurfaceHandler : BaseSurfaceHandler
    {
        protected override bool Stick(StickHandler handler, Collider2D surface)
        {
            var rb = handler.Rigidbody2D;
            var velocity = rb.linearVelocity;
            var contactPoint = surface.ClosestPoint(rb.position);
            var normal = (contactPoint - rb.position).normalized;
            rb.linearVelocity = Vector2.Reflect(velocity, normal);
            
            return base.Stick(handler, surface);
        }
    }
}