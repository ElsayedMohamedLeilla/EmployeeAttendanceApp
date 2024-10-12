using Dawem.Contract.BusinessValidation.AdminPanel.DefaultLookups;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultOfficialHolidaysTypes;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.AdminPanel.DefaultLookups
{
    public class DefaultOfficialHolidaysBLValidation : IDefaultOfficialHolidayBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo; // header
        public DefaultOfficialHolidaysBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }

        public async Task<bool> CreateValidation(CreateDefaultOfficialHolidaysDTO model)
        {
            var checkOfficialHolidaysTypeDuplicate = await repositoryManager
                .DefaultOfficialHolidayRepository.Get(c => c.Name == model.Name && c.LookupType == LookupsType.OfficialHoliday).AnyAsync();
            if (checkOfficialHolidaysTypeDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryHolidayTypeNameIsDuplicated);
            }
            if (model.NameTranslations.Count < 2) // user must enter arabic and english values as minimun
            {
                throw new BusinessValidationException(AmgadKeys.SorryYouMustEnterNameArabicAndEnglishAtLeastForThisHoliday);
            }


            return true;
        }


        public async Task<bool> UpdateValidation(UpdateDefaultOfficialHolidaysDTO model)
        {
            var checkOfficialHolidaysTypeDuplicate = await repositoryManager
                .DefaultOfficialHolidayRepository.Get(c =>
                c.Name == model.Name && c.Id != model.Id && c.LookupType == LookupsType.OfficialHoliday).AnyAsync();
            if (checkOfficialHolidaysTypeDuplicate)
            {
                throw new BusinessValidationException(AmgadKeys.SorryHolidayNameIsDuplicated);
            }
            if (model.NameTranslations.Count < 2) // user must enter arabic and english values as minimun
            {
                throw new BusinessValidationException(AmgadKeys.SorryYouMustEnterNameArabicAndEnglishAtLeastForThisHoliday);
            }

            return true;
        }


    }
}
