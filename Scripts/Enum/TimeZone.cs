using System;

namespace kameffee.unity1week202109.Enum
{
    [Flags]
    public enum TimeZone
    {
        Morning = 1 << 0,
        Evening = 1 << 1,
        Night = 1 << 2,
    }
}