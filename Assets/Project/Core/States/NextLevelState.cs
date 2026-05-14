using Cysharp.Threading.Tasks;
using Project.Core.Services;
using Project.Shared.Scripts.StateMachine;
using UnityEngine;
using UnityEngine.Events;
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
            if (_mapSpawner.HasMaps)
                _mapSpawner.LoadNextLevel().Forget();
        }
    }
}