using Dawem.Models.Dtos.Employees.Attendance;
using Dawem.Models.Response.Employees.Attendances.WeeksAttendances;

namespace Dawem.Contract.BusinessLogic.Attendances.Schedules
{
    public interface IScheduleBL
    {
        Task<int> Create(CreateScheduleModel model);
        Task<bool> Update(UpdateScheduleModel model);
        Task<GetScheduleInfoResponseModel> GetInfo(int weekAttendanceId);
        Task<GetScheduleByIdResponseModel> GetById(int weekAttendanceId);
        Task<GetSchedulesResponse> Get(GetSchedulesCriteria model);
        Task<GetSchedulesForDropDownResponse> GetForDropDown(GetSchedulesCriteria model);
        Task<bool> Delete(int weekAttendanceId);
    }
}
