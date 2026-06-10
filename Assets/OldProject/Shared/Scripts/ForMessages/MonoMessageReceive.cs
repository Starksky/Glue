using System.Linq;
using R3;
using UnityEngine;
using UnityEngine.Timeline;
using Zenject;

namespace Project.Shared.Scripts.ForMessages
{
    [DefaultExecutionOrder(-10)]
    public class MonoMessageReceive : BaseMonoMessage
    {
        [SerializeField] private MonoMessageHandler[] handlers;

        [Inject] private MessageBrokersService _messageBrokersService;
        
        private void Awake()
        {
            _messageBrokersService.Subscribe<SignalAsset>(Chanel, m =>
                {
                    if (handlers.FirstOrDefault(h => h.message.name == m.name)?.handler is {} handler)
                    {
                        Debug.Log($"{gameObject.name} Receive signal: {m.name}");
                        handler.Invoke();
                    }
                })
                .RegisterTo(destroyCancellationToken);
        }
    }
}