using System.Threading;
using _Project._Common.Scripts.Utils;

namespace _Project.Features.Surfaces.Common.Scripts.Presentation
{
    public class StickableSession
    {
        public float Multiplier { get; set; } = 1f;
        public RestartableTimer Timer { get; } = new RestartableTimer();
        public CancellationToken DisableCancellationToken => _disableCancellationTokenSource.Token;
        private CancellationTokenSource _disableCancellationTokenSource;

        public void Begin()
        {
            _disableCancellationTokenSource = new CancellationTokenSource();
        }
        
        public void End()
        {
            Timer.Stop();
            _disableCancellationTokenSource?.Cancel();
            _disableCancellationTokenSource = null;
        }
        
        public virtual void Dispose()
        {
            End();
            Timer.Dispose();
        }
    }
}