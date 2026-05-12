using Project.Core.Surfaces.Scripts;
using Project.Shared.Scripts.Extensions;
using R3;
using SaintsField;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Core.Player.Scripts
{
    [DefaultExecutionOrder(10)]
    [RequireComponent(typeof(Rigidbody2D), 
        typeof(SlingshotHandler))]
    public class StickHandler : MonoBehaviour
    {
        private const float COMPENSATION_GRAVITY_MULTIPLIER = 1.08f;
        
        
        [SerializeField, ReadOnly, GetComponent(typeof(Rigidbody2D))]
        private Rigidbody2D rigidbody2D;
        [SerializeField, ReadOnly, GetComponent(typeof(SlingshotHandler))] private SlingshotHandler slingshotHandler;
        [SerializeField] private UnityEvent EventStiked;
        
        
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private readonly ReactiveProperty<bool> _isStick = new ReactiveProperty<bool>();
        private Collider2D _currentSurface;
        private BaseSurfaceHandler _currentSurfaceHandler;
        private Rigidbody2DParams _defaultRigidbody2DParams;
        private bool _isCompensationGravity;

        
        public Rigidbody2D Rigidbody2D => rigidbody2D;
        public Rigidbody2DParams DefaultRigidbody2DParams => _defaultRigidbody2DParams;
        public float ThrowForceMultiplier
        {
            get => slingshotHandler.ThrowForceMultiplier;
            set => slingshotHandler.ThrowForceMultiplier = value;
        }
        public Vector2 LastThrowVelocity
        {
            get => slingshotHandler.LastThrowVelocity;
            set => slingshotHandler.LastThrowVelocity = value;
        }
        public float StickMultiplier { get; set; } = 1f;
        public float UnstickMultiplier { get; set; } = 1f;
        public bool IsCompensationGravity
        {
            get => _isCompensationGravity;
            set => _isCompensationGravity = value;
        }

        
        
        private void Awake()
        {
            _defaultRigidbody2DParams.Set(rigidbody2D);
            _isStick.Subscribe(OnChangedStick).AddTo(_compositeDisposable);
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
            
            EventStiked.Invoke();
        }

        /*private void OnTriggerStay2D(Collider2D other)
        {
            if (other.isTrigger)
                return;
            
            if (_currentSurface == other)
                if (!_isStick.Value && _currentSurfaceHandler)
                    _isStick.Value = _currentSurfaceHandler.OnStick(this, _currentSurface);
        }*/

        /*private void OnTriggerExit2D(Collider2D other)
        {
            if (other.isTrigger)
                return;
            
            if (_currentSurface == other)
                if (_isStick.Value && _currentSurfaceHandler)
                {
                    _currentSurfaceHandler.OnUnstick(this, _currentSurface);
                    _isStick.Value = false;
                }
        }*/
        
        private void FixedUpdate() 
        {
            if (IsCompensationGravity)
            {
                Vector2 gravityForce = Physics2D.gravity * rigidbody2D.mass * COMPENSATION_GRAVITY_MULTIPLIER * rigidbody2D.gravityScale;
                rigidbody2D.AddForce(-gravityForce, ForceMode2D.Force);
            }
             
            if (_isStick.Value && _currentSurfaceHandler)
                _isStick.Value = _currentSurfaceHandler.OnUpdateStick(this, _currentSurface);
        }

        private void OnChangedStick(bool stick)
        {
            if (stick && _currentSurfaceHandler)
                _currentSurfaceHandler.StickParams.Apply(rigidbody2D);
            else rigidbody2D.ApplyParams(_defaultRigidbody2DParams);
        }
    }
}