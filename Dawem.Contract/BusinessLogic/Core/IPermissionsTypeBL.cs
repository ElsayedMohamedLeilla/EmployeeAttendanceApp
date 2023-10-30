using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Core.PermissionsTypes;
using Dawem.Models.Response.Core.PermissionsTypes;

namespace Dawem.Contract.BusinessLogic.Core
{
    public interface IPermissionsTypeBL
    {
        Task<int> Create(CreatePermissionsTypeDTO model);
        Task<bool> Update(UpdatePermissionsTypeDTO model);
        Task<GetPermissionsTypeInfoResponseDTO> GetInfo(int PermissionsTypeId);
        Task<GetPermissionsTypeByIdResponseDTO> GetById(int PermissionsTypeId);
        Task<GetPermissionsTypeResponseDTO> Get(GetPermissionsTypeCriteria model);
        Task<GetPermissionsTypeDropDownResponseDTO> GetForDropDown(GetPermissionsTypeCriteria model);
        Task<bool> Delete(int PermissionsTypeId);
    }
}
