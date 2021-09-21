using System;
using UniRx;

namespace kameffee.unity1week202109.View
{
    public interface IPlayable
    {
        void Play();
        
        IObservable<Unit> OnComplete { get; }
    }
}