using Dawem.Contract.BusinessValidationCore.Subscriptions;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Shared;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;


namespace Dawem.Validation.BusinessValidationCore.Subscriptions
{

    public class NameTranslationBLValidationCore : INameTranslationBLValidationCore
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public NameTranslationBLValidationCore(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> NameTranslationsValidation(List<NameTranslationModel> NameTranslations)
        {
            var getArabicAndEnglishLanguages = await repositoryManager.LanguageRepository.
                Get(l => l.ISO2 == LeillaKeys.Ar || l.ISO2 == LeillaKeys.En).
                ToListAsync();
            if (getArabicAndEnglishLanguages != null && getArabicAndEnglishLanguages.Count == 2)
            {
                var languagesIds = getArabicAndEnglishLanguages.Select(l => l.Id).ToList();
                var checkArAndEnCount = NameTranslations.Where(nt => languagesIds.Contains(nt.LanguageId)).Count() == 2;

                if (!checkArAndEnCount)
                    throw new BusinessValidationException(LeillaKeys.SorryYouMustEnterNamesInArabicAndEnglishLanguages);

            }

            return true;
        }

    }
}
