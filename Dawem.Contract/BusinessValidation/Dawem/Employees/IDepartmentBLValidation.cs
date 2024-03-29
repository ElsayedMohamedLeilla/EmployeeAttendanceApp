using Dawem.Models.Dtos.Dawem.Employees.Departments;

namespace Dawem.Contract.BusinessValidation.Dawem.Employees
{
    public interface IDepartmentBLValidation
    {
        Task<bool> CreateValidation(CreateDepartmentModel model);
        Task<bool> UpdateValidation(UpdateDepartmentModel model);
    }
}
