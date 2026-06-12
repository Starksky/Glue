using _Project.Features.Surfaces.Common.Scripts.Infrastructure;
using _Project.Features.Surfaces.Stick.Scripts.Contracts;
using UnityEngine;

namespace _Project.Features.Surfaces.Stick.Scripts.Infrastructure
{
    [CreateAssetMenu(fileName = "StickSurfaceConfig", menuName = "Surface", order = 0)]
    public class StickableSurfaceConfigSo : BaseSurfaceConfigSo, IStickableSurfaceConfig
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