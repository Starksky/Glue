using System;
using R3;
using UnityEngine;
using Zenject;

namespace Project.Core.Services
{
    public class GameService : IInitializable, IDisposable
    {
        private ReactiveProperty<Transform> _playerTransform = new ReactiveProperty<Transform>();
        public ReadOnlyReactiveProperty<Transform> PlayerTransform => _playerTransform;
        private ReactiveProperty<Grid> _currentMap = new ReactiveProperty<Grid>();
        public ReadOnlyReactiveProperty<Grid> CurrentMap => _currentMap;
        
        public void Initialize()
        {
            
        }
        
        public void Dispose()
        {
            _playerTransform.Dispose();
            _currentMap.Dispose();
        }

        public void SetPlayer(Transform t) => _playerTransform.Value = t;
        public void SetMap(Grid g) => _currentMap.Value = g;
    }
}