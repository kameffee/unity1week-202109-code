using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace kameffee.unity1week202109.Domain
{
    /// <summary>
    /// ゲーム開始前のカウント
    /// </summary>
    public class CountDownModel
    {
        public IObservable<int> OnChangeCount => onChangeCount;
        private readonly Subject<int> onChangeCount = new Subject<int>();

        /// <summary>
        /// カウントダウン
        /// スタートと同時に抜ける
        /// </summary>
        /// <param name="maxCount"></param>
        /// <param name="cancellationToken"></param>
        public async UniTask StartCount(int maxCount, CancellationToken cancellationToken)
        {
            var count = maxCount;
            while (count > 0)
            {
                onChangeCount.OnNext(count);
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: cancellationToken);
                count--;
            }

            onChangeCount.OnNext(count);
        }
    }
}