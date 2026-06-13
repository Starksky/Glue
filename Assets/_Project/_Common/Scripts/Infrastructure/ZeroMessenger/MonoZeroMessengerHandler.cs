using System;
using UnityEngine.Events;
using UnityEngine.Timeline;

namespace _Project._Common.Scripts.Infrastructure.ZeroMessenger
{
    [Serializable]
    public class MonoZeroMessengerHandler
    {
        public SignalAsset message;
        public UnityEvent handler;
    }
}