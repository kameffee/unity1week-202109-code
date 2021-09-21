using System.Threading;
using Cysharp.Threading.Tasks;
using kameffee.unity1week202109.Domain;
using kameffee.unity1week202109.UseCase;
using kameffee.unity1week202109.View;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace kameffee.unity1week202109.Presenter
{
    public sealed class IntroPresenter : IInitializable, IAsyncStartable
    {
        [Inject]
        private FadeModel fadeModel;

        [Inject]
        private BgmController bgmController;

        private readonly IPlayable playable;
        private readonly IntroModel model;

        private readonly CompositeDisposable disposable = new CompositeDisposable();

        public IntroPresenter(IntroModel model, IPlayable playable)
        {
            this.model = model;
            this.playable = playable;
        }

        public void Initialize()
        {
            playable.OnComplete
                .Subscribe(_ => model.Complete().Forget())
                .AddTo(disposable);
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            bgmController.Play(3);
            if (fadeModel.IsOut.Value)
            {
                await fadeModel.FadeIn(1, cancellationToken: cancellation);
            }

            playable.Play();
        }
    }
}