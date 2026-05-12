using Project.Core.Player.Scripts;
using Project.Shared.Scripts.Extensions;
using UnityEngine;

namespace Project.Core.Surfaces.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SimpleSurfaceHandler : BaseSurfaceHandler
    {
        [SerializeField] private float stickVelocityThreshold = 20f;
        [SerializeField] private float stickDistanceThreshold = 1f;
        [SerializeField] private float stickForce = 10f;
        [SerializeField] private bool isJoint;
        
        private Vector2 _lastDeltaMove;
        private Vector2 _velocity;
        private Vector2 _lastPosition;
        private FixedJoint2D _currentJoint;

        public bool IsJoint
        {
            get => isJoint;
            set => isJoint = value;
        }
        
        private void FixedUpdate()
        {
            Vector2 currentPosition = rigidbody ? rigidbody.position : transform.position;
            _lastDeltaMove = currentPosition - _lastPosition;
            _velocity = _lastDeltaMove / Time.fixedDeltaTime;
            _lastPosition = currentPosition;
        }

        protected override bool Stick(StickHandler handler, Collider2D surface)
        {
            handler.IsCompensationGravity = true;
            return base.Stick(handler, surface);
        }
        
        protected override bool UpdateStick(StickHandler handler, Collider2D surface)
        {
            var rb = handler.Rigidbody2D;
            var position = rb.position;
            var point = surface.ClosestPoint(position);
            var dir = point - position;
            var distance = dir.magnitude;
            
            var isStick = isActiveAndEnabled && !(Vector2.Dot(rb.linearVelocity.normalized, dir) < 1f &&
                        (rb.linearVelocity.magnitude > stickVelocityThreshold || distance > stickDistanceThreshold));

            if (!isStick)
                return false;
            
            rb.AddForce(dir * stickForce * handler.StickMultiplier);
            
            if (isJoint)
            {
                Vector2 velocityDiff = (_velocity - rb.linearVelocity) * _velocity.normalized.Absolute();
                rb.linearVelocity += velocityDiff;
            }
            
            return true;
        }

        protected override void Unstick(StickHandler handler, Collider2D surface)
        {
            base.Unstick(handler, surface);
            handler.IsCompensationGravity = false;
        }
    }
}