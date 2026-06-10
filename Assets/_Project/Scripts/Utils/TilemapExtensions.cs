using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Scripts.Utils
{
    public static class TilemapExtensions
    {
        public static Bounds GetWorldBounds(this Tilemap tilemap)
        {
            tilemap.CompressBounds();
            Bounds localBounds = tilemap.localBounds;
            Vector3 worldCenter = tilemap.transform.TransformPoint(localBounds.center);
            Vector3 worldSize = Vector3.Scale(localBounds.size, tilemap.transform.lossyScale);
        
            return new Bounds(worldCenter, worldSize);
        }
    }
}