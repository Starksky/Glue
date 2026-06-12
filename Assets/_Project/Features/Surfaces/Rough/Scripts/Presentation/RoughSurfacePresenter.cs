using _Project._Common.Scripts.Contracts.Interfaces;
using _Project.Features.Surfaces.Common.Scripts.Presentation;
using _Project.Features.Surfaces.Rough.Scripts.Contracts;
using R3;
using UnityEngine;

namespace _Project.Features.Surfaces.Rough.Scripts.Presentation
{
    public class RoughSurfacePresenter : StickableSurfacePresenter
    {
        private readonly IRoughSurfaceConfig _config;
        private CompositeDisposable _disposables = new CompositeDisposable();
        
        public RoughSurfacePresenter(IRoughSurfaceConfig config) : base(config)
        {
            _config = config;
        }

        private void OnCompletedTimer(IPhysicBody2D body, StickableSession session)
        {
            body.IsCompensationGravity = false;
            session.Multiplier = _config.ScaleShiftStick;
        }
        
        public override bool BeginContact(IPhysicBody2D body, Collider2D colliderSurface)
        {
            var session = GetSession(body);
            session.Begin();
            session.Multiplier = 1f;
            session.Timer.Stop();
            session.Timer.OnCompleted.Subscribe(_ => OnCompletedTimer(body, session))
                .RegisterTo(session.DisableCancellationToken);
            session.Timer.Start(_config.DelayByShift);
            return base.BeginContact(body, colliderSurface);
        }
        
        public override void EndContact(IPhysicBody2D body)
        {
            var session = GetSession(body);
            session.End();
            base.EndContact(body);
        }

        public override void Dispose()
        {
            base.Dispose();
            _disposables.Dispose();
        }
    }
}