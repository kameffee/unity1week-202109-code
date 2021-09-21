using kameffee.unity1week202109.Domain;
using kameffee.unity1week202109.Entity;
using kameffee.unity1week202109.Presenter;
using kameffee.unity1week202109.UseCase;
using kameffee.unity1week202109.View;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace kameffee.unity1week202109.Installer
{
    public sealed class RootLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private SimpleFadeView fadeViewPrefab;

        [Header("Sound")]
        [SerializeField]
        private BgmPreset bgmPreset;

        [SerializeField]
        private SePreset sePreset;

        protected override void Configure(IContainerBuilder builder)
        {
            // フェード
            builder.RegisterComponentInNewPrefab(fadeViewPrefab, Lifetime.Singleton).As<IFadeView>();
            builder.Register<FadeModel>(Lifetime.Singleton).AsSelf();
            builder.RegisterEntryPoint<FadePresenter>();

            // サウンド
            builder.Register<SoundSettingsModel>(Lifetime.Singleton);

            // BGM
            builder.RegisterInstance<BgmPreset>(bgmPreset);
            builder.Register<BgmModel>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.RegisterComponentOnNewGameObject<BgmPlayer>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<BgmController>(Lifetime.Singleton);

            // SE
            builder.RegisterInstance<SePreset>(sePreset);
            builder.Register<SeModel>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponentOnNewGameObject<SePlayer>(Lifetime.Transient).As<ISePlayer>();
            builder.RegisterFactory<ISePlayer>(container => container.Resolve<ISePlayer>, Lifetime.Scoped)
                .AsImplementedInterfaces();
            builder.Register<SeController>(Lifetime.Singleton);
        }
    }
}