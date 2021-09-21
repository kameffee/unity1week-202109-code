using System;
using kameffee.unity1week202109.Domain;
using kameffee.unity1week202109.View;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace kameffee.unity1week202109.Presenter
{
    public class TimeZonePresenter : IInitializable, IDisposable
    {
        private readonly TimeZoneModel timeZoneModel;
        private readonly ITimeZoneView timeZoneView;
        private readonly FieldTimeZonePointList fieldTimeZonePointList;

        private readonly CompositeDisposable disposable = new CompositeDisposable();

        public TimeZonePresenter(TimeZoneModel timeZoneModel, ITimeZoneView timeZoneView, FieldTimeZonePointList fieldTimeZonePointList)
        {
            this.timeZoneModel = timeZoneModel;
            this.timeZoneView = timeZoneView;
            this.fieldTimeZonePointList = fieldTimeZonePointList;
        }

        public void Initialize()
        {
            Debug.Log($"fieldTimeZonePointList: {fieldTimeZonePointList.Count}");

            fieldTimeZonePointList.OnChangePoint
                .Subscribe(id => timeZoneModel.SetTimeZone(id))
                .AddTo(disposable);
            
            timeZoneModel.Current
                .SkipLatestValueOnSubscribe()
                .Subscribe(timeZoneData =>
                {
                    timeZoneView.SetNextSprite(timeZoneData.Sky);
                    timeZoneView.FadeNext(10f);
                })
                .AddTo(disposable);
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }
    }
}