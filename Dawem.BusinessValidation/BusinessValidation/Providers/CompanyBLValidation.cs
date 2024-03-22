using Dawem.Contract.BusinessValidation.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;


namespace Dawem.Validation.BusinessValidation.Employees
{

    public class CompanyBLValidation : ICompanyBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public CompanyBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> CreateValidation(CreateCompanyModel model)
        {
            var checkCompanyDuplicate = await repositoryManager
                .CompanyRepository.Get(c => !c.IsDeleted && c.Name == model.Name)
                .AnyAsync();
            if (checkCompanyDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryCompanyNameIsDuplicated);
            }

            return true;
        }
        public async Task<bool> UpdateValidation(UpdateCompanyModel model)
        {
            /*var checkCompanyDuplicate = await repositoryManager
                .CompanyRepository.Get(c => !c.IsDeleted && c.Name == model.Name && c.Id != model.Id)
                .AnyAsync();
            if (checkCompanyDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryCompanyNameIsDuplicated);
            }*/

            return true;
        }
    }
}
