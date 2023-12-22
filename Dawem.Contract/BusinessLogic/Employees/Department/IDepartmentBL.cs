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
        Task<GetDepartmentInfoResponseModel> GetInfo(int DepartmentId);
        Task<GetDepartmentByIdResponseModel> GetById(int DepartmentId);
        Task<GetDepartmentsResponse> Get(GetDepartmentsCriteria model);
        Task<GetDepartmentsForDropDownResponse> GetForDropDown(GetDepartmentsCriteria model);
        Task<GetDepartmentsForTreeResponse> GetForTree(GetDepartmentsForTreeCriteria model);
        Task<bool> Delete(int DepartmentId);
        Task<bool> Enable(int GroupId);
        Task<bool> Disable(DisableModelDTO model);
        Task<GetDepartmentsInformationsResponseDTO> GetDepartmentsInformations();
    }
}
