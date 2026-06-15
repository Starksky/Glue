using System.Linq;
using Project.Shared.Scripts.ForInterractive;
using UnityEngine;

public enum ETypeEventCollider
{
    trigger,
    collision
}

public class EventsCollider2D : MonoBehaviour
{
    [SerializeField] private ETypeEventCollider typeEventCollider;
    [SerializeField] private EventCollider2DHandler[] handlers;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (typeEventCollider != ETypeEventCollider.trigger)
            return;
        
        if (handlers.FirstOrDefault(h => h.typeEvent.HasFlag(ETypeEventHandler.Enter)
                                        && h.tags.Contains(other.tag)) is {} eventHandler)
            eventHandler.handler?.Invoke(other);
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (typeEventCollider != ETypeEventCollider.trigger)
            return;
        
        if (handlers.FirstOrDefault(h => h.typeEvent.HasFlag(ETypeEventHandler.Stay)
                                         && h.tags.Contains(other.tag)) is {} eventHandler)
            eventHandler.handler?.Invoke(other);
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (typeEventCollider != ETypeEventCollider.trigger)
            return;
        
        if (handlers.FirstOrDefault(h => h.typeEvent.HasFlag(ETypeEventHandler.Exit)
                                         && h.tags.Contains(other.tag)) is {} eventHandler)
            eventHandler.handler?.Invoke(other);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (typeEventCollider != ETypeEventCollider.collision)
            return;
        
        if (handlers.FirstOrDefault(h => h.typeEvent.HasFlag(ETypeEventHandler.Enter)
                                         && h.tags.Contains(other.transform.root.tag)) is {} eventHandler)
            eventHandler.handler?.Invoke(other.collider);
    }
    
    private void OnCollisionStay2D(Collision2D other)
    {
        if (typeEventCollider != ETypeEventCollider.collision)
            return;
        
        if (handlers.FirstOrDefault(h => h.typeEvent.HasFlag(ETypeEventHandler.Stay)
                                         && h.tags.Contains(other.transform.root.tag)) is {} eventHandler)
            eventHandler.handler?.Invoke(other.collider);
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (typeEventCollider != ETypeEventCollider.collision)
            return;
        
        if (handlers.FirstOrDefault(h => h.typeEvent.HasFlag(ETypeEventHandler.Exit)
                                         && h.tags.Contains(other.transform.root.tag)) is {} eventHandler)
            eventHandler.handler?.Invoke(other.collider);
    }
}
