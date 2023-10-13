using Dawem.Domain.Entities.Provider;
using Dawem.Models.Response;

namespace Dawem.Contract.BusinessLogic.Provider
{
    public interface ICompanyBL
    {
        BaseResponseT<Company> Create(Company company);
    }
}
