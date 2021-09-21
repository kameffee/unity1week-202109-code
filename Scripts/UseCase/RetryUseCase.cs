using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using kameffee.unity1week202109.Domain;
using kameffee.unity1week202109.View;
using UnityEngine.SceneManagement;

namespace kameffee.unity1week202109.UseCase
{
    public sealed class RetryUseCase
    {
        private readonly PlayerModel playerModel;
        private readonly IStartPoint startPoint;
        private readonly FadeModel fadeModel;

        public RetryUseCase(PlayerModel playerModel, IStartPoint startPoint, FadeModel fadeModel)
        {
            this.playerModel = playerModel;
            this.startPoint = startPoint;
            this.fadeModel = fadeModel;
        }

        public async UniTask Run(CancellationToken cancellationToken)
        {
            // フェードアウト
            await fadeModel.FadeOut(cancellationToken: cancellationToken);

            SceneManager.LoadScene("Main");
        }

#if false
        public async UniTask Run(CancellationToken cancellationToken)
        {
            // フェードアウト
            await fadeModel.FadeOut(cancellationToken: cancellationToken);

            // Playerを初期ポジションへ
            playerModel.Respawn(startPoint.Position);
            playerModel.SetActive(false);

            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellationToken);

            // フェードイン
            await fadeModel.FadeIn(cancellationToken: cancellationToken);
        }
#endif
    }
}