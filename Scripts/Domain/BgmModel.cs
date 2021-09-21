using System;
using Cysharp.Threading.Tasks;
using kameffee.unity1week202109.View;
using UniRx;
using UnityEngine;

namespace kameffee.unity1week202109.Domain
{
    public sealed class BgmModel : IBgmModel, IDisposable
    {
        private readonly IBgmPlayer bgmPlayer;

        private readonly SoundSettingsModel settingsModel;

        private readonly CompositeDisposable disposable = new CompositeDisposable();

        public bool IsPlaying => bgmPlayer.IsPlaying;

        public BgmModel(IBgmPlayer bgmPlayer, SoundSettingsModel settingsModel)
        {
            this.bgmPlayer = bgmPlayer;
            this.settingsModel = settingsModel;

            this.settingsModel.BGMVolume
                .Subscribe(volume => bgmPlayer.SetVolume(volume))
                .AddTo(disposable);
        }

        public void Play(AudioClip audioClip, float fadeInTime = 0)
        {
            this.bgmPlayer.Play(audioClip, fadeInTime);
        }

        public async UniTask Stop(float fadeOutTime = 0f)
        {
            await this.bgmPlayer.Stop(fadeOutTime);
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }
    }
}