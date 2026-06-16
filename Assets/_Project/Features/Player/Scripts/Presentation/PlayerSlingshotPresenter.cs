using System;
using _Project._Common.Scripts.Contracts.Interfaces;
using _Project._Common.Scripts.Infrastructure.ZeroMessenger;
using _Project._Common.Scripts.Signals;
using _Project.Features.Player.Scripts.Contracts;
using R3;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Features.Player.Scripts.Presentation
{
    public class PlayerSlingshotPresenter : IPlayerSlingshotPresenter, IInitializable, IFixedTickable, IDisposable
    {
        private readonly IPhysicBody2D _physicBody2D;
        private readonly IPlayerSlingshotService _playerSlingshotService;

        private IPlayerSlingshotConfig _config;
        private bool _isDragging;
        private Vector2 _currentDeltaDragBodyPosition;
        private Vector2 _currentDeltaDragPosition;
        private Vector2 _currentContactDirection;
        private float _currentStrength;
        
        
        [Inject]
        public PlayerSlingshotPresenter(IPhysicBody2D physicBody2D, IPlayerSlingshotService playerSlingshotService)
        {
            _physicBody2D = physicBody2D;
            _playerSlingshotService = playerSlingshotService;
        }

        public void Initialize()
        {
            _config = _playerSlingshotService.GetState();
            _playerSlingshotService.OnPlayerSlingshotConfigChange += OnChangeState;
        }
        
        public void Dispose()
        {
            _playerSlingshotService.OnPlayerSlingshotConfigChange -= OnChangeState;
        }
        
        private void OnChangeState(IPlayerSlingshotConfig config)
        {
            _config = config;
        }

        public void FixedTick()
        {
            FixedUpdateDrag();
        }
        
        public Vector2 Position => _physicBody2D.Position;
        public Vector2 DeltaDrag => _currentDeltaDragPosition;
        public float StrengthDrag => _currentStrength;
        
        public void BeginDrag()
        {
            _isDragging = true;
            _currentDeltaDragBodyPosition = Vector2.zero;
            _currentDeltaDragPosition = Vector2.zero;
        }

        public void StayDrag(Vector2 positionMouse, Vector2 contactClosestPoint)
        {
            if (!_isDragging)
                return;

            var position = _physicBody2D.Position;
            position += _currentDeltaDragBodyPosition;
            
            var delta = position - positionMouse;
            var deltaBody = delta;
            var distance = delta.magnitude;
            
            if (distance > _config.MaxDragDistanceBody)
                deltaBody = deltaBody.normalized * _config.MaxDragDistanceBody;
            
            if (distance > _config.MaxDragDistance)
            {
                distance = _config.MaxDragDistance;
                delta = delta.normalized * _config.MaxDragDistance;
            }

            _currentDeltaDragPosition = delta;
            _currentStrength = distance / _config.MaxDragDistance;
            _currentDeltaDragBodyPosition = deltaBody;
            
            _currentContactDirection = (_physicBody2D.Position - contactClosestPoint).normalized;
        }

        public void EndDrag(Vector2 contactClosestPoint)
        {
            if (!_isDragging)
                return;
            
            if (_currentStrength > 0.1f)
                ApplyThrowForce(_currentDeltaDragBodyPosition, _currentStrength, contactClosestPoint);
            
            _currentDeltaDragBodyPosition = Vector2.zero;
            _currentDeltaDragPosition = Vector2.zero;
            _currentStrength = 0f;
            _isDragging = false;
        }

        private void FixedUpdateDrag()
        {
            if (!_isDragging || !_physicBody2D.IsCompensationGravity)
                return;
            
            if (Vector2.Angle(_currentContactDirection, _currentDeltaDragBodyPosition.normalized) < _config.AngleForForce)
                _physicBody2D.AddForce(-_currentContactDirection * _currentStrength * _config.DragForce);
        }
        
        private void ApplyThrowForce(Vector2 direction, float strength, Vector2 contactClosestPoint)
        {
            direction = direction.normalized;
            Vector2 throwForce = direction * strength * _config.ThrowForce;
            Vector2 dirContact = (_physicBody2D.Position - contactClosestPoint).normalized;

            _physicBody2D.AddTorqueImpulse(Vector3.Cross(dirContact, throwForce.normalized).z * throwForce.magnitude);
            _physicBody2D.AddImpulse(throwForce);
        }
    }
}