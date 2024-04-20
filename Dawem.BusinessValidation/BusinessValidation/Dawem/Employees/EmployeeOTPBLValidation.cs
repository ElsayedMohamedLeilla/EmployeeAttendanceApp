using Dawem.Contract.BusinessValidation.Dawem.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Employees.Users;

namespace Dawem.Validation.BusinessValidation.Dawem.Employees
{

    public class EmployeeOTPBLValidation : IEmployeeOTPBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public EmployeeOTPBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }

        public Task<bool> CreateValidation(CreateEmployeeOTPDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<int> PreSignUpValidation(PreSignUpDTO model)
        {
            throw new NotImplementedException();
        }
    }
}
