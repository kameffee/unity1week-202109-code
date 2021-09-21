using UnityEngine;

namespace kameffee.unity1week202109.View
{
    public interface ISePlayer
    {
        bool IsPlaying { get; }

        void Play(AudioClip audioClip);

        void SetVolume(float volume);
    }
}