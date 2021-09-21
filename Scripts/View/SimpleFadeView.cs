using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace kameffee.unity1week202109.View
{
    public class SimpleFadeView : MonoBehaviour, IFadeView
    {
        [SerializeField]
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void Initialize(bool isOut)
        {
            if (isOut)
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                canvasGroup.alpha = 1;
            }
            else
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.alpha = 0;
            }
        }

        public async UniTask FadeOut(float duration = 1)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            await canvasGroup.DOFade(1, duration).SetEase(Ease.Linear);
        }

        public async UniTask FadeIn(float duration = 1)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            await canvasGroup.DOFade(0, duration).SetEase(Ease.Linear);
        }
    }
}
