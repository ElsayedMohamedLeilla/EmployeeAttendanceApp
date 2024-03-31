using Dawem.Contract.BusinessValidation.Dawem.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Core.Responsibilities;
using Dawem.Models.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Dawem.Core
{
    public class ResponsibilityBLValidation : IResponsibilityBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public ResponsibilityBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> CreateValidation(CreateResponsibilityModel model)
        {
            var checkResponsibilityDuplicate = await repositoryManager.
                ResponsibilityRepository.
                Get(c => ((requestInfo.IsAdminPanel && c.CompanyId == null) || c.CompanyId == requestInfo.CompanyId) &&
                c.Name == model.Name &&
                c.IsForAdminPanel == requestInfo.IsAdminPanel).
                AnyAsync();

            if (checkResponsibilityDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryResponsibilityNameIsDuplicated);
            }

            return true;
        }
        public async Task<bool> UpdateValidation(UpdateResponsibilityModel model)
        {
            var checkResponsibilityDuplicate = await repositoryManager
                .ResponsibilityRepository.
                Get(c => ((requestInfo.IsAdminPanel && c.CompanyId == null) || c.CompanyId == requestInfo.CompanyId) &&
                c.Name == model.Name &&
                c.IsForAdminPanel == requestInfo.IsAdminPanel && c.Id != model.Id).AnyAsync();
            if (checkResponsibilityDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryResponsibilityNameIsDuplicated);
            }

            return true;
        }
    }
}