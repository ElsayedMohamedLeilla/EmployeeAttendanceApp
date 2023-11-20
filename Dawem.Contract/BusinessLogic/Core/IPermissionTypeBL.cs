using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Core.PermissionsTypes;
using Dawem.Models.Response.Core.PermissionsTypes;

namespace Dawem.Contract.BusinessLogic.Core
{
    public interface IPermissionTypeBL
    {
        Task<int> Create(CreatePermissionTypeDTO model);
        Task<bool> Update(UpdatePermissionTypeDTO model);
        Task<GetPermissionsTypeInfoResponseDTO> GetInfo(int PermissionsTypeId);
        Task<GetPermissionsTypeByIdResponseDTO> GetById(int PermissionsTypeId);
        Task<GetPermissionsTypeResponseDTO> Get(GetPermissionsTypesCriteria model);
        Task<GetPermissionsTypeDropDownResponseDTO> GetForDropDown(GetPermissionsTypesCriteria model);
        Task<bool> Delete(int PermissionsTypeId);
    }
}
