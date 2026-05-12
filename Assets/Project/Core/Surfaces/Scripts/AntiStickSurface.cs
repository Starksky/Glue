using Project.Core.Player.Scripts;
using UnityEngine;

namespace Project.Core.Surfaces.Scripts
{
    public class AntiStickSurface : BaseSurfaceHandler
    {
        [SerializeField] private float forceMultiplier = 1.5f;
        protected override bool Stick(StickHandler handler, Collider2D surface)
        {
            handler.IsCompensationGravity = false;
            var linearVelocity = -handler.Rigidbody2D.linearVelocity;
            handler.Rigidbody2D.linearVelocity = linearVelocity * forceMultiplier;
            return base.Stick(handler, surface);
        }
    }
}