using Dawem.Models.Response.Schedules.WeekDays;

namespace Dawem.Contract.BusinessLogic.Dawem.General
{
    public interface IGeneralBL
    {
        List<GetWeekDaysDTO> GetWeekDays();
    }
}
