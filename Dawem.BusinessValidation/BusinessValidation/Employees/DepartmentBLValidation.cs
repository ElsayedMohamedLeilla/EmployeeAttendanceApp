using Dawem.Contract.BusinessValidation.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.Departments;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;


namespace Dawem.Validation.BusinessValidation.Employees
{

    public class DepartmentBLValidation : IDepartmentBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public DepartmentBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> CreateValidation(CreateDepartmentModel model)
        {
            var checkDepartmentDuplicate = await repositoryManager
                .DepartmentRepository.Get(c => !c.IsDeleted && c.CompanyId == requestInfo.CompanyId && c.Name == model.Name).AnyAsync();
            if (checkDepartmentDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryDepartmentNameIsDuplicated);
            }
            if (model.ParentId is not null)
            {
                var checkDepartmentParent = await repositoryManager
              .DepartmentRepository.Get(c => !c.IsDeleted && c.CompanyId == requestInfo.CompanyId && c.Id == model.ParentId).AnyAsync();
                if (!checkDepartmentParent)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryYouMustSelectValidParent);
                }
            }        
            if(model.ManagerId == 0)
            {
                throw new BusinessValidationException(AmgadKeys.SorryYouMustSelectDepartmentManager);
            }
           
            return true;
        }
        public async Task<bool> UpdateValidation(UpdateDepartmentModel model)
        {
            var checkDepartmentDuplicate = await repositoryManager
                .DepartmentRepository.Get(c => !c.IsDeleted && c.CompanyId == requestInfo.CompanyId &&
                c.Name == model.Name && c.Id != model.Id).AnyAsync();
            if (checkDepartmentDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryDepartmentNameIsDuplicated);
            }

            var checkDepartmentParent = await repositoryManager
                .DepartmentRepository.Get(c => !c.IsDeleted && c.CompanyId == requestInfo.CompanyId && c.Id == model.ParentId).AnyAsync();
            if (!checkDepartmentParent)
            {
                throw new BusinessValidationException(LeillaKeys.SorryYouMustSelectValidParent);
            }

            if (model.Id == model.ParentId)
            {
                throw new BusinessValidationException(LeillaKeys.SorryParentDepartmentMustNotEqualToCurrentDepartment);
            }

            return true;
        }
    }
}
