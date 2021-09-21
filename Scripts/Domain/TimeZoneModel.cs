using kameffee.unity1week202109.Entity;
using kameffee.unity1week202109.Enum;
using UniRx;
using UnityEngine;

namespace kameffee.unity1week202109.Domain
{
    /// <summary>
    /// 時間帯状態
    /// </summary>
    public class TimeZoneModel
    {
        private readonly TimeZoneBundle bundle;
        public IReadOnlyReactiveProperty<TimeZoneData> Current => current;
        private readonly ReactiveProperty<TimeZoneData> current = new ReactiveProperty<TimeZoneData>();

        public TimeZoneModel(TimeZoneBundle bundle, TimeZone initialTimeZone)
        {
            this.bundle = bundle;
            current.Value = bundle.GetData(initialTimeZone);
        }

        public void SetTimeZone(int id)
        {
            current.Value = bundle.GetData(id);
        }
    }
}