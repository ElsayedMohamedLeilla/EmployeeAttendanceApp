using Dawem.Models.Dtos.Employees.Departments;

namespace Dawem.Contract.BusinessValidation.Employees
{
    public interface IPlanBLValidation
    {
        Task<bool> CreateValidation(CreatePlanModel model);
        Task<bool> UpdateValidation(UpdatePlanModel model);
    }
}
