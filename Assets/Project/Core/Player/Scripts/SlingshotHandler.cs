using System;
using R3;
using SaintsField;
using UnityEngine;

namespace Project.Core.Player.Scripts
{
    [RequireComponent(typeof(Rigidbody2D),
        typeof(CircleCollider2D))]
    public class SlingshotHandler : MonoBehaviour
    {
        [SerializeField, ReadOnly, GetComponent(typeof(Rigidbody2D))]
        private Rigidbody2D rigidbody2D;
        [SerializeField, ReadOnly, GetComponent(typeof(CircleCollider2D))]
        private CircleCollider2D collider2D;
        [SerializeField] private CircleCollider2D helpCollider2D;

        [Space]
        [SerializeField] private float delayToAttach = 0.4f;
        [SerializeField] private float delayToKinematic = 0.05f;
        [SerializeField] private float maxDragDistanceBody = 0.25f;
        [SerializeField] private float maxDragDistance = 1.8f;
        [SerializeField] private float forceMultiplier = 8f;
        [SerializeField] private float forceAwakeMultiplier = 0.2f;
        [SerializeField] private float awakeRadiusCollider = 0.4f;
        [SerializeField] private float defaultRadiusCollider = 0.5f;
        [Space] 
        [SerializeField] private bool debugAwake;
        
        private CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private RestartableTimer _timerForDelayToAttach;
        private RestartableTimer _timerForDelayToKinematic;
        
        private bool _isFlying;
        private bool _isDragging;
        private bool _isAwake;
        
        private Vector2 _currentDeltaDragBodyPosition;
        private Vector2 _currentDeltaDragPosition;
        private float _currentStrength;
        private Vector2 _currentAwakeForce;
        private Vector2 _lastContactPosition;
        private Collider2D _currentAttachCollider;

        public Subject<Collider2D> EventEnterTrigger { get; } = new Subject<Collider2D>();
        
        public Collider2D Collider => collider2D;
        public float CurrentStrength => _currentStrength;
        public Vector2 Position => rigidbody2D.position;
        public Vector2 CurrentDelta => _currentDeltaDragPosition;
        public bool IsFlying => _isFlying;

        private void Awake()
        {
            _timerForDelayToAttach = new RestartableTimer(delayToAttach);
            _timerForDelayToKinematic = new RestartableTimer(delayToKinematic);
            
            _timerForDelayToKinematic.OnCompleted.Subscribe(_ => 
                ToKinematic()).AddTo(_compositeDisposable);
        }

        private void OnEnable()
        {
            _timerForDelayToAttach.Start();
            collider2D.radius = defaultRadiusCollider;
        }

        private void OnDestroy()
        {
            _timerForDelayToAttach.Dispose();
            _timerForDelayToKinematic.Dispose();
            _compositeDisposable.Dispose();
        }

        private void Update()
        {
            _timerForDelayToAttach.Update();
            _timerForDelayToKinematic.Update();
        }
        
        /*private void FixedUpdate()
        {
            if (_isAwake && _currentAttachCollider)
            {
                _lastContactPosition = _currentAttachCollider.ClosestPoint(transform.position);
                
                var dirConnect = (_lastContactPosition - (Vector2)transform.position).normalized;
                
                if (debugAwake)
                    Debug.DrawRay(transform.position,  dirConnect, Color.magenta, 1f);
                
                _currentAwakeForce = dirConnect
                                     * -Physics2D.gravity.y 
                                     * forceAwakeMultiplier;
                
                rigidbody2D.AddForce(_currentAwakeForce, ForceMode2D.Force);
            }
        }*/

        /*private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.isTrigger || _isDragging)
                return;
            
            if (_currentAttachCollider != other)
            {
                if (!_timerForDelayToAttach.IsCompleted &&
                    !_timerForDelayToAttach.IsRunning)
                    _timerForDelayToAttach.Start();
                
                _currentAttachCollider = other;
            }
            
            if (_timerForDelayToAttach.IsCompleted)
                ToSleep();
        }*/


        /*private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.isTrigger || _isDragging)
                return;
            
            EventEnterTrigger.OnNext(other);
            
            if (_timerForDelayToAttach.IsCompleted)
                ToSleep();
        }*/
        
