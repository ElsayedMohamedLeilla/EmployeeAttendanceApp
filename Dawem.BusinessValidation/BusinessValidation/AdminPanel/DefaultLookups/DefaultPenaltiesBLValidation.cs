using Dawem.Contract.BusinessValidation.AdminPanel.DefaultLookups;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultPenalties;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.AdminPanel.DefaultLookups
{
    public class DefaultPenaltiesBLValidation : IDefaultPenaltiesBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo; // header
        public DefaultPenaltiesBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }

        public async Task<bool> CreateValidation(CreateDefaultPenaltiesDTO model)
        {
            var checkPenaltiesDuplicate = await repositoryManager
                .DefaultPenaltiesRepository.Get(c => c.Name == model.Name && c.LookupType == LookupsType.Penalties).AnyAsync();
            if (checkPenaltiesDuplicate)
            {
                throw new BusinessValidationException(AmgadKeys.SorryPenaltyNameIsDuplicated);
            }
            if (model.NameTranslations.Count < 2) // user must enter arabic and english values as minimun
            {
                throw new BusinessValidationException(AmgadKeys.SorryYouMustEnterNameArabicAndEnglishAtLeastForThisPenaltie);
            }


            return true;
        }


        public async Task<bool> UpdateValidation(UpdateDefaultPenaltiesDTO model)
        {
            var checkPenaltiesDuplicate = await repositoryManager
                .DefaultPenaltiesRepository.Get(c =>
                c.Name == model.Name && c.Id != model.Id && c.LookupType == LookupsType.Penalties).AnyAsync();
            if (checkPenaltiesDuplicate)
            {
                throw new BusinessValidationException(AmgadKeys.SorryPenaltyNameIsDuplicated);
            }
            if (model.NameTranslations.Count < 2) // user must enter arabic and english values as minimun
            {
                throw new BusinessValidationException(AmgadKeys.SorryYouMustEnterNameArabicAndEnglishAtLeastForThisPenaltie);
            }

            return true;
        }


    }
}
