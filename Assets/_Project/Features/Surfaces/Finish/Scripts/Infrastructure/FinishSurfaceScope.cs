using _Project.Features.Surfaces.Finish.Scripts.Contracts;
using _Project.Features.Surfaces.Finish.Scripts.Presentation;
using VContainer;
using VContainer.Unity;

namespace _Project.Features.Surfaces.Finish.Scripts.Infrastructure
{
    public class FinishSurfaceScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<FinishSurfacePresenter>(Lifetime.Scoped)
				.As<IFinishSurfacePresenter>();
        }
    }
}