using Dawem.Contract.BusinessValidation.AdminPanel.DefaultLookups;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultJustificationsTypes;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.AdminPanel.DefaultLookups
{
    public class DefaultJustificationsTypeBLValidation : IDefaultJustificationTypeBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo; // header
        public DefaultJustificationsTypeBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }

        public async Task<bool> CreateValidation(CreateDefaultJustificationsTypeDTO model)
        {
            var checkJustificationsTypeDuplicate = await repositoryManager
                .DefaultJustificationTypeRepository.Get(c => c.Name == model.Name && c.LookupType == LookupsType.JustificationsTypes).AnyAsync();
            if (checkJustificationsTypeDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryJustificationsTypeNameIsDuplicated);
            }
            if (model.NameTranslations.Count < 2) // user must enter arabic and english values as minimun
            {
                throw new BusinessValidationException(AmgadKeys.SorryYouMustEnterNameArabicAndEnglishAtLeastForThisJustification);
            }


            return true;
        }


        public async Task<bool> UpdateValidation(UpdateDefaultJustificationsTypeDTO model)
        {
            var checkJustificationsTypeDuplicate = await repositoryManager
                .DefaultJustificationTypeRepository.Get(c =>
                c.Name == model.Name && c.Id != model.Id && c.LookupType == LookupsType.JustificationsTypes).AnyAsync();
            if (checkJustificationsTypeDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryJustificationsTypeNameIsDuplicated);
            }
            if (model.NameTranslations.Count < 2) // user must enter arabic and english values as minimun
            {
                throw new BusinessValidationException(AmgadKeys.SorryYouMustEnterNameArabicAndEnglishAtLeastForThisJustification);
            }

            return true;
        }


    }
}
