using System.Linq;
using Cysharp.Threading.Tasks;
using kameffee.unity1week202109.Domain;
using kameffee.unity1week202109.Entity;
using UnityEngine;

namespace kameffee.unity1week202109.UseCase
{
    public sealed class BgmController
    {
        private readonly IBgmModel bgmModel;
        private readonly BgmPreset bgmPreset;

        public bool IsPlaying => bgmModel.IsPlaying;

        public int Current { get; private set; } = -1;

        public BgmController(IBgmModel bgmModel, BgmPreset bgmPreset)
        {
            this.bgmModel = bgmModel;
            this.bgmPreset = bgmPreset;
        }

        public void Play(int id, float fadeTime = 0)
        {
            AudioClip clip = bgmPreset.GetAudioClip(id);
            Current = id;
            bgmModel.Play(clip, fadeTime);
        }

        public async UniTask Stop(float fadeTime = 0f)
        {
            Current = -1;
            await bgmModel.Stop(fadeTime);
        }
    }
}