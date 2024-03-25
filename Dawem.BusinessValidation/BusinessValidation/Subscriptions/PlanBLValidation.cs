using Dawem.Contract.BusinessValidation.Subscriptions;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Subscriptions.Plans;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;


namespace Dawem.Validation.BusinessValidation.Subscriptions
{

    public class PlanBLValidationBLValidation : IPlanBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public PlanBLValidationBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> CreateValidation(CreatePlanModel model)
        {

            #region Validate Arabic And English Languages

            var getArabicAndEnglishLanguages = await repositoryManager.LanguageRepository.
                Get(l => l.ISO2 == LeillaKeys.Ar || l.ISO2 == LeillaKeys.En).
                ToListAsync();
            if (getArabicAndEnglishLanguages != null && getArabicAndEnglishLanguages.Count == 2)
            {
                var languagesIds = getArabicAndEnglishLanguages.Select(l => l.Id).ToList();
                var checkArAndEnCount = model.NameTranslations.Where(nt => languagesIds.Contains(nt.LanguageId)).Count() == 2;

                if (!checkArAndEnCount)
                    throw new BusinessValidationException(LeillaKeys.SorryYouMustEnterNamesInArabicAndEnglishLanguages);

            }

            #endregion


            var checkPlanOverlap = await repositoryManager
                .PlanRepository.Get(c => !c.IsDeleted && (model.MinNumberOfEmployees >= c.MinNumberOfEmployees &&
                model.MinNumberOfEmployees <= c.MaxNumberOfEmployees || model.MaxNumberOfEmployees >= c.MinNumberOfEmployees &&
                model.MaxNumberOfEmployees <= c.MaxNumberOfEmployees)).AnyAsync();
            if (checkPlanOverlap)
            {
                throw new BusinessValidationException(LeillaKeys.SorryPlanOverlapWithOtherPlanInNumberOfEmployees);
            }

            return true;
        }
        public async Task<bool> UpdateValidation(UpdatePlanModel model)
        {
            /*var checkPlanDuplicate = await repositoryManager
                .PlanRepository.Get(c => !c.IsDeleted && c.CompanyId == requestInfo.CompanyId &&
                c.Name == model.Name && c.Id != model.Id).AnyAsync();
            if (checkPlanDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryPlanNameIsDuplicated);
            }*/

            var checkPlanOverlap = await repositoryManager
                .PlanRepository.Get(c => !c.IsDeleted && (model.MinNumberOfEmployees >= c.MinNumberOfEmployees &&
                model.MinNumberOfEmployees <= c.MaxNumberOfEmployees || model.MaxNumberOfEmployees >= c.MinNumberOfEmployees &&
                model.MaxNumberOfEmployees <= c.MaxNumberOfEmployees) &&
                 c.Id != model.Id).AnyAsync();
            if (checkPlanOverlap)
            {
                throw new BusinessValidationException(LeillaKeys.SorryPlanOverlapWithOtherPlanInNumberOfEmployees);
            }

            return true;
        }
    }
}
