using _Project.Features.Surfaces.Common.Scripts.Infrastructure;
using _Project.Features.Surfaces.Rough.Scripts.Contracts;
using UnityEngine;

namespace _Project.Features.Surfaces.Rough.Scripts.Infrastructure
{
    [CreateAssetMenu(fileName = "RoughSurfaceConfig", menuName = "Surface/RoughSurfaceConfig", order = 0)]
    public class RoughSurfaceConfigSo : StickableSurfaceConfigSo, IRoughSurfaceConfig
    {
        [Header("Rough Surface")]
        [SerializeField] private float delayByShift = 2f;
        [SerializeField] private float scaleShiftStick = 0.25f;

        public float DelayByShift => delayByShift;
        public float ScaleShiftStick => scaleShiftStick;
    }
}