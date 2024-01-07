using Dawem.Contract.BusinessValidation.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.FingerprintEnforcements.NonComplianceActions;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.FingerprintEnforcements
{
    public class NonComplianceActionBLValidation : INonComplianceActionBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public NonComplianceActionBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> CreateValidation(CreateNonComplianceActionModel model)
        {
            var checkNonComplianceActionDuplicate = await repositoryManager
                .NonComplianceActionRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.Name == model.Name).AnyAsync();
            if (checkNonComplianceActionDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryNonComplianceActionNameIsDuplicated);
            }

            return true;
        }
        public async Task<bool> UpdateValidation(UpdateNonComplianceActionModel model)
        {
            var checkNonComplianceActionDuplicate = await repositoryManager
                .NonComplianceActionRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
                c.Name == model.Name && c.Id != model.Id).AnyAsync();
            if (checkNonComplianceActionDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryNonComplianceActionNameIsDuplicated);
            }

            return true;
        }
    }
}