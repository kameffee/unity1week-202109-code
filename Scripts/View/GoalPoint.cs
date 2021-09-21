using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace kameffee.unity1week202109.View
{
    public class GoalPoint : MonoBehaviour, IGoalPoint
    {
        public IObservable<Unit> OnEnterPlayer => this.OnTriggerEnter2DAsObservable()
            .Where(col => col.CompareTag("Player"))
            .AsUnitObservable();
    }
}