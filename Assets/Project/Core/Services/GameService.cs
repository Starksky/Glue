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

        
        private ReactiveProperty<Grid> _currentMap = new ReactiveProperty<Grid>();
        public ReadOnlyReactiveProperty<Grid> CurrentMap => _currentMap;

        
        private ReactiveProperty<int> _bestTryCount = new ReactiveProperty<int>();
        public ReadOnlyReactiveProperty<int> BestTryCount => _bestTryCount;
        
        
        private ReactiveProperty<int> _bestTime = new ReactiveProperty<int>();
        public ReadOnlyReactiveProperty<int> BestTime => _bestTime;
        
        
        private ReactiveProperty<int> _currentTime = new ReactiveProperty<int>();
        public ReadOnlyReactiveProperty<int> CurrentTime => _currentTime;
        
        
        private ReactiveProperty<int> _currentTryCount = new ReactiveProperty<int>();
        public ReadOnlyReactiveProperty<int> CurrentTryCount => _currentTryCount;

        private ReactiveProperty<int> _currentStars = new ReactiveProperty<int>();
        public ReadOnlyReactiveProperty<int> CurrentStars => _currentStars;
        
        public bool IsStartedTimer => _cancellationTimer?.IsCancellationRequested == false;
        
        private CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private CancellationTokenSource _cancellationTimer;
        
        public void Initialize()
        {
            
        }
        
        public void Dispose()
        {
            _currentTryCount.Dispose();
            _currentTime.Dispose();
            
            _bestTime.Dispose();
            _bestTryCount.Dispose();
            
            _playerTransform.Dispose();
            _currentMap.Dispose();
            
            _compositeDisposable.Dispose();
        }

        public void SetPlayer(Transform t) => _playerTransform.Value = t;
        public void SetMap(Grid g) => _currentMap.Value = g;
        public void SetBestTryCount(int c) => _bestTryCount.Value = c;
        public void SetBestTime(int t) => _bestTime.Value = t;
        
        public void AddTryCount() => _currentTryCount.Value++;

        public void StartTimer() => StartTimerAsync().Forget();
        public void StopTimer() => _cancellationTimer?.Cancel();
        public void CalcStars()
        {
            StopTimer();

            var maxCountStars = 3;
            float bestCount = _bestTryCount.Value * maxCountStars;
            float bestTime = _bestTime.Value * maxCountStars;
            int placeCount = Mathf.FloorToInt(bestCount / _currentTryCount.Value);
            int placeTime = Mathf.FloorToInt(bestTime / _currentTime.Value);
            int place = placeCount <= placeTime ? placeCount : placeTime;
            
            _currentStars.Value = Mathf.Clamp(place, 1, maxCountStars);
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