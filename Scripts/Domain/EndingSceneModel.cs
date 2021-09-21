using Cysharp.Threading.Tasks;
using kameffee.unity1week202109.UseCase;
using UnityEngine.SceneManagement;
using VContainer;

namespace kameffee.unity1week202109.Domain
{
    public sealed class EndingSceneModel
    {
        [Inject]
        private FadeModel fadeModel;

        [Inject]
        private BgmController bgmController;

        public async UniTask NextScene()
        {
            await UniTask.WhenAll(
                fadeModel.FadeOut(3f),
                bgmController.Stop(3f));

            await SceneManager.LoadSceneAsync("Outro");
        }
    }
}