using Dawem.Models.Response.Schedules.WeekDays;

namespace Dawem.Contract.BusinessLogic.General
{
    public interface IGeneralBL
    {
        List<GetWeekDaysDTO> GetWeekDays();
    }
}
