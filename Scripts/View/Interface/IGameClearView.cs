using System;
using Cysharp.Threading.Tasks;
using UniRx;

namespace kameffee.unity1week202109.View
{
    public interface IGameClearView
    {
        IObservable<Unit> OnRanking { get; }

        IObservable<Unit> OnEnding { get; }

        UniTask Open();

        UniTask Close();
    }
}