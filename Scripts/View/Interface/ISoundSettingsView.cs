using System;
using UniRx;

namespace kameffee.unity1week202109.View
{
    public interface ISoundSettingsView
    {
        IObservable<float> OnChangeBgmVolume { get; }

        IObservable<float> OnChangeSeVolume { get; }
        
        IObservable<Unit> OnChangeEndSe { get; }

        void SetBgmVolume(float volume);

        void SetSeVolume(float volume);
    }
}