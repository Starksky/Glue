using Project.Core.Player.Scripts;
using UnityEngine;

namespace Project.Core.Surfaces.Scripts
{
    public class SimpleSurfaceHandler : BaseSurfaceHandler
    {
        [SerializeField] private float stickForce = 10f;
        
        protected override void UpdateStick(StickHandler handler, Collider2D surface)
        {
            var position = handler.Rigidbody2D.position;
            var stickPoint = surface.ClosestPoint(position);
            Vector2 dir = stickPoint - position;
            
            handler.Rigidbody2D.AddForce(dir.normalized * stickForce * handler.StickScale, ForceMode2D.Force);
        }
    }
}