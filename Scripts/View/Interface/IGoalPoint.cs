using System;
using UniRx;

namespace kameffee.unity1week202109.View
{
    public interface IGoalPoint
    {
        IObservable<Unit> OnEnterPlayer { get; }
    }
}