using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.DTOs.Dawem.Screens;
using Dawem.Models.Response.AdminPanel.Subscriptions.Plans;
using Dawem.Models.Response.Dawem.Screens;

namespace Dawem.Contract.BusinessLogic.Dawem.Screens
{
    public interface IScreenBL
    {
        Task<int> Create(CreateScreenModel model);
        Task<bool> Update(UpdateScreenModel model);
        Task<GetScreenInfoResponseModel> GetInfo(int screenId);
        Task<GetScreenByIdResponseModel> GetById(int screenId);
        Task<GetScreensResponse> Get(GetScreensCriteria model);
        Task<bool> Delete(int screenId);
        Task<bool> Enable(int screenId);
        Task<bool> Disable(DisableModelDTO model);
        Task<GetScreensInformationsResponseDTO> GetScreensInformations();
    }
}
