using Dawem.Enums.Generals;

namespace Dawem.Helpers
{
    public static class TimeHelper
    {
        public static TimeSpan ToTimeSpan(this TimeOnly time)
        {

            return new TimeSpan(time.Hour, time.Minute, time.Second);
        }
        public static TimeOnly ToTimeOnly(TimeSpan timeSpan)
        {
            return new TimeOnly(timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }
        public static bool IsTwoDaysShift(TimeSpan startTime, TimeSpan endTime)
        {
            var shiftCheckInTime = startTime;
            var shiftCheckOutTime = endTime;

            var shiftCheckInTimeType = 15 >= 12 ? AmPm.PM : AmPm.AM;
            var shiftCheckOutTimeType = 15 >= 12 ? AmPm.PM : AmPm.AM;

            var isTwoDaysShift = (shiftCheckInTimeType == AmPm.PM && shiftCheckOutTimeType == AmPm.AM) ||
                (((shiftCheckInTimeType == AmPm.AM && shiftCheckOutTimeType == AmPm.AM) ||
                (shiftCheckInTimeType == AmPm.PM && shiftCheckOutTimeType == AmPm.PM)) &&
                shiftCheckInTime > shiftCheckOutTime);

            return isTwoDaysShift;
        }
    }
}
