using System;
using Project.Core.Surfaces.Scripts;
using Project.Shared.Scripts.Extensions;
using R3;
using SaintsField;
using UnityEngine;

namespace Project.Core.Player.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class StickHandler : MonoBehaviour
    {
        [SerializeField, ReadOnly, GetComponent(typeof(Rigidbody2D))]
        private Rigidbody2D rigidbody2D;

        private CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private ReactiveProperty<bool> _isStick = new ReactiveProperty<bool>();
        private Collider2D _currentSurface;
        private BaseSurfaceHandler _currentSurfaceHandler;
        private Rigidbody2DParams _defaultRigidbody2DParams;

        public Rigidbody2D Rigidbody2D => rigidbody2D;
        public Rigidbody2DParams DefaultRigidbody2DParams => _defaultRigidbody2DParams;

        public float StickScale { get; set; } = 1f;
        public float UnstickScale { get; set; } = 1f;
        
        private void Awake()
        {
            _defaultRigidbody2DParams.Set(rigidbody2D);
            _isStick.Subscribe(OnStick).AddTo(_compositeDisposable);
        }

        private void OnDestroy()
        {
            _compositeDisposable.Dispose();
            _isStick.Dispose();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.isTrigger)
                return;

            if (other.TryGetComponent<BaseSurfaceHandler>(out var surfaceHandler))
            {
                if (_isStick.Value && _currentSurfaceHandler)
                    _currentSurfaceHandler.OnUnstick(this, _currentSurface);
                
                _isStick.Value = false;
                _currentSurface = other;
                _currentSurfaceHandler = surfaceHandler;
                
                if (_currentSurfaceHandler)
                    _isStick.Value = _currentSurfaceHandler.OnStick(this, _currentSurface);
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.isTrigger)
                return;
            
            if (_currentSurface == other)
                if (!_isStick.Value && _currentSurfaceHandler)
                    _isStick.Value = _currentSurfaceHandler.OnStick(this, _currentSurface);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.isTrigger)
                return;
            
            if (_currentSurface == other)
                if (_isStick.Value && _currentSurfaceHandler)
                {
                    _currentSurfaceHandler.OnUnstick(this, _currentSurface);
                    _isStick.Value = false;
                }
        }

        private void FixedUpdate()
        {
            if (_isStick.Value && _currentSurfaceHandler)
                _isStick.Value = _currentSurfaceHandler.OnUpdateStick(this, _currentSurface);
        }

        private void OnStick(bool stick)
        {
            if (stick && _currentSurfaceHandler)
                _currentSurfaceHandler.StickParams.Apply(rigidbody2D);
            else rigidbody2D.ApplyParams(_defaultRigidbody2DParams);
        }
    }
}