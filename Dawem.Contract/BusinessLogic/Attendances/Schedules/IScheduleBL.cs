using Dawem.Models.Dtos.Attendances.Schedules;
using Dawem.Models.Response.Attendances.Schedules;

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
