using _Project.Scripts.Contracts.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Repositories.Surfaces
{
    public abstract class BaseSurfaceConfigSo : ScriptableObject, IBaseSurfaceConfig
    {
        [SerializeField] private float linearDumping;
        [SerializeField] private float angularDumping;
        
        public float LinearDumping => linearDumping;
        public float AngularDumping => angularDumping;
    }
}