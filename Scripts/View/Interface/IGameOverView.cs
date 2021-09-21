using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace kameffee.unity1week202109.View
{
    public interface IGameOverView
    {
        UniTask Open(CancellationToken cancellationToken);

        UniTask Close(CancellationToken cancellationToken);

        IObservable<Unit> OnClickRetry();
    }
}