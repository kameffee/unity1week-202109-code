using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace kameffee.unity1week202109.View
{
    public class SoundSettingsView : MonoBehaviour, ISoundSettingsView
    {
        [SerializeField]
        private Slider bgmSlider;

        [SerializeField]
        private Slider seSlider;

        public IObservable<float> OnChangeBgmVolume => bgmSlider.OnValueChangedAsObservable();

        public IObservable<float> OnChangeSeVolume => seSlider.OnValueChangedAsObservable();

        public IObservable<Unit> OnChangeEndSe => seSlider.OnPointerUpAsObservable().AsUnitObservable();

        public void SetBgmVolume(float volume) => bgmSlider.value = volume;

        public void SetSeVolume(float volume) => seSlider.value = volume;
    }
}