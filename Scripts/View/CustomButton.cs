using System;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace kameffee.unity1week202109.View
{
    public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,
        IPointerUpHandler, IPointerClickHandler
    {
        public IObservable<Unit> OnClickAsObservable() => onClick;
        private readonly Subject<Unit> onClick = new Subject<Unit>();

        [Header("Targets")]
        [SerializeField]
        private Graphic targetGraphic;

        [SerializeField]
        private Graphic iconGraphic;

        [SerializeField]
        private TextMeshProUGUI text;

        [Header("Settings")]
        [SerializeField]
        private Color enterColor = Color.yellow;

        [SerializeField]
        private float onDownScaling = 0.95f;

        private Color textDefaultColorCache;
        private Color iconDefaultColorCache;

        private RectTransform rectTransform;

        protected void Awake()
        {
            textDefaultColorCache = text != null ? text.color : Color.white;
            iconDefaultColorCache = iconGraphic != null ? iconGraphic.color : Color.white;
            rectTransform = transform as RectTransform;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            targetGraphic?.DOFade(0.95f, 0.2f);

            text?.DOFaceColor(enterColor, 0.2f);
            iconGraphic?.DOColor(enterColor, 0.2f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            targetGraphic?.DOFade(0.8f, 0.2f);

            text?.DOFaceColor(textDefaultColorCache, 0.2f);
            iconGraphic?.DOColor(iconDefaultColorCache, 0.2f);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            rectTransform.DOScale(onDownScaling, 0.15f);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            rectTransform.DOScale(1, 0.15f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // SEを鳴らす.
            onClick.OnNext(Unit.Default);
        }
    }
}