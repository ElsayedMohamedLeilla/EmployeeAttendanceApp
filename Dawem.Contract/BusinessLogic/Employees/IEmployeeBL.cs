using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Response.Employees.Employee;

namespace Dawem.Contract.BusinessLogic.Employees
{
    public interface IEmployeeBL
    {
        Task<int> Create(CreateEmployeeModel model);
        Task<bool> Update(UpdateEmployeeModel model);
        Task<GetEmployeeInfoResponseModel> GetInfo(int employeeId);
        Task<GetCurrentEmployeeInfoResponseModel> GetCurrentEmployeeInfo();
        Task<GetEmployeeByIdResponseModel> GetById(int employeeId);
        Task<GetEmployeesResponse> Get(GetEmployeesCriteria model);
        Task<GetEmployeesForDropDownResponse> GetForDropDown(GetEmployeesCriteria model);
        Task<bool> Disable(DisableModelDTO model);
        Task<bool> Enable(int employeeId);
        Task<bool> Delete(int employeeId);
    }
}
