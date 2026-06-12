using _Project.Features.Surfaces.Common.Scripts.Contracts;

namespace _Project.Features.Surfaces.Stick.Scripts.Contracts
{
    public interface IStickableSurfaceConfig : IBaseSurfaceConfig
    {
        public float VelocityThreshold { get; }
        public float DistanceThreshold { get; }
        public float Force { get; }
        public bool IsJoint { get; }
    }
}