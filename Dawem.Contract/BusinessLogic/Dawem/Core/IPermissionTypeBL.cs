using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Dawem.Core.PermissionsTypes;
using Dawem.Models.Response.Dawem.Core.PermissionsTypes;

namespace Dawem.Contract.BusinessLogic.Dawem.Core
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
        Task<GetPermissionsTypesInformationsResponseDTO> GetPermissionTypesInformations();
    }
}
