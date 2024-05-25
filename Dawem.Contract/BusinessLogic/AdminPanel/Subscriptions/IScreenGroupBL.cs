using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.DTOs.Dawem.Screens.ScreenGroups;
using Dawem.Models.Response.AdminPanel.Subscriptions.Screens;

namespace Dawem.Contract.BusinessLogic.AdminPanel.Subscriptions
{
    public interface IScreenGroupBL
    {
        Task<int> Create(CreateScreenGroupModel model);
        Task<bool> Update(UpdateScreenGroupModel model);
        Task<GetScreenGroupInfoResponseModel> GetInfo(int screenGroupId);
        Task<GetScreenGroupByIdResponseModel> GetById(int screenGroupId);
        Task<GetScreenGroupsResponse> Get(GetScreenGroupsCriteria model);
        Task<GetScreenGroupsForDropDownResponse> GetForDropDown(GetScreenGroupsCriteria model);
        Task<bool> Delete(int screenGroupId);
        Task<bool> Enable(int screenGroupId);
        Task<bool> Disable(DisableModelDTO model);
        Task<GetScreenGroupsInformationsResponseDTO> GetScreenGroupsInformations();
    }
}
