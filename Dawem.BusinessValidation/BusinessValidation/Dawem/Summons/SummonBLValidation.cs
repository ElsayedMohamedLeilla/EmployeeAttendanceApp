using Dawem.Contract.BusinessValidation.Dawem.Summons;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Summons.Summons;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Dawem.Summons
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
                && c.ForType == model.ForType && c.LocalDateAndTime == model.LocalDateAndTime).AnyAsync();
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
                && c.ForType == model.ForType && c.LocalDateAndTime == model.LocalDateAndTime && c.Id != model.Id).AnyAsync();
            if (checkSummonDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorrySummonIsDuplicated);
            }
            var utcDateTime = DateTime.UtcNow;

            var checkIfStarted = await repositoryManager
                .SummonRepository.Get(c => !c.IsDeleted && c.CompanyId == requestInfo.CompanyId
                && c.Id == model.Id
                && utcDateTime >= c.StartDateAndTimeUTC).AnyAsync();
            if (checkIfStarted)
            {
                throw new BusinessValidationException(LeillaKeys.SorryEditSummonNotAllowedAfterSummonStarted);
            }

            return true;
        }
        public async Task<bool> DeleteValidation(int summonId)
        {
            var utcDateTime = DateTime.UtcNow;

            var checkIfStarted = await repositoryManager
                .SummonRepository.Get(c => !c.IsDeleted && c.CompanyId == requestInfo.CompanyId
                 && c.Id == summonId
                && utcDateTime >= c.StartDateAndTimeUTC).AnyAsync();

            if (checkIfStarted)
            {
                throw new BusinessValidationException(LeillaKeys.SorryDeleteSummonNotAllowedAfterSummonStarted);
            }

            return true;
        }
        public async Task<bool> EnableValidation(int summonId)
        {
            var utcDateTime = DateTime.UtcNow;

            var checkIfStarted = await repositoryManager
                .SummonRepository.Get(c => !c.IsDeleted && c.CompanyId == requestInfo.CompanyId
                 && c.Id == summonId
                && utcDateTime >= c.StartDateAndTimeUTC).AnyAsync();

            if (checkIfStarted)
            {
                throw new BusinessValidationException(LeillaKeys.SorryEnableSummonNotAllowedAfterSummonStarted);
            }

            return true;
        }
        public async Task<bool> DisableValidation(int summonId)
        {
            var utcDateTime = DateTime.UtcNow;

            var checkIfStarted = await repositoryManager
                .SummonRepository.Get(c => !c.IsDeleted && c.CompanyId == requestInfo.CompanyId
                 && c.Id == summonId
                && utcDateTime >= c.StartDateAndTimeUTC).AnyAsync();

            if (checkIfStarted)
            {
                throw new BusinessValidationException(LeillaKeys.SorryDisableSummonNotAllowedAfterSummonStarted);
            }

            return true;
        }
    }
}
