using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.Playables;

namespace kameffee.unity1week202109.View
{
    public class PlayableNext : MonoBehaviour
    {
        [SerializeField]
        private PlayableDirector playableDirector;

        [SerializeField]
        private CustomButton button;

        private void Start()
        {
            button.gameObject.SetActive(false);
            button.OnClickAsObservable()
                .Subscribe(_ => Resume())
                .AddTo(this);
        }

        public void Pause()
        {
            button.gameObject.SetActive(true);
            playableDirector.Pause();
        }

        public void Resume()
        {
            button.gameObject.SetActive(false);
            playableDirector.Resume();
        }
    }
}