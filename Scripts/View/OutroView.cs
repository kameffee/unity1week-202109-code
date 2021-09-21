using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace kameffee.unity1week202109.View
{
    public class OutroView : MonoBehaviour, IOutroView
    {
        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private CanvasGroup buttonGroup;

        [SerializeField]
        private CustomButton returnButton;

        public IObservable<Unit> OnClickReturn => returnButton.OnClickAsObservable();

        private void Awake()
        {
            canvasGroup.alpha = 0;
            buttonGroup.alpha = 0;
        }

        public async UniTask Open(CancellationToken cancellationToken)
        {
            await canvasGroup.DOFade(1, 3)
                .SetEase(Ease.Linear)
                .WithCancellation(cancellationToken);

            await buttonGroup.DOFade(1f, 1f)
                .SetEase(Ease.Linear)
                .WithCancellation(cancellationToken);
        }
    }
}