using Project.Core.Player.Scripts;
using Project.Core.Services;
using Project.Shared.Scripts.ForAddressables;
using Project.Shared.Scripts.ForMessages;
using Project.Shared.Scripts.ForPool;
using UnityEngine;
using Zenject;

namespace Project.Core.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private SlingshotHandler prefabPlayer;
        [SerializeField] private MapHandler prefabMap;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AddressablePrefabsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<DynamicPoolsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MessageBrokersService>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MapSpawner>().AsSingle();

            Container.BindFactory<MapHandler, MapHandler.Factory>()
                .FromComponentInNewPrefab(prefabMap);
        }
    }
}