using UnityEngine;

namespace _Project.Scripts.Infrastructure.PoolObject
{
    public interface IPoolable
    {
        public void Show();
        public void Hide();
        
        public void OnSpawn(Vector3 position, Quaternion rotation, Transform parent);
        
        public void OnDespawn();
    }
}