using UnityEngine;

namespace _Project._Common.Scripts.Contracts.Interfaces
{
    public interface IPhysicBody2D
    {
        public Vector2 Position { get; set; }
        public Vector2 LinearVelocity { get; set; }
        public Vector2 LastLinearVelocity { get; }
        public float AngularVelocity { get; set; }
        
        public float Mass { get; set; }
        public float LinearDamping { get; set; }
        public float AngularDamping { get; set; }
        
        public bool IsCompensationGravity {get; set;}

        public void AddForce(Vector2 force);
        public void AddImpulse(Vector2 impulse);
        public void AddTorqueImpulse(float force);
        public void MoveTo(Vector2 position);
        public void Snapshot();
        public void RestoreToSnapshot();
    }
}