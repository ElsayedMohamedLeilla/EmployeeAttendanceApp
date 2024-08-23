using Dawem.Contract.BusinessValidation.Dawem.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultVacationsTypes;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Dawem.Core
{
    public class DefaultVacationsTypeBLValidation : IDefaultVacationTypeBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo; // header
        public DefaultVacationsTypeBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }

        public async Task<bool> CreateValidation(CreateDefaultVacationsTypeDTO model)
        {
            var checkVacationsTypeDuplicate = await repositoryManager
                .DefaultVacationTypeRepository.Get(c => c.Name == model.Name && c.LookupType == LookupsType.VacationsTypes).AnyAsync();
            if (checkVacationsTypeDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryVacationsTypeNameIsDuplicated);
            }
            if (model.NameTranslations.Count < 2) // user must enter arabic and english values as minimun
            {
                throw new BusinessValidationException(AmgadKeys.SorryYouMustEnterNameArabicAndEnglishAtLeastForThisVacation);
            }


            return true;
        }


        public async Task<bool> UpdateValidation(UpdateDefaultVacationsTypeDTO model)
        {
            var checkVacationsTypeDuplicate = await repositoryManager
                .DefaultVacationTypeRepository.Get(c =>
                c.Name == model.Name && c.Id != model.Id && c.LookupType == LookupsType.VacationsTypes).AnyAsync();
            if (checkVacationsTypeDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryVacationsTypeNameIsDuplicated);
            }
            if (model.NameTranslations.Count < 2) // user must enter arabic and english values as minimun
            {
                throw new BusinessValidationException(AmgadKeys.SorryYouMustEnterNameArabicAndEnglishAtLeastForThisVacation);
            }

            return true;
        }


    }
}
