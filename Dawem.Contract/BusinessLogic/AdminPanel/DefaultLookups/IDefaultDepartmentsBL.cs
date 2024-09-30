using Dawem.Models.Criteria.DefaultLookups;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultDepartments;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultDepartments;

namespace Dawem.Contract.BusinessLogic.AdminPanel.DefaultLookups
{
    public interface IDefaultDepartmentsBL
    {
        Task<int> Create(CreateDefaultDepartmentsDTO model);
        Task<bool> Update(UpdateDefaultDepartmentsDTO model);
        Task<GetDefaultDepartmentsInfoResponseDTO> GetInfo(int DefaultDepartmentsId);
        Task<GetDefaultDepartmentsByIdResponseDTO> GetById(int DefaultDepartmentsId);
        Task<GetDefaultDepartmentsResponseDTO> Get(GetDefaultDepartmentsCriteria model);
        Task<GetDefaultDepartmentsDropDownResponseDTO> GetForDropDown(GetDefaultDepartmentsCriteria model);
        Task<bool> Delete(int DefaultDepartmentsId);
        public Task<bool> Enable(int GroupId);
        public Task<bool> Disable(DisableModelDTO model);


    }
}
