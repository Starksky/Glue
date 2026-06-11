using _Project.Scripts.Contracts.Interfaces;
using _Project.Scripts.Presentation.UI;
using VContainer;
using VContainer.Unity;

namespace _Project.Features.UI.MainMenu.Scripts.Infrastructure
{
    public class MenuScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<MainMenuPresenter>(Lifetime.Scoped)
                .As<IMainMenuPresenter>();
        }
    }
}