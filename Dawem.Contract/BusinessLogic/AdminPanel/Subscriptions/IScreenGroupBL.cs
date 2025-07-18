﻿using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.DTOs.Dawem.Screens.ScreenGroups;
using Dawem.Models.DTOs.Dawem.Screens.Screens;
using Dawem.Models.Response.AdminPanel.Screens.ScreenGroups;

namespace Dawem.Contract.BusinessLogic.AdminPanel.Subscriptions
{
    public interface IScreenGroupBL
    {
        Task<int> Create(CreateScreenGroupModel model);
        Task<bool> Update(UpdateScreenGroupModel model);
        Task<GetScreenGroupInfoResponseModel> GetInfo(int screenGroupId);
        Task<GetScreenGroupByIdResponseModel> GetById(int screenGroupId);
        Task<GetScreenGroupsResponse> Get(GetScreensCriteria model);
        Task<GetScreenGroupsForDropDownResponse> GetForDropDown(GetScreensCriteria model);
        Task<bool> Delete(int screenGroupId);
        Task<bool> Enable(int screenGroupId);
        Task<bool> Disable(DisableModelDTO model);
        Task<GetScreenGroupsInformationsResponseDTO> GetScreenGroupsInformations();
    }
}
