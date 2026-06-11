using UnityEngine;

namespace _Project._Common.Scripts.Infrastructure.PoolObject
{
    public class MonoPoolable : MonoBehaviour
    {
        private GameObjectPool<MonoPoolable> _pool;
        private Vector3 _spawnedPosition;
        private Transform _spawnedParent;
        private bool _isFree;
        
        public void OnSpawned(GameObjectPool<MonoPoolable> pool)
        {
            _pool = pool;
            _spawnedPosition = transform.position;
            _spawnedParent = transform.parent;
        }
        
        public void Respawn()
        {
            if (!_isFree)
                return;
            
            _pool.Spawn(_spawnedPosition, Quaternion.identity, _spawnedParent);
        }
        public void Free()
        {
            _isFree = true;
            _pool.Free(this);
        }
    }
}