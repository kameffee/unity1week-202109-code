using System;
using Cysharp.Threading.Tasks;
using kameffee.unity1week202109.UseCase;
using UniRx;
using UnityEngine.SceneManagement;
using VContainer;

namespace kameffee.unity1week202109.Domain
{
    public sealed class IntroModel
    {
        public IObservable<Unit> OnTap => onTap;
        private readonly Subject<Unit> onTap = new Subject<Unit>();

        [Inject]
        private FadeModel fadeModel;

        [Inject]
        private BgmController bgmController;

        public void Next() => onTap.OnNext(Unit.Default);

        public async UniTask Complete()
        {
            await UniTask.WhenAll(
                fadeModel.FadeOut(3),
                bgmController.Stop(3));

            // インゲームへ遷移
            await SceneManager.LoadSceneAsync("Main");
        }
    }
}