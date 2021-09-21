using System;
using kameffee.unity1week202109.Domain;
using kameffee.unity1week202109.View;
using UniRx;
using VContainer.Unity;

namespace kameffee.unity1week202109.Presenter
{
    public class TimeCountPresenter : IInitializable, IDisposable
    {
        private readonly TimeCountModel model;
        private readonly ITimeCountView view;

        private readonly CompositeDisposable disposable = new CompositeDisposable();

        public TimeCountPresenter(TimeCountModel model, ITimeCountView view)
        {
            this.model = model;
            this.view = view;
        }

        public void Initialize()
        {
            model.CurrentTime
                .Subscribe(time => view.Render(TimeSpan.FromSeconds(time)))
                .AddTo(disposable);
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }
    }
}