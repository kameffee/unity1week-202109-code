using Cysharp.Threading.Tasks;
using UnityEngine;

namespace kameffee.unity1week202109.Domain
{
    public interface IBgmModel
    {
        bool IsPlaying { get; }

        void Play(AudioClip audioClip, float fadeInTime = 0f);

        UniTask Stop(float fadeInTime = 0f);
    }
}