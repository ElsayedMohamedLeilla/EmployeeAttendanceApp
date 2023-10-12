using Dawem.Domain.Entities.Provider;
using Dawem.Models.Response;

namespace SmartBusinessERP.BusinessLogic.Provider.Contract
{
    public interface ICompanyBL
    {
        BaseResponseT<Company> Create(Company company);
    }
}
