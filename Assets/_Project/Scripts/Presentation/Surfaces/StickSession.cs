using _Project.Scripts.Utils;

namespace _Project.Scripts.Presentation.Surfaces
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