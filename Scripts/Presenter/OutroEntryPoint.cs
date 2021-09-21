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
    public class OutroEntryPoint : IInitializable, IAsyncStartable, IDisposable
    {
        [Inject]
        private FadeModel fadeModel;

        [Inject]
        private BgmController bgmController;

        private readonly OutroSceneModel model;
        private readonly PlayerModel playerModel;
        private readonly IOutroView view;

        private readonly CompositeDisposable disposable = new CompositeDisposable();

        public OutroEntryPoint(OutroSceneModel model, PlayerModel playerModel, IOutroView view)
        {
            this.model = model;
            this.playerModel = playerModel;
            this.view = view;
        }

        public void Initialize()
        {
            if (!fadeModel.IsOut.Value)
            {
                fadeModel.SetState(true);
            }

            view.OnClickReturn
                .Subscribe(_ => UniTask.Void(async () => await model.ReturnTitle()))
                .AddTo(disposable);
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            bgmController.Play(2);

            await UniTask.Delay(TimeSpan.FromSeconds(5), cancellationToken: cancellation);

            playerModel.SetActive(true);

            if (fadeModel.IsOut.Value)
            {
                await fadeModel.FadeIn(12f, cancellation);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(2));

            await view.Open(cancellation);
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }
    }
}