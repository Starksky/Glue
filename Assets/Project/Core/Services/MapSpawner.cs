using System;
using Cysharp.Threading.Tasks;
using Project.Shared.Scripts.ForAddressables;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;
using Object = UnityEngine.Object;

namespace Project.Core.Services
{
    public class MapSpawner : IInitializable, IDisposable
    {
        private DiContainer _container;
        private AssetReferenceCatalog _maps;
        private int _currentID = -1;
        private AsyncOperationHandle<GameObject> _currentHandler;
        private GameObject _currentMap;
        
        public MapSpawner(DiContainer container, AssetReferenceCatalog maps)
        {
            _container = container;
            _maps = maps;
        }

        public void Initialize()
        {
            //LoadNextMap().Forget();
        }

        public async UniTask LoadNextLevel()
        {
            var ids = _maps.GetIDs();
            int next = _currentID + 1 >= ids.Length ? 0 : _currentID + 1;
            var nextId = ids[next];
            var entry = _maps.GetEntry(nextId);
            
            if (_currentMap)
                Object.Destroy(_currentMap);
            if (_currentHandler.IsValid())
                Addressables.Release(_currentHandler);
            
            _currentHandler = entry.prefabRef.LoadAssetAsync();
            
            var prefab = await _currentHandler.Task;
            if (prefab == null)
                throw new Exception($"Not found prefab \"{entry.id}\"");
            
            _currentID = next;
            _currentMap = _container.InstantiatePrefab(prefab);
        }
        
        public void Dispose()
        {
            if (_currentHandler.IsValid())
                Addressables.Release(_currentHandler);
        }
    }
}