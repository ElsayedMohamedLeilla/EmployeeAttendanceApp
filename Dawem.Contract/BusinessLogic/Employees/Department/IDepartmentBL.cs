using Dawem.Models.Dtos.Employees.Department;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Response.Employees.Departments;
using Dawem.Models.Response.Requests.Vacations;

namespace Dawem.Contract.BusinessLogic.Employees.Department
{
    public interface IDepartmentBL
    {
        Task<int> Create(CreateDepartmentModel model);
        Task<bool> Update(UpdateDepartmentModel model);
        Task<GetDepartmentInfoResponseModel> GetInfo(int departmentId3);
        Task<GetDepartmentByIdResponseModel> GetById(int departmentId);
        Task<GetDepartmentsResponse> Get(GetDepartmentsCriteria model);
        Task<GetDepartmentsForDropDownResponse> GetForDropDown(GetDepartmentsCriteria model);
        Task<GetDepartmentsForTreeResponse> GetForTree(GetDepartmentsForTreeCriteria model);
        Task<bool> Delete(int departmentId);
        Task<bool> Enable(int departmentId);
        Task<bool> Disable(DisableModelDTO model);
        Task<GetDepartmentsInformationsResponseDTO> GetDepartmentsInformations();
    }
}
