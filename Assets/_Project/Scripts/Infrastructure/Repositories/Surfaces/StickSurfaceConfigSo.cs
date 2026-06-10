using _Project.Scripts.Contracts.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Repositories.Surfaces
{
    [CreateAssetMenu(fileName = "StickSurfaceConfig", menuName = "Surface", order = 0)]
    public class StickSurfaceConfigSo : BaseSurfaceConfigSo, IStickSurfaceConfig
    {
        [SerializeField] private float velocityThreshold = 20f;
        [SerializeField] private float distanceThreshold = 1f;
        [SerializeField] private float force = 10f;
        [SerializeField] private bool isJoint;
        
        public float VelocityThreshold => velocityThreshold;
        public float DistanceThreshold => distanceThreshold;
        public float Force => force;
        public bool IsJoint => isJoint;
    }
}