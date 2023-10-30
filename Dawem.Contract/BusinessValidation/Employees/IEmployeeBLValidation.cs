using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Contract.BusinessValidation.Employees
{
    public interface IEmployeeBLValidation
    {
        Task<bool> CreateValidation(CreateEmployeeModel model);
        Task<bool> UpdateValidation(UpdateEmployeeModel model);
    }
}
