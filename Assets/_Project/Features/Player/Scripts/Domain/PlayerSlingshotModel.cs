using _Project.Features.Player.Scripts.Contracts;
using _Project.Features.Player.Scripts.Contracts.DTOs;

namespace _Project.Features.Player.Scripts.Domain
{

    public class PlayerSlingshotModel : IPlayerSlingshotModel
    {
        private float _throwForcePercent = 1f;
        private IPlayerSlingshotConfig _config;
        
        public float ThrowForce { get; private set; }

        public PlayerSlingshotModel(IPlayerSlingshotConfig config)
        {
            _config = config;
            ThrowForce = config.ThrowForce;
        }

        public void AddThrowForcePercent(int percent)
        {
            _throwForcePercent += percent / 100f;
            _throwForcePercent = _throwForcePercent < 0f ? 1f : _throwForcePercent;
            ThrowForce = _config.ThrowForce * _throwForcePercent;
            ThrowForce = ThrowForce <= 0 ? _config.ThrowForce : ThrowForce;
        }

        public IPlayerSlingshotConfig GetState()
        {
            var state = new PlayerSlingshotState(
                _config.MaxDragDistanceBody,
                _config.MaxDragDistance,
                ThrowForce,
                _config.AngleForForce,
                _config.DragForce);
            
            return state;
        }
    }
}