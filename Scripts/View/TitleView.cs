using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace kameffee.unity1week202109.View
{
    public interface ITitleView
    {
        public IObservable<Unit> OnClickStart { get; }
    }
    
    public class TitleView : MonoBehaviour, ITitleView
    {
        [SerializeField]
        private CustomButton startButton;

        public IObservable<Unit> OnClickStart => startButton.OnClickAsObservable();
    }
}