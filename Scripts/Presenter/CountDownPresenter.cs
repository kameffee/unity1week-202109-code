using kameffee.unity1week202109.Domain;
using kameffee.unity1week202109.View;
using UniRx;
using VContainer.Unity;

namespace kameffee.unity1week202109.Presenter
{
    public sealed class CountDownPresenter : IInitializable
    {
        private readonly CountDownModel model;
        private readonly ICountDownView view;

        public CountDownPresenter(CountDownModel model, ICountDownView view)
        {
            this.model = model;
            this.view = view;
        }

        public void Initialize()
        {
            model.OnChangeCount
                .Subscribe(count => view.Render(count));
        }
    }
}