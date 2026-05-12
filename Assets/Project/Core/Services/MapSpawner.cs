using Project.Core.Player.Scripts;
using UnityEngine;
using Zenject;

namespace Project.Core.Services
{
    public class MapSpawner : IInitializable
    {
        private readonly MapHandler.Factory _mapFactory;

        public MapSpawner(MapHandler.Factory mapFactory)
        {
            _mapFactory = mapFactory;
        }

        public void Initialize()
        {
            _mapFactory.Create();
        }
    }
}