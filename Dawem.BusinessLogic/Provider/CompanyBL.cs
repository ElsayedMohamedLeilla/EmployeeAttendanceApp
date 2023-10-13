using Dawem.Contract.BusinessLogic.Provider;
using SmartBusinessERP.Data;
using SmartBusinessERP.Data.UnitOfWork;
using SmartBusinessERP.Domain.Entities.Provider;
using SmartBusinessERP.Enums;
using SmartBusinessERP.Helpers;
using SmartBusinessERP.Models.Context;
using SmartBusinessERP.Models.Response;
using SmartBusinessERP.Repository.Provider.Contract;

namespace Dawem.BusinessLogic.Provider
{
    public class CompanyBL : ICompanyBL
    {
        private IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly ICompanyRepository companyRepository;
        private readonly RequestHeaderContext userContext;
        public CompanyBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
               RequestHeaderContext _userContext, ICompanyRepository _companyRepository)
        {
            unitOfWork = _unitOfWork;
            userContext = _userContext;

            companyRepository = _companyRepository;
        }

        public BaseResponseT<Company> Create(Company company)
        {
            BaseResponseT<Company> response = new BaseResponseT<Company>();
            try
            {
                response.Result = companyRepository.Insert(company);
                unitOfWork.Save();
                response.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                TranslationHelper.SetException(response, ex);
            }
            return response;
        }


    }
}
