using UnityEngine;
using UnityEngine.Timeline;
using Zenject;

namespace Project.Shared.Scripts.ForMessages
{
    public class MonoMessageSender : BaseMonoMessage
    {
        [Inject] private MessageBrokersService _messageBrokersService;
        public void SendMessage(SignalAsset signal) => _messageBrokersService.Publish(Chanel, signal);
    }
}