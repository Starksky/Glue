using System;
using SaintsField;
using UnityEngine;

namespace Project.Core.Player.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class StickHandler : MonoBehaviour
    {
        [SerializeField, ReadOnly, GetComponent(typeof(Rigidbody2D))]
        private Rigidbody2D rigidbody2D;
        
        [SerializeField] private float stickThreshold = 50f;
        [SerializeField] private float stickForce = 50f; // Сила прижатия к стене
        [SerializeField] private float unstickThreshold = 0.5f;
        [SerializeField] private float linearDumping = 0.5f;
        [SerializeField] private float angularDamping = 0.5f;
        
        private Collider2D _currentSurface;
        private bool isStick;
        private Vector2 stickPoint;
        private float _gravityScaleDefault;
        private float _linearDumpingDefault;
        private float _angularDampingDefault;
        private void Awake()
        {
            _gravityScaleDefault = rigidbody2D.gravityScale;
        }

        private void OnEnable()
        {
            //throw new NotImplementedException();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.isTrigger)
                return;
            
            if (rigidbody2D.linearVelocity.magnitude > stickThreshold)
                AttachToSurface(other.ClosestPoint(transform.position));
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.isTrigger)
                return;
            
            if (isStick)
                stickPoint = other.ClosestPoint(transform.position);
        }

        private void AttachToSurface(Vector2 point)
        {
            //rigidbody2D.gravityScale = 0;
            
            //rigidbody2D.linearVelocity = Vector2.zero;
            //rigidbody2D.
            /*rigidbody2D.linearVelocity = Vector2.zero;
            rigidbody2D.angularVelocity = 0f;*/
            //rigidbody2D.angularDamping = 1f;
            stickPoint = point;
            isStick = true;
            /*Vector2 toStickPoint = (stickPoint - (Vector2)transform.position).normalized;
            var v = rigidbody2D.linearVelocity * toStickPoint;
            rigidbody2D.linearVelocity += v;*/
        }
        
        private void StickToSurface()
        {
            Vector2 toStickPoint = stickPoint - (Vector2)transform.position;
            
            
            if (toStickPoint.magnitude > unstickThreshold)
            {
                rigidbody2D.linearDamping = _linearDumpingDefault;
                rigidbody2D.angularDamping = _angularDampingDefault;
                rigidbody2D.gravityScale = _gravityScaleDefault;
                isStick = false;
            }
            else
            {
                rigidbody2D.gravityScale = 0;
                rigidbody2D.linearDamping = linearDumping;
                rigidbody2D.angularDamping = angularDamping;
                rigidbody2D.AddForce(toStickPoint.normalized * stickForce, ForceMode2D.Force);
            }
        }

        private void FixedUpdate()
        {
            if (isStick)
                StickToSurface();
        }
    }
}