using _Project.Features.Surfaces.Common.Scripts.Infrastructure;
using _Project.Features.Surfaces.Spring.Scripts.Contracts;
using UnityEngine;

namespace _Project.Features.Surfaces.Spring.Scripts.Infrastructure
{
    [CreateAssetMenu(fileName = "SpringSurfaceConfig", menuName = "Surface/SpringSurfaceConfig", order = 0)]
    public class SpringSurfaceConfigSo : StickableSurfaceConfigSo, ISpringSurfaceConfig
    {
        [Header("Spring Surface")]
        [SerializeField, Range(1, 100)] private int addThrowForcePercent = 100;
        public int AddThrowForcePercent => addThrowForcePercent;
    }
}