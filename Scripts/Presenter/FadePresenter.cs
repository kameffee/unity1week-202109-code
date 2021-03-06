using System;
using kameffee.unity1week202109.Domain;
using kameffee.unity1week202109.View;
using UniRx;
using VContainer.Unity;

namespace kameffee.unity1week202109.Presenter
{
    public class FadePresenter : IStartable, IDisposable
    {
        private readonly FadeModel model;
        private readonly IFadeView view;

        private readonly CompositeDisposable compositeDisposable = new CompositeDisposable();

        public FadePresenter(FadeModel model, IFadeView view)
        {
            this.model = model;
            this.view = view;
        }

        public void Start()
        {
            view.Initialize(model.IsOut.Value);
            model.OnFadeOut
                .Subscribe(async duration =>
                {
                    await view.FadeOut(duration);
                    model.SetState(true);
                })
                .AddTo(compositeDisposable);

            model.OnFadeIn
                .Subscribe(async duration =>
                {
                    await view.FadeIn(duration);
                    model.SetState(false);
                })
                .AddTo(compositeDisposable);
        }

        public void Dispose()
        {
            compositeDisposable?.Dispose();
        }
    }
}