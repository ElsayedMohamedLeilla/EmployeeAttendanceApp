using Dawem.Models.Dtos.Employees.Departments;

namespace Dawem.Contract.BusinessValidation.Employees
{
    public interface IDepartmentBLValidation
    {
        Task<bool> CreateValidation(CreateDepartmentModel model);
        Task<bool> UpdateValidation(UpdateDepartmentModel model);
    }
}
