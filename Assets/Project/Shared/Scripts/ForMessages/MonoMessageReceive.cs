using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;
using Zenject;

namespace Project.Shared.Scripts.ForMessages
{
    public class MonoMessageReceive : BaseMonoMessage
    {
        [SerializeField] private MonoMessageHandler[] handlers;

        [Inject] private MessageBrokersService _messageBrokersService;
        
        private void Awake()
        {
            _messageBrokersService.Subscribe<SignalAsset>(Chanel, m => handlers.FirstOrDefault(h => h.message == m)?.handler?.Invoke());
        }
    }
}