using System;
using kameffee.unity1week202109.UseCase;
using kameffee.unity1week202109.View;
using UniRx;
using UnityEditor;
using UnityEngine;
using VContainer;

namespace kameffee.unity1week202109.Domain
{
    public sealed class PlayerModel : IDisposable
    {
        // 操作可能状態
        public IReadOnlyReactiveProperty<bool> IsActive => isActive;
        private readonly ReactiveProperty<bool> isActive = new ReactiveProperty<bool>();

        // 速度
        public IReadOnlyReactiveProperty<Vector2> Velocity => velocity;
        private readonly ReactiveProperty<Vector2> velocity = new ReactiveProperty<Vector2>();

        // 移動速度
        public IReadOnlyReactiveProperty<float> MoveSpeed => moveSpeed;
        private readonly ReactiveProperty<float> moveSpeed = new ReactiveProperty<float>();

        // 追加されてる速度
        public IReadOnlyReactiveProperty<float> BoostSpeed => boostSpeed;
        private readonly ReactiveProperty<float> boostSpeed = new ReactiveProperty<float>();

        // ジャンプした時
        public IObservable<Unit> OnJump => onJump;
        private readonly Subject<Unit> onJump = new Subject<Unit>();

        // 回転中
        public IObservable<Unit> OnSpin => onSpin;
        private readonly Subject<Unit> onSpin = new Subject<Unit>();

        // 地面に着いているか
        public IReadOnlyReactiveProperty<bool> IsGround => isGround;
        private readonly ReactiveProperty<bool> isGround = new ReactiveProperty<bool>();

        // 回転成功
        public IObservable<int> OnSuccessSpin => onSuccessSpin.Where(count => count > 0);
        private readonly Subject<int> onSuccessSpin = new Subject<int>();

        // 転んだ
        public IObservable<Unit> OnStan => onStan;
        private readonly Subject<Unit> onStan = new Subject<Unit>();

        // 復活
        public IObservable<Vector2> OnRespawn => onRespawn;
        private readonly Subject<Vector2> onRespawn = new Subject<Vector2>();

        [Inject]
        private SeController seController;

        private CharacterSettings characterSettings;
        private readonly CompositeDisposable disposable = new CompositeDisposable();

        public PlayerModel()
        {
        }

        public void Initialize(CharacterSettings settings)
        {
            this.characterSettings = settings;

            boostSpeed.Value = 0;
            moveSpeed.Value = settings.MoveSpeed;
            isGround.Value = false;
            isActive.Value = false;

            disposable.Clear();

            Observable.EveryUpdate()
                .Where(_ => isActive.Value)
                .Where(_ => boostSpeed.Value > 0)
                .Subscribe(_ =>
                {
                    var to = boostSpeed.Value - (0.5f * Time.deltaTime);
                    // 0を下回らないようにする.
                    boostSpeed.Value = Mathf.Clamp(to, 0, characterSettings.MaxBootSpeed);
                })
                .AddTo(disposable);

            // 回転後着地に成功
            OnSuccessSpin
                .Subscribe(spinCount =>
                {
                    float addSpeed = 4 + spinCount * 2f;
                    boostSpeed.Value += addSpeed;
                    Debug.Log($"Spin: {spinCount} AddSpeed : {addSpeed}");
                })
                .AddTo(disposable);
        }

        /// <summary>
        /// リスポーン
        /// </summary>
        /// <param name="position"></param>
        public void Respawn(Vector2 position) => onRespawn.OnNext(position);

        /// <summary>
        /// 自動移動
        /// </summary>
        /// <param name="autoMove"></param>
        public void SetActive(bool autoMove) => isActive.Value = autoMove;

        /// <summary>
        /// スピード設定
        /// </summary>
        /// <param name="speed"></param>
        public void SetMoveSpeed(float speed) => this.moveSpeed.Value = speed;

        public void UpdateSpeed(Vector2 vector2) => velocity.Value = vector2;

        /// <summary>
        /// ジャンプ
        /// </summary>
        public void Jump()
        {
            // 地面についてないとジャンプできない
            if (isGround.Value)
            {
                seController.Play(0);
                onJump.OnNext(Unit.Default);
            }
        }

        /// <summary>
        /// 空中回転
        /// </summary>
        public void Spin() => onSpin.OnNext(Unit.Default);

        /// <summary>
        /// 着地, 地面を離れた
        /// </summary>
        /// <param name="isGround"></param>
        public void SetIsGround(bool isGround) => this.isGround.Value = isGround;

        /// <summary>
        /// 回転後の着地
        /// </summary>
        /// <param name="count"></param>
        public void SuccessSpin(int count) => onSuccessSpin.OnNext(count);

        /// <summary>
        /// 転んだ
        /// </summary>
        public void Stan()
        {
            seController.Play(1);
            onStan.OnNext(Unit.Default);
        }

        public void Dispose()
        {
            moveSpeed?.Dispose();
            onJump?.Dispose();
            isGround?.Dispose();
            onSuccessSpin?.Dispose();
            onStan?.Dispose();
        }
    }
}