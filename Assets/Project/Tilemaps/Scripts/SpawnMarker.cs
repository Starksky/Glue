using System;
using Project.Shared.Scripts.ForAddressables;
using SaintsField;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Project.Tilemaps.Scripts
{
    [CreateAssetMenu(fileName = "SpawnMarker", menuName = "Tiles/SpawnMarker")]
    public class SpawnMarker : TileBase
    {
        public Sprite previewSprite;
        public Color color;
        public AssetReferenceCatalog catalog;
        [AdvancedDropdown(nameof(GetIDs))] public string refAsset;
        public string[] GetIDs() => catalog ? catalog.GetIDs() : new string[]{};
        
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = previewSprite;
            tileData.color = color;
            tileData.transform = Matrix4x4.identity;
            tileData.gameObjectEntityId = EntityId.None;
            tileData.flags = TileFlags.LockAll;
            tileData.colliderType = Tile.ColliderType.None;
        }
        
        public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
        {
            return false;
        }
    }
}