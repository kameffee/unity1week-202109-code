using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace kameffee.unity1week202109.View
{
    public interface ITimeZoneChangePoint
    {
        IObservable<int> OnChangeTimeZone { get; }
    }
    
    public class TimeZoneChangePoint : MonoBehaviour, ITimeZoneChangePoint
    {
        [SerializeField]
        private int timeZoneId;

        public IObservable<int> OnChangeTimeZone => this.OnTriggerEnter2DAsObservable()
            .Where(collider => collider.gameObject.TryGetComponent<Player>(out var player))
            .Select(_=> timeZoneId);
    }
}