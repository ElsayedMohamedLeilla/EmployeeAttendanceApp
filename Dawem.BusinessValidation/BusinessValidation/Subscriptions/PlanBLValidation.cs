using Dawem.Contract.BusinessValidation.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.Departments;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;


namespace Dawem.Validation.BusinessValidation.Employees
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
            /*var checkPlanDuplicate = await repositoryManager
                .PlanRepository.Get(c => !c.IsDeleted && c.CompanyId == requestInfo.CompanyId && c.Name == model.Name).AnyAsync();
            if (checkPlanDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryPlanNameIsDuplicated);
            }*/

            var checkPlanOverlap = await repositoryManager
                .PlanRepository.Get(c => !c.IsDeleted && ((model.MinNumberOfEmployees >= c.MinNumberOfEmployees &&
                model.MinNumberOfEmployees <= c.MaxNumberOfEmployees) || (model.MaxNumberOfEmployees >= c.MinNumberOfEmployees &&
                model.MaxNumberOfEmployees <= c.MaxNumberOfEmployees))).AnyAsync();
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
                .PlanRepository.Get(c => !c.IsDeleted && ((model.MinNumberOfEmployees >= c.MinNumberOfEmployees &&
                model.MinNumberOfEmployees <= c.MaxNumberOfEmployees) || (model.MaxNumberOfEmployees >= c.MinNumberOfEmployees &&
                model.MaxNumberOfEmployees <= c.MaxNumberOfEmployees)) &&
                 c.Id != model.Id).AnyAsync();
            if (checkPlanOverlap)
            {
                throw new BusinessValidationException(LeillaKeys.SorryPlanOverlapWithOtherPlanInNumberOfEmployees);
            }

            return true;
        }
    }
}
