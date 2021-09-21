using System;
using UniRx;
using UnityEngine;

namespace kameffee.unity1week202109.Domain
{
    public sealed class TimeCountModel : IDisposable
    {
        public IReadOnlyReactiveProperty<double> CurrentTime => currentTime;
        private readonly ReactiveProperty<double> currentTime = new ReactiveProperty<double>();
        
        private IDisposable timeCountDisposable;

        /// <summary>
        /// 時間計測を開始
        /// </summary>
        public void Start()
        {
            if (timeCountDisposable != null)
            {
                return;
            }

            timeCountDisposable = Observable.EveryUpdate()
                .Subscribe(_ => Count());
        }

        private void Count()
        {
            currentTime.Value += Time.deltaTime;
        }

        /// <summary>
        /// 0:00.000にする.
        /// </summary>
        public void Reset() => currentTime.Value = 0;

        /// <summary>
        /// 時間計測を止める
        /// </summary>
        public void Stop()
        {
            timeCountDisposable.Dispose();
            timeCountDisposable = null;
        }

        public void Dispose()
        {
            currentTime?.Dispose();
            timeCountDisposable?.Dispose();
        }
    }
}