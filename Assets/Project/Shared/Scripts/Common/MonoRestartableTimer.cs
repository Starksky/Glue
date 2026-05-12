using System;
using R3;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Shared.Scripts.Common
{
    public class MonoRestartableTimer : MonoBehaviour
    {
        [SerializeField] private float duration;
        [SerializeField] private bool startOnEnable;
        [SerializeField] private UnityEvent EventSuccess;
        
        private RestartableTimer _restartableTimer = new RestartableTimer();

        private void Awake()
        {
            _restartableTimer.OnCompleted.Subscribe(_ => EventSuccess.Invoke()).RegisterTo(destroyCancellationToken);
        }
        
        private void OnEnable()
        {
            if (startOnEnable)
                _restartableTimer.Start(duration);
        }

        private void OnDestroy()
        {
            _restartableTimer.Dispose();
        }
        
        public void Restart() => _restartableTimer.Start(duration);
        public void Stop() => _restartableTimer.Stop();
    }
}