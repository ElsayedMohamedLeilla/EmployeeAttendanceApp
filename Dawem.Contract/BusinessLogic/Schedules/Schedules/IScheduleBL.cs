using Dawem.Models.Dtos.Schedules.Schedules;
using Dawem.Models.Response.Schedules.Schedules;

namespace Dawem.Contract.BusinessLogic.Schedules.Schedules
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
