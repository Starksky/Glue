using _Project.Features.Surfaces.Common.Scripts.Contracts;
using UnityEngine;

namespace _Project.Features.Surfaces.Common.Scripts.Infrastructure
{
    [CreateAssetMenu(fileName = "StickableSurfaceConfig", menuName = "Surface/StickableSurfaceConfig", order = 0)]
    public class StickableSurfaceConfigSo : BaseSurfaceConfigSo, IStickableSurfaceConfig
    {
        [Header("Stickable Surface")]
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