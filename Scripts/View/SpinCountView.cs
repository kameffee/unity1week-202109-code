using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace kameffee.unity1week202109.View
{
    public sealed class SpinCountView : MonoBehaviour, ISpinCountView
    {
        [SerializeField]
        private TextMeshProUGUI text;

        public void Render(int count)
        {
            text.DOFade(0, 0f);
            text.rectTransform.DOScale(0f, 0f);

            text.text = $"{count} スピン！";

            text.rectTransform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
            text.DOFade(1, 0.2f)
                .OnComplete(() => text.DOFade(0, 3f));
        }
    }
}