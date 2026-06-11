using _Project.Features.UI.MainMenu.Scripts.Contracts;
using _Project.Features.UI.MainMenu.Scripts.Presentation;
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