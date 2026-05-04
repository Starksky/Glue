using SaintsField;
using UnityEngine;

namespace Project.Core.Surfaces.Scripts
{
    public class StickSurfaceHandler : MonoBehaviour
    {
        [SerializeField] private Collider2D colliderSurface;
        [SerializeField, ReadOnly] private Rigidbody2D rigidbody2D;

        [SerializeField] private bool isGravity;
        [SerializeField] private float stickThresholdMin = 0f;
        [SerializeField] private float stickThresholdMax = 1f;
        [SerializeField] private float stickForce = 50f;
        [SerializeField] private float unstickThreshold = 0.5f;
        [SerializeField] private float linearDumping = 0.5f;
        [SerializeField] private float angularDamping = 0.5f;
        
        private Collider2D _currentSurface;
        private bool isStick;
        private Vector2 stickPoint;
        private float _gravityScaleDefault;
        private float _linearDumpingDefault;
        private float _angularDampingDefault;
        private RestartableTimer _restartableTimer;
        
        
        private void Initialize(Collider2D other)
        {
            rigidbody2D = other.attachedRigidbody;
            _gravityScaleDefault = rigidbody2D.gravityScale;
            _linearDumpingDefault = rigidbody2D.linearDamping;
            _angularDampingDefault = rigidbody2D.angularDamping;
        }
        

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            if (!rigidbody2D)
                Initialize(other);
            
            var length = rigidbody2D.linearVelocity.magnitude;
            if (length >= stickThresholdMin && length <= stickThresholdMax)
                AttachToSurface();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                return;
            
            var length = rigidbody2D.linearVelocity.magnitude;
            if (length >= stickThresholdMin && length <= stickThresholdMax)
                AttachToSurface();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.isTrigger)
                return;
            
            ToDefault();
        }

        private void AttachToSurface()
        {
            rigidbody2D.gravityScale = !isGravity ? 0 : rigidbody2D.gravityScale;
            rigidbody2D.linearDamping = linearDumping;
            rigidbody2D.angularDamping = angularDamping;
            isStick = true;
        }
        
        private void StickToSurface()
        { 
            var closestPoint = colliderSurface.ClosestPoint(rigidbody2D.position);
            Vector2 position = rigidbody2D.position;
            Vector2 toStickPoint = closestPoint - position;

            if (toStickPoint.magnitude > unstickThreshold)
                ToDefault();
            else
            {
                rigidbody2D.gravityScale = !isGravity ? 0 : rigidbody2D.gravityScale;
                rigidbody2D.linearDamping = linearDumping;
                rigidbody2D.angularDamping = angularDamping;
                Debug.DrawRay(position,  toStickPoint.normalized, Color.magenta, 1f);
                rigidbody2D.AddForce(toStickPoint.normalized * stickForce, ForceMode2D.Force);
            }
        }

        private void ToDefault()
        {
            rigidbody2D.linearDamping = _linearDumpingDefault;
            rigidbody2D.angularDamping = _angularDampingDefault;
            rigidbody2D.gravityScale = _gravityScaleDefault;
            isStick = false;
        }


        private void FixedUpdate()
        {
            if (isStick)
                StickToSurface();
        }
    }
}