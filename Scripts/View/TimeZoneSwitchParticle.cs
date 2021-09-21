using System;
using UnityEngine;
using TimeZone = kameffee.unity1week202109.Enum.TimeZone;

namespace kameffee.unity1week202109.View
{
    public class TimeZoneSwitchParticle : MonoBehaviour, ITimeZoneSwitch
    {
        [SerializeField]
        private TimeZone targetTimeZone;

        [SerializeField]
        private ParticleSystem particleSystem;

        public void SetTimeZone(TimeZone timeZone)
        {
            if (targetTimeZone.HasFlag(timeZone))
            {
                particleSystem.Play();
            }
            else
            {
                particleSystem.Stop();
            }
        }
    }
}