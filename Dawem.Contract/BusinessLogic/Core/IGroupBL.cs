using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Core.Groups;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Response.Core.Groups;

namespace Dawem.Contract.BusinessLogic.Core
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
