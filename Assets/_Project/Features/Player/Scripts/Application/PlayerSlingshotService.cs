using System;
using _Project._Common.Scripts.Contracts.Interfaces;
using _Project._Common.Scripts.Infrastructure.ZeroMessenger;
using _Project._Common.Scripts.Signals;
using _Project.Features.Player.Scripts.Contracts;
using R3;
using VContainer.Unity;

namespace _Project.Features.Player.Scripts.Application
{

    public class PlayerSlingshotService : IPlayerSlingshotService, IInitializable, IDisposable
    {
        private readonly IPhysicBody2D _physicBody2D;
        private readonly IPlayerSlingshotModel _playerSlingshotModel;
        private readonly IZeroMessengerService _zeroMessengerService;
        private CompositeDisposable _disposables = new ();
        
        public event Action<IPlayerSlingshotConfig> OnPlayerSlingshotConfigChange;
        
        public PlayerSlingshotService(
            IPhysicBody2D physicBody2D,
            IPlayerSlingshotModel  playerSlingshotModel, 
            IZeroMessengerService zeroMessengerService)
        {
            _physicBody2D = physicBody2D;
            _playerSlingshotModel = playerSlingshotModel;
            _zeroMessengerService = zeroMessengerService;
        }
        
        public void Initialize()
        {
            _zeroMessengerService.Subscribe(_physicBody2D, 
                    (AddThrowForcePercentSignal s) => AddThrowForcePercent(s.percent))
                .AddTo(_disposables);
        }

        public IPlayerSlingshotConfig GetState() => _playerSlingshotModel.GetState();
        
        private void AddThrowForcePercent(int percent)
        {
            _playerSlingshotModel.AddThrowForcePercent(percent);
            OnPlayerSlingshotConfigChange?.Invoke(GetState());
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}