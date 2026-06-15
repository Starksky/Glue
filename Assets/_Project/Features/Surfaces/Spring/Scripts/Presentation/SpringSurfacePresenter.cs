using _Project._Common.Scripts.Contracts.Interfaces;
using _Project._Common.Scripts.Infrastructure.ZeroMessenger;
using _Project._Common.Scripts.Signals;
using _Project.Features.Surfaces.Common.Scripts.Presentation;
using _Project.Features.Surfaces.Spring.Scripts.Contracts;
using UnityEngine;

namespace _Project.Features.Surfaces.Spring.Scripts.Presentation
{
    public class SpringSurfacePresenter : StickableSurfacePresenter
    {
        private readonly ISpringSurfaceConfig _config;
        private readonly IZeroMessengerService _zeroMessengerService;
        
        public SpringSurfacePresenter(ISpringSurfaceConfig config, 
            IZeroMessengerService zeroMessengerService) : base(config)
        {
            _config = config;
            _zeroMessengerService = zeroMessengerService;
        }
        public override bool BeginContact(IPhysicBody2D body, Collider2D colliderSurface)
        {
            _zeroMessengerService.Publish(body, new AddThrowForcePercentSignal
            {
                percent = _config.AddThrowForcePercent
            });
            return base.BeginContact(body, colliderSurface);
        }
        public override void EndContact(IPhysicBody2D body)
        {
            _zeroMessengerService.Publish(body, new AddThrowForcePercentSignal
            {
                percent = -_config.AddThrowForcePercent
            });
            base.EndContact(body);
        }
    }
}