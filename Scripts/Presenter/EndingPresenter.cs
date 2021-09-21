using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using kameffee.unity1week202109.Domain;
using kameffee.unity1week202109.UseCase;
using kameffee.unity1week202109.View;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace kameffee.unity1week202109.Presenter
{
    public class EndingPresenter : IInitializable, IAsyncStartable, IDisposable
    {
        [Inject]
        private FadeModel fadeModel;

        [Inject]
        private BgmController bgmController;

        private readonly EndingSceneModel sceneModel;
        private readonly IPlayable playable;

        private readonly CompositeDisposable disposable = new CompositeDisposable();

        public EndingPresenter(EndingSceneModel sceneModel, IPlayable playable)
        {
            this.sceneModel = sceneModel;
            this.playable = playable;
        }

        public void Initialize()
        {
            playable.OnComplete
                .Subscribe(_ => UniTask.Void(async () => await sceneModel.NextScene()))
                .AddTo(disposable);
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            bgmController.Play(1);

            if (fadeModel.IsOut.Value)
            {
                await fadeModel.FadeIn(cancellationToken: cancellation);
            }

            // 開始
            playable.Play();
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }
    }
}