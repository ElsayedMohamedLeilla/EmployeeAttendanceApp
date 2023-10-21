using Dawem.Contract.BusinessLogic.Provider;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Provider;
using Dawem.Models.Context;

namespace Dawem.BusinessLogic.Provider
{
    public class CompanyBL : ICompanyBL
    {
        private IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestHeaderContext;
        public CompanyBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
               RequestInfo _requestHeaderContext, IRepositoryManager _repositoryManager)
        {
            unitOfWork = _unitOfWork;
            requestHeaderContext = _requestHeaderContext;
            repositoryManager = _repositoryManager;
        }

        public async Task<Company> Create(Company company)
        {
            var insertedCompany = repositoryManager.CompanyRepository.Insert(company);
            await unitOfWork.SaveAsync();
            return insertedCompany;
        }
    }
}
