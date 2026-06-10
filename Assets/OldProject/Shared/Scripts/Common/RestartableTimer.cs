using R3;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

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

        bool result = false;
        while(!result)
        {
            if (_cancellationTokenSource?.IsCancellationRequested ?? true)
                break;
            
            result = Update();
            await UniTask.Yield();
        }
        
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
        _isRunning = false;
    }
    
    private bool Update()
    {
        if (_timer < _durationSeconds)
            _timer += Time.deltaTime;
        else
        {
            _isCompleted = true;
            _isRunning = false;
            _onTimerCompleted.OnNext(Unit.Default);
        }

        return _isCompleted;
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