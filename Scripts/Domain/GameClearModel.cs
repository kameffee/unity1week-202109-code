using System;
using naichilab;
using UniRx;

namespace kameffee.unity1week202109.Domain
{
    public sealed class GameClearModel
    {
        public IObservable<Unit> OnOpen => onOpen;
        private readonly Subject<Unit> onOpen = new Subject<Unit>();

        public IObservable<Unit> OnClose => onClose;
        private readonly Subject<Unit> onClose = new Subject<Unit>();

        // エンディング開始
        public IObservable<Unit> OnEnding => onEnding;
        private readonly Subject<Unit> onEnding = new Subject<Unit>();

        private readonly TimeCountModel timeCountModel;

        public GameClearModel(TimeCountModel timeCountModel)
        {
            this.timeCountModel = timeCountModel;
        }

        public void Open() => onOpen.OnNext(Unit.Default);

        public void Close() => onClose.OnNext(Unit.Default);

        public void Ending() => onEnding.OnNext(Unit.Default);

        /// <summary>
        /// ランキング表示
        /// </summary>
        public void Ranking()
        {
            // 送信 & 表示
            TimeSpan scoreTime = TimeSpan.FromSeconds(timeCountModel.CurrentTime.Value);
            RankingLoader.Instance.SendScoreAndShowRanking(scoreTime);
        }
    }
}