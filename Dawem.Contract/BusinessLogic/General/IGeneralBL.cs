using Dawem.Models.Response.Attendances.WeekDays;

namespace Dawem.Contract.BusinessLogic.General
{
    public interface IGeneralBL
    {
        List<GetWeekDaysDTO> GetWeekDays();
    }
}
