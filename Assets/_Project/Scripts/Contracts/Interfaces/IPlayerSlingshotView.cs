using UnityEngine;

namespace _Project.Scripts.Contracts.Interfaces
{
    public interface IPlayerSlingshotView
    {
        public Vector2 Position { get; }
        public Vector2 DeltaDrag { get; }
        public float StrengthDrag { get; }
        
        public void OnBeginDrag();
        public void OnStayDrag(Vector2 positionMouse);
        public void OnEndDrag();
    }
}