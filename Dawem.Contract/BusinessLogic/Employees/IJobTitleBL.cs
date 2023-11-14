using Dawem.Models.Dtos.Employees.JobTitle;
using Dawem.Models.Response.Employees.JobTitles;

namespace Dawem.Contract.BusinessLogic.Employees
{
    public interface IJobTitleBL
    {
        Task<int> Create(CreateJobTitleModel model);
        Task<bool> Update(UpdateJobTitleModel model);
        Task<GetJobTitleInfoResponseModel> GetInfo(int jobTitleId);
        Task<GetJobTitleByIdResponseModel> GetById(int jobTitleId);
        Task<GetJobTitlesResponse> Get(GetJobTitlesCriteria model);
        Task<GetJobTitlesForDropDownResponse> GetForDropDown(GetJobTitlesCriteria model);
        Task<bool> Delete(int jobTitleId);
    }
}
