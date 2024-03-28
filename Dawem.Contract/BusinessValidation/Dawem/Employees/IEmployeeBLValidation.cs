using Dawem.Models.Dtos.Dawem.Employees.Employees;

namespace Dawem.Contract.BusinessValidation.Dawem.Employees
{
    public interface IEmployeeBLValidation
    {
        Task<bool> CreateValidation(CreateEmployeeModel model);
        Task<bool> UpdateValidation(UpdateEmployeeModel model);
    }
}
