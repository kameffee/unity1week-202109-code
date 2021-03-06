using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace kameffee.unity1week202109.View
{
    public class BgmPlayer : MonoBehaviour, IBgmPlayer
    {
        private AudioSource AudioSource => audioSource ? audioSource : (audioSource = GetComponent<AudioSource>());
        private AudioSource audioSource;

        private float CurrentVolume { get; set; }

        private void Awake()
        {
            this.gameObject.AddComponent<AudioSource>();
            DontDestroyOnLoad(this.gameObject);
        }

        public bool IsPlaying => AudioSource.isPlaying;

        public void Play(float fadeInTime = 0)
        {
            AudioSource.volume = 0;
            AudioSource.DOFade(CurrentVolume, fadeInTime);
            AudioSource.loop = true;
            AudioSource.Play();
        }

        public void Play(AudioClip audioClip, float fadeInTime = 0f)
        {
            if (audioClip != null)
                AudioSource.clip = audioClip;

            AudioSource.loop = true;
            if (fadeInTime > 0f)
                AudioSource.volume = 0;
            AudioSource.DOFade(CurrentVolume, fadeInTime);
            AudioSource.Play();
        }

        public async UniTask Stop(float fadeOutTime = 0f)
        {
            await AudioSource.DOFade(0, fadeOutTime);
            AudioSource.Stop();
        }

        public void SetVolume(float volume)
        {
            CurrentVolume = volume;
            AudioSource.volume = volume;
        }
    }
}