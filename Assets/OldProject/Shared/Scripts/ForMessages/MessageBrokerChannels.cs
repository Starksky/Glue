using UnityEngine;

namespace Project.Shared.Scripts.ForMessages
{
    [CreateAssetMenu(fileName = "MessageBrokerChannels", menuName = "Create/MessageBrokerChannels", order = 0)]
    public class MessageBrokerChannels : ScriptableObject
    {
        [SerializeField] private string[] channels;
        public string[] Channels => channels;
    }
}