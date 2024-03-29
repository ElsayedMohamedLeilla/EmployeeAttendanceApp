using Dawem.Models.Dtos.Dawem.Employees.JobTitles;

namespace Dawem.Contract.BusinessValidation.Dawem.Employees
{
    public interface IJobTitleBLValidation
    {
        Task<bool> CreateValidation(CreateJobTitleModel model);
        Task<bool> UpdateValidation(UpdateJobTitleModel model);
    }
}
