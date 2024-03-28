using Dawem.Models.Criteria.Core;
using Dawem.Models.Response.Core.Roles;

namespace Dawem.Contract.BusinessLogic.Dawem.Core
{
    public interface IRoleBL
    {
        Task<GetRoleDropDownResponseDTO> GetForDropDown(GetRolesCriteria model);
    }
}
