using Dawem.Contract.BusinessValidation.Summons;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Summons.Summons;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Summons
{

    public class SummonBLValidation : ISummonBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public SummonBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> CreateValidation(CreateSummonModel model)
        {
            var checkFingerprintEnforcementDuplicate = await repositoryManager
                .SummonRepository.Get(c => !c.IsDeleted && c.CompanyId == requestInfo.CompanyId
                && c.ForType == model.ForType && c.FingerprintDate.Date == model.FingerprintDate).AnyAsync();
            if (checkFingerprintEnforcementDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorrySummonIsDuplicated);
            }

            return true;
        }
        public async Task<bool> UpdateValidation(UpdateSummonModel model)
        {
            var checkSummonDuplicate = await repositoryManager
                .SummonRepository.Get(c => !c.IsDeleted && c.CompanyId == requestInfo.CompanyId
                && c.ForType == model.ForType && c.FingerprintDate.Date == model.FingerprintDate && c.Id != model.Id).AnyAsync();
            if (checkSummonDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorrySummonIsDuplicated);
            }

            return true;
        }
    }
}
