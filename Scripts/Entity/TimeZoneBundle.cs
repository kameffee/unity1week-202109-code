using System;
using System.Linq;
using UnityEngine;
using TimeZone = kameffee.unity1week202109.Enum.TimeZone;

namespace kameffee.unity1week202109.Entity
{
    [Serializable]
    public class TimeZoneData
    {
        [SerializeField]
        private TimeZone timeZone;
        
        [SerializeField]
        private Sprite sky;

        public TimeZone TimeZone => timeZone;
        
        public Sprite Sky => sky;
    }

    [CreateAssetMenu(fileName = "TimeZone", menuName = "")]
    public class TimeZoneBundle : ScriptableObject
    {
        [SerializeField]
        private TimeZoneData[] dataList;

        public TimeZoneData GetData(int id) => dataList[id];

        public TimeZoneData GetData(TimeZone timeZone)
        {
            return dataList.First(data => data.TimeZone == timeZone);
        }
    }
}