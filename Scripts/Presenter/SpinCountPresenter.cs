using kameffee.unity1week202109.Domain;
using kameffee.unity1week202109.View;
using UniRx;
using VContainer.Unity;

namespace kameffee.unity1week202109.Presenter
{
    public sealed class SpinCountPresenter : IInitializable
    {
        private readonly PlayerModel playerModel;
        private readonly ISpinCountView view;
        private readonly CompositeDisposable disposable = new CompositeDisposable();

        public SpinCountPresenter(PlayerModel playerModel, ISpinCountView view)
        {
            this.playerModel = playerModel;
            this.view = view;
        }

        public void Initialize()
        {
            playerModel.OnSuccessSpin
                .Where(count => count > 0)
                .Subscribe(count => view.Render(count))
                .AddTo(disposable);
        }
    }
}