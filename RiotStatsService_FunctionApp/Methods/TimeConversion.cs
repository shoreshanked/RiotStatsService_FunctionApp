using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiotStatsService_FunctionApp
{
    class TimeConversion
    {
        public static DateTimeOffset UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(unixTimeStamp);
            return dateTimeOffset;
        }

        public static TimeSpan SecondsToMinutes(long seconds)
        {
            // Unix timestamp is seconds past epoch
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return time;
        }
    }
}
