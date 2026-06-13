using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project._Common.Scripts.Infrastructure.Repositories
{
    [CreateAssetMenu(fileName = "AssetReferenceCatalog", menuName = "Game/Asset Reference Catalog")]
    public class AssetReferenceCatalog : ScriptableObject, IReadOnlyList<AssetReferenceGameObject>
    {
        [SerializeField] private AssetReferenceGameObject[] entries;
        
        private Dictionary<string, AssetReferenceGameObject> _cache;
        
        private AssetReferenceGameObject GetEntry(string id)
        {
            if (_cache == null)
                BuildCache();
            return _cache.GetValueOrDefault(id);
        }
        
        public bool TryGetValue(string id, out AssetReferenceGameObject value)
        {
            value = null;
            if (_cache == null) 
                BuildCache();
            return _cache?.TryGetValue(id, out value) ?? false;
        }
        
        public string[] GetNames() 
        {
            if (_cache == null) 
                BuildCache();
            return _cache?.Keys.ToArray();
        }
        private void BuildCache()
        {
            _cache = new Dictionary<string, AssetReferenceGameObject>();
            foreach (var entry in entries)
                if (entry != null)
                    _cache[entry.editorAsset.name] = entry;
        }
        public IEnumerator<AssetReferenceGameObject> GetEnumerator()
            => (IEnumerator<AssetReferenceGameObject>) entries.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
        public int Count => entries.Length;
        public AssetReferenceGameObject this[int index] => entries[index];
        public AssetReferenceGameObject this[string index] => GetEntry(index);
    }
}