using System;
using Project.Core.Services;
using Project.Core.Surfaces.Scripts;
using R3;
using SaintsField;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Project.Core.Player.Scripts
{
    [RequireComponent(typeof(Rigidbody2D),
        typeof(CircleCollider2D))]
    public class SlingshotHandler : MonoBehaviour, IPoolable<Vector3, IMemoryPool>
    {
        [SerializeField, ReadOnly, GetComponent(typeof(Rigidbody2D))]
        private Rigidbody2D rigidbody2D;
        [SerializeField, ReadOnly, GetComponent(typeof(CircleCollider2D))]
        private CircleCollider2D collider2D;

        [Space]
        [SerializeField] private float maxDragDistanceBody = 0.25f;
        [SerializeField] private float maxDragDistance = 1.8f;
        [SerializeField] private float maxForce = 8f;
        [SerializeField] private float angleForForce = 45f;
        [SerializeField] private float dragForce = 10f;
        
        [Space,SerializeField] private UnityEvent EventSpawned;
        [SerializeField] private UnityEvent EventDrag;

        [Inject] private GameService _gameService; 
        
        private bool _isDragging;
        private Vector2 _spawnPosition;
        private Vector2 _currentDeltaDragBodyPosition;
        private Vector2 _currentDeltaDragPosition;
        private float _currentStrength;
        private Collider2D _currentAttachCollider;
        private Vector2 _dragPosition;
        private Rigidbody2DParams _rigidbody2DParamsBuffer;

        private IMemoryPool _pool;
        public Subject<Collider2D> EventEnterTrigger { get; } = new Subject<Collider2D>();

        public float ThrowForceMultiplier { get; set; } = 1f;
        public Vector2 LastThrowVelocity { get; set; } 
        public float CurrentStrength => _currentStrength;
        public Vector2 Position => rigidbody2D.position;
        public Vector2 CurrentDelta => _currentDeltaDragPosition;
        public bool IsFlying => !_currentAttachCollider;

        private void Awake()
        {
            _gameService.SetPlayer(transform);
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.isTrigger || _isDragging)
                return;

            _currentAttachCollider = other;
        }
        
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.isTrigger || _isDragging)
                return;

            _currentAttachCollider = other;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.isTrigger || _isDragging)
                return;
            
            Debug.Log("OnTriggerExit2D");
            _currentAttachCollider = null;
        }

        private void ClearRigidbody()
        {
            rigidbody2D.angularVelocity = 0f;
            rigidbody2D.linearVelocity = Vector2.zero;
            _rigidbody2DParamsBuffer.Set(rigidbody2D);
            rigidbody2D.angularDamping = 0.05f;
            rigidbody2D.linearDamping = 0f;
        }
        
        
        private void ApplyThrowForce(Vector2 direction, float strength)
        {
            /*if (!_currentAttachCollider)
                return;*/
            
            Vector2 throwForce = direction * strength * maxForce * ThrowForceMultiplier;
            Vector2 r = (Vector2)transform.position - _currentAttachCollider.ClosestPoint(transform.position);

            rigidbody2D.AddTorque(Vector3.Cross(r, throwForce.normalized).z * throwForce.magnitude, ForceMode2D.Impulse);
            rigidbody2D.AddForce(throwForce, ForceMode2D.Impulse);
            LastThrowVelocity = rigidbody2D.linearVelocity;
        }
        
        public void ApplyForce(Vector2 force, ForceMode2D mode = ForceMode2D.Force)
            => rigidbody2D.AddForce(force, mode);

        public void StartDrag()
        {
            _isDragging = true;
            _currentDeltaDragBodyPosition = Vector2.zero;
            _currentDeltaDragPosition = Vector2.zero;
            EventDrag.Invoke();
        }
        
        public void UpdateDrag(Vector2 positionMouse)
        {
            if (!_isDragging)
                return;

            var position = (Vector2)transform.position;
            position += _currentDeltaDragBodyPosition;
            
            var delta = position - positionMouse;
            var deltaBody = delta;
            var distance = delta.magnitude;
            
            if (distance > maxDragDistanceBody)
                deltaBody = deltaBody.normalized * maxDragDistanceBody;
            
            if (distance > maxDragDistance)
            {
                distance = maxDragDistance;
                delta = delta.normalized * maxDragDistance;
            }

            _currentDeltaDragPosition = delta;
            _currentStrength = distance / maxDragDistance;
            _currentDeltaDragBodyPosition = deltaBody;
            //position -= deltaBody;
            //transform.position = position;
            
            if (_currentAttachCollider)
            {
                Vector2 r = (Vector2)transform.position - _currentAttachCollider.ClosestPoint(transform.position);
                if (Vector2.Angle(r.normalized, deltaBody.normalized) < angleForForce)
                    rigidbody2D.AddForce(-r.normalized * _currentStrength * dragForce, ForceMode2D.Force);
            }
        }
        
        public void EndDrag()
        {
            if (!_isDragging)
                return;
            
            //rigidbody2D.position += _currentDeltaDragBodyPosition;

            if (_currentStrength > 0.1f)
            {
                ClearRigidbody();
                ApplyThrowForce(_currentDeltaDragBodyPosition.normalized, _currentStrength);
                _rigidbody2DParamsBuffer.Apply(rigidbody2D);
            }
            
            _currentDeltaDragBodyPosition = Vector2.zero;
            _currentDeltaDragPosition = Vector2.zero;
            _currentStrength = 0f;
            _isDragging = false;
        }

        public void RepeatApplyThrowForce() => rigidbody2D.linearVelocity = LastThrowVelocity;

        public void OnDespawned()
        {
            Debug.Log("OnDespawned");
        }
        
        public void OnSpawned(Vector3 p1, IMemoryPool pool)
        {
            _pool = pool;
            _isDragging = false;
            _currentDeltaDragBodyPosition = Vector2.zero;
            _currentDeltaDragPosition = Vector2.zero;
            transform.position = (Vector2)p1;
            ClearRigidbody();
            
            EventSpawned.Invoke();
        }

        public void Despawn()
        {
            Debug.Log("Despawn");
            _pool.Despawn(this);
        }
        
        
        public class Pool : MonoPoolableMemoryPool<Vector3, IMemoryPool, SlingshotHandler>
        {}
    }
}