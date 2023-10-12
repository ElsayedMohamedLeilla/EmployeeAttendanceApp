using SmartBusinessERP.Domain.Entities.Provider;
using SmartBusinessERP.Models.Response;

namespace SmartBusinessERP.BusinessLogic.Provider.Contract
{
    public interface ICompanyBL 
    {
        BaseResponseT<Company> Create(Company company);
    }
}
