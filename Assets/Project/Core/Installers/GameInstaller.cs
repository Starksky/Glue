using Project.Shared.Scripts.ForMessages;
using Zenject;

namespace Project.Core.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MessageBrokersService>().AsSingle();
        }
    }
}