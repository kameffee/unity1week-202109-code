using DG.Tweening;
using UnityEngine;

namespace kameffee.unity1week202109.View
{
    public interface ITimeZoneView
    {
        void SetNextSprite(Sprite sprite);

        void FadeNext(float time);
    }
    
    /// <summary>
    /// 変化する背景
    /// </summary>
    public class TimeZoneBackgroundView : MonoBehaviour, ITimeZoneView
    {
        [SerializeField]
        private SpriteRenderer mainSprite;

        [SerializeField]
        private SpriteRenderer nextSprite;

        public void SetNextSprite(Sprite sprite)
        {
            nextSprite.DOFade(0, 0);
            nextSprite.sprite = sprite;
        }

        public void FadeNext(float time)
        {
            nextSprite.DOFade(1, time)
                .OnComplete(() =>
                {
                    mainSprite.sprite = nextSprite.sprite;
                    mainSprite.DOFade(1, 0);
                    nextSprite.DOFade(0, 0);
                });
        }
    }
}