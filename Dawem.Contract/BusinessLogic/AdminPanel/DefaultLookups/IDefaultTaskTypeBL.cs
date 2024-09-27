using Dawem.Models.Criteria.DefaultLookups;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultTasksTypes;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultTasksTypes;

namespace Dawem.Contract.BusinessLogic.AdminPanel.DefaultLookups
{
    public interface IDefaultTaskTypeBL
    {
        Task<int> Create(CreateDefaultTasksTypeDTO model);
        Task<bool> Update(UpdateDefaultTasksTypeDTO model);
        Task<GetDefaultTasksTypeInfoResponseDTO> GetInfo(int DefaultTasksTypeId);
        Task<GetDefaultTasksTypeByIdResponseDTO> GetById(int DefaultTasksTypeId);
        Task<GetDefaultTasksTypeResponseDTO> Get(GetDefaultTaskTypeCriteria model);
        Task<GetDefaultTasksTypeDropDownResponseDTO> GetForDropDown(GetDefaultTaskTypeCriteria model);
        Task<bool> Delete(int DefaultTasksTypeId);

        public Task<bool> Enable(int GroupId);

        public Task<bool> Disable(DisableModelDTO model);


    }
}
