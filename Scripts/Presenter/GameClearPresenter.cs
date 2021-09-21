using kameffee.unity1week202109.Domain;
using kameffee.unity1week202109.View;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace kameffee.unity1week202109.Presenter
{
    public class GameClearPresenter : MonoBehaviour, IInitializable
    {
        private readonly GameClearModel model;
        private readonly IGameClearView view;

        private readonly CompositeDisposable disposable = new CompositeDisposable();

        public GameClearPresenter(GameClearModel model, IGameClearView view)
        {
            this.model = model;
            this.view = view;
        }

        public void Initialize()
        {
            model.OnOpen.Subscribe(_ => view.Open()).AddTo(disposable);
            model.OnClose.Subscribe(_ => view.Close()).AddTo(disposable);

            view.OnRanking.Subscribe(_ => model.Ranking()).AddTo(disposable);
            view.OnEnding.Subscribe(_ => model.Ending()).AddTo(disposable);
        }
    }
}