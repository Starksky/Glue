using System;
using System.Collections.Generic;
using _Project._Common.Scripts.Contracts.Interfaces;
using _Project.Features.Walls.Common.Scripts.Presentation;
using _Project.Features.Walls.Stick.Scripts.Contracts;
using UnityEngine;

namespace _Project.Features.Walls.Stick.Scripts.Presentation
{
    public class StickSurfacePresenter : BaseSurfacePresenter, IDisposable
    {
        private readonly IStickSurfaceConfig _config;
        private Dictionary<IPhysicBody2D, StickSession> _sessions = new Dictionary<IPhysicBody2D, StickSession>();

        public StickSurfacePresenter(IStickSurfaceConfig config)
        {
            _config = config;
        }
        
        public override bool BeginContact(IPhysicBody2D body, Collider2D colliderSurface)
        {
            body.IsCompensationGravity = true;
            
            body.Snapshot();
            body.LinearDamping = _config.LinearDumping;
            body.AngularDamping = _config.AngularDumping;
            
            return base.BeginContact(body, colliderSurface);
        }

        public override bool StayContact(IPhysicBody2D body, Collider2D colliderSurface)
        {
            var session = GetSession<StickSession>(body);
            
            var position = body.Position;
            var point = colliderSurface.ClosestPoint(position);
            var dir = point - position;
            var distance = dir.magnitude;
            
            var isStick = !(Vector2.Dot(body.LinearVelocity.normalized, dir) < 1f &&
                                                  (body.LinearVelocity.magnitude > _config.VelocityThreshold || distance > _config.DistanceThreshold));

            if (!isStick)
                return false;
            
            var force = dir * _config.Force * session.Multiplier;
            body.AddForce(force);
            
            if (_config.IsJoint)
            {
                if (body.LinearVelocity.magnitude < Velocity.magnitude)
                    body.MoveTo(body.Position + Velocity * Time.fixedDeltaTime);
            }
            
            return true;
        }

        public override void EndContact(IPhysicBody2D body)
        {
            body.IsCompensationGravity = false;
            body.RestoreToSnapshot();
            base.EndContact(body);
        }
        
        protected T GetSession<T>(IPhysicBody2D body) where T : StickSession, new()
        {
            if (_sessions.TryGetValue(body, out var session))
                return (T)session;
            
            session =  new T();
            _sessions[body] = session;
            
            return (T)session;
        }

        // for optimization
        protected void DisposeSession<T>(IPhysicBody2D body) where T : StickSession, new()
        {
            if (!_sessions.TryGetValue(body, out var session))
                return;
            
            session.Dispose();
            _sessions.Remove(body);
        }
        
        public void Dispose()
        {
            foreach (var session in _sessions)
                session.Value.Dispose();
            
            _sessions.Clear();
        }
    }
}