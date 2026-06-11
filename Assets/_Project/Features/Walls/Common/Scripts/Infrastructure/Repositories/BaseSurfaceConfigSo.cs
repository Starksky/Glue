using _Project.Features.Walls.Common.Scripts.Contracts;
using UnityEngine;

namespace _Project.Features.Walls.Common.Scripts.Infrastructure.Repositories
{
    public abstract class BaseSurfaceConfigSo : ScriptableObject, IBaseSurfaceConfig
    {
        [SerializeField] private float linearDumping;
        [SerializeField] private float angularDumping;
        
        public float LinearDumping => linearDumping;
        public float AngularDumping => angularDumping;
    }
}