using _Project._Common.Scripts.Utils;

namespace _Project.Features.Surfaces.Stick.Scripts.Presentation
{
    public class StickableSession
    {
        public float Multiplier { get; set; } = 1f;
        public RestartableTimer Timer { get; } = new RestartableTimer();
        
        public virtual void Dispose()
        {
            Timer.Dispose();
        }
    }
}