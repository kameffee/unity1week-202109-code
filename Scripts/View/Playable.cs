using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Playables;

namespace kameffee.unity1week202109.View
{
    public class Playable : MonoBehaviour, IPlayable
    {
        [SerializeField]
        private PlayableDirector playableDirector;

        public void Play() => playableDirector.Play();

        public IObservable<Unit> OnComplete => onComplete;
        private readonly Subject<Unit> onComplete = new Subject<Unit>();

        private void Start()
        {
            this.UpdateAsObservable()
                .Where(_ => playableDirector.duration <= playableDirector.time)
                .Subscribe(_ =>
                {
                    onComplete.OnNext(Unit.Default);
                    onComplete.OnCompleted();
                }).AddTo(this);
        }
    }
}