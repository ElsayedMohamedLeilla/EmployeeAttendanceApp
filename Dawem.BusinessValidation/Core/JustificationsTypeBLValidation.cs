using Dawem.Contract.BusinessValidation.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Core.JustificationsTypes;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.Core
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
                throw new BusinessValidationException(DawemKeys.SorryJustificationsTypeNameIsDuplicated);
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
                throw new BusinessValidationException(DawemKeys.SorryJustificationsTypeNameIsDuplicated);
            }

            return true;
        }


    }
}
