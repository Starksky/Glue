using UnityEngine;

namespace _Project.Features.Player.Scripts.Contracts
{
    public interface IPlayerSlingshotPresenter
    {
        public Vector2 Position { get; }
        public Vector2 DeltaDrag { get; }
        public float StrengthDrag { get; }
        
        public void BeginDrag();
        public void StayDrag(Vector2 positionMouse, Vector2 contactClosestPoint);
        public void EndDrag(Vector2 contactClosestPoint);
    }
}