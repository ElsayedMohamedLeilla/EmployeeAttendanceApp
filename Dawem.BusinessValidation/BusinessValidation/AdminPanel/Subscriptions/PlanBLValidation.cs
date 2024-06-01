using Dawem.Contract.BusinessValidation.AdminPanel.Subscriptions;
using Dawem.Contract.BusinessValidationCore.AdminPanel.Subscriptions;
using Dawem.Contract.Repository.Manager;
using Dawem.Domain.Entities;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Models.Dtos.Dawem.Subscriptions.Plans;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;


namespace Dawem.Validation.BusinessValidation.AdminPanel.Subscriptions
{

    public class PlanBLValidationBLValidation : IPlanBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        private readonly INameTranslationBLValidationCore nameTranslationBLValidationCore;
        public PlanBLValidationBLValidation(IRepositoryManager _repositoryManager,
            RequestInfo _requestInfo,
            INameTranslationBLValidationCore _nameTranslationBLValidationCore)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
            nameTranslationBLValidationCore = _nameTranslationBLValidationCore;
        }
        public async Task<bool> CreateValidation(CreatePlanModel model)
        {

            var checkPlanOverlap = await repositoryManager
                .PlanRepository.Get(c => !c.IsDeleted && (model.MinNumberOfEmployees >= c.MinNumberOfEmployees &&
                model.MinNumberOfEmployees <= c.MaxNumberOfEmployees || model.MaxNumberOfEmployees >= c.MinNumberOfEmployees &&
                model.MaxNumberOfEmployees <= c.MaxNumberOfEmployees)).AnyAsync();
            if (checkPlanOverlap)
            {
                throw new BusinessValidationException(LeillaKeys.SorryPlanOverlapWithOtherPlanInNumberOfEmployees);
            }

            #region Check Trial

            if (model.IsTrial)
            {
                var checkTrial = await repositoryManager.
                PlanRepository.Get(c => !c.IsDeleted && c.IsTrial).
                AnyAsync();
                if (checkTrial)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryThereIsAlreadyTrialPlanYouMustAddOneTrialPlan);
                }
            }

            #endregion

            #region Validate Name Translations

            await nameTranslationBLValidationCore.NameTranslationsValidation(model.NameTranslations);

            #region Validate Duplication

            await ValidatePlanNameDuplication(model.NameTranslations);

            #endregion

            #endregion

            return true;
        }
        public async Task<bool> UpdateValidation(UpdatePlanModel model)
        {
            var checkPlanOverlap = await repositoryManager
                .PlanRepository.Get(c => !c.IsDeleted && (model.MinNumberOfEmployees >= c.MinNumberOfEmployees &&
                model.MinNumberOfEmployees <= c.MaxNumberOfEmployees || model.MaxNumberOfEmployees >= c.MinNumberOfEmployees &&
                model.MaxNumberOfEmployees <= c.MaxNumberOfEmployees) &&
                 c.Id != model.Id).AnyAsync();
            if (checkPlanOverlap)
            {
                throw new BusinessValidationException(LeillaKeys.SorryPlanOverlapWithOtherPlanInNumberOfEmployees);
            }

            #region Check Trial

            if (model.IsTrial)
            {
                var checkTrial = await repositoryManager.
                PlanRepository.Get(c => !c.IsDeleted && c.IsTrial && c.Id != model.Id).
                AnyAsync();
                if (checkTrial)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryThereIsAlreadyTrialPlanYouMustAddOneTrialPlan);
                }
            }

            #endregion

            #region Validate Name Translations

            await nameTranslationBLValidationCore.NameTranslationsValidation(model.NameTranslations);

            #region Validate Duplication

            await ValidatePlanNameDuplication(model.NameTranslations);

            #endregion

            #endregion

            return true;
        }
        private async Task<bool> ValidatePlanNameDuplication(List<NameTranslationModel> nameTranslations)
        {
            foreach (var nameTranslation in nameTranslations)
            {
                var checkNameDuplicate = await repositoryManager.PlanNameTranslationRepository.
                    Get(pt => pt.Id != nameTranslation.Id && nameTranslation.Name == pt.Name &&
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

            return true;
        }
    }
}
