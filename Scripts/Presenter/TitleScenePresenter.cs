using System;
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
    public sealed class TitleScenePresenter : IInitializable, IAsyncStartable, IDisposable
    {
        [Inject]
        private FadeModel fadeModel;

        [Inject]
        private BgmController bgmController;

        private readonly TitleModel titleModel;
        private readonly ITitleView titleView;

        private readonly CompositeDisposable disposable = new CompositeDisposable();

        public TitleScenePresenter(TitleModel titleModel, ITitleView titleView)
        {
            this.titleModel = titleModel;
            this.titleView = titleView;
        }

        public void Initialize()
        {
            bgmController.Play(0);

            titleView.OnClickStart
                .Subscribe(_ => titleModel.GameStart().Forget())
                .AddTo(disposable);
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            if (fadeModel.IsOut.Value)
            {
                await fadeModel.FadeIn(cancellationToken: cancellation);
            }
        }
    }
}