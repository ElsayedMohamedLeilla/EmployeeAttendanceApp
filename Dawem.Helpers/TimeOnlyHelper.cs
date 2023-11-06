using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawem.Helpers
{
    public static class TimeOnlyHelper
    {
        public static TimeSpan ToTimeSpan(this TimeOnly time)
        {
            
            return new TimeSpan(time.Hour, time.Minute, time.Second);
        }

        public static TimeOnly ToTimeOnly(TimeSpan timeSpan)
        {
            return new TimeOnly(timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }
    }
}
