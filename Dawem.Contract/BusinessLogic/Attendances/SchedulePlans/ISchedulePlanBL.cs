using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Response.Attendances.Schedules;

namespace Dawem.Contract.BusinessLogic.Attendances.SchedulePlans
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
    }
}
