using System;
using TMPro;
using UnityEngine;

namespace kameffee.unity1week202109.View
{
    public class TimeCountView : MonoBehaviour, ITimeCountView
    {
        [SerializeField]
        private TextMeshProUGUI timeText;

        public void Render(TimeSpan time)
        {
            timeText.text = time.ToString(@"mm\:ss\.fff");
        }
    }
}