using System;
using Project.Core.Player.Scripts;
using R3;
using UnityEngine;

namespace Project.Core.Surfaces.Scripts
{
    
    public class RoughSurfaceHandler : MonoBehaviour
    {
        [SerializeField] private CompositeCollider2D colliderSurface;

        private CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private RestartableTimer _restartableTimer;
        private SlingshotHandler _slingshotHandler;
        private bool _isAttached;
        private CompositeDisposable _compositeDisposableTrigger = new CompositeDisposable();

        private void Awake()
        {
            _restartableTimer = new RestartableTimer(2f);
            _restartableTimer.OnCompleted.Subscribe(_ => _slingshotHandler.ToAwake()).AddTo(_compositeDisposable);
        }
        
        private void OnDestroy()
        {
            _compositeDisposable.Dispose();
            _compositeDisposableTrigger.Dispose();
            _restartableTimer.Dispose();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;
            
            Debug.Log("enter");
            if (!_slingshotHandler)
                _slingshotHandler = other.transform.root.GetComponent<SlingshotHandler>();
            
            _slingshotHandler.EventEnterTrigger.Subscribe(_ =>
            {
                if (_ == colliderSurface)
                    return;
                
                _restartableTimer.Stop();
                _slingshotHandler.ToUnAwake();
                _compositeDisposableTrigger.Clear();
            }).AddTo(_compositeDisposableTrigger);

            _restartableTimer.Start();
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;
            
            Debug.Log("exit");
            _compositeDisposableTrigger.Clear();
            _restartableTimer.Stop();
            _slingshotHandler.ToUnAwake();
        }

        private void Update()
        {
            _restartableTimer.Update();
        }
        
        private void FixedUpdate()
        {
            if (!_restartableTimer.IsCompleted)
                return;
            
            var position = _slingshotHandler.transform.position;
            var contact = colliderSurface.ClosestPoint(position);
            var dirConnect = (contact - (Vector2)position).normalized;
            
            Debug.DrawRay(position,  dirConnect, Color.magenta, 1f);
            
            _slingshotHandler.ApplyForce(dirConnect
                                         * -Physics2D.gravity.y
                                         * 1f);
        }
    }
}