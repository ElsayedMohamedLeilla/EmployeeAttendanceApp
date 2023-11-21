using Dawem.Models.Dtos.Schedules.SchedulePlans;
using Dawem.Models.Response.Schedules.SchedulePlans;

namespace Dawem.Contract.BusinessLogic.Schedules.SchedulePlans
{
    public interface ISchedulePlanBL
    {
        Task<int> Create(CreateSchedulePlanModel model);
        Task<bool> Update(UpdateSchedulePlanModel model);
        Task<GetSchedulePlanInfoResponseModel> GetInfo(int weekAttendanceId);
        Task<GetSchedulePlanByIdResponseModel> GetById(int weekAttendanceId);
        Task<GetSchedulePlansResponse> Get(GetSchedulePlansCriteria model);
        Task<GetSchedulePlansForDropDownResponse> GetForDropDown(GetSchedulePlansCriteria model);
        Task<bool> Delete(int weekAttendanceId);
        Task HandleSchedulePlanBackgroundJob();
    }
}
