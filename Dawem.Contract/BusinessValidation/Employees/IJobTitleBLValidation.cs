using Dawem.Models.Dtos.Employees.JobTitles;

namespace Dawem.Contract.BusinessValidation.Employees
{
    public interface IJobTitleBLValidation
    {
        Task<bool> CreateValidation(CreateJobTitleModel model);
        Task<bool> UpdateValidation(UpdateJobTitleModel model);
    }
}
