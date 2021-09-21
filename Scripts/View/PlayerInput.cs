using System;
using UniRx;
using UnityEngine;

namespace kameffee.unity1week202109.View
{
    public sealed class PlayerInput : IPlayerInput, IDisposable
    {
        public IObservable<Unit> OnDown => onJump;
        public IObservable<Unit> OnHold => onJumpHold;

        public IObservable<Unit> OnUp => onUp;

        private readonly Subject<Unit> onJump = new Subject<Unit>();
        private readonly Subject<Unit> onJumpHold = new Subject<Unit>();
        private readonly Subject<Unit> onUp = new Subject<Unit>();

        private readonly CompositeDisposable disposable = new CompositeDisposable();

        public PlayerInput()
        {
            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                .Subscribe(_ => onJump.OnNext(Unit.Default))
                .AddTo(disposable);

            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
                .Subscribe(_ => onJumpHold.OnNext(Unit.Default))
                .AddTo(disposable);
            
            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space))
                .Subscribe(_ => onUp.OnNext(Unit.Default))
                .AddTo(disposable);
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }
    }
}