using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Shared.Scripts.ForAddressables;
using SaintsField;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Tilemaps;
using Zenject;

namespace Project.Tilemaps.Scripts
{
    [RequireComponent(typeof(Grid))]
    public class SpawnMarkersHandler : MonoBehaviour
    {
        [SerializeField, ReadOnly, GetComponent(typeof(Grid))]
        private Grid grid;

        [Inject] private DiContainer _container;
        [Inject] private AddressablePrefabsService _addressablePrefabsService;

        private Dictionary<string, GameObject> _cache = new();
        private List<AsyncOperationHandle> _handles = new ();

        private void Awake()
        {
            var tilemaps = grid.GetComponentsInChildren<Tilemap>();

            foreach (var tilemap in tilemaps)
            {
                BoundsInt bounds = tilemap.cellBounds;
                for (int x = bounds.xMin; x < bounds.xMax; x++)
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int cellPosition = new Vector3Int(x, y, 0);
                    
                    if (tilemap.GetTile(cellPosition) is not SpawnMarker spawnMarker)
                        continue;
                    
                    SpawnMarker(tilemap.CellToWorld(cellPosition) + Vector3.one * 0.5f, spawnMarker);
                }
            }
        }

        private async void SpawnMarker(Vector3 position, SpawnMarker marker)
        {
            if (marker.catalog.GetEntry(marker.refAsset) is not {} entry)
                return;

            var prefab = await (entry.isCachedGlobalStorage ? 
                _addressablePrefabsService.GetPrefab(entry) : 
                GetLocalPrefab(entry));
            
            _container.InstantiatePrefab(prefab, position, Quaternion.identity, transform);
        }

        public async UniTask<GameObject> GetLocalPrefab(AssetReferenceCatalog.AssetReferenceEntry entry)
        {
            if (!_cache.TryGetValue(entry.id, out var prefab))
            {
                var handler = entry.prefabRef.LoadAssetAsync();
                await handler.Task;
                prefab = handler.Result;
                _cache[entry.id] = prefab;
                _handles.Add(handler);
            }

            return prefab;
        }
        
        private void OnDestroy()
        {
            foreach (var handle in _handles)
                if (handle.IsValid()) 
                    Addressables.Release(handle);
        
            _handles.Clear();
            _cache.Clear();
        }
    }
}