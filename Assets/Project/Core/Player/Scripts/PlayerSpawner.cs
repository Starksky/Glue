using System;
using JetBrains.Annotations;
using Project.Shared.Scripts.ForAddressables;
using Project.Shared.Scripts.ForPool;
using R3;
using SaintsField;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Project.Core.Player.Scripts
{
    public class MonoPlayerSpawner : MonoBehaviour
    {
        [SerializeField] private AssetReferenceCatalog catalog;
        [SerializeField, Dropdown(nameof(GetIDs))] private string refPrefab;
        [SerializeField] private float delayRespawn;
        [SerializeField] private UnityEvent EventSpawned;
        public string[] GetIDs() => catalog ? catalog.GetIDs() : new string[]{};
        
        [Inject] private DynamicPoolsService _poolsService;
        private SlingshotHandler.Pool _pool;
        private RestartableTimer _restartableTimer = new RestartableTimer();
        
        private async void Awake()
        {
            _restartableTimer.OnCompleted.Subscribe(_ => Spawn()).RegisterTo(destroyCancellationToken);
            _pool = await _poolsService.GetPoolAsync<SlingshotHandler, SlingshotHandler.Pool>(catalog.GetEntry(refPrefab));
            Spawn();
        }
        
        private void Spawn() 
        {
            Debug.Log("Spawn");
            _pool.Spawn(transform.position, _pool);
            EventSpawned.Invoke();
        }
        
        [UsedImplicitly]
        public void Respawn() {
            _restartableTimer.Start(delayRespawn);
        }
    }
}