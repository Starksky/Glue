using _Project.Features.Surfaces.Common.Scripts.Contracts;

namespace _Project.Features.Surfaces.Spring.Scripts.Contracts
{
    public interface ISpringSurfaceConfig : IStickableSurfaceConfig
    {
        public int AddThrowForcePercent { get; }
    }
}