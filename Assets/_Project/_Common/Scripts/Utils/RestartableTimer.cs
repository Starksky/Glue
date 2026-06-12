using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace _Project._Common.Scripts.Utils
{
    public class RestartableTimer : IDisposable
    {
        private float _durationSeconds;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly Subject<Unit> _onTimerCompleted = new();
        private bool _isRunning;
        private bool _isCompleted;
        private float _timer;

        private async UniTask OnRunning()
        {
            _isRunning = true;
            _cancellationTokenSource = new CancellationTokenSource();

            while(!_isCompleted)
            {
                if (_cancellationTokenSource?.IsCancellationRequested ?? true)
                    break;
            
                _isCompleted = Update();
                await UniTask.Yield();
            }
            
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            _isRunning = false;
            
            if (_isCompleted)
                _onTimerCompleted.OnNext(Unit.Default);
        }
    
        private bool Update()
        {
            bool result = _timer >= _durationSeconds;
            
            if (!result)
                _timer += Time.deltaTime;

            return result;
        }
    
        public void Start(float time)
        {
            Stop();
            _durationSeconds = time;
            OnRunning().Forget();
        }

        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
            _isRunning = false;
            _isCompleted = false;
            _timer = 0f;
        }
    
        public bool IsCompleted => _isCompleted;
        public bool IsRunning => _isRunning;
    
        public Observable<Unit> OnCompleted => _onTimerCompleted;

        public void Dispose()
        {
            Stop();
            _onTimerCompleted?.Dispose();
            _cancellationTokenSource?.Dispose();
        }
    }
}