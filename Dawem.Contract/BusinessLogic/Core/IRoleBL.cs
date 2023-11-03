using Dawem.Models.Criteria.Core;
using Dawem.Models.Response.Core.Roles;

namespace Dawem.Contract.BusinessLogic.Core
{
    public interface IRoleBL
    {
        Task<GetRoleDropDownResponseDTO> GetForDropDown(GetRoleCriteria model);
    }
}
