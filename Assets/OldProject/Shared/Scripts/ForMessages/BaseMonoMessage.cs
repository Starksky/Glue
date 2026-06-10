using SaintsField;
using UnityEngine;

namespace Project.Shared.Scripts.ForMessages
{
    public abstract class BaseMonoMessage : MonoBehaviour
    {
        [SerializeField] private MessageBrokerChannels configChannels;
        [SerializeField, Dropdown(nameof(GetChannels))] private string chanel;
        private string[] GetChannels() => configChannels?.Channels ?? new string[]{};
        protected string Chanel => chanel;
    }
}