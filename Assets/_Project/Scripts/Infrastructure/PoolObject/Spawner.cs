using System;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.Infrastructure.PoolObject
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private MonoPoolable prefab;
        
        [SerializeField] private int initialCount;
        [SerializeField] private bool spawnInParent;
        [SerializeField] private bool spawnByEnable;
        
        private GameObjectPool<MonoPoolable> _pool;
        
        [Inject]
        public void Construct(Func<MonoPoolable, int, GameObjectPool<MonoPoolable>> poolFactory)
        {
            _pool = poolFactory(prefab, initialCount);
        }

        private void OnEnable()
        {
            if (spawnByEnable)
                Spawn();
        }

        public void Spawn()
        {
            var spawned = _pool.Spawn(transform.position, transform.rotation, spawnInParent ? transform : null);
            spawned.OnSpawned(_pool);
        }

        private void OnDestroy()
        {
            _pool.Dispose();
        }
    }
}