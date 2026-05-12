using Project.Core.Player.Scripts;
using UnityEngine;

namespace Project.Core.Surfaces.Scripts
{
    public class SpringSurfaceHandler : SimpleSurfaceHandler
    {
        [SerializeField] private float springForceMultiplier;

        protected override bool Stick(StickHandler handler, Collider2D surface)
        {
            handler.ThrowForceMultiplier = springForceMultiplier;
            return base.Stick(handler, surface);
        }
        protected override void Unstick(StickHandler handler, Collider2D surface)
        {
            base.Unstick(handler, surface);
            handler.ThrowForceMultiplier = 1f;
        }
    }
}