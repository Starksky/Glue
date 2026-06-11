namespace _Project.Scripts.Contracts.Interfaces
{
    public interface IStickSurfaceConfig : IBaseSurfaceConfig
    {
        public float VelocityThreshold { get; }
        public float DistanceThreshold { get; }
        public float Force { get; }
        public bool IsJoint { get; }
    }
}