using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace kameffee.unity1week202109.View
{
    public sealed class GameClearView : MonoBehaviour, IGameClearView
    {
        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private CustomButton rankingButton;

        [SerializeField]
        private CustomButton endingButton;

        public IObservable<Unit> OnRanking => rankingButton.OnClickAsObservable();

        public IObservable<Unit> OnEnding => endingButton.OnClickAsObservable();

        private void Awake()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.DOFade(0, 0);
        }

        public async UniTask Open()
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            await canvasGroup.DOFade(1, 2).WithCancellation(this.GetCancellationTokenOnDestroy());
        }

        public async UniTask Close()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            await canvasGroup.DOFade(0, 1).WithCancellation(this.GetCancellationTokenOnDestroy());
        }
    }
}