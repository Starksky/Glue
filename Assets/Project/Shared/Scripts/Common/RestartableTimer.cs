using R3;
using System;
using UnityEngine;

public class RestartableTimer : IDisposable
{
    private float _durationSeconds;
    private readonly Subject<Unit> _onTimerCompleted = new();
    private bool _isRunning;
    private bool _isCompleted;
    private float _timer;

    public RestartableTimer(float durationSeconds)
    {
        _durationSeconds = durationSeconds;
    }

    public void Update()
    {
        if (!_isRunning)
            return;
        
        if (_timer < _durationSeconds)
            _timer += Time.deltaTime;
        else
        {
            _isCompleted = true;
            _isRunning = false;
            _onTimerCompleted.OnNext(Unit.Default);
        }
    }
    
    public void Start(float time)
    {
        Stop();
        _durationSeconds = time;
        _isRunning = true;
    }
    public void Start() => Start(_durationSeconds);
    
    public void Stop()
    {
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
    }
}