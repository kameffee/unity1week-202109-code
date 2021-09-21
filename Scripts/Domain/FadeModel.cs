using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace kameffee.unity1week202109.Domain
{
    public class FadeModel
    {
        public IReadOnlyReactiveProperty<bool> IsOut => isOut;
        private readonly ReactiveProperty<bool> isOut = new ReactiveProperty<bool>(false);

        public IObservable<float> OnFadeOut => onFadeOut;
        private readonly Subject<float> onFadeOut = new Subject<float>();

        public IObservable<float> OnFadeIn => onFadeIn;
        private readonly Subject<float> onFadeIn = new Subject<float>();

        public async UniTask FadeOut(float duration = 1f, CancellationToken cancellationToken = default)
        {
            onFadeOut.OnNext(duration);
            await UniTask.WaitUntil(() => isOut.Value, cancellationToken: cancellationToken);
        }

        public async UniTask FadeIn(float duration = 1f, CancellationToken cancellationToken = default)
        {
            onFadeIn.OnNext(duration);
            await UniTask.WaitUntil(() => !isOut.Value, cancellationToken: cancellationToken);
        }

        public void SetState(bool isOut)
        {
            this.isOut.Value = isOut;
        }
    }
}