using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Dawem.Core.Groups;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Response.Dawem.Core.Groups;

namespace Dawem.Contract.BusinessLogic.Dawem.Core
{
    public interface IGroupBL
    {
        Task<int> Create(CreateGroupDTO model);
        Task<bool> Update(UpdateGroupDTO model);
        Task<GetGroupInfoResponseDTO> GetInfo(int GroupId);
        Task<GetGroupByIdResponseDTO> GetById(int GroupId);
        Task<GetGroupResponseDTO> Get(GetGroupCriteria model);
        Task<GetGroupDropDownResponseDTO> GetForDropDown(GetGroupCriteria model);
        Task<bool> Delete(int GroupId);
        Task<bool> Enable(int GroupId);
        Task<bool> Disable(DisableModelDTO model);
        Task<GetGroupsInformationsResponseDTO> GetGroupsInformations();
    }
}
