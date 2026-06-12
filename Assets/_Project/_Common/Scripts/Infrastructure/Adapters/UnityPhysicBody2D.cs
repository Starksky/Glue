using System.Collections.Generic;
using _Project._Common.Scripts.Contracts.Data;
using _Project._Common.Scripts.Contracts.Interfaces;
using JetBrains.Annotations;
using SaintsField;
using UnityEngine;

namespace _Project._Common.Scripts.Infrastructure.Adapters
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class UnityPhysicBody2D : MonoBehaviour, IPhysicBody2D
    {
        private const float COMPENSATION_GRAVITY_MULTIPLIER = 1.08f;

        [SerializeField, ReadOnly, GetComponent(typeof(Rigidbody2D))]
        private Rigidbody2D body2D;

        private Stack<SnapshotPhysicBody2D> _snapshots = new();

        public Vector2 Position
        {
            get => body2D.position;
            set => body2D.position = value;
        }
        
        public Vector2 LinearVelocity
        {
            get => body2D.linearVelocity;
            set => body2D.linearVelocity = value;
        }
        public float AngularVelocity
        {
            get => body2D.angularVelocity;
            set => body2D.angularVelocity = value;
        }
        public float Mass
        {
            get => body2D.mass;
            set => body2D.mass = value;
        }
        public float LinearDamping
        {
            get => body2D.linearDamping;
            set => body2D.linearDamping = value;
        }
        public float AngularDamping
        {
            get => body2D.angularDamping;
            set => body2D.angularDamping = value;
        }
        
        public Vector2 LastLinearVelocity { get; private set; }
        public bool IsCompensationGravity { get; set; }
        
        public void AddForce(Vector2 force)
        {
            body2D.AddForce(force);
            LastLinearVelocity = LinearVelocity;
        }
        public void AddImpulse(Vector2 impulse) 
        {
            body2D.AddForce(impulse, ForceMode2D.Impulse);
            LastLinearVelocity = LinearVelocity;
        }
        public void AddTorqueImpulse(float force) => body2D.AddTorque(force, ForceMode2D.Impulse);
        public void MoveTo(Vector2 position) => body2D.MovePosition(position);

        public void Snapshot()
        {
            var snapshot = new SnapshotPhysicBody2D(Mass, LinearDamping, AngularDamping);
            _snapshots.Push(snapshot);
        }
        public void RestoreToSnapshot()
        {
            if (_snapshots.Count == 0)
                return;
            
            var snapshot = _snapshots.Pop();
            
            Mass = snapshot.Mass;
            LinearDamping = snapshot.LinearDamping;
            AngularDamping = snapshot.AngularDamping;
        }

        private void FixedUpdate()
        {
            if (!IsCompensationGravity)
                return;

            Vector2 gravityForce = Physics2D.gravity * body2D.mass * COMPENSATION_GRAVITY_MULTIPLIER * body2D.gravityScale;
            body2D.AddForce(-gravityForce, ForceMode2D.Force);
        }
    }
}