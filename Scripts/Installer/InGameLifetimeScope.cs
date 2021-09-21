using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using kameffee.unity1week202109.Domain;
using kameffee.unity1week202109.Entity;
using kameffee.unity1week202109.Enum;
using kameffee.unity1week202109.Presenter;
using kameffee.unity1week202109.UseCase;
using kameffee.unity1week202109.View;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace kameffee.unity1week202109.Installer
{
    public sealed class InGameLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private PlayerCamera playerFollowCamera;

        [SerializeField]
        private TimeZoneBundle timeZoneBundle;

        [SerializeField]
        private GoalPoint goalPoint;

        protected override void Configure(IContainerBuilder builder)
        {
            // 入力
            builder.Register<PlayerInput>(Lifetime.Scoped).AsImplementedInterfaces();

            // プレイヤー
            builder.Register<PlayerModel>(Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<Player>().AsSelf();
            builder.RegisterEntryPoint<PlayerPresenter>();

            // カメラ
            builder.RegisterInstance(playerFollowCamera).AsImplementedInterfaces();
            builder.RegisterEntryPoint<PlayerCameraPresenter>();

            // 回転カウント
            builder.RegisterComponentInHierarchy<SpinCountView>().AsImplementedInterfaces();
            builder.RegisterEntryPoint<SpinCountPresenter>();

            // ゲームオーバー
            // これだとうまくいかない
            // builder.RegisterComponentInHierarchy<GameOverView>().As<IGameOverView>().AsImplementedInterfaces();
            builder.RegisterComponent(FindObjectOfType<GameOverView>()).As<IGameOverView>();

            // スタートポイント
            builder.RegisterComponent(FindObjectOfType<StartPoint>()).AsImplementedInterfaces();

            // タイムゾーン
            builder.RegisterInstance<TimeZoneBundle>(timeZoneBundle);
            builder.RegisterComponent(FindObjectOfType<TimeZoneBackgroundView>()).As<ITimeZoneView>();
            builder.Register<TimeZoneModel>(Lifetime.Singleton).WithParameter("initialTimeZone", TimeZone.Night);
            builder.RegisterEntryPoint<TimeZonePresenter>();

            var list = FindObjectsOfType<TimeZoneChangePoint>().Cast<ITimeZoneChangePoint>().ToList();
            builder.Register<FieldTimeZonePointList>(Lifetime.Scoped)
                .WithParameter<IReadOnlyList<ITimeZoneChangePoint>>(list);

            builder.Register<BgmController>(Lifetime.Scoped);

            List<ITimeZoneSwitch> switchList =
                FindObjectsOfType<TimeZoneSwitchParticle>().Cast<ITimeZoneSwitch>().ToList();
            builder.RegisterInstance<IReadOnlyList<ITimeZoneSwitch>>(switchList);
            builder.RegisterEntryPoint<TimeZoneSwitchPresenter>();

            // ゴール
            builder.RegisterComponent(goalPoint).As<IGoalPoint>();

            // カウントダウン
            builder.Register<CountDownModel>(Lifetime.Scoped).AsSelf();
            builder.RegisterComponentInHierarchy<CountDownView>().AsImplementedInterfaces();
            builder.RegisterEntryPoint<CountDownPresenter>();

            // 経過時間
            builder.Register<TimeCountModel>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<TimeCountView>().AsImplementedInterfaces();
            builder.RegisterEntryPoint<TimeCountPresenter>();

            // クリア画面
            builder.Register<GameClearModel>(Lifetime.Scoped).AsSelf();
            builder.RegisterComponentInHierarchy<GameClearView>().AsImplementedInterfaces();
            builder.RegisterEntryPoint<GameClearPresenter>();

            // ゲームループ関連
            builder.Register<RetryUseCase>(Lifetime.Scoped).AsSelf();
            builder.RegisterEntryPoint<GameCycle>();
        }
    }
}