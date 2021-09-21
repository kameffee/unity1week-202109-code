using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using kameffee.unity1week202109.Domain;
using kameffee.unity1week202109.View;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace kameffee.unity1week202109.UseCase
{
    /// <summary>
    /// ゲームサイクル
    /// </summary>
    public sealed class GameCycle : IInitializable, IAsyncStartable, IDisposable
    {
        [Inject]
        private BgmController bgmController;

        private readonly PlayerModel playerModel;
        private readonly RetryUseCase retryUseCase;
        private readonly CountDownModel countDownModel;
        private readonly IGameOverView gameOverView;
        private readonly GameClearModel gameClearModel;
        private readonly TimeCountModel timeCountModel;
        private readonly FadeModel fadeModel;
        private readonly IGoalPoint goalPoint;
        private readonly CompositeDisposable disposable = new CompositeDisposable();

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public GameCycle(
            PlayerModel playerModel,
            IGameOverView gameOverView,
            GameClearModel gameClearModel,
            RetryUseCase retryUseCase,
            CountDownModel countDownModel,
            FadeModel fadeModel,
            IGoalPoint goalPoint,
            TimeCountModel timeCountModel)
        {
            this.playerModel = playerModel;
            this.retryUseCase = retryUseCase;
            this.countDownModel = countDownModel;
            this.gameOverView = gameOverView;
            this.gameClearModel = gameClearModel;
            this.fadeModel = fadeModel;
            this.goalPoint = goalPoint;
            this.timeCountModel = timeCountModel;
        }

        public void Initialize()
        {
#if UNITY_EDITOR
            // Debug: 強制クリア
            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.Alpha1))
                .Subscribe(_ => GameClear(cancellationTokenSource.Token).Forget())
                .AddTo(disposable);

            // Debug: 強制ゲームオーバー
            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.Alpha2))
                .Subscribe(_ => GameOver(cancellationTokenSource.Token).Forget())
                .AddTo(disposable);
#endif

            // プレイヤーが転んだらゲームオーバー
            playerModel.OnStan
                .Subscribe(_ => GameOver(cancellationTokenSource.Token).Forget())
                .AddTo(disposable);

            // リトライ
            gameOverView.OnClickRetry()
                .Subscribe(_ => Retry(cancellationTokenSource.Token).Forget())
                .AddTo(disposable);

            // ゲームクリア
            goalPoint.OnEnterPlayer
                .Subscribe(_ => GameClear(cancellationTokenSource.Token).Forget())
                .AddTo(disposable);

            // エンディング
            gameClearModel.OnEnding
                .Subscribe(_ => StartEnding().Forget())
                .AddTo(disposable);
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            // ゲームスタート
            await GameStart(cancellation);
        }

        public async UniTask GameStart(CancellationToken cancellationToken)
        {
            Debug.Log("GameStart");

            timeCountModel.Reset();

            if (fadeModel.IsOut.Value)
            {
                await fadeModel.FadeIn(cancellationToken: cancellationToken);
            }

            // カウントダウンする
            await countDownModel.StartCount(3, cancellationToken);

            if (bgmController.Current != 0)
            {
                bgmController.Play(0);
            }

            timeCountModel.Start();

            playerModel.SetActive(true);
        }

        /// <summary>
        /// ゲームオーバー
        /// </summary>
        /// <param name="cancellationToken"></param>
        public async UniTaskVoid GameOver(CancellationToken cancellationToken)
        {
            Debug.Log("GameOver");

            timeCountModel.Stop();

            // キャラ移動停止
            playerModel.SetActive(false);

            // ゲームオーバー画面を開く
            await gameOverView.Open(cancellationToken);
        }

        public async UniTaskVoid Retry(CancellationToken cancellationToken)
        {
            Debug.Log("Retry");

            // ゲームオーバー画面を閉じる
            await gameOverView.Close(cancellationToken);

            // リトライ処理
            await retryUseCase.Run(cancellationToken);
        }

        /// <summary>
        /// ゲームクリア
        /// </summary>
        public async UniTaskVoid GameClear(CancellationToken cancellationToken)
        {
            Debug.Log("Game Clear!!");

            timeCountModel.Stop();

            // プレイヤー無効化
            playerModel.SetActive(false);

            // クリア演出

            // 表示開始
            gameClearModel.Open();
        }

        public async UniTaskVoid StartEnding()
        {
            // クリア画面を閉じる
            gameClearModel.Close();

            await fadeModel.FadeOut(cancellationToken: cancellationTokenSource.Token);

            // エンディングへ遷移
            await SceneManager.LoadSceneAsync("Ending").WithCancellation(cancellationTokenSource.Token);
        }

        public void Dispose()
        {
            playerModel?.Dispose();
            disposable?.Dispose();
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
        }
    }
}