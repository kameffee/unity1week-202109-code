using System.Collections.Generic;
using kameffee.unity1week202109.Domain;
using kameffee.unity1week202109.Entity;
using kameffee.unity1week202109.View;
using UniRx;
using VContainer.Unity;

namespace kameffee.unity1week202109.Presenter
{
    public class TimeZoneSwitchPresenter : IInitializable
    {
        private readonly TimeZoneModel timeZoneModel;
        private readonly IReadOnlyList<ITimeZoneSwitch> timeZoneSwitches;

        private readonly CompositeDisposable disposable = new CompositeDisposable();
        
        public TimeZoneSwitchPresenter(
            TimeZoneModel timeZoneModel,
            IReadOnlyList<ITimeZoneSwitch> timeZoneSwitches)
        {
            this.timeZoneModel = timeZoneModel;
            this.timeZoneSwitches = timeZoneSwitches;
        }

        public void Initialize()
        {
            timeZoneModel.Current
                .Subscribe(UpdateTimeZone)
                .AddTo(disposable);
        }

        private void UpdateTimeZone(TimeZoneData timeZoneData)
        {
            foreach (var timeZoneSwitch in timeZoneSwitches)
            {
                timeZoneSwitch.SetTimeZone(timeZoneData.TimeZone);
            }
        }
    }
}