using UnityEngine;

namespace _Project.Scripts.Contracts.Interfaces
{
    public interface ICameraView
    {
        public void SetFreeScreenPosition(Vector3? position);
        public Vector2 ScreenToWorldPoint(Vector2 screenPos);
    }
}