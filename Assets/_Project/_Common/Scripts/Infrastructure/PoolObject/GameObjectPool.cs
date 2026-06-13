using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace _Project._Common.Scripts.Infrastructure.PoolObject
{
    public static class GameObjectPoolExtensions
    {
        public static RegistrationBuilder RegisterGameObjectPool(this IContainerBuilder builder, LifetimeScope lifetimeScope, Lifetime lifetime)
            => builder.RegisterFactory<MonoPoolable, int, GameObjectPool<MonoPoolable>>(resolver =>
                (prefab, initCount) 
                    => new GameObjectPool<MonoPoolable>(prefab, initCount, lifetimeScope), 
                lifetime);
    }
    
    public class GameObjectPool<T> : IDisposable where T : MonoBehaviour
    {
        private readonly Stack<T> _pool = new();
        private readonly T _prefab;
        private readonly LifetimeScope _lifetimeScope;
        private readonly List<T> _instances = new();

        public GameObjectPool(T prefab, int initialCount, LifetimeScope lifetimeScope)
        {
            _prefab = prefab;
            _lifetimeScope = lifetimeScope;

            for (int i = 0; i < initialCount; i++)
            {
                var instance = Create();
                instance.gameObject.SetActive(false);
                _pool.Push(instance);
            }
        }
        
        public T Spawn(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var instance = _pool.Count > 0 ? _pool.Pop() : Create();
            
            instance.transform.SetParent(parent);
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            
            instance.gameObject.SetActive(true);
            return instance;
        }

        public void Free(T instance)
        {
            instance.gameObject.SetActive(false);
            _pool.Push(instance);
        }

        private T Create()
        {
            T result;
            
            if (!_prefab.TryGetComponent<LifetimeScope>(out var scope))
                result = _lifetimeScope.Container.Instantiate(_prefab);
            else
            {
                var childScope = _lifetimeScope.CreateChildFromPrefab(scope);
                result = childScope.GetComponent<T>();
            }
            
            _instances.Add(result);
            return result;
        }
        
        public void Dispose()
        {
            _instances.ForEach(instance =>
            {
                if (instance != null)
                    Object.Destroy(instance.gameObject);
            });
            _instances.Clear();
            _pool.Clear();
        }
    }
}