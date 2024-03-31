using Dawem.Contract.BusinessValidation.Dawem.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Core.JustificationsTypes;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Dawem.Core
{
    public class JustificationsTypeBLValidation : IJustificationTypeBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo; // header
        public JustificationsTypeBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }

        public async Task<bool> CreateValidation(CreateJustificationsTypeDTO model)
        {
            var checkJustificationsTypeDuplicate = await repositoryManager
                .JustificationsTypeRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.Name == model.Name).AnyAsync();
            if (checkJustificationsTypeDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryJustificationsTypeNameIsDuplicated);
            }

            return true;
        }


        public async Task<bool> UpdateValidation(UpdateJustificationsTypeDTO model)
        {
            var checkJustificationsTypeDuplicate = await repositoryManager
                .JustificationsTypeRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
                c.Name == model.Name && c.Id != model.Id).AnyAsync();
            if (checkJustificationsTypeDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryJustificationsTypeNameIsDuplicated);
            }

            return true;
        }


    }
}
