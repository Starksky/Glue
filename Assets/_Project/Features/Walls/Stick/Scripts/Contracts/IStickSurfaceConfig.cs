using _Project.Features.Walls.Common.Scripts.Contracts;

namespace _Project.Features.Walls.Stick.Scripts.Contracts
{
    public interface IStickSurfaceConfig : IBaseSurfaceConfig
    {
        public float VelocityThreshold { get; }
        public float DistanceThreshold { get; }
        public float Force { get; }
        public bool IsJoint { get; }
    }
}