using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace kameffee.unity1week202109.View
{
    public interface ICountDownView
    {
        void Render(int count);
    }
    
    public class CountDownView : MonoBehaviour, ICountDownView
    {
        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private TextMeshProUGUI text;

        private void Awake()
        {
            text.DOFade(0, 0);
        }

        public void Render(int count)
        {
            text.text = count > 0 ? count.ToString() : "スタート！";

            Sequence sequence = DOTween.Sequence();
            sequence.Append(text.DOFade(1, 0.2f).SetEase(Ease.Linear));
            sequence.Join(text.rectTransform.DOScale(1, 0.2f).SetEase(Ease.OutBack));
            sequence.AppendInterval(0.2f);
            sequence.Append(text.DOFade(0f, 0.2f));
        }
    }
}