using _Project.Features.Surfaces.Common.Scripts.Contracts;

namespace _Project.Features.Surfaces.Rough.Scripts.Contracts
{
    public interface IRoughSurfaceConfig : IStickableSurfaceConfig
    {
        public float DelayByShift { get; }
        public float ScaleShiftStick { get; }
    }
}