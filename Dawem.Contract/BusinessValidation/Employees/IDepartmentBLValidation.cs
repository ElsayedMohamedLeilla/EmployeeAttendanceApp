using Dawem.Models.Dtos.Provider;

namespace Dawem.Contract.BusinessValidation.Employees
{
    public interface IDepartmentBLValidation
    {
        Task<bool> CreateValidation(CreateDepartmentModel model);
        Task<bool> UpdateValidation(UpdateDepartmentModel model);
    }
}
