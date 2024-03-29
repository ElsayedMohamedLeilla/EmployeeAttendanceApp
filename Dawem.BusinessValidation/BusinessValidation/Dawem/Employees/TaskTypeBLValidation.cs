using Dawem.Contract.BusinessValidation.Dawem.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Employees.TaskTypes;
using Dawem.Models.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Dawem.Employees
{

    public class TaskTypeBLValidation : ITaskTypeBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public TaskTypeBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> CreateValidation(CreateTaskTypeModel model)
        {
            var checkTaskTypeDuplicate = await repositoryManager
                .TaskTypeRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.Name == model.Name).AnyAsync();
            if (checkTaskTypeDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryTaskTypeNameIsDuplicated);
            }

            return true;
        }
        public async Task<bool> UpdateValidation(UpdateTaskTypeModel model)
        {
            var checkTaskTypeDuplicate = await repositoryManager
                .TaskTypeRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
                c.Name == model.Name && c.Id != model.Id).AnyAsync();
            if (checkTaskTypeDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryTaskTypeNameIsDuplicated);
            }

            return true;
        }
    }
}
