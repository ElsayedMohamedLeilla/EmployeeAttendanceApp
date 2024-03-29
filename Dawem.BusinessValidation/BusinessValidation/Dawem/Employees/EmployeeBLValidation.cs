using Dawem.Contract.BusinessValidation.Dawem.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Dawem.Employees
{

    public class EmployeeBLValidation : IEmployeeBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public EmployeeBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> CreateValidation(CreateEmployeeModel model)
        {
            var checkEmployeeDuplicate = await repositoryManager
                .EmployeeRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.Name == model.Name).AnyAsync();

            if (checkEmployeeDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryEmployeeNameIsDuplicated);
            }

            var checkEmployeeNumberDuplicate = await repositoryManager
               .EmployeeRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.EmployeeNumber == model.EmployeeNumber).AnyAsync();
            if (checkEmployeeNumberDuplicate)
            {
                throw new BusinessValidationException(AmgadKeys.SorryEmployeeNumberIsDuplicated);
            }

            #region Employees Count

            var checkIsTrialSubscription = await repositoryManager.SubscriptionRepository.
                Get(s => !s.IsDeleted && s.CompanyId == requestInfo.CompanyId && s.Plan.IsTrial).
                AnyAsync();

            var getEmployeesCount = await repositoryManager.EmployeeRepository.
                Get(e => e.IsDeleted && e.CompanyId == requestInfo.CompanyId).
                CountAsync();

            if (checkIsTrialSubscription)
            {
                var getPlanTrialEmployeesCount = (await repositoryManager.DawemSettingRepository.
                            GetEntityByConditionAsync(d => !d.IsDeleted && d.Type == DawemSettingType.PlanTrialEmployeesCount))?.
                            Integer ?? 0;

                if (getEmployeesCount >= getPlanTrialEmployeesCount)
                    throw new BusinessValidationException(messageCode: null, message:
                        TranslationHelper.GetTranslation(LeillaKeys.SorryYouReachTheMaxNumberOfEmployeesInYourCompany, requestInfo.Lang) +
                        LeillaKeys.SpaceThenDashThenSpace +
                        TranslationHelper.GetTranslation(LeillaKeys.YouAreOnTrialSubscription, requestInfo.Lang) +
                        LeillaKeys.SpaceThenDashThenSpace +
                        TranslationHelper.GetTranslation(LeillaKeys.MaxNumberOfEmployees, requestInfo.Lang) +
                        LeillaKeys.Space + getPlanTrialEmployeesCount);
            }
            else
            {
                var companyNumberOfEmployees = await repositoryManager
               .CompanyRepository.Get(c => c.Id == requestInfo.CompanyId)
               .Select(c => c.NumberOfEmployees)
               .FirstOrDefaultAsync();

                if (getEmployeesCount >= companyNumberOfEmployees)
                    throw new BusinessValidationException(messageCode: null, message: TranslationHelper.GetTranslation(LeillaKeys.SorryYouReachTheMaxNumberOfEmployeesInYourCompany, requestInfo.Lang) +
                        LeillaKeys.SpaceThenDashThenSpace +
                        TranslationHelper.GetTranslation(LeillaKeys.MaxNumberOfEmployees, requestInfo.Lang) +
                        LeillaKeys.Space +
                         companyNumberOfEmployees);
            }

            #endregion


            return true;
        }
        public async Task<bool> UpdateValidation(UpdateEmployeeModel model)
        {
            var checkEmployeeDuplicate = await repositoryManager
                .EmployeeRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
                c.Name == model.Name && c.Id != model.Id).AnyAsync();
            if (checkEmployeeDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryEmployeeNameIsDuplicated);
            }

            var checkEmployeeNumberDuplicate = await repositoryManager
              .EmployeeRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
              c.EmployeeNumber == model.EmployeeNumber &&
              c.Id != model.Id).AnyAsync();
            if (checkEmployeeNumberDuplicate)
            {
                throw new BusinessValidationException(AmgadKeys.SorryEmployeeNumberIsDuplicated);
            }

            return true;
        }
    }
}
