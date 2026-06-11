using _Project._Common.Scripts.Infrastructure.PoolObject;
using _Project.Scripts.Utils;
using R3;
using UnityEngine;
using VContainer;

namespace _Project._Common.Scripts.Infrastructure.MonoStateMachine.States
{
    public class RespawnState : BaseState
    {
        [SerializeField] private float timer;

        private MonoPoolable _monoPoolable;
        private RestartableTimer _restartableTimer = new RestartableTimer();

        [Inject]
        public void Construct(MonoPoolable monoPoolable)
        {
            _monoPoolable = monoPoolable;
        }
        
        private void Awake()
        {
            _restartableTimer.OnCompleted.Subscribe(_ => _monoPoolable.Respawn())
                .RegisterTo(destroyCancellationToken);
        }

        protected override void OnStateStart()
        {
            _restartableTimer.Start(timer);
            _monoPoolable.Free();
        }

        protected override void OnStateExit()
        {
            _restartableTimer.Stop();
        }

        private void OnDestroy()
        {
            _restartableTimer.Dispose();
        }
    }
}