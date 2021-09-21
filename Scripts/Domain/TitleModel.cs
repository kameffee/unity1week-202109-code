using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace kameffee.unity1week202109.Domain
{
    public class TitleModel
    {
        private readonly FadeModel fadeModel;

        public TitleModel(FadeModel fadeModel)
        {
            this.fadeModel = fadeModel;
        }

        public async UniTaskVoid GameStart()
        {
            await fadeModel.FadeOut();

            await SceneManager.LoadSceneAsync("Intro");
        }
    }
}