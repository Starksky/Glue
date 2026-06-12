using _Project.Features.Surfaces.Common.Scripts.Contracts;
using UnityEngine;

namespace _Project.Features.Surfaces.Common.Scripts.Infrastructure
{
    public abstract class BaseSurfaceConfigSo : ScriptableObject, IBaseSurfaceConfig
    {
        [SerializeField] private float linearDumping = 4f;
        [SerializeField] private float angularDumping = 3f;
        
        public float LinearDumping => linearDumping;
        public float AngularDumping => angularDumping;
    }
}