using Dawem.Models.Response.Employees.Attendances.WeeksAttendances;

namespace Dawem.Contract.BusinessLogic.Employees.Department
{
    public interface IGeneralBL
    {
        List<GetWeekDaysDTO> GetWeekDays();
    }
}
