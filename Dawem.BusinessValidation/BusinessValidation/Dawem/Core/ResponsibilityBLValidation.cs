using Dawem.Contract.BusinessValidation.Dawem.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Core.Responsibilities;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
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
                Get(responsibility => ((requestInfo.CompanyId > 0 && responsibility.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && responsibility.CompanyId == null)) &&
                responsibility.Name == model.Name &&
                responsibility.Type == requestInfo.AuthenticationType).
                AnyAsync();

            if (checkResponsibilityDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryResponsibilityNameIsDuplicated);
            }

            var checkForemployeesDuplicate = await repositoryManager.
                ResponsibilityRepository.
                Get(responsibility => ((requestInfo.CompanyId > 0 && responsibility.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && responsibility.CompanyId == null)) &&
                responsibility.ForEmployeesApplication &&  model.ForEmployeesApplication &&
                responsibility.Type == requestInfo.AuthenticationType).
                AnyAsync();

            if (checkResponsibilityDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryThereIsAnotherResponsibilityForEmployeesApplicationOnlyOneIsAllowed);
            }

            return true;
        }
        public async Task<bool> UpdateValidation(UpdateResponsibilityModel model)
        {
            var checkResponsibilityDuplicate = await repositoryManager
                .ResponsibilityRepository.
                Get(responsibility => ((requestInfo.CompanyId > 0 && responsibility.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && responsibility.CompanyId == null)) &&
                responsibility.Name == model.Name &&
                responsibility.Type == requestInfo.AuthenticationType && responsibility.Id != model.Id).AnyAsync();
            if (checkResponsibilityDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryResponsibilityNameIsDuplicated);
            }

            var checkForemployeesDuplicate = await repositoryManager.
                ResponsibilityRepository.
                Get(responsibility => ((requestInfo.CompanyId > 0 && responsibility.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && responsibility.CompanyId == null)) &&
                responsibility.ForEmployeesApplication && model.ForEmployeesApplication &&
                responsibility.Type == requestInfo.AuthenticationType && responsibility.Id != model.Id).
                AnyAsync();

            if (checkResponsibilityDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryThereIsAnotherResponsibilityForEmployeesApplicationOnlyOneIsAllowed);
            }

            return true;
        }
    }
}