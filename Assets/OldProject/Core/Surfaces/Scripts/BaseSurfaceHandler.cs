using Project.Core.Player.Scripts;
using SaintsField;
using UnityEngine;

namespace Project.Core.Surfaces.Scripts
{
    public abstract class BaseSurfaceHandler : MonoBehaviour
    {
        [SerializeField, ReadOnly, GetComponent(typeof(Rigidbody2D))]
        protected Rigidbody2D rigidbody;
        
        [SerializeField] private bool showDebugHitSurface;
        [SerializeField] private Rigidbody2DParams stickParams;
        
        public Rigidbody2DParams StickParams => stickParams;

        public bool OnStick(StickHandler handler, Collider2D surface)
            => Stick(handler, surface);
        
        public bool OnUpdateStick(StickHandler handler, Collider2D surface) 
        {
            if (showDebugHitSurface)
            {
                var position = handler.Rigidbody2D.position;
                var stickPoint = surface.ClosestPoint(position);
                Vector2 dir = stickPoint - position;
                Debug.DrawRay(position, dir, Color.magenta, 1f);
            }

            bool isStick = UpdateStick(handler, surface);
            
            if (!isStick)
                Unstick(handler, surface);

            return isStick;
        }
        
        public void OnUnstick(StickHandler handler, Collider2D surface)
        {
            Unstick(handler, surface);
        }

        protected virtual bool Stick(StickHandler handler, Collider2D surface) => true;
        protected virtual bool UpdateStick(StickHandler handler, Collider2D surface) => true;
        protected virtual void Unstick(StickHandler handler, Collider2D surface){}
    }
}