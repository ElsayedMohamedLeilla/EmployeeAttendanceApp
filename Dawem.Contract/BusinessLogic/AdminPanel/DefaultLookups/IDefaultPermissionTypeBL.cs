using Dawem.Models.Criteria.DefaultLookups;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultPermissionsTypes;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultPermissionsTypes;

namespace Dawem.Contract.BusinessLogic.AdminPanel.DefaultLookups
{
    public interface IDefaultPermissionTypeBL
    {
        Task<int> Create(CreateDefaultPermissionsTypeDTO model);
        Task<bool> Update(UpdateDefaultPermissionsTypeDTO model);
        Task<GetDefaultPermissionsTypeInfoResponseDTO> GetInfo(int DefaultPermissionsTypeId);
        Task<GetDefaultPermissionsTypeByIdResponseDTO> GetById(int DefaultPermissionsTypeId);
        Task<GetDefaultPermissionsTypeResponseDTO> Get(GetDefaultPermissionTypeCriteria model);
        Task<GetDefaultPermissionsTypeDropDownResponseDTO> GetForDropDown(GetDefaultPermissionTypeCriteria model);
        Task<bool> Delete(int DefaultPermissionsTypeId);

        public Task<bool> Enable(int GroupId);

        public Task<bool> Disable(DisableModelDTO model);


    }
}
