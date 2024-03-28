using Dawem.Models.Dtos.Dawem.Schedules.SchedulePlans;
using Dawem.Models.Response.Dawem.Schedules.SchedulePlans;

namespace Dawem.Contract.BusinessLogic.Dawem.Schedules.SchedulePlans
{
    public interface ISchedulePlanBL
    {
        Task<int> Create(CreateSchedulePlanModel model);
        Task<bool> Update(UpdateSchedulePlanModel model);
        Task<GetSchedulePlanInfoResponseModel> GetInfo(int schedulePlanId);
        Task<GetSchedulePlanByIdResponseModel> GetById(int schedulePlanId);
        Task<GetSchedulePlansResponse> Get(GetSchedulePlansCriteria model);
        Task<GetSchedulePlansForDropDownResponse> GetForDropDown(GetSchedulePlansCriteria model);
        Task<bool> Delete(int schedulePlanId);
        Task HandleSchedulePlanBackgroundJob();
        Task<GetSchedulePlansInformationsResponseDTO> GetSchedulePlansInformations();
    }
}
