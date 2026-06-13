using System;
using System.Linq;
using _Project._Common.Scripts.Contracts.Interfaces;
using _Project._Common.Scripts.Infrastructure.Repositories;
using _Project._Common.Scripts.Infrastructure.ZeroMessenger;
using _Project._Common.Scripts.Signals;
using R3;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;
using VContainer.Unity;

namespace _Project.Features.Map.Scripts.Infrastructure
{
    public class MapSpawner : MonoBehaviour
    {
        [SerializeField] AssetReferenceCatalog catalog;
        
        private LifetimeScope _lifetimeScope;
        private ILoaderScreen _loaderScreen;
        private AsyncOperationHandle<GameObject> _handleMap;
        private string[] _mapNames;
        private int _currentIndexMap;
        private GameObject _currentMap;
        private ZeroMessengerService _zeroMessengerService;


        [Inject]
        public void Construct(
            LifetimeScope lifetimeScope, 
            ILoaderScreen loaderScreen,
            ZeroMessengerService zeroMessengerService)
        {
            _zeroMessengerService = zeroMessengerService;
            _lifetimeScope = lifetimeScope;
            _loaderScreen = loaderScreen;
        }

        private void Awake()
        {
            _zeroMessengerService.Subscribe<MapCompleteSignal>(_ => SpawnNextMap())
                .RegisterTo(destroyCancellationToken);
        }
        
        private void Start()
        {
            _mapNames = catalog.GetNames();
            
            //for test
            _zeroMessengerService.Publish(new MapCompleteSignal());
            //SpawnNextMap();
        }
        
        private void InstantiateMap(GameObject prefab)
        {
            if (prefab.TryGetComponent<LifetimeScope>(out var childLifetimeScope))
            {
                var scope = _lifetimeScope.CreateChildFromPrefab(childLifetimeScope);
                scope.transform.SetParent(transform);
                _currentMap = scope.gameObject;
            }
            else 
                _currentMap = _lifetimeScope.Container.Instantiate(prefab, parent: transform);
        }

        private async void SpawnNextMap()
        {
            try
            {
                _loaderScreen.FadeIn();
                
                Destroy(_currentMap);
                
                if (_handleMap.IsValid()) 
                    Addressables.Release(_handleMap);
                
                var mapName = _mapNames[_currentIndexMap];
                if (catalog.TryGetValue(mapName, out var assetReferenceGameObject))
                {
                    _handleMap = assetReferenceGameObject.LoadAssetAsync();
                    var prefab = await _handleMap.Task;
                    InstantiateMap(prefab);
                }
                else Debug.LogError($"Map {mapName} not found");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                _currentIndexMap++;
                _currentIndexMap = _currentIndexMap > _mapNames.Length - 1 ? 0 : _currentIndexMap;
                _loaderScreen.FadeOut();
            }
        }
        
        private void OnDestroy()
        {
            if (_handleMap.IsValid()) 
                Addressables.Release(_handleMap);
        }
    }
}
