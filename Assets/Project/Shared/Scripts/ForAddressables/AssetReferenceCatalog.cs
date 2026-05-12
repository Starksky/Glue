using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Project.Shared.Scripts.ForAddressables
{
    [CreateAssetMenu(fileName = "AssetReferenceCatalog", menuName = "Game/Asset Reference Catalog")]
    public class AssetReferenceCatalog : ScriptableObject
    {
        [System.Serializable]
        public class AssetReferenceEntry
        {
            public string id;
            public bool isCachedGlobalStorage;
            public AssetReferenceGameObject prefabRef;
        }

        [SerializeField] private AssetReferenceEntry[] entries;
        
        private Dictionary<string, AssetReferenceEntry> _cache;
        
        public AssetReferenceEntry GetEntry(string id)
        {
            if (_cache == null) BuildCache();
            return _cache.GetValueOrDefault(id);
        }
        public string[] GetIDs() => entries.Select(e => e.id).ToArray();
        private void BuildCache()
        {
            _cache = new Dictionary<string, AssetReferenceEntry>();
            foreach (var entry in entries)
                if (!string.IsNullOrEmpty(entry.id) && entry.prefabRef != null)
                    _cache[entry.id] = entry;
        }
    }
}