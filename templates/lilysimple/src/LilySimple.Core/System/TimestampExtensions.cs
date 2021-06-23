using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class TimestampExtensions
    {
        public static DateTime ToDateTime(this long timestamp)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            TimeSpan toNow = new TimeSpan(timestamp);
            return startTime.Add(toNow);
        }

        public static long ToTimestamp(this DateTime dateTime)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            return (dateTime.Ticks - startTime.Ticks);
        }
    }
}
