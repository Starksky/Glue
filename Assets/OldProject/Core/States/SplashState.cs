using Project.Core.Services;
using Project.Shared.Scripts.StateMachine;
using Zenject;

namespace Project.Core.States
{
    public class DieState : BaseState
    {
        [Inject] private GameService _gameService;
        
        protected override void OnStateStart()
        {
            base.OnStateStart();
            _gameService.StopTimer();
        }
    }
}