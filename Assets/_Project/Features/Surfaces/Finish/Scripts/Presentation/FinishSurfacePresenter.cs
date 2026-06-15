using _Project._Common.Scripts.Infrastructure.ZeroMessenger;
using _Project._Common.Scripts.Signals;
using _Project.Features.Surfaces.Finish.Scripts.Contracts;

namespace _Project.Features.Surfaces.Finish.Scripts.Presentation
{
    public class FinishSurfacePresenter : IFinishSurfacePresenter
    {
        private readonly IZeroMessengerService _zeroMessengerService;
        public FinishSurfacePresenter(IZeroMessengerService  zeroMessengerService)
        {
            _zeroMessengerService = zeroMessengerService;

        }
        public void OnFinish()
        {
            _zeroMessengerService.Publish(new MapCompleteSignal());
        }
    }
}