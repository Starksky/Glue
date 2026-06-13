using System.Linq;
using Generated;
using R3;
using UnityEngine;
using UnityEngine.Timeline;
using VContainer;

namespace _Project._Common.Scripts.Infrastructure.ZeroMessenger
{
    [DefaultExecutionOrder(-10)]
    public class MonoZeroMessengerReceive : MonoBehaviour
    {
        [SerializeField] private MessengerChannels channel;
        [SerializeField] private MonoZeroMessengerHandler[] handlers;
        private ZeroMessengerService _zeroMessengerService;

        [Inject]
        public void Construct(ZeroMessengerService zeroMessengerService)
        {
            _zeroMessengerService = zeroMessengerService;
        }
        
        private void Awake()
        {
            _zeroMessengerService.Subscribe<MessengerChannels, SignalAsset>(channel, m =>
                {
                    if (handlers.FirstOrDefault(h => h.message.name == m.name)?.handler is not {} handler)
                        return;
                    
                    Debug.Log($"{gameObject.name} Receive signal: {m.name}");
                    handler.Invoke();
                })
            .RegisterTo(destroyCancellationToken);
        }
    }
}