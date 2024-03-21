using Dawem.Contract.BusinessValidation.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.Employees;


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


            return true;
        }
        public async Task<bool> UpdateValidation(UpdateCompanyModel model)
        {


            return true;
        }
    }
}
