using System;
using Cysharp.Threading.Tasks;
using kameffee.unity1week202109.UseCase;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace kameffee.unity1week202109.Domain
{
    public class OutroSceneModel
    {
        [Inject]
        private FadeModel fadeModel;

        [Inject]
        private BgmController bgmController;

        private float fadeTime = 5f;

        public async UniTask ReturnTitle()
        {
            await UniTask.WhenAll(
                fadeModel.FadeOut(fadeTime),
                bgmController.Stop(fadeTime));

            await SceneManager.LoadSceneAsync("Title");
        }
    }
}