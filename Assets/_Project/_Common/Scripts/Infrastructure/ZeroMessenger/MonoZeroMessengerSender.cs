using Generated;
using UnityEngine;
using UnityEngine.Timeline;
using VContainer;

namespace _Project._Common.Scripts.Infrastructure.ZeroMessenger
{
    public class MonoZeroMessengerSender : MonoBehaviour
    {
        [SerializeField] private MessengerChannels channel;
        private IZeroMessengerService _zeroMessengerService;
        
        [Inject]
        public void Construct(IZeroMessengerService zeroMessengerService)
        {
            _zeroMessengerService = zeroMessengerService;
        }
        public void SendMessage(SignalAsset signal) => _zeroMessengerService.Publish(channel, signal);
    }
}