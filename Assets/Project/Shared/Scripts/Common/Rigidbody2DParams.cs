using System;
using UnityEngine;

namespace Project.Core.Surfaces.Scripts
{
    [Serializable]
    public struct Rigidbody2DParams
    {
        public float linearDumping;
        public float angularDumping;

        public void Set(Rigidbody2D body)
        {
            linearDumping = body.linearDamping;
            angularDumping = body.angularDamping;
        }
        
        public void Apply(Rigidbody2D body)
        {
            body.linearDamping = linearDumping;
            body.angularDamping = angularDumping;
        }
    }
}