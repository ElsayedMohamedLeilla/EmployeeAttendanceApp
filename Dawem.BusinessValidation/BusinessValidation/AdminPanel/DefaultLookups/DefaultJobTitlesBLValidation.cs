using Dawem.Contract.BusinessValidation.AdminPanel.DefaultLookups;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultJobTitles;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.AdminPanel.DefaultLookups
{
    public class DefaultJobTitlesBLValidation : IDefaultJobTitlesBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo; // header
        public DefaultJobTitlesBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }

        public async Task<bool> CreateValidation(CreateDefaultJobTitlesDTO model)
        {
            var checkJobTitlesDuplicate = await repositoryManager
                .DefaultJobTitlesRepository.Get(c => c.Name == model.Name && c.LookupType == LookupsType.JobTitles).AnyAsync();
            if (checkJobTitlesDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryJobTitleNameIsDuplicated);
            }
            if (model.NameTranslations.Count < 2) // user must enter arabic and english values as minimun
            {
                throw new BusinessValidationException(AmgadKeys.SorryYouMustEnterNameArabicAndEnglishAtLeastForThisJobTitle);
            }


            return true;
        }


        public async Task<bool> UpdateValidation(UpdateDefaultJobTitlesDTO model)
        {
            var checkJobTitlesDuplicate = await repositoryManager
                .DefaultJobTitlesRepository.Get(c =>
                c.Name == model.Name && c.Id != model.Id && c.LookupType == LookupsType.JobTitles).AnyAsync();
            if (checkJobTitlesDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryJobTitleNameIsDuplicated);
            }
            if (model.NameTranslations.Count < 2) // user must enter arabic and english values as minimun
            {
                throw new BusinessValidationException(AmgadKeys.SorryYouMustEnterNameArabicAndEnglishAtLeastForThisJobTitle);
            }

            return true;
        }


    }
}
