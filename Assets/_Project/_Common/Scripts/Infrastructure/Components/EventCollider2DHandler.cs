using System;
using SaintsField;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Shared.Scripts.ForInterractive
{
    [Flags]
    public enum ETypeEventHandler
    {
        Enter = 1,
        Stay = 2,
        Exit = 4
    }
    
    [Serializable]
    public class EventCollider2DHandler
    {
        [Tag] public string[] tags;
        public ETypeEventHandler typeEvent = ETypeEventHandler.Enter;
        public UnityEvent<Collider2D> handler;
    }
}