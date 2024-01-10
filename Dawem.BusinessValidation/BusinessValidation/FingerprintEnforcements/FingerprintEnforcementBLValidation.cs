using Dawem.Contract.BusinessValidation.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.FingerprintEnforcements.FingerprintEnforcements;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.FingerprintEnforcements
{

    public class FingerprintEnforcementBLValidation : IFingerprintEnforcementBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public FingerprintEnforcementBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> CreateValidation(CreateFingerprintEnforcementModel model)
        {
            var checkFingerprintEnforcementDuplicate = await repositoryManager
                .FingerprintEnforcementRepository.Get(c => !c.IsDeleted && c.CompanyId == requestInfo.CompanyId
                && c.ForType == model.ForType && c.FingerprintDate.Date == model.FingerprintDate).AnyAsync();
            if (checkFingerprintEnforcementDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryFingerprintEnforcementIsDuplicated);
            }

            return true;
        }
        public async Task<bool> UpdateValidation(UpdateFingerprintEnforcementModel model)
        {
            var checkFingerprintEnforcementDuplicate = await repositoryManager
                .FingerprintEnforcementRepository.Get(c => !c.IsDeleted && c.CompanyId == requestInfo.CompanyId
                && c.ForType == model.ForType && c.FingerprintDate.Date == model.FingerprintDate && c.Id != model.Id).AnyAsync();
            if (checkFingerprintEnforcementDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryFingerprintEnforcementIsDuplicated);
            }

            return true;
        }
    }
}
