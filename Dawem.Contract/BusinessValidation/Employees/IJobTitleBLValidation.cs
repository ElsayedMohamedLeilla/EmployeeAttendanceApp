using Dawem.Models.Dtos.Employees.JobTitle;

namespace Dawem.Contract.BusinessValidation.Employees
{
    public interface IJobTitleBLValidation
    {
        Task<bool> CreateValidation(CreateJobTitleModel model);
        Task<bool> UpdateValidation(UpdateJobTitleModel model);
    }
}
