using System.Collections.Generic;
using kameffee.unity1week202109.Domain;
using kameffee.unity1week202109.Presenter;
using kameffee.unity1week202109.UseCase;
using kameffee.unity1week202109.View;
using VContainer;
using VContainer.Unity;

namespace kameffee.unity1week202109.Installer
{
    public sealed class TitleLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<TitleView>().AsImplementedInterfaces();
            builder.Register<TitleModel>(Lifetime.Scoped);
            builder.RegisterEntryPoint<TitleScenePresenter>();

            // 音量設定
            builder.RegisterComponentInHierarchy<SoundSettingsView>().AsImplementedInterfaces();
            builder.RegisterEntryPoint<SoundSettingsPresenter>();

            // 背景スクロール
            var scrollBackgrounds = FindObjectsOfType<ScrollBackground>();
            builder.RegisterInstance<IReadOnlyList<IScrollBackground>>(scrollBackgrounds);
            builder.RegisterEntryPoint<TitleBackgroundScrollPresenter>();

            builder.Register<BgmController>(Lifetime.Scoped).AsSelf();
        }
    }
}