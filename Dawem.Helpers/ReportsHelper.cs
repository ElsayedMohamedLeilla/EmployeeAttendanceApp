using Dawem.Enums.Generals;

namespace Dawem.Helpers
{
    public class ReportsHelper
    {
        public static int GetShouldAttendCount(List<WeekDay> weekDays, DateTime dateFrom, DateTime dateTo)
        {
            var dates = DateHelper.GetDatesBetweenTwoDates(dateFrom, dateTo);
            return dates.Count(d => weekDays.Contains((WeekDay)d.DayOfWeek));
        }
        public static decimal SumListOfNumber(List<int> list)
        {
            return list == null ? 0 : list.Sum();
        }
    }
}
