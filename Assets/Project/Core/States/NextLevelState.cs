using Cysharp.Threading.Tasks;
using Project.Core.Services;
using Project.Shared.Scripts.StateMachine;
using UnityEngine;
using Zenject;

namespace Project.Core.States
{
    public class NextLevelState :  BaseState
    {
        [SerializeField] private float _delayBeforeNextLevel;
        [Inject] private MapSpawner _mapSpawner;
        
        protected override async void OnStateStart()
        {
            base.OnStateStart();
            await UniTask.WaitForSeconds(_delayBeforeNextLevel, cancellationToken: destroyCancellationToken);
            _mapSpawner.LoadNextLevel().Forget();
        }
    }
}