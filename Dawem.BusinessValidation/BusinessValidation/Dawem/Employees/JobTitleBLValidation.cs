using Dawem.Contract.BusinessValidation.Dawem.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Employees.JobTitles;
using Dawem.Models.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Dawem.Employees
{

    public class JobTitleBLValidation : IJobTitleBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public JobTitleBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> CreateValidation(CreateJobTitleModel model)
        {
            var checkJobTitleDuplicate = await repositoryManager
                .JobTitleRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.Name == model.Name).AnyAsync();
            if (checkJobTitleDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryJobTitleNameIsDuplicated);
            }

            return true;
        }
        public async Task<bool> UpdateValidation(UpdateJobTitleModel model)
        {
            var checkJobTitleDuplicate = await repositoryManager
                .JobTitleRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
                c.Name == model.Name && c.Id != model.Id).AnyAsync();
            if (checkJobTitleDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryJobTitleNameIsDuplicated);
            }

            return true;
        }
    }
}
