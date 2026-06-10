using System.Collections.Generic;
using Project.Core.Player.Scripts;
using R3;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Core.Surfaces.Scripts
{
    public enum ETypeEventSurface
    {
        stick,
        update,
        unstick
    }
    public class EventSurfaceHandler : SimpleSurfaceHandler
    {
        [SerializeField] private ETypeEventSurface typeEvent; 
        [SerializeField] private float delayApplyEvent;
        [SerializeField] private UnityEvent EventApply;

        private CompositeDisposable _compositeDisposable = new CompositeDisposable();
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
        
        protected override bool Stick(StickHandler handler, Collider2D surface)
        {
            var timer = GetTimer(handler);
            if (typeEvent == ETypeEventSurface.stick)
                timer.OnCompleted.Subscribe(_ => OnApply()).AddTo(_compositeDisposable);
            if (typeEvent != ETypeEventSurface.unstick)
                timer.Start(delayApplyEvent);
            
            return base.Stick(handler, surface);
        }
        
        protected override bool UpdateStick(StickHandler handler, Collider2D surface)
        {
            if (typeEvent == ETypeEventSurface.update)
                if (GetTimer(handler) is { IsCompleted: true } timer)
                {
                    timer.Stop();
                    EventApply.Invoke();
                }
            
            return base.UpdateStick(handler, surface);
        }

        protected override void Unstick(StickHandler handler, Collider2D surface)
        {
            var timer = GetTimer(handler);
            if (typeEvent == ETypeEventSurface.unstick)
            {
                timer.OnCompleted.Subscribe(_ => OnApply()).AddTo(_compositeDisposable);
                timer.Start(delayApplyEvent);
            }
            else timer.Stop();
            base.Unstick(handler, surface);
        }

        private void OnApply()
        {
            _compositeDisposable.Clear();
            EventApply.Invoke();
        }
    }
}