using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class TimestampExtensions
    {
        public static DateTime ToDateTime(this long timestamp, TimeZoneInfo timeZoneInfo)
        {
            DateTime startTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), timeZoneInfo);
            TimeSpan toNow = new TimeSpan(timestamp);
            return startTime.Add(toNow);
        }

        public static long ToTimestamp(this DateTime dateTime, TimeZoneInfo timeZoneInfo)
        {
            DateTime startTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), timeZoneInfo);
            return (dateTime.Ticks - startTime.Ticks);
        }
    }
}
