using UnityEngine;

namespace _Project._Common.Scripts.Contracts.Interfaces
{
    public interface IPlayerBodyView
    {
        public Transform Transform { get; }
        public Vector2? ContactClosestPoint { get; }
        public bool HasContactWithSurface { get; }
    }
}