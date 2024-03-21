using Dawem.Contract.BusinessValidation.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Core.Groups;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Core
{
    public class GroupBLValidation : IGroupBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo; // header
        public GroupBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }

        public async Task<bool> CreateValidation(CreateGroupDTO model)
        {
            var checkGroupDuplicate = await repositoryManager
                .GroupRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.Name == model.Name).AnyAsync();
            if (checkGroupDuplicate)
            {
                throw new BusinessValidationException(AmgadKeys.SorryGroupNameIsDuplicated);
            }
            if (model.Employees.Count < 2)
            {
                throw new BusinessValidationException(AmgadKeys.SorryGroupCantBeCreatedWithLessThan2Employee);
            }
            if (model.Employees.Any(item => item.EmployeeId == 0))
            {
                throw new BusinessValidationException(AmgadKeys.SorryThisEmployeeNotFound);
            }

            bool hasDuplicates = model.EmployeeIds.Count != model.EmployeeIds.Distinct().Count();
            if (hasDuplicates)
            {
                throw new BusinessValidationException(AmgadKeys.SorryCantAddEmployeeInTheSameGroupTwice);
            }
            List<Employee> employees = repositoryManager.EmployeeRepository
           .GetAll()
           .Where(employee => model.EmployeeIds.Contains(employee.Id))
           .ToList();
            if (employees.Count != model.EmployeeIds.Count)
            {
                throw new BusinessValidationException(AmgadKeys.SorrySomeAddedEmployeeNotFound);
            }

            return true;
        }


        public async Task<bool> UpdateValidation(UpdateGroupDTO model)
        {
            var checkGroupDuplicate = await repositoryManager
                .GroupRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
                c.Name == model.Name && c.Id != model.Id).AnyAsync();
            if (checkGroupDuplicate)
            {
                throw new BusinessValidationException(AmgadKeys.SorryGroupNameIsDuplicated);
            }
            if (model.Employees.Count < 2)
            {
                throw new BusinessValidationException(AmgadKeys.SorryGroupCantBeCreatedWithLessThan2Employee);
            }
            if (model.Employees.Any(item => item.EmployeeId == 0))
            {
                throw new BusinessValidationException(AmgadKeys.SorryThisEmployeeNotFound);
            }
            bool hasDuplicates = model.EmployeeIds.Count != model.EmployeeIds.Distinct().Count();
            if (hasDuplicates)
            {
                throw new BusinessValidationException(AmgadKeys.SorryCantAddEmployeeInTheSameGroupTwice);

            }
            List<Employee> employees = repositoryManager.EmployeeRepository
           .GetAll()
           .Where(employee => model.EmployeeIds.Contains(employee.Id))
           .ToList();
            if (employees.Count != model.EmployeeIds.Count)
            {
                throw new BusinessValidationException(AmgadKeys.SorrySomeAddedEmployeeNotFound);
            }

            return true;
        }


    }
}
