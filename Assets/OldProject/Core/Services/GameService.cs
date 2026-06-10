using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using Zenject;

namespace Project.Core.Services
{
    public class GameService : IInitializable, IDisposable
    {
        private ReactiveProperty<Transform> _playerTransform = new ReactiveProperty<Transform>();
        public ReadOnlyReactiveProperty<Transform> PlayerTransform => _playerTransform;
        
        private ReactiveProperty<MapHandler> _currentMap = new ReactiveProperty<MapHandler>();
        public ReadOnlyReactiveProperty<MapHandler> CurrentMap => _currentMap;
        
        
        private ReactiveProperty<int> _currentTime = new ReactiveProperty<int>();
        public ReadOnlyReactiveProperty<int> CurrentTime => _currentTime;
        
        private ReactiveProperty<int> _currentTryCount = new ReactiveProperty<int>();
        public ReadOnlyReactiveProperty<int> CurrentTryCount => _currentTryCount;

        private ReactiveProperty<int> _currentStars = new ReactiveProperty<int>();
        public ReadOnlyReactiveProperty<int> CurrentStars => _currentStars;
        
        
        public bool IsStartedTimer => _cancellationTimer?.IsCancellationRequested == false;

        private MapHandler _currentMapHandler;
        private CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private CancellationTokenSource _cancellationTimer;
        
        public void Initialize()
        {
            
        }
        
        public void Dispose()
        {
            _currentTryCount.Dispose();
            _currentTime.Dispose();
            
            _playerTransform.Dispose();
            _currentMap.Dispose();
            
            _compositeDisposable.Dispose();
        }

        public void SetPlayer(Transform t) => _playerTransform.Value = t;
        public void SetMap(MapHandler g) => _currentMap.Value = g;
        
        public void AddTryCount() => _currentTryCount.Value++;

        public void StartTimer() => StartTimerAsync().Forget();
        public void StopTimer() => _cancellationTimer?.Cancel();
        public void Finish()
        {
            StopTimer();

            var mapHandler = _currentMap.Value;
            var maxCountStars = 3;
            int placeCount = mapHandler.CountTryForFirstPlace >= _currentTryCount.Value ? maxCountStars :
                mapHandler.CountTryForSecondPlace >= _currentTryCount.Value ? maxCountStars - 1 : 1;
            bool inRangeTimer = mapHandler.Timer >= _currentTime.Value;
            int place = inRangeTimer ? placeCount : 1;
            
            _currentStars.Value = place;
        }
        
        private async UniTask StartTimerAsync()
        {
            _cancellationTimer?.Cancel();
            _cancellationTimer = new CancellationTokenSource();
            
            float timer = 0;
            while (true)
            {
                if (_cancellationTimer.IsCancellationRequested)
                    break;
                
                timer += Time.deltaTime;
                _currentTime.Value = (int)timer;
                await UniTask.Yield();
            }
            
            _cancellationTimer.Dispose();
            _cancellationTimer = null;
        }
        
        public void ClearTry()
        {
            _currentTryCount.Value = 0;
            _currentTime.Value = 0;
            _currentStars.Value = 0;
        }
    }
}