using Dawem.Contract.BusinessValidationCore.AdminPanel.Subscriptions;
using Dawem.Contract.Repository.Manager;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;


namespace Dawem.Validation.BusinessValidationCore.AdminPanel.Subscriptions
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

            #region Validate Duplication

            foreach (var nameTranslation in NameTranslations)
            {
                var checkNameDuplicate = await repositoryManager.PlanNameTranslationRepository.
                    Get(pt => nameTranslation.Name == pt.Name &&
                    nameTranslation.LanguageId == pt.LanguageId).
                    Select(l => new
                    {
                        l.Name,
                        LanguageName = l.Language.NativeName
                    }).FirstOrDefaultAsync();

                if (checkNameDuplicate != null)
                {
                    throw new BusinessValidationException(messageCode: null,
                        message: TranslationHelper.GetTranslation(LeillaKeys.SorryPlanNameIsDuplicated, requestInfo.Lang) +
                        LeillaKeys.SpaceThenDashThenSpace + TranslationHelper.GetTranslation(LeillaKeys.DuplicatedPlanName, requestInfo.Lang) + LeillaKeys.ColonsThenSpace +
                        checkNameDuplicate.Name + LeillaKeys.SpaceThenDashThenSpace + TranslationHelper.GetTranslation(LeillaKeys.DuplicatedPlanLanguage, requestInfo.Lang) + LeillaKeys.ColonsThenSpace +
                        checkNameDuplicate.LanguageName);
                }
            }

            #endregion

            return true;
        }

    }
}
