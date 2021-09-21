using System;
using System.Collections.Generic;
using kameffee.unity1week202109.View;
using UniRx;

namespace kameffee.unity1week202109.Domain
{
    /// <summary>
    /// フィールド上にあるタイムゾーン変化ポイントリスト
    /// </summary>
    public class FieldTimeZonePointList
    {
        public IObservable<int> OnChangePoint => onChangePoint;
        private readonly Subject<int> onChangePoint = new Subject<int>();

        public int Count => timeZoneChangePoints.Count;
        
        private readonly IReadOnlyList<ITimeZoneChangePoint> timeZoneChangePoints;

        private readonly CompositeDisposable disposable = new CompositeDisposable();

        public FieldTimeZonePointList(IReadOnlyList<ITimeZoneChangePoint> timeZoneChangePoints)
        {
            this.timeZoneChangePoints = timeZoneChangePoints;

            foreach (var timeZoneChangePoint in timeZoneChangePoints)
            {
                timeZoneChangePoint.OnChangeTimeZone
                    .Subscribe(id => onChangePoint.OnNext(id))
                    .AddTo(disposable);
            }
        }
    }
}