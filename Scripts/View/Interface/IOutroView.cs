using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace kameffee.unity1week202109.View
{
    public interface IOutroView
    {
        public UniTask Open(CancellationToken cancellationToken);

        IObservable<Unit> OnClickReturn { get; }
    }
}