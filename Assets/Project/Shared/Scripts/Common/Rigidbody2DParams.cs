using System;
using UnityEngine;

namespace Project.Core.Surfaces.Scripts
{
    [Serializable]
    public struct Rigidbody2DParams
    {
        public float gravityScale;
        public float linearDumping;
        public float angularDumping;

        public void Set(Rigidbody2D body)
        {
            gravityScale = body.gravityScale;
            linearDumping = body.linearDamping;
            angularDumping = body.angularDamping;
        }
        
        public void Apply(Rigidbody2D body)
        {
            body.gravityScale = gravityScale;
            body.linearDamping = linearDumping;
            body.angularDamping = angularDumping;
        }
    }
}