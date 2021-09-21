using kameffee.unity1week202109.Domain;
using kameffee.unity1week202109.Presenter;
using kameffee.unity1week202109.View;
using VContainer;
using VContainer.Unity;

namespace kameffee.unity1week202109.Installer
{
    public class IntroLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent<Playable>(FindObjectOfType<Playable>()).AsImplementedInterfaces();
            builder.Register<IntroModel>(Lifetime.Scoped);
            builder.RegisterEntryPoint<IntroPresenter>();
        }
    }
}