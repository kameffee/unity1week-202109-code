using System;
using UniRx;

namespace kameffee.unity1week202109.View
{
    public interface IPlayerInput
    {
        IObservable<Unit> OnDown { get; }
        
        IObservable<Unit> OnHold { get; }
        
        IObservable<Unit> OnUp { get; }
    }
}