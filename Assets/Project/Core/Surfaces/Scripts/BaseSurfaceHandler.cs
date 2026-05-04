using Project.Core.Player.Scripts;
using UnityEngine;

namespace Project.Core.Surfaces.Scripts
{
    public abstract class BaseSurfaceHandler : MonoBehaviour
    {
        [SerializeField] private bool showDebugHitSurface;
        [SerializeField] private float stickThreshold = 20f;
        [SerializeField] private float unstickThreshold = 1f;
        [SerializeField] private Rigidbody2DParams stickParams;
        
        public Rigidbody2DParams StickParams => stickParams;

        private bool CanStick(Rigidbody2D body) => body.linearVelocity.magnitude <= stickThreshold;
        
        public bool OnStick(StickHandler handler, Collider2D surface)
        {
            if (!CanStick(handler.Rigidbody2D))
                return false;
            
            Stick(handler, surface);
            
            return true;
        }
        
        public bool OnUpdateStick(StickHandler handler, Collider2D surface) 
        {
            var position = handler.Rigidbody2D.position;
            var stickPoint = surface.ClosestPoint(position);
            Vector2 dir = stickPoint - position;

            if (showDebugHitSurface)
                Debug.DrawRay(position,  dir, Color.magenta, 1f);
            
            var isUnstick = dir.magnitude > unstickThreshold;
            if (isUnstick)
                Unstick(handler, surface);
            else UpdateStick(handler, surface);
            
            return !isUnstick;
        }
        
        public void OnUnstick(StickHandler handler, Collider2D surface)
        {
            Unstick(handler, surface);
        }
        
        
        protected virtual void Stick(StickHandler handler, Collider2D surface){}
        protected virtual void UpdateStick(StickHandler handler, Collider2D surface){}
        protected virtual void Unstick(StickHandler handler, Collider2D surface){}
    }
}