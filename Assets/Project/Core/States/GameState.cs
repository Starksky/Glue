using Project.Core.Services;
using Project.Shared.Scripts.StateMachine;
using Zenject;

namespace Project.Core.States
{
    public class GameState : BaseState
    {
        [Inject] private GameService _gameService;
        
        protected override void OnStateStart()
        {
            base.OnStateStart();
            _gameService.StartTimer();
        }
        
        public void AddTryCount() => _gameService.AddTryCount();
    }
}