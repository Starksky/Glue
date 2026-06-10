using _Project.Scripts.Contracts.Interfaces;
using _Project.Scripts.Presentation.UI;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.Scopes
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