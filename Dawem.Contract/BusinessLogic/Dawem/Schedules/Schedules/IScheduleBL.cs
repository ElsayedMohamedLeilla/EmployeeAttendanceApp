using Dawem.Models.Dtos.Dawem.Schedules.Schedules;
using Dawem.Models.Response.Dawem.Schedules.Schedules;

namespace Dawem.Contract.BusinessLogic.Dawem.Schedules.Schedules
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
