using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Response.Employees.Attendances.WeeksAttendances;

namespace Dawem.Contract.BusinessLogic.WeekAttendances
{
    public interface IWeekAttendanceBL
    {
        Task<int> Create(CreateWeekAttendanceModel model);
        Task<bool> Update(UpdateWeekAttendanceModel model);
        Task<GetWeekAttendanceInfoResponseModel> GetInfo(int weekAttendanceId);
        Task<GetWeekAttendanceByIdResponseModel> GetById(int weekAttendanceId);
        Task<GetWeekAttendancesResponse> Get(GetWeekAttendancesCriteria model);
        Task<GetWeekAttendancesForDropDownResponse> GetForDropDown(GetWeekAttendancesCriteria model);
        Task<bool> Delete(int weekAttendanceId);
    }
}