        /*private void OnTriggerStay2D(Collider2D other)
        {
            if (other.isTrigger || _isDragging)
                return;

            //_currentAttachCollider = other;

            if (_timerForDelayToAttach.IsCompleted/* && 
                !_timerForDelayToKinematic.IsRunning#1#)
                if (!helpCollider2D.IsTouching(other))
                    ToSleep(0.5f - other.friction);
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.isTrigger || _isDragging)
                return;
            
            _timerForDelayToKinematic.Stop();
        }*/
        
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.isTrigger || _isDragging)
                return;

            _currentAttachCollider = other;
        }

        private void ApplyThrowForce(Vector2 direction, float strength)
        {
            Vector2 throwForce = direction * strength * forceMultiplier;
            Vector2 r = (Vector2)transform.position - _currentAttachCollider.ClosestPoint(transform.position);
            rigidbody2D.AddTorque(Vector3.Cross(r, throwForce.normalized).z * throwForce.magnitude, ForceMode2D.Impulse);
            rigidbody2D.AddForce(throwForce, ForceMode2D.Impulse);
        }
        
        private void ClearVelocity()
        {
            rigidbody2D.linearVelocity = Vector2.zero;
            rigidbody2D.angularVelocity = 0;
            _currentAwakeForce = Vector2.zero;
        }
        
        private void ToDynamic(bool clear = true)
        {
            _timerForDelayToKinematic.Stop();
            _timerForDelayToAttach.Start();
            
            if (clear)
                ClearVelocity();
            
            rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            rigidbody2D.constraints = RigidbodyConstraints2D.None;
            collider2D.radius = defaultRadiusCollider;
        }
        
        private void ToKinematic(bool clear = true)
        {
            _timerForDelayToAttach.Stop();
            
            if (clear)
                ClearVelocity();
            
            rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        
        private void ToDelayKinematic(float time) => _timerForDelayToKinematic.Start(time);

        public void ApplyForce(Vector2 force, ForceMode2D mode = ForceMode2D.Force)
            => rigidbody2D.AddForce(force, mode);
        public bool IsOverlapPoint(Vector2 point) => collider2D.OverlapPoint(point);
        public void ResetPosition(Transform point) => ResetPosition(point.position);
        public void ResetPosition(Vector2 position)
        {
            _timerForDelayToKinematic.Stop();
            
            _isDragging = false;
            _isFlying = true;
            _currentDeltaDragBodyPosition = Vector2.zero;
            _currentDeltaDragPosition = Vector2.zero;

            ToKinematic();
            
            rigidbody2D.position = position;
            transform.position = position;
            
            ToDynamic(false);
        }

        public void ToAwake()
        {
            if (rigidbody2D.bodyType != RigidbodyType2D.Kinematic)
                return;

            //_isAwake = true;
            //_timerForDelayToKinematic.Stop();
            //_timerForDelayToAttach.Stop();
            
            ToDynamic(false);
            _timerForDelayToAttach.Stop();

            collider2D.radius = awakeRadiusCollider;
        }

        public void ToUnAwake()
        {
            if (rigidbody2D.bodyType != RigidbodyType2D.Dynamic)
                return;
            
            ToDynamic(false);
        }

        public void ToSleep(float friction)
        {
            if (rigidbody2D.bodyType != RigidbodyType2D.Dynamic)
                return;

            _isAwake = false;
            _isFlying = false;

            _timerForDelayToAttach.Stop();
            //ClearVelocity();
            ToDelayKinematic(friction);
        }

        public void StartDrag()
        {
            _isDragging = true;
            _currentDeltaDragBodyPosition = Vector2.zero;
            _currentDeltaDragPosition = Vector2.zero;
        }
        
        public void UpdateDrag(Vector2 positionMouse)
        {
            if (!_isDragging)
                return;

            var position = rigidbody2D.position;
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
            position -= deltaBody;
            rigidbody2D.position = position;
        }
        
        public void EndDrag()
        {
            if (!_isDragging)
                return;
            
            rigidbody2D.position += _currentDeltaDragBodyPosition;

            if (_currentStrength > 0.1f)
            {
                ToDynamic();
                ApplyThrowForce(_currentDeltaDragBodyPosition.normalized, _currentStrength);
                _timerForDelayToAttach.Start();
                _isFlying = true;
            }
            
            _currentDeltaDragBodyPosition = Vector2.zero;
            _currentDeltaDragPosition = Vector2.zero;
            _currentStrength = 0f;
            _isDragging = false;
        }
    }
}