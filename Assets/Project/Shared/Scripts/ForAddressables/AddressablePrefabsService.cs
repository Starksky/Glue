using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Project.Shared.Scripts.ForAddressables
{
    public class AddressablePrefabsService : IInitializable, IDisposable
    {
        private Dictionary<string, GameObject> _cache = new();
        private List<AsyncOperationHandle> _handles = new ();

        public void Initialize()
        {
            
        }

        public async UniTask<GameObject> GetPrefab(AssetReferenceCatalog.AssetReferenceEntry entry)
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
        
        public void Dispose()
        {
            foreach (var handle in _handles)
                if (handle.IsValid()) 
                    Addressables.Release(handle);
        
            _handles.Clear();
            _cache.Clear();
        }
    }
}