using kameffee.unity1week202109.Domain;
using kameffee.unity1week202109.Presenter;
using kameffee.unity1week202109.View;
using VContainer;
using VContainer.Unity;

namespace kameffee.unity1week202109.Installer
{
    public class EndingLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Playable
            builder.RegisterComponentInHierarchy<Playable>().AsImplementedInterfaces();

            builder.Register<EndingSceneModel>(Lifetime.Scoped);
            builder.RegisterEntryPoint<EndingPresenter>();
        }
    }
}