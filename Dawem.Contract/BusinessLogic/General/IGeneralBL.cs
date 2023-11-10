using Dawem.Models.Response.Employees.Attendances.WeekDays;

namespace Dawem.Contract.BusinessLogic.General
{
    public interface IGeneralBL
    {
        List<GetWeekDaysDTO> GetWeekDays();
    }
}
