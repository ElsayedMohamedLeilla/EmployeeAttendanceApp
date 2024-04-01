using Dawem.Contract.BusinessValidation.Dawem.Summons;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Summons.Sanctions;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Dawem.Summons
{
    public class SanctionBLValidation : ISanctionBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public SanctionBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> CreateValidation(CreateSanctionModel model)
        {
            var checkSanctionDuplicate = await repositoryManager
                .SanctionRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.Name == model.Name).AnyAsync();
            if (checkSanctionDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorrySanctionNameIsDuplicated);
            }

            return true;
        }
        public async Task<bool> UpdateValidation(UpdateSanctionModel model)
        {
            var checkSanctionDuplicate = await repositoryManager
                .SanctionRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
                c.Name == model.Name && c.Id != model.Id).AnyAsync();
            if (checkSanctionDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorrySanctionNameIsDuplicated);
            }

            return true;
        }
    }
}