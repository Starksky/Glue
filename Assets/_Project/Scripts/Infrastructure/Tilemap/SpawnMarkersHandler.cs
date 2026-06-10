using _Project.Scripts.Infrastructure.PoolObject;
using SaintsField;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.Tilemap
{
    public class SpawnMarkersHandler : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private Grid grid;
        private LifetimeScope _lifetimeScope;

        [Inject]
        public void Construct(LifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }
        
        private void OnValidate()
        {
            if (grid == null)
                grid = GetComponentInParent<Grid>();
        }

        private void Reset()
        {
            if (grid == null)
                grid = GetComponentInParent<Grid>();
        }

        private void Awake()
        {
            var tilemaps = grid.GetComponentsInChildren<UnityEngine.Tilemaps.Tilemap>();

            foreach (var tilemap in tilemaps)
            {
                BoundsInt bounds = tilemap.cellBounds;
                for (int x = bounds.xMin; x < bounds.xMax; x++)
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int cellPosition = new Vector3Int(x, y, 0);
                    
                    if (tilemap.GetTile(cellPosition) is not SpawnMarker spawnMarker)
                        continue;
                    
                    SpawnMarker(tilemap.CellToWorld(cellPosition) + Vector3.one * 0.5f, 
                        spawnMarker,
                        tilemap.transform);
                }
            }
        }

        private void SpawnMarker(Vector3 position, SpawnMarker marker, Transform parent)
        {
            var prefab = marker.reference;
            _lifetimeScope.Container.Instantiate(prefab, position, Quaternion.identity, parent);
        }
    }
}