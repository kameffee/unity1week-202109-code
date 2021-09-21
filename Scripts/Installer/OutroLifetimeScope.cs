using System.Collections.Generic;
using kameffee.unity1week202109.Domain;
using kameffee.unity1week202109.Presenter;
using kameffee.unity1week202109.View;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace kameffee.unity1week202109.Installer
{
    public class OutroLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private OutroGround[] outroGrounds;

        [SerializeField]
        private Player player;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<PlayerModel>(Lifetime.Scoped);
            builder.RegisterInstance(player);
            builder.RegisterEntryPoint<OutroPlayerPresenter>();

            builder.RegisterInstance<IReadOnlyList<IOutroGround>>(outroGrounds);
            builder.RegisterEntryPoint<RecycleGroundPresenter>();

            builder.RegisterComponentInHierarchy<OutroView>().AsImplementedInterfaces();
            builder.Register<OutroSceneModel>(Lifetime.Scoped);
            builder.RegisterEntryPoint<OutroEntryPoint>();
        }
    }
}