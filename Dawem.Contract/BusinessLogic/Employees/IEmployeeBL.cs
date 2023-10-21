using Dawem.Models.Dtos.Identity;
using Dawem.Models.Dtos.Provider;
using Dawem.Models.Response.Employees;

namespace Dawem.Contract.BusinessLogic.Provider
{
    public interface IEmployeeBL
    {
        Task<int> Create(CreateEmployeeModel model);
        Task<bool> Update(UpdateEmployeeModel model);
        Task<GetEmployeeInfoResponseModel> GetInfo(int employeeId);
        Task<GetEmployeeByIdResponseModel> GetById(int employeeId);
        Task<GetEmployeesResponse> Get(GetEmployeesCriteria model);
        Task<GetEmployeesForDropDownResponse> GetForDropDown(GetEmployeesCriteria model);
        Task<bool> Delete(int employeeId);
    }
}
