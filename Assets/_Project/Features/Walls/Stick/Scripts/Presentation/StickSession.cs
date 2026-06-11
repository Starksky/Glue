using _Project._Common.Scripts.Utils;

namespace _Project.Features.Walls.Stick.Scripts.Presentation
{
    public class StickSession
    {
        public float Multiplier { get; set; } = 1f;
        public RestartableTimer Timer { get; } = new RestartableTimer();
        
        public virtual void Dispose()
        {
            Timer.Dispose();
        }
    }
}