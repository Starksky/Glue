using UnityEngine;
using UnityEngine.Tilemaps;

namespace Project.Shared.Scripts.Extensions
{
    public static class GridExtensions
    {
        public static Bounds GetBounds(this Grid grid)
        {
            var tilemaps = grid.GetComponentsInChildren<Tilemap>();
            Bounds combinedBounds = new Bounds(Vector3.zero, Vector3.zero);
            foreach (var tilemap in tilemaps)
                combinedBounds.Encapsulate(tilemap.GetWorldBounds());
            return combinedBounds;
        }
    }
}