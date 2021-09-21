using System;
using kameffee.unity1week202109.Domain;
using kameffee.unity1week202109.UseCase;
using kameffee.unity1week202109.View;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace kameffee.unity1week202109.Presenter
{
    public sealed class SoundSettingsPresenter : IInitializable, IDisposable
    {
        [Inject]
        private SeController seController;
        
        private readonly SoundSettingsModel settingsModel;
        private readonly ISoundSettingsView view;

        private readonly CompositeDisposable disposable = new CompositeDisposable();

        public SoundSettingsPresenter(SoundSettingsModel settingsModel, ISoundSettingsView view)
        {
            this.settingsModel = settingsModel;
            this.view = view;
        }

        public void Initialize()
        {
            settingsModel.BGMVolume.Subscribe(volume => view.SetBgmVolume(volume)).AddTo(disposable);
            settingsModel.SeVolume.Subscribe(volume => view.SetSeVolume(volume)).AddTo(disposable);

            view.OnChangeBgmVolume
                .Subscribe(volume => settingsModel.SetBgmVolume(volume))
                .AddTo(disposable);

            view.OnChangeSeVolume
                .Subscribe(volume => settingsModel.SetSeVolume(volume))
                .AddTo(disposable);
            
            // 離したときに音を鳴らす
            view.OnChangeEndSe
                .Subscribe(_ => seController.Play(0))
                .AddTo(disposable);
        }

        public void Dispose()
        {
            settingsModel?.Dispose();
            disposable?.Dispose();
        }
    }
}