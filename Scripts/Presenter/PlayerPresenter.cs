using System;
using kameffee.unity1week202109.Domain;
using kameffee.unity1week202109.View;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace kameffee.unity1week202109.Presenter
{
    public sealed class PlayerPresenter : IInitializable, IDisposable
    {
        private CharacterSettings characterSettings;

        private readonly PlayerModel playerModel;
        private readonly Player view;
        private readonly IPlayerInput playerInput;
        private readonly CompositeDisposable disposable = new CompositeDisposable();

        public PlayerPresenter(PlayerModel playerModel, Player view, IPlayerInput playerInput)
        {
            this.playerModel = playerModel;
            this.view = view;
            this.playerInput = playerInput;
        }

        public void Initialize()
        {
            characterSettings = view.CharacterSettings;

            playerModel.Initialize(characterSettings);
            view.SetJumpPower(characterSettings.JumpPower);
            view.SetRotateAngle(characterSettings.RotateSpeed);

            // 初期化
            playerModel.SetMoveSpeed(view.MoveSpeed);

            // ジャンプ
            playerInput.OnDown
                .Where(_ => playerModel.IsActive.Value)
                .Subscribe(_ => playerModel.Jump())
                .AddTo(disposable);

            // 空中回転
            playerInput.OnHold
                .Where(_ => playerModel.IsActive.Value)
                .Subscribe(_ => playerModel.Spin())
                .AddTo(disposable);

            playerInput.OnUp
                .Where(_ => playerModel.IsActive.Value)
                .Subscribe(_ => view.SpinStop())
                .AddTo(disposable);

            // 復活時
            playerModel.OnRespawn
                .Subscribe(position =>
                {
                    playerModel.Initialize(characterSettings);
                    view.Initialize(position);
                })
                .AddTo(disposable);

            // 着地
            playerModel.IsGround
                .Where(isGround => isGround)
                .Subscribe(_ => view.OnTouchGround())
                .AddTo(disposable);

            // スピード変化
            playerModel.BoostSpeed
                .Subscribe(_ =>
                {
                    Debug.Log($"speed:{playerModel.MoveSpeed.Value} boost:{playerModel.BoostSpeed.Value}");
                    view.SetMoveSpeed(playerModel.MoveSpeed.Value + playerModel.BoostSpeed.Value);
                })
                .AddTo(disposable);

            // 宙返り成功
            playerModel.OnSuccessSpin.Subscribe(count => view.OnSpeedUp(count)).AddTo(disposable);
            // ジャンプ
            playerModel.OnJump.Subscribe(_ => view.Jump()).AddTo(disposable);
            // 回転
            playerModel.OnSpin.Subscribe(_ => view.Spin()).AddTo(disposable);
            // 自動移動
            playerModel.IsActive.Subscribe(autoMove => view.SetAutoMove(autoMove)).AddTo(disposable);

            playerModel.OnStan.Subscribe(_ => view.SetStanState(true)).AddTo(disposable);

            view.Speed.Subscribe(speed => playerModel.UpdateSpeed(speed)).AddTo(disposable);
            view.OnJump.Subscribe(_ => playerModel.Jump()).AddTo(disposable);
            view.IsGround.Subscribe(isGround => playerModel.SetIsGround(isGround)).AddTo(disposable);
            view.OnSuccessSpin.Subscribe(count => playerModel.SuccessSpin(count)).AddTo(disposable);
            view.OnStan.Subscribe(_ => playerModel.Stan()).AddTo(disposable);
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }
    }
}