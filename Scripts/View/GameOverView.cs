using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace kameffee.unity1week202109.View
{
    public sealed class GameOverView : MonoBehaviour, IGameOverView
    {
        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private CustomButton retryButton;

        public void Start()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        public async UniTask Open(CancellationToken cancellationToken)
        {
            await canvasGroup.DOFade(1, 0.5f).WithCancellation(cancellationToken);
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        public async UniTask Close(CancellationToken cancellationToken)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            await canvasGroup.DOFade(0, 0.5f).WithCancellation(cancellationToken);
        }

        public IObservable<Unit> OnClickRetry() => retryButton.OnClickAsObservable();
    }
}