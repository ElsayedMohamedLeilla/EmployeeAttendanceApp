using Dawem.Contract.BusinessValidation.Dawem.Subscriptions;
using Dawem.Contract.BusinessValidationCore.AdminPanel.Subscriptions;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Subscriptions.Plans;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;


namespace Dawem.Validation.BusinessValidation.Dawem.Subscriptions
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
            #region Validate Arabic And English Languages

            await nameTranslationBLValidationCore.NameTranslationsValidation(model.NameTranslations);

            #endregion

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

            return true;
        }
        public async Task<bool> UpdateValidation(UpdatePlanModel model)
        {
            #region Validate Arabic And English Languages

            await nameTranslationBLValidationCore.NameTranslationsValidation(model.NameTranslations);

            #endregion

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

            return true;
        }
    }
}
