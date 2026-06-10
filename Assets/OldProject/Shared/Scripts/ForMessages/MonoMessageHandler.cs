using System;
using UnityEngine.Events;
using UnityEngine.Timeline;

namespace Project.Shared.Scripts.ForMessages
{
    [Serializable]
    public class MonoMessageHandler
    {
        public SignalAsset message;
        public UnityEvent handler;
    }
}