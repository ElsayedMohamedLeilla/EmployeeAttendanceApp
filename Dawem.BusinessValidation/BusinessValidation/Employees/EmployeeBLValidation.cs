using Dawem.Contract.BusinessValidation.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Employees
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
                throw new BusinessValidationException(DawemKeys.SorryEmployeeNameIsDuplicated);
            }

            return true;
        }
        public async Task<bool> UpdateValidation(UpdateEmployeeModel model)
        {
            var checkEmployeeDuplicate = await repositoryManager
                .EmployeeRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
                c.Name == model.Name && c.Id != model.Id).AnyAsync();
            if (checkEmployeeDuplicate)
            {
                throw new BusinessValidationException(DawemKeys.SorryEmployeeNameIsDuplicated);
            }

            return true;
        }
    }
}
