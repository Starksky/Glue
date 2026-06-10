using System;
using SaintsField;
using UnityEngine;

namespace Project.Core.Player.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class OldStickHandler : MonoBehaviour
    {
        [SerializeField, ReadOnly, GetComponent(typeof(Rigidbody2D))]
        private Rigidbody2D rigidbody2D;

        [SerializeField] private float delayStick = 0.02f;
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
        
        
        private void Awake()
        {
            _gravityScaleDefault = rigidbody2D.gravityScale;
            _linearDumpingDefault = rigidbody2D.linearDamping;
            _angularDampingDefault = rigidbody2D.angularDamping;
            _restartableTimer = new RestartableTimer();
        }

        private void OnEnable()
        {
            _restartableTimer.Start(delayStick);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.isTrigger)
                return;
            
            var length = rigidbody2D.linearVelocity.magnitude;
            if (length >= stickThresholdMin && length <= stickThresholdMax)
                AttachToSurface(other.ClosestPoint(transform.position));
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.isTrigger)
                return;
            
            var length = rigidbody2D.linearVelocity.magnitude;
            if (isStick)
                stickPoint = other.ClosestPoint(transform.position);
            else if (length >= stickThresholdMin && length <= stickThresholdMax)
                AttachToSurface(other.ClosestPoint(transform.position));
        }

        /*private void OnTriggerExit2D(Collider2D other)
        {
            if (other.isTrigger)
                return;
            
            ToDefault();
        }*/

        private void AttachToSurface(Vector2 point)
        {
            _restartableTimer.Stop();
            rigidbody2D.gravityScale = 0;
            rigidbody2D.linearDamping = linearDumping;
            rigidbody2D.angularDamping = angularDamping;
            stickPoint = point;
            isStick = true;
        }
        
        private void StickToSurface()
        {
            Vector2 toStickPoint = stickPoint - (Vector2)transform.position;

            if (toStickPoint.magnitude > unstickThreshold)
                ToDefault();
            else
            {
                rigidbody2D.gravityScale = 0;
                rigidbody2D.linearDamping = linearDumping;
                rigidbody2D.angularDamping = angularDamping;
                rigidbody2D.AddForce(toStickPoint.normalized * stickForce, ForceMode2D.Force);
            }
        }

        private void ToDefault()
        {
            rigidbody2D.linearDamping = _linearDumpingDefault;
            rigidbody2D.angularDamping = _angularDampingDefault;
            rigidbody2D.gravityScale = _gravityScaleDefault;
            isStick = false;
            _restartableTimer.Start(delayStick);
        }

        private void FixedUpdate()
        {
            if (isStick)
                StickToSurface();
        }
    }
}