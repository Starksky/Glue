using System.Collections.Generic;
using Project.Core.Player.Scripts;
using UnityEngine;

namespace Project.Core.Surfaces.Scripts
{
    public class RoughSurfaceHandler : SimpleSurfaceHandler
    {
        [SerializeField] private float delayApplyRough;
        [SerializeField] private float roughScaleUnstick;
        
        private Dictionary<StickHandler, RestartableTimer> _timers = new Dictionary<StickHandler, RestartableTimer>();

        private void OnDestroy()
        {
            foreach (var t in _timers)
                t.Value.Dispose();
            _timers.Clear();
            _timers = null;
        }

        private RestartableTimer GetTimer(StickHandler body)
        {
            if (!_timers.TryGetValue(body, out var timer))
            {
                timer = new RestartableTimer();
                _timers[body] = timer;
            }
            return timer;
        }
        
        protected override void Stick(StickHandler handler, Collider2D surface)
        {
            handler.StickScale = 1f;
            
            base.Stick(handler, surface);
            
            GetTimer(handler)
                .Start(delayApplyRough);
        }

        protected override void UpdateStick(StickHandler handler, Collider2D surface)
        {
            if (GetTimer(handler).IsCompleted)
            {
                handler.Rigidbody2D.gravityScale = handler.DefaultRigidbody2DParams.gravityScale;
                handler.StickScale = roughScaleUnstick;
            }
            base.UpdateStick(handler, surface);
        }

        protected override void Unstick(StickHandler handler, Collider2D surface)
        {
            handler.StickScale = 1f;
            GetTimer(handler).Stop();
            base.Unstick(handler, surface);
        }
    }
}