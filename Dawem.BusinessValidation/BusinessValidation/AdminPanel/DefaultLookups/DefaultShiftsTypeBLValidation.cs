using Dawem.Contract.BusinessValidation.AdminPanel.DefaultLookups;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultShiftsTypes;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.AdminPanel.DefaultLookups
{
    public class DefaultShiftsTypeBLValidation : IDefaultShiftTypeBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo; // header
        public DefaultShiftsTypeBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }

        public async Task<bool> CreateValidation(CreateDefaultShiftsTypeDTO model)
        {
            var checkShiftsTypeDuplicate = await repositoryManager
                .DefaultShiftTypeRepository.Get(c => c.Name == model.Name && c.LookupType == LookupsType.ShiftsTypes).AnyAsync();
            if (checkShiftsTypeDuplicate)
            {
                throw new BusinessValidationException(AmgadKeys.SorryShiftsTypeNameIsDuplicated);
            }
            if (model.NameTranslations.Count < 2) // user must enter arabic and english values as minimun
            {
                throw new BusinessValidationException(AmgadKeys.SorryYouMustEnterNameArabicAndEnglishAtLeastForThisShift);
            }


            return true;
        }


        public async Task<bool> UpdateValidation(UpdateDefaultShiftsTypeDTO model)
        {
            var checkShiftsTypeDuplicate = await repositoryManager
                .DefaultShiftTypeRepository.Get(c =>
                c.Name == model.Name && c.Id != model.Id && c.LookupType == LookupsType.ShiftsTypes).AnyAsync();
            if (checkShiftsTypeDuplicate)
            {
                throw new BusinessValidationException(AmgadKeys.SorryShiftsTypeNameIsDuplicated);
            }
            if (model.NameTranslations.Count < 2) // user must enter arabic and english values as minimun
            {
                throw new BusinessValidationException(AmgadKeys.SorryYouMustEnterNameArabicAndEnglishAtLeastForThisShift);
            }

            return true;
        }


    }
}
