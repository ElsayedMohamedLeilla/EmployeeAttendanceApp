using Dawem.Models.Dtos.Schedules.Schedules;
using Dawem.Models.Response.Schedules.Schedules;

namespace Dawem.Contract.BusinessLogic.Schedules.Schedules
{
    public interface IScheduleBL
    {
        Task<int> Create(CreateScheduleModel model);
        Task<bool> Update(UpdateScheduleModel model);
        Task<GetScheduleInfoResponseModel> GetInfo(int scheduleId);
        Task<GetScheduleByIdResponseModel> GetById(int scheduleId);
        Task<GetSchedulesResponse> Get(GetSchedulesCriteria model);
        Task<GetSchedulesForDropDownResponse> GetForDropDown(GetSchedulesCriteria model);
        Task<bool> Delete(int scheduleId);
        Task<GetSchedulesInformationsResponseDTO> GetSchedulesInformations();
    }
}
