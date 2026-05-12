using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Shared.Scripts.ForAddressables;
using UnityEngine;
using Zenject;

namespace Project.Shared.Scripts.ForPool
{
    public class DynamicPoolsService : IInitializable, IDisposable
    {
        private DiContainer _container;
        private AddressablePrefabsService _addressablePrefabsService;
        
        private Dictionary<string, IMemoryPool> _pools = new();
        
        public DynamicPoolsService(DiContainer container, AddressablePrefabsService addressablePrefabsService)
        {
            _container = container;
            _addressablePrefabsService = addressablePrefabsService;
        }

        public void Initialize()
        {
            
        }

        public async UniTask<T> GetPoolAsync<TComponent, T>(AssetReferenceCatalog.AssetReferenceEntry entry) where T : class, IMemoryPool
        {
            if (_pools.TryGetValue(entry.id, out var existingPool))
                return existingPool as T;

            var prefab = await _addressablePrefabsService.GetPrefab(entry);
            if (prefab == null)
                throw new Exception($"Not found prefab \"{entry.id}\"");

            var pool = CreatePool<TComponent, T>(prefab);
            _pools[entry.id] = pool;
            return pool;
        }
        
        private T CreatePool<TComponent, T>(GameObject prefab) where T : class, IMemoryPool
        {
            _container.BindMemoryPool<TComponent, T>()
                .ExpandByOneAtATime()
                .FromComponentInNewPrefab(prefab);
            return _container.Resolve<T>();
        }
        
        public void Dispose()
        {
            _pools.Clear();
        }
    }
}